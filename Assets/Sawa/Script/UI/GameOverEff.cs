using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverEff : MonoBehaviour
{
    [SerializeField] Image GOEff;
    [SerializeField] Text text;
    bool isPlay;    //エフェクト再生フラグ

    float alpha = 0;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "GameOver";

        GOEff.color = new Color(1, 1, 1, 0);
        text.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        addAlpha();
    }

    //徐々にalphaを足す
    void addAlpha()
    {
        alpha += Time.deltaTime;
        GOEff.color = new Color(1, 1, 1, alpha);
        text.color = new Color(1, 1, 1, alpha);
    }

    public void setIsPlay(bool Flag)
    {
        isPlay = Flag;

        GOEff.enabled = true;
        text.enabled = true;
    }
    public bool getIsPlay()
    {
        return isPlay;
    }
}
