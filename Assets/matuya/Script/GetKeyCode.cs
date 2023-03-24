using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class GetKeyCode : MonoBehaviour
{
    [System.Serializable]
    public class Testdata1
    {
        public int id;
        public int keyint;
        public string name;
        public string keycode;
    }

    [System.Serializable]  //JSONデータに変換できるようにする
    public class TestData
    {
        public Testdata1[] data;
    }
    TestData testjson;
    // Start is called before the first frame update
    void Start()
    {
        Awake();
    }
    //起動時の読み込み処理
    public void Awake()
    {
        //AssetDatabase.Refresh();
        string test = Resources.Load<TextAsset>("Json/KeyCode").ToString();
        testjson = JsonUtility.FromJson<TestData>(test);
        LoadKey();
    }
    //保存の処理
    public void SaveKeyCode(TestData test)
    {
        StreamWriter writer;
        string jsonstr = JsonUtility.ToJson(test);
        writer = new StreamWriter(Application.dataPath + "/Resources/Json/KeyCode.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }
    public void GetData(Text Key, int i, int keynum)
    {
        testjson.data[i].keycode = Key.text;
        testjson.data[i].keyint = keynum;
    }
    public string LoadText(int i)
    {
        return testjson.data[i].keycode;
    }
    public string Loadname(int i)
    {
        return testjson.data[i].name;
    }
    public void LoadKey()
    {
        KeyScript.Up =          (KeyCode)testjson.data[0].keyint;
        KeyScript.Down =        (KeyCode)testjson.data[1].keyint;
        KeyScript.Left =        (KeyCode)testjson.data[2].keyint;
        KeyScript.Right =       (KeyCode)testjson.data[3].keyint;
        KeyScript.Dash =        (KeyCode)testjson.data[4].keyint;
        KeyScript.Dodge =       (KeyCode)testjson.data[5].keyint;
        KeyScript.Fire =        (KeyCode)testjson.data[6].keyint;
        KeyScript.Aim =         (KeyCode)testjson.data[7].keyint;
        KeyScript.Reload =      (KeyCode)testjson.data[8].keyint;
        KeyScript.Weapon1 =     (KeyCode)testjson.data[9].keyint;
        KeyScript.Weapon2 =     (KeyCode)testjson.data[10].keyint;
        KeyScript.Switch =      (KeyCode)testjson.data[11].keyint;
        KeyScript.Action =      (KeyCode)testjson.data[12].keyint;
        KeyScript.Inventory =   (KeyCode)testjson.data[13].keyint;
        KeyScript.Map =         (KeyCode)testjson.data[14].keyint;

    }
    public void SaveText()
    {
        LoadKey();
        SaveKeyCode(testjson);
    }
}
