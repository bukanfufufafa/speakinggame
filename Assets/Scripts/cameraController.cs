using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;   // Player
    public Vector3 offset;     // Jarak kamera ke player
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = transform.position.z; // jaga Z kamera

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
