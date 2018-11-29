using System.Collections.Generic;
using UnityEngine;

public class LevelInPlayManager : MonoBehaviour, IHandler<StartGameEventMessage>, IHandler<RemoveHeartEventMessage> {

    public static LevelInPlayManager Instance { get; private set; }

    public LevelInfo LevelInfo;

    public int CurrentLevelIdx;

    public int MouseOnLevel
    {
        get { return CurrentLevelIdx * DatabaseManager.Instance.MousePerLevelKoef; }
    }

    private int _mouseTappedCount;
    public int MouseTappedCount
    {
        get { return _mouseTappedCount; }
        set
        {
            if (value == MouseOnLevel)
                AppManager.Instance.EventAgregator.SendMessageAll(new LevelDoneEventMessage());

            _mouseTappedCount = value;
        }

    }

    public int[] MouseProbabilityOfAppearance
    {
        get { return DatabaseManager.Instance.GetMouseProbabilityOfAppearance(CurrentLevelIdx); }
    }
    public Vector3 MouseFrequency
    {
        get { return DatabaseManager.Instance.GetMouseFrequencyOfAppearance(CurrentLevelIdx); }
    }
    public Vector2 MouseSpeed
    {
        get { return DatabaseManager.Instance.GetMouseMovementSpeed(CurrentLevelIdx); }
    }

    public int HeartsNow
    {
        get { return _heartsNow; }
        set
        {
            _heartsNow = value;
            if (_heartsNow == 0)
            {
                AppManager.Instance.EventAgregator.SendMessageAll(new EndGameEventMessage());
            }
        }
    }

    private int _heartsNow;

    [Header("Object bank's")]
    [SerializeField]
    private List<GameObject> _allStaticObjects;
    [SerializeField]
    private List<BezierFollow> _allMouses;
    [SerializeField]
    private List<GameObject> _allRoutes;

    public int RoutesCount
    {
        get { return _allRoutes.Count; }
    }
    
    void Awake()
    {
        Instance = this;
        Init();
    }

    public void Init(LevelInfo levelInfo = null)
    {
        LevelInfo = levelInfo;
        _mouseTappedCount = 0;
        AppManager.Instance.EventAgregator.AddHandler<StartGameEventMessage>(this);
        AppManager.Instance.EventAgregator.AddHandler<RemoveHeartEventMessage>(this);
    }

    private void OnDestroy()
    {
        AppManager.Instance.EventAgregator.RemoveHandler<StartGameEventMessage>(this);
        AppManager.Instance.EventAgregator.RemoveHandler<RemoveHeartEventMessage>(this);
    }


    public void AddRoute(GameObject route)
    {
        _allRoutes.Add(route);
    }
    public void AddStaticObject(GameObject staticObject)
    {
        _allStaticObjects.Add(staticObject);
    }
    public void AddMouse(BezierFollow mouse)
    {
        _allMouses.Add(mouse);
    }

    public GameObject GetRouteGOByIdx(int idx)
    {
        return _allRoutes[idx];
    }

    public void Handle(StartGameEventMessage message)
    {
        HeartsNow = DatabaseManager.Instance.HeartsAtStart;
    }

    public void Handle(RemoveHeartEventMessage message)
    {
        HeartsNow--;
    }

}
