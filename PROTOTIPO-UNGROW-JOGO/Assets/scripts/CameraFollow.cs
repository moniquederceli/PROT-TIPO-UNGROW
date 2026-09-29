using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float smoothSpeed = 5f;
    public float offsetX = 0f;

    [Header("Limite inicial da fase")]
    public Transform startLimit;

    void LateUpdate()
    {
        if (player == null)
            return;

        // Posição horizontal que a câmera gostaria de ter
        float targetX = player.position.x + offsetX;

        // Calcula metade da largura que a câmera enxerga
        float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        // Se existir um StartLimit, impede a câmera de passar dele
        if (startLimit != null)
        {
            Collider2D limitCollider = startLimit.GetComponent<Collider2D>();

            if (limitCollider != null)
            {
                float minimumCameraX =
                    limitCollider.bounds.max.x + cameraHalfWidth;

                targetX = Mathf.Max(targetX, minimumCameraX);
            }
        }

        Vector3 targetPosition = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}