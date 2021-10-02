using System.Collections;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Animator), typeof(EnemyViewer))]
public class GameController: MonoBehaviour
{
    public static GameController Instance;
    public event Action OnEnemyDead;

    private Animator _animator;
    private EnemyViewer _enemyChecker;

    [Header("Player settings")]
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameObject _rangedWeapon;
    [SerializeField] private GameObject _meleeWeapon;

    [Header("Finishing settings")]
    [SerializeField] private KeyCode _finishingKey;
    [SerializeField] private float _finishingDistance;

    [Header("Pop up")]
    [SerializeField] private TMP_Text _readyTitle;

    private Coroutine _waitForKill;
    private int _horizontalDirection;
    private int _verticalDirection;
    private bool _isWaitingForKill;
    
    public bool IsEnemyKilled { get; set; }

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
        
        _animator.SetFloat(_horizontalDirection, rightLeft);
        if(backForward == 0 && rightLeft != 0)
            _animator.SetFloat(_verticalDirection, rightLeft != 0 ? backForward + 0.1f : backForward);
        else
            _animator.SetFloat(_verticalDirection, backForward);

        transform.Translate(new Vector3(rightLeft, 0, backForward) * _playerSpeed * Time.fixedDeltaTime);

        Vector3 rotation = backForward > 0 ? new Vector3(0, _rotationSpeed * rightLeft * Time.fixedDeltaTime, 0) : new Vector3(0, -_rotationSpeed * rightLeft * Time.fixedDeltaTime, 0);
        transform.Rotate(backForward != 0 ? rotation : Vector3.zero);
    }

    public void OnEnemyDeadInvoke () => OnEnemyDead?.Invoke();

    public void PlayerReset()
    {
        SetWeapon(_rangedWeapon);
    }

    private void Kill()
    {       
        CorrectPosition();
        SetWeapon(_meleeWeapon);
        _animator.SetTrigger("isFinishing");

        IsEnemyKilled = true;
    }

    private void CorrectPosition()
    {
        Vector3 enemyDirection = _enemyChecker.EnemyPosition.forward;
        transform.position = _enemyChecker.EnemyPosition.position - enemyDirection * _finishingDistance;
        transform.rotation = _enemyChecker.EnemyPosition.rotation;
    }

    private void SetWeapon(GameObject weapon)
    {
        _meleeWeapon.SetActive(weapon == _meleeWeapon);
        _rangedWeapon.SetActive(weapon == _rangedWeapon);
    }

    private void OnReadyForFinishing()
    {
        _readyTitle.gameObject.SetActive(true);

        if (!_isWaitingForKill)
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
                Kill();
                _isWaitingForKill = false;
                yield break;
            }
        }
    }
}
