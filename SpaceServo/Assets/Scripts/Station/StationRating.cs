using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this manages the player's station raiting

public class StationRating : MonoBehaviour
{
    public float Value
    {
        get
        {
            if (values.Count == 0) return 0.55f;

            float average = 0;
            foreach (float value in values)
            {
                average += value;
            }
            average = average / values.Count;
            return average;
        }
    }

    private List<float> values = new List<float>();

    void Start()
    {
        UI.UpdateRatingText();
    }

    public void AddCustomerSatisfaction(float satisfaction)
    {
        values.Add(satisfaction);
        UI.UpdateRatingText();
    }
}