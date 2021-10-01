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
                speechs[num].Add("안녕하세요 사향님, 처음 뵙겠습니다.");
                speechs[num].Add("전 당신을 안내할 매니저 OIF찍찍이 라고 합니다.");
                speechs[num].Add("편하게 윾매니저라고 불러주세요.");
                speechs[num].Add("저는 사향님이 캐릭터 샵을 운영한다는 소식에 회사 운영을 도와주러 온 매니저입니다.");
                speechs[num].Add("샵 운영은 처음이시죠?");
                break;

            case 2:
                speechs[num].Add("아, 그러시군요!");
                speechs[num].Add("그럼 제 도움이 별로 필요 없겠네요.");
                speechs[num].Add("그럼 전 가보겠습니다!");
                speechs[num].Add("멋있고 유명한 캐릭터 샵이 되길 기원합니다!");
                break;

            case 3:
                speechs[num].Add("역시 그러실줄 알고 제가 온겁니다.");
                speechs[num].Add("제가 하나부터 차근차근 설명해드리겠습니다.");
                speechs[num].Add("돈을 버는 방법은 간단합니다.");
                speechs[num].Add("제일 처음에는 아무것도 없으실겁니다.");
                speechs[num].Add("그럴 경우 돈을 벌기 위해선 스스로 일을 하셔야합니다.");
                speechs[num].Add("일을 하기 위해 한번 화면을 클릭 해 1000원을 모아보시겠어요?");
                break;
            case 4:
                speechs[num].Add("힘드시죠? 역시 세상에 쉽게 돈을 벌 수 있는 방법은 없네요.");
                speechs[num].Add("아무튼 이렇게 화면을 터치를 한다면 돈을 벌 수 있습니다.");
                speechs[num].Add("그럼 돈을 더 많이 벌어볼까요?");
                speechs[num].Add("한번 밑에 있는 “캐릭터 샵” 라는 버튼을 클릭해보시겠어요?");
                break;
            case 5:
                speechs[num].Add("잘 하셨습니다.");
                speechs[num].Add("이렇게 금사향을 강화할 시 레벨이 올라가고, ");
                speechs[num].Add("레벨에 따라 클릭당 버는 돈이 증가합니다.");
                speechs[num].Add("다음 설명할 내용은 스킬에 관련된 내용입니다.");
                speechs[num].Add("스킬에 대해 설명을 들으시겠습니까?");
                break;
            case 6:
                speechs[num].Add("알겠습니다. 멋있고 대단한 회사로 성장하시길 기원하겠습니다.");
                speechs[num].Add("지금까지 제 설명을 들어주셔서 감사합니다!");
                break;
            case 7:
                speechs[num].Add("알겠습니다.스킬에 대해서도 설명을 해드리겠습니다.");
                speechs[num].Add("스킬은 총 세가지가 있습니다.");
                speechs[num].Add("첫 번째 스킬은 트이유라는 스킬로, ");
                speechs[num].Add("5번 클릭 시 2번 추가 클릭이 되는 스킬입니다.");
                speechs[num].Add("백문이 불여일견이라고, 직접 한번 사용해보시겠어요?");
                speechs[num].Add("스킬 아이콘을 클릭할 시 스킬이 사용됩니다.");
                break;
            case 8:
                speechs[num].Add("지속시간이 너무 짧죠…?");
                speechs[num].Add("하… 너무 아쉽네요.");
                speechs[num].Add("또한 스킬엔 쿨타임이 있으니 아이콘이 활성화 될 때까진 사용하지 못합니다.");
                speechs[num].Add("그럼 한번 지속시간을 늘려볼까요?");
                speechs[num].Add("스킬을 강화한다면 지속시간이 늘어납니다");
                speechs[num].Add("하지만 아쉽게도 스킬을 강화하려면 새로운 재화인 골드가 필요합니다.");
                speechs[num].Add("뭐 처음이시니까 특별히 제가 100골드를 선물로 드리죠.");
                speechs[num].Add("한번 강화해보세요!");
                break;
            case 9:
                speechs[num].Add("스킬은 이정도가 답니다.");
                speechs[num].Add("강화하면 지속시간이 늘어나고,");
                speechs[num].Add("강화하기위해선 골드가 필요합니다.");
                speechs[num].Add("근데 두번째 스킬은 조금 다릅니다.");
                speechs[num].Add("지속 시간은 존재하지 않고 클릭당 돈의 n배의 돈을 즉시 지급받는 스킬입니다.");
                speechs[num].Add("이 스킬은 강화 할 시 n의 크기가 점점 증가합니다.");
                speechs[num].Add("쿨타임은 강화한다고 해서 줄어들지 않습니다.");
                speechs[num].Add("마지막 세번째 스킬은 사향님이 버는 모든 돈을 4배로 증가 시켜 돈을 받습니다.");
                speechs[num].Add("이 또한 지속시간동안만 적용 됩니다");
                speechs[num].Add("스킬 관련 설명은 여기서 마치겠습니다.");
                speechs[num].Add("지금까지 제 설명을 들어주셔서 감사합니다");
                speechs[num].Add("멋있고 대단한 회사로 성장하시길 기원하겠습니다.");
                break;
            case 10:
                speechs[num].Add("안녕하세요, 사향님! 또 만나네요!");
                speechs[num].Add("이번엔 찍찍이에 대해 궁금하셔서 절 부르셨군요?");
                speechs[num].Add("특별히 제가 찍찍이에 대해 설명을 드리겠습니다.");
                speechs[num].Add("찍찍이는 단순히 그냥 직원들이라고 생각하시면 됩니다.");
                speechs[num].Add("찍찍이들은 사향님이 고용 하신다면 모두 사향님을 위해 열심히 일 해줄겁니다.");
                speechs[num].Add("일을 해 버는 돈은 초당으로 들어옵니다.");
                speechs[num].Add("한번 확인 하기 위해 직원을 고용해봅시다.");
                speechs[num].Add("밑에 있는 “찍찍이” 라는 버튼을 클릭해보시겠어요?");
                break;
            case 11:
                speechs[num].Add("이름 밑에 뜨는 설명이 초당 벌리는 돈의 정보입니다.");
                speechs[num].Add("한번 10초 정도 기다려보면서 편안하게 돈을 벌어봅시다.");
                break;
            case 12:
                speechs[num].Add("이번엔 찍찍이를 강화해봅시다!");
                speechs[num].Add("강화를 하면 당연하겠지만 mPs 가 늘어납니다.");
                speechs[num].Add("응애 찍찍이를 강화해주세요!");
                break;
            case 13:
                speechs[num].Add("잘했습니다.");
                speechs[num].Add("이렇게 강화를해 레벨을 올리면 편안하게 돈을 많이 벌 수 있습니다.");
                speechs[num].Add("또한 조건을 달성하면 특정 찍찍이들이 잠금이 해제됩니다.");
                speechs[num].Add("직원에 대한 설명은 여기서 답니다.");
                speechs[num].Add("지금까지 제 설명을 들어주셔서 감사합니다.");
                speechs[num].Add("멋있고 대단한 회사로 성장하시길 기원하겠습니다.");
                break;
            case 14:
                speechs[num].Add("안녕하세요, 사향님! 또 만나네요!");
                speechs[num].Add("이번엔 펫에 대해 궁금하셔서 절 부르셨군요?");
                speechs[num].Add("특별히 제가 펫에 대해 설명을 드리겠습니다.");
                speechs[num].Add("펫은 캐릭터 샵에서 판매하는 인형들입니다.");
                speechs[num].Add("그리고 보유했을 때 일정 시간마다 한번씩 자동으로 클릭을 해줍니다");
                speechs[num].Add("펫은 특정해서 구매하는 것은 불가능하고, 뽑기를 통해 뽑을 수 있습니다.");
                speechs[num].Add("뽑기는 또한 돈으로는 구매 할 수 없으며, 골드를 통해 구매 할 수 있습니다.");
                speechs[num].Add("한번 확인 하기 위해 펫을 뽑아봅시다.");
                speechs[num].Add("밑에 있는 “펫” 이라는 버튼을 클릭해보시겠어요?");
                break;
            case 15:
                speechs[num].Add("그럼 이젠 펫을 장착해볼까요?");
                speechs[num].Add("펫을 장착하셔야 펫의 능력을 사용할 수 있습니다.");
                speechs[num].Add("화살표로 가리키는 버튼을 클릭해 펫을 장착해보세요!");
                break;
            case 16:
                speechs[num].Add("잘하셨습니다! 펫의 정보는 오른쪽 상단에 도감 버튼을 클릭하신다면 보실 수 있습니다.");
                speechs[num].Add("펫은 클릭형과 시간형으로 나누어져있고, ");
                speechs[num].Add("클릭형은 클릭수를 늘려주고 시간형은 쿨타임을 감소시켜줍니다.");
                speechs[num].Add("펫을 강화하면 맞는 타입의 효과가 증가, 또는 감소가 됩니다.");
                speechs[num].Add("또한 펫을 구매한다면 mPc의 일정량이 증가됩니다.");
                speechs[num].Add("펫에 대한 설명은 여기서 마치면서, 꿀팁을 하나 드리겠습니다.");
                speechs[num].Add("자동클릭의 기본 쿨타임은 100초입니다. 그러기에 저는 시간형을 먼저 강화를 하길 추천합니다.");
                speechs[num].Add("지금까지 제 설명을 들어주셔서 감사합니다.");
                speechs[num].Add("멋있고 대단한 회사로 성장하시길 기원하겠습니다.");
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
            GameManager.Inst.UI.ShowMessage("다시 입력해주세요.");
            return;
        }
        SettingPart(int.Parse(input.text) - 1);
    }
    public void SettingPart(int num)
    {
        if (num >= maxPartNum || num < 0)
        {
            GameManager.Inst.UI.ShowMessage("다시 입력해주세요.");
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
            GameManager.Inst.UI.ShowMessage("설정을 눌러 튜토리얼을 클릭하면 튜토리얼을 다시 진행 하실수 있습니다!", 0.3f, 0.15f, 2f, 18);
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
                    StartCoroutine(Message("어? 이미 돈을 지급을 했군요. 죄송합니다. 제가 요즘 건망증이 심해서.. ㅎㅎ"));
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
                Debug.Log("응에");
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
            StartCoroutine(Message(string.Format("오! {0}을 뽑으셨군요! 역시 튜토리얼때 뽑는 뽑기는 망하는게 국룰인가봐요ㅎㅎ")));

        }
        else if (pet.petNum == 9)
        {
            StartCoroutine(Message(string.Format("헐! {0}을 뽑으셨어요???!!! 운이 대박좋으시네요!!!! {0}은 펫중에 가장 좋은 펫이에요!!")));
        }
        else
        {
            StartCoroutine(Message(string.Format("오! {0}을 뽑으셨군요! 운이 좋으시네요!! {0}을 뽑으시다니...")));
        }
        NextPart();
    }
    private IEnumerator UseSkillEvent()
    {
        string[] message = { "스킬이 사용됐어요!!!", "지속시간이 있으니 지속시간동안 최대한의 효율을 뽑아봅시다!", "화면을 막 터치해주세요! 이번엔 저도 도와드릴께요!! 지속시간은 30초입니다!" };
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
                StartCoroutine(Message("어? 이미 강화 하셨군요! 죄송합니다. 제가 요즘 건망증이 심해졌네요..ㅎㅎ"));
                return;
            }
            StartCoroutine(Message("지금 가리키는 버튼을 클릭해 본인(금사향)을 강화 하세요."));
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
            StartCoroutine(Message("어? 이미 고용 하셨군요! 죄송합니다. 제가 요즘 건망증이 심해졌네요..ㅎㅎ"));
            return;
        }
        StartCoroutine(Message("지금 가리키는 버튼을 클릭해 응애 찍찍이를 고용 하세요."));
        InvokeRepeating("CheckClickBuyStaff", 0f, 0.5f);
        isStop = true;
        ShowArrowPoint(new Vector2(52.6f, -156.5f), true);
    }
    private void CheckClickPetBtn()
    {
        petBtn.onClick.RemoveListener(CheckClickPetBtn);

        if (GameManager.Inst.CurrentUser.goldCoin < 100)
        {
            StartCoroutine(Message("어? 돈이 부족하시네요? 특별히 제가 부족한 돈을 드리겠습니다. 지금 가리키는 버튼을 클릭해 펫을 뽑아보세요."));
            GameManager.Inst.CurrentUser.goldCoin = 100;
            GameManager.Inst.UI.UpdateMoneyPanal();
        }
        else
        {
            StartCoroutine(Message("지금 가리키는 버튼을 클릭해 펫을 뽑아보세요."));
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
