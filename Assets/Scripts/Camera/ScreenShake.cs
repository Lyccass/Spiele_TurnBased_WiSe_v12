using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
 public static ScreenShake Instance{get; private set;}
    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
       
        if(Instance != null){
             Debug.LogError("Error, more than one ScreenShake");
             Destroy(gameObject);
        return;

    }
    Instance = this;
    cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }



    public void Shake(float intensity = 1f)
    {
          cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
