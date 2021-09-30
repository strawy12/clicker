using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IBookStaff : IBookObject
{
    private Staff staff = null;
    public void SetIBookStaff(Staff staff)
    {
        if (button == null)
        {
            button = GetComponent<Button>();
            image = transform.GetChild(0).GetComponent<Image>();
        }

        image.sprite = GameManager.Inst.UI.SoldierSpriteArray[staff.staffNum];
        this.staff = staff;
        UpdatePanal();
    }

    public override void UpdatePanal()
    {
        if (staff.isLocked)
        {
            image.color = Color.black;
            button.interactable = false;

        }
        else
        {
            image.color = Color.white;
            button.interactable = true;
        }
    }
    public override void ShowInfoPanal()
    {
        GameManager.Inst.UI.ShowInfoPanal(true, staff);
    }
}
