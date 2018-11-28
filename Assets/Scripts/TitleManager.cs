using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	public GameObject titleMenu;
	public GameObject introMenu;

	private bool titleActive = true;
	private bool introActive = false;

	// Update is called once per frame
	void Update () {
		if (titleActive) {
			if (Input.GetButtonDown("Submit")) {
				SceneManager.LoadScene("GameScene");
			} else if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();
			} else if (Input.GetKeyDown (KeyCode.Space)) {
				titleMenu.SetActive (false);
				titleActive = false;
				introMenu.SetActive (true);
				introActive = true;
			}
		}
		if (introActive) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				introMenu.SetActive (false);
				introActive = false;
				titleMenu.SetActive (true);
				titleActive = true;
			}
		}
	}
}
