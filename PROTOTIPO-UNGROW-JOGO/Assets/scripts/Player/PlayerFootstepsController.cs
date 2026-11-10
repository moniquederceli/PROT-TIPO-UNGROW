
using UnityEngine;

// Controla os sons de passos do protagonista.
//
// O som so toca quando:
// 1. O jogador esta se movimentando horizontalmente.
// 2. O jogador esta encostando no chao.
// 3. O jogo nao esta pausado.
//
// Utiliza o sistema de deteccao de chao ja existente
// no PlayerMovement, sem modificar a fisica da Carol.

[RequireComponent(typeof(AudioSource))]
public class PlayerFootstepsController : MonoBehaviour
{
    [Header("Componentes")]

    // Script original de movimentacao do protagonista.
    private PlayerMovement playerMovement;

    // Componente de fisica do Player.
    private Rigidbody2D rb;

    // Responsavel por reproduzir o som dos passos.
    private AudioSource footstepsAudio;

    [Header("Configuracoes")]

    // Velocidade horizontal minima para tocar passos.
    [SerializeField] private float velocidadeMinima = 0.1f;

    private void Awake()
    {
        // Busca o PlayerMovement no objeto pai.
        playerMovement = GetComponentInParent<PlayerMovement>();

        // Busca o Rigidbody2D do protagonista.
        rb = GetComponentInParent<Rigidbody2D>();

        // Busca o AudioSource deste objeto.
        footstepsAudio = GetComponent<AudioSource>();

        // Impede o som de iniciar automaticamente.
        footstepsAudio.playOnAwake = false;

        // Permite repetir o audio enquanto estiver correndo.
        footstepsAudio.loop = true;
    }

    private void Update()
    {
        // Verifica se os componentes estao disponiveis.
        if (playerMovement == null ||
            rb == null ||
            playerMovement.groundCheck == null)
        {
            PararPassos();
            return;
        }

        // Verifica se o personagem esta no chao.
        //
        // Reutiliza o PontoDope, o raio e a Layer Chao
        // configurados no PlayerMovement.
        bool estaNoChao = Physics2D.OverlapCircle(
            playerMovement.groundCheck.position,
            playerMovement.groundCheckRadius,
            playerMovement.groundLayer
        ) != null;

        // Verifica se existe movimento horizontal.
        bool estaAndando =
            Mathf.Abs(rb.linearVelocity.x) > velocidadeMinima;

        // Verifica se o jogador esta com os controles ativos.
        // Evita passos durante a transicao da bossfight.
        bool movimentoAtivo = playerMovement.isActiveAndEnabled;

        // Verifica se o jogo nao esta pausado.
        bool jogoAtivo = Time.timeScale > 0f;

        // O som so deve tocar se todas as condicoes
        // estiverem sendo atendidas simultaneamente.
        bool deveTocar =
            estaNoChao &&
            estaAndando &&
            movimentoAtivo &&
            jogoAtivo;

        if (deveTocar)
        {
            // Inicia o audio somente se nao estiver tocando.
            // Evita reiniciar o som a cada frame.
            if (!footstepsAudio.isPlaying)
                footstepsAudio.Play();
        }
        else
        {
            // Interrompe os passos ao parar, pular,
            // cair ou pausar o jogo.
            PararPassos();
        }
    }

    // Centraliza a logica de interrupcao dos passos.
    private void PararPassos()
    {
        if (footstepsAudio != null &&
            footstepsAudio.isPlaying)
        {
            footstepsAudio.Stop();
        }
    }

    // Garante que o audio pare caso o objeto
    // seja desativado durante o jogo.
    private void OnDisable()
    {
        PararPassos();
    }
}
