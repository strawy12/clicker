using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite clickCharaCterSprite = null;
    [SerializeField] private Sprite normalCharaCterSprite = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalCharaCterSprite;
    }
    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            transform.Rotate(0f, 0f, -10f);
        }
    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.Inst.UI.OnClickDisPlay();

        }
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

    }

}
