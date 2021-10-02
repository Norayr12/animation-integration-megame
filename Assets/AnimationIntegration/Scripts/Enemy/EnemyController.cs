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
        GameController.Instance.OnEnemyDead += OnEnemyDead;

        _ragdollController.ToggleRagdoll(false);
    }

    private void OnEnemyDead()
    {
        _ragdollController.ToggleRagdoll(true);
        _animator.enabled = false;
        _enemySpawner.Spawn();
    }
}
