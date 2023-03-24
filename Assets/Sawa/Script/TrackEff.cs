using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackEff : MonoBehaviour
{
    [SerializeField] GameObject TrackObj;
    [SerializeField] Vector2 OffsetPos;

    ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //追尾先の存在を確かめる
        if(TrackObj != null)
        {
            particle.transform.position = (Vector2)TrackObj.transform.position + OffsetPos;
        }
        else
        {
            //追尾先がないなら消す
            Destroy(this.gameObject);
        }
    }

    public void TurnEff(bool isFlip, float ScaleX)
    {
        //エフェクトを反転
        particle.transform.localScale = new Vector3(ScaleX, 1, 1);

        //Offset値の符号を変える
        OffsetPos.x *= -1;
    }

    //エフェクトの表示変更
    public void setActive(bool Flag)
    {
        if (!Flag)
        {
            particle.Stop();
        }
        else
        {
            particle.Play();
        }
    }

    public void setTackObj(GameObject obj)
    {
        TrackObj = obj;
    } 
}
