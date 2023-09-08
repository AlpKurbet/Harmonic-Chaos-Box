using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DronePositionController : MonoBehaviour
{
    public Transform area; 
    public float movingSpeed = 1.0f;
    public float theMinX = -5.0f; 
    public float theMaxX = 5.0f; 
    public float theMinZ = -5.0f; 
    public float theMaxZ = 5.0f; 

    private Vector3 targetPoisitonToGo;

    void Start()
    {
        //Start with a random position and then let Update() do the rest
        targetPoisitonToGo = GetRandomPosition();
    }

    void Update()
    {
        // Go to the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPoisitonToGo, movingSpeed * Time.deltaTime);

        // Check if the object if there or it's really close 
        if (Vector3.Distance(transform.position, targetPoisitonToGo) < 0.1f)
        {
            // New Random Place to Go
            targetPoisitonToGo = GetRandomPosition();
        }
    }

    // Create a random Vector 3 position  with hte given boundries
    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(theMinX, theMaxX);
        float randomZ = Random.Range(theMinZ, theMaxZ);
        return new Vector3(randomX, transform.position.y, randomZ);
    }
}
