using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Video;

public class CameraController : MonoBehaviour
{
[SerializeField] private float moveSpeed = 5f;
[SerializeField] private float rotationSpeed = 100f;

[SerializeField] private float zoomAmount = 1f;
[SerializeField]private const float MIN_FOLLOW_Y_OFFSET =2f;
[SerializeField]private const float MAX_FOLLOW_Y_OFFSET =25f;
[SerializeField] private float zoomSpeed = 5f;
private Vector3 targetFollowOffset;
private CinemachineTransposer cinemachineTransposer;
private Vector3 inputMoveDir = new Vector3(0,0,0);


[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;


private void Start()
{
    cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    
}
 private void Update() 
 {
    HandleMovement();
    HandleRotation();
    HandleZoom();
 }

private void HandleMovement()
{  
    // Reset the input move direction at the start of each frame
    inputMoveDir = Vector3.zero;
    
    if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
    {
        inputMoveDir.z = +1f;
    }
     if(Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.DownArrow))
    {
        inputMoveDir.z = -1f;
    }
     if(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.LeftArrow))
    {
        inputMoveDir.x = -1f;
    }
     if(Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.RightArrow))
    {
        inputMoveDir.x = +1f;
    }
    
    Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
    transform.position += moveVector * moveSpeed * Time.deltaTime;
}


 private void HandleRotation()
 {

    Vector3 rotationVector = new Vector3(0,0,0);

    if(Input.GetKey(KeyCode.Q)|| Input.GetKey(KeyCode.Keypad1))
    {
        rotationVector.y = -1f;
    }
     if(Input.GetKey(KeyCode.E)|| Input.GetKey(KeyCode.Keypad3))
    {
        rotationVector.y = +1f;
    }

    transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;

 }


 private void HandleZoom()
 {

    if (Input.mouseScrollDelta.y > 0)
    {
          targetFollowOffset.y -= zoomAmount;
    }
       if (Input.mouseScrollDelta.y < 0)
    {
          targetFollowOffset.y += zoomAmount;
    }
    targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET,MAX_FOLLOW_Y_OFFSET);
    cinemachineTransposer.m_FollowOffset =  Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime*zoomSpeed);

 }

}
