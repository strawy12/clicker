using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollScript : ScrollRect
{

    bool forParent = false;
    [SerializeField] ScrollRect mainScrollRect = null;
    [SerializeField] RectTransform mainContent = null;


    public override void OnBeginDrag(PointerEventData eventData)
    {
        forParent = Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y);
        if(forParent)
        {
            mainScrollRect.OnBeginDrag(eventData);
        }
        else
        {
            base.OnBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            mainScrollRect.OnDrag(eventData);
        }
        else
        {
            base.OnDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            mainScrollRect.OnEndDrag(eventData);
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
