using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("O quão longe ele vai para os lados a partir do centro.")]
    public float distanciaMaxima = 0.5f; 
    
    [Tooltip("A velocidade do vai e vem.")]
    public float velocidade = 3f;       

    private Vector3 posicaoInicial;

    void Start()
    {
        // Salva o ponto exato onde o inimigo foi colocado na fase
        posicaoInicial = transform.position;
    }

    void Update()
    {
        // Calcula o movimento de vai e vem usando a função matemática Seno
        float deslocamento = Mathf.Sin(Time.time * velocidade) * distanciaMaxima;

        // Atualiza a posição do inimigo somando o deslocamento apenas no eixo X
        transform.position = new Vector3(
            posicaoInicial.x + deslocamento, 
            transform.position.y, 
            transform.position.z
        );
    }
}
