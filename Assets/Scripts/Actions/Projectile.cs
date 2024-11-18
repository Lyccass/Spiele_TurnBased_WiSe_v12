using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform VFXPrefab;
    private Vector3 targetPosition;
    private float delayTimer = 0f; // Timer to accumulate time
    private float delayDuration = 0.5f; // Duration of the delay in seconds
    private bool isDelayed = false; // Flag to check if the delay has passed


    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update() 
    {
        if (!isDelayed)
        {
            // Accumulate the delay timer
            delayTimer += Time.deltaTime;

            // Check if the delay duration has passed
            if (delayTimer >= delayDuration)
            {
                isDelayed = true; // Set the delay flag
            }
            return; // Exit Update until the delay has passed
        }

        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
        float moveSpeed = 145f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;

            trailRenderer.transform.parent = null;

            Destroy(gameObject);
            Instantiate(VFXPrefab, targetPosition, Quaternion.identity);
        }
    }
}
