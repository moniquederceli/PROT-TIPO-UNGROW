using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("Configurações do Boss")]
    [Tooltip("O quão longe o Boss vai para os lados.")]
    public float distanciaMaxima = 0.5f;

    [Tooltip("A velocidade do Boss.")]
    public float velocidade = 3f;

    private Vector3 posicaoInicial;
    private float tempoMovimento = 0f;

    void Start()
    {
        posicaoInicial = transform.position;
    }

    void Update()
    {
        tempoMovimento += Time.deltaTime;

        float deslocamento =
            Mathf.Sin(tempoMovimento * velocidade) *
            distanciaMaxima;

        transform.position = new Vector3(
            posicaoInicial.x + deslocamento,
            posicaoInicial.y,
            posicaoInicial.z
        );
    }

    public void ResetBoss()
    {
        // Volta o Boss para a posição inicial
        transform.position = posicaoInicial;

        // Reinicia o movimento
        tempoMovimento = 0f;

        Debug.Log("Boss reiniciado!");
    }
}