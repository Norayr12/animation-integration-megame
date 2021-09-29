using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private Animator _animator;

    [Header("Player settings")]
    [SerializeField] private float _playerSpeed;

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
        _animator.SetFloat(_verticalDirection, backForward);

        transform.Translate(new Vector3(rightLeft, 0, backForward) * _playerSpeed * Time.fixedDeltaTime);
    }
}
