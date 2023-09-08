using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class FirstPersonMovement : MonoBehaviour
{


    public AK.Wwise.Event stepSounds;

    public float speed = 5;
    public bool stepping = true;
    public bool inputFromUser = true;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;
    

    private Vector3 savedPosition;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        if((Math.Abs(Input.GetAxis("Horizontal")) > (0.1f * speed)) || (Math.Abs(Input.GetAxis("Vertical")) > (0.1f * speed)) )
        {
            inputFromUser = true;
        }
        else
        {
            inputFromUser = false;
        }
        
        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
        
        float dist = Vector3.Distance(savedPosition, transform.position);

        if(stepping && inputFromUser)
        {
                if(Math.Abs(dist) > 1.0f)
                {
                    StartCoroutine(StepSound());
                    stepping = false;
                }

        }
    }


    IEnumerator StepSound()
    {
        savedPosition = transform.position;
        if(IsRunning)
        {
            yield return new WaitForSeconds(0.4f * ((2.0f * speed)/runSpeed));
        }

        else
        {
            yield return new WaitForSeconds(0.4f);
        }
        
        
        stepSounds.Post(gameObject); 
        stepping = true;

    }
}