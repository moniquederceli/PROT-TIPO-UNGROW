using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("O quão longe ele vai para os lados a partir do centro.")]
    public float distanciaMaxima = 0.5f;

    [Tooltip("A velocidade do vai e vem.")]
    public float velocidade = 3f;

    private Vector3 posicaoInicial;

    private float tempoMovimento = 0f;

    void Start()
    {
        // Salva o ponto exato onde o inimigo foi colocado na fase
        posicaoInicial = transform.position;
    }

    void Update()
    {
        // Conta o tempo do movimento
        tempoMovimento += Time.deltaTime;

        // Calcula o movimento de vai e vem
        float deslocamento =
            Mathf.Sin(tempoMovimento * velocidade) *
            distanciaMaxima;

        transform.position = new Vector3(
            posicaoInicial.x + deslocamento,
            posicaoInicial.y,
            posicaoInicial.z
        );
    }

    public void ResetMovement()
    {
        // Reinicia o tempo do movimento
        tempoMovimento = 0f;

        // Volta exatamente para a posição inicial
        transform.position = posicaoInicial;

        Debug.Log("inimigo reiniciado!");
    }
}