using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPscript : MonoBehaviour
{

	float maxhp;
	float hp = 100f;
	public bool immortal = false;

	void Start() {
		maxhp = hp;
	}

	public void Death() {
		Destroy(gameObject);
	}

	public void Damage(float v) {
		SetHp(hp - Mathf.Max(v, 0));
	}

	public void Heal(float v) {
		SetHp(hp + Mathf.Max(v, 0));
	}

	public void Attack(HPscript other, float v) {
		other.Damage(v);
	}





	public float GetHp() {
		return hp;
	}
	public void SetHp(float v) {
		if (immortal) return;
		hp = Mathf.Min(v, maxhp);
		if (hp <= 0)
			Death();
	}

	public float GetMaxHp() {
		return maxhp;
	}
	public void SetMaxHp(float v) {
		maxhp = v;
	}
}
