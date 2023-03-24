using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Weighting : WeaponChip {

	
	float value1, value2, magazine1, magazine2;
	bool isMultiply;

	void Awake() {
		Name = "�d�@�֏e�@�\";
	}

	protected override void SetupChip() {
		value1 = base.GetStdMultiply(11f, level);
		value2 = 0.5f;
		string str = "�e�̒e�e�ʂ�" + (value1 * 100f).ToString("F0") + "���܂���" + (level * 4) + "�������܂����A�ړ����x��" + ConvertPercentMultiply(value2) + "���܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�e�̒e�e�ʂ�" + (base.GetStdMultiply(11f, level) * 100f).ToString("F0") + "���܂���" + (level * 4) + "�������܂����A�ړ����x��" + ConvertPercentMultiply(0.5f) + "���܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		magazine1 = (int)(base.ws.maxMagazine * value1);
		magazine2 = base.ws.maxMagazine + level * 4;
		isMultiply = magazine1 > magazine2;

		base.ws.moveSpeedMultiply *= value2;

		if (isMultiply)
			base.ws.maxMagazine = Mathf.RoundToInt(base.ws.maxMagazine * value1);
		else
			base.ws.maxMagazine += level * 4;

	}

	protected override void OnChipUnLoad() {
		if (isMultiply)
			base.ws.maxMagazine = Mathf.RoundToInt(base.ws.maxMagazine / value1);
		else
			base.ws.maxMagazine -= level * 4;

		base.ws.moveSpeedMultiply /= value2;
	}
	
}
