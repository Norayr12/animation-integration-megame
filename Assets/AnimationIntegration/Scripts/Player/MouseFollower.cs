using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [Header("Follower settings")]
    [SerializeField] private Transform _follower;
    [SerializeField] private float _maxRotationAngle;
    [SerializeField] private LayerMask _followLayer;

    private Quaternion _lastHeadRotationValue;
    private Quaternion _lastPlayerRotation;

    private void LateUpdate()
    {       
        Vector3 result = Vector3.zero;
        Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, _followLayer))
            result = hit.point - _follower.position;

        float angle = Mathf.Atan2(result.x, result.z) * Mathf.Rad2Deg + 90;
        angle += angle < 0 ? 360 : 0;

        float normalAngle = angle - 90;
        normalAngle += normalAngle < 0 ? 360 : 0;

        float firstDelta = Mathf.Abs(normalAngle - transform.eulerAngles.y);
        float secondDelta = 360 - (Mathf.Abs(normalAngle - transform.eulerAngles.y));

        if (firstDelta <= _maxRotationAngle || secondDelta <= _maxRotationAngle)
        {
            _follower.rotation = Quaternion.Euler(0, angle, 0);
            _lastHeadRotationValue = _follower.rotation;
            _lastPlayerRotation = transform.rotation;
        }
        else
        {
            float parentDelta = transform.eulerAngles.y - _lastPlayerRotation.eulerAngles.y;
            parentDelta -= parentDelta > 360 ? 360 : 0;
            parentDelta += parentDelta < 0 ? 360 : 0;

            _follower.rotation = Quaternion.Euler(0, _lastHeadRotationValue.eulerAngles.y + parentDelta, 0);
        }
    }
}