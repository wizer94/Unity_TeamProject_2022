using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Through : WeaponChip
{

	float value;

	void Awake() {
		Name = "�V���[�v�o���b�g";
	}

	protected override void SetupChip() {
		if (level <= 5)
			value = base.GetStdMultiply(1f, 2f, level, 5) / 2f;
		else
			value = base.GetStdMultiply(1f, 3f, level - 5, 20);

		string str = "�e���G���ђʂ���悤�ɂȂ�";
		if (value != 1) {
			str += value < 1 ? "�܂����A" : "�A";
			str += "�ђʂ̂��тɃ_���[�W��" + ConvertPercentMultiply(value) + "���܂��B";
		}
		else {
			str += "�܂��B";
		}
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "";
		if (level <= 5) {
			str = "�e���G���ђʂ���悤�ɂȂ�";
			if (level != 5) {
				str += "�܂����A�ђʂ̂��тɃ_���[�W��" + ConvertPercentMultiply(base.GetStdMultiply(1f, 2f, level, 5) / 2f) + "���܂��B";
			}
			else {
				str += "�܂��B";
			}
		}
		else {
			str = "�e���G���ђʂ���悤�ɂȂ�A�ђʂ̂��тɃ_���[�W��" + base.GetStdMultiply(1f, 3f, level - 5, 20) + "���܂��B";
		}
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.through = value;

	}

	protected override void OnChipUnLoad() {
		base.ws.through = 0;

	}
}
