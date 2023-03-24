using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootLoad : MonoBehaviour
{
	//引用箇所

	[SerializeField] TextAsset RootData;
	Vector2 InsPos;

    public string[,] LoadRoot()
	{
		string rawData = RootData.text;
		//行ごとに分割
		string[] lineData = rawData.Split('\n');

		//初期座標の取得
		string[] PosIndex = lineData[0].Split(',');
		InsPos.x = float.Parse(PosIndex[0]);
		InsPos.y = float.Parse(PosIndex[1]);

		//個々を格納する配列を宣言
		string[,] rtv = new string[lineData.Length - 2, 2];

		//列(縦)
		for (int line = 0; line < lineData.Length - 2; line++)
		{
			string[] index = lineData[line + 1].Split(',');
			//行(横)
			for (int row = 0; row < 2; row++)
			{
				rtv[line, row] = index[row];
			}
		}
		return rtv;
	}

	public Vector2 getInsPos()
    {
		return InsPos;
    }
}
