
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(LoadGameplayScene("fase 1"));
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("UltimaCena"))
        {
            Debug.Log("Nenhum save encontrado!");
            return;
        }

        PlayerPrefs.SetInt("CarregandoSave", 1);
        PlayerPrefs.Save();

        string ultimaCena = PlayerPrefs.GetString("UltimaCena");

        StartCoroutine(LoadGameplayScene(ultimaCena));
    }

    IEnumerator LoadGameplayScene(string sceneName)
    {
        Time.timeScale = 1f;

        // Carrega o sistema de pause apenas se ainda não existir.
        if (FindAnyObjectByType<PauseManager>() == null)
        {
            yield return SceneManager.LoadSceneAsync(
                "pause",
                LoadSceneMode.Additive
            );
        }

        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
