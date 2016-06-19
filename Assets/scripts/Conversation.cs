using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationLine
{

}

public class DelegateLine : ConversationLine
{
    public delegate void DelegateLineDelegate();
    public DelegateLineDelegate OnReachLine;
    public DelegateLine(DelegateLineDelegate onReachLine)
    {
        this.OnReachLine = onReachLine;
    }
}

public class DialogueLine : ConversationLine
{
    public SpeakerType Speaker { get; set; }
    public string Text { get; set; }
    public float Time { get; set; }

    public DialogueLine(SpeakerType speaker, string text, float time)
    {
        this.Speaker = speaker;
        this.Text = text;
        this.Time = time;
    }
}

public class GamePauseLine : ConversationLine
{
    public bool Pause { get; set; }

    public GamePauseLine(bool pause)
    {
        Pause = pause;
    }
}


public class Conversation
{
    List<ConversationLine> conversationLineList = new List<ConversationLine>();
    public int Position { get; set; }
    public bool WaitingForPlayer { get; set; }

    public Conversation()
    {
        Position = -1;
    }

    public void Add(ConversationLine conversationLine)
    {
        conversationLineList.Add(conversationLine);
    }

    public virtual ConversationLine GetNextLine()
    {
        Position++;
        if (Position >= conversationLineList.Count)
            return null;
        return conversationLineList[Position];
    }
}

public class RandomSuggestionConversation : Conversation
{
    private static readonly string[] intro = { "Oh, it's my", "You've found my", "Look, it's my", "Ah! My design for a", "I wondered where I put that", };
    private static readonly string[] segment1 = { "steam-powered","authentic", "anti-garotting","reversable","replacable"};
    private static readonly string[] segment2 = { "noodle", "hat", "safety", "electric", "walking stick", "fire", "horse", "hair", "sausage" };
    private static readonly string[] segment3 = { "cannon", "repellant", "wax", "extender", "lamp", "pump", "engine", "gun", "replacer", "crayons", "matches", "measurer","extruder" };

    private static readonly string[] suggestion1 = { "fish-flavoured", "exotic", "fantastic" };
    private static readonly string[] suggestion2 = { "giant", "explosive", "purple-haired"};
    private static readonly string[] suggestion3 = { "lizard", "banana", "platypus"};

    public override ConversationLine GetNextLine()
    {
        return new DialogueLine(SpeakerType.Professor, GenerateRandomLine(), 0);
    }

    private string GenerateRandomLine()
    {
        return "..." + suggestion1[Random.Range(0, suggestion1.Length)] + " " + suggestion2[Random.Range(0, suggestion2.Length)] + " " + suggestion3[Random.Range(0, suggestion3.Length)];
    }

    public static DialogueLine RandomBluePrintResponse()
    {
        return new DialogueLine(SpeakerType.Professor,intro[Random.Range(0, intro.Length)] + " " + segment1[Random.Range(0, segment1.Length)] + " " + segment2[Random.Range(0, segment2.Length)] + " " + segment3[Random.Range(0, segment3.Length)],0.75f);
    }
}