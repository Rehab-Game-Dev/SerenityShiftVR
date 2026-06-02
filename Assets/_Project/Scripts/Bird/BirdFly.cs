using UnityEngine;

public class BirdFly : MonoBehaviour
{
    [Header("הגדרות טיסה")]
    public float flySpeed = 15f;     // flight speed of the bird
    public float wobbleAmount = 1f; // amount of vertical wobble
    
    private float randomOffset;

    void Start()
    {
        // gives each bird a different starting point for the wobble
        randomOffset = Random.Range(0f, 10f);
    }

    void Update()
    {
        // 1. Move forward only (in the direction it is facing)
        transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);


        // 2. Slight wave-like movement up and down (Wobble) to make it feel alive
        float wobble = Mathf.Sin(Time.time + randomOffset) * wobbleAmount * Time.deltaTime;
        transform.Translate(Vector3.up * wobble);
    }
}