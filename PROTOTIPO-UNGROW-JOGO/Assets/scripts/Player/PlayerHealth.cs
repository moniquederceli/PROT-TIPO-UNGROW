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
    transform.position =
        CheckpointManager.Instance.GetCheckpointPosition();

    Rigidbody2D rb = GetComponent<Rigidbody2D>();

    if (rb != null)
        rb.linearVelocity = Vector2.zero;

    CheckpointManager.Instance.RestaurarCamera(
        Camera.main
    );

    currentHealth = maxHealth;
    canTakeDamage = true;

    Debug.Log("Player voltou para o checkpoint!");
}
}