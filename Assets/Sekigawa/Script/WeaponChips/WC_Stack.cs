using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Stack : WeaponChip {

	float value1;
	int value2;

	void Awake() {
		Name = "�G�N�X�y���G���X";
	}

	protected override void SetupChip() {

		value1 = base.GetStdMultiply(8f, 19f, level, 25);
		value2 = 8 - (int)Mathf.Ceil((level + 1) / 5f);
		string str = "�e��" + (value2 - 1) + "�񓖂Ă�Ǝ��ɓ��Ă��e�̃_���[�W��" + ConvertPercentMultiply(value1) + "���܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�e��" + (8 - (int)Mathf.Ceil((level + 1) / 5f) - 1) + "�񓖂Ă�Ǝ��ɓ��Ă��e�̃_���[�W��" + ConvertPercentMultiply(base.GetStdMultiply(8f, 19f, level, 25)) + "���܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.stackPower_ = value1;
		base.ws.stack_ = value2;

	}

	protected override void OnChipUnLoad() {
		base.ws.stackPower_ = 5;
		base.ws.stack_ = 0;

	}
}