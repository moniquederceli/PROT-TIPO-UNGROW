using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float invulnerabilityTime = 1f;

    public LivesUI livesUI;

    private int currentHealth;
    private bool canTakeDamage = true;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage)
            return;

        canTakeDamage = false;

        currentHealth -= damage;

        Debug.Log("Vida atual: " + currentHealth);

        if (livesUI != null)
        {
            livesUI.LoseLife();
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(WaitForDeath());
            return;
        }

        Invoke(nameof(ResetDamage), invulnerabilityTime);
    }

    void ResetDamage()
    {
        canTakeDamage = true;
    }

    System.Collections.IEnumerator WaitForDeath()
    {
        Time.timeScale = 0f;

    yield return new WaitForSecondsRealtime(
        livesUI.deathAnimationTime
    );

    Time.timeScale = 1f;

    Die();
    }

    public void Die()
{
    Debug.Log("PLAYER MORREU!");

    if (CheckpointManager.Instance != null &&
        CheckpointManager.Instance.TemCheckpoint())
    {
        RespawnAtCheckpoint();
    }
    else
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}

void RespawnAtCheckpoint()
{
    // Volta para o checkpoint
    transform.position =
        CheckpointManager.Instance.GetCheckpointPosition();

    // Para o Rigidbody
    Rigidbody2D rb = GetComponent<Rigidbody2D>();

    if (rb != null)
        rb.linearVelocity = Vector2.zero;

    // Volta a câmera para a posição antes da transição
    CheckpointManager.Instance.RestaurarCamera();

    // Liga novamente o CameraFollow
    CameraFollow cameraFollow =
        Camera.main.GetComponent<CameraFollow>();

    if (cameraFollow != null)
        cameraFollow.enabled = true;

    // Reseta o BossTrigger
    BossTrigger bossTrigger =
    FindAnyObjectByType<BossTrigger>();

    if (bossTrigger != null)
        bossTrigger.ResetTrigger();

    // Recupera a vida
    currentHealth = maxHealth;
    canTakeDamage = true;

    if (livesUI != null)
{
    livesUI.ResetLives();
}

    // Permite o movimento novamente
    PlayerMovement movement =
        GetComponent<PlayerMovement>();

    if (movement != null)
        movement.enabled = true;

    Debug.Log("Player voltou para o checkpoint!");
}

}