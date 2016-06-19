using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MicTools;

public class GameController : MonoBehaviour
{
    public BlueprintPickupBehaviour blueprintPrefab;

    private Text ScoreText { get; set; }
    private PlayerBehaviour PlayerBehaviour { get; set; }
    private ConversationBehaviour ConversationBehaviour { get; set; }

    private int score;
    private int Score { get { return score; } set { score = value; ScoreText.text = value.ToString(); } }

    public bool PlayPaused { get; set; }

    private MicTools.MicrophoneController microphoneController;
    private MicTools.MicrophoneInput microphoneInput;

    void Start()
    {
        PlayerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        ConversationBehaviour = GameObject.FindObjectOfType<ConversationBehaviour>();
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        Score = 0;

        microphoneController = GetComponent<MicTools.MicrophoneController>();
        microphoneInput = GetComponent<MicTools.MicrophoneInput>();
        microphoneController.microphoneActive = true;

        Conversation startConvo = new Conversation();
        /*startConvo.Add(new GamePauseLine(true));
        startConvo.Add(new DelegateLine(delegate() { PlayerBehaviour.GravityEnabled = false; PlayerBehaviour.SpeechEnabled = false; StartCoroutine(IntroAnimation()); }));
        startConvo.Add(new DialogueLine(SpeakerType.Professor, "It works!",2));
        startConvo.Add(new DialogueLine(SpeakerType.Sound, "", 2));
        startConvo.Add(new GamePauseLine(false));
        startConvo.Add(new DialogueLine(SpeakerType.Professor, "Right. See that thing you're holding? Turn that to move left and right.",3));
        startConvo.Add(new DialogueLine(SpeakerType.Professor, "Or you can use my new invention, the QWERTY keyboard.", 2));
        startConvo.Add(new DialogueLine(SpeakerType.Professor, "Use the left and right arrow keys.", 2));*/
        startConvo.Add(new DelegateLine(delegate() { PlayerBehaviour.GravityEnabled = true; }));
        //startConvo.Add(new DialogueLine(SpeakerType.Professor, "Look out! You're falling", 0.2f));
        //startConvo.Add(new DialogueLine(SpeakerType.Professor, "The flying machine is powered by words. Say something quick!", 0.1f));
        startConvo.Add(new DelegateLine(delegate() { PlayerBehaviour.SpeechEnabled = true; }));
        //startConvo.Add(new DialogueLine(SpeakerType.Sound, "Doc: The flying machine is powered by words. Say something quick!", 2f));
        //startConvo.Add(new DialogueLine(SpeakerType.Professor, "My patented phono-motor converts speech into motion.", 0.4f));
        //startConvo.Add(new DialogueLine(SpeakerType.Professor, "The more ridiculous a phrase, the more thrust the phono-motor can generate.", 0.4f));
        //startConvo.Add(new DialogueLine(SpeakerType.Professor, "Don't worry! I compiled a dictionary of ridiculous phrases for just such an occasion!", 0.4f));
        //startConvo.Add(new DialogueLine(SpeakerType.Professor, "Now where did I put it?...", 1f));
        startConvo.Add(new DelegateLine(delegate() { CreateBlueprints(); }));
        //startConvo.Add(new DialogueLine(SpeakerType.Sound, " ", 1f));
        startConvo.Add(new DelegateLine(delegate() { ConversationBehaviour.StartConversation(new RandomSuggestionConversation()); } ));
        ConversationBehaviour.StartConversation(startConvo);

    }

    private void CreateBlueprints()
    {
        for (int i = 0; i < 5; i++)
        {
            BlueprintPickupBehaviour newBlueprint = Instantiate<BlueprintPickupBehaviour>(blueprintPrefab);
            newBlueprint.transform.position = new Vector3(0,-10,0);
        }
    }

    private IEnumerator IntroAnimation()
    {
        Vector3 target = new Vector3(0,3,0);
        Vector3 current = PlayerBehaviour.transform.position;
        float interpolate = 0f;
        while (interpolate < 1f)
        {
            interpolate += Time.deltaTime*0.3f;
            PlayerBehaviour.transform.position = new Vector3(Mathf.Lerp(current.x, target.x, interpolate), Mathf.Lerp(current.y, target.y, interpolate), Mathf.Lerp(current.z, target.z, interpolate));
            yield return null;
        }
    }

    void Update()
    {
        if (Input.GetKeyUp("p"))
            PlayPaused = !PlayPaused;
        if (Input.GetKey("up"))
            InputEvent();
    }

    public void InputEvent()
    {
        if (!PlayPaused)
        {
            ConversationBehaviour.IncrementSpeechEventCounter();
            PlayerBehaviour.Thrust();
        }
    }

    void OnSoundEvent(MicTools.SoundEvent se)
    {
        if (se == MicTools.SoundEvent.SyllablePeak)
            InputEvent();
    }

    private void HumInput()
    {
        if (microphoneInput.NormalisedPeakAutocorrelation > SyllableDetectionAlgorithm.npaThreshold)
            InputEvent();
    }

    /// <summary>
    /// Called when flying machine gets too low.
    /// </summary>
    public void CrashFloor()
    {
        ConversationBehaviour.OneOffConversationLine(new DialogueLine(SpeakerType.Professor,"You need to say something to make it fly!",2f));
    }

    public void GotBlueprint()
    {
        ConversationBehaviour.OneOffConversationLine(RandomSuggestionConversation.RandomBluePrintResponse());
        Score++;
    }

}
