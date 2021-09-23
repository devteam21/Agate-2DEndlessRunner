using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private Transform target;
    [SerializeField] private float horizontalOffset;

    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = target.position.x + horizontalOffset;
        transform.position = newPosition;
    }
}
