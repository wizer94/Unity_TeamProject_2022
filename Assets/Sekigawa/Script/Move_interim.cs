using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_interim : MonoBehaviour
{

	[SerializeReference] float speed = 3;

	void Start() {
		
	}
	
	void Update() {
		float dt = Time.deltaTime;

		Vector2 vec = new Vector2(0, 0);
		if (KeyScript.InputOn(KeyScript.Dir.Up))
			vec += new Vector2(0, +1);
		if (KeyScript.InputOn(KeyScript.Dir.Down))
			vec += new Vector2(0, -1);
		if (KeyScript.InputOn(KeyScript.Dir.Right))
			vec += new Vector2(+1, 0);
		if (KeyScript.InputOn(KeyScript.Dir.Left))
			vec += new Vector2(-1, 0);
		if (vec.x * vec.y != 0)
			vec *= 0.7071f;

		transform.position += new Vector3(vec.x, vec.y, 0) * speed * dt;

		Vector2 mouseVector = GetMousePos() - transform.position;
		bool isFlip = mouseVector.x < 0;
		transform.localScale = new Vector3(isFlip ? -1 : 1, 1, 1);
	}

	Vector3 GetMousePos() {
		Vector2 inp = Input.mousePosition;
		return Camera.main.ScreenToWorldPoint(new Vector3(inp.x, inp.y, 10));
	}
}
