using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleScript : MonoBehaviour
{

	public GameObject weapon;
	[SerializeField] private bool WeaponFind = false;
	GameObject preWeapon;
	WeaponScript ws;
	public Vector3 trailPos = Vector3.zero;
	[SerializeField] Sprite normal;
	[SerializeField] Sprite focus;
	public bool isFocus = false;
	[SerializeField] SpriteRenderer dotSpr;
	[SerializeField] GameObject circleObj;
	[SerializeField] SpriteRenderer circleSpr;

	[SerializeField] GameObject lineTop;
	[SerializeField] GameObject lineBottom;
	[SerializeField] GameObject lineRight;
	[SerializeField] GameObject lineLeft;
	[SerializeField] SpriteRenderer lineTopSpr;
	[SerializeField] SpriteRenderer lineBottomSpr;
	[SerializeField] SpriteRenderer lineRightSpr;
	[SerializeField] SpriteRenderer lineLeftSpr;

	[SerializeField] GameObject canvas;
	[SerializeField] Image ammoImage;
	[SerializeField] Text ammoText;

	LineRenderer lr;

	float scattering = 1f;
	float circleSizeMuliply = 1f;
	public float outRange = 0f;

	void Start() {
		lr = GetComponent<LineRenderer>();
		if (WeaponFind && !weapon) {
			if (GameObject.FindGameObjectWithTag("Weapon")) {
				weapon = GameObject.FindGameObjectWithTag("Weapon");
				WeaponScript ws = weapon.GetComponent<WeaponScript>();
				ws.reticle = gameObject;
				//Debug.Log(ws.reticle);
			}
		}

		DontDestroyOnLoad(this.gameObject);
	}

	void LateUpdate() {

		transform.position = trailPos;
	}

	void Update() {
		if (Time.timeScale <= 0) return;
		else if(!InventoryManager.openingInventory)
			Cursor.visible = false;

		if (weapon) {
			if (ws == null || preWeapon != weapon)
				ws = weapon.GetComponent<WeaponScript>();


			lr.enabled = ws.GetCapturedObject() != null;
			transform.localScale = Vector3.one * (ws.autoAiming ? 1.5f : 1f);
			if (ws.GetCapturedObject()) {
				Vector2 mousePos = GetMousePos();
				dotSpr.transform.position = mousePos;
				canvas.transform.position = mousePos;
				Vector3[] poss = new Vector3[] { trailPos, mousePos };
				lr.SetPositions(poss);
			}
			else {
				dotSpr.transform.localPosition = Vector3.zero;
				canvas.transform.localPosition = Vector3.zero;
			}
			

			trailPos = ws.GetCapturedPos();
			float range = Vector2.Distance(trailPos, weapon.transform.position);
			float bowstring = 2f * range * Mathf.Sin(Mathf.Min(ws.GetNowBlur(), 180) * Mathf.Deg2Rad * 0.5f);
			float goodRange = bowstring * 0.4f / Mathf.Pow(ws.cohesive, 0.5f);
			SetCircleSizeMuliply(Mathf.Max(1f, goodRange));

			Color reticleColor = new Color(1f, 1f, 1f);
			isFocus = false;
			if (goodRange <= 1f) {
				reticleColor = new Color(0f, 1f, 0f);
				if (ws.GetBlurRate() <= 0) {
					reticleColor = new Color(1f, 1f, 0f);
					isFocus = true;
				}
			}

			if (ws.GetParent()) {
				float fixRange = Vector2.Distance(ws.GetParent().transform.position, trailPos) - ws.GetGunLength();
				float rangedStart = ws.maxRange * ws.GetRangeBlur().x;
				float rangedEnd = ws.maxRange * ws.GetRangeBlur().y;
				outRange = Limit(0f, (fixRange - rangedStart) / (rangedEnd - rangedStart), 1f);
			}
				SetReticleColor(reticleColor);
			SetScatterring(ws.GetNowBlur() / 90f * 3f + 1f);

			if (!ws.IsInfBullet())
				SetAmmoText(ws.GetMagazine().ToString());
			if (!ws.IsReloading()) {
				SetAmmoGauge((float)ws.GetMagazine() / ws.maxMagazine);
			}
			else {
				SetAmmoGauge(ws.GetNowReloadTime() / ws.reloadTime);
			}

			preWeapon = weapon;
		}
		else {
			ws = null;
		}

		dotSpr.sprite = isFocus ? focus : normal;
		Scattering();
		SetLineAlpha(1f - outRange);
	}

	Vector3 GetMousePos() {
		Vector2 inp = Input.mousePosition;
		return Camera.main.ScreenToWorldPoint(new Vector3(inp.x, inp.y, 10));
	}

	public void SetAmmoText(string str) {
		ammoText.text = str;
	}
	public void SetAmmoGauge(float v) {
		ammoImage.fillAmount = v;
	}

	public void SetReticleColor(Color c) {
		dotSpr.color = c;
		circleSpr.color = c;
		lineTopSpr.color = c;
		lineBottomSpr.color = c;
		lineRightSpr.color = c;
		lineLeftSpr.color = c;
	}


	void SetLineAlpha(float alpha) {
		lineTopSpr.color = new Color(lineTopSpr.color.r, lineTopSpr.color.g, lineTopSpr.color.b, alpha);
		lineBottomSpr.color = new Color(lineBottomSpr.color.r, lineBottomSpr.color.g, lineBottomSpr.color.b, alpha);
		lineRightSpr.color = new Color(lineRightSpr.color.r, lineRightSpr.color.g, lineRightSpr.color.b, alpha);
		lineLeftSpr.color = new Color(lineLeftSpr.color.r, lineLeftSpr.color.g, lineLeftSpr.color.b, alpha);
	}

	public void SetScatterring(float level) {
		scattering = level;
	}
	public void SetCircleSizeMuliply(float multi) {
		circleSizeMuliply = multi;
	}

	void Scattering() {
		scattering = Limit(1f, scattering, 4f);
		SetLineRadius(Mathf.Pow(scattering, 1.5f) * (scattering > 1f ? 0.4f: 0.3f));
		SetCircleSize(scattering * circleSizeMuliply);
	}

	void SetLineRadius(float radius) {
		lineTop.transform.localPosition = new Vector3(0, radius, 0);
		lineBottom.transform.localPosition = new Vector3(0, -radius, 0);
		lineRight.transform.localPosition = new Vector3(radius, 0, 0);
		lineLeft.transform.localPosition = new Vector3(-radius, 0, 0);
	}

	float Limit(float min, float a, float max) {
		if (a < min) return min;
		if (a > max) return max;
		return a;
	}

	void SetCircleSize(float size) {
		SetCircleScale(size);
		SetCircleAlpha(1f / size * 0.5f);
	}

	void SetCircleScale(float scale) {
		circleObj.transform.localScale = Vector3.one * 0.2f * Mathf.Sqrt(scale);
	}

	void SetCircleAlpha(float alpha) {
		circleSpr.color = new Color(circleSpr.color.r, circleSpr.color.g, circleSpr.color.b, alpha);
	}


}
