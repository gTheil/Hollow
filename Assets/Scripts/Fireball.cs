using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	public float speed; // Determina a velocidade com que a bola de fogo se move
	public int attack; // Valor de dano que a bola de fogo causará ao personagem quando se colidirem
	public Vector2 knockback; // Distância que a bola de fogo empurrará o personagem quando se colidirem

	protected Rigidbody2D rb;

	// Método que vira a imagem da bola de fogo
	protected void Flip () {
		Vector3 scale = transform.localScale; // Recebe o valor da escala do inimigo
		scale.x *= -1; // Inverte o valor x (horizontal) da escala entre valores positivos e negativos
		transform.localScale = scale; // Atualiza o valor da escala do inimigo com o novo valor positivo ou negativo para virar sua imagem
	}
}
