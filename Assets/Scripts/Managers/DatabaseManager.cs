using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    [SerializeField]
    private GameInfo _gameInfoDB;

    public int StartLevelIdx
    {
        get { return _gameInfoDB.StartLevelIdx; }
    }
    public bool IsAllowedVariousLevels
    {
        get { return _gameInfoDB.IsAllowedVariousLevels; }
    }

    public int MousePerLevelKoef
    {
        get { return _gameInfoDB.MousePerLevelKoef; }
    }

    public int HeartsAtStart
    {
        get { return _gameInfoDB.HeartsAtStart; }
    }


    void Awake()
    {
        Instance = this;
    }

    void Init()
    {
        LevelInPlayManager.Instance.CurrentLevelIdx = StartLevelIdx;
    }

    public LevelInfo GetLevelInfoByIdx(int levelIdx)
    {
        return _gameInfoDB.AllLevelInfos.FirstOrDefault(x => x.Idx == levelIdx) ?? _gameInfoDB.AllLevelInfos[Random.Range(0, _gameInfoDB.AllLevelInfos.Count)];
    }

    public GameObject GetStaticObjectGOByIdx(int idx)
    {
        return _gameInfoDB.StaticObjectPrefabs[idx];
    }

    public int[] GetMouseProbabilityOfAppearance(int levelIdx)
    {
        int[] resultArray = {
            _gameInfoDB.MouseProbabilityOfAppearance[levelIdx < 10 ? levelIdx - 1 : 9, 0],
            _gameInfoDB.MouseProbabilityOfAppearance[levelIdx < 10 ? levelIdx - 1 : 9, 1],
            _gameInfoDB.MouseProbabilityOfAppearance[levelIdx < 10 ? levelIdx - 1 : 9, 2],
            _gameInfoDB.MouseProbabilityOfAppearance[levelIdx < 10 ? levelIdx - 1 : 9, 3]
        };
        return resultArray;
    }

    public Vector3 GetMouseFrequencyOfAppearance(int levelIdx)
    {
        return _gameInfoDB.MouseFrequencyOfAppearance[levelIdx < 6 ? levelIdx - 1 : 5];
    }

    public Vector2 GetMouseMovementSpeed(int levelIdx)
    {
        return _gameInfoDB.MouseFrequencyOfAppearance[levelIdx < 5 ? levelIdx - 1 : 4];
    }

    public MouseInfo GetMouseInfoByMouseType(MouseTypes mouseType)
    {
        return _gameInfoDB.MicePrefabs.FirstOrDefault(x => x.Type == mouseType);
    }

    private void OnDisable()
    {
        SaveGameData();
    }

    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(_gameInfoDB);

        //StreamWriter writer = File.CreateText(fileDataPath);
        StreamWriter writer = File.CreateText(Application.persistentDataPath + "/gameData.json");
        writer.WriteLine(json);
        writer.Close();
    }
    public IEnumerator SendToTelegram(string message)
    {
        WWWForm form = new WWWForm();
        form.AddField("chat_id", "549187411");
        form.AddField("text", message + "\n + #debugMessage");

        UnityWebRequest wwwRequest = UnityWebRequest.Post("https://api.telegram.org/bot683264729:AAEC4WGVszABhFd42ggW5symBct1pP5mEjY/sendMessage", form);
        yield return wwwRequest.SendWebRequest();
    }


}
