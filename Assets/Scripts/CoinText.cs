using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoinText : MonoBehaviour
{
    private Text coinText = null;
    private Image coinImage = null;

    public void Show()
    {
        if(coinImage == null)
        {
            coinImage = transform.GetChild(0).GetComponent<Image>();
        }
        if(coinText == null)
        {
            coinText = GetComponent<Text>();
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        coinText.text = string.Format("+{0}", GameManager.Inst.CurrentUser.mPc);
        transform.position = GameManager.Inst.MousePos;
        float targetPositionY = rectTransform.anchoredPosition.y + 100f;
        gameObject.SetActive(true);

        rectTransform.DOAnchorPosY(targetPositionY, 0.5f);
        coinImage.DOFade(0f, 0.5f);
        coinText.DOFade(0f, 0.5f).OnComplete(() => Despawn());
    }

    private void Despawn()
    {
        coinText.DOFade(1f, 0f);
        coinImage.DOFade(1f, 0f);
        transform.SetParent(GameManager.Inst.Pool);
        gameObject.SetActive(false);
    }
    //private Vector3 SetPosition(long num)
    //{
    //    //if (MousePos.x < GameManager.Inst.MaxPos.x) return MousePos;
    //    //int digit = (int)Mathf.Log10(num) + 1;
    //    //Vector3 targetPos = MousePos;
    //    //switch (digit)
    //    //{
    //    //    case 1:
    //    //        targetPos.x = 1.94f;
    //    //        break;
    //    //    case 2:
    //    //        targetPos.x = 1.8f;
    //    //        break;
    //    //    case 3:
    //    //        targetPos.x = 1.7f;
    //    //        break;
    //    //    case 4:
    //    //        targetPos.x = 1.6f;
    //    //        break;
    //    //}
    //    //return targetPos;
    //}
}
