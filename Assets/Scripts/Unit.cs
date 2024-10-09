using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

[SerializeField]float rotateSpeed = 10f;
    [SerializeField ]private Animator unitAnimator;
    private Vector3 targetPosition;
    private void Awake() {
        targetPosition = transform.position;
    }
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
            //smoothing the rotation
            
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
            unitAnimator.SetBool("isWalking",true);
        }
        else
        {
            unitAnimator.SetBool("isWalking",false);
            isMoving = false; // Stop moving when close enough
        }

       
    }

    public void Move(Vector3 newTargetPosition)  
    {
        this.targetPosition = newTargetPosition;
        isMoving = true;  // Start moving
    }
}
