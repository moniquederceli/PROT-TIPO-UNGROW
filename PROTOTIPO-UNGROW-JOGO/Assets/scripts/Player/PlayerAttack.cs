
using UnityEngine;
using UnityEngine.InputSystem;

// Controla o ataque basico do protagonista.
//
// Usa a direcao visual do PlayerVisual para
// identificar se o ataque deve atingir a
// esquerda ou a direita.
//
// Nao altera a movimentacao ou a fisica.

public class PlayerAttack : MonoBehaviour
{
    [Header("Componentes")]

    // SpriteRenderer utilizado pelo PlayerVisual.
    // Permite descobrir a direcao do personagem.
    [SerializeField] private SpriteRenderer playerVisual;

    [Header("Configuracoes do Ataque")]

    // Dano causado por cada ataque.
    [SerializeField] private int attackDamage = 1;

    // Distancia do centro do ataque em relacao ao Player.
    [SerializeField] private float attackDistance = 0.7f;

    // Tamanho da area atingida.
    [SerializeField] private float attackRadius = 0.7f;

    // Tempo minimo entre ataques.
    [SerializeField] private float attackCooldown = 0.35f;

    private float nextAttackTime = 0f;

    // Chamado pelo PlayerInput ao usar a acao Attack.
    public void OnAttack(InputValue value)
    {
        // Ignora quando o botao e solto.
        if (!value.isPressed)
            return;

        // Impede ataques durante a pausa.
        if (Time.timeScale == 0f)
            return;

        // Respeita o intervalo entre ataques.
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + attackCooldown;

        PerformAttack();
    }

    private void PerformAttack()
    {
        // Verifica para qual lado o personagem olha.
        // O PlayerVisual ja utiliza flipX para isso.
        float direction = 1f;

        if (playerVisual != null && playerVisual.flipX)
            direction = -1f;

        // Calcula o centro da area de ataque.
        Vector2 attackPosition =
            (Vector2)transform.position +
            Vector2.right * direction * attackDistance;

        // Procura objetos com Collider2D na area.
        Collider2D[] targets = Physics2D.OverlapCircleAll(
            attackPosition,
            attackRadius
        );

        // Verifica quais objetos sao inimigos.
        foreach (Collider2D target in targets)
        {
            EnemyHealth enemy =
                target.GetComponentInParent<EnemyHealth>();

            // Aplica dano somente aos inimigos
            // que possuem o componente EnemyHealth.
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }

        Debug.Log("Player realizou um ataque!");
    }

    // Desenha a area de ataque na Scene
    // quando o Player estiver selecionado.
    private void OnDrawGizmosSelected()
    {
        float direction = 1f;

        if (playerVisual != null && playerVisual.flipX)
            direction = -1f;

        Vector2 attackPosition =
            (Vector2)transform.position +
            Vector2.right * direction * attackDistance;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            attackPosition,
            attackRadius
        );
    }
}
