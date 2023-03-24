using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_BlurRelief : WeaponChip {

	float valueMin, valueMax;

	void Awake() {
		Name = "�X�^�r���C�U�[";
	}

	protected override void SetupChip() {
		valueMin = 1f / base.GetStdMultiply(10f, level);
		valueMax = 1f / base.GetStdMultiply(5f, level);
		string str = "�ˌ����̃u����" + ConvertPercentMultiply(valueMax) + "���܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�ˌ����̃u����" + ConvertPercentMultiply(1f / base.GetStdMultiply(5f, level)) + "���܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.minBlur *= valueMin;
		base.ws.maxBlur *= valueMax;
		base.ws.cohesive += level / 5;
		
	}

	protected override void OnChipUnLoad() {

		base.ws.minBlur /= valueMin;
		base.ws.maxBlur /= valueMax;
		base.ws.cohesive -= level / 5;

	}
}