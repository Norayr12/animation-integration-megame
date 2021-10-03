using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn settings")]
    [SerializeField] private float _respawnTime;

    public float RespawnTime
    {
        get
        {
            return _respawnTime;
        }
    }

    public void Spawn()
    {
        float x = Random.Range(0, Screen.width);
        float y = Random.Range(0, Screen.height);
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
        
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            transform.position = hit.point;

        }
        
    }

}  
