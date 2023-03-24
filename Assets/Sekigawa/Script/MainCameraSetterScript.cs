using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraSetterScript : MonoBehaviour
{
	
	void Start() {
		GetComponent<Canvas>().worldCamera = Camera.main;
	}
	
	void Update() {
		
	}
}
