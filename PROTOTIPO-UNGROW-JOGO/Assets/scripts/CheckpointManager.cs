using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private Transform checkpoint;
    private Vector3 cameraPosition;
    private bool checkpointAtivo = false;

    void Awake()
    {
        Instance = this;
    }

    public void AtivarCheckpoint(Transform novoCheckpoint)
    {
        checkpoint = novoCheckpoint;

        // Guarda a posição da câmera antes da transição para o Boss
        cameraPosition = Camera.main.transform.position;

        checkpointAtivo = true;

        Debug.Log("Checkpoint ativado!");
    }

    public bool TemCheckpoint()
    {
        return checkpointAtivo && checkpoint != null;
    }

    public Vector3 GetCheckpointPosition()
    {
        return checkpoint.position;
    }

    public void RestaurarCamera()
    {
        if (Camera.main != null)
        {
            Camera.main.transform.position = cameraPosition;
        }
    }
}