using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootLoad : MonoBehaviour
{
	//���p�ӏ�

	[SerializeField] TextAsset RootData;
	Vector2 InsPos;

    public string[,] LoadRoot()
	{
		string rawData = RootData.text;
		//�s���Ƃɕ���
		string[] lineData = rawData.Split('\n');

		//�������W�̎擾
		string[] PosIndex = lineData[0].Split(',');
		InsPos.x = float.Parse(PosIndex[0]);
		InsPos.y = float.Parse(PosIndex[1]);

		//�X���i�[����z���錾
		string[,] rtv = new string[lineData.Length - 2, 2];

		//��(�c)
		for (int line = 0; line < lineData.Length - 2; line++)
		{
			string[] index = lineData[line + 1].Split(',');
			//�s(��)
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
