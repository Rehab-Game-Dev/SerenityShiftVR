using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public List<GameObject> birdsInScene; 
    public int amountToSpawn = 15;
    public bool activateOnStart = true; // Add this toggle
    
    void Start()
    {
        // Deactivate all birds first
        DeactivateAllBirds();
        
        // If this flag is true (for medium level), activate birds immediately
        if (activateOnStart)
        {
            ActivateBirds();
        }
    }
    
    // GameManager can still call this function (for demo level)
    public void ActivateBirds()
    {
        ActivateRandomBirds();
        Debug.Log("Birds activated!");
    }
    
    void DeactivateAllBirds()
    {
        foreach (GameObject bird in birdsInScene)
        {
            if (bird != null)
            {
                bird.SetActive(false);
            }
        }
    }
    
    void ActivateRandomBirds()
    {
        List<GameObject> availableBirds = new List<GameObject>(birdsInScene);
        
        for (int i = 0; i < amountToSpawn; i++)
        {
            if (availableBirds.Count == 0) break;
            
            int randomIndex = Random.Range(0, availableBirds.Count);
            GameObject selectedBird = availableBirds[randomIndex];
            selectedBird.SetActive(true);
            
            availableBirds.RemoveAt(randomIndex);
        }
    }
}