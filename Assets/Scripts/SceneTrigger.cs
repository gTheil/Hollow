using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			SceneManager.LoadScene("EndScene");
		}
	}
}
