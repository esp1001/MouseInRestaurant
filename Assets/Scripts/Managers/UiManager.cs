using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour, IHandler<StartGameEventMessage>, IHandler<RemoveHeartEventMessage>, IHandler<LevelDoneEventMessage>, IHandler<EndGameEventMessage>, IHandler<LevelReadyEventMessage>
{

    public static UiManager Instance { get; private set; }

    [Header("UI GameObjects")]
    public GameObject LogoPanel;
    public Image LogoPanelImage;
    public List<Image> LogoParts;
    public GameObject StartButton;
    public TextMeshProUGUI LogoInfoTextGood;
    public TextMeshProUGUI LogoInfoTextBad;
    private readonly string _levelDoneText = "Level %1 done!";
    private readonly string _loseText = "<size=180%>You lose!</font>\n<size=60%>Mouse captured the world!</font>";


    private readonly float _logoPanelTimeEffect = 0.5f;

    [SerializeField] private List<Image> _heartImages;

    [Header("Variables")] private float _heartFillTime = 0.3f;


    void Awake()
    {
        Instance = this;
        Init();
    }

    public void Init()
    {
        AppManager.Instance.EventAgregator.AddHandler<StartGameEventMessage>(this);
        AppManager.Instance.EventAgregator.AddHandler<RemoveHeartEventMessage>(this);
        AppManager.Instance.EventAgregator.AddHandler<LevelDoneEventMessage>(this);
        AppManager.Instance.EventAgregator.AddHandler<EndGameEventMessage>(this);
        ShowLogoPanel(true, true, false);
        ShowLogoText(false);
    }

    private void OnDestroy()
    {
        AppManager.Instance.EventAgregator.RemoveHandler<StartGameEventMessage>(this);
        AppManager.Instance.EventAgregator.RemoveHandler<RemoveHeartEventMessage>(this);
        AppManager.Instance.EventAgregator.RemoveHandler<LevelDoneEventMessage>(this);
        AppManager.Instance.EventAgregator.RemoveHandler<EndGameEventMessage>(this);
    }

    #region Handles
    public void Handle(StartGameEventMessage message)
    {
        StartGame(message.StartLevelIdx);
    }
    public void Handle(RemoveHeartEventMessage message)
    {
        for(int i = _heartImages.Count - 1; i > -1; i--)
        {
            if (_heartImages[i].fillAmount > 0)
            {
                HeartFillEffect(_heartImages[i], false, _heartFillTime);
                break;
            }
        }
    }
    public void Handle(LevelDoneEventMessage message)
    {
        ShowLogoPanel(true, false);
        ShowLogoText(true, true, _levelDoneText.Replace("%1", LevelInPlayManager.Instance.CurrentLevelIdx.ToString("00")));
      
        Invoke("StartNextLevel", 1f);
    }

    public void StartNextLevel()
    {
        LevelInPlayManager.Instance.CurrentLevelIdx++;
        AppManager.Instance.EventAgregator.SendMessageAll(new GenerateLevelEventMessage(LevelInPlayManager.Instance.CurrentLevelIdx));
    }


    public void Handle(EndGameEventMessage message)
    {
        ShowLogoPanel(true, true);
        ShowLogoText(true, false, _loseText);
    }

    public void Handle(LevelReadyEventMessage message)
    {
        ShowLogoPanel(false, false);
    }
    #endregion

    public void StartGame(int startLevelIdx)
    {
        ShowLogoPanel(false, false);
        ResetHeartsEffect();
    }

    #region Manage Logo panel & etc
    public void ShowLogoPanel(bool showPanel, bool showStartButton, bool withEffects = true)
    {
        if (!withEffects)
        {
            LogoPanel.SetActive(showPanel);
            StartButton.SetActive(showStartButton);
            return;
        }

        if (showPanel)
        {
            LogoPanelImage.fillAmount = 0;
            LogoPanelImage.DOFillAmount(1, _logoPanelTimeEffect).OnComplete(
                delegate
                {
                    LogoPanel.SetActive(true);
                });

            foreach (var curPart in LogoParts)
            {
                curPart.DOColor(Color.white, _logoPanelTimeEffect / 2f).SetDelay(_logoPanelTimeEffect / 2f);
            }

            StartButton.SetActive(showStartButton);
        }
        else
        {
            StartButton.SetActive(showStartButton);
            ShowLogoText(false);

            LogoPanelImage.fillAmount = 1;
            LogoPanelImage.DOFillAmount(0, _logoPanelTimeEffect).OnComplete(
                delegate
                {
                    LogoPanel.SetActive(false);
                });

            foreach (var curPart in LogoParts)
            {
                curPart.DOColor(Color.clear, _logoPanelTimeEffect / 2f).SetDelay(_logoPanelTimeEffect / 2f);
            }
        }

    }

    public void ShowLogoText(bool showText, bool showGoodOrBad = true, string text = "")
    {
        if (showText)
        {
            LogoInfoTextGood.gameObject.SetActive(showGoodOrBad);
            LogoInfoTextBad.gameObject.SetActive(!showGoodOrBad);
            if (showGoodOrBad)
                LogoInfoTextGood.text = text;
            else
                LogoInfoTextBad.text = text;
        }
        else
        {
            LogoInfoTextGood.gameObject.SetActive(false);
            LogoInfoTextBad.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Heart's effects

    public void ResetHeartsEffect()
    {
        int heartCount = 0;
        foreach (var curHeart in _heartImages)
        {
            curHeart.gameObject.SetActive(true);
            HeartFillEffect(curHeart, true, _heartFillTime * heartCount + 0.5f);
            heartCount++;
        }
        HeartBigEffect(_heartFillTime * heartCount + 0.5f);
    }

    public void HeartFillEffect(Image heart, bool fillOrEmpty, float delay)
    {
        heart.fillAmount = fillOrEmpty ? 0 : 1;
        heart.DOFillAmount(fillOrEmpty ? 1 : 0, _heartFillTime).SetDelay(delay);
    }

    public void HeartBigEffect(float delay)
    {
        foreach (var curHeart in _heartImages)
        {
            curHeart.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), _heartFillTime).SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad).SetDelay(delay);
        }
    }

    #endregion


}

