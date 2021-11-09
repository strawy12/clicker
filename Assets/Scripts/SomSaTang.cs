using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EPoolingType = GameManager.EPoolingType;

public class SomSaTang : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        if (transform.position.x >= GameManager.Inst.MaxPos.x + 0.5f)
        {
            Despawn();
        }
    }
    private void Despawn()
    {
        transform.DOKill();
        transform.SetParent(GameManager.Inst.Pool);
        gameObject.SetActive(false);
    }

    public void ClickSomSatang()
    {
        GameManager.Inst.CurrentUser.UpdateMoney(GameManager.Inst.CurrentUser.mPc * 100, true);
        GameManager.Inst.CurrentUser.bigHeartClickCnt++;
        transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            Despawn();
        });
    }
}
