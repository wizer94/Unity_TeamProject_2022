using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextGenerator : MonoBehaviour
{

	EnemyClass ec;
	float HPbfr = 0;
	//ダメージテキストのプレファブ
	public GameObject damageText_;
	[System.NonSerialized] public bool isCritical = false;
	AudioSource AS;
	AudioClip damageSE, critSE;

	void Start() {
		ec = GetComponent<EnemyClass>();
		AS = GetComponent<AudioSource>();
		InvokeRepeating("InvokeDamageUpDate", 0, 0.1f);
		damageSE = Resources.Load<AudioClip>("SoundEffect/hit");
		critSE = Resources.Load<AudioClip>("SoundEffect/critical");
		AS = gameObject.AddComponent<AudioSource>();
		AS.playOnAwake = false;
		AS.volume = 0.1f;
	}

	void InvokeDamageUpDate() {
		float HP = ec.getHp();
		if (HP < HPbfr) {
			float damage = HPbfr - HP;
			float size = (Mathf.Min(damage / (ec.getMaxHp() * 0.9f), 1) + 1f) / 2f;
			string text = damage >= 1 ? Mathf.Floor(damage).ToString("F0") : "1";
			Vector2 pos = transform.position + new Vector3(0, 1f, 0);
			if (!isCritical) {
				GenerateDamageText(pos, text, size);
				PlaySE(damageSE);
			}
			else {
				GenerateDamageText(pos, text, size * 1.6f, Color.yellow, Color.red, FontStyle.BoldAndItalic);
				PlaySE(critSE);
			}
		}
		isCritical = false;

		HPbfr = ec.getHp();
	}

	void Update() {
		if (!ec.getAppearFlag()) {
			InvokeDamageUpDate();
		}
	}

	void PlaySE(AudioClip ac, float volume = 1f, float pitch = 1f) {
		AS.volume *= volume;
		AS.pitch *= pitch;
		AS.PlayOneShot(ac);
	}


	//HPが減った時のテキストを生成----------------------------------------------------------


	public void GenerateDamageText(Vector3 pos, string str, float size = 1f) {
		GameObject obj = Instantiate(damageText_, pos, Quaternion.identity);
		DamageTextScript damageText = obj.GetComponent<DamageTextScript>();
		damageText.size = size;
		damageText.str = str;
		damageText.style = FontStyle.Bold;
	}
	public void GenerateDamageText(Vector3 pos, string str, float size, Color color, Color outColor, FontStyle font = FontStyle.Bold) {
		GameObject obj = Instantiate(damageText_, pos, Quaternion.identity);
		DamageTextScript damageText = obj.GetComponent<DamageTextScript>();
		damageText.size = size;
		damageText.str = str;
		obj.transform.GetChild(0).GetComponent<Text>().color = color;
		obj.transform.GetChild(0).GetComponent<Outline>().effectColor = outColor;
		damageText.style = font;
	}
}
