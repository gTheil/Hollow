using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public GameObject pauseMenu;
	public Transform cursor;
	public Transform[] menuOptions;

	private bool active = false;
	private int optionID = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.P)) {
			active = !active;
			optionID = 0;
			if (active) {
				pauseMenu.SetActive(true);
			} else {
				pauseMenu.SetActive(false);
			}
		}

		if (active) {
			Vector3 cursorPosition = menuOptions[optionID].position;
			cursor.position = new Vector3(cursorPosition.x - 100, cursorPosition.y, cursorPosition.z);
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				optionID++;
				if (optionID > (menuOptions.Length - 1)) {
					optionID = 0;
				}
			} else if (Input.GetKeyDown(KeyCode.UpArrow)) {
				optionID--;
				if (optionID < 0) {
					optionID = (menuOptions.Length - 1);
				}
			}
		}

	}
}
