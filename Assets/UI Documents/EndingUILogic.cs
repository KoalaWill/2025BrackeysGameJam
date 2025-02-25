using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class EndingUILogic : MonoBehaviour
{
    public static EndingUILogic instance;

    private string timeText;
    private bool aniFinished = false;
    public bool test = false;

    public UIDocument uiDocument;
    private VisualElement shader;
    private Label timeLabel;
    private Label newRecordLabel;
    private Button backToMenuButton;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            onGameOver();
            test = false;
        }
    }

    void subscribeToEvents()
    {
        Debug.Log("EventsSubscribed");

        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        shader = root.Q<VisualElement>("shader");
        timeLabel = root.Q<Label>("timeLabel");
        backToMenuButton = root.Q<Button>("backToMenuButton");

        backToMenuButton.clicked += backToMenuButtonPressed;
    }

    void unSubscribeFromEvents()
    {
        Debug.Log("EventsUnsubscribed");

        backToMenuButton.clicked -= backToMenuButtonPressed;
    }

    void shaderOpenAni()
    {
        shader.RemoveFromClassList("endingShaderOff");
    }
    
    void shaderCloseAni()
    {
        shader.AddToClassList("endingShaderOff");
    }

    void backToMenuButtonPressed()
    {
        Debug.Log("backToMenuButtonPressed");

        shaderCloseAni();
        unSubscribeFromEvents();
        uiDocument.enabled = false;

        GameManager.instance.ChangeState(GameManager.GameState.MainMenu);
        HistoryUILogic.instance.stopwatch.reset();
        HistoryUILogic.instance.plusTotalTime();
        HistoryUILogic.instance.saveStopwatchTime();

        LoadingUILogic.instance.addScenesToLaod("UITest - startScene");
        LoadingUILogic.instance.loadScenes();
    }

    public async void onGameOver()
    {
        aniFinished = false;
        GameManager.instance.ChangeState(GameManager.GameState.GameOver);
        timeText = HistoryUILogic.formatTimeSpan(HistoryUILogic.instance.totalTime.TotalSeconds.ToString());
        HistoryUILogic.instance.saveRecordAndResetStopWatch();

        uiDocument.enabled = true;
        subscribeToEvents();
        timeLabel.text = timeText;

        await Task.Delay(1); //idk why
        shaderOpenAni();
        await Task.Delay(5000);
        aniFinished = true;
    }
}
