using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_PauseMenu : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button exitToMenuButton;

    private void OnEnable()
    {
        resumeButton.onClick.AddListener(OnResumeButtonPress);
        settingsButton.onClick.AddListener(OnSettingsButtonPress);
        exitToMenuButton.onClick.AddListener(OnExitToMenuButtonPress);
    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveListener(OnResumeButtonPress);
        settingsButton.onClick.RemoveListener(OnSettingsButtonPress);
        exitToMenuButton.onClick.RemoveListener(OnExitToMenuButtonPress);
    }


    private void OnResumeButtonPress()
    {
        Game.PauseGame(false);
    }

    private void OnSettingsButtonPress()
    {
        // TODO: implement settings menu
    }

    private void OnExitToMenuButtonPress()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
