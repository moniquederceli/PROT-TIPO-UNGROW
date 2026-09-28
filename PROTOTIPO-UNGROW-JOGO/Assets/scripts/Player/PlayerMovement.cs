using UnityEngine;
using UnityEngine.InputSystem; // Linha obrigatória para o novo sistema

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Configurações do Pulo")]
    public Transform groundCheck;      
    public float groundCheckRadius = 0.2f; 
    public LayerMask groundLayer;      
    private bool isGrounded;           

    private Rigidbody2D rb;
    private float horizontal; // Guarda a direção do movimento

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Linha corrigida aqui!
    }

    void Update()
    {
        Move();
    }

    // Chamado automaticamente pelo Player Input (Novo Sistema) ao andar
    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        horizontal = inputVector.x; // Pega a direção esquerda/direita (-1, 0 ou 1)
    }

    // Chamado automaticamente pelo Player Input (Novo Sistema) ao apertar o botão de Pulo
    public void OnJump()
    {
        // Verifica se está no chão usando física
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // SÓ aplica a força se o boneco estiver tocando no chão
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );
        }
    }

    void Move()
    {
        // Movimenta usando a variável atualizada pelo OnMove
        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed,
            rb.linearVelocity.y
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
