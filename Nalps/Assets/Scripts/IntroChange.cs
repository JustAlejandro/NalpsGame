using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroChange : MonoBehaviour
{
    public string SceneName;
    public WordySpeak ws;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ws.exitDia()) {
            SceneManager.LoadScene(SceneName);
        }
    }
}
