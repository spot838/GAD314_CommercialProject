using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_WinScreen : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    public void OnResumePress()
    {
        Time.timeScale = 1.0f;
        UI.ShowWinScreen(false);
    }

    public void OnSurveyPress()
    {
        Application.OpenURL(Game.SurveyURL);
    }

    public void OnDebugPress()
    {
        UI.ShowDebugInfo();
    }

    public void OnMainMenuPress()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
