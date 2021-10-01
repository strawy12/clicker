using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InFoPanal : MonoBehaviour
{
    private Image itemImage = null;
    private Text nameText = null;
    private Text infoText = null;
    private Text effectInfoText = null;

    private void Start()
    {
        nameText = transform.GetChild(0).GetComponent<Text>();
        itemImage = transform.GetChild(1).GetComponent<Image>();
        effectInfoText = transform.GetChild(3).GetComponent<Text>();
        infoText = transform.GetChild(4).GetComponent<Text>();
    }

    public void SetInfo(Staff staff)
    {
        itemImage.sprite = GameManager.Inst.UI.SoldierSpriteArray[staff.staffNum];
        nameText.text = staff.staffName;
        effectInfoText.text = string.Format("1초 당 {0} 원 획득", staff.mPs);
        infoText.text = staff.info;
    }

    public void SetInfo(Pet pet)
    {

        string type = "";
        itemImage.sprite = GameManager.Inst.UI.PetSpriteArray[pet.petNum];
        if(pet.petNum % 2 == 0)
        {
            type = "시간형";
            effectInfoText.text = string.Format("클릭 쿨타임: - {0}초", pet.petNum + pet.level * 1.25f);
        }
        else
        {
            type = "클릭형";
            effectInfoText.text = string.Format("클릭횟수: {0}", pet.petNum * pet.level);
        }
        nameText.text = string.Format("{0}\n[{1}]", pet.petName, type);
        infoText.text = pet.info;


    }
}
