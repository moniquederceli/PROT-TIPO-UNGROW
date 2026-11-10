
using System.Collections;
using UnityEngine;

// Controla a transicao entre a musica de exploracao
// e a musica de batalha contra o Boss.
//
// Este script deve ficar no objeto MusicManager,
// que possui dois componentes AudioSource.

public class BossMusicController : MonoBehaviour
{
    [Header("Configuracoes de Musica")]

    // Volume maximo utilizado pelas duas musicas.
    [SerializeField] private float volumeMaximo = 0.35f;

    // Tempo, em segundos, para realizar a troca gradual.
    [SerializeField] private float duracaoTransicao = 1.5f;

    // Referencias aos dois componentes de audio.
    private AudioSource musicaExploracao;
    private AudioSource musicaBoss;

    // Guarda a transicao atual, permitindo interrompe-la.
    private Coroutine transicaoAtual;

    private void Awake()
    {
        // Busca todos os AudioSources do MusicManager.
        AudioSource[] audios = GetComponents<AudioSource>();

        // Verifica se existem os dois componentes necessarios.
        // Se faltar algum, avisa no Console e desativa o script.
        if (audios.Length < 2)
        {
            Debug.LogError(
                "MusicManager precisa de 2 AudioSources!"
            );

            enabled = false;
            return;
        }

        // O primeiro AudioSource e a musica de exploracao.
        // O segundo AudioSource e a musica do Boss.
        //
        // IMPORTANTE: manter esta ordem no Inspector.
        musicaExploracao = audios[0];
        musicaBoss = audios[1];

        // Define os volumes iniciais.
        // Apenas a musica de exploracao sera ouvida.
        musicaExploracao.volume = volumeMaximo;
        musicaBoss.volume = 0f;
    }

    private void Start()
    {
        // Garante que a musica de exploracao comece.
        // Se o Play On Awake ja iniciou, nao toca novamente.
        if (musicaExploracao != null &&
            !musicaExploracao.isPlaying)
        {
            musicaExploracao.Play();
        }
    }

    // Chamado pelo BossTrigger quando o jogador
    // entra na arena do Boss.
    public void StartBossMusic()
    {
        if (!enabled || musicaBoss == null)
            return;

        // Interrompe uma transicao anterior, se existir.
        // Isso evita duas rotinas alterando os volumes juntas.
        if (transicaoAtual != null)
            StopCoroutine(transicaoAtual);

        // Inicia a troca da exploracao para a batalha.
        transicaoAtual = StartCoroutine(
            FazerTransicao(musicaExploracao, musicaBoss)
        );
    }

    // Restaura a musica de exploracao quando a arena
    // e reiniciada por algum sistema do jogo.
    public void RestoreExplorationMusic()
    {
        if (!enabled || musicaExploracao == null)
            return;

        // Cancela qualquer transicao em andamento.
        if (transicaoAtual != null)
            StopCoroutine(transicaoAtual);

        // Troca a musica do Boss pela exploracao.
        transicaoAtual = StartCoroutine(
            FazerTransicao(musicaBoss, musicaExploracao)
        );
    }

    // Coroutine responsavel pelo crossfade.
    // Uma musica diminui enquanto a outra aumenta.
    private IEnumerator FazerTransicao(
        AudioSource saindo,
        AudioSource entrando)
    {
        // Comeca a reproduzir a proxima musica,
        // caso ela ainda nao esteja tocando.
        if (!entrando.isPlaying)
            entrando.Play();

        // Guarda os volumes atuais.
        float volumeInicialSaindo = saindo.volume;
        float volumeInicialEntrando = entrando.volume;

        float tempo = 0f;

        // Executa a transicao gradualmente.
        while (tempo < duracaoTransicao)
        {
            // Usa o tempo do jogo.
            tempo += Time.deltaTime;

            // Calcula o progresso entre 0 e 1.
            float t = Mathf.Clamp01(
                tempo / Mathf.Max(0.01f, duracaoTransicao)
            );

            // Diminui o volume da musica anterior.
            saindo.volume = Mathf.Lerp(
                volumeInicialSaindo,
                0f,
                t
            );

            // Aumenta o volume da nova musica.
            entrando.volume = Mathf.Lerp(
                volumeInicialEntrando,
                volumeMaximo,
                t
            );

            // Aguarda o proximo frame.
            yield return null;
        }

        // Garante os volumes finais exatos.
        saindo.volume = 0f;
        entrando.volume = volumeMaximo;

        // Para a musica anterior para nao consumir
        // processamento de audio desnecessariamente.
        saindo.Stop();

        // Indica que a transicao terminou.
        transicaoAtual = null;
    }
}
