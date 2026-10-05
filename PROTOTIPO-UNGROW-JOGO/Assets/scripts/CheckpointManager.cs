using UnityEngine.SceneManagement;
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

    if (PlayerPrefs.GetInt("CarregandoSave", 0) == 1)
    {
        PlayerPrefs.SetInt("CarregandoSave", 0);
        PlayerPrefs.Save();

        Invoke(nameof(CarregarCheckpointSalvo), 0.2f);
    }
}

    public void AtivarCheckpoint(Transform novoCheckpoint)
    {
        checkpoint = novoCheckpoint;

        cameraPosition = Camera.main.transform.position;

        checkpointAtivo = true;

        PlayerPrefs.SetString(
    "UltimaCena",
    SceneManager.GetActiveScene().name
);

PlayerPrefs.SetFloat("CheckpointX", checkpoint.position.x);
PlayerPrefs.SetFloat("CheckpointY", checkpoint.position.y);
PlayerPrefs.SetFloat("CheckpointZ", checkpoint.position.z);

PlayerPrefs.Save();

        Debug.Log(
            "Checkpoint ativado: " +
            checkpoint.name
        );
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

    public void CarregarCheckpointSalvo()
{
    if (!PlayerPrefs.HasKey("CheckpointX"))
        return;

    float x = PlayerPrefs.GetFloat("CheckpointX");
    float y = PlayerPrefs.GetFloat("CheckpointY");
    float z = PlayerPrefs.GetFloat("CheckpointZ");

    Vector3 posicaoSalva = new Vector3(x, y, z);

    GameObject player = GameObject.FindGameObjectWithTag("Player");

    if (player != null)
    {
        player.transform.position = posicaoSalva;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    checkpointAtivo = true;

    Debug.Log("Save carregado! Player voltou para o checkpoint.");
}
}