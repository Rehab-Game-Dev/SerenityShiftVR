using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatAmplitude = 0.5f;  // How high/low it moves (in meters)
    public float floatSpeed = 1f;        // How fast it floats
    
    [Header("Rotation (Optional)")]
    public bool rotateWhileFloating = true;
    public float rotationSpeed = 30f;    // Degrees per second
    public Vector3 rotationAxis = Vector3.up; // Which axis to rotate around
    
    private Vector3 startPosition;
    private float timeOffset;

    void Start()
    {
        // Remember the starting position
        startPosition = transform.position;
        
        // Random offset so multiple floating objects don't sync
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        // Calculate the float offset using sine wave for smooth motion
        float floatOffset = Mathf.Sin((Time.time + timeOffset) * floatSpeed) * floatAmplitude;
        
        // Apply the floating motion
        transform.position = startPosition + new Vector3(0, floatOffset, 0);
        
        // Optional rotation
        if (rotateWhileFloating)
        {
            transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
        }
    }
}