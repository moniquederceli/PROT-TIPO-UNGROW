using System.Collections;
using UnityEngine;

public class BossDefeatSimulator : MonoBehaviour
{
    [Header("Barreira")]
    public Collider2D finalLimit;

    [Header("Tempo para derrotar o Boss")]
    public float defeatTime = 10f;

    private bool timerStarted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (timerStarted)
            return;

        if (!other.CompareTag("Player"))
            return;

        timerStarted = true;

        StartCoroutine(DefeatBoss());
    }

    IEnumerator DefeatBoss()
    {
        Debug.Log("Boss encontrado! Começando contagem de 10 segundos...");

        yield return new WaitForSeconds(defeatTime);

        if (finalLimit != null)
        {
            finalLimit.enabled = false;
        }

        Debug.Log("Boss derrotado! Passagem para a próxima fase liberada!");
    }
}