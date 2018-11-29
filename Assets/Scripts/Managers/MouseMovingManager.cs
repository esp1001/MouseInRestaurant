using UnityEngine;

public class MouseMovingManager : MonoBehaviour, IHandler<LevelReadyEventMessage>, IHandler<LevelDoneEventMessage>
{
    public static MouseMovingManager Instance { get; set; }

    private readonly float _delayBeforeStart;
    private bool _isMovingStarted;


    private float _timer;

    void Awake ()
	{
	    Instance = this;
        Init();
	}

    public void Init()
    {
        AppManager.Instance.EventAgregator.AddHandler<LevelReadyEventMessage>(this);
        AppManager.Instance.EventAgregator.AddHandler<LevelDoneEventMessage>(this);
    }

    private void OnDestroy()
    {
        AppManager.Instance.EventAgregator.RemoveHandler<LevelReadyEventMessage>(this);
        AppManager.Instance.EventAgregator.RemoveHandler<LevelDoneEventMessage>(this);
    }
    void Update()
    {
        if(!_isMovingStarted || LevelInPlayManager.Instance == null)
            return;

        _timer += Time.deltaTime;
        if (_timer > LevelInPlayManager.Instance.MouseFrequency.y)
        {
            _timer = 0;
            GenerateMouses(Mathf.RoundToInt(DatabaseManager.Instance.GetMouseFrequencyOfAppearance(LevelInPlayManager.Instance.CurrentLevelIdx).y));
        }
    }

    public void Handle(LevelReadyEventMessage message)
    {
        StartMouseMoving();
    }
    public void Handle(LevelDoneEventMessage message)
    {
        StopMouseMoving();
    }

    private void StartMouseMoving()
    {
        _isMovingStarted = true;
    }

    private void StopMouseMoving()
    {
        _isMovingStarted = false;
    }

    public void GenerateMouses(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //LevelInPlayManager.Instance.MouseCounter++;
            //if (LevelInPlayManager.Instance.MouseCounter > LevelInPlayManager.Instance.MouseOnLevel)
            //    continue;

            var curMouseInfo = ChooseMouseType(LevelInPlayManager.Instance.CurrentLevelIdx);

            if (curMouseInfo.Prefab == null)
                return;

            var curMouseGO = Instantiate(curMouseInfo.Prefab, new Vector3(9999,9999), Quaternion.identity);
            //curMouseGO.transform.position = Vector3.zero;
            curMouseGO.transform.localScale = curMouseInfo.Scale;

            var curMouseBezierFollow = curMouseGO.GetComponent<BezierFollow>();
            if (curMouseBezierFollow == null)
                return;

            curMouseBezierFollow.MouseInfo = curMouseInfo;

            LevelInPlayManager.Instance.AddMouse(curMouseBezierFollow);

            ChooseRoute(curMouseBezierFollow);
        }
    }

    private MouseInfo ChooseMouseType(int levelIdx)
    {
        var result = Random.Range(0, 100);
        MouseTypes choosedMouseType;

        if (result < LevelInPlayManager.Instance.MouseProbabilityOfAppearance[1])
            choosedMouseType= MouseTypes.MouseTypeA;
        else if (result < LevelInPlayManager.Instance.MouseProbabilityOfAppearance[1] + LevelInPlayManager.Instance.MouseProbabilityOfAppearance[2])
            choosedMouseType = MouseTypes.MouseTypeB;
        else
            choosedMouseType = MouseTypes.MouseTypeC;

        return DatabaseManager.Instance.GetMouseInfoByMouseType(choosedMouseType);
    }

    private void ChooseRoute(BezierFollow mouse)
    {
        var choosedRoute = LevelInPlayManager.Instance.GetRouteGOByIdx(Random.Range(0, LevelInPlayManager.Instance.RoutesCount));
        mouse.gameObject.transform.SetParent(choosedRoute.transform);
        mouse.IsStartSideIsStart = Random.Range(0, 100) < 50;
        var allSubRoutes = choosedRoute.GetComponentsInChildren<Route>();
        foreach (var curSubRoute in allSubRoutes)
        {
            mouse.Routes.Add(curSubRoute.transform);
        }
    }


}
