using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float wakeUpX; // Determina a distância em X que o personagem deve estar do inimigo para que o mesmo o persiga
	public float wakeUpY; // Determina a distância em Y que o personagem deve estar do inimigo para que o mesmo o persiga
	public float speed; // Determina a velocidade com que o inimigo se move
	public int hp; // Valor de vida do inimigo
	public GameObject drop; // Referência ao item que o inimigo deixará para trás ao morrer
	public Consumable consumable; // O item específico que será deixado para trás
	public int attack; // Valor de dano que o inimigo causará ao personagem quando se colidirem
	public float knockback; // Distância que o inimigo empurrará o personagem quando se colidirem
	public int gold; // Quantidade de ouro que o inimigo concede ao morrer

	protected Transform player; // Referência ao personagem
	protected Rigidbody2D rb; // Componente que adiciona física
	protected Animator anim; // Gerenciador de animação
	protected Vector3 playerDistance; // Determina a distãncia entre o inimigo e o personagem
	protected bool facingRight = false; // Determina a direção em que o inimigo está virado
	protected bool isDead = false; // Determina se o inimigo está morto
	protected SpriteRenderer sprite; // Referência à imagem do inimigo

	// Inicialização
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform; // Lê a posição do personagem no cenário
		rb = GetComponent<Rigidbody2D>(); // Inicializa a física do inimigo
		anim = GetComponent<Animator>(); // Inicializa o gerenciador de animação do inimigo
		sprite = GetComponent<SpriteRenderer>(); // Inicializa a imagem do inimigo
	}

	// Método que vira a imagem do inimigo
	protected void Flip () {
		facingRight = !facingRight; // Inverte o valor de facingRight, fazendo o inimigo virar a esquerda se estiver virado a direita e vice-versa
		Vector3 scale = transform.localScale; // Recebe o valor da escala do inimigo
		scale.x *= -1; // Inverte o valor x (horizontal) da escala entre valores positivos e negativos
		transform.localScale = scale; // Atualiza o valor da escala do inimigo com o novo valor positivo ou negativo pra virar sua imagem
	}

	// Método chamado ao inimigo entrar em contato com um ataque do personagem
	public void TakeDamage(int damage) {
		hp -= damage; // Diminui a vida do inimigo pelo dano do ataque
		if (hp <= 0) { // Caso sua vida chegue a zero
			isDead = true; // Colocado em estado de morte
			rb.velocity = Vector2.zero; // Seu corpo para de se mover
			anim.SetTrigger("Dead"); // Roda sua animação de morte
			FindObjectOfType<Player>().gold += gold;
			if (consumable != null) { // Caso algum consumível tenha sido atribuído ao inimigo
				GameObject tempItem = Instantiate(drop, transform.position, transform.rotation); // Instancia um objeto no cenário
				tempItem.GetComponent<MapConsumable>().consumable = consumable; // Atribui ao objeto um consumível
			}
		} else { // Caso contrário
			StartCoroutine(DamageCoroutine()); // Roda a rotina de dano
		}
	}

	// Rotina de dano
	IEnumerator DamageCoroutine() {
		// Faz com que a imagem do inimigo torne-se vermelha brevemente
		for (float i = 0; i < 0.2f; i+= 0.2f) {
			sprite.color = Color.red;
			yield return new WaitForSeconds(0.1f);
			sprite.color = Color.white;
			yield return new WaitForSeconds(0.1f);
		}
	}

	// Método que remove o inimigo do cenário
	public void DestroyEnemy() {
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		// Concede ao personagem a habilidade Morte e a adiciona ao inventário assim que ele matar um inimigo
		if (!player.deathSkill)
			player.SetPlayerSkill(PlayerSkill.death);
			Destroy(gameObject);
	}

	// Método chamado ao inimigo colidir com o personagem
	protected void OnCollisionEnter2D(Collision2D other) {
		Player player = other.gameObject.GetComponent<Player>();
		if (player != null) {
			player.TakeDamage(attack); // Causa dano ao personagem
			player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * knockback * (playerDistance.x / Mathf.Abs(playerDistance.x)), ForceMode2D.Impulse); // Empurra o personagem uma determinada distância
		}
	}
}
