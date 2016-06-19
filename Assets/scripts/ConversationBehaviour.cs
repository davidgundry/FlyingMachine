using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum SpeakerType
{
    Player,
    Professor,
    Sound
}

public class ConversationBehaviour : MonoBehaviour {

    private GameObject ConversationPanel { get; set; }
    private Image MoveOnIcon { get; set; }
    private Text TextBox { get; set; }

    private IntroBehaviour IntroBehaviour { get; set; }
    private bool Cutscene { get; set; }

    private Conversation Conversation { get; set; }
    private IEnumerator ShowTextCoroutine { get; set; }

    private GameController GameController { get; set; }

    public int SpeechEventsSinceTextChange { get; set; }
    private bool WaitConversationLock { get; set; }

	void Start () {
        if (GameObject.FindObjectOfType<GameController>())
        {
            MoveOnIcon = GameObject.Find("MoveOnIcon").GetComponent<Image>();
            TextBox = GameObject.Find("TextBox").GetComponent<Text>();
            ConversationPanel = GameObject.Find("ConversationPanel");
            GameController = GameObject.FindObjectOfType<GameController>();
            Cutscene = false;
        }
        else if (GameObject.FindObjectOfType<IntroBehaviour>())
        {
            Cutscene = true;
            IntroBehaviour = GameObject.FindObjectOfType<IntroBehaviour>();
            TextBox = GameObject.Find("TextBox").GetComponent<Text>();
        }
        else
            Debug.LogWarning("Converation Behaviour found neither IntroBehaviour or Game Controller");
	}

    void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            if (ShowTextCoroutine != null)
                HurryConversation();
            else if (Conversation.WaitingForPlayer)
                MoveOnConversation();
        }


        if (SpeechEventsSinceTextChange > 2)
        {
            SpeechEventsSinceTextChange = 0;
            StartCoroutine(WaitThenMoveOnConversation(1f));
        }

    }

    private IEnumerator WaitThenMoveOnConversation(float wait)
    {
        WaitConversationLock = true;
        yield return new WaitForSeconds(wait);
        SpeechEventsSinceTextChange = 0;
        if (WaitConversationLock)
            MoveOnConversation();
    }

    public void StartConversation(Conversation conversation)
    {
        Conversation = conversation;
        ShowConversationPanel();
        DoConversationLine(conversation.GetNextLine());
    }

    public void StopConversation()
    {
        HideConversationPanel();
    }

    private void WaitingForPlayer(bool waiting)
    {
        if (waiting)
        {
            Conversation.WaitingForPlayer = true;
            if (!Cutscene)
                MoveOnIcon.enabled = true;
        }
        else
        {
            Conversation.WaitingForPlayer = false;
            if (!Cutscene)
                MoveOnIcon.enabled = false;
        }
    }

    private void MoveOnConversation()
    {
        if (ShowTextCoroutine !=null)
        {
            StopCoroutine(ShowTextCoroutine);
            ShowTextCoroutine = null;
        }
        WaitingForPlayer(false);
        SpeechEventsSinceTextChange = 0;
        DoConversationLine(Conversation.GetNextLine());
    }

    private void HurryConversation()
    {
        StopCoroutine(ShowTextCoroutine);
        ShowTextCoroutine = null;
        WaitingForPlayer(true);
        SpeechEventsSinceTextChange = 0;
    }

    public void OneOffConversationLine(ConversationLine conversationLine)
    {
        if (ShowTextCoroutine != null)
        {
            StopCoroutine(ShowTextCoroutine);
            ShowTextCoroutine = null;
        }
        SpeechEventsSinceTextChange = 0;
        WaitingForPlayer(false);
        
        DoConversationLine(conversationLine);
    }

    private void DoConversationLine(ConversationLine conversationLine)
    {
        WaitConversationLock = false;
        ShowConversationPanel();
        if (conversationLine != null)
        {
            if (conversationLine.GetType() == typeof(DialogueLine))
            {
                DisplayDialogueLine((DialogueLine)conversationLine);
            }
            else if (conversationLine.GetType() == typeof(GamePauseLine))
            {
                if (Cutscene)
                    IntroBehaviour.PlayPaused = ((GamePauseLine)conversationLine).Pause;
                else
                    GameController.PlayPaused = ((GamePauseLine)conversationLine).Pause;
                MoveOnConversation();
            }
            else if (conversationLine.GetType() == typeof(DelegateLine))
            {
                ((DelegateLine)conversationLine).OnReachLine();
                MoveOnConversation();
            }
        }
        else
        {
            StopConversation();
        }
    }

    public void DisplayDialogueLine(DialogueLine dialogueLine)
    {
        switch (dialogueLine.Speaker)
        {
            case SpeakerType.Player:
                ShowTextCoroutine = ShowText("You: " + dialogueLine.Text, 5, dialogueLine.Time);
                StartCoroutine(ShowTextCoroutine);
                break;
            case SpeakerType.Professor:
                ShowTextCoroutine = ShowText("Doc: " + dialogueLine.Text, 4, dialogueLine.Time);
                StartCoroutine(ShowTextCoroutine);
                break;
            case SpeakerType.Sound:
                ShowTextCoroutine = ShowText(dialogueLine.Text, dialogueLine.Text.Length-1, dialogueLine.Time);
                StartCoroutine(ShowTextCoroutine);
                break;
        }
    }

    private IEnumerator ShowText(string text, int startingIndex, float timer = 0)
    {
        int characterCount = startingIndex;
        while (characterCount < text.Length)
        {
            characterCount++;
            TextBox.text = text.Substring(0, characterCount);
            yield return new WaitForSeconds(0.02f);
        }
        WaitingForPlayer(true);
        if (timer > 0)
        {
            StartCoroutine(WaitThenMoveOnConversation(timer));
        }
    }


    public void HideConversationPanel()
    {
        if (!Cutscene)
            ConversationPanel.SetActive(false);
    }

    public void ShowConversationPanel()
    {
        if (!Cutscene)
            ConversationPanel.SetActive(true);
    }

    public void IncrementSpeechEventCounter()
    {
        SpeechEventsSinceTextChange++;
    }

}
