using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

	[Header("基本")]
	[Tooltip("この武器の名前")]
	public string weaponName;
	[Tooltip("押しっぱなしで連射できるか")]
	public bool semiauto = true;
	public bool semiauto_def = true;

	bool autoAim = false;
	public bool autoAiming = false;
	[Tooltip("オートエイムが適用される距離")]
	[System.NonSerialized] public float autoAimRange = 2.0f;
	GameObject capturedObj = null;

	bool bfrInputable = false;
	bool bfrInput = false;

	[Tooltip("弾１つあたりのダメージ")]
	public float damage = 100;
	float damage_def = 100;
	[SerializeField] float dps = 100;
	[Tooltip("弾のダメージ減衰")]
	public AnimationCurve attenuation;
	[Tooltip("弾のダメージ減衰倍率。Xが至近距離、Yが最大距離")]
	[System.NonSerialized] public AnimationCurve attenuationMultiply;
	[Tooltip("１秒間に何回発射できるか")]
	public float perShot = 1f;
	[Tooltip("弾の速さ")]
	public float speed = 5f;
	[Tooltip("弾の大きさ")]
	public float size = 1f;
	[Tooltip("１回の発射で放つ弾の数\n小数は確率で＋１")]
	public float count = 1;
	[Tooltip("弾の飛ぶ距離")]
	public float maxRange = 10f;
	Vector2 rangeBlur = new Vector2(0.8f, 1.2f);
	[System.NonSerialized] public float crit_rate = 0.05f;
	[System.NonSerialized] public float crit_multiply = 2f;

	[Header("拡散")]
	[Tooltip("集弾性。\n１で完全にランダムに飛び、\n大きくなるほど真ん中に飛びやすくなる\n３が推奨")]
	public int cohesive = 2;
	[Tooltip("最も正確なときの射撃のブレ(度)")]
	public float minBlur = 2f;
	[Tooltip("最も拡散が激しいときの射撃のブレ(度)")]
	public float maxBlur = 30f;
	float blurRate = 0;
	float nowBlur = 0;
	[Tooltip("１回の発射で加算されるブレの割合(度)")]
	public float blurAccRate = 20f;
	[Tooltip("非射撃時に回復するブレの割合(％)\n200なら0.5秒で最大ブレから最小ブレになる")]
	public float blurRechargeRate = 200f;

	[Header("弾倉")]
	[Tooltip("最大時の弾倉")]
	public int maxMagazine = 20;
	int magazine = 20;
	bool infBullet = false;
	[Tooltip("リロードにかかる時間(秒)")]
	public float reloadTime = 1.5f;
	float nowReloadTime = 0;
	bool reloading = false;
	bool reload_want = false;

	[Header("その他")]
	[Tooltip("この武器を持っているときの移動速度倍率(％)")]
	public float moveSpeedMultiply = 100f;
	[Tooltip("この武器を射撃しているときの移動速度倍率(％)")]
	public float moveSpeedMultiply_activate = 75f;
	[Tooltip("弾が貫通するときの威力減衰倍率。0で貫通しない")]
	public float through = 0;
	public Vector2 offset;
	public AnimationCurve recoil;
	float playRecoilTime = 1;
	float recoilAngle = 0;

	public bool isAds = false;
	float adsTimeX = 0f;

	public float cameraSize = 1f;

	[SerializeField] GameObject bullet_;
	[SerializeField] GameObject bulletMass_;
	[SerializeField] GameObject cartridgeEffect_;
	[SerializeField] GameObject flashEffect_;
	GameObject flashEffect;
	int flashingFlame = 0;
	int flashVisibleFlame = 8;
	float flashSize = 2.5f;
	[SerializeField] AudioClip shotSound;
	[SerializeField] AudioClip setSound;
	[SerializeField] AudioClip reloadStart, reloadEnd;


	GameObject playerObj;
	PlayerController pc;
	PlayerStat ps;

	AudioSource AS;

	Transform parent;
	Vector2 aimPos;
	public Vector2 offsetPos;
	[System.NonSerialized] public GameObject reticle;
	float gunLength = 0;
	bool shotable = true;
	bool isFlip = false;
	float fixRate = 1f;

	[System.NonSerialized] public int hit_bulletID = -1; //前回Hitした弾のID
	int bulletID_ = 0; //設定用

	[System.NonSerialized] public int stack_ = 0;
	[System.NonSerialized] public int stacking = 0;
	[System.NonSerialized] public float stackPower_ = 5f;

	[SerializeField] int continuity = 0;
	[System.NonSerialized] public int continuityMaxCount = 20;
	[System.NonSerialized] public float continuityDamage = 1f;
	[System.NonSerialized] public float continuityPerShot = 1f;
	[System.NonSerialized] public int continuityMissable = 0;
	int continuityMiss = 0;
	float killCoolTimeMax = 1.0f;
	float killCoolTime = 0f;

	[System.NonSerialized] public float homing_Acc = 0f;
	GameObject homingTarget = null;

	[System.NonSerialized] public int fireLevel = 0;
	[System.NonSerialized] public float fireTime = 0;

	int chipCount = 0;
	string allChipNames = "";

	[System.NonSerialized] public Inventory.SlotItem[] chipDatas = new Inventory.SlotItem[5];

	Camera mainCamera;
	CameraScript cs;

	void Awake() {
		damage_def = damage;
		semiauto_def = semiauto;
		attenuationMultiply = AnimationCurve.Linear(0, 1, 1, 1);
	}

	void Start() {
		playerObj = GameObject.FindGameObjectWithTag("player");
		pc = playerObj.GetComponent<PlayerController>();
		ps = playerObj.GetComponent<PlayerStat>();
		AS = GetComponent<AudioSource>();
		SetMainCamera();
		cameraSize = mainCamera.orthographicSize;
		flashEffect = Instantiate(flashEffect_, transform);
		flashEffect.SetActive(false);

		if (GameObject.FindGameObjectWithTag("Reticle")) {
			reticle = GameObject.FindGameObjectWithTag("Reticle");
		}

		magazine = maxMagazine;

		parent = transform.parent;
		if (minBlur > maxBlur)
			maxBlur = minBlur;
		nowBlur = minBlur;
		gunLength = Vector2.Distance(Vector2.zero, offset + offsetPos);

		maxRange *= fixRate;
		speed *= fixRate;

		bullet_ = (GameObject)Resources.Load("Bullet");
		bulletMass_ = (GameObject)Resources.Load("BulletMass");
		float criticalDamageAve = 1 + crit_rate * crit_multiply;
		dps = damage * perShot * count * criticalDamageAve;

		/*
		if (maxMagazine == 0) {
			infBullet = true;
			reticleScript.SetAmmoText("∞");
		}
		*/
	}

	void Update() {
		if (Time.timeScale <= 0) return;

		float dt = Time.deltaTime;

		//メインカメラを取得
		SetMainCamera();

		//Aim座標を代入
		aimPos = GetAimersPos();
		if (!autoAim && !homingTarget)
			capturedObj = null;


		//リコイル演出処理
		if (playRecoilTime < 1) {
			recoilAngle = recoil.Evaluate(playRecoilTime);
			playRecoilTime += dt * perShot;
		}
		else {
			recoilAngle = recoil.Evaluate(0);
			playRecoilTime = 1;
		}

		//武器画像の座標・向き調整
		Vector2 mouseVector = aimPos - (Vector2)transform.position;
		isFlip = mouseVector.x < 0;
		SetAngleOffset();
		SetPositionOffset();

		//マガジンが最大を超えないようにする
		if (magazine > maxMagazine)
			magazine = maxMagazine;

		//リロード処理
		if (!reloading && KeyScript.InputDown(KeyScript.Dir.Reload) && magazine < maxMagazine)
			reload_want = true;
		if (reload_want)
			StartReload();
		if (reloading) {
			nowReloadTime += dt;
		}

		//チップが変更されたら更新をかける
		if(allChipNames != GetChipNames()) {
			ReloadChip();
		}

		//単射モードのときに前入力に対応
		if (bfrInputable && KeyScript.InputDown(KeyScript.Dir.Fire))
			bfrInput = true;


		//ADS&カメラ処理
		//if (KeyScript.InputOn(KeyScript.Dir.Aim))
		//{
		//	//if (pc.isDash || pc.isDodge)
		//	//	isAds = false;
		//	//else
		//	isAds = true;
		//	if (pc.isDash)
		//		isAds = false;
		//	if (pc.isDodge)
		//		isAds = false;
		//}
		if (!InventoryManager.openingInventory) {
			isAds = KeyScript.InputOn(KeyScript.Dir.Aim);
		}


		float adsTimeSpeed = 4f;
		if (isAds) {
			pc.isDash = false;
			pc.anim.SetBool("isADS", true);
			if (adsTimeX < 1f)
				adsTimeX += adsTimeSpeed * dt;
			if (adsTimeX >= 1f)
				adsTimeX = 1f;
		}
		else {
			if (adsTimeX > 0f)
				adsTimeX -= adsTimeSpeed * dt;
			if (adsTimeX <= 0f)
				adsTimeX = 0f;
		}

		float reticleWeight = 0.3f;
		Vector2 adsVector = reticle.transform.position - pc.transform.position;
		cs.trailPos = (Vector2)pc.transform.position + adsVector * reticleWeight * EaseOut(adsTimeX);


		//発射処理
		if (semiauto) {
			if (KeyScript.InputOn(KeyScript.Dir.Fire))
				Shot(dt);
		}
		else {
			if (KeyScript.InputDown(KeyScript.Dir.Fire) || bfrInput)
				Shot(dt);
		}

		//ブレ回復処理
		if (shotable) {
			if (blurRate > 0)
				blurRate -= blurRechargeRate * dt;
			if (blurRate < 0)
				blurRate = 0;
		}

		//デバッグ
		//左右Shift＋スペース＋P
		if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.P)) {
			damage *= 5f;
			perShot *= 4f;
			maxBlur *= 0.3f;
			minBlur *= 0.3f;
			maxMagazine *= 5;
			semiauto = true;
			
		}

		//プレイヤーの移動速度調整
		pc.moveMultiply = moveSpeedMultiply * 0.01f * (shotable ? 1f : moveSpeedMultiply_activate * 0.01f) * (isAds ? 0.5f : 1f);


		//カメラスムージング
		mainCamera.orthographicSize += (cameraSize - mainCamera.orthographicSize) * 10f * dt;

		//ブレ処理
		SetNowBlur();

		//キルクールタイム処理
		if(killCoolTime > 0) {
			killCoolTime -= dt;
		}
		if (killCoolTime < 0)
			killCoolTime = 0;

		chipCount = GetChipCount();
		allChipNames = GetChipNames();
		if (flashEffect.activeSelf) {
			flashingFlame++;
			flashEffect.transform.localScale = new Vector3(isFlip ? -1 : 1, 1, 1) * flashSize * (float)(flashVisibleFlame - flashingFlame + 1)/ flashVisibleFlame;
		}
		if (flashingFlame > flashVisibleFlame) {
			flashEffect.SetActive(false);
			flashingFlame = 0;
		}
	}

	void LateUpdate() {

		SetCameraSize(maxRange * 0.6f);
	}

	void ResetChip() {

		foreach (WeaponChip wc in GetComponents<WeaponChip>()) {
			wc.Destroy();
		}
	}
	void LoadChildChip() {

		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).GetComponent<WeaponChip>()) {
				WeaponChip wc = transform.GetChild(i).GetComponent<WeaponChip>();
				wc.SetWS(this);
				wc.AddChip();
			}
		}
		WeaponChip[] wcs = GetComponents<WeaponChip>();
		foreach (WeaponChip wc in wcs) {
			foreach (WeaponChip other in wcs) {
				//相手方のコンポーネントが有効で、自身ではなく、型が同じコンポーネント
				if (wc.enabled && other.enabled && wc != other && wc.GetType() == other.GetType()) {
					wc.level += other.level;
					other.Destroy();
				}
			}
		}
		foreach (WeaponChip wc in wcs) {
			if (wc.enabled) {
				wc.SetWS(this);
				wc.LoadChip();
			}
		}
	}

	public void ReloadChip() {
		ResetChip();
		LoadChildChip();
	}


	public void SetCameraSize(float size) {
		size = Limit(4f, size, 16f);
		float maxSize = size;
		float minSize = size * 0.9f;
		//mainCamera.orthographicSize = (maxSize - minSize) * (1f - EaseOut(adsTimeX)) + minSize;
		cameraSize = isAds ? minSize : maxSize;
	}


	void CartridgeEffect(){
		GameObject cartridge = Instantiate(cartridgeEffect_, transform.position, Quaternion.Euler(-90, 0, 0));
		ParticleSystem.MainModule psMain = cartridge.GetComponent<ParticleSystem>().main;
		psMain.startSize = psMain.startSize.constant * Limit(0.5f, Mathf.Sqrt(damage * count / 100f), 1.5f);
	}

	void Flashing(Vector2 pos) {
		flashEffect.transform.position = pos;
		//flashEffect.transform.localEulerAngles = new Vector3(flashEffect.transform.localEulerAngles.x, flashEffect.transform.localEulerAngles.y, transform.localEulerAngles.z - 90f);
		flashEffect.transform.rotation = Quaternion.identity;
		flashEffect.SetActive(true);
		flashingFlame = 0;
	}

	string GetChipNames() {
		string rtv = "";
		for (int i = 0; i < transform.childCount; i++) {
			rtv += transform.GetChild(i).name + ",";
		}
		return rtv;
	}
	int GetChipCount() {
		int rtv = 0;
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).GetComponent<WeaponChip>())
				rtv++;
		}
		return rtv;
	}
	public GameObject[] GetChips() {
		GameObject[] rtv = new GameObject[GetChipCount()];
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).GetComponent<WeaponChip>())
				rtv[i] = transform.GetChild(i).gameObject;
		}
		return rtv;
	}
	public WeaponChip[] GetChipsComponent() {
		WeaponChip[] rtv = new WeaponChip[GetChipCount()];
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).GetComponent<WeaponChip>())
				rtv[i] = transform.GetChild(i).GetComponent<WeaponChip>();
		}
		return rtv;
	}

	float Limit(float min, float a, float max) {
		if (a < min) return min;
		if (a > max) return max;
		return a;
	}

	float EaseIn(float x) {
		return x * x;
	}
	float EaseOut(float x) {
		return 2f * x - x * x;
	}
	

	void SetAngleOffset() {
		Vector2 mouseVector = aimPos - (Vector2)transform.position;
		float rotateAngle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
		int sign = Mathf.Abs(rotateAngle) < 90 ? 1 : -1;
		transform.rotation = Quaternion.AngleAxis(rotateAngle + sign * recoilAngle + (isFlip ? 180 : 0), Vector3.forward);
	}
	void SetPositionOffset() {
		Vector2 mouseVector = aimPos - (Vector2)parent.transform.position;
		float rotateAngle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
		//transform.rotation = Quaternion.AngleAxis(rotateAngle + (isFlip ? 180 : 0), Vector3.forward);


		float mouseAngle = Mathf.Atan2(mouseVector.y, mouseVector.x);
		float initialAngle = Mathf.Atan2(offsetPos.y, offsetPos.x);
		float dist = Vector2.Distance(Vector2.zero, offsetPos);
		float sumAngle = mouseAngle + initialAngle;

		transform.localPosition = new Vector2((isFlip ? -1 : 1) * Mathf.Cos(sumAngle), Mathf.Sin(sumAngle)) * dist;

	}

	Vector3 GetMousePos() {
		Vector2 inp = Input.mousePosition;
		return Camera.main.ScreenToWorldPoint(new Vector3(inp.x, inp.y, 10));
	}

	Vector2 GetAutoAimPos() {
		GameObject[] enemyImgObjs = GetAllEnemys();
		Vector3 mousePos = GetMousePos();
		float minimum_dist = autoAimRange;
		GameObject rtvObj = null;
		foreach(GameObject enemyImgObj in enemyImgObjs) {
			float enemy2mouse = Vector2.Distance(enemyImgObj.transform.position, mousePos);
			float weapon2mouse = Vector2.Distance(transform.position, mousePos);
			if(enemy2mouse < autoAimRange && weapon2mouse < maxRange * rangeBlur.y) {
				if(enemy2mouse < minimum_dist) {
					if (enemyImgObj.GetComponent<BoxCollider2D>() && enemyImgObj.GetComponent<BoxCollider2D>().enabled) {
						rtvObj = enemyImgObj;
						minimum_dist = enemy2mouse;
					}
				}
			}
		}
		Vector2 rtv = mousePos;
		autoAiming = rtvObj != null;
		capturedObj = rtvObj;
		if (rtvObj != null) {
			rtv = rtvObj.transform.position;
		}
		return rtv;
	}

	Vector2 GetAimersPos() {
		if (autoAim) return GetAutoAimPos();
		return GetMousePos();
	}

	void Shot(float dt) {
		if (pc && pc.isDodge) return;
		if (!shotable) return;
		if (Time.timeScale <= 0) return;
		if (InventoryManager.openingInventory) return;
		if (reloading) return;
		if (magazine <= 0 && !infBullet) return;


		pc.isDash = false;

		bfrInput = false;

		BulletMass bm = Instantiate(bulletMass_, transform.position, Quaternion.identity).GetComponent<BulletMass>();
		bm.ws = this;

		for (int c = 0; c < perShot * dt; c++) {
			if (magazine > 0) {

				for (int i = 0; i < (int)count; i++) {
					OneShot(bm);
				}
				float prob = count - (int)count;
				if (Random.value < prob)
					OneShot(bm);

				if (blurRate < 100)
					blurRate += blurAccRate * (isAds ? 0.75f : 1f);
				if (blurRate > 100)
					blurRate = 100;
				if (!infBullet)
					magazine--;
			}
		}
		UpdateBulletID();

		//薬莢処理
		CartridgeEffect();
		//マズルフラッシュ処理
		Flashing(GetShotPos());

		PlaySE(shotSound);
		Invoke("ShotRateWait", 1f / perShot / CalcContinuity(continuityPerShot));
		if (magazine > 0) {
			if (!semiauto) {
				Invoke("ShotRateWaitSE", Mathf.Max(0, 1f / perShot - 0.1f));
				Invoke("BeforeShotWait", Mathf.Max(0, 1f / perShot - 0.25f));
			}
		}

		playRecoilTime = 0;
		shotable = false;
	}

	Vector2 GetShotPos() {
		Vector2 offsetPos = Vector2.zero;
		Vector2 mouseVec = aimPos - (Vector2)parent.transform.position;
		float mouseAngle = Mathf.Atan2(mouseVec.y, mouseVec.x);
		float initialAngle = (isFlip ? -1 : 1) * Mathf.Atan2(offset.y, offset.x);
		float dist = Vector2.Distance(Vector2.zero, offset);
		float sumAngle = mouseAngle + initialAngle;
		offsetPos = new Vector2(Mathf.Cos(sumAngle), Mathf.Sin(sumAngle)) * dist * fixRate;
		return (Vector2)transform.position + offsetPos;
	}

	void OneShot(BulletMass bm) {

		Vector2 shotPos = GetShotPos();
		GameObject bullet = Instantiate(bullet_, shotPos, Quaternion.identity);
		bullet.transform.localScale *= size;
		Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
		BulletController bc = bullet.GetComponent<BulletController>();
		Vector2 vec = aimPos - shotPos;
		float angle = Mathf.Atan2(vec.y, vec.x);
		bc.damage = damage * CalcContinuity(continuityDamage);

		bullet.transform.SetParent(bm.transform, true);
		BulletHitter bh = new BulletHitter(bc);
		bm.bulletHitter.Add(bh);

		SetNowBlur();

		float outputAngle = angle + (RandomizeSum(cohesive) - 0.5f) * nowBlur * Mathf.Deg2Rad;
		rigid.velocity = new Vector2(Mathf.Cos(outputAngle), Mathf.Sin(outputAngle)) * speed;
		bc.speed = speed;
		bullet.transform.localRotation = Quaternion.Euler(0, 0, outputAngle * Mathf.Rad2Deg);

		bc.ws = this;
		bc.bm = bm;
		bc.massID = bm.bulletHitter.Count - 1;
		bc.bulletID = GetBulletID();
		bc.liveTime = maxRange * Random.Range(rangeBlur.x, rangeBlur.y) / speed;
		bc.attenuation = attenuation;
		bc.attenuationMultiply = attenuationMultiply;
		bc.crit_rate = crit_rate * (isAds ? 1.5f : 1f);
		bc.crit_multiply = crit_multiply;
		bc.through = through;
		bc.fireLevel = fireLevel;
		bc.fireTime = fireTime;

		bc.P_Stat = ps;
		if (stack_ > 0) {
			bc.stack = !IsStacked();
			bc.stackPower = stackPower_;
		}
		if (homing_Acc > 0) {
			GameObject[] enemyImgObjs = GetAllEnemys();
			Vector3 mousePos = GetMousePos();
			float minimum_dist = maxRange;
			
			GameObject rtvObj = null;
			homingTarget = null;
			foreach (GameObject enemyImgObj in enemyImgObjs) {
				float enemy2mouse = Vector2.Distance(enemyImgObj.transform.position, mousePos);
				if (enemy2mouse < minimum_dist) {
					if (enemyImgObj.GetComponent<BoxCollider2D>() && enemyImgObj.GetComponent<BoxCollider2D>().enabled) {
						rtvObj = enemyImgObj;
						minimum_dist = enemy2mouse;
					}
				}
			}
			homingTarget = rtvObj;
			bc.target = homingTarget;
			capturedObj = rtvObj;
		}
		bc.homing_Acc = homing_Acc;
	}

	//取得するのはタグのついているImageなので注意
	GameObject[] GetAllEnemys() {
		GameObject[] normalEnemy = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] otherEnemy = GameObject.FindGameObjectsWithTag("OtherEnemy");
		GameObject[] dummyEnemy = GameObject.FindGameObjectsWithTag("Dummy");
		GameObject[] allEnemy = new GameObject[normalEnemy.Length + otherEnemy.Length + dummyEnemy.Length];
		normalEnemy.CopyTo(allEnemy, 0);
		otherEnemy.CopyTo(allEnemy, normalEnemy.Length);
		dummyEnemy.CopyTo(allEnemy, normalEnemy.Length + otherEnemy.Length);
		return allEnemy;
	}

	float CalcContinuity(float v) {
		return (v - 1) * Mathf.Min(1f, (float)continuity / continuityMaxCount) + 1;
	}

	public bool IsStacked() {
		return stacking >= stack_ - 1;
	}
	public void OnHit(BulletController bullet, float damage) {
		if (IsStacked()) {
			stacking = 0;
		}
	}
	public void OnAnyHit(BulletController bullet) {
		continuity++;
		continuityMiss = 0;
		if (bullet.stack) {
			stacking++;
			//Debug.Log(stacking);
		}
		

	}

	public void OnKill() {
		PlayerStat.enemy_cnt++;
		killCoolTime = killCoolTimeMax;
	}

	public void OnNotHit(BulletController bullet) {

	}
	public void OnNeverHit() {
		if (killCoolTime <= 0) {
			continuityMiss++;
			if (continuityMiss > continuityMissable) {
				continuityMiss = 0;
				continuity = 0;
			}
		}
	}

	void SetNowBlur() {
		float maxBlurF = maxBlur;
		float minBlurF = minBlur;
		if (isAds) {
			maxBlurF *= 0.8f;
			minBlurF *= 0.4f;
		}
		nowBlur = (maxBlurF - minBlurF) * (blurRate * 0.01f) + minBlurF;
		rangeBlur = new Vector2(1f - Mathf.Min(180, nowBlur) / 360f, 1f + Mathf.Min(180, nowBlur) / 360f);
	}

	void ShotRateWait() {

		shotable = true;
		if (KeyScript.InputOn(KeyScript.Dir.Dash))
			pc.isDash = true;
		bfrInputable = false;
		if (magazine <= 0 && !infBullet) {
			StartReload();
		}
	}
	void ShotRateWaitSE() {
		PlaySE(setSound);
	}
	void BeforeShotWait() {
		bfrInputable = true;
	}

	void StartReload() {
		if (!shotable) return;
		if (reloading) return;

		reloading = true;
		reload_want = false;
		PlaySE(reloadStart);
		Invoke("Reloaded", reloadTime);
		for(int i=0;i<3;i++)
			CartridgeEffect();
		Invoke("ReloadedSE", Mathf.Max(0, reloadTime - 0.4f));
	}
	void Reloaded() {
		magazine = maxMagazine;
		nowReloadTime = 0;
		reloading = false;
	}
	public void ReloadedSE() {
		PlaySE(reloadEnd);
	}

	void SetMainCamera() {
		mainCamera = Camera.main;
		cs = mainCamera.GetComponent<CameraScript>();
	}

	float RandomizeSum(int level) {
		float sum = 0;
		for (int i = 0; i < level; i++)
			sum += Random.value;
		return sum / level;
	}

	void PlaySE(AudioClip ac) {
		float ratio = damage * CalcContinuity(continuityDamage) / damage_def;
		AS.pitch = Mathf.Max(0.2f, 1f / Mathf.Pow(ratio, 0.4f));
		AS.volume = 0.1f * Limit(1f, Mathf.Sqrt(ratio), 2);
		AS.PlayOneShot(ac);
	}


	public GameObject GetCapturedObject() {
		return capturedObj;
	}
	public Vector2 GetCapturedPos() {
		if (capturedObj)
			return capturedObj.transform.position;
		return aimPos;
	}
	public bool IsAutoAim() {
		return autoAim;
	}
	public void SetAutoAim(bool v) {
		autoAim = v;
	}
	public int GetBulletID() {
		return bulletID_;
	}
	public void UpdateBulletID() {
		bulletID_++;
	}
	public Vector2 GetAimPos() {
		return aimPos;
	}
	public Transform GetParent() {
		return parent;
	}
	public float GetNowBlur() {
		return nowBlur;
	}
	public float GetBlurRate() {
		return blurRate;
	}
	public float GetGunLength() {
		return gunLength;
	}
	public Vector2 GetRangeBlur() {
		return rangeBlur;
	}
	public bool IsInfBullet() {
		return infBullet;
	}
	public bool IsReloading() {
		return reloading;
	}
	public int GetMagazine() {
		return magazine;
	}
	public float GetNowReloadTime() {
		return nowReloadTime;
	}
	public float GetReloadCompleteRate() {
		if (!IsReloading()) return 1f;
		return nowReloadTime / reloadTime;
	}

}
