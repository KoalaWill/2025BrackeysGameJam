using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class HistoryUILogic : MonoBehaviour
{
    public static HistoryUILogic instance;

    public bool showStopWatch = false;
    public bool resetPlayerPrefsToggle = false;
    private bool eventsSubscribed = false;

    public UIDocument uiDocument;
    private VisualElement shader;
    private Button closeButton;
    private Label personalBestLable;
    private Label completedTimesLable;
    private Label averageLable;

    private string personalBestTime;
    private int completedTimes;
    private string averageTime;

    public stopwatchFunc stopwatch = new stopwatchFunc();
    public TimeSpan totalTime;
    public TimeSpan savedTime;
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        getPlayerPrefs();
    }

    // Start is called before the first frame update
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();

        eventsSubscribed = false;
        uiDocument.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && uiDocument.enabled)
            escapePressed();
        if (uiDocument.enabled && !eventsSubscribed)
        {
            subscribeToEvents();
            shaderOpenAni();
            getPlayerPrefs();
            setPlayerPrefsValue();
        }
        else if (!uiDocument.enabled && eventsSubscribed)
            unSubscribeFromEvents();

        if (showStopWatch)
        {
            plusTotalTime();
        }

        if (resetPlayerPrefsToggle)
        {
            resetPlayerPrefs();
            resetPlayerPrefsToggle = false;
        }
    }

    void shaderOpenAni()
    {
        shader.RemoveFromClassList("shaderUp");
    }

    void shaderCloseAni()
    {
        shader.AddToClassList("shaderUp");
    }

    void subscribeToEvents()
    {
        UnityEngine.Debug.Log("EventsSubscribed");
        eventsSubscribed = true;

        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        shader = root.Q<VisualElement>("shader");
        closeButton = root.Q<Button>("closeButton");
        personalBestLable = root.Q<Label>("PersonalBestLabel");
        completedTimesLable = root.Q<Label>("CompletedTimesLabel");
        averageLable = root.Q<Label>("AverageLabel");

        closeButton.clicked += closeButtonPressed;
        setPlayerPrefsValue();
    }

    void unSubscribeFromEvents()
    {
        UnityEngine.Debug.Log("EventsUnSubscribed");
        eventsSubscribed = false;

        closeButton.clicked -= closeButtonPressed;
    }

    void getPlayerPrefs()
    {
        personalBestTime = PlayerPrefs.GetString("personalBestTime", "0");
        completedTimes = PlayerPrefs.GetInt("completedTimes", 0);
        averageTime = PlayerPrefs.GetString("averageTime", "0");
    }

    void setPlayerPrefsValue()
    {
        personalBestLable.text = personalBestTime == "0" ? "--:--:---" : formatTimeSpan(personalBestTime);
        completedTimesLable.text = completedTimes.ToString();
        averageLable.text = averageTime == "0" ? "--:--:---" :  formatTimeSpan(averageTime);
    }

    void resetPlayerPrefs()
    {
        PlayerPrefs.SetString("personalBestTime", "0");
        PlayerPrefs.SetInt("completedTimes", 0);
        PlayerPrefs.SetString("averageTime", "0");
    }

    async void escapePressed()
    {
        UnityEngine.Debug.Log("escapePressed");
        shaderCloseAni();
        await Task.Delay(500);
        uiDocument.enabled = false;
    }

    async void closeButtonPressed()
    {
        UnityEngine.Debug.Log("closeButtonPressed");
        shaderCloseAni();
        await Task.Delay(500);
        uiDocument.enabled = false;
    }

    public void createStopwatch()
    {
        totalTime= TimeSpan.Zero;
        savedTime = TimeSpan.FromMilliseconds(double.Parse(PlayerPrefs.GetString("savedTime", "0")));
    }

    public void saveStopwatchTime()
    {
        totalTime = stopwatch.elapsedTime + savedTime;
        PlayerPrefs.SetString("savedTime", (totalTime + savedTime).TotalMilliseconds.ToString());
    }

    public void resetSavedStopwatchTime()
    {
        PlayerPrefs.SetString("savedTime", "0");
        savedTime = TimeSpan.Zero;
    }

    public void plusTotalTime()
    {
        totalTime = stopwatch.elapsedTime + savedTime;
        //UnityEngine.Debug.Log(totalTime);
    }

    public static string formatTimeSpan(string totalSeconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(double.Parse(totalSeconds));
        return $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}:{timeSpan.Milliseconds:000}";
    }

    public void saveRecordAndResetStopWatch()
    {
        plusTotalTime();
        PlayerPrefs.SetInt("completedTimes", PlayerPrefs.GetInt("completedTimes") + 1);
        double sum = (totalTime + savedTime).TotalMilliseconds;
        if (sum < double.Parse(personalBestTime) || double.Parse(personalBestTime) == 0)
        {
            PlayerPrefs.SetString("personalBestTime", sum.ToString());
        }
        PlayerPrefs.SetString("averageTime", ((double.Parse(averageTime) * completedTimes + sum) / (completedTimes + 1)).ToString());

        //getPlayerPrefs();
        //setPlayerPrefsValue();
        stopwatch.reset();
    }
}

public class stopwatchFunc
{
    private Stopwatch stopwatch;
    private TimeSpan pausedTime;

    public stopwatchFunc()
    {
        stopwatch = new Stopwatch();
        pausedTime = TimeSpan.Zero;
    }
    public void start()
    {
        if (!stopwatch.IsRunning)
            stopwatch.Start();
    }

    public void pause()
    {
        if (stopwatch.IsRunning)
        {
            stopwatch.Stop();
            //pausedTime = stopwatch.Elapsed;
        }
    }

    public void resume()
    {
        if (!stopwatch.IsRunning)
        {
            stopwatch.Start();
        }
    }

    public void reset()
    {
        stopwatch.Reset();
        stopwatch.Stop();
        pausedTime = TimeSpan.Zero;
    }

    public TimeSpan elapsedTime
    {
        get
        {
            return stopwatch.Elapsed + pausedTime;
        }
    }
}
