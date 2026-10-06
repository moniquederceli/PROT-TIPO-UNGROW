using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("fase 1");
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

    SceneManager.LoadScene(ultimaCena);
}
}