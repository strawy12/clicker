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
        exclamationImage.gameObject.SetActive(false);
        this.staff = staff;
        UpdatePanal();
    }

    public override void UpdatePanal()
    {
        if (!staff.isSold)
        {
            isShow = false;
            image.color = Color.black;
            button.interactable = false;
        }
        else
        {
            isShow = staff.isShow;

            if (isShow)
            {
                exclamationImage.gameObject.SetActive(true);
            }
            else
            {
                exclamationImage.gameObject.SetActive(false);
            }

            image.color = Color.white;
            button.interactable = true;
        }
    }
    public override void ShowInfoPanal()
    {
        if(staff.isShow)
        {
            staff.isShow = false;
            isShow = staff.isShow;
        }
        
        GameManager.Inst.UI.ShowInfoPanal(true, staff);

        if(exclamationImage.gameObject.activeSelf)
        {
            exclamationImage.gameObject.SetActive(false);
        }
    }
}
