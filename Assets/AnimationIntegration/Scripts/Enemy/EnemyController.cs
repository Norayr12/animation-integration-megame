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
        _ragdollController.SetRagdoll(false);
    }
}
