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
        }

        image.sprite = GameManager.Inst.UI.PetSpriteArray[pet.petNum];
        this.pet = pet;
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
            image.color = Color.white;
            button.interactable = true;
        }
    }

    public override void ShowInfoPanal()
    {
        GameManager.Inst.UI.ShowInfoPanal(true, pet);
    }
}
