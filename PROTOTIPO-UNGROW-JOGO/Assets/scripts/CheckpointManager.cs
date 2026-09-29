using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    public Transform checkpoint;

    private bool checkpointAtivo = false;

    private Vector3 cameraPosition;
    private bool cameraSalva = false;

    void Awake()
    {
        Instance = this;
    }

    public void AtivarCheckpoint(
        Transform novoCheckpoint,
        Camera camera
    )
    {
        checkpoint = novoCheckpoint;
        checkpointAtivo = true;

        if (camera != null)
        {
            cameraPosition = camera.transform.position;
            cameraSalva = true;
        }

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

    public void RestaurarCamera(Camera camera)
    {
        if (camera != null && cameraSalva)
        {
            camera.transform.position = cameraPosition;
        }
    }
}