using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerManager : MonoBehaviour
{
    void Awake()
    {
        CanvasScaler canvas = GetComponent<CanvasScaler>();
        float scaleheight = ((float)Screen.width / Screen.height) / (9 / 18.5f); // (가로 / 세로)
        if(scaleheight < 1f)
        {
            canvas.matchWidthOrHeight = 1f - Mathf.Clamp(scaleheight, 0f, 1f);
        }
        else
        {
            canvas.matchWidthOrHeight = 1f;
        }
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
}