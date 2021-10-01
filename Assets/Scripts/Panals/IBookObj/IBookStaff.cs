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
            exclamationImage = transform.GetChild(1).GetComponent<Image>();
        }

        image.sprite = GameManager.Inst.UI.StaffSpriteArray[staff.staffNum];
        this.staff = staff;
        if (!staff.isShow)
        {
            exclamationImage.gameObject.SetActive(false);
            isLocked = false;
            GameManager.Inst.UI.ShowNewIBook(false);
        }
        UpdatePanal();
    }

    public override void UpdatePanal()
    {
        if (!staff.isSold)
        {
            image.color = Color.black;
            button.interactable = false;
            exclamationImage.gameObject.SetActive(false);
            GameManager.Inst.UI.ShowNewIBook(false);

        }
        else
        {
            if (isLocked)
            {
                isLocked = false;
                GameManager.Inst.UI.ShowNewIBook(true);
                exclamationImage.gameObject.SetActive(true);
            }
            image.color = Color.white;
            button.interactable = true;
        }
    }
    public override void ShowInfoPanal()
    {
        exclamationImage.gameObject.SetActive(false);

        GameManager.Inst.UI.ShowInfoPanal(true, staff);
    }
}
