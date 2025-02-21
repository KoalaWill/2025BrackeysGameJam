using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class TimerUILogic : MonoBehaviour
{
    public static TimerUILogic instance;

    private bool eventsSubscribed;

    public UIDocument uiDocument;
    private VisualElement shader;
    private Label timerLabel;
    private Label personalBestTimeLabel;

    // Start is called before the first frame update
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();

        if (SettingsUILogic.instance.timerDisplayEnabled)
            uiDocument.enabled = true;
        else
            uiDocument.enabled = false;

        eventsSubscribed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SettingsUILogic.instance.timerDisplayEnabled && !eventsSubscribed)
        {
            uiDocument.enabled = true;
            subscribeToEvents();
        }
        else if (!SettingsUILogic.instance.timerDisplayEnabled && eventsSubscribed)
        {
            uiDocument.enabled = false;
            unSubscribeFromEvents();
        }
        if (SettingsUILogic.instance.timerDisplayEnabled)
            displayTimer();
    }
    
    void subscribeToEvents()
    {
        Debug.Log("EventsSubscribed");
        eventsSubscribed = true;

        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        shader = root.Q<VisualElement>("shader");
        timerLabel = root.Q<Label>("timerLabel");
        personalBestTimeLabel = root.Q<Label>("personalBestTimeLabel");
    }

    void unSubscribeFromEvents()
    {
        Debug.Log("EventsUnSubscribed");
        eventsSubscribed = false;
    }    

    void displayTimer()
    {
        HistoryUILogic.instance.plusTotalTime();
        timerLabel.text = HistoryUILogic.formatTimeSpan(HistoryUILogic.instance.totalTime.TotalSeconds.ToString());
    }
}
