using UnityEngine;

// this manages the player's station raiting

public class StationRating : MonoBehaviour
{
    [field: SerializeField] public float Raiting { get; private set; } = 50; // between 0 and 100 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UI.UpdateRatingText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
