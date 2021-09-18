using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PetInfo : MonoBehaviour
{
    [SerializeField] GameObject petObjectTemp = null;
    private Image petImage = null;
    private Text petNameText = null;
    private Text petInfoText = null;
    private Sprite petSprite = null;
    // Start is called before the first frame update
    void FindComponents()
    {
        petImage = transform.GetChild(2).GetComponent<Image>();
        petNameText = transform.GetChild(3).GetComponent<Text>();
        petInfoText = transform.GetChild(4).GetComponent<Text>();
    }

    public void SetInfo(Sprite petSprite, string petName, string petInfo)
    {
        if (petImage == null || petNameText == null || petInfoText == null)
            FindComponents();
        petImage.sprite = petSprite;
        petNameText.text = petName;
        petInfoText.text = petInfo;
        this.petSprite = petSprite;
    }

    public void OnClickMounting()
    {
        gameObject.SetActive(false);
        GameObject buffPanal = Instantiate(petObjectTemp, petObjectTemp.transform.parent);
        buffPanal.transform.GetChild(0).GetComponent<Image>().sprite = petSprite;
        buffPanal.SetActive(true);
        buffPanal.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InOutBack);
    }
}
