using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private Animator _animator;

    [Header("Player settings")]
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _rotationSpeed;

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
}
