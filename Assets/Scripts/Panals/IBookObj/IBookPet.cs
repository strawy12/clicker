using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IBookPet : IBookObject
{
    private Pet pet = null;
    public void SetIBookPet(Pet pet)
    {
        if (button == null)
        {
            button = GetComponent<Button>();
            image = transform.GetChild(0).GetComponent<Image>();
            exclamationImage = transform.GetChild(1).GetComponent<Image>();
        }

        image.sprite = GameManager.Inst.UI.PetSpriteArray[pet.petNum];
        this.pet = pet;
        if(!pet.isLocked)
        {
        }
        UpdatePanal();
    }

    public override void UpdatePanal()
    {
        if (pet.isLocked)
        {
            image.color = Color.black;
            button.interactable = false;
        }
        else
        {
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
        isShow = false;
        GameManager.Inst.UI.ShowInfoPanal(true, pet);
        exclamationImage.gameObject.SetActive(false);
    }
}
