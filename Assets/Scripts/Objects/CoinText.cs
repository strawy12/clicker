using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BigInteger = System.Numerics.BigInteger;

public class CoinText : PoolObject
{
    private bool setUp = false;
    private Text coinText = null;
    private Image coinImage = null;
    private RectTransform crtRectTransform = null; // crt = current


    private float tartgetPosY { get { return crtRectTransform.anchoredPosition.y + 100f; } }
    public void Show(BigInteger money)
    {
        if (!setUp)
        {
            SetUpComponent();
        }

        coinText.text = string.Format("+{0} ¿ø", GameManager.Inst.MoneyUnitConversion(money));
        transform.localPosition = ConstantItems.COINTEXT_FIXED_VALUE;
        gameObject.SetActive(true);

        crtRectTransform.DOAnchorPosY(tartgetPosY, 0.5f);
        coinImage.DOFade(0f, 0.5f);
        coinText.DOFade(0f, 0.5f).OnComplete(() => Despawn());
    }

    override protected void Despawn()
    {
        coinText.DOFade(1f, 0f);
        coinImage.DOFade(1f, 0f);

        base.Despawn();
    }

    private void SetUpComponent()
    {
        coinImage ??= transform.GetChild(0).GetComponent<Image>();
        coinText ??= GetComponent<Text>();
        crtRectTransform ??= GetComponent<RectTransform>();

        setUp = true;
    }
}
