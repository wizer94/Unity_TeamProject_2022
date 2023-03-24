using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_ContinuityHit : WeaponChip {

	float value1, value2;
	int missable;

	void Awake() {
		Name = "�A���X�g�b�p�u��";
	}

	protected override void SetupChip() {
		value1 = base.GetStdMultiply(2f, 10f, level, 25);
		value2 = base.GetStdMultiply(1.25f, 2f, level, 25);
		string str = "�A���q�b�g���Ƃɏe����������A" +
					 "�ő�Ń_���[�W��" + ConvertPercentMultiply(value1) + "�A" +
					 "���˃��[�g��" + ConvertPercentMultiply(value2) + "���܂��B";
		missable = level / 5;
		if (missable >= 1)
			str += "\n" + missable + "��O���Ă����ʂ��p������܂��B";
		base.SetDescription(str);
	}


	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�A���q�b�g���Ƃɏe����������A" +
					 "�ő�Ń_���[�W��" + ConvertPercentMultiply(base.GetStdMultiply(2f, 10f, level, 25)) + "�A" +
					 "���˃��[�g��" + ConvertPercentMultiply(base.GetStdMultiply(1.25f, 2f, level, 25)) + "���܂��B";
		if (level / 5 >= 1)
			str += "\n" + level / 5 + "��O���Ă����ʂ��p������܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.continuityDamage = value1;
		base.ws.continuityPerShot = value2;
		base.ws.continuityMissable = missable;

	}

	protected override void OnChipUnLoad() {
		base.ws.continuityDamage = 1f;
		base.ws.continuityPerShot = 1f;
		base.ws.continuityMissable = 0;
	}
}
