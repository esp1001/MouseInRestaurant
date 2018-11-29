using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LogoEffect : MonoBehaviour
{
    private Tween _logoEffectTween;

	void OnEnable()
	{
	    MouseTailMoving();
	}

    void OnDisable()
    {
        _logoEffectTween.Kill();
    }

    void MouseTailMoving()
    {
        _logoEffectTween = transform.DORotate(new Vector3(0, 0, -110), 2).SetDelay(1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

}
