using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_StartScreen : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStartButtonPress);
        exitButton.onClick.AddListener(OnExitButtonPress);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStartButtonPress);
        exitButton.onClick.RemoveListener(OnExitButtonPress);
    }

    private void OnStartButtonPress()
    {
        SceneManager.LoadScene(1);
    }

    private void OnExitButtonPress()
    {
        Application.Quit();
    }
}
