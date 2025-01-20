#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
public static InputManager Instance{get; private set;}
private void Awake()
   {
    if(Instance != null){
        Debug.LogError("Error, more than one InputManager");
        Destroy(gameObject);
        return;
    }
     Instance = this;
   }

public UnityEngine.Vector2 GetMouseScreenPosition()
{
#if USE_NEW_INPUT_SYSTEM
    return Mouse.current.position.ReadValue();
#else
    return Input.mousePosition;
#endif
}

public bool IsMouseButtonDown()
{
    return Input.GetMouseButtonDown(0);
}

public UnityEngine.Vector2 GetCameraMoveVector()
{
    UnityEngine.Vector2 inputMoveDir = new UnityEngine.Vector2(0,0);
    // Reset the input move direction at the start of each frame
    
    if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
    {
        inputMoveDir.y = +1f;
    }
     if(Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.DownArrow))
    {
        inputMoveDir.y = -1f;
    }
     if(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.LeftArrow))
    {
        inputMoveDir.x = -1f;
    }
     if(Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.RightArrow))
    {
        inputMoveDir.x = +1f;
    }

    return inputMoveDir;
}

public float GetCameraRotateAmount()
{
    float rotateAmout = 0f;
    if(Input.GetKey(KeyCode.Q))
    {
        rotateAmout = +1f;
    }

    if(Input.GetKey(KeyCode.E))
    {
        rotateAmout = -1f;
    }
    return rotateAmout;
}

public float GetCameraZoomAmount()
{
    float zoomAmount = 0f;
    if (Input.mouseScrollDelta.y > 0)
    {
          zoomAmount = -1f;
    }
       if (Input.mouseScrollDelta.y < 0)
    {
        zoomAmount = +1f;
    }
    return zoomAmount;
}

}
