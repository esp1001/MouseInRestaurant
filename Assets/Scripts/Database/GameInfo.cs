using System;
using UnityEngine;
using System.Collections.Generic;

public enum MouseTypes
{
    MouseTypeA = 0,
    MouseTypeB = 1,
    MouseTypeC = 2
}

[Serializable]
public class MouseInfo
{
    public MouseTypes Type;
    public bool IsOrdinary;
    public GameObject Prefab;
    public Vector3 Scale;
}

[Serializable]
public class StaticObject
{
    public int StaticObjectPrefabIdx;
    public Vector2 CoordinatesToInstantiate;
}

[Serializable]
public class LevelInfo
{
    public string LevelName;
    public int Idx;
    public Sprite FloorTexture;
    public List<StaticObject> StaticObjects;
    public List<GameObject> Routes;
}

[CreateAssetMenu(fileName = "GameInfoDB", menuName = "Mouse/Create GameInfo DB")]
public class GameInfo : ScriptableObject
{

    [Header("Variables")]
    public int StartLevelIdx;
    public bool IsAllowedVariousLevels;

    [Header("All levels info")]
    public List<LevelInfo> AllLevelInfos;

    [Header("Static objects prefabs ")]
    public List<GameObject> StaticObjectPrefabs;

    [Header("Mice prefabs ")]
    public List<MouseInfo> MicePrefabs;

    [Header("Mouse per level koef")]
    public int MousePerLevelKoef;

    [Header("Hearts at Start")]
    public readonly int HeartsAtStart = 3;

    [SerializeField]
    public int[,] MouseProbabilityOfAppearance = new int[10, 4]
    {
        { 1, 40, 40, 20},
        { 2, 45, 45, 10},
        { 3, 40, 40, 20},
        { 4, 35, 35, 30},
        { 5, 33, 33, 34},
        { 6, 33, 33, 34},
        { 7, 33, 33, 34},
        { 8, 33, 33, 34},
        { 9, 33, 33, 34},
        {10, 50, 50, 34}
    };

    public Vector3[] MouseFrequencyOfAppearance = new Vector3[6]
    {
        new Vector3(1, 1, 1.5f),
        new Vector3(2, 1, 1),
        new Vector3(3, 1, 0.7f),
        new Vector3(4, 2, 1.3f),
        new Vector3(5, 2, 1),
        new Vector3(6, 3, 2)
    };

    public Vector2[] MouseMovementSpeed = new Vector2[5]
    {
        new Vector2(1, 1),
        new Vector2(2, 1.3f),
        new Vector2(3, 1.6f),
        new Vector2(4, 1.9f),
        new Vector2(5, 2.2f)
    };
}
