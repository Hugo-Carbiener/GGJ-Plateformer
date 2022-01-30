using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Dialogue
{
    public int id;
    public string sentence;
    public int character;
    public int emotion;
    public bool invisibleChar;

    public Dialogue(int id_, string sentence_, int character_, int emotion_, bool invisibleChar_)
    {
        this.id = id_;
        this.sentence = sentence_;
        this.character = character_;
        this.emotion = emotion_;
        this.invisibleChar = invisibleChar_;
    }
}

public class TextWriter : MonoBehaviour
{

    public Text txt;
    public List<Image> ptr;
    public GameObject dialBox;
    public TextAsset ta;
    public int currentDialogueId;
    public float speed;
    private GameManager gm;
    private int index;
    private int charact;
    private int emotion;
    private string sentence;
    public bool isWriting;
    [SerializeField] List<Dialogue> dial = new List<Dialogue>();
    public List<Sprite> portrait_Axo;
    public List<Sprite> portrait_Geck;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //ActivateDialogue();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (isWriting)
            {
                isWriting = false;
            }
            else
            {
                AddText();
            }
        }
    }

    public void AddText()
    {
        if (currentDialogueId >= dial.Count)
        {
            dialBox.SetActive(false);
            EnablePlayerMouvment();
            return;
        }

        isWriting = true;
        index = 0;
        

        sentence = GetSentence(currentDialogueId).sentence;
        charact = GetSentence(currentDialogueId).character;
        emotion = GetSentence(currentDialogueId).emotion;
        StartCoroutine(WriteText(sentence, sentence.Length, charact, true));
        currentDialogueId++;

        ActivateCharacter(charact, emotion);
    }

    public IEnumerator WriteText(string sentence, int charNb, int charact, bool invisibleChar)
    {

        while (index < charNb && isWriting)
        {
            yield return new WaitForSeconds(speed);
            string text = sentence.Substring(0, index);
            if (invisibleChar)
            {
                text += "<color=#00000000>" + sentence.Substring(index) + "</color>";
            }
            txt.text = text;
            index++;
        }
      
        isWriting = false;
        invisibleChar = false;
        txt.text = sentence.Substring(0, charNb);
        yield break;
    }

    public void ActivateCharacter(int id, int emo)
    {
        foreach(var x in ptr)
        {
            x.gameObject.SetActive(false);
        }

        ptr[id].gameObject.SetActive(true);

        if (id == 1)
        {
            ptr[id].sprite = portrait_Axo[emo];
        }
        else
        {
            ptr[id].sprite = portrait_Geck[emo];
        }
    }

    public void EnablePlayerMouvment()
    {
        // enable player mouv when dialogue is ended;
        Debug.Log("You can now play !");
    }

    public void ActivateDialogue()
    {
        ta = gm.currentTableauManager.text_for_the_tab;
        AnalyseTextAsset(ta);
        dialBox.SetActive(true);
        currentDialogueId = 0;
        AddText();
    }

    public Dialogue GetSentence(int id)
    {
        return dial.Find(dials => dials.id == id);
    }

    public void AnalyseTextAsset(TextAsset ta)
    {
        string ss = "";
        int cc = 0;
        int ee = 0;
        int id = 0;

        var l = ta.text.Split('\n');

        for (int i = 0; i < l.Length; i++)
        {
            id = i;
            var c = l[i].Split(char.Parse("|"));

            ss = c[0];

            int ccc;
            int.TryParse(c[1], out ccc);
            cc = ccc;

            int eee;
            int.TryParse(c[2], out eee);
            ee = eee;

            dial.Add(new Dialogue(id, ss, cc, ee, true));
        } 
    }

}