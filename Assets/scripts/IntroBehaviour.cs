using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroBehaviour : MonoBehaviour {

    public Transform blueprintPrefab;

    Conversation IntroConv { get; set; }
    private ConversationBehaviour ConversationBehaviour { get; set; }
    private Camera MainCamera { get; set; }

    public bool PlayPaused { get; set; }
    private float CameraShake { get; set; }

	// Use this for initialization
	void Start () {
        MainCamera = GameObject.FindObjectOfType<Camera>();

        ConversationBehaviour = GameObject.FindObjectOfType<ConversationBehaviour>();
        IntroConv = new Conversation();
        IntroConv.Add(new DelegateLine(delegate() { StartCoroutine(MoveCamera(new Vector3(0, 2f, MainCamera.transform.position.z),0.2f)); }));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "London, 1903", 2f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "Doctor Allophone's Workshop", 2f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 1f));
        IntroConv.Add(new DelegateLine(delegate() { CameraShake = 1f; }));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.1f));
        IntroConv.Add(new DelegateLine(delegate() { CameraShake = 0f; }));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound,"*bang*",1));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.5f));
        IntroConv.Add(new DelegateLine(delegate() { CameraShake = 1f; }));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.1f));
        IntroConv.Add(new DelegateLine(delegate() { CameraShake = 0f; }));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "*crash*", 1));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 1f));
        IntroConv.Add(new DialogueLine(SpeakerType.Professor, "I've done it!", 1.5f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.5f));
        IntroConv.Add(new DialogueLine(SpeakerType.Professor, "At last, my flying machine is complete!", 1.5f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.5f));
        IntroConv.Add(new DialogueLine(SpeakerType.Professor, "Go and pull that lever!", 1.5f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.25f));
        IntroConv.Add(new DialogueLine(SpeakerType.Player, "Are you sure this will work?", 1.5f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.25f));
        IntroConv.Add(new DialogueLine(SpeakerType.Professor, "It's my greatest invention! Of course it will work!", 1.5f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.3f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "*clunk*", 1));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.5f));
        IntroConv.Add(new DelegateLine(delegate() { CameraShake = 0.1f; }));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "*whir*", 1));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.5f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "*rattle*", 1));
        IntroConv.Add(new DelegateLine(delegate() { CameraShake = 0.2f; }));
        IntroConv.Add(new DialogueLine(SpeakerType.Professor, "Look out!", 0.5f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "*crash*", 1));
        IntroConv.Add(new DialogueLine(SpeakerType.Professor, "Oh no! It's going to...", 0.01f));
        IntroConv.Add(new DialogueLine(SpeakerType.Professor, "Not that stack of papers!", 0.2f));
        IntroConv.Add(new DelegateLine(delegate() { CameraShake = 0.5f; }));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "*smash*", 1));
        IntroConv.Add(new DelegateLine(delegate() { CameraShake = 0; }));
        IntroConv.Add(new DelegateLine(delegate() { StartCoroutine(MoveCamera(new Vector3(0, 3.5f, MainCamera.transform.position.z), 1f)); }));
        IntroConv.Add(new DelegateLine(delegate() { StartCoroutine(FountainAnimation()); }));
        IntroConv.Add(new DialogueLine(SpeakerType.Professor, "My inventions!", 2f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.25f));
        IntroConv.Add(new DialogueLine(SpeakerType.Professor, "Quick! Into the flying machine!\nGet my designs back before they blow away!", 3f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.5f));
        IntroConv.Add(new DialogueLine(SpeakerType.Player, "Are you sure this is safe?", 1.5f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.1f));
        IntroConv.Add(new DialogueLine(SpeakerType.Professor, "No time for thinking about yourself, science is at stake!", 2f));
        IntroConv.Add(new DialogueLine(SpeakerType.Sound, "", 0.5f));
        IntroConv.Add(new DelegateLine(delegate() { Application.LoadLevel("main"); }));
        ConversationBehaviour.StartConversation(IntroConv);
	}

    private IEnumerator MoveCamera(Vector3 target, float speed)
    {
        Vector3 current = MainCamera.transform.position;
        float interpolate = 0;
        while (interpolate < 1)
        {
            interpolate += Time.deltaTime * speed;
            MainCamera.transform.position = new Vector3(Mathf.Lerp(current.x, target.x, interpolate), Mathf.Lerp(current.y, target.y, interpolate), Mathf.Lerp(current.z, target.z, interpolate));
            yield return null;
        }
        
    }

    private IEnumerator FountainAnimation()
    {
        List<Transform> blueprints = new List<Transform>();
        float timer = 0;
        while (timer < 2.1f)
        {
            if (timer < 1f)
            {
                Transform newBlueprint = Instantiate(blueprintPrefab);
                newBlueprint.transform.position = new Vector3(0, 0, 1f);
                blueprints.Add(newBlueprint);
            }
            foreach (Transform blueprint in blueprints)
            {
                blueprint.transform.position = blueprint.transform.position + new Vector3((Random.value-0.5f) * 0.4f, 0.08f, 0);
            }
            timer += Time.deltaTime;
            yield return null;
        }

    }

	// Update is called once per frame
	void Update () {
	    if (CameraShake > 0)
        {
            MainCamera.transform.position = new Vector3(Random.value * CameraShake,2+ Random.value * CameraShake, MainCamera.transform.position.z);
        }

	}
}
