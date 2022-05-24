using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites : MonoBehaviour
{
    [SerializeField] Material material = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            material.SetColor("_Color", Color.red);
    }

}
