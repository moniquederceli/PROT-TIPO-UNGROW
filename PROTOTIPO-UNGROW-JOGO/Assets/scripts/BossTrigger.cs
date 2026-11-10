
using System.Collections;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [Header("Boss")]

    // Posicao utilizada como destino da camera.
    public Transform bossSpawn;

    // Script responsavel pelo movimento do Boss.
    public BossMovement bossMovement;

    [Header("Camera")]

    // Camera principal do jogo.
    public Camera mainCamera;

    // Script que acompanha o jogador.
    public CameraFollow cameraFollow;

    [Header("Player")]

    // Script de movimentacao do protagonista.
    public PlayerMovement playerMovement;

    [Header("Transicao")]

    // Tempo para a camera chegar ate o Boss.
    public float transitionDuration = 1.5f;

    // NOVO: referencia ao controlador de musicas.
    // Permite trocar a trilha quando a arena e ativada.
    [Header("Musica")]
    public BossMusicController musicController;

    // Impede que o gatilho seja ativado repetidamente.
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nao executa novamente se ja foi ativado.
        if (activated)
            return;

        // Ignora objetos que nao sejam o Player.
        if (!other.CompareTag("Player"))
            return;

        activated = true;

        // NOVO: comeca a transicao para a musica do Boss.
        // A musica muda junto com o inicio da camera.
        if (musicController != null)
            musicController.StartBossMusic();

        // Inicia a sequencia original da entrada do Boss.
        StartCoroutine(StartBossTransition(other.transform));
    }

    IEnumerator StartBossTransition(Transform player)
    {
        // Desativa temporariamente os comandos do jogador.
        if (playerMovement != null)
            playerMovement.enabled = false;

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        // Interrompe a velocidade atual do protagonista.
        if (playerRb != null)
            playerRb.linearVelocity = Vector2.zero;

        // Desliga o acompanhamento automatico da camera.
        if (cameraFollow != null)
            cameraFollow.enabled = false;

        // Salva a posicao atual da camera.
        Vector3 startPosition = mainCamera.transform.position;

        // Calcula a posicao de destino na arena do Boss.
        // Mantem os valores Y e Z da camera.
        Vector3 targetPosition = new Vector3(
            bossSpawn.position.x,
            startPosition.y,
            startPosition.z
        );

        float elapsed = 0f;

        // Move a camera gradualmente ate o Boss.
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / transitionDuration;

            // Suaviza o inicio e o fim do movimento.
            t = Mathf.SmoothStep(0f, 1f, t);

            mainCamera.transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                t
            );

            yield return null;
        }

        // Garante que a camera chegou ao destino.
        mainCamera.transform.position = targetPosition;

        // Mantem o jogador parado antes de liberar o controle.
        if (playerRb != null)
            playerRb.linearVelocity = Vector2.zero;

        // Devolve os comandos de movimento ao jogador.
        if (playerMovement != null)
            playerMovement.enabled = true;

        Debug.Log("Boss Fight comecou!");
    }

    // Reinicia o estado do gatilho.
    // Pode ser chamado pelos sistemas de reinicio da arena.
    public void ResetTrigger()
    {
        activated = false;

        // NOVO: restaura a musica de exploracao
        // quando este metodo for chamado.
        if (musicController != null)
            musicController.RestoreExplorationMusic();

        // Mantem a logica original de reinicio do Boss.
        if (bossMovement != null)
        {
            bossMovement.ResetBoss();
        }
    }
}
