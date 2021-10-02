using System;
using UnityEngine;

public class EnemyViewer : MonoBehaviour
{
    public event Action OnReadyForFinishing;
    public event Action OnEnemyFar;

    [Header("Finishing settings")]
    [SerializeField] private GameObject _enemy;
    [SerializeField] private float _minDistance;

    public Transform EnemyPosition
    {
        get 
        {
            return _enemy.transform;
        }
    }

    private void Update()
    {
        if (CheckForDistance() && CheckForBack() && !GameController.Instance.IsEnemyKilled)    
            OnReadyForFinishing?.Invoke();
        else
            OnEnemyFar?.Invoke();
    }

    private bool CheckForBack()
    {
        Vector3 pos = transform.InverseTransformPoint(_enemy.transform.position);

        return pos.z > 0;
    }

    private bool CheckForDistance () => Vector3.Distance(transform.position, _enemy.transform.position) <= _minDistance;

}


