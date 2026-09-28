using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    void Update()
    {
        transform.Translate(
            Vector2.left * moveSpeed * Time.deltaTime
        );
    }
}