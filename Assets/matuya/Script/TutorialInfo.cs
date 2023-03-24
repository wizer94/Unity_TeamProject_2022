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

    string[] ttext = { "�Z�[�t�G���A", "����̐؂�ւ�", "PlayerChip", "WeaponChip", "WeaponChip�̋���" };
    string[] etext = {         
        "�G����̍U�����󂯂Ȃ��G���A�ł��B\n�܂��A�������̗͂��񕜂ł��܂��B",
        "�Z�[�t�G���A���ŃC���x���g���̕�������N���b�N���邱�Ƃŕ���̐؂�ւ����s���܂��B\n���C���A�T�u�����Ƃ��؂�ւ��邱�Ƃ��ł��܂��B",
        "�������g�������ł���`�b�v�ł��B\n�C���x���g������h���b�O&�h���b�v�ł��邱�Ƃ��ł��܂��B\n�T�܂ł��邱�Ƃ��ł��܂��B",
        "����������ł���`�b�v�ł��B\n�C���x���g������h���b�O&�h���b�v�ł��邱�Ƃ��ł��܂��B\n�T�܂ł��邱�Ƃ��ł��܂��B",
        "�C���x���g�����瓯���`�b�v�̓������x�����d�˂邱�ƂŃ`�b�v�̃��x���������ł��܂��B\n�P���x������ő�T���x���܂ł���܂��B"
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
        //�Z�[�u�G���A�ɐG�ꂽ��
        if (collision.gameObject.CompareTag("SavePointT"))
        {
            me.SetActive(true);
            title.text = ttext[0];
            text.text = etext[0];
            img.sprite = sp[0];
        }
        //PlayerChip�ɐG�ꂽ��
        if (collision.CompareTag("PlayerChipT"))
        {
            me.SetActive(true);
            title.text = ttext[2];
            text.text = etext[2];
            img.sprite = sp[2];
        }
        //WeaponChip�ɐG�ꂽ��
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
