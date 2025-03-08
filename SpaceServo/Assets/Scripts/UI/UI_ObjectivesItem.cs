using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ObjectivesItem : MonoBehaviour
{
    [SerializeField] TMP_Text objectiveText;
    [SerializeField] RawImage objectiveIcon;

    Objective objective;

    public void Initilize(Objective objective)
    {
        this.objective = objective;
        objectiveText.text = objective.ObjectiveText;
    }
}
