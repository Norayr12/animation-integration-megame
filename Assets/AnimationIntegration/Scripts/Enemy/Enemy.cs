using UnityEngine;

[RequireComponent(typeof(Animator), typeof(EnemySpawner))]
public class Enemy : MonoBehaviour
{
    private Animator _animator;
    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemySpawner = GetComponent<EnemySpawner>();
    }
}
