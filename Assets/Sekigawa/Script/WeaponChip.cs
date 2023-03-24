using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponChip : MonoBehaviour
{

	public Sprite icon;
	public string Name = "";
	[Multiline]
	public string description = "説明変数";
	[Range(1, 25)]
	public int level = 1;
	public bool loaded = false;
	public bool droping = true;

	protected WeaponScript ws;
	SpriteRenderer sr;
	BoxCollider2D bc2d;

	protected void Start() {
		SetupChip();
		if (!GetComponent<WeaponScript>()) {
			gameObject.name = Name + level;
			if (GetComponent<SpriteRenderer>())
				sr = GetComponent<SpriteRenderer>();
			SetColorFromLevel();
			if (GetComponent<BoxCollider2D>())
				bc2d = GetComponent<BoxCollider2D>();

			if (droping) {
				sr.sprite = icon;
			}
		}
	}

	void Update() {
		bool enable = droping || transform.CompareTag("Weapon");
		if(sr)
			sr.enabled = enable;
		if(bc2d)
			bc2d.enabled = enable;

	}

	public void AddChip() {
		if (ws && enabled) {
			WeaponChip wc = ws.gameObject.AddComponent(GetType()) as WeaponChip;
			wc.icon = icon;
			wc.level = level;
		}
	}

	public WeaponChip GetSameChip() {
		if (!ws) return null;

		WeaponChip[] weaponChips = ws.GetComponents<WeaponChip>();
		foreach (WeaponChip wc in weaponChips) {
			//相手方のコンポーネントが有効で、型が同じコンポーネント
			if (wc.enabled && GetType() == wc.GetType()) {
				return wc;
			}
		}
		return null;
	}

	public bool Equals(WeaponChip wc) {
		return GetType() == wc.GetType();
	}

	
	protected void OnDestroy() {
		if (GetComponent<WeaponScript>()) {
			//UnLoadChip();
		}
	}
	

	public void SetWS(WeaponScript w) {
		ws = w;
	}

	public void Destroy() {
		enabled = false;
		UnLoadChip();
		Destroy(this);
	}

	public void LoadChip() {
		if (ws && enabled) {
			droping = false;
			OnChipLoad();
			//Debug.Log("load " + Name + " Lv" + level);
		}
	}
	public void UnLoadChip() {
		if (ws) {
			loaded = false;
			if (level > 0) {
				OnChipUnLoad();
				//Debug.Log("unload " + Name + " Lv" + level);
			}
		}
	}



	protected virtual void SetupChip() {

	}
	protected virtual string GetDescription(int level) {
		return "chip側で未定義です。";
	}
	protected virtual void OnChipLoad() {

	}
	protected virtual void OnChipUnLoad() {

	}


	protected void SetDescription(string str) {
		description = "【Lv." + level + "】\n" + str;
	}
	public void SetDescription() {
		description = "【Lv." + level + "】\n" + GetDescription(level);
	}
	public void SetMergeDescription(List<int> chips) {
		string temp = "【Lv.";
		for (int i = 0; i < chips.Count - 1; i++)
			temp += chips[i] + "+";
		temp += chips[chips.Count - 1];
		temp += "】\n";
		int sum = 0;
		foreach (int i in chips)
			sum += i;
		temp += GetDescription(sum);

		description = temp;
	}

	void SetColor(Color c) {
		if (sr) {
			sr.color = c;
		}
	}
	public void SetColorFromLevel() {
		SetColor(GetColorFromLevel());
	}
	public Color GetColorFromLevel() {
		Color rtv = Color.white;
		if (level < 5) {
			float colorGB = Mathf.Max(0f, 1f - (this.level - 1) / 4f);
			rtv = new Color(1f, colorGB, colorGB);
		}
		else {
			rtv = Color.green;
		}
		return rtv;
	}

	public int AddLevel(int add) {
		UnLoadChip();
		this.level += add;
		gameObject.name = this.Name + this.level;
		SetColorFromLevel();
		LoadChip();
		return this.level;
	}
	public void SetLevel(int v) {
		UnLoadChip();
		this.level = v;
		SetColorFromLevel();
		gameObject.name = this.Name + this.level;
		LoadChip();
	}

	public void ChangeLevel(int v) {
		level = v;
		SetColorFromLevel();
		gameObject.name = this.Name + this.level;
		SetupChip();
	}


	public float Pow(float b, int p) {
		if (p == 1)
			return b;
		float rtv = 1;
		for (int i = 0; i < p; i++)
			rtv *= b;
		return rtv;
	}

	public float GetStdMultiply(float max, float time, int maxTime = 25) {
		float min = 1f;
		AnimationCurve stdMuliply = new AnimationCurve(new Keyframe(0, 0, 0, 0.75f / maxTime), new Keyframe(maxTime, 1, 0, 0));
		return (max - min) * stdMuliply.Evaluate(time) + min;
	}
	public float GetStdMultiply(float min, float max, float time, int maxTime) {
		AnimationCurve stdMuliply = new AnimationCurve(new Keyframe(0, 0, 0, 0.75f / maxTime), new Keyframe(maxTime, 1, 0, 0));
		return (max - min) * stdMuliply.Evaluate(time) + min;
	}

	public string ConvertPercentMultiply(float v, string upper = "上昇", string downer = "低下") {
		float percent = (v - 1f) * 100f;
		return Mathf.Abs(percent).ToString("F0") + "％" + (percent >= 0 ? upper : downer);
	}

}
