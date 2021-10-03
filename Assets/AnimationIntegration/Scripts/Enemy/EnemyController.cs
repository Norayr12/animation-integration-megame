using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(EnemySpawner), typeof(RagdollController))]
public class EnemyController: MonoBehaviour
{
    private Animator _animator;
    private EnemySpawner _enemySpawner;
    private RagdollController _ragdollController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemySpawner = GetComponent<EnemySpawner>();
        _ragdollController = GetComponent<RagdollController>();
    }

    private void Start()
    {
        GameController.Instance.OnEnemyAttacked += OnEnemyAttacked;
        GameController.Instance.OnAttackEnd += OnAttackEnd;

        _ragdollController.ToggleRagdoll(false);
    }

    private void OnEnemyAttacked()
    {
        _ragdollController.ToggleRagdoll(true);
        _animator.enabled = false;       
    }

    private void OnAttackEnd()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(_enemySpawner.RespawnTime);
        _enemySpawner.Spawn();
        _ragdollController.ToggleRagdoll(false);
        _animator.enabled = true;

        GameController.Instance.IsEnemyKilled = false;
    }
}
