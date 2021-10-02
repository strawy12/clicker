using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    [SerializeField] private SpriteRenderer background = null;
    [SerializeField] private Sprite normalCharaCterSprite = null;
    private bool isTouch = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalCharaCterSprite;
        float width = ScreenSize.GetScreenToWorldWidth;
        background.transform.localScale = Vector3.one * width;
        spriteRenderer.transform.localScale = Vector3.one * width;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.currentSelectedGameObject == null && !EventSystem.current.IsPointerOverGameObject())
        {
            isTouch = true;
            transform.Rotate(0f, 0f, -10f);
        }
        else
        {
            isTouch = false;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnMouseUp()
    {
        if (isTouch)
        {
            GameManager.Inst.UI.OnClickDisPlay();
            SoundManager.Inst.SetEffectSound(4);

        }
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

    }

}

