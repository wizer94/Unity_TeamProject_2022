using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_SemiautoSwitch : WeaponChip {

	float value, downValue, upValue;

	void Awake() {
		Name = "�g�����X�t�H�[��";
	}

	protected override void SetupChip() {

		value = base.GetStdMultiply(3.5f, level);
		string str = "�e���A��/�P�˂��؂�ւ��A�_���[�W�┭�˃��[�g���ω����܂��B";
		base.SetDescription(str);
		float multiply = 2f;
		downValue = 1f / multiply;
		upValue = multiply * base.GetStdMultiply(4f, level);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�e���A��/�P�˂��؂�ւ��A�_���[�W�┭�˃��[�g���ω����܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		if (!ws.semiauto_def) {
			base.ws.semiauto = true;
			base.ws.perShot *= value;
		}
		else {
			base.ws.semiauto = false;
			base.ws.perShot *= downValue;
			base.ws.damage *= upValue;
		}

	}

	protected override void OnChipUnLoad() {

		if (!ws.semiauto_def) {
			base.ws.semiauto = false;
			base.ws.perShot /= value;
		}
		else {
			base.ws.semiauto = true;
			base.ws.perShot /= downValue;
			base.ws.damage /= upValue;
		}

	}

}
