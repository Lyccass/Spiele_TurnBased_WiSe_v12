using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    private bool isMoving = false; // To keep track of movement state

    private void Update() 
    {
        float stoppingDistance = 0.1f;

        if (isMoving && Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            // Move toward target position
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            isMoving = false; // Stop moving when close enough
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 newTargetPosition = MouseWorld.GetPosition();
            //Debug.Log("Target position: " + newTargetPosition);  
            Move(newTargetPosition);
        }
    }

    private void Move(Vector3 newTargetPosition)  
    {
        this.targetPosition = newTargetPosition;
        isMoving = true;  // Start moving
    }
}
