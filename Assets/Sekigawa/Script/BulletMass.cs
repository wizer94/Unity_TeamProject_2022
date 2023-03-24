using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMass : MonoBehaviour
{

	public WeaponScript ws;
	public List<BulletHitter> bulletHitter = new List<BulletHitter>();
	bool anyHit = false;

	void Update() {
		int count = 0;
		for (int i = 0; i < bulletHitter.Count; i++) {
			if (bulletHitter[i].bullet_)
				count++;
		}
		if(count == 0) {
			if(!anyHit)
				OnNeverHit();
			Destroy(gameObject);
		}
	}

	public void OnHit(int massID) {
		bulletHitter[massID].Hit();


		//ˆÈ‰ºOnAnyHit—pˆ—
		if (!anyHit)
			OnAnyHit();
		anyHit = true;
	}
	public void OnAnyHit() {

	}

	public void OnNotHit(int massID) {


		
	}

	public void OnNeverHit() {
		ws.OnNeverHit();
	}
}

[System.Serializable]
public class BulletHitter {
	public BulletController bullet_;
	[SerializeField] bool hit_;
	public BulletHitter(BulletController bullet) {
		bullet_ = bullet;
		hit_ = false;
	}
	public void Hit() {
		hit_ = true;
	}
	public bool IsHit() {
		return hit_;
	}
}