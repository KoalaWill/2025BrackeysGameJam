using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class chatBoxUILogic : MonoBehaviour
{
    public static chatBoxUILogic instance;

    public bool test = false;
    private bool textFinished;

    private UIDocument uiDocument;
    private VisualElement shader;
    private Button chatBoxButton;
    private Label characterNameLabel;
    private Label chatTextLabel;

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
            enableChatBox("Steve", "I love minecraft. Movie Coming out in 2025. \nPerhaps we'll get to have minecraft movie befroer GTA VI. Holy shit. SUck on DEEZ NUTs.", 50);
            test = false;
        }
    }

    void subscribeToEvents()
    {
        Debug.Log("EventsSubscribed");

        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        shader = root.Q<VisualElement>("shader");
        chatBoxButton = root.Q<Button>("chatBoxButton");
        characterNameLabel = root.Q<Label>("characterNameLabel");
        chatTextLabel = root.Q<Label>("chatTextLabel");

        chatBoxButton.clicked += chatBoxButtonPressed;
    }

    void unSubscribeFromEvents()
    {
        Debug.Log("EventsUnSubscribed");

        chatBoxButton.clicked -= chatBoxButtonPressed;
    }

    void shaderOpenAni()
    {
        shader.RemoveFromClassList("chatBoxShaderOff");
    }

    void shaderCloseAni()
    {
        shader.AddToClassList("chatBoxShaderOff");
    }

    async void chatBoxButtonPressed()
    {
        if (!textFinished)
            textFinished = true;
        else
        {
            shaderCloseAni();
            await Task.Delay(100);

            unSubscribeFromEvents();
            uiDocument.enabled = false;
        }
    }

    public async void enableChatBox(string characterName, string chatText, int msPerCha)
    {
        textFinished = false;
        subscribeToEvents();
        uiDocument.enabled = true;

        characterNameLabel.text = characterName;
        chatTextLabel.text = "";

        await Task.Delay(1); //idk why
        shaderOpenAni();
        await Task.Delay(1000);

        for (int i = 0; i < chatText.Length; i++)
        {
            if (textFinished)
                break;
            chatTextLabel.text += chatText[i];
            await Task.Delay(msPerCha);
        }
        chatTextLabel.text = chatText;
        textFinished = true;
    }
}
