using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

	public WeaponScript ws;
	public BulletMass bm;
	public PlayerStat P_Stat;
	public int bulletID = -1;
	public int massID = -1;
	public float speed = 1;
	public float damage = 100f;
	public float damageMultiply = 1f;
	public float liveTime = 0.5f;
	public float startLiveTime;
	float timeRate = 0;
	public float size = 1f;
	public float crit_rate = 0.05f;
	public float crit_multiply = 2f;

	// Playerchip
	public float pl_konshin = 1.0f;
	public float pl_haisui = 1.0f;
	public float pl_crit_rate = 0.0f;
	public float pl_crit_multi = 0.0f;

	float shrinkTime = 0.1f;
	Vector3 shrinkSize = Vector3.zero;
	Rigidbody2D rb;
	TrailRenderer tr;

	public AnimationCurve attenuation;
	public AnimationCurve attenuationMultiply;
	public float through = 0f;
	public int throughCount = 0;
	int hitCount = 0;
	public bool stack = false;
	public float stackPower = 1f;
	public GameObject target;
	public float homing_Acc = 0f;
	bool homingStart = false;
	public int fireLevel = 0;
	public float fireTime = 0;

	void Start() {
		tr = GetComponent<TrailRenderer>();
		if (homing_Acc > 0)
			tr.minVertexDistance = 0.01f;
		rb = GetComponent<Rigidbody2D>();
		size = transform.localScale.x;
		SetLineWidth(size * 0.03f);
		Invoke("Destroy", liveTime);
		Invoke("StartHoming", 1f / speed);
		startLiveTime = liveTime;
	}
	
	void Update() {
		float dt = Time.deltaTime;

		if(homingStart && target) {
			Vector2 toVec = (target.transform.position - transform.position).normalized * speed;
			Vector2 selfVec = rb.velocity;
			float power = Mathf.Min((toVec - selfVec).magnitude, 5f);
			rb.velocity += new Vector2(Mathf.Sign(toVec.x - selfVec.x), Mathf.Sign(toVec.y - selfVec.y)) * homing_Acc * speed * speed * power * dt * 0.15f;
			transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg);

			if(through > 0 && Vector2.SqrMagnitude(target.transform.position - transform.position) < 0.5f * 0.5f) {
				homingStart = false;
				Invoke("StartHoming", 2f / rb.velocity.magnitude);
			}
		}

		if(liveTime <= shrinkTime) {
			if(shrinkSize == Vector3.zero)
				shrinkSize = transform.localScale;
			transform.localScale = shrinkSize * liveTime / shrinkTime;
			SetLineWidth(size * 0.03f * liveTime / shrinkTime);
		}
		damageMultiply = attenuation.Evaluate(timeRate) * attenuationMultiply.Evaluate(timeRate);

		liveTime -= dt;
		timeRate = 1f - liveTime / startLiveTime;

		PlayerChipApply();
	}

	void PlayerChipApply()
    {
		if (P_Stat)
		{
			
			// 渾身（HPが100%（実際は98%以上）の場合ダメージアップ）
			if (P_Stat.HP >= P_Stat.Max_HP * P_Stat.FullHPDamage_HP)
				pl_konshin = P_Stat.FullHPDamage;
			else
				pl_konshin = 1.0f;
			// 背水（HPが35%以下の場合ダメージアップ）
			if (P_Stat.HP <= P_Stat.Max_HP * P_Stat.LessHPDamage_HP)
				pl_haisui = P_Stat.LessHPDamage;
			else
				pl_haisui = 1.0f;
			// クリティカル
			pl_crit_rate = P_Stat.Crit_Up;
			pl_crit_multi = P_Stat.CritDamage_Up;
		}
	}

	void Destroy() {
		if(hitCount <= 0) {
			if (ws) {
				ws.OnNotHit(this);
			}
			if (bm) {
				bm.OnNotHit(massID);
			}
		}
		Destroy(gameObject);
	}

	void StartHoming() {
		if(homing_Acc > 0)
			homingStart = true;
	}

	void SetLineWidth(float width) {
		tr.startWidth = width;
		tr.endWidth = width;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "OtherEnemy" || other.gameObject.tag == "Dummy") {
			if (other.GetComponent<EnemyImg>()) {
				EnemyImg enemy = other.GetComponent<EnemyImg>();
				bool isCrit = Random.value < (crit_rate + pl_crit_rate);
				DamageTextGenerator dtg = enemy.enemyObj.GetComponent<DamageTextGenerator>();
				if(isCrit)
					dtg.isCritical = true;
				float stack_multiply = 1f;
				if(ws && ws.stack_ > 0 &&  ws.IsStacked()) {
					dtg.isCritical = true;
					stack_multiply = stackPower;
				}


				float damage_fix = damage * damageMultiply * (isCrit ? (crit_multiply + pl_crit_multi) : 1f) * stack_multiply * pl_konshin * pl_haisui;

				if (ws) {
					ws.OnHit(this, damage_fix);

					if (ws.hit_bulletID != bulletID) {
						ws.OnAnyHit(this);
					}
					ws.hit_bulletID = bulletID;
					
				}
				if (bm) {
					bm.OnHit(massID);
				}

				EnemyClass ec = enemy.enemyObj.GetComponent<EnemyClass>();
				enemy.CollHitDamage(damage_fix);
				if (fireLevel > 0 && ec.GetComponent<Buff>())
					ec.GetComponent<Buff>().AddBuff(Buff.Type.Fire, fireLevel - 1, fireTime);

				if (ec && ec.getHp() <= 0)
					ws.OnKill();
				damage *= through;
				//Debug.Log(damage);
				throughCount++;
				hitCount++;
			}
			if(damage <= 0)
				Destroy();
		}
		else if (other.gameObject.tag == "Wall") {
			Destroy();
		}
	}
	

}
