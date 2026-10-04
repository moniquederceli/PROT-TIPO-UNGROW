
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseTrigger : MonoBehaviour
{
    public string nextSceneName = "Fase2";
    public SceneTransition sceneTransition;

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated || !other.CompareTag("Player"))
            return;

        activated = true;
        StartCoroutine(ChangePhase(other.gameObject));
    }

    private IEnumerator ChangePhase(GameObject player)
    {
        PlayerMovement movement =
            player.GetComponent<PlayerMovement>();

        Rigidbody2D rb =
            player.GetComponent<Rigidbody2D>();

        if (movement != null)
            movement.enabled = false;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (sceneTransition != null)
        {
            sceneTransition.FadeToBlack();

            yield return new WaitForSecondsRealtime(
                sceneTransition.fadeDuration
            );
        }

        SceneManager.LoadScene(nextSceneName);
    }
}

