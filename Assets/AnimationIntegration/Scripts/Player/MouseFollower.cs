using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [Header("Follower settings")]
    [SerializeField] private Transform _follower;
    [SerializeField] private float _maxRotationAngle;
    [SerializeField] private LayerMask _followLayer;

    private void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
        float pRot = transform.rotation.eulerAngles.y;
        float playerRotationNormalized = pRot > 180 ? pRot - 360 : pRot;

        float fRot = _follower.eulerAngles.y;
        float followerRotationNormalized = fRot > 180 ? fRot - 360 : fRot;
       
        Vector3 result = Vector3.zero;
        Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, _followLayer))
            result = hit.point - _follower.position;

        float angle = Mathf.Atan2(result.x, result.z) * Mathf.Rad2Deg + 90;
        float delta = Mathf.Abs(transform.eulerAngles.y - angle);
        delta -= delta > 270 ? 270 : 0;        

        _follower.rotation = Quaternion.Euler(0, angle, 0);        
    }

    public float WrapAngle(float angle, float wrap)
    {
        float value = angle;

        if (value >= wrap) 
            value -= 360.0f;
        else if  (value <= -wrap) 
            value += 360.0f;

        return value;
    }
}
//                                          YA V MAGAZ POSHOL