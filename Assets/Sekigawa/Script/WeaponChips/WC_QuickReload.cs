using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_QuickReload : WeaponChip {

	float value;

	void Awake() {
		Name = "�f�t�g�n���h";
	}

	protected override void SetupChip() {
		value = base.GetStdMultiply(8, level);
		string str = "�����[�h���x��" + ConvertPercentMultiply(value) + "���܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�����[�h���x��" + ConvertPercentMultiply(base.GetStdMultiply(8, level)) + "���܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.reloadTime /= value;

	}
	protected override void OnChipUnLoad() {
		base.ws.reloadTime *= value;

	}

}
