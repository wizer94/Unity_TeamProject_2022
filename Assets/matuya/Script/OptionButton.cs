using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    GetKeyCode getkey;

    private Text[] text;
    private string[] savetext;
    private string[] textname;
    private int num_;
    private string key;
    private int keyint;
    private bool keyflag, saveflag, emptyflag;
    public GameObject emptytext;
    public GameObject backinfo;
    public GameObject optioninfo;
    public GameObject warning;
    public GameObject warningtext;

    float arufa;
    float fadespeed, fadetime;

    // Start is called before the first frame update
    void Start()
    {
        keyflag = emptyflag = false;
        saveflag = true;
        textname = new string[15];
        text = new Text[15];
        savetext = new string[15];
        getkey = GetComponent<GetKeyCode>();

        arufa = 255f;
        fadespeed = 0.5f;
        fadetime = 0f;

        for (int i = 0; i < text.Length; i++)
        {
            textname[i] = getkey.Loadname(i);
            //Debug.Log(textname[i]);
            text[i] = GameObject.Find("optionInfo/ScrollView/Content/KeyConfig/" + textname[i] + "/Button/" + textname[i]).GetComponent<Text>();
            text[i].text = getkey.LoadText(i);
            savetext[i] = getkey.LoadText(i);
        }
        backinfo.SetActive(false);
        emptytext.SetActive(false);
        warning.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (keyflag)
            KeyChange(num_);

        KeyCodeEmpty();
        emptytext.SetActive(emptyflag);

        if (emptyflag) {
            if (fadetime > 0) {
                fadetime -= Time.deltaTime;
            }
            else {
                Fadeout();
            }
        }
    }

    public void KeyChange(int num)
    {
        foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(code))
            {
                key = code.ToString();
                keyint = (int)code;
                Debug.Log(code);
                keyflag = false;
                text[num].text = key;
                getkey.GetData(text[num], num, keyint);
                CheckKey(num);
            }
        }
    }

    public void GetWalkUpButtonDown()
    {
        string keytext = "_";
        text[0].text = keytext;
        num_ = 0;
        keyflag = true;
    }
    public void GetWalkDownButtonDown()
    {
        string keytext = "_";
        text[1].text = keytext;
        num_ = 1;
        keyflag = true;
    }
    public void GetWalkLeftButtonDown()
    {
        string keytext = "_";
        text[2].text = keytext;
        num_ = 2;
        keyflag = true;
    }
    public void GetWalkRightButtonDown()
    {
        string keytext = "_";
        text[3].text = keytext;
        num_ = 3;
        keyflag = true;
    }
    public void GetDashButtonDown()
    {
        string keytext = "_";
        text[4].text = keytext;
        num_ = 4;
        keyflag = true;
    }
    public void GetAvoidanceButtonDown()
    {
        string keytext = "_";
        text[5].text = keytext;
        num_ = 5;
        keyflag = true;
    }
    public void GetAttackButtonDown()
    {
        string keytext = "_";
        text[6].text = keytext;
        num_ = 6;
        keyflag = true;
    }
    public void GetADSButtonDown()
    {
        string keytext = "_";
        text[7].text = keytext;
        num_ = 7;
        keyflag = true;
    }
    public void GetReloadButtonDown()
    {
        string keytext = "_";
        text[8].text = keytext;
        num_ = 8;
        keyflag = true;
    }
    public void GetWeapon1ButtonDown()
    {
        string keytext = "_";
        text[9].text = keytext;
        num_ = 9;
        keyflag = true;
    }
    public void GetWeapon2ButtonDown()
    {
        string keytext = "_";
        text[10].text = keytext;
        num_ = 10;
        keyflag = true;
    }
    public void GetSwapWeaponButtonDown()
    {
        string keytext = "_";
        text[11].text = keytext;
        num_ = 11;
        keyflag = true;
    }
    public void GetActionButtonDown()
    {
        string keytext = "_";
        text[12].text = keytext;
        num_ = 12;
        keyflag = true;
    }
    public void GetInventoryButtonDown()
    {
        string keytext = "_";
        text[13].text = keytext;
        num_ = 13;
        keyflag = true;
    }
    public void GetMapButtonDown()
    {
        string keytext = "_";
        text[14].text = keytext;
        num_ = 14;
        keyflag = true;
    }

    //キーバインド重複阻止
    public void CheckKey(int num)
    {
        for (int i = 0; i < text.Length; i++) {
            if (text[num].text == text[i].text && num != i) {
                text[i].text = "";
                emptyflag = true;
            }
        }
    }    
    //キーに空欄があるかどうか
    public void KeyCodeEmpty()
    {
        for (int i = 0; i < text.Length; i++) {
            if (text[i].text == "") {
                emptyflag = true;
                return;
            }
        }
        emptyflag = false;
    }
    //キーが変わっているかどうかのっチェック
    public void CheckKeyChange()
    {
        for (int i = 0; i < text.Length; i++) {
            if (text[i].text != savetext[i]) {
                saveflag = false;
                break;
            }
        }
    }
    //フェードアウト用
    public void Fadeout()
    {
        Color color = warning.GetComponent<Image>().color;
        color.a -= Time.deltaTime * fadespeed;
        warning.GetComponent<Image>().color = color;
        Color color2 = warningtext.GetComponent<Text>().color;
        color2.a -= Time.deltaTime * fadespeed;
        warningtext.GetComponent<Text>().color = color2;
    }
    public void SetArufa()
    {
        Color color = warning.GetComponent<Image>().color;
        color.a = 1f;
        warning.GetComponent<Image>().color = color;
        Color color2 = warningtext.GetComponent<Text>().color;
        color2.a = 1f;
        warningtext.GetComponent<Text>().color = color2;
    }
    public void PushBackButtonDown()
    {
        if (emptyflag) {
            SetArufa();
            warning.SetActive(true);
            fadetime = 1f;
            return;
        }

        CheckKeyChange();
        if (saveflag)
        {
            optioninfo.SetActive(false);
            EscGameQuitScript.optionflag = false;
            EscGameQuitScript.pause_check = true;
            SaveText();
        }
        else
            backinfo.SetActive(true);
    }
    public void GetSaveYesButtonDown()
    {
        SaveText();
        backinfo.SetActive(false);
        optioninfo.SetActive(false);
        EscGameQuitScript.optionflag = false;
        EscGameQuitScript.pause_check = true;
    }
    public void GetSaveNoButtonDown()
    {
        backinfo.SetActive(false);
    }
    public void SaveText()
    {
        getkey.SaveText();
    }
}
