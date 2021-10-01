using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using BigInteger = System.Numerics.BigInteger;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] Collider2D touchScreen = null;
    [SerializeField] RectTransform textPanal = null;
    [SerializeField] Image progressBtn = null;
    [SerializeField] GameObject managerObj = null;
    [SerializeField] RectTransform arrowPoint = null;
    [SerializeField] Button randomPickPetBtn = null;
    [SerializeField] Button companyBtn = null;
    [SerializeField] Button staffBtn = null;
    [SerializeField] Button petBtn = null;
    [SerializeField] Button upgradeBtn = null;
    [SerializeField] RectTransform[] blackPanal = null;
    [SerializeField] private int maxPartNum = 0;

    private Text messageText = null;
    private Dictionary<int, List<string>> speechs;
    private int partNum = 1;
    private float textSpeed = 0.03f;
    private int storyCnt = 0;
    public int progressPartNum { get; private set; } = 0;
    private bool isTyping = false;
    private bool isStop = false;
    private void Start()
    {
        messageText = textPanal.transform.GetChild(3).GetComponent<Text>();
        speechs = new Dictionary<int, List<string>>();
        SpeechSetting(maxPartNum);
        progressPartNum = 0;
        if (GameManager.Inst.CurrentUser.isTuto[0])
        {
            if (!GameManager.Inst.CurrentUser.isTuto[1])
            {
                partNum = 7;
                progressPartNum = 2;
                if (GameManager.Inst.CurrentUser.skills[0].endTime != "")
                {
                    GameManager.Inst.CurrentUser.skills[0].endTime = "";
                    GameManager.Inst.CurrentUser.skills[0].endDurationTime = "";
                    GameManager.Inst.CurrentUser.skills[0].isUsed = false;
                }
            }
            else
            {
                return;
            }
        }
        StartCoroutine(GameStart(true));
    }

    private void SpeechSetting(int cnt)
    {
        for (int i = 1; i <= cnt; i++)
        {
            speechs.Add(i, new List<string>());
            AddSpeech(i);
        }
    }

    private void AddSpeech(int num)
    {

        switch (num)
        {
            case 1:
                speechs[num].Add("�ȳ��ϼ��� �����, ó�� �˰ڽ��ϴ�.");
                speechs[num].Add("�� ����� �ȳ��� �Ŵ��� OIF������ ��� �մϴ�.");
                speechs[num].Add("���ϰ� ���Ŵ������ �ҷ��ּ���.");
                speechs[num].Add("���� ������� ĳ���� ���� ��Ѵٴ� �ҽĿ� ȸ�� ��� �����ַ� �� �Ŵ����Դϴ�.");
                speechs[num].Add("�� ��� ó���̽���?");
                break;

            case 2:
                speechs[num].Add("��, �׷��ñ���!");
                speechs[num].Add("�׷� �� ������ ���� �ʿ� ���ڳ׿�.");
                speechs[num].Add("�׷� �� �����ڽ��ϴ�!");
                speechs[num].Add("���ְ� ������ ĳ���� ���� �Ǳ� ����մϴ�!");
                break;

            case 3:
                speechs[num].Add("���� �׷����� �˰� ���� �°̴ϴ�.");
                speechs[num].Add("���� �ϳ����� �������� �����ص帮�ڽ��ϴ�.");
                speechs[num].Add("���� ���� ����� �����մϴ�.");
                speechs[num].Add("���� ó������ �ƹ��͵� �����ǰ̴ϴ�.");
                speechs[num].Add("�׷� ��� ���� ���� ���ؼ� ������ ���� �ϼž��մϴ�.");
                speechs[num].Add("���� �ϱ� ���� �ѹ� ȭ���� Ŭ�� �� 1000���� ��ƺ��ðھ��?");
                break;
            case 4:
                speechs[num].Add("�������? ���� ���� ���� ���� �� �� �ִ� ����� ���׿�.");
                speechs[num].Add("�ƹ�ư �̷��� ȭ���� ��ġ�� �Ѵٸ� ���� �� �� �ֽ��ϴ�.");
                speechs[num].Add("�׷� ���� �� ���� ������?");
                speechs[num].Add("�ѹ� �ؿ� �ִ� ��ĳ���� ���� ��� ��ư�� Ŭ���غ��ðھ��?");
                break;
            case 5:
                speechs[num].Add("�� �ϼ̽��ϴ�.");
                speechs[num].Add("�̷��� �ݻ����� ��ȭ�� �� ������ �ö󰡰�, ");
                speechs[num].Add("������ ���� Ŭ���� ���� ���� �����մϴ�.");
                speechs[num].Add("���� ������ ������ ��ų�� ���õ� �����Դϴ�.");
                speechs[num].Add("��ų�� ���� ������ �����ðڽ��ϱ�?");
                break;
            case 6:
                speechs[num].Add("�˰ڽ��ϴ�. ���ְ� ����� ȸ��� �����Ͻñ� ����ϰڽ��ϴ�.");
                speechs[num].Add("���ݱ��� �� ������ ����ּż� �����մϴ�!");
                break;
            case 7:
                speechs[num].Add("�˰ڽ��ϴ�.��ų�� ���ؼ��� ������ �ص帮�ڽ��ϴ�.");
                speechs[num].Add("��ų�� �� �������� �ֽ��ϴ�.");
                speechs[num].Add("ù ��° ��ų�� Ʈ������� ��ų��, ");
                speechs[num].Add("5�� Ŭ�� �� 2�� �߰� Ŭ���� �Ǵ� ��ų�Դϴ�.");
                speechs[num].Add("�鹮�� �ҿ��ϰ��̶��, ���� �ѹ� ����غ��ðھ��?");
                speechs[num].Add("��ų �������� Ŭ���� �� ��ų�� ���˴ϴ�.");
                break;
            case 8:
                speechs[num].Add("���ӽð��� �ʹ� ª�ҡ�?");
                speechs[num].Add("�ϡ� �ʹ� �ƽ��׿�.");
                speechs[num].Add("���� ��ų�� ��Ÿ���� ������ �������� Ȱ��ȭ �� ������ ������� ���մϴ�.");
                speechs[num].Add("�׷� �ѹ� ���ӽð��� �÷������?");
                speechs[num].Add("��ų�� ��ȭ�Ѵٸ� ���ӽð��� �þ�ϴ�");
                speechs[num].Add("������ �ƽ��Ե� ��ų�� ��ȭ�Ϸ��� ���ο� ��ȭ�� ��尡 �ʿ��մϴ�.");
                speechs[num].Add("�� ó���̽ôϱ� Ư���� ���� 100��带 ������ �帮��.");
                speechs[num].Add("�ѹ� ��ȭ�غ�����!");
                break;
            case 9:
                speechs[num].Add("��ų�� �������� ��ϴ�.");
                speechs[num].Add("��ȭ�ϸ� ���ӽð��� �þ��,");
                speechs[num].Add("��ȭ�ϱ����ؼ� ��尡 �ʿ��մϴ�.");
                speechs[num].Add("�ٵ� �ι�° ��ų�� ���� �ٸ��ϴ�.");
                speechs[num].Add("���� �ð��� �������� �ʰ� Ŭ���� ���� n���� ���� ��� ���޹޴� ��ų�Դϴ�.");
                speechs[num].Add("�� ��ų�� ��ȭ �� �� n�� ũ�Ⱑ ���� �����մϴ�.");
                speechs[num].Add("��Ÿ���� ��ȭ�Ѵٰ� �ؼ� �پ���� �ʽ��ϴ�.");
                speechs[num].Add("������ ����° ��ų�� ������� ���� ��� ���� 4��� ���� ���� ���� �޽��ϴ�.");
                speechs[num].Add("�� ���� ���ӽð����ȸ� ���� �˴ϴ�");
                speechs[num].Add("��ų ���� ������ ���⼭ ��ġ�ڽ��ϴ�.");
                speechs[num].Add("���ݱ��� �� ������ ����ּż� �����մϴ�");
                speechs[num].Add("���ְ� ����� ȸ��� �����Ͻñ� ����ϰڽ��ϴ�.");
                break;
            case 10:
                speechs[num].Add("�ȳ��ϼ���, �����! �� �����׿�!");
                speechs[num].Add("�̹��� �����̿� ���� �ñ��ϼż� �� �θ��̱���?");
                speechs[num].Add("Ư���� ���� �����̿� ���� ������ �帮�ڽ��ϴ�.");
                speechs[num].Add("�����̴� �ܼ��� �׳� �������̶�� �����Ͻø� �˴ϴ�.");
                speechs[num].Add("�����̵��� ������� ��� �ϽŴٸ� ��� ������� ���� ������ �� ���̴ٰϴ�.");
                speechs[num].Add("���� �� ���� ���� �ʴ����� ���ɴϴ�.");
                speechs[num].Add("�ѹ� Ȯ�� �ϱ� ���� ������ ����غ��ô�.");
                speechs[num].Add("�ؿ� �ִ� �������̡� ��� ��ư�� Ŭ���غ��ðھ��?");
                break;
            case 11:
                speechs[num].Add("�̸� �ؿ� �ߴ� ������ �ʴ� ������ ���� �����Դϴ�.");
                speechs[num].Add("�ѹ� 10�� ���� ��ٷ����鼭 ����ϰ� ���� ����ô�.");
                break;
            case 12:
                speechs[num].Add("�̹��� �����̸� ��ȭ�غ��ô�!");
                speechs[num].Add("��ȭ�� �ϸ� �翬�ϰ����� mPs �� �þ�ϴ�.");
                speechs[num].Add("���� �����̸� ��ȭ���ּ���!");
                break;
            case 13:
                speechs[num].Add("���߽��ϴ�.");
                speechs[num].Add("�̷��� ��ȭ���� ������ �ø��� ����ϰ� ���� ���� �� �� �ֽ��ϴ�.");
                speechs[num].Add("���� ������ �޼��ϸ� Ư�� �����̵��� ����� �����˴ϴ�.");
                speechs[num].Add("������ ���� ������ ���⼭ ��ϴ�.");
                speechs[num].Add("���ݱ��� �� ������ ����ּż� �����մϴ�.");
                speechs[num].Add("���ְ� ����� ȸ��� �����Ͻñ� ����ϰڽ��ϴ�.");
                break;
            case 14:
                speechs[num].Add("�ȳ��ϼ���, �����! �� �����׿�!");
                speechs[num].Add("�̹��� �꿡 ���� �ñ��ϼż� �� �θ��̱���?");
                speechs[num].Add("Ư���� ���� �꿡 ���� ������ �帮�ڽ��ϴ�.");
                speechs[num].Add("���� ĳ���� ������ �Ǹ��ϴ� �������Դϴ�.");
                speechs[num].Add("�׸��� �������� �� ���� �ð����� �ѹ��� �ڵ����� Ŭ���� ���ݴϴ�");
                speechs[num].Add("���� Ư���ؼ� �����ϴ� ���� �Ұ����ϰ�, �̱⸦ ���� ���� �� �ֽ��ϴ�.");
                speechs[num].Add("�̱�� ���� �����δ� ���� �� �� ������, ��带 ���� ���� �� �� �ֽ��ϴ�.");
                speechs[num].Add("�ѹ� Ȯ�� �ϱ� ���� ���� �̾ƺ��ô�.");
                speechs[num].Add("�ؿ� �ִ� ���ꡱ �̶�� ��ư�� Ŭ���غ��ðھ��?");
                break;
            case 15:
                speechs[num].Add("�׷� ���� ���� �����غ����?");
                speechs[num].Add("���� �����ϼž� ���� �ɷ��� ����� �� �ֽ��ϴ�.");
                speechs[num].Add("ȭ��ǥ�� ����Ű�� ��ư�� Ŭ���� ���� �����غ�����!");
                break;
            case 16:
                speechs[num].Add("���ϼ̽��ϴ�! ���� ������ ������ ��ܿ� ���� ��ư�� Ŭ���ϽŴٸ� ���� �� �ֽ��ϴ�.");
                speechs[num].Add("���� Ŭ������ �ð������� ���������ְ�, ");
                speechs[num].Add("Ŭ������ Ŭ������ �÷��ְ� �ð����� ��Ÿ���� ���ҽ����ݴϴ�.");
                speechs[num].Add("���� ��ȭ�ϸ� �´� Ÿ���� ȿ���� ����, �Ǵ� ���Ұ� �˴ϴ�.");
                speechs[num].Add("���� ���� �����Ѵٸ� mPc�� �������� �����˴ϴ�.");
                speechs[num].Add("�꿡 ���� ������ ���⼭ ��ġ�鼭, ������ �ϳ� �帮�ڽ��ϴ�.");
                speechs[num].Add("�ڵ�Ŭ���� �⺻ ��Ÿ���� 100���Դϴ�. �׷��⿡ ���� �ð����� ���� ��ȭ�� �ϱ� ��õ�մϴ�.");
                speechs[num].Add("���ݱ��� �� ������ ����ּż� �����մϴ�.");
                speechs[num].Add("���ְ� ����� ȸ��� �����Ͻñ� ����ϰڽ��ϴ�.");
                break;
        }
    }

    public void SettingTextSPeed(float value)
    {
        textSpeed = value * 0.1f;
    }

    public void StopTutorial()
    {
        StartCoroutine(GameStart(false));
    }

    public void OnClickProgressBtn()
    {
        if (isTyping) return;
        if (CheckEvent()) return;
        StartMessage();
    }

    private void NextPart()
    {
        isStop = false;
        partNum++;
        storyCnt = 0;
    }

    private void SetTouchScreen(bool isClicker)
    {
        touchScreen.enabled = isClicker;
        progressBtn.gameObject.SetActive(!isClicker);
    }

    private IEnumerator Message(string message)
    {
        isTyping = true;
        float posY = managerObj.transform.position.y;
        Coroutine coroutine = StartCoroutine(TalkEffectManager(0.1f));
        SoundManager.Inst.SetTutoEffectAudio(5);
        for (int i = 0; i < message.Length; i++)
        {
            
            messageText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(textSpeed);
        }
        StopCoroutine(coroutine);
        SoundManager.Inst.StopTutoEffect();
        managerObj.transform.DOMoveY(posY, 0.1f);
        isTyping = false;
    }

    private IEnumerator TalkEffectManager(float delay)
    {
        float posY = managerObj.transform.position.y;
        while (isTyping)
        {
            managerObj.transform.DOMoveY(posY + 0.3f, delay);
            yield return new WaitForSeconds(delay + 0.05f);
            managerObj.transform.DOMoveY(posY, delay);
            yield return new WaitForSeconds(delay + 0.05f);
        }
    }

    public void OnClickSelectingBtn(bool isPositive)
    {
        if (!isStop || isTyping) return;
        isStop = false;
        partNum += isPositive ? 2 : 1;
        storyCnt = 0;
        GameManager.Inst.UI.ShowSelectingPanal(false);
        progressPartNum++;
        StartMessage();
    }

    private bool CheckEvent()
    {
        if (speechs[partNum].Count <= storyCnt || isStop)
        {
            CheckPartNum(partNum);
            return true;
        }
        return false;
    }

    private void StartMessage()
    {
        StartCoroutine(Message(speechs[partNum][storyCnt]));
        storyCnt++;
    }

    public void OnClickSettingPartTuto(InputField input)
    {
        if (input.text == "")
        {
            GameManager.Inst.UI.ShowMessage("�ٽ� �Է����ּ���.");
            return;
        }
        SettingPart(int.Parse(input.text) - 1);
    }
    public void SettingPart(int num)
    {
        if (num >= maxPartNum || num < 0)
        {
            GameManager.Inst.UI.ShowMessage("�ٽ� �Է����ּ���.");
            return;
        }
        progressPartNum = num + 1;
        storyCnt = 0;
        switch (num)
        {
            case 0:
                partNum = 0;
                break;
            case 1:
                partNum = 7;
                break;
            case 2:
                partNum = 10;
                break;
            case 3:
                partNum = 14;
                break;
        }
        StartCoroutine(GameStart(true));
    }

    private IEnumerator GameStart(bool isStart)
    {
        if (isStart)
        {
            SoundManager.Inst.SetBGM(1);
            GameManager.Inst.isTutorial = true;
            isTyping = true;
            SetTouchScreen(false);
            blackPanal[0].gameObject.SetActive(true);
            blackPanal[1].gameObject.SetActive(true);
            GameManager.Inst.UI.seletingBtns[0].onClick.AddListener(() => OnClickSelectingBtn(true));
            GameManager.Inst.UI.seletingBtns[1].onClick.AddListener(() => OnClickSelectingBtn(false));
            yield return new WaitForSeconds(2f);
            touchScreen.transform.DOMoveX(0.75f, 0.5f);
            textPanal.gameObject.SetActive(true);
            textPanal.transform.localScale = new Vector3(0f, 1f, 1f);
            managerObj.transform.DOScale(1f, 0.3f).OnComplete(() => textPanal.transform.DOScaleX(1f, 0.3f));
            blackPanal[0].DOAnchorPosY(0f, 0.5f);
            blackPanal[1].DOAnchorPosY(0f, 0.5f);
            yield return new WaitForSeconds(0.7f);
            if (GameManager.Inst.CurrentUser.isTuto[0])
            {
                SetSkillTutorial();
                yield return new WaitForSeconds(0.5f);
            }
            StartMessage();
        }
        else
        {
            progressBtn.gameObject.SetActive(false);
            SoundManager.Inst.SetBGM(0);
            blackPanal[0].DOAnchorPosY(-100f, 0.5f);
            blackPanal[1].DOAnchorPosY(100f, 0.5f);
            touchScreen.transform.DOMoveX(0f, 0.5f);
            textPanal.transform.DOScaleX(0f, 0.5f);
            managerObj.transform.DOScale(0f, 0.5f);
            yield return new WaitForSeconds(0.6f);
            GameManager.Inst.UI.ShowMessage("������ ���� Ʃ�丮���� Ŭ���ϸ� Ʃ�丮���� �ٽ� ���� �ϽǼ� �ֽ��ϴ�!", 0.3f, 0.15f, 2f, 18);
            yield return new WaitForSeconds(2f);
            GameManager.Inst.isTutorial = false;
            SetTouchScreen(true);
            textPanal.gameObject.SetActive(false);
            blackPanal[1].gameObject.SetActive(false);
            blackPanal[0].gameObject.SetActive(false);
            CancelInvoke();

            if (GameManager.Inst.CurrentUser.tutoAllClear)
            {
                GameManager.Inst.CurrentUser.isTuto[4] = true;
                GameManager.Inst.CurrentUser.goldCoin += 300;
            }

            StopAllCoroutines();
        }
    }

    private void SetSkillTutorial()
    {
        blackPanal[1].DOAnchorPosY(100f, 0.5f);
        blackPanal[0].DOAnchorPosY(-100f, 0.5f);
        progressBtn.rectTransform.sizeDelta = new Vector2(progressBtn.rectTransform.sizeDelta.x, 500f);
        progressBtn.rectTransform.localPosition = new Vector2(progressBtn.rectTransform.localPosition.x, 123.5f);
        GameManager.Inst.UI.OnClickShowBtn(0);
        textPanal.DOAnchorPosY(518f, 0.5f);
        managerObj.transform.DOMoveY(-0.63f, 0.5f);
    }


    private void CheckPartNum(int num)
    {
        switch (num)
        {
            case 1:
                GameManager.Inst.UI.ShowSelectingPanal(true);
                isStop = true;
                break;

            case 2:
            case 6:
                GameManager.Inst.CurrentUser.isTuto[0] = true;
                GameManager.Inst.CurrentUser.isTuto[1] = true;
                StartCoroutine(GameStart(false));
                return;

            case 3:
                if (isStop) return;
                blackPanal[1].DOAnchorPosY(100f, 0.5f);
                InvokeRepeating("CheckTouchScreen", 0f, 0.5f);
                break;

            case 4:
                blackPanal[0].DOAnchorPosY(-100f, 0.5f);
                progressBtn.rectTransform.sizeDelta = new Vector2(progressBtn.rectTransform.sizeDelta.x, 500f);
                progressBtn.rectTransform.localPosition = new Vector2(progressBtn.rectTransform.localPosition.x, 123.5f);
                isStop = true;

                companyBtn.onClick.AddListener(CheckClickCompanyBtn);
                break;

            case 5:
                isStop = true;
                GameManager.Inst.UI.ShowSelectingPanal(true);
                GameManager.Inst.CurrentUser.isTuto[0] = true;
                break;

            case 7:
                if (isStop) return;
                isStop = true;
                ShowArrowPoint(new Vector2(-82f, -217.3f), false);
                if (GameManager.Inst.CurrentUser.skills[0].endTime != "")
                {
                    GameManager.Inst.CurrentUser.skills[0].endTime = "";
                    GameManager.Inst.CurrentUser.skills[0].endDurationTime = "";
                    GameManager.Inst.CurrentUser.skills[0].isUsed = false;
                }
                InvokeRepeating("CheckClickSkillBtn", 0f, 0.5f);
                break;

            case 8:
                if (isStop) return;
                if (GameManager.Inst.CurrentUser.goldCoin == 0 && GameManager.Inst.CurrentUser.skills[0].level == 1)
                {
                    GameManager.Inst.CurrentUser.goldCoin += 100;
                    GameManager.Inst.UI.UpdateMoneyPanal();
                }
                else
                {
                    NextPart();
                    StartCoroutine(Message("��? �̹� ���� ������ �߱���. �˼��մϴ�. ���� ���� �Ǹ����� ���ؼ�.. ����"));
                }
                isStop = true;
                ShowArrowPoint(new Vector2(66.9f, -214.7f), true);
                InvokeRepeating("CheckClickSkillUpgradeBtn", 0f, 0.5f);
                break;

            case 9:

                StartCoroutine(GameStart(false));
                if (!GameManager.Inst.CurrentUser.isTuto[1])
                {
                    GameManager.Inst.CurrentUser.isTuto[1] = true;
                    GameManager.Inst.CurrentUser.goldCoin += 150;
                }
                break;

            case 10:
                isStop = true;
                Debug.Log("����");
                staffBtn.onClick.AddListener(CheckClickStaffBtn);
                break;

            case 11:
                isStop = true;
                SetTouchScreen(true);
                Invoke("DelayStory", 10f);
                break;

            case 12:
                isStop = true;
                InvokeRepeating("CheckClickUpgradeStaff", 0f, 0.5f);
                break;

            case 13:
                StartCoroutine(GameStart(false));
                if (!GameManager.Inst.CurrentUser.isTuto[2])
                {
                    GameManager.Inst.CurrentUser.isTuto[2] = true;
                    GameManager.Inst.CurrentUser.goldCoin += 50;
                }
                break;
            case 14:
                isStop = true;
                petBtn.onClick.AddListener(CheckClickPetBtn);
                break;
            case 15:
                isStop = true;
                ShowArrowPoint(new Vector2(-57.2f, -240f), false);
                InvokeRepeating("CheckClickPetMountingBtn", 0f, 0.5f);
                break;
            case 16:
                StartCoroutine(GameStart(false));
                if (!GameManager.Inst.CurrentUser.isTuto[3])
                {
                    GameManager.Inst.CurrentUser.isTuto[3] = true;
                    GameManager.Inst.CurrentUser.goldCoin += 50;
                }
                break;
        }
    }

    private void ShowArrowPoint(Vector2 pos, bool isRight)
    {
        arrowPoint.localPosition = pos;
        if (isRight)
        {
            arrowPoint.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            arrowPoint.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        StartCoroutine(ArrowPointEffect());
    }

    private void DelayStory()
    {
        NextPart();
        SetTouchScreen(false);
    }

    private void CheckClickPetMountingBtn()
    {
        if (GameManager.Inst.CurrentUser.petCount > 0)
        {
            NextPart();
            CancelInvoke("CheckClickPetMountingBtn");
        }
    }

    private void CheckClickSkillUpgradeBtn()
    {
        if (GameManager.Inst.CurrentUser.skills[0].level != 1)
        {
            NextPart();
            CancelInvoke("CheckClickSkillUpgradeBtn");
        }
    }

    private void CheckClickSkillBtn()
    {
        if (GameManager.Inst.CurrentUser.skills[0].isUsed)
        {
            isStop = false;
            StartCoroutine(UseSkillEvent());
            CancelInvoke("CheckClickSkillBtn");

        }
    }

    private void CheckClickUpgradeStaff()
    {
        if (GameManager.Inst.CurrentUser.staffs[0].level > 1)
        {

            NextPart();
            CancelInvoke("CheckClickUpgradeStaff");
        }
    }

    private void CheckClickBuyStaff()
    {
        if (GameManager.Inst.CurrentUser.staffs[0].isSold)
        {
            NextPart();
            CancelInvoke("CheckClickBuyStaff");

        }
    }
    private void CheckClickRandomPickBtn()
    {
        randomPickPetBtn.onClick.RemoveListener(CheckClickRandomPickBtn);
        Pet pet = null;
        foreach (Pet pet_ in GameManager.Inst.CurrentUser.pets)
        {
            if (!pet_.isLocked)
            {
                pet = pet_;
                return;
            }
        }
        if (pet.petNum < 6)
        {
            StartCoroutine(Message(string.Format("��! {0}�� �����̱���! ���� Ʃ�丮�� �̴� �̱�� ���ϴ°� �����ΰ����䤾��")));

        }
        else if (pet.petNum == 9)
        {
            StartCoroutine(Message(string.Format("��! {0}�� �����̾��???!!! ���� ��������ó׿�!!!! {0}�� ���߿� ���� ���� ���̿���!!")));
        }
        else
        {
            StartCoroutine(Message(string.Format("��! {0}�� �����̱���! ���� �����ó׿�!! {0}�� �����ôٴ�...")));
        }
        NextPart();
    }
    private IEnumerator UseSkillEvent()
    {
        string[] message = { "��ų�� ���ƾ��!!!", "���ӽð��� ������ ���ӽð����� �ִ����� ȿ���� �̾ƺ��ô�!", "ȭ���� �� ��ġ���ּ���! �̹��� ���� ���͵帱����!! ���ӽð��� 30���Դϴ�!" };
        isStop = true;
        for (int i = 0; i < message.Length; i++)
        {
            StartCoroutine(Message(message[i]));
            yield return new WaitForSeconds(message[i].Length * 0.03f + 0.4f);
        }

        touchScreen.enabled = true;
        progressBtn.gameObject.SetActive(false);
        isTyping = true;
        SoundManager.Inst.SetSpeedBGM(1.3f);
        StartCoroutine(TalkEffectManager(0.05f));
        StartCoroutine(ManagerClick());

        yield return new WaitForSeconds(27f);
        SoundManager.Inst.SetSpeedBGM(1f);

        SetTouchScreen(false);
        NextPart();
        isTyping = false;

        StartMessage();
    }

    private IEnumerator ManagerClick()
    {
        while (isTyping)
        {
            yield return new WaitForSeconds(0.3f);
            GameManager.Inst.UI.OnClickDisPlay();
        }
    }

    private void CheckCKickUpgradeBtn()
    {
        upgradeBtn.onClick.RemoveListener(CheckCKickUpgradeBtn);
        NextPart();
    }

    private void CheckClickCompanyBtn()
    {
        companyBtn.onClick.RemoveListener(CheckClickCompanyBtn);
        textPanal.DOAnchorPosY(518f, 0.5f);
        managerObj.transform.DOMoveY(-0.63f, 0.5f).OnComplete(() =>
        {
            if (GameManager.Inst.CurrentUser.sahayang.level > 1)
            {
                NextPart();
                StartCoroutine(Message("��? �̹� ��ȭ �ϼ̱���! �˼��մϴ�. ���� ���� �Ǹ����� �������׿�..����"));
                return;
            }
            StartCoroutine(Message("���� ����Ű�� ��ư�� Ŭ���� ����(�ݻ���)�� ��ȭ �ϼ���."));
            upgradeBtn.onClick.AddListener(CheckCKickUpgradeBtn);
            isStop = true;
            ShowArrowPoint(new Vector2(52.6f, -156.5f), true);
        });

    }

    private void CheckClickStaffBtn()
    {
        staffBtn.onClick.RemoveListener(CheckClickStaffBtn);

        if (GameManager.Inst.CurrentUser.staffs[0].isSold)
        {
            NextPart();
            StartCoroutine(Message("��? �̹� ��� �ϼ̱���! �˼��մϴ�. ���� ���� �Ǹ����� �������׿�..����"));
            return;
        }
        StartCoroutine(Message("���� ����Ű�� ��ư�� Ŭ���� ���� �����̸� ��� �ϼ���."));
        InvokeRepeating("CheckClickBuyStaff", 0f, 0.5f);
        isStop = true;
        ShowArrowPoint(new Vector2(52.6f, -156.5f), true);
    }
    private void CheckClickPetBtn()
    {
        petBtn.onClick.RemoveListener(CheckClickPetBtn);

        if (GameManager.Inst.CurrentUser.goldCoin < 100)
        {
            StartCoroutine(Message("��? ���� �����Ͻó׿�? Ư���� ���� ������ ���� �帮�ڽ��ϴ�. ���� ����Ű�� ��ư�� Ŭ���� ���� �̾ƺ�����."));
            GameManager.Inst.CurrentUser.goldCoin = 100;
            GameManager.Inst.UI.UpdateMoneyPanal();
        }
        else
        {
            StartCoroutine(Message("���� ����Ű�� ��ư�� Ŭ���� ���� �̾ƺ�����."));
        }
        randomPickPetBtn.onClick.AddListener(CheckClickRandomPickBtn);
        isStop = true;
        ShowArrowPoint(new Vector2(52.6f, -156.5f), true);
    }

    private void CheckTouchScreen()
    {
        if (GameManager.Inst.CurrentUser.money > 1000)
        {
            touchScreen.enabled = false;
            progressBtn.gameObject.SetActive(true);
            NextPart();
            CancelInvoke("CheckTouchScreen");
        }
        else
        {
            SetTouchScreen(true);
            isStop = true;

        }
    }
    private IEnumerator ArrowPointEffect()
    {
        arrowPoint.gameObject.SetActive(true);
        while (isStop)
        {
            arrowPoint.DOAnchorPosX(arrowPoint.localPosition.x + 10f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            arrowPoint.DOAnchorPosX(arrowPoint.localPosition.x - 10f, 0.5f);
            yield return new WaitForSeconds(0.5f);

        }
        arrowPoint.gameObject.SetActive(false);

    }
}
