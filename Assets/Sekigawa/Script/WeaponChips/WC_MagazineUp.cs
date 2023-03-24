using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_MagazineUp : WeaponChip {

	float value, magazine1, magazine2;
	bool isMultiply;

	void Awake() {
		Name = "�g���}�K�W��";
	}

	protected override void SetupChip() {
		value = base.GetStdMultiply(5f, level);
		string str = "�e�̒e�e�ʂ�" + (value * 100f).ToString("F0") + "���܂���" + level + "�������܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�e�̒e�e�ʂ�" + (base.GetStdMultiply(5f, level) * 100f).ToString("F0") + "���܂���" + level + "�������܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		magazine1 = (int)(base.ws.maxMagazine * value);
		magazine2 = base.ws.maxMagazine + level;
		isMultiply = magazine1 > magazine2;

		if (isMultiply)
			base.ws.maxMagazine = Mathf.RoundToInt(base.ws.maxMagazine * value);
		else
			base.ws.maxMagazine += level;

	}

	protected override void OnChipUnLoad() {
		if (isMultiply)
			base.ws.maxMagazine = Mathf.RoundToInt(base.ws.maxMagazine / value);
		else
			base.ws.maxMagazine -= level;

	}

}
