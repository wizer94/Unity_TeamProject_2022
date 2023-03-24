using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Fire : WeaponChip {

	int value1, value2;

	void Awake() {
		Name = "�����g�_�E��";
	}

	protected override void SetupChip() {
		value1 = level / 5 + 1;
		value2 = level % 5 + 1 + value1;

		float sum = 40f * value1 * value1 * value2;

		string str = "�G�ɓ�����Ɣ��΂��A" + value2 + "�b�Ԃɂ킽���Čp���_���[�W�A\n" +
					 "���v�Ŗ�" + sum + "�_���[�W��^���܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�G�ɓ�����Ɣ��΂��A" + (level % 5 + 1 + value1) + "�b�Ԃɂ킽���Čp���_���[�W�A\n" +
					 "���v�Ŗ�" + (40f * (level / 5 + 1) * (level / 5 + 1) * (level % 5 + 1 + (level / 5 + 1))) + "�_���[�W��^���܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.fireLevel = value1;
		base.ws.fireTime = value2;
	}
	protected override void OnChipUnLoad() {
		base.ws.fireLevel = 0;
		base.ws.fireTime = 0;
	}
}