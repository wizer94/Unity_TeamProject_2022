using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateButton : MonoBehaviour
{
    public GameObject optionInfo;
    public GameObject titlebutton;
    public GameObject statebutton;
    public GameObject finishinfo;

    public Text but1, but2, but3, but4;

    public GameObject player;
    public GameObject reticle;
    public PlayerStat P_Stat;

    Camera maincamera;
    // Start is called before the first frame update
    void Start()
    {
        maincamera = Camera.main;

        Cursor.visible = true;
        optionInfo.SetActive(false);
        titlebutton.SetActive(false);
        finishinfo.SetActive(false);

        player = PlayerGenerateScript.player;
        reticle = PlayerGenerateScript.reticle;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            titlebutton.SetActive(true);
            statebutton.SetActive(false);
        }        

    }

    //ボタンが押された時の処理
    public void LoadButtonDown()
    {

    }
    public void StateButtonDown()
    {
        if (player != null)
        {
            player.SetActive(true);
            reticle.SetActive(true); 
            player.transform.position = new Vector3(0,-8, player.transform.position.z);
            P_Stat = player.GetComponent<PlayerStat>();
            P_Stat.HP = P_Stat.Max_HP;
        }
        if(StaticVariable.Tutorialed)
            StaticBgmScript.PlayStage();
        SceneManager.LoadScene(StaticVariable.Tutorialed ? "GameScene01" : "Tutorial");
    }
    public void OptionButtonDown()
    {
        optionInfo.SetActive(true);
        TitleManager.openInfo = true;
    }
    public void BackButtonDown()
    {
        optionInfo.SetActive(false);
        TitleManager.openInfo = false;
    }
    public void FinishButtonDown()
    {
        finishinfo.SetActive(true);
    }
    public void FinishinfoYesDown()
    {
        Application.Quit();
    }
    public void FinishinfoNosDown()
    {
        finishinfo.SetActive(false);
    }

    //ボタンに触れているときの処理
    public void OnPointerbut1Enter()   
    {
        but1.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }
    public void OnPointerbut2Enter()
    {
        but2.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }
    public void OnPointerbut3Enter()
    {
        but3.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }
    public void OnPointerbut4Enter()
    {
        but4.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }
    //ボタンが離れた時の処理
    public void OnPointerExit()
    {
        but1.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        but2.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        but3.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        but4.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}