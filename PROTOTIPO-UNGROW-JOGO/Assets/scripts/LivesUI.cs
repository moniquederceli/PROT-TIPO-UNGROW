using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Image flower1;
    public Image flower2;
    public Image flower3;

    public float deathAnimationTime = 0.3f;

    private int lives = 3;

    public void LoseLife()
    {
        if (lives <= 0)
            return;

        lives--;

        Image flower = null;

        if (lives == 2)
        {
            flower = flower3;
        }
        else if (lives == 1)
        {
            flower = flower2;
        }
        else if (lives == 0)
        {
            flower = flower1;
        }

        if (flower != null)
        {
            Animator animator = flower.GetComponent<Animator>();

            if (animator != null)
            {
                animator.SetTrigger("Die");
            }

            StartCoroutine(HideFlowerAfterAnimation(flower));
        }
    }

    IEnumerator HideFlowerAfterAnimation(Image flower)
    {
        yield return new WaitForSecondsRealtime(deathAnimationTime);

        flower.gameObject.SetActive(false);
    }
}