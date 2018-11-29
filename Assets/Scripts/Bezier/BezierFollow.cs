using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{

    [SerializeField]
    public List<Transform> Routes;

    private int _routeToGo;

    private float _tParam;


    private Vector2 _shipPosition;

    private float _speedModifier;

    private bool _coroutineAllowed;

    public MouseInfo MouseInfo;

    public bool IsStartSideIsStart;

    void Start()
    {
        _routeToGo = IsStartSideIsStart ? 0 : Routes.Count - 1;
        _tParam = 0f;
        _speedModifier = 0.1f;
        _coroutineAllowed = true;
    }

    void Update()
    {
        if (_coroutineAllowed)
            StartCoroutine(GoByTheRoute(_routeToGo));
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        _coroutineAllowed = false;


        Vector2 p0 = Routes[routeNumber].GetChild(IsStartSideIsStart ? 0 : 3).position;
        Vector2 p1 = Routes[routeNumber].GetChild(IsStartSideIsStart ? 1 : 2).position;
        Vector2 p2 = Routes[routeNumber].GetChild(IsStartSideIsStart ? 2 : 1).position;
        Vector2 p3 = Routes[routeNumber].GetChild(IsStartSideIsStart ? 3 : 0).position;

        while (_tParam < 1)
        {
            _tParam += Time.deltaTime * _speedModifier;

            _shipPosition = Mathf.Pow(1 - _tParam, 3) * p0
                            + 3 * Mathf.Pow(1 - _tParam, 2) * _tParam * p1
                            + 3 * (1 - _tParam) * Mathf.Pow(_tParam, 2) * p2
                            + Mathf.Pow(_tParam, 3) * p3;

            transform.position = _shipPosition;



            var _tParam2 = _tParam + 0.1f;

            var _shipPositionLookAt = Mathf.Pow(1 - _tParam2, 3) * p0
                            + 3 * Mathf.Pow(1 - _tParam2, 2) * _tParam2 * p1
                            + 3 * (1 - _tParam2) * Mathf.Pow(_tParam2, 2) * p2
                            + Mathf.Pow(_tParam2, 3) * p3;

            transform.right = new Vector3(_shipPositionLookAt.x, _shipPositionLookAt.y, 0) - transform.position;

            yield return new WaitForEndOfFrame();
        }

        _tParam = 0f;

        if(IsStartSideIsStart)
            _routeToGo += 1;
        else
            _routeToGo -= 1;

        //if (_routeToGo > Routes.Count - 1)
        //_routeToGo = 0;

        if ((IsStartSideIsStart && _routeToGo < Routes.Count) || (!IsStartSideIsStart && _routeToGo >= 0))
            _coroutineAllowed = true;
        else if (MouseInfo.IsOrdinary)
        {
            AppManager.Instance.EventAgregator.SendMessageAll(new RemoveHeartEventMessage());
        }
    }

    public void OnMouseClick()
    {
        var allMousePartsSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        if (MouseInfo.IsOrdinary)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            foreach (var curSpriteRenderer in allMousePartsSpriteRenderers)
            {
                curSpriteRenderer.DOColor(Color.clear, 0.5f).OnComplete(delegate {gameObject.SetActive(false);});
                curSpriteRenderer.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
            }
            AppManager.Instance.EventAgregator.SendMessageAll(new AddOneMouseEventMessage());
        }
        else
        {
            foreach (var curSpriteRenderer in allMousePartsSpriteRenderers)
            {
                curSpriteRenderer.DOColor(Color.red, 0.3f).SetLoops(2, LoopType.Yoyo);
                curSpriteRenderer.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f).SetLoops(2, LoopType.Yoyo); 
            }
            AppManager.Instance.EventAgregator.SendMessageAll(new RemoveHeartEventMessage());
        }
    }
}
