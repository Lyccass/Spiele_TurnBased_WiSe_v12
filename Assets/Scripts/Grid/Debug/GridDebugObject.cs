using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
[SerializeField] TextMeshPro tmp;
 private object gridObject;
 public virtual void SetGridObject(object gridObject)
 {
    this.gridObject = gridObject;
 }

 protected virtual void Update()
 {
   tmp.text= gridObject.ToString();
 }
}
