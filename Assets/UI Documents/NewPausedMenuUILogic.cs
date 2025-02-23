using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class NewPausedMenuUILogic : MonoBehaviour
{
    //private bool eventsSubscribed;

    private UIDocument uiDocument;
    private VisualElement shader;
    private Button resumeButton;
    private VisualElement pausedIcon;
    private UnityEngine.UIElements.Label pausedLabel;
    private UnityEngine.UIElements.Label resumeIndicator;
    private Button otherOptionsButton;
    private VisualElement buttonSet;
    private Button settingsButton;
    private Button quitToMenuButton;
    private Button quitToDesktopButton;

    private bool otherOptionsButtonOnHover;
    private bool buttonSetOnHover;

    // Start is called before the first frame update
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        resumeIndicator = root.Q<UnityEngine.UIElements.Label>("resumeIndicator");

        //eventsSubscribed = false;
        uiDocument.enabled = false;
        otherOptionsButtonOnHover = false;
        buttonSetOnHover = false;

        resumeIndicatorGlow();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapePressed();
        }
    }

    void subscribeToEvents()
    {
        //eventsSubscribed = true;

        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        shader = root.Q<VisualElement>("shader");
        resumeButton = root.Q<Button>("resumeButton");
        pausedIcon = root.Q<VisualElement>("pausedIcon");
        pausedLabel = root.Q<UnityEngine.UIElements.Label>("pausedLabel");
        resumeIndicator = root.Q<UnityEngine.UIElements.Label>("resumeIndicator");
        otherOptionsButton = root.Q<Button>("otherOptionsButton");
        buttonSet = root.Q<VisualElement>("buttonSet");
        settingsButton = root.Q<Button>("settingsButton");
        quitToMenuButton = root.Q<Button>("quitToMenuButton");
        quitToDesktopButton = root.Q<Button>("quitToDesktopButton");

        resumeButton.clicked += resumeButtonPressed;
        otherOptionsButton.RegisterCallback<MouseEnterEvent>(evt => otherOptionsButtonOnHoverEnter());
        otherOptionsButton.RegisterCallback<MouseLeaveEvent>(evt => otherOptionsButtonOnHoverExit());
        buttonSet.RegisterCallback<MouseEnterEvent>(evt => buttonSetOnHoverEnter());
        buttonSet.RegisterCallback<MouseLeaveEvent>(evt => buttonSetOnHoverExit());
        settingsButton.clicked += settingsButtonPressed;
        quitToMenuButton.clicked += quitToMenuButtonPressed;
        quitToDesktopButton.clicked += quitToDesktopButtonPressed;
    }

    void unSubscribeFromEvents()
    {
        //eventsSubscribed = false;

        resumeButton.clicked -= resumeButtonPressed;
        otherOptionsButton.UnregisterCallback<MouseEnterEvent>(evt => otherOptionsButtonOnHoverEnter());
        otherOptionsButton.UnregisterCallback<MouseLeaveEvent>(evt => otherOptionsButtonOnHoverExit());
        buttonSet.UnregisterCallback<MouseEnterEvent>(evt => buttonSetOnHoverEnter());
        buttonSet.UnregisterCallback<MouseLeaveEvent>(evt => buttonSetOnHoverExit());
        settingsButton.clicked -= settingsButtonPressed;
        quitToMenuButton.clicked -= quitToMenuButtonPressed;
        quitToDesktopButton.clicked -= quitToDesktopButtonPressed;
    }

    void shaderOpenAni()
    {
        shader.RemoveFromClassList("pausedShaderMinimize");
    }

    void shaderCloseAni()
    {
        shader.AddToClassList("pausedShaderMinimize");
    }

    async void escapePressed()
    {
        if (!uiDocument.enabled)
        {
            uiDocument.enabled = true;

            GameManager.instance.ChangeState(GameManager.GameState.Paused);
            HistoryUILogic.instance.stopwatch.pause();
            HistoryUILogic.instance.plusTotalTime();

            subscribeToEvents();
            await Task.Delay(1); // idk why
            shaderOpenAni();
            await Task.Delay(500);
            pausedIconAndLabelOpenAni();
            await Task.Delay(1100);
        }
        else if (!SettingsUILogic.instance.uiDocument.enabled)
        {
            pausedIconAndLabelCloseAni();
            await Task.Delay(1100);
            shaderCloseAni();
            await Task.Delay(500);
            unSubscribeFromEvents();

            HistoryUILogic.instance.stopwatch.resume();
            HistoryUILogic.instance.plusTotalTime();

            uiDocument.enabled = false;
        }
    }

    async void resumeButtonPressed()
    {
        Debug.Log("resumeButtonPressed");

        pausedIconAndLabelCloseAni();
        await Task.Delay(1100);
        shaderCloseAni();
        await Task.Delay(500);
        unSubscribeFromEvents();

        HistoryUILogic.instance.stopwatch.resume();
        HistoryUILogic.instance.plusTotalTime();

        uiDocument.enabled = false;
    }

    async void pausedIconAndLabelOpenAni()
    {
        pausedIcon.RemoveFromClassList("pausedIconRight");
        await Task.Delay(500);
        pausedLabel.RemoveFromClassList("pausedLabelLeft");
    }

    async void pausedIconAndLabelCloseAni()
    {
        pausedLabel.AddToClassList("pausedLabelLeft");
        await Task.Delay(600);
        pausedIcon.AddToClassList("pausedIconRight");
    }

    async void resumeIndicatorGlow()
    {
        while(true)
        {
            resumeIndicator.AddToClassList("resumeIndicatorOn");
            await Task.Delay(1000);
            resumeIndicator.RemoveFromClassList("resumeIndicatorOn");
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
        if (!otherOptionsButtonOnHover && !buttonSetOnHover)
        {
            otherOptionsButton.AddToClassList("otherOptionsButton-style-onHover");
            buttonSet.RemoveFromClassList("buttonSetDown");
        }
    }

    void otherOptionsClose()
    {
        if (!otherOptionsButtonOnHover && !buttonSetOnHover)
            otherOptionsButton.RemoveFromClassList("otherOptionsButton-style-onHover");
        buttonSet.AddToClassList("buttonSetDown");
    }
    void settingsButtonPressed()
    {
        Debug.Log("settingsButtonPressed");
        SettingsUILogic.instance.uiDocument.enabled = true;
    }

    void quitToMenuButtonPressed()
    {
        Debug.Log("quitToMenuButtonPressed");

        GameManager.instance.ChangeState(GameManager.GameState.MainMenu);
        HistoryUILogic.instance.stopwatch.reset();
        HistoryUILogic.instance.plusTotalTime();
        HistoryUILogic.instance.saveStopwatchTime();

        LoadingUILogic.instance.addScenesToLaod("UITest - startScene"); //scene 1
        LoadingUILogic.instance.loadScenes();
    }


    void quitToDesktopButtonPressed()
    {
        Debug.Log("quitToDesktopButtonPressed");
    }
}
