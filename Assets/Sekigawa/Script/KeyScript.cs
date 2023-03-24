using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {


	/*
	--------使い方--------

	KeyScript.InputDown(KeyScript.Dir.Reload)
	↑はR(リロードボタン)を押下した瞬間だけTrue

	KeyScript.InputOn(KeyScript.Dir.Down)
	↑はS(下キー)を押下している間True

	*/

	public static KeyCode Left = KeyCode.A;
    public static KeyCode Right = KeyCode.D;
    public static KeyCode Up = KeyCode.W;
    public static KeyCode Down = KeyCode.S;
    public static KeyCode Fire = KeyCode.Mouse0;
    public static KeyCode Aim = KeyCode.Mouse1;
    public static KeyCode Dash = KeyCode.LeftShift;
    public static KeyCode Switch = KeyCode.Q;
    public static KeyCode Weapon1 = KeyCode.Alpha1;
    public static KeyCode Weapon2 = KeyCode.Alpha2;
    public static KeyCode Action = KeyCode.F;
    public static KeyCode Reload = KeyCode.R;
    public static KeyCode Dodge = KeyCode.Space;
    public static KeyCode Inventory = KeyCode.E;
    public static KeyCode Map = KeyCode.Tab;

	static bool enable = true;

    public enum Dir {
		Left,
		Right,
		Up,
		Down,
		Fire,
		Aim,
		Dash,
		Switch,
		Weapon1,
		Weapon2,
		Action,
		Reload,
		Dodge,
		Inventory,
		Map,
	}
	public static bool InputDown(Dir d) {
		if (!enable)
			return false;

		if (d == Dir.Left) return Input.GetKeyDown(Left);
		if (d == Dir.Right) return Input.GetKeyDown(Right);
		if (d == Dir.Up) return Input.GetKeyDown(Up);
		if (d == Dir.Down) return Input.GetKeyDown(Down);
		if (d == Dir.Fire) return Input.GetKeyDown(Fire);
		if (d == Dir.Aim) return Input.GetKeyDown(Aim);
		if (d == Dir.Dash) return Input.GetKeyDown(Dash);
		if (d == Dir.Switch) return Input.GetKeyDown(Switch);
		if (d == Dir.Weapon1) return Input.GetKeyDown(Weapon1);
		if (d == Dir.Weapon2) return Input.GetKeyDown(Weapon2);
		if (d == Dir.Action) return Input.GetKeyDown(Action);
		if (d == Dir.Reload) return Input.GetKeyDown(Reload);
		if (d == Dir.Dodge) return Input.GetKeyDown(Dodge);
		if (d == Dir.Inventory) return Input.GetKeyDown(Inventory);
		if (d == Dir.Map) return Input.GetKeyDown(Map);

		return false;
	}


	public static bool InputUp(Dir d) {
		if (!enable)
			return false;

		if (d == Dir.Left) return Input.GetKeyUp(Left);
		if (d == Dir.Right) return Input.GetKeyUp(Right);
		if (d == Dir.Up) return Input.GetKeyUp(Up);
		if (d == Dir.Down) return Input.GetKeyUp(Down);
		if (d == Dir.Fire) return Input.GetKeyUp(Fire);
		if (d == Dir.Aim) return Input.GetKeyUp(Aim);
		if (d == Dir.Dash) return Input.GetKeyUp(Dash);
		if (d == Dir.Switch) return Input.GetKeyUp(Switch);
		if (d == Dir.Weapon1) return Input.GetKeyUp(Weapon1);
		if (d == Dir.Weapon2) return Input.GetKeyUp(Weapon2);
		if (d == Dir.Action) return Input.GetKeyUp(Action);
		if (d == Dir.Reload) return Input.GetKeyUp(Reload);
		if (d == Dir.Dodge) return Input.GetKeyUp(Dodge);
		if (d == Dir.Inventory) return Input.GetKeyUp(Inventory);
		if (d == Dir.Map) return Input.GetKeyUp(Map);

		return false;
	}


	public static bool InputOn(Dir d) {
		if (!enable)
			return false;

		if (d == Dir.Left) return Input.GetKey(Left);
		if (d == Dir.Right) return Input.GetKey(Right);
		if (d == Dir.Up) return Input.GetKey(Up);
		if (d == Dir.Down) return Input.GetKey(Down);
		if (d == Dir.Fire) return Input.GetKey(Fire);
		if (d == Dir.Aim) return Input.GetKey(Aim);
		if (d == Dir.Dash) return Input.GetKey(Dash);
		if (d == Dir.Switch) return Input.GetKey(Switch);
		if (d == Dir.Weapon1) return Input.GetKey(Weapon1);
		if (d == Dir.Weapon2) return Input.GetKey(Weapon2);
		if (d == Dir.Action) return Input.GetKey(Action);
		if (d == Dir.Reload) return Input.GetKey(Reload);
		if (d == Dir.Dodge) return Input.GetKey(Dodge);
		if (d == Dir.Inventory) return Input.GetKey(Inventory);
		if (d == Dir.Map) return Input.GetKey(Map);

		return false;
	}

	public static bool GetEnable() {
		return enable;
	}
	public static void SetEnable(bool e) {
		enable = e;
	}
	public static void ToggleEnable() {
		enable = !enable;
	}


}
