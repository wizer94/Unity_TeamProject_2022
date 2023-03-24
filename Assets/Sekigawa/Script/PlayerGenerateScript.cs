using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerateScript : MonoBehaviour
{

	public GameObject player_;
	public static GameObject player;
	public GameObject reticle_;
	public static GameObject reticle;

	void Start() {
		if (player == null) {
			player = Instantiate(player_, transform.position, Quaternion.identity);
			player.name = player.name.Replace("(Clone)", "");
			DontDestroyOnLoad(player);
		}
		else {
			player.gameObject.SetActive(true);
		}

		if (reticle == null) {
			reticle = Instantiate(reticle_, transform.position, Quaternion.identity);
			reticle.name = reticle.name.Replace("(Clone)", "");
			DontDestroyOnLoad(reticle);
		}
	}

}
