using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
	
	public enum Type {
		Fire = 0,
		Ice,


		length	//ŒÂ”ŠÇ——p”Ô†B•ÒW‹Ö~
	}

	public class buff {
		public Type type;
		public int level;
		public float remain;
		public float time;

		public buff() {
			level = 0;
			remain = 0;
			time = 0;
		}
	}

	buff[] buffes = new buff[(int)Type.length];

	EnemyClass enemy;
	EnemyImg enemyImg;
	float dt = 1f;

	void Start() {
		for(int i = 0; i < buffes.Length; i++) {
			buffes[i] = new buff();
		}
		enemy = GetComponent<EnemyClass>();
		if(enemy != null)
			enemyImg = enemy.enemyImg.GetComponent<EnemyImg>();
	}
	
	void Update() {
		dt = Time.deltaTime;

		for (int i = 0; i < buffes.Length; i++) {
			if (buffes[i].remain <= 0)
				continue;

			switch ((Type)i) {
				case Type.Fire:
					OnFire(buffes[i]);
					break;
				case Type.Ice:
					OnIce(buffes[i]);
					break;
			}

			buffes[i].remain -= dt;
			if (buffes[i].remain < 0)
				buffes[i].remain = 0;
		}
	}

	void Damaging(float dmg) {
		if (enemyImg != null)
			enemyImg.CollHitDamage(dmg);
	}

	public bool ExistBuff(Type type) {
		return GetBuff(type).remain > 0;
	}

	public buff GetBuff(Type type) {
		return buffes[(int)type];
	}

	public void SetBuff(Type type, int level, float remain) {
		buff temp = new buff();
		temp.level = level;
		temp.remain = remain;

		buffes[(int)type] = temp;
	}

	public void AddBuff(Type type, int level, float remain) {
		if (!ExistBuff(type)) {
			SetBuff(type, level, remain);
		}
		else {
			if(level > GetBuff(type).level)
				SetBuff(type, level, remain);
			GetBuff(type).remain = remain;
		}
	}

	void OnFire(buff b) {

		float x = b.level;
		float dmgPer = 0.5f / (x + 1);
		float rand = 1f + Random.Range(-0.1f, 0.1f) + Random.Range(-0.05f, 0.05f);
		float damage = 20f * (x + 1) * rand;

		if(b.time == 0)
			Damaging(damage);

		if(b.remain > 0)
			b.time += dt;

		if(b.time >= dmgPer)
			b.time = 0;
	}

	void OnIce(buff b) {

	}
}
