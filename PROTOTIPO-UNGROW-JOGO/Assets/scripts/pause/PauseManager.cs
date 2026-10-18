
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    public GameObject pausePanel;

    private bool isPaused = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "menu")
            return;

        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        TestarCliqueUI();
    }

    public void PauseGame()
    {
        if (pausePanel == null)
        {
            Debug.LogError("PausePanel não está atribuído!");
            return;
        }

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Debug.Log("PAUSA ABERTA");
    }

    public void ResumeGame()
    {
        Debug.Log("RETOMAR JOGO");

        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void ReturnToMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("Menu");
    }

    public void RestartLevel()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;

        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        SceneManager.LoadScene(cenaAtual);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    void TestarCliqueUI()
    {
        if (!isPaused || Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        EventSystem eventSystem = EventSystem.current;

        if (eventSystem == null)
        {
            Debug.LogError("Nenhum EventSystem ativo!");
            return;
        }

        PointerEventData pointer = new PointerEventData(eventSystem);
        pointer.position = Mouse.current.position.ReadValue();

        List<RaycastResult> resultados =
            new List<RaycastResult>();

        eventSystem.RaycastAll(pointer, resultados);

        foreach (RaycastResult resultado in resultados)
        {
            string nome = resultado.gameObject.name;

            Debug.Log("Mouse atingiu: " + nome);

            if (nome == "ResumeButton")
            {
                ResumeGame();
                return;
            }

            if (nome == "MenuButton")
            {
                ReturnToMenu();
                return;
            }

            if (nome == "RestartButton")
            {
                RestartLevel();
                return;
            }

            if (nome == "QuitButton")
            {
                QuitGame();
                return;
            }
        }
    }
}
