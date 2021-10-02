using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController: MonoBehaviour
{
    private Animator _animator;

    [Header("Player settings")]
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameObject _rangedWeapon;
    [SerializeField] private GameObject _meleeWeapon;

    private int _horizontalDirection, _verticalDirection; 

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _horizontalDirection = Animator.StringToHash("horizontalDirection");
        _verticalDirection = Animator.StringToHash("verticalDirection");
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

    private void Update()
    {
        if (CheckForDistance())
            Finishing();
    }

    public bool CheckForDistance()
    {
        return false;
    }

    public void Finishing()
    {
        SetWeapon(_meleeWeapon);
        _animator.SetTrigger("isFinishing");
    }

    private void SetWeapon(GameObject weapon)
    {
        _meleeWeapon.SetActive(weapon == _meleeWeapon);
        _rangedWeapon.SetActive(weapon == _rangedWeapon);
    }

    
}
