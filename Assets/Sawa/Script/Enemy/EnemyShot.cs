using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
	public float damage;
	public float liveTime = 0.5f;
	public float size = 1f;
	public float crit_multiply = 2.5f;
	float shrinkTime = 0.1f;
	Vector3 shrinkSize = Vector3.zero;
	TrailRenderer tr;

	float ShotAngle;
	float maxBlurAng;
	[SerializeField] public float ShotSpeed;

	Vector3 move = Vector3.zero;
	float DistDecayTime;	//距離減衰時間

	void Start()
	{
		tr = GetComponent<TrailRenderer>();
		size = transform.localScale.x;
		Invoke("Destroy", liveTime);

		DistDecayTime = 0;

		//ブレ角の設定
		float BlurAngle = Random.Range(-maxBlurAng, maxBlurAng) * Mathf.Deg2Rad;

		//移動
		move.x = Mathf.Cos(ShotAngle + BlurAngle) * ShotSpeed;
		move.y = Mathf.Sin(ShotAngle + BlurAngle) * ShotSpeed;
	}

    private void FixedUpdate()
    {
		float dt = Time.deltaTime;

		if (liveTime <= shrinkTime)
		{
			if (shrinkSize == Vector3.zero)
				shrinkSize = transform.localScale;
			transform.localScale = shrinkSize * liveTime / shrinkTime;
		}

		transform.Translate(move);

		//距離減衰
		Distance_Decay();

		liveTime -= dt;
	}

    void Destroy()
	{
		Destroy(gameObject);
	}

	//プレイヤとの当たり判定
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag != "Enemy")
        {
            if (col.gameObject.tag == "player")
            {
				//自身も消滅
				Destroy();
			}
        }
		//壁と接触していたら
		if(col.gameObject.tag == "Wall")
        {
			//消滅する
			Destroy();
        }
    }

	public void setAngle(float ang)
    {
		ShotAngle = ang;
    }
	public void setMaxBlurAngle(float ang)
    {
		maxBlurAng = ang;
    }

	//距離減衰
	void Distance_Decay()
    {
		//距離減衰
		if(DistDecayTime >= 0.1f)
        {
			damage *= 0.95f;

			DistDecayTime = 0;
		}

		DistDecayTime += Time.deltaTime;
    }
}
