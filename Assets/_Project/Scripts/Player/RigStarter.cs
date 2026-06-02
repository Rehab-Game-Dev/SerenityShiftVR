using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigStarter : MonoBehaviour
{
    void Start()
    {
        // We give the system a fraction of a second to breathe, then restart
        Invoke("RestartRig", 0.1f);
    }

    void RestartRig()
    {
        // Find the component
        RigBuilder rigBuilder = GetComponent<RigBuilder>();
        
        if (rigBuilder != null)
        {
            // This command rebuilds the connections and removes errors
            rigBuilder.Build(); 
        }
    }
}