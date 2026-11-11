
using UnityEngine;

// Controla a vida dos inimigos comuns e do Boss.
//
// INIMIGOS COMUNS:
// - Recebem dano do protagonista.
// - Desaparecem quando a vida chega a zero.
//
// BOSS:
// - Recebe dano normalmente.
// - Na demo, ativa a tela de agradecimento ao morrer.
// - Futuramente, pode liberar a passagem para outra fase.
//
// IMPORTANTE:
// O Demo End Controller deve ser preenchido apenas
// no Boss da fase 1. Nos inimigos comuns, deixar vazio.

public class EnemyHealth : MonoBehaviour
{
    [Header("Configuracoes de Vida")]

    // Quantidade maxima de vida do inimigo.
    public int maxHealth = 1;

    // Vida atual, controlada internamente.
    private int currentHealth;

    // Impede que a morte seja processada mais de uma vez.
    private bool isDead = false;

    [Header("Configuracoes do Boss")]

    // Barreira que bloqueia a passagem para a proxima fase.
    // Deixar vazio nos inimigos comuns.
    [SerializeField] private Collider2D finalLimit;

    [Header("Conclusao da Demo")]

    // Controlador da tela temporaria de agradecimento.
    // Preencher somente no Boss da fase 1.
    [SerializeField] private DemoEndController demoEndController;

    private void Awake()
    {
        // Define a vida inicial do inimigo.
        // Garante que tenha pelo menos 1 ponto de vida.
        currentHealth = Mathf.Max(1, maxHealth);
    }

    // Chamado quando o protagonista acerta o inimigo.
    public void TakeDamage(int damage)
    {
        // Ignora ataques se o inimigo ja morreu
        // ou se o dano recebido for invalido.
        if (isDead || damage <= 0)
            return;

        // Reduz a vida conforme o dano recebido.
        currentHealth -= damage;

        Debug.Log(
            gameObject.name + " - Vida: " + currentHealth
        );

        // Se a vida acabou, executa a derrota.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Executa a morte do inimigo ou Boss.
    private void Die()
    {
        // Marca o inimigo como derrotado.
        isDead = true;

        Debug.Log(gameObject.name + " foi derrotado!");

        // Se o controlador da demo estiver configurado,
        // significa que este inimigo encerra a fase.
        if (demoEndController != null)
        {
            // Inicia a sequencia de agradecimento.
            // O controlador espera 2 segundos
            // antes de exibir a tela final.
            demoEndController.MostrarVitoria();

            Debug.Log(
                "Demo concluida! Tela de agradecimento iniciada."
            );
        }
        else if (finalLimit != null)
        {
            // Comportamento original do Boss.
            //
            // Se a tela temporaria nao estiver configurada,
            // libera a passagem para a proxima fase.
            //
            // Assim poderemos remover a tela da demo
            // futuramente sem reescrever este sistema.
            finalLimit.enabled = false;

            Debug.Log(
                "Boss derrotado! Passagem liberada!"
            );
        }

        // Remove o inimigo derrotado da cena.
        //
        // Futuramente podemos adicionar animacoes,
        // efeitos visuais e sons de morte antes disso.
        Destroy(gameObject);
    }
}
