using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this manages the player's station raiting

public class StationRating : MonoBehaviour
{
    // treat these as const's
    [field: SerializeField] public float MIN_RATING { get; private set; } = 0f;
    [field: SerializeField] public float MAX_RATING { get; private set; } = 50f;


    public float Value // a number between 0 and 50, each 10 represents a full star
    {
        get
        {
            if (values.Count == 0) return 0;

            float average = 0;
            foreach (float value in values)
            {
                average += value;
            }
            average = average / values.Count;
            //Mathf.Clamp(average, 0f, 1.0f); // the value should already be clamed before it even gets here
            return average;
        }
    }

    private List<float> values = new List<float>();

    void Start()
    {
        UI.UpdateRatingVisual();
        //UI.UpdateRatingText();
    }

    public void AddCustomerSatisfaction(float satisfaction)
    {
        satisfaction = Mathf.Clamp(satisfaction, MIN_RATING, MAX_RATING);
        values.Add(satisfaction);
        UI.UpdateRatingVisual();
        //UI.UpdateRatingText();
    }
}