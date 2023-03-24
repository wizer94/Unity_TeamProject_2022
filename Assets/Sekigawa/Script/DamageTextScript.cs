using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextScript : MonoBehaviour
{

	public string str;
	public float size = 1f;
	public FontStyle style = FontStyle.Bold;

	Text text;

	float gravity = 8f;
	Vector2 vector = Vector2.zero;
	Color startColor;

	float time = 0.8f;

	Camera mainCamera;


	void Start() {
		transform.localScale *= size;
		text = transform.GetChild(0).GetComponent<Text>();
		text.text = str;
		startColor = text.color;
		text.fontStyle = style;
		mainCamera = Camera.main;

		vector = new Vector2(Random.Range(-1f, 1f), 2f + Random.Range(-0.3f, 0.3f));

		Invoke("Destroy", time);
	}
	
	void Update() {
		float dt = Time.deltaTime;

		vector.y -= gravity * dt;
		transform.position += new Vector3(vector.x, vector.y, 0) * dt;
		InsideCamera();

		float alpha = Mathf.Min(time / 0.3f, 1f);
		text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

		time -= dt;
	}

	bool InsideCamera() {
		Vector2 pos = transform.position;
		Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(Vector2.zero);
		Vector2 topRight = mainCamera.ViewportToWorldPoint(Vector2.one);

		bool rtv = false;
		float margin = 0.6f;
		if(pos.x < bottomLeft.x + margin) {
			transform.position = new Vector2(bottomLeft.x + margin, pos.y);
			rtv = true;
		}
		if (pos.y < bottomLeft.y + margin) {
			transform.position = new Vector2(pos.x, bottomLeft.y + margin);
			rtv = true;
		}
		if (pos.x > topRight.x - margin) {
			transform.position = new Vector2(topRight.x - margin, pos.y);
			rtv = true;
		}
		if (pos.y > topRight.y) {
			transform.position = new Vector2(pos.x, topRight.y - margin);
			rtv = true;
		}
		//Debug.Log(rtv);

		return rtv;
	}

	void Destroy() {
		Destroy(gameObject);
	}
}
