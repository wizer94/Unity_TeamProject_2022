using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public Text timeText;   //クリアタイム
    public Text defeatText; //撃破数
    private Text but1, but2;
    [SerializeField] Text closeText, continueText;
    public GameObject finishinfo, restartinfo;
    public GameObject player, reticle;

    [SerializeField] GameObject getWeaponObject;
    [SerializeField] Text weaponName;
    [SerializeField] Image outlineImage, weaponImage;

    public static float playTime;
    public static int defeatNum;

    private int hour, minute, second;

    // Start is called before the first frame update
    void Start()
    {
        timeText = GameObject.Find("Canvas/ClearTime").GetComponent<Text>();
        defeatText = GameObject.Find("Canvas/NumDefeats").GetComponent<Text>();
        but1 = GameObject.Find("Canvas/Return").GetComponent<Text>();
        but2 = GameObject.Find("Canvas/GameEnd").GetComponent<Text>();

        player = GameObject.FindGameObjectWithTag("player");
        reticle = GameObject.FindGameObjectWithTag("Reticle");

        defeatNum = PlayerStat.enemy_cnt;
        playTime = PlayerStat.ExistTime;

        if (defeatNum >= 1000000)
            defeatNum = 9999999;
        if (playTime >= 3600000)
            playTime = 3599999f;

        hour = (int)playTime / 3600;
        minute = (int)playTime / 60 % 60;
        second = (int)playTime % 60;
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = "Clear Time : " + hour.ToString("d3") + ":" + minute.ToString("d2") + ":" + second.ToString("d2");
        defeatText.text = "撃破数 : " + defeatNum.ToString();

        Cursor.visible = true;
        Time.timeScale = 0;
    }

	private void OnEnable() {
		if (player) {
            WeaponManager wm = player.GetComponent<WeaponManager>();
            int num = wm.ReleaseWeaponRandom();
            if(num == -1) {
                getWeaponObject.SetActive(false);
                return;
			}
			else {
                getWeaponObject.SetActive(true);
                weaponName.text = wm.guns[num].GetComponent<WeaponScript>().weaponName;
                Sprite weaponSprite = wm.guns[num].GetComponent<SpriteRenderer>().sprite;
                weaponImage.sprite = weaponSprite;
                outlineImage.sprite = weaponSprite;
            }
		}
    }

	public void OnClickClose() {
        gameObject.SetActive(false);
	}

    public void OnClickContinue() {
        if(StaticVariable.Level < 10)
            StaticVariable.Level++;
        player.transform.position = new Vector3(10, -10, player.transform.position.z);
        string sceneName = "GameScene01";
        player.GetComponent<UI>().PlUI.setMapUI(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void RestartButtonDown()
    {
        restartinfo.SetActive(true);
    }
    public void RestartYesButtonDown()
    {
        //以下 削除するオブジェクトを追加する
        player.SetActive(false);
        reticle.SetActive(false);
        //---------------------------------------------------
        SceneManager.LoadScene("Title");
    }

    public void RestartNoButtonDown()
    {
        restartinfo.SetActive(false);
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

    public void OnPointerbut1Enter()
    {
        but1.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }
    public void OnPointerbut2Enter()
    {
        but2.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }

    public void OnPointerEnter(Text target) {
        target.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }

    public void OnPointerExit()
    {
        but1.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        but2.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        closeText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        continueText.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
    }
}
