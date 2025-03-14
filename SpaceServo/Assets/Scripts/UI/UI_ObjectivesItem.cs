using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ObjectivesItem : MonoBehaviour
{
    [SerializeField] TMP_Text objectiveText;
    [SerializeField] RawImage objectiveIcon;
    [SerializeField] Texture completedTexture;
    [SerializeField] Texture inprogressTexture;

    Objective objective;

    public void Initilize(Objective objective)
    {
        this.objective = objective;
        objectiveText.text = objective.ObjectiveText;
        objectiveIcon.texture =  objective.Complete ? completedTexture : inprogressTexture;
        objectiveText.fontStyle = objective.Complete ? FontStyles.Strikethrough : FontStyles.Normal;
    }
}
