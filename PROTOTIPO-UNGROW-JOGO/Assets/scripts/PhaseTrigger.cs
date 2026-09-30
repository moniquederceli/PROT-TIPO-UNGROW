using System.Collections;
using UnityEngine;

public class PhaseTrigger : MonoBehaviour
{
    public Transform newStartLimit;

    public SceneTransition sceneTransition;

    public float delayAfterFade = 0.5f;

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated)
            return;

        if (!other.CompareTag("Player"))
            return;

        activated = true;

        StartCoroutine(ChangePhase(other.gameObject));
    }

    IEnumerator ChangePhase(GameObject player)
    {
        PlayerMovement movement =
            player.GetComponent<PlayerMovement>();

        if (movement != null)
            movement.enabled = false;

        Rigidbody2D rb =
            player.GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // FADE
        if (sceneTransition != null)
        {
            sceneTransition.FadeToBlack();

            yield return new WaitForSeconds(
                sceneTransition.fadeDuration
            );
        }

        // MUDA O LIMITE
        CameraFollow cameraFollow =
            Camera.main.GetComponent<CameraFollow>();

        if (cameraFollow != null)
        {
            cameraFollow.ChangeStartLimit(
                newStartLimit
            );

            cameraFollow.enabled = true;
        }

        // MOVE O PLAYER PARA O INÍCIO DA FASE 2
        Vector3 spawnPosition = new Vector3(
            newStartLimit.position.x + 1f,
            player.transform.position.y,
            player.transform.position.z
        );

        player.transform.position = spawnPosition;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // POSICIONA A CÂMERA
        if (cameraFollow != null)
        {
            float cameraHalfWidth =
                Camera.main.orthographicSize *
                Camera.main.aspect;

            float targetX =
                player.transform.position.x;

            Collider2D limitCollider =
                newStartLimit.GetComponent<Collider2D>();

            if (limitCollider != null)
            {
                float minimumCameraX =
                    limitCollider.bounds.max.x +
                    cameraHalfWidth;

                targetX = Mathf.Max(
                    targetX,
                    minimumCameraX
                );
            }

            Camera.main.transform.position =
                new Vector3(
                    targetX,
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z
                );
        }

        yield return new WaitForSeconds(
            delayAfterFade
        );

        if (sceneTransition != null)
        {
            sceneTransition.FadeFromBlack();
        }

        if (movement != null)
            movement.enabled = true;

        Debug.Log("FASE 2 ATIVADA!");
    }
}