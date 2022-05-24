using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Character : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    [SerializeField] private SpriteRenderer background = null;
    [SerializeField] private Sprite normalCharaCterSprite = null;
    private bool isTouch = false;

    private Vector3 originPos = Vector3.zero;

    private void Awake()
    {
        SetVar();
    }
    private void Start()
    {
        SetScaleToScreen();
    }
    private void OnMouseDown()
    {
        if (EventSystem.current.currentSelectedGameObject == null && !EventSystem.current.IsPointerOverGameObject())
        {
            isTouch = true;
        }
        else
        {
            isTouch = false;
        }
    }

    private void OnMouseUp()
    {
        if (isTouch)
        {
            TouchEvent();
        }

    }

    private void TouchEvent()
    {
        transform.DOShakePosition(0.3f, 0.5f, 5).OnComplete(() => transform.DOMove(originPos, 0.2f));
        GameManager.Inst.UI.OnClickDisPlay();
        SoundManager.Inst.SetEffectSound(4);
    }

    private void SetScaleToScreen()
    {
        float width = ConstantItems.ScreenToWorldWidth;
        background.transform.localScale = Vector3.one * width;
        spriteRenderer.transform.localScale = Vector3.one * width;
    }

    private void SetVar()
    {
        originPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalCharaCterSprite;
    }




}

