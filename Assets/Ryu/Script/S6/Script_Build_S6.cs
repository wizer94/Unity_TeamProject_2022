using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Build_S6 : MonoBehaviour
{
    public GameObject mapPrefab_0;//コライダー無し
    public GameObject mapPrefab_1;
    public GameObject mapPrefab_2;
    public GameObject mapPrefab_3;
    public GameObject mapPrefab_4;
    public GameObject mapPrefab_5;
    public GameObject mapPrefab_6;
    public GameObject mapPrefab_7;
    public GameObject mapPrefab_8;
    public GameObject mapPrefab_9;
    public GameObject mapPrefab_10;
    public GameObject mapPrefab_11;

    public GameObject mapPrefab_12;//コライダー付き
    public GameObject mapPrefab_13;
    public GameObject mapPrefab_14;
    public GameObject mapPrefab_15;
    public GameObject mapPrefab_16;
    public GameObject mapPrefab_17;
    public GameObject mapPrefab_18;
    public GameObject mapPrefab_19;
    public GameObject mapPrefab_20;
    public GameObject mapPrefab_21;
    public GameObject mapPrefab_22;
    float MapChipSize_X = 1.8f;         //チップの横の大きさ
    float MapChipSize_Y = 0.9f;
    int MapSize_X = 36;                //横のチップ枚数
    int MapSize_Y = 36;
    int Start_X = -32;                  //横の原点
    int Start_Y = 16;
    float Set_Y = 0.6f;
    int[,] mapData =
    {
        { 00,00,00,00,00,00,04,00,00,00,00,00,22,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,04,00,00,00,00,00,22,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,04,00,00,00,00,00,22,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 04,00,00,00,00,00,22,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,22,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,22,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 22,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,16,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,16,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,16,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
        { 00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00},
    };
    // Start is called before the first frame update
    void Start()
    {
        for (int y = 0; y < MapSize_Y; ++y)
        {
            for (int x = 0; x < MapSize_X; ++x)
            {
                if (x % 2 == 0 && y % 2 == 0)
                {
                    Vector3 pos = new Vector3(x * MapChipSize_X + Start_X, y * -MapChipSize_Y + Start_Y + Set_Y, 0);
                    GameObject go;
                    switch (mapData[y, x])
                    {
                        case 0:
                            //go = Instantiate(mapPrefab_0, pos, Quaternion.identity) as GameObject;
                            break;
                        case 1:
                            go = Instantiate(mapPrefab_1, pos, Quaternion.identity) as GameObject;
                            break;
                        case 2:
                            go = Instantiate(mapPrefab_2, pos, Quaternion.identity) as GameObject;
                            break;
                        case 3:
                            go = Instantiate(mapPrefab_3, pos, Quaternion.identity) as GameObject;
                            break;
                        case 4:
                            go = Instantiate(mapPrefab_4, pos, Quaternion.identity) as GameObject;
                            break;
                        case 5:
                            go = Instantiate(mapPrefab_5, pos, Quaternion.identity) as GameObject;
                            break;
                        case 6:
                            go = Instantiate(mapPrefab_6, pos, Quaternion.identity) as GameObject;
                            break;
                        case 7:
                            go = Instantiate(mapPrefab_7, pos, Quaternion.identity) as GameObject;
                            break;
                        case 8:
                            go = Instantiate(mapPrefab_8, pos, Quaternion.identity) as GameObject;
                            break;
                        case 9:
                            go = Instantiate(mapPrefab_9, pos, Quaternion.identity) as GameObject;
                            break;
                        case 10:
                            go = Instantiate(mapPrefab_10, pos, Quaternion.identity) as GameObject;
                            break;
                        case 11:
                            go = Instantiate(mapPrefab_11, pos, Quaternion.identity) as GameObject;
                            break;
                        case 12:
                            go = Instantiate(mapPrefab_12, pos, Quaternion.identity) as GameObject;
                            break;
                        case 13:
                            go = Instantiate(mapPrefab_13, pos, Quaternion.identity) as GameObject;
                            break;
                        case 14:
                            go = Instantiate(mapPrefab_14, pos, Quaternion.identity) as GameObject;
                            break;
                        case 15:
                            go = Instantiate(mapPrefab_15, pos, Quaternion.identity) as GameObject;
                            break;
                        case 16:
                            go = Instantiate(mapPrefab_16, pos, Quaternion.identity) as GameObject;
                            break;
                        case 17:
                            go = Instantiate(mapPrefab_17, pos, Quaternion.identity) as GameObject;
                            break;
                        case 18:
                            go = Instantiate(mapPrefab_18, pos, Quaternion.identity) as GameObject;
                            break;
                        case 19:
                            go = Instantiate(mapPrefab_19, pos, Quaternion.identity) as GameObject;
                            break;
                        case 20:
                            go = Instantiate(mapPrefab_20, pos, Quaternion.identity) as GameObject;
                            break;
                        case 21:
                            go = Instantiate(mapPrefab_21, pos, Quaternion.identity) as GameObject;
                            break;
                        case 22:
                            go = Instantiate(mapPrefab_22, pos, Quaternion.identity) as GameObject;
                            break;
                    }
                }                       //偶数行のチップ生成
                else if (x % 2 != 0 && y % 2 != 0)
                {
                    Vector3 pos = new Vector3(x * MapChipSize_X + Start_X, y * -MapChipSize_Y + Start_Y + Set_Y, 0);
                    GameObject go;
                    switch (mapData[y, x])
                    {
                        case 0:
                            //go = Instantiate(mapPrefab_0, pos, Quaternion.identity) as GameObject;
                            break;
                        case 1:
                            go = Instantiate(mapPrefab_1, pos, Quaternion.identity) as GameObject;
                            break;
                        case 2:
                            go = Instantiate(mapPrefab_2, pos, Quaternion.identity) as GameObject;
                            break;
                        case 3:
                            go = Instantiate(mapPrefab_3, pos, Quaternion.identity) as GameObject;
                            break;
                        case 4:
                            go = Instantiate(mapPrefab_4, pos, Quaternion.identity) as GameObject;
                            break;
                        case 5:
                            go = Instantiate(mapPrefab_5, pos, Quaternion.identity) as GameObject;
                            break;
                        case 6:
                            go = Instantiate(mapPrefab_6, pos, Quaternion.identity) as GameObject;
                            break;
                        case 7:
                            go = Instantiate(mapPrefab_7, pos, Quaternion.identity) as GameObject;
                            break;
                        case 8:
                            go = Instantiate(mapPrefab_8, pos, Quaternion.identity) as GameObject;
                            break;
                        case 9:
                            go = Instantiate(mapPrefab_9, pos, Quaternion.identity) as GameObject;
                            break;
                        case 10:
                            go = Instantiate(mapPrefab_10, pos, Quaternion.identity) as GameObject;
                            break;
                        case 11:
                            go = Instantiate(mapPrefab_11, pos, Quaternion.identity) as GameObject;
                            break;
                        case 12:
                            go = Instantiate(mapPrefab_12, pos, Quaternion.identity) as GameObject;
                            break;
                        case 13:
                            go = Instantiate(mapPrefab_13, pos, Quaternion.identity) as GameObject;
                            break;
                        case 14:
                            go = Instantiate(mapPrefab_14, pos, Quaternion.identity) as GameObject;
                            break;
                        case 15:
                            go = Instantiate(mapPrefab_15, pos, Quaternion.identity) as GameObject;
                            break;
                        case 16:
                            go = Instantiate(mapPrefab_16, pos, Quaternion.identity) as GameObject;
                            break;
                        case 17:
                            go = Instantiate(mapPrefab_17, pos, Quaternion.identity) as GameObject;
                            break;
                        case 18:
                            go = Instantiate(mapPrefab_18, pos, Quaternion.identity) as GameObject;
                            break;
                        case 19:
                            go = Instantiate(mapPrefab_19, pos, Quaternion.identity) as GameObject;
                            break;
                        case 20:
                            go = Instantiate(mapPrefab_20, pos, Quaternion.identity) as GameObject;
                            break;
                        case 21:
                            go = Instantiate(mapPrefab_21, pos, Quaternion.identity) as GameObject;
                            break;
                        case 22:
                            go = Instantiate(mapPrefab_22, pos, Quaternion.identity) as GameObject;
                            break;
                    }
                }                       //奇数行のチップ生成
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
