using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour {

    public static AppManager Instance { get; private set; }

    public IEventAgregator EventAgregator;

    void Awake()
    {
        EventAgregator = new EventAgregator();
        Instance = this;
    }

}
