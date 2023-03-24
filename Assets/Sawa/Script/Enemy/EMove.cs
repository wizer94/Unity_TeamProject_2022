using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------------------------
//敵　移動クラス
//-----------------------------------------------------------------

public class EMove
{
    public enum Angle      //方向
    {
        Non,
        Left,
        Right,
        Up,
        Down,
        RightUp,
        LeftUp,
        RightDown,
        LeftDown
    }

    Angle MoveAngle = Angle.Non;     //移動方向

    bool DireChangeFlag = true;    //方向変更フラグ
    Angle[] NoMoveAngle = new Angle[4];   //移動できない方向を代入する

    //移動方向テーブル
    [SerializeField] int[] randTable = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };
    int cnt = 0;

    //-----------------------------------------------------------------
    //このクラスの初期化メソッド（使用時最初に呼ぶ）
    public void EMoveInitialize()
    {
        ShuffleDirTable();
    }

    //敵　行動関数-----------------------------------------------------------------------
    public void AimPlayer()    //プレイヤに向かって移動
    {
        //プレイヤに向かって移動（ナビメッシュ）
        //処理なし（個々で行う）
    }

    //-----------------------------------------------------------------
    public Vector2 Search(float moveSpeed, bool[] NoMoveAngle)       //索敵
    {
        Vector2 vec = Vector2.zero;

        //移動可能な方向を探す
        Angle[] schAngle = new Angle[8] {    //四方向
            Angle.Non, Angle.Non, Angle.Non, Angle.Non,
            Angle.Non, Angle.Non, Angle.Non, Angle.Non,
        };
        //移動不可方向
        Angle[] NoAngle = new Angle[8] {    //四方向
            Angle.Non, Angle.Non, Angle.Non, Angle.Non,
            Angle.Non, Angle.Non, Angle.Non, Angle.Non,
        };

        //移動方向が移動可能か不可か調べる
        //左----------------------------------------
        if (NoMoveAngle[0])
            schAngle[0] = Angle.Left;
        else
            NoAngle[0] = Angle.Left;
        //右----------------------------------------
        if (NoMoveAngle[1])
            schAngle[1] = Angle.Right;
        else
            NoAngle[1] = Angle.Right;
        //上----------------------------------------
        if (NoMoveAngle[2])
        {
            //上・右上・左上全て
            schAngle[2] = Angle.Up;
            schAngle[4] = Angle.RightUp;
            schAngle[5] = Angle.LeftUp;
        }
        else
        {
            //上・右上・左上全て
            NoAngle[2] = Angle.Up;
            NoAngle[4] = Angle.RightUp;
            NoAngle[5] = Angle.LeftUp;
        }
        //下----------------------------------------
        if (NoMoveAngle[3])
        {
            schAngle[3] = Angle.Down;
            schAngle[6] = Angle.RightDown;
            schAngle[7] = Angle.LeftDown;
        }
        else
        {
            NoAngle[3] = Angle.Down;
            NoAngle[6] = Angle.RightDown;
            NoAngle[7] = Angle.LeftDown;
        }

        //移動方向変更　||　壁と接触した場合
        if (DireChangeFlag)
        {
            //ランダムで移動方向を決める
          
            //次の移動方向がNonでないなら
            if(schAngle[randTable[cnt]] != Angle.Non)
            {
                //移動方向を保存
                MoveAngle = schAngle[randTable[cnt]];

                //方向を決めたのでフラグを戻す
                setMoveDir(false);
            }
            cnt++;
            if(cnt >= 8)
            {
                cnt = 0;
                //ランダムテーブルが最後まで来たら配列の中身を入れ替える
                ShuffleDirTable();
            }
        }
        //一定時間移動
        else
        {
            //八方向を確認する
            for(int i= 0; i < 8; ++i)
            {
                //移動方向,進行不可方向の方向が同じなら
                if (MoveAngle == NoAngle[i])
                {
                    //移動方向を変更するフラグを立てる
                    setMoveDir(true);

                    //このメソッドを抜ける
                    return vec;
                }
            }

            //決めた方向に移動
            switch (MoveAngle)
            {
                //左------------------------------------------
                case Angle.Left:
                    vec = GetMove(180, moveSpeed);
                    break;
                //右------------------------------------------
                case Angle.Right:
                    vec = GetMove(0, moveSpeed);
                    break;
                //上------------------------------------------
                case Angle.Up:
                    vec = GetMove(90, moveSpeed);
                    break;
                //下------------------------------------------
                case Angle.Down:
                    vec = GetMove(270, moveSpeed);
                    break;
                //右上------------------------------------------
                case Angle.RightUp:
                    vec = GetMove(45, moveSpeed);
                    break;
                //左上------------------------------------------
                case Angle.LeftUp:
                    vec = GetMove(135, moveSpeed);
                    break;
                //右下------------------------------------------
                case Angle.RightDown:
                    vec = GetMove(315, moveSpeed);
                    break;
                //左下------------------------------------------
                case Angle.LeftDown:
                    vec = GetMove(225, moveSpeed);
                    break;
            }
        }

        return vec;
    }

    //-----------------------------------------------------------------
    public Vector2 Stop()         //移動停止
    {
        Vector2 vec = Vector2.zero;

        return vec;
    }

    //-----------------------------------------------------------------
    public Vector2 Escape(GameObject player,GameObject me,float moveSpeed)       //にげる
    {
        //プレイヤの反対に逃げる
        Vector2 vec = Vector2.zero;

        //プレイヤと自身の位置から逃げる方向を決める
        //角度を求める
        Vector2 dt = me.transform.position - player.transform.position;
        float rad = Mathf.Atan2(dt.y, dt.x);

        vec = GetMove(to_deg(rad), moveSpeed);

        return vec;
    }
    //-----------------------------------------------------------------
    public Vector2 TakeDistance(GameObject player, GameObject me, float moveSpeed)
    {
        //プレイヤの反対に逃げる
        Vector2 vec = Vector2.zero;

        //プレイヤと自身の位置から逃げる方向を決める
        //角度を求める
        Vector2 dt = me.transform.position - player.transform.position;
        float rad = Mathf.Atan2(dt.y, dt.x);

        vec = GetMove(to_deg(rad), moveSpeed);

        return vec;
    }
    //-----------------------------------------------------------------
    public Vector2 Rush(Vector3 RushPoint, GameObject me, float moveSpeed)
    {
        //プレイヤの反対に逃げる
        Vector2 vec = Vector2.zero;

        //プレイヤと自身の位置から逃げる方向を決める
        //角度を求める
        Vector2 dt = RushPoint - me.transform.position;
        float r = Mathf.Sqrt(dt.x * dt.x + dt.y * dt.y);

        vec = new Vector2(dt.x / 10, dt.y / 10);
        
        return vec;
    }
    //-----------------------------------------------------------------
    public Vector2 Avoidance()    //回避
    {
        //回避行動後壁と当たる場合もある
        Vector2 vec = Vector2.zero;
        int rn = Random.Range(0, 2);
        //左右ランダムで回避する
        if(rn == 0)
        {
            vec = new Vector2(500, 0);
        }
        else
        {
            vec = new Vector2(-500, 0);
        }

        return vec;
    }

    //-----------------------------------------------------------------
    public Vector2 SearchDrone(string dir,float moveSpeed)  //ドローン用の移動(一定ルート周回)
    {
        //一定ルートを周回
        Vector2 vec = Vector2.zero;

        switch (dir)
        {
            //左-----------------------------------
            case "←":
                vec = GetMove(180, moveSpeed);
                break;
            //右-----------------------------------
            case "→":
                vec = GetMove(0, moveSpeed);
                break;
            //上-----------------------------------
            case "↑":
                vec = GetMove(90, moveSpeed);
                break;
            //下-----------------------------------
            case "↓":
                vec = GetMove(270, moveSpeed);
                break;
            //左上---------------------------------
            case "左上":
                vec = GetMove(155, moveSpeed);
                break;
            //右上---------------------------------
            case "右上":
                vec = GetMove(30, moveSpeed);
                break;
            //左下---------------------------------
            case "左下":
                vec = GetMove(210, moveSpeed);
                break;
            //左下---------------------------------
            case "右下":
                vec = GetMove(335, moveSpeed);
                break;
            //それ以外
            default:

                break;
        }

        return vec;
    }
    //-----------------------------------------------------------------
    public Vector2 ReturnDrone(Vector2 me, Vector2 target,float moveSpeed)
    {
        //引数の目的地に移動する
        Vector2 rtv = Vector2.zero;

        Vector2 dt = target - me;
        float temp = dt.x * dt.x + dt.y * dt.y;
        float r = Mathf.Sqrt(temp);

        rtv.x = dt.x / r * moveSpeed;
        rtv.y = dt.y / r * moveSpeed;

        return rtv;
    }

    //setter・getter
    //-----------------------------------------------------------------
    public void setNoMoveAngle(Angle ang,int num)
    {
        NoMoveAngle[num] = ang;

        Debug.Log(NoMoveAngle[num]);
    }
    //-----------------------------------------------------------------
    public void setMoveDir(bool Flag)
    {
        //移動方向の再抽選
        DireChangeFlag = Flag;
    }
    //-----------------------------------------------------------------
    public Angle getAngle()
    {
        return MoveAngle;
    }

    //関数群
    //---------------------------------------------------------------------------------------------------------
    //度数法をラジアンに変換するメソッド
    public float To_rad(float deg)      //度数法をラジアンに変換する
    {
        //ラジアンに変換して返す
        return (deg * Mathf.PI) / 180.0f;
    }
    //---------------------------------------------------------------------------------------------------------
    //ラジアンを度数法に変換するメソッド
    public float to_deg(float ang)     //ラジアンから度数法へ変換
    {
        return (ang * 180.0f) / Mathf.PI;
    }
    //移動量を返すメソッド---------------------------------------------------------------------------
    public Vector2 GetMove(float deg,float Speed)    //引数の角度から移動量を返す
    {
        //移動量を求める
        Vector2 move;
        move.x = Mathf.Cos(To_rad(deg)) * Speed;
        move.y = Mathf.Sin(To_rad(deg)) * Speed;

        return move;
    }
    //-----------------------------------------------------------------
    //randTableを入れ替える関数
    void ShuffleDirTable()
    {
        for (int i = 0; i < 8; ++i)
        {
            int rnd = Random.Range(0, 8);

            int temp = randTable[i];
            randTable[i] = randTable[rnd];
            randTable[rnd] = temp;
        }
    }
    //-----------------------------------------------------------------
    //-----------------------------------------------------------------
    //-----------------------------------------------------------------
}
