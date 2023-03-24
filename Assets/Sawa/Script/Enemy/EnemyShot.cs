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
	float DistDecayTime;	//������������

	void Start()
	{
		tr = GetComponent<TrailRenderer>();
		size = transform.localScale.x;
		Invoke("Destroy", liveTime);

		DistDecayTime = 0;

		//�u���p�̐ݒ�
		float BlurAngle = Random.Range(-maxBlurAng, maxBlurAng) * Mathf.Deg2Rad;

		//�ړ�
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

		//��������
		Distance_Decay();

		liveTime -= dt;
	}

    void Destroy()
	{
		Destroy(gameObject);
	}

	//�v���C���Ƃ̓����蔻��
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag != "Enemy")
        {
            if (col.gameObject.tag == "player")
            {
				//���g������
				Destroy();
			}
        }
		//�ǂƐڐG���Ă�����
		if(col.gameObject.tag == "Wall")
        {
			//���ł���
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

	//��������
	void Distance_Decay()
    {
		//��������
		if(DistDecayTime >= 0.1f)
        {
			damage *= 0.95f;

			DistDecayTime = 0;
		}

		DistDecayTime += Time.deltaTime;
    }
}
