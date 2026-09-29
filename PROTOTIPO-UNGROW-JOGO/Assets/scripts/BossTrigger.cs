using System.Collections;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [Header("Boss")]
    public Transform bossSpawn;
    public Transform bossCheckpoint;

    [Header("Camera")]
    public Camera mainCamera;
    public CameraFollow cameraFollow;

    [Header("Player")]
    public PlayerMovement playerMovement;

    [Header("Transição")]
    public float transitionDuration = 1.5f;

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated)
            return;

        if (!other.CompareTag("Player"))
            return;

        activated = true;

        // Ativa o checkpoint
        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.AtivarCheckpoint(
                bossCheckpoint,
                mainCamera
            );
        }

        StartCoroutine(StartBossTransition(other.transform));
    }

    IEnumerator StartBossTransition(Transform player)
    {
        // Para o Player
        if (playerMovement != null)
            playerMovement.enabled = false;

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        if (playerRb != null)
            playerRb.linearVelocity = Vector2.zero;

        // Desliga o CameraFollow
        if (cameraFollow != null)
            cameraFollow.enabled = false;

        // Posição inicial da câmera
        Vector3 startPosition = mainCamera.transform.position;

        // Posição final da câmera
        Vector3 targetPosition = new Vector3(
            bossSpawn.position.x,
            startPosition.y,
            startPosition.z
        );

        // Faz a câmera deslizar
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / transitionDuration;

            t = Mathf.SmoothStep(0f, 1f, t);

            mainCamera.transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                t
            );

            yield return null;
        }

        // Garante que a câmera chegou ao destino
        mainCamera.transform.position = targetPosition;

        // O Player NÃO é movido.
        // Ele permanece onde estava quando chegou na FinishLine.

        if (playerRb != null)
            playerRb.linearVelocity = Vector2.zero;

        // Libera o Player
        if (playerMovement != null)
            playerMovement.enabled = true;

        Debug.Log("Boss Fight começou!");
    }
}