using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_RangeUp : WeaponChip {


	float value1, value2;

	void Awake() {
		Name = "�����O�o����";
	}

	protected override void SetupChip() {
		value1 = base.GetStdMultiply(20f, level);
		value2 = base.GetStdMultiply(3f, level);
		string str = "�e�̔�ԋ�����" + value1.ToString("F1") + "m�������A" +
					 "���x��" + ConvertPercentMultiply(value2) + "���܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�e�̔�ԋ�����" + base.GetStdMultiply(20f, level).ToString("F1") + "m�������A" +
					 "���x��" + ConvertPercentMultiply(base.GetStdMultiply(3f, level)) + "���܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.maxRange += value1;
		base.ws.speed *= value2;

	}

	protected override void OnChipUnLoad() {
		base.ws.maxRange -= value1;
		base.ws.speed /= value2;

	}
}