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
        exclamationImage.gameObject.SetActive(false);
        this.pet = pet;
        UpdatePanal();
    }

    public override void UpdatePanal()
    {
        if (pet.isLocked)
        {
            isShow = false;
            image.color = Color.black;
            button.interactable = false;
        }
        else
        {
            isShow = pet.isShow;

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
        if (pet.isShow)
        {
            pet.isShow = false;
            isShow = pet.isShow;
        }

        GameManager.Inst.UI.ShowInfoPanal(true, pet);

        if (exclamationImage.gameObject.activeSelf)
        {
            exclamationImage.gameObject.SetActive(false);
        }
    }
}
