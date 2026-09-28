using UnityEngine;
using UnityEngine.SceneManagement;

public class Deadline : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDied();
        }
    }

    void PlayerDied()
    {
        Debug.Log("Player morreu ao cair!");

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}
