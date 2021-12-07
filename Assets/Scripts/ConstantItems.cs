using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstantItems
{
    public static float ScreenToWorldHeight
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var height = edgeVector.y * 0.5f;
            return height;
        }
    }

    public static float ScreenToWorldWidth 
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var width = edgeVector.x * 0.5f;
            return width;
        }
    }
}
