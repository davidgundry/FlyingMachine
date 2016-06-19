using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Launcher : MonoBehaviour {

    public bool IntroductionDone { get; set; }
    private float StartTime { get; set; }
    private bool StartedLoad { get; set; }

    void Start()
    {
        DontDestroyOnLoad(this);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        StartTime = Time.time;
    }


    void Update()
    {
        if ((Time.time > StartTime + 2f) && (!StartedLoad))
        {
            StartCoroutine("loadLevel");
            StartedLoad = true;
        }
    }

    IEnumerator loadLevel()
    {
        
        AsyncOperation async = Application.LoadLevelAsync("intro");
        async.allowSceneActivation = true;
        while (!async.isDone)
        {
            //int progress = (int)(async.progress * 100);
      
            yield return (0);
        }
        
        yield return 0;
    }


}
