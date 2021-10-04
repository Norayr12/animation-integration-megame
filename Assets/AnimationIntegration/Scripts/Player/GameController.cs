using System;
using System.Collections;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(Animator), typeof(EnemyViewer))]
public class GameController: MonoBehaviour
{
    public static GameController Instance;
    public event Action OnEnemyAttacked;
    public event Action OnAttackEnd;

    private Animator _animator;
    private EnemyViewer _enemyChecker;

    [Header("Player settings")]
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameObject _rangedWeapon;
    [SerializeField] private GameObject _meleeWeapon;
    [SerializeField] private Transform _headBone;

    [Header("Finishing settings")]
    [SerializeField] private KeyCode _finishingKey;
    [SerializeField] private float _finishingDistance;

    [Header("Pop up")]
    [SerializeField] private TMP_Text _readyTitle;

    private Coroutine _waitForKill;
    private int _horizontalDirection;
    private int _verticalDirection;
    private bool _isWaitingForKill;
    private bool _isFinishing;
    
    public bool IsEnemyKilled { get; set; }

    public bool IsHeadMaxRotated { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _animator = GetComponent<Animator>();
        _enemyChecker = GetComponent<EnemyViewer>();
        _horizontalDirection = Animator.StringToHash("horizontalDirection");
        _verticalDirection = Animator.StringToHash("verticalDirection");
    }

    private void Start()
    {
        _enemyChecker.OnReadyForFinishing += OnReadyForFinishing;
        _enemyChecker.OnEnemyFar += OnEnemyFar;
    }

    private void FixedUpdate()
    {
        float rightLeft = Input.GetAxis("Horizontal");
        float backForward = Input.GetAxis("Vertical");

        if (!_isFinishing)
        {
            _animator.SetFloat(_horizontalDirection, rightLeft);
            if (backForward == 0 && rightLeft != 0)
            {
                _animator.SetFloat(_verticalDirection, rightLeft != 0 ? backForward + 0.1f : backForward);
            }
            else
            {
                _animator.SetFloat(_verticalDirection, backForward);
            }
            transform.Translate(new Vector3(rightLeft, 0, backForward) * _playerSpeed * Time.fixedDeltaTime);

            Vector3 rotation = backForward > 0 ? new Vector3(0, _rotationSpeed * rightLeft * Time.fixedDeltaTime, 0) : new Vector3(0, -_rotationSpeed * rightLeft * Time.fixedDeltaTime, 0);
            transform.Rotate(backForward != 0 ? rotation : Vector3.zero);

            if (IsHeadMaxRotated)
                _headBone.Rotate(backForward != 0 ? rotation : Vector3.zero);
        }
    }

    public void OnEnemyAttackedInvoke () => OnEnemyAttacked?.Invoke();

    public void OnAttackEndInvoke()
    {
        OnAttackEnd?.Invoke();      
        _animator.SetFloat(_verticalDirection, 0f);

        SetWeapon(_rangedWeapon);
        ToggleFinishing();
    }

    public void ToggleFinishing () => _isFinishing = !_isFinishing;

    private IEnumerator Kill()
    {
        ToggleFinishing();

        Vector3 enemyDirection = _enemyChecker.EnemyPosition.forward;
        Vector3 targetPosition = _enemyChecker.EnemyPosition.position - enemyDirection * _finishingDistance;
        Vector3 startPosition = transform.position;

        float step = 0f;
        while (step < 1)
        {
            yield return new WaitForFixedUpdate();

            float x = Mathf.Lerp(startPosition.x, targetPosition.x, step);
            float y = Mathf.Lerp(startPosition.y, targetPosition.y, step);
            float z = Mathf.Lerp(startPosition.z, targetPosition.z, step);

            float nextX = Mathf.Lerp(startPosition.x, targetPosition.x, step + 0.05f);
            float nextY = Mathf.Lerp(startPosition.y, targetPosition.y, step + 0.05f);
            float nextZ = Mathf.Lerp(startPosition.z, targetPosition.z, step + 0.05f);

            transform.position = new Vector3(x, y, z);

            _animator.SetFloat(_verticalDirection, 1f);
            _animator.SetFloat(_horizontalDirection, 0f);

            transform.LookAt(new Vector3(nextX, nextY, nextZ));
            step += 0.05f;
        }

        transform.rotation = _enemyChecker.EnemyPosition.rotation;
        SetWeapon(_meleeWeapon);
        _animator.SetTrigger("isFinishing");

        IsEnemyKilled = true;
    }

    private void SetWeapon(GameObject weapon)
    {
        _meleeWeapon.SetActive(weapon == _meleeWeapon);
        _rangedWeapon.SetActive(weapon == _rangedWeapon);
    }

    private void OnReadyForFinishing()
    {
        _readyTitle.gameObject.SetActive(true);

        if (!_isWaitingForKill && !_isFinishing)
        {
            _waitForKill = StartCoroutine(WaitForKill());
            _isWaitingForKill = true;
        }
    }

    private void OnEnemyFar()
    {
        _readyTitle.gameObject.SetActive(false);

        if (_isWaitingForKill)
        {
            StopCoroutine(_waitForKill);
            _isWaitingForKill = false;
        }
    }

    private IEnumerator WaitForKill()
    {
        while (true)
        {
            yield return null;

            if (Input.GetKeyDown(_finishingKey))
            {
                _isWaitingForKill = false;
                StartCoroutine(Kill());                
                yield break;
            }
        }
    }
}
