using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_BlurAccUp : WeaponChip {

	float value1, value2;

	void Awake() {
		Name = "�R���y���Z�C�^�[";
	}

	protected override void SetupChip() {
		value1 = 1f / base.GetStdMultiply(8f, level);
		value2 = base.GetStdMultiply(5f, level);
		string str = "�ˌ����ɉ��Z�����u����" + ConvertPercentMultiply(value1) + "���A" +
					 "��ˌ����Ƀu�����񕜂��鑬�x��" + ConvertPercentMultiply(value2) + "���܂��B";
		base.SetDescription(str);
	}


	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�ˌ����ɉ��Z�����u����" + ConvertPercentMultiply(1f / base.GetStdMultiply(8f, level)) + "���A" +
					 "��ˌ����Ƀu�����񕜂��鑬�x��" + ConvertPercentMultiply(base.GetStdMultiply(5f, level)) + "���܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.blurAccRate *= value1;
		base.ws.blurRechargeRate *= value2;
		
	}

	protected override void OnChipUnLoad() {
		base.ws.blurAccRate /= value1;
		base.ws.blurRechargeRate /= value2;
	}
}