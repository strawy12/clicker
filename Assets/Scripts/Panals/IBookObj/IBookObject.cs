using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IBookObject : MonoBehaviour
{
    protected Button button = null;
    protected Image image = null;
    protected Image exclamationImage = null;
    public bool isShow { get; protected set; } = false;
    private void Awake()
    {

    }

    public virtual void UpdatePanal()
    {

    }
    public virtual void ShowInfoPanal()
    {

    }
}
