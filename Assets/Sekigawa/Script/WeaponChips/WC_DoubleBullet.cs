using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_DoubleBullet : WeaponChip {

	float value, bfrCount, damageMultiply;

	void Awake() {
		Name = "ダブルバレル";
	}

	protected override void SetupChip() {
		value = level * 0.5f;
		string str = "弾が";
		str += level >= 2 ? "追加で" + (int)(value) + "個発射され" : "";
		str += level % 2 == 0 ? "ます。" : "、確率でもう１個追加されます。";
		str += "総合的なダメージは変化しません。";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "弾が";
		str += level >= 2 ? "追加で" + (int)(level * 0.5f) + "個発射され" : "";
		str += level % 2 == 0 ? "ます。" : "、確率でもう１個追加されます。";
		str += "総合的なダメージは変化しません。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		bfrCount = base.ws.count;
		base.ws.count += value;
		damageMultiply = bfrCount / base.ws.count;
		base.ws.damage *= damageMultiply;
	}

	protected override void OnChipUnLoad() {

		base.ws.count -= value;
		base.ws.damage /= damageMultiply;
	}

	bool IsInt(float a) {
		return (int)a == a;
	}
}