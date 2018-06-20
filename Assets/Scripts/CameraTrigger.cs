using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {

	public Vector2 maxXAndY; // Coordenadas X e Y máximas e mínimas que a câmera pode ter
	public Vector2 minXAndY; // Coordenadas X e Y mínimas que a câmera pode ter

	private CameraFollow cameraFollow; // Referência ao script que controla a movimentação da câmera

	// Inicialização
	void Start () {
		cameraFollow = FindObjectOfType<CameraFollow>(); // Inicializa a câmera
	}

	// Ao colidir com o personagem
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			cameraFollow.maxXAndY = maxXAndY; // Passa as coordenadas máximas da transição para a câmera
			cameraFollow.minXAndY = minXAndY; // Passa as coordenadas mínimas da transição para a câmera
		}
	}

}
