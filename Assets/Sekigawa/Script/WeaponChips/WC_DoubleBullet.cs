using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_DoubleBullet : WeaponChip {

	float value, bfrCount, damageMultiply;

	void Awake() {
		Name = "�_�u���o����";
	}

	protected override void SetupChip() {
		value = level * 0.5f;
		string str = "�e��";
		str += level >= 2 ? "�ǉ���" + (int)(value) + "���˂���" : "";
		str += level % 2 == 0 ? "�܂��B" : "�A�m���ł����P�ǉ�����܂��B";
		str += "�����I�ȃ_���[�W�͕ω����܂���B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�e��";
		str += level >= 2 ? "�ǉ���" + (int)(level * 0.5f) + "���˂���" : "";
		str += level % 2 == 0 ? "�܂��B" : "�A�m���ł����P�ǉ�����܂��B";
		str += "�����I�ȃ_���[�W�͕ω����܂���B";
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