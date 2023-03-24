using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カメラ

public class CameraScript : MonoBehaviour
{
    //可変長配列　敵拡張用
    [SerializeField] List<GameObject> EnemyList = new List<GameObject>();
    [SerializeField] List<NormalEnemy> EnemyScript = new List<NormalEnemy>();

    public Vector2 trailPos = Vector2.zero;

    GameObject playerObj;
    Transform playerTransform;

    [SerializeField] bool CallEnemyFlag;

    float JuTime;
    [SerializeField] float Dist_EnemyActive;  //有・無効化の距離

    public Vector2 mousePos;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("player");
        playerTransform = playerObj.transform;
        mainCamera = GetComponent<Camera>();

        //初めからシーンにいる敵をリストに登録
        RegisterEnemy();
        //カメラ外判定
        CheckEnemy();

        CallEnemyFlag = false;

        JuTime = 0;
    }
    //-------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        //一定時間ごとに
        if(JuTime >= 0.2f)
        {
            //カメラ外判定
            CheckEnemy();

            JuTime = 0;
        }

        JuTime += Time.deltaTime;
    }

    void LateUpdate()
    {
        MoveCamera(trailPos);
        mousePos = GetMousePos();
    }


    //-------------------------------------------------------------------------
    private Vector3 GetMousePos()
    {
        Vector2 inp = Input.mousePosition;
        return mainCamera.ScreenToWorldPoint(new Vector3(inp.x, inp.y, 10));
    }

    //-------------------------------------------------------------------------
    public int RegisterEnemy() {
        GameObject[] enemyObjs = GameObject.FindGameObjectsWithTag("Enemy");
        int rtv = 0;
        foreach (GameObject enemyObj in enemyObjs) {
            GameObject enemy = enemyObj.GetComponent<EnemyImg>().enemyObj;
            EnemyList.Add(enemy);
            EnemyScript.Add(enemy.GetComponent<NormalEnemy>());
            rtv++;
        }
        return rtv;
    }
    //-------------------------------------------------------------------------
    public void CheckEnemy() {
        //非表示になっている全ての敵と距離計算を行う
        for (int i = 0; i < EnemyList.Count; ++i) {
            //すでに存在しない場合、リストから削除して次のループへ
            if (!EnemyList[i]) {
                EnemyList.RemoveAt(i);
                continue;
            }

            //カメラと距離計算を行う
            Vector2 dt = this.gameObject.transform.position - EnemyList[i].transform.position;

            float r = Mathf.Sqrt(dt.x * dt.x + dt.y * dt.y);

            if(r < Dist_EnemyActive)
            {
                EnemyScript[i].setIsSearch(true);
            }
            else
            {
                EnemyScript[i].setIsSearch(false);
            }

        }
    }
    //-------------------------------------------------------------------------
    public void AddEnemy(GameObject enemy)
    {
        //要素を追加
        EnemyList.Add(enemy);
        //スクリプトを追加
        EnemyScript.Add(enemy.GetComponent<NormalEnemy>());
    }
    //-------------------------------------------------------------------------
    //リストから指定された敵を消す
    public void RemoveEnemy(string name)
    {
        for (int i = 0; i < EnemyList.Count; ++i)
        {
            string parent = EnemyList[i].transform.parent.name;

            if (parent == name)
            {
                EnemyList.Remove(EnemyList[i]);
                EnemyScript.Remove(EnemyScript[i]);

                return;
            }
        }
    }
    //-------------------------------------------------------------------------
    public void MoveCamera(Vector2 pos)
    {
        float ortho = mainCamera.orthographicSize;
        float wphRate = (float)Screen.width / Screen.height;
        Vector2 camera_leftBottom = new Vector2(pos.x - ortho * wphRate, pos.y - ortho);
        Vector2 camera_rightTop = new Vector2(pos.x + ortho * wphRate, pos.y + ortho);
        Vector2 map_leftBottom = new Vector2(-32f, -15.5f);
        Vector2 map_rightTop = new Vector2(31f, 17f);
        if (camera_leftBottom.x < map_leftBottom.x) {
            pos = new Vector2(map_leftBottom.x + ortho * wphRate, pos.y);
            //Debug.Log("left");
        }
        if (camera_rightTop.x > map_rightTop.x) {
            pos = new Vector2(map_rightTop.x - ortho * wphRate, pos.y);
            //Debug.Log("right");
        }
        if (camera_leftBottom.y < map_leftBottom.y) {
            pos = new Vector2(pos.x, map_leftBottom.y + ortho);
            //Debug.Log("bottom");
        }
        if (camera_rightTop.y > map_rightTop.y) {
            pos = new Vector2(pos.x, map_rightTop.y - ortho);
            //Debug.Log("top");
        }

        float posx_inmap = Limit(map_leftBottom.x, pos.x, map_rightTop.x);
        float posy_inmap = Limit(map_leftBottom.y, pos.y, map_rightTop.y);
        transform.position = new Vector3(posx_inmap, posy_inmap, this.gameObject.transform.position.z);
    }
    float Limit(float min, float a, float max) {
        if (a < min) return min;
        if (a > max) return max;
        return a;
    }
    //-------------------------------------------------------------------------
    public void setCallEnemyFlag(bool Flag)
    {
        CallEnemyFlag = Flag;

        //trueがセットされたら
        if (CallEnemyFlag)
        {
            //敵を呼ぶ
            ChengeAimPlayer();
        }
    }
    public bool getCallEnemyFlag()
    {
        return CallEnemyFlag;
    }
    //-------------------------------------------------------------------------
    void ChengeAimPlayer()
    {
        //探索状態の敵を招集する
        for (int i = 0; i < EnemyList.Count; ++i)
        {
            //探索ないなら
            if (EnemyScript[i].getIsSearch())
            {
                EnemyScript[i].setState_AimPlayer();
            }
        }
    }
    //-------------------------------------------------------------------------
    //-------------------------------------------------------------------------
}
