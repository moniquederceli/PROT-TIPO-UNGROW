
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controla a tela temporaria de conclusao da demo.
//
// Quando o Boss e derrotado:
// 1. Congela o jogo para impedir a entrada na fase 2.
// 2. Aguarda 2 segundos.
// 3. Mostra a mensagem de agradecimento.
//
// A tela pode ser removida futuramente,
// quando as proximas fases estiverem prontas.

public class DemoEndController : MonoBehaviour
{
    [Header("Interface")]

    // Painel com a mensagem de agradecimento.
    [SerializeField] private GameObject endPanel;

    [Header("Configuracoes")]

    // Tempo de espera antes de mostrar a tela.
    [SerializeField] private float atrasoVitoria = 2f;

    // Impede que a conclusao seja ativada duas vezes.
    private bool finalizado = false;

    private void Awake()
    {
        // Esconde a tela ao iniciar a fase.
        if (endPanel != null)
            endPanel.SetActive(false);
    }

    // Chamado quando o Boss e derrotado.
    public void MostrarVitoria()
    {
        if (finalizado)
            return;

        finalizado = true;

        // Congela a jogabilidade imediatamente.
        Time.timeScale = 0f;

        StartCoroutine(ExibirTela());
    }

    private IEnumerator ExibirTela()
    {
        // Espera 2 segundos reais mesmo com o tempo pausado.
        yield return new WaitForSecondsRealtime(atrasoVitoria);

        // Mostra o painel de conclusao.
        if (endPanel != null)
            endPanel.SetActive(true);

        // Libera o cursor para clicar no botao.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Chamado pelo botao Voltar ao Menu.
    public void VoltarAoMenu()
    {
        // Restaura o tempo antes de trocar de cena.
        Time.timeScale = 1f;

        // Carrega o menu principal do jogo.
        SceneManager.LoadScene("menu");
    }
}
