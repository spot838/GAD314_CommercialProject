using UnityEngine;

// this manages the player's station raiting

public class StationRating : MonoBehaviour
{
    [field: SerializeField] public float Value { get; private set; } = 50; // between 0 and 100 

    void Start()
    {
        UI.UpdateRatingText();
    }
}