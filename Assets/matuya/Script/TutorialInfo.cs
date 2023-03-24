using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInfo : MonoBehaviour
{
    GameObject player;
    GameObject me;
    Image img;
    public Sprite[] sp;
    Text text, title;

    bool click_check;

    string[] ttext = { "セーフエリア", "武器の切り替え", "PlayerChip", "WeaponChip", "WeaponChipの強化" };
    string[] etext = {         
        "敵からの攻撃を受けないエリアです。\nまた、失った体力を回復できます。",
        "セーフエリア内でインベントリの武器を左クリックすることで武器の切り替えが行えます。\nメイン、サブ両方とも切り替えることができます。",
        "自分自身を強化できるチップです。\nインベントリからドラッグ&ドロップでつけることができます。\n５つまでつけることができます。",
        "武器を強化できるチップです。\nインベントリからドラッグ&ドロップでつけることができます。\n５つまでつけることができます。",
        "インベントリから同じチップの同じレベルを重ねることでチップのレベルを強化できます。\n１レベルから最大５レベルまであります。"
    };
    bool flag;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        me = GameObject.Find("TutorialCanvas");
        img = GameObject.Find("GB/Image").GetComponent<Image>();
        title = GameObject.Find("GB/TextInfo/Titile").GetComponent<Text>();
        text = GameObject.Find("GB/TextInfo/Text").GetComponent<Text>();

        click_check = false;

        me.SetActive(false);

        Invoke("TutorialDamage", 0.1f);
    }

    public void TutorialDamage()
    {
        player.GetComponent<PlayerController>().HitDamage(50f);
    }

    private void Update()
    {
        this.transform.position = player.transform.position;

        NextText();
    }

    void NextText()
	{
        if (title.text != null)
        {
            if (title.text == ttext[0])
            {
                Time.timeScale = 0.0f;
                if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    title.text = ttext[1];
                    text.text = etext[1];
                    img.sprite = sp[1];
                }
            }
            if (title.text == ttext[1])
            {
                Time.timeScale = 0.0f;
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    title.text = null;
                    text.text = null;
                    img.sprite = null;
                    me.SetActive(false);

                    Time.timeScale = 1.0f;
                }
            }

            if (title.text == ttext[3])
            {
                Time.timeScale = 0.0f;
                if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    title.text = ttext[4];
                    text.text = etext[4];
                    img.sprite = sp[4];
                }
            }
            if (title.text == ttext[2] || title.text == ttext[4])
            {
                Time.timeScale = 0.0f;
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    title.text = null;
                    text.text = null;
                    img.sprite = null;
                    me.SetActive(false);

                    Time.timeScale = 1.0f;
                }
            }
		}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //セーブエリアに触れたら
        if (collision.gameObject.CompareTag("SavePointT"))
        {
            me.SetActive(true);
            title.text = ttext[0];
            text.text = etext[0];
            img.sprite = sp[0];
        }
        //PlayerChipに触れたら
        if (collision.CompareTag("PlayerChipT"))
        {
            me.SetActive(true);
            title.text = ttext[2];
            text.text = etext[2];
            img.sprite = sp[2];
        }
        //WeaponChipに触れたら
        if (collision.CompareTag("WeaponChipT"))
        {
            me.SetActive(true);
            title.text = ttext[3];
            text.text = etext[3];
            img.sprite = sp[3];
            //Time.timeScale = 0.0f;
        }
    }
}
