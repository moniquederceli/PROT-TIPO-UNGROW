using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Image[] flowers;

    public float deathAnimationTime = 0.6f;

    private int currentLife;

    void Start()
    {
        currentLife = flowers.Length;
    }

    public void LoseLife()
{
    Debug.Log("LOSE LIFE FOI CHAMADO!");

    if (currentLife <= 0)
    {
        Debug.Log("NÃO HÁ MAIS VIDAS!");
        return;
    }

    currentLife--;

    Debug.Log("VIDAS RESTANTES: " + currentLife);

    Image flower = flowers[currentLife];

    Debug.Log("FLOR ESCOLHIDA: " + flower.name);

    Animator animator = flower.GetComponent<Animator>();

    if (animator != null)
    {
        Debug.Log("ANIMATOR ENCONTRADO NA FLOR!");

        animator.SetTrigger("Die");

        Debug.Log("TRIGGER DIE ENVIADO!");
    }
    else
    {
        Debug.LogError("A FLOR NÃO POSSUI ANIMATOR!");
    }

    StartCoroutine(RemoveFlowerAfterAnimation(flower));
}
    IEnumerator RemoveFlowerAfterAnimation(Image flower)
    {
        yield return new WaitForSecondsRealtime(deathAnimationTime);

        flower.gameObject.SetActive(false);
    }

    public void ResetLives()
    {
        currentLife = flowers.Length;

        foreach (Image flower in flowers)
        {
            flower.gameObject.SetActive(true);
        }

        Debug.Log("Vidas restauradas!");
    }
}