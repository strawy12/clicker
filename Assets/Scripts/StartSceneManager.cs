using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Text startText = null;
    [SerializeField] private Image startFadeImage = null;
    private bool isStop = false;
    void Start()
    {
        StartCoroutine(FadeText());
        SoundManager.Inst.SetBGM(1);
    }

    public void OnClickTouchStart()
    {
        isStop = true;
        startFadeImage.DOFade(1f, 1f).OnComplete(() => SceneManager.LoadScene("Main"));
    }

    private IEnumerator FadeText()
    {
        while(!isStop)
        {
            startText.DOFade(1f, 0.7f);
            yield return new WaitForSeconds(0.7f);
            startText.DOFade(0f, 0.7f);
            yield return new WaitForSeconds(0.7f);
        }
    }

}
