﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{

    [SerializeField]
    private Transform[] _controlPoints;

    private Vector2 _gizmosPosition;

    [SerializeField]
    private Color _color;

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            _gizmosPosition = Mathf.Pow(1 - t, 3) * _controlPoints[0].position
                              + 3 * Mathf.Pow(1 - t, 2) * t * _controlPoints[1].position
                              + 3 * (1 - t) * Mathf.Pow(t, 2) * _controlPoints[2].position
                              + Mathf.Pow(t, 3) * _controlPoints[3].position;

            Gizmos.color = _color;
            Gizmos.DrawSphere(_gizmosPosition, 0.1f);
        }

        Gizmos.DrawLine(new Vector2(_controlPoints[0].position.x, _controlPoints[0].position.y),
            new Vector2(_controlPoints[1].position.x, _controlPoints[1].position.y));

        Gizmos.DrawLine(new Vector2(_controlPoints[2].position.x, _controlPoints[2].position.y),
            new Vector2(_controlPoints[3].position.x, _controlPoints[3].position.y));
    }

}
