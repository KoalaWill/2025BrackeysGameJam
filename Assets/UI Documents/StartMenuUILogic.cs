using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuUILogic : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement shader;
    private Button startButton;
    private UnityEngine.UIElements.Label startIndicator;
    private Button otherOptionsButton;
    private VisualElement buttonSet;
    private Button historyButton;
    private Button settingsButton;
    private Button quitButton;

    private bool otherOptionsButtonOnHover = false;
    private bool buttonSetOnHover = false;

    // Start is called before the first frame update
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        shader = root.Q<VisualElement>("shader");
        startButton = root.Q<Button>("startButton");
        startIndicator = root.Q<UnityEngine.UIElements.Label>("startIndicator");
        otherOptionsButton = root.Q<Button>("otherOptionsButton");
        buttonSet = root.Q<VisualElement>("buttonSet");
        historyButton = root.Q<Button>("historyButton");
        settingsButton = root.Q<Button>("settingsButton");
        quitButton = root.Q<Button>("quitButton");

        startButton.clicked += startButtonPressed;
        otherOptionsButton.RegisterCallback<MouseEnterEvent>(evt => otherOptionsButtonOnHoverEnter());
        otherOptionsButton.RegisterCallback<MouseLeaveEvent>(evt => otherOptionsButtonOnHoverExit());
        buttonSet.RegisterCallback<MouseEnterEvent>(evt => buttonSetOnHoverEnter());
        buttonSet.RegisterCallback<MouseLeaveEvent>(evt => buttonSetOnHoverExit());
        historyButton.clicked += historyButtonPressed;
        settingsButton.clicked += settingsButtonPressed;
        quitButton.clicked += quitButtonPressed;

        startIndicatorGlow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startButtonPressed()
    {
        Debug.Log("startButtonPressed");

        GameManager.instance.ChangeState(GameManager.GameState.Playing);
        LoadingUILogic.instance.addScenesToLaod("GameScene1"); //scene 1
        LoadingUILogic.instance.loadScenes();

        HistoryUILogic.instance.createStopwatch();
        HistoryUILogic.instance.plusTotalTime();
        HistoryUILogic.instance.stopwatch.start();
    }

    async void startIndicatorGlow()
    {
        while(true)
        {
            startIndicator.AddToClassList("startIndicatorOn");
            await Task.Delay(1000);
            startIndicator.RemoveFromClassList("startIndicatorOn");
            await Task.Delay(1000);
        }
    }    

    void otherOptionsButtonOnHoverEnter()
    {
        otherOptionsOpen();
        otherOptionsButtonOnHover = true;
    }

    void otherOptionsButtonOnHoverExit()
    {
        otherOptionsButtonOnHover = false;
        otherOptionsClose();
    }

    void buttonSetOnHoverEnter()
    {
        otherOptionsOpen();
        otherOptionsButtonOnHover = true;
    }

    void buttonSetOnHoverExit()
    {
        otherOptionsButtonOnHover = false;
        otherOptionsClose();
    }

    void otherOptionsOpen()
    {
        if(!otherOptionsButtonOnHover && !buttonSetOnHover)
        {
            otherOptionsButton.AddToClassList("otherOptionsButton-style-onHover");
            buttonSet.RemoveFromClassList("buttonSetDown");
        }
    }

    void otherOptionsClose()
    {
        if(!otherOptionsButtonOnHover && !buttonSetOnHover)
        otherOptionsButton.RemoveFromClassList("otherOptionsButton-style-onHover");
        buttonSet.AddToClassList("buttonSetDown");
    }

    void historyButtonPressed()
    {
        Debug.Log("historyButtonPressed");
        HistoryUILogic.instance.uiDocument.enabled = true;
    }

    void settingsButtonPressed()
    {
        Debug.Log("settingsButtonPressed");
        SettingsUILogic.instance.uiDocument.enabled = true;
    }

    void quitButtonPressed()
    {
        Debug.Log("quitButtonPressed");
    }
}
