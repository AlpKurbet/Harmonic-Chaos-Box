using UnityEngine;

public class CubeRotatorController : MonoBehaviour
{
    public float theRotationSpeed = 30.0f; 

    void Update()
    {
        // Rotate the object along all three axes with specified speed vars
        transform.Rotate(Vector3.right * theRotationSpeed * 0.5f * Time.deltaTime);
        transform.Rotate(Vector3.up * theRotationSpeed * 0.6f * Time.deltaTime);
        transform.Rotate(Vector3.forward * theRotationSpeed * 0.4f *Time.deltaTime);
    }
}
