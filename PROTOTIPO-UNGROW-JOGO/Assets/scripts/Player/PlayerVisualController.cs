
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Configuracoes")]
    [SerializeField] private float movementThreshold = 0.05f;

    private void Awake()
    {
        // Busca os componentes automaticamente
        if (playerRb == null)
            playerRb = GetComponentInParent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerRb == null || animator == null || spriteRenderer == null)
            return;

        float speedX = playerRb.linearVelocity.x;

        // Alterna entre Idle e Run
        bool isMoving = Mathf.Abs(speedX) > movementThreshold;
        animator.SetBool("IsMoving", isMoving);

        // Vira o personagem para a direcao do movimento
        if (speedX > movementThreshold)
        {
            spriteRenderer.flipX = false;
        }
        else if (speedX < -movementThreshold)
        {
            spriteRenderer.flipX = true;
        }

        // Quando parado, nao altera a ultima direcao
    }
}
