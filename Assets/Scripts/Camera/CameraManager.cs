using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;
    private void Start() 
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionComplete += BaseAction_OnAnyActionComplete;
    }

    private void ShowActionCamera()
    {
       // actionCameraGameObject.SetActive(true);
    }

     private void HideActionCamera()
    {
        // actionCameraGameObject.SetActive(false);
    }
    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case RangedAction rangedAction:

                Unit actionUnit = rangedAction.GetUnit();
                Unit targetUnit = rangedAction.GetTargetUnit();

                Vector3 cameraCharacterHeight = Vector3.up *1.7f;
                Vector3 attackDir = (targetUnit.GetWordPosition() - actionUnit.GetWordPosition()).normalized;
                float shoulderOffsetAmount = .5f;
                Vector3 shoulderOffset = Quaternion.Euler(0,90,0) * attackDir* shoulderOffsetAmount;

                Vector3 actionCameraPosition =
                    actionUnit.GetWordPosition() + 
                    cameraCharacterHeight + 
                    shoulderOffset + 
                    (attackDir * -1);
                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWordPosition()+ cameraCharacterHeight);
                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionComplete(object sender, EventArgs e)
    {
         switch (sender)
        {
            case RangedAction rangedAction:
                HideActionCamera();
                break;
        }
    }
}
