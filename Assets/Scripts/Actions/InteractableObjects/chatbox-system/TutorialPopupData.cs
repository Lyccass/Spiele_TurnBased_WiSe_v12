using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialPopupData
{
    public Vector2Int triggerPosition; // Position where the popup appears
    [TextArea(3, 5)] public List<string> messages; // Multiple messages for this tutorial
}
