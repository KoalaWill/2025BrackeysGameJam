using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class InputOverlayUILogic : MonoBehaviour
{
    public static InputOverlayUILogic instance;

    private bool eventsSubscribed;
    private string jumpKeyBindText;
    private string leftKeyBindText;
    private string rightKeyBindText;
    private KeyCode jumpKey { get; set; }
    private KeyCode leftKey { get; set; }
    private KeyCode rightKey { get; set; }

    public UIDocument uiDocument;
    private VisualElement shader;
    private VisualElement jumpKeyDisplay;
    private VisualElement leftKeyDisplay;
    private VisualElement rightKeyDisplay;
    private Label jumpKeyLabel;
    private Label leftKeyLabel;
    private Label rightKeyLabel;
    private Label jumpKeyBind;
    private Label leftKeyBind;
    private Label rightKeyBind;

    // Start is called before the first frame update
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();

        if (SettingsUILogic.instance.inputOverlayEnabled)
            uiDocument.enabled = true;
        else
            uiDocument.enabled = false;

        eventsSubscribed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SettingsUILogic.instance.inputOverlayEnabled && !eventsSubscribed)
        {
            uiDocument.enabled = true;
            subscribeToEvents();
        }
        else if(!SettingsUILogic.instance.inputOverlayEnabled && eventsSubscribed)
        {
            uiDocument.enabled = false;
            unSubscribeFromEvents();
        }
        if (SettingsUILogic.instance.inputOverlayEnabled && SettingsUILogic.instance.keyBindChanged)
        {
            getPlayerPrefs();
            setKeyBindsText();
            SettingsUILogic.instance.keyBindChanged = false;
        }
        if (SettingsUILogic.instance.inputOverlayEnabled)
            refreshKeyStatus();
    }

    void subscribeToEvents()
    {
        Debug.Log("EventsSubscribed");
        eventsSubscribed = true;

        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        shader = root.Q<VisualElement>("shader");
        jumpKeyDisplay = root.Q<VisualElement>("jumpKeyDisplay");
        leftKeyDisplay = root.Q<VisualElement>("leftKeyDisplay");
        rightKeyDisplay = root.Q<VisualElement>("rightKeyDisplay");
        jumpKeyLabel = root.Q<Label>("jumpKeyLabel");
        leftKeyLabel = root.Q<Label>("leftKeyLabel");
        rightKeyLabel = root.Q<Label>("rightKeyLabel");
        jumpKeyBind = root.Q<Label>("jumpKeyBind");
        leftKeyBind = root.Q<Label>("leftKeyBind");
        rightKeyBind = root.Q<Label>("rightKeyBind");

        getPlayerPrefs();
        setKeyBindsText();
    }

    void unSubscribeFromEvents()
    {
        Debug.Log("EventsUnSubscribed");
        eventsSubscribed = false;
    }

    public void getPlayerPrefs()
    {
        jumpKeyBindText = PlayerPrefs.GetString("jumpkey", "Space");
        leftKeyBindText = PlayerPrefs.GetString("leftkey", "A");
        rightKeyBindText = PlayerPrefs.GetString("rightkey", "D");
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), jumpKeyBindText);
        leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), leftKeyBindText);
        rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), rightKeyBindText);
    }

    public void setKeyBindsText()
    {
        jumpKeyBind.text = "- " + jumpKeyBindText + " -";
        leftKeyBind.text = "- " + leftKeyBindText + " -";
        rightKeyBind.text = "- " + rightKeyBindText + " -";
    }

    void refreshKeyStatus()
    {
        if (Input.GetKey(jumpKey))
        {
            jumpKeyDisplay.style.backgroundColor = new Color(1f, 1f, 1f, 180f / 255f); ;
            jumpKeyLabel.style.color = new Color(0f, 0f, 0f);
            jumpKeyBind.style.color = new Color(0f, 0f, 0f);
        }
        else
        {
            jumpKeyDisplay.style.backgroundColor = new Color(0f, 0f, 0f, 70f / 255f);
            jumpKeyLabel.style.color = new Color(1f, 1f, 1f);
            jumpKeyBind.style.color = new Color(1f, 1f, 1f);
        }
        if (Input.GetKey(leftKey))
        {
            leftKeyDisplay.style.backgroundColor = new Color(1f, 1f, 1f, 180f / 255f); ;
            leftKeyLabel.style.color = new Color(0f, 0f, 0f);
            leftKeyBind.style.color = new Color(0f, 0f, 0f);
        }
        else
        {
            leftKeyDisplay.style.backgroundColor = new Color(0f, 0f, 0f, 70f / 255f);
            leftKeyLabel.style.color = new Color(1f, 1f, 1f);
            leftKeyBind.style.color = new Color(1f, 1f, 1f);
        }
        if (Input.GetKey(rightKey))
        {
            rightKeyDisplay.style.backgroundColor = new Color(1f, 1f, 1f, 180f / 255f); ;
            rightKeyLabel.style.color = new Color(0f, 0f, 0f);
            rightKeyBind.style.color = new Color(0f, 0f, 0f);
        }
        else
        {
            rightKeyDisplay.style.backgroundColor = new Color(0f, 0f, 0f, 70f / 255f);
            rightKeyLabel.style.color = new Color(1f, 1f, 1f);
            rightKeyBind.style.color = new Color(1f, 1f, 1f);
        }
    }
}
