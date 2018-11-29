﻿using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerateLevelManager : MonoBehaviour, 
    IHandler<StartGameEventMessage>, 
    IHandler<GenerateLevelEventMessage>, 
    IHandler<AddOneMouseEventMessage>
{
    public static GenerateLevelManager Instance { get; private set; }

    [Header("Level GameObject's")] [SerializeField] private GameObject _floorGO;
    [SerializeField] private GameObject _staticObjectsDockGO;
    [SerializeField] private GameObject _routesDockGO;

    [SerializeField] private TextMeshProUGUI _levelTitleText;
    [SerializeField] private TextMeshProUGUI _curLevelIdxText;
    [SerializeField] private TextMeshProUGUI _nextLevelIdxText;
    [SerializeField] private Image _mouseTappedProgressImage;
    [SerializeField] private TextMeshProUGUI _mouseTappedProgressText;

    void Awake()
    {
        Instance = this;
        Init();
    }

    public void Init()
    {
        AppManager.Instance.EventAgregator.AddHandler<StartGameEventMessage>(this);
        AppManager.Instance.EventAgregator.AddHandler<GenerateLevelEventMessage>(this);
        AppManager.Instance.EventAgregator.AddHandler<AddOneMouseEventMessage>(this);
    }

    private void OnDestroy()
    {
        AppManager.Instance.EventAgregator.RemoveHandler<StartGameEventMessage>(this);
        AppManager.Instance.EventAgregator.RemoveHandler<GenerateLevelEventMessage>(this);
        AppManager.Instance.EventAgregator.RemoveHandler<AddOneMouseEventMessage>(this);
    }

    #region Handlers
    public void Handle(StartGameEventMessage message)
    {
        LevelInPlayManager.Instance.CurrentLevelIdx = 1;
        GenerateLevel(message.StartLevelIdx);
    }
    public void Handle(GenerateLevelEventMessage message)
    {
        GenerateLevel(message.LevelIdx);
    }

    public void Handle(AddOneMouseEventMessage message)
    {
        UpdateTappedMouses();    
    }

    #endregion

    public void GenerateLevel(int levelIdx)
    {
        LevelInPlayManager.Instance.Init(
            DatabaseManager.Instance.GetLevelInfoByIdx(DatabaseManager.Instance.IsAllowedVariousLevels
                ? levelIdx
                : 1));

        PrepareLevelProgressPanel(LevelInPlayManager.Instance.LevelInfo.Idx);
        AddingRoutes();
        AddingStaticObjects();

        AppManager.Instance.EventAgregator.SendMessageAll(new LevelReadyEventMessage());
    }

    private void PrepareLevelProgressPanel(int curLevelIdx)
    {
        _levelTitleText.text = "Level " + curLevelIdx.ToString("00");
        _curLevelIdxText.text = curLevelIdx.ToString();
        _nextLevelIdxText.text = (curLevelIdx + 1).ToString();
        _mouseTappedProgressText.text = "0 / " + LevelInPlayManager.Instance.MouseOnLevel;
        _mouseTappedProgressImage.fillAmount = 0;
    }

    public void AddingRoutes()
    {
        foreach (var curRoute in LevelInPlayManager.Instance.LevelInfo.Routes)
        {
            var curRouteGO = Instantiate(curRoute, _routesDockGO.transform);
            curRouteGO.transform.localPosition = Vector3.zero;
            curRouteGO.transform.localScale = Vector3.one;
            LevelInPlayManager.Instance.AddRoute(curRouteGO);
        }
    }

    public void AddingStaticObjects()
    {
        foreach (var curStaticObject in LevelInPlayManager.Instance.LevelInfo.StaticObjects)
        {
            var curStaticObjectGO = Instantiate( DatabaseManager.Instance.GetStaticObjectGOByIdx(curStaticObject.StaticObjectPrefabIdx), _staticObjectsDockGO.transform);
            curStaticObjectGO.transform.localPosition = curStaticObject.CoordinatesToInstantiate;
            curStaticObjectGO.transform.localScale = Vector3.one;
            LevelInPlayManager.Instance.AddStaticObject(curStaticObjectGO);
        }
    }

    void UpdateTappedMouses()
    {
        _mouseTappedProgressText.text = LevelInPlayManager.Instance.MouseTappedCount + " / " + LevelInPlayManager.Instance.MouseOnLevel;
        _mouseTappedProgressImage.fillAmount = LevelInPlayManager.Instance.MouseTappedCount / (float)LevelInPlayManager.Instance.MouseOnLevel;
    }

}