using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void StartGameButton()
    {
        AppManager.Instance.EventAgregator.SendMessageAll(new StartGameEventMessage(DatabaseManager.Instance.StartLevelIdx));
    }

}
