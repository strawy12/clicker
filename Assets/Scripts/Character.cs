using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    [SerializeField] private SpriteRenderer background = null;
    [SerializeField] private Sprite normalCharaCterSprite = null;

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
        if (!EventSystem.current.currentSelectedGameObject)
        {
            transform.Rotate(0f, 0f, -10f);
        }
    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.Inst.UI.OnClickDisPlay();
            SoundManager.Inst.SetEffectSound(4);
        }
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

    }

}
