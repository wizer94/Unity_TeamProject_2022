using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Proximity : WeaponChip {

	float max;
	AnimationCurve ac;

	void Awake() {
		Name = "���������";
	}

	protected override void SetupChip() {

		max = base.GetStdMultiply(10f, level);
		float maxDmgEnd = 0.1f;
		float oneDmgPoint = 0.5f;
		float max2one = (max - 1f) / (maxDmgEnd - oneDmgPoint);
		float one2zero = 1f / (oneDmgPoint - 1f);
		Keyframe[] keys = {
			new Keyframe(0, max, 0, 0),
			new Keyframe(0.1f, max, 0, max2one),
			new Keyframe(0.5f, 1, max2one, one2zero),
			new Keyframe(1, 0f, one2zero, 0)
		};
		ac = new AnimationCurve(keys);
		string str = "�e�̎��ߋ����̃_���[�W��" + ConvertPercentMultiply(max) + "���܂����A" +
					 "�������L�т�ɂꌸ���������Ȃ�܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�e�̎��ߋ����̃_���[�W��" + ConvertPercentMultiply(base.GetStdMultiply(10f, level)) + "���܂����A" +
					 "�������L�т�ɂꌸ���������Ȃ�܂��B";
		rtv += str;
		return rtv;
	}


	protected override void OnChipLoad(){
		SetupChip();

		base.ws.attenuationMultiply = ac;

	}

	protected override void OnChipUnLoad() {

		base.ws.attenuationMultiply = AnimationCurve.Linear(0, 1, 1, 1);
	}
}
