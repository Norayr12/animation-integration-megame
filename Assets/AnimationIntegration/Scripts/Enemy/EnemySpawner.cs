using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn settings")]
    [SerializeField] private float _respawnTime;

    public void Spawn () => StartCoroutine(Cooldown());

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_respawnTime);
        gameObject.SetActive(false);

        GameController.Instance.IsEnemyKilled = false;
    }
}
