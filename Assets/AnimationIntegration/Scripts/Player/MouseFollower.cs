using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [Header("Follower settings")]
    [SerializeField] private Transform _follower;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _maxRotationAngle;

    private void LateUpdate()
    {
        
        Vector3 result = Vector3.zero;
        Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag != "Ground")
            {              
                return;
            }

            result = hit.point - _follower.position;
        }           

        float angle = Mathf.Atan2(result.x, result.z) * Mathf.Rad2Deg + 90;
        _follower.rotation = Quaternion.Euler(0, angle, 0);                
    }
}
