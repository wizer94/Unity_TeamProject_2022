using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Inventory;

public class scene : MonoBehaviour
{
    GameObject fade;

    public GameObject Inventory;
    public InventoryManager i_M;
    public static bool Stage_start;

    public bool FadeOver = false;

    // Start is called before the first frame update
    void Start()
    {
        Stage_start = false;
        i_M = Inventory.GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!fade)
            fade = GameObject.FindGameObjectWithTag("fade");
    }

    //シーン遷移とプレイヤー位置指定
    void T_To_S1()
    {
        Stage_start = true;
        i_M.RemoveItem();

        SceneManager.LoadScene("GameScene01");
        StaticVariable.Tutorialed = true;

        PlayerGenerateScript.player.transform.localScale = Vector3.one;
        PlayerGenerateScript.player.GetComponent<WeaponManager>().RecreateGuns();

        StaticBgmScript.PlayStage();

        Vector3 tmp = GameObject.Find("player").transform.position;
		GameObject.Find("player").transform.position = new Vector3(10, -10, tmp.z);
        Fader();
    }
    void S1_To_S4()
    {
        SceneManager.LoadScene("GameScene04");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(3, -12, tmp.z);
        Fader();
    }
    void S1_To_S5()
    {
        SceneManager.LoadScene("GameScene05");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(-25.5f, -10.7f, tmp.z);
        Fader();
    }
    void S3_To_S6()
    {
        SceneManager.LoadScene("GameScene06");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(26, -13, tmp.z);
        Fader();
    }
    void S4_To_S1()
    {
        SceneManager.LoadScene("GameScene01");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(3, 14.5f, tmp.z);
        Fader();
    }
    void S4_To_S5()
    {
        SceneManager.LoadScene("GameScene05");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(-27, 5, tmp.z);
        Fader();
    }
    void S5_To_S4()
    {
        SceneManager.LoadScene("GameScene04");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(27, 6, tmp.z);
        Fader();
    }
    void S5_To_S1()
    {
        SceneManager.LoadScene("GameScene01");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(23.5f, 13.5f, tmp.z);
        Fader();
    }
    void S5_To_S8()
    {
        SceneManager.LoadScene("GameScene08");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(-4.5f, -12, tmp.z);
        Fader();
    }
    void S5_To_S6()
    {
        SceneManager.LoadScene("GameScene06");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(-27, -3.5f, tmp.z);
        Fader();
    }
    void S6_To_S5()
    {
        SceneManager.LoadScene("GameScene05");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(26, -5, tmp.z);
        Fader();
    }
    void S6_To_S3()
    {
        SceneManager.LoadScene("GameScene03");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(23, 14, tmp.z);
        Fader();
    }
    void S8_To_S5()
    {
        SceneManager.LoadScene("GameScene05");
        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(-10, 14, tmp.z);
        Fader();
    }
    void S6_To_S9()
    {
        SceneManager.LoadScene("GameScene09");

        StaticBgmScript.PlayBoss();

        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(12, -10, tmp.z);
        Fader();
    }
    void S8_To_S9()
    {
        SceneManager.LoadScene("GameScene09");

        StaticBgmScript.PlayBoss();

        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(-26, 6, tmp.z);
        Fader();
    }
    void S9_To_S8()
    {
        SceneManager.LoadScene("GameScene08");

        StaticBgmScript.PlayStage();

        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(27, 3, tmp.z);
        Fader();
    }
    void S9_To_S6()
    {
        SceneManager.LoadScene("GameScene06");

        StaticBgmScript.PlayStage();

        Vector3 tmp = GameObject.Find("player").transform.position;
        GameObject.Find("player").transform.position = new Vector3(10, 9, tmp.z);
        Fader();
    }

    void Fader() {
        //if (fade)
            fade.GetComponent<FadeController>().isFadeIn = true;
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        //次に移動するシーン名
        string NextSceneName = "";


        //Tutorial
        if (other.gameObject.tag == "enter_T")
        {
            NextSceneName = "GameScene01";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("T_To_S1", 0.01f);
        }
        //S1
        if (other.gameObject.tag == "enterS1_T")
        {
            NextSceneName = "GameScene04";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S1_To_S4", 0.01f);
        }
        if (other.gameObject.tag == "enterS1_TR")
        {
            NextSceneName = "GameScene05";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S1_To_S5", 0.01f);
        }
        //S3
        if (other.gameObject.tag == "enterS3_T")
        {
            NextSceneName = "GameScene06";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S3_To_S6", 0.01f);
        }
        //S4
        if (other.gameObject.tag == "enterS4_B")
        {
            NextSceneName = "GameScene01";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S4_To_S1", 0.01f);
        }
        if (other.gameObject.tag == "enterS4_R")
        {
            NextSceneName = "GameScene05";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S4_To_S5", 0.01f);
        }
        //S5
        if (other.gameObject.tag == "enterS5_L")
        {
            NextSceneName = "GameScene04";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S5_To_S4", 0.01f);

        }
        if (other.gameObject.tag == "enterS5_LB")
        {
            NextSceneName = "GameScene01";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S5_To_S1", 0.01f);

        }
        if (other.gameObject.tag == "enterS5_T")
        {
            NextSceneName = "GameScene08";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S5_To_S8", 0.01f);
        }
        if (other.gameObject.tag == "enterS5_R")
        {
            NextSceneName = "GameScene06";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S5_To_S6", 0.01f);
        }
        //S6
        if (other.gameObject.tag == "enterS6_L")
        {
            NextSceneName = "GameScene05";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S6_To_S5", 0.01f);
        }
        if (other.gameObject.tag == "enterS6_B")
        {
            NextSceneName = "GameScene03";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S6_To_S3", 0.01f);
        }
        if (other.gameObject.tag == "enterS6_T")
        {
            NextSceneName = "GameScene09";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S6_To_S9", 0.01f);
        }
        //S8
        if (other.gameObject.tag == "enterS8_B")
        {
            NextSceneName = "GameScene05";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S8_To_S5", 0.01f);
        }
        if (other.gameObject.tag == "enterS8_R")
        {
            NextSceneName = "GameScene09";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S8_To_S9", 0.01f);
        }
        //S9
        if (other.gameObject.tag == "enterS9_L")
        {
            NextSceneName = "GameScene08";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S9_To_S8", 0.01f);
        }
        if (other.gameObject.tag == "enterS9_B")
        {
            NextSceneName = "GameScene06";

            fade.GetComponent<FadeController>().isFadeOut = true;
            Invoke("S9_To_S6", 0.01f);
        }

        //マップUIを変更する
        if (NextSceneName != "")
        {
            GameObject Pl_UI = GameObject.FindGameObjectWithTag("player").transform.Find("PlayerUI").gameObject;
            Pl_UI.GetComponent<PlayerUI>().setMapUI(NextSceneName);
        }
    }
}
