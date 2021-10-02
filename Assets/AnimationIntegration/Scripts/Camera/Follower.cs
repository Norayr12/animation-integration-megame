using UnityEngine;

public class Follower : MonoBehaviour
{
    [Header("Follow settings")]
    [SerializeField] private GameObject _target;
    [SerializeField] private float _smoothValue;
    [SerializeField] private float _cameraHeight;
    [SerializeField] private float _zOffset;

    private void FixedUpdate()
    {
        float xPos = Mathf.Lerp(transform.position.x, _target.transform.position.x, _smoothValue * Time.deltaTime);
        float zPos = Mathf.Lerp(transform.position.z, _target.transform.position.z - _zOffset, _smoothValue * Time.deltaTime);
        transform.position = new Vector3(xPos, _cameraHeight, zPos);        
    }
}
