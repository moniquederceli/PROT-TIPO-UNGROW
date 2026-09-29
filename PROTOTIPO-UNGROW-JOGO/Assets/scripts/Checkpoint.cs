using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool ativado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (ativado)
            return;

        ativado = true;

        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.AtivarCheckpoint(transform);
        }

        Debug.Log("Checkpoint da Boss Fight ativado!");
    }
}