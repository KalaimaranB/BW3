using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [Header("Local Variables")]
    [SerializeField] private RawImage backgroundBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private RawImage characterIcon;
    [SerializeField] private RawImage borderImage;

    [Header("Characters")]
    [SerializeField] private List<Character> characters;

    private Animator anim;

    [SerializeField]
    private string[] specialPhrases = new string[0];

    [SerializeField]
    private List<Message> waitList = new List<Message>();
    [SerializeField]
    [ReadOnly]
    private Message currentMessage;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckForNewMessages()
    {
        if (waitList.Count>0)
        {
            StartCoroutine(SendMessageToDialogueBox(waitList[0].message, waitList[0].messageDisplayTime, waitList[0].characterName));
            waitList.Remove(waitList[0]);
        }
        else
        {
        }
    }

    public IEnumerator SendMessageToDialogueBox(string message, float displayTime, string characterName)
    {
        currentMessage = new Message(message, characterName, displayTime);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Deactivated Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Deactivate Box"))
        {
            anim.SetTrigger("Activate");
        }

        dialogueText.text = replacedMessage(message);
        AssignBoxBasedOnCharacterName(characterName);
        yield return new WaitForSeconds(displayTime);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Activated Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Activate Box"))
        {
            anim.SetTrigger("Deactivate");
        }
        currentMessage.characterName = "";
        currentMessage.message = "";
        currentMessage = null;
        dialogueText.text = "";
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Activated Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Activate Box") || anim.GetCurrentAnimatorStateInfo(0).IsName("Deactivate Box"))
        {
            yield return null;
        }
        CheckForNewMessages();
    }

    private string replacedMessage(string message)
    {
        string toReturn = message;


        //First replace the character names
        foreach(Character character in characters)
        {
            toReturn = toReturn.ReplaceInsensitive(character.CharacterName, character.CharacterName.ToUpper());
        }
        //Then vehicle names
        foreach(FactionTypes.Unit unit in Finder.FactionManager.WesternFrontier.vehicleUnits)
        {
            toReturn = toReturn.ReplaceInsensitive(unit.UnitName, unit.UnitName.ToUpper());
        }
        //Then aircraft names
        foreach (FactionTypes.Unit unit in Finder.FactionManager.WesternFrontier.aviationUnits)
        {
            toReturn = toReturn.ReplaceInsensitive(unit.UnitName, unit.UnitName.ToUpper());
        }
        //Then ship names
        foreach (FactionTypes.Unit unit in Finder.FactionManager.WesternFrontier.navalUnits)
        {
            toReturn = toReturn.ReplaceInsensitive(unit.UnitName, unit.UnitName.ToUpper());
        }
        //Then infantry names
        foreach (FactionTypes.Unit unit in Finder.FactionManager.WesternFrontier.infantryUnits)
        {
            toReturn = toReturn.ReplaceInsensitive(unit.UnitName, unit.UnitName.ToUpper());
        }

        //Then special phrase
        foreach(string str in specialPhrases)
        {
            toReturn = toReturn.ReplaceInsensitive(str, str.ToUpper());
        }

        return toReturn;
    }

    private void AssignBoxBasedOnCharacterName(string name)
    {
        Character targetCharacter = characters.Find(i => i.CharacterName == name);
        if (targetCharacter!=null)
        {
            backgroundBox.color = targetCharacter.backroundColor;
            characterIcon.texture = targetCharacter.characterIcon;
            borderImage.color = targetCharacter.characterBackgroundColor;
        }
        else
        {
            Debug.LogWarning(name+" --> This character does not exist!");
        }
    }


    public void SendMessageToBox(string message, string characterName, float displayTime)
    {
        Message mess = new Message(message, characterName, displayTime);
        if (currentMessage.message =="" && currentMessage.characterName=="")
        {
            StartCoroutine(SendMessageToDialogueBox(message, displayTime, characterName));
        }
        else
        {
            waitList.Add(mess);
        }
    }


    [System.Serializable]
    public class Message
    {
        [TextArea(1,10)]
        public string message;
        public string characterName;
        public float messageDisplayTime;

        public Message (string messageText, string character, float displayTime)
        {
            message = messageText;
            characterName = character;
            messageDisplayTime = displayTime;
        }
    }

    [System.Serializable]
    public class Character
    {
        public Texture characterIcon;
        public Identification.Faction characterFaction;
        public string CharacterName;
        public Color backroundColor;
        public Color characterBackgroundColor;
    }
}