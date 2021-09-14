using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlavePanal : UpgradePanal
{
    [SerializeField] private Text soldierNameText = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private Text amoutText = null;
    [SerializeField] private Image soldierImage = null;

    private Sprite soldierSprite = null;
}
