using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscGameQuitScript : MonoBehaviour
{
	public static GameObject escquit;

	public GameObject inven;
	InventoryManager i_M;

	public GameObject pause;
	public GameObject finishinfo;
	public GameObject option;
	public GameObject restartinfo;
	public GameObject player;
	public GameObject reticle;
	public Camera cam;
	public GameObject starsParent;
	GameObject[] stars;

	public static bool pause_check;
	public static bool optionflag;

	public Text but1, but2, but3, but4;

	void Start() {
		if (escquit == null) {
			DontDestroyOnLoad(this.gameObject);
			escquit = this.gameObject;
		}
		else {
			if (escquit != gameObject)
				Destroy(gameObject);
		}
		pause_check = false;
		optionflag = false;
		pause.SetActive(pause_check);
		finishinfo.SetActive(false);
		option.SetActive(false);
		restartinfo.SetActive(false);

		int count = starsParent.transform.childCount;
		stars = new GameObject[count];
		for(int i = 0; i < count; i++) {
			stars[i] = starsParent.transform.GetChild(i).gameObject;
		}

		player = GameObject.FindGameObjectWithTag("player");
		reticle = GameObject.FindGameObjectWithTag("Reticle");
		inven = GameObject.FindGameObjectWithTag("Inventory");
		i_M = inven.GetComponent<InventoryManager>();
	}
	
	void Update() {
		if (!i_M.bViewInventoryPanel)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				pause_check = !pause_check;
				if (!pause_check) {
					finishinfo.SetActive(false);
					restartinfo.SetActive(false);
				}
				Cursor.visible = pause_check;
			}
		}

		if (!optionflag)
			PauseOnOff();

		if (cam == null) {
			cam = Camera.main;
			pause.GetComponent<Canvas>().worldCamera = cam;
		}
	}

	void PauseOnOff()
	{
		Time.timeScale = pause_check ? 0 : 1;
		if (pause) {
			for (int i = 0; i < stars.Length; i++)
				stars[i].SetActive(false);
			for (int i = 0; i < stars.Length && i < StaticVariable.Level; i++)
				stars[i].SetActive(true);
		}
		pause.SetActive(pause_check);
	}

	public void ContinueButtonDown()
    {
		pause_check = false;
    }

	public void OptionButtonDown()
    {
		option.SetActive(true);
		optionflag = true;
    }

	public void FinishInfoButtonDown()
    {
		finishinfo.SetActive(true);
    }

	public void EndYesButtonDown()
    {
		UnityEngine.Application.Quit();
    }

	public void EndNoButtonDown()
	{
		finishinfo.SetActive(false);
	}

	public void RestateButtonDown()
    {
		restartinfo.SetActive(true);
	}
	public void RestateYesButtonDown()
	{
		player.SetActive(false);
		reticle.SetActive(false);
		SceneManager.LoadScene("Title");
		Destroy(this.gameObject);
	}

	public void RestateNoButtonDown()
	{
		restartinfo.SetActive(false);
	}

	//ボタンに触れているときの処理・色の変更
	public void OnPointerbut1Enter()
	{
		but1.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	}
	public void OnPointerbut2Enter()
	{
		but2.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	}
	public void OnPointerbut3Enter()
	{
		but3.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	}
	public void OnPointerbut4Enter()
	{
		but4.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	}

	//ボタンが離れた時の処理
	public void OnPointerExit()
	{
		but1.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		but2.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		but3.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		but4.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	}
}
