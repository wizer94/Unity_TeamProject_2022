using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    float fadeSpeed = 0.02f;                //フェードスピード管理
    float alfa;                             //透明度管理

    public bool isFadeIn = false;           //フェードイン処理の状態管理
    public bool isFadeOut = false;          //フェードアウト処理の状態管理

    private GameObject pauseUIInstance;

    Image fadeImage;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        fadeImage = GetComponent<Image>();
        alfa = fadeImage.color.a;
        player = GameObject.Find("player");
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeIn == true || isFadeOut == true)
        {
            Time.timeScale = 0.0f;
        }
        if (isFadeIn && isFadeOut)
            isFadeOut = false;
        if(isFadeIn) {
            StartFadeIn();
        }
        if (isFadeOut)
        {
            StartFadeOut();
        }
    }
    void StartFadeIn() {

        alfa -= fadeSpeed;
        SetAlpha();
        if(alfa <= 0.0f)
        {
            isFadeIn = false;
            fadeImage.enabled = false;
            Time.timeScale = 1;
        }
    }
    void StartFadeOut()
    {
        fadeImage.enabled = true;
        alfa += fadeSpeed;
        SetAlpha();
        if(alfa >= 1.0f)
        {
            isFadeOut = false;
            player.GetComponent<scene>().FadeOver = true;
        }
    }
    void SetAlpha()
    {
        fadeImage.color = new Color(0, 0, 0, alfa);
    }
}
