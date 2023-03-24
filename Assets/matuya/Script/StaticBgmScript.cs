using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticBgmScript : MonoBehaviour
{

	public static AudioSource bgm;
	/*
	[SerializeField] AudioClip[] ac;
	[SerializeField] Slider sliderBGM, sliderSE;
	[SerializeField] Toggle toggleBGM, toggleSE;
	public static float BGMvolume = 1.0f;
	public static float SEvolume = 1.0f;
	public static bool BGM_isOn = true;
	public static bool SE_isOn = true;
	float correction = 0.01f;
	*/
	public static AudioClip title, stage, boss;

	void Start() {
		if (bgm == null) {
			DontDestroyOnLoad(this.gameObject);
			bgm = GetComponent<AudioSource>();
		}

		title = Resources.Load("BGM/Tutorial") as AudioClip;
		stage = Resources.Load("BGM/Stage") as AudioClip;
		boss = Resources.Load("BGM/Boss") as AudioClip;
		
		Play(title);
	}
	
	void Update() {
		
	}

	public static void Play(AudioClip ac) {
		if(ac != bgm) {
			bgm.clip = ac;
			bgm.Play();
		}
	}
	public static void PlayTitle() {
		if (title != bgm) {
			bgm.clip = title;
			bgm.Play();
		}
	}
	public static void PlayStage() {
		if (stage != bgm) {
			bgm.clip = stage;
			bgm.Play();
		}
	}
	public static void PlayBoss() {
		if (boss != bgm) {
			bgm.clip = boss;
			bgm.Play();
		}
	}
}
