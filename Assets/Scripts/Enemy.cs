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
	public Vector2 knockback;
	public int gold; // Quantidade de ouro que o inimigo concede ao morrer
	public bool facingRight; // Determina a direção em que o inimigo está virado
	public bool buffable = false;
	public float buffDuration;
	public bool redBuffOn = false;
	public bool blueBuffOn = false;
	public Vector3 playerDistance; // Determina a distãncia entre o inimigo e o personagem

	protected Transform player; // Referência ao personagem
	protected Rigidbody2D rb; // Componente que adiciona física
	protected Animator anim; // Gerenciador de animação
	protected bool isDead = false; // Determina se o inimigo está morto
	protected SpriteRenderer sprite; // Referência à imagem do inimigo
	protected bool isBuffed = false;
	protected float buffEnd;
	protected bool woke = false;

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
		transform.localScale = scale; // Atualiza o valor da escala do inimigo com o novo valor positivo ou negativo para virar sua imagem
	}

	// Método chamado ao inimigo entrar em contato com um ataque do personagem
	public void TakeDamage(int damage) {
		if (redBuffOn)
			hp -= (damage * 2); // Diminui a vida do inimigo pelo dano do ataque
		else if (blueBuffOn)
			hp -= (damage / 2);
		else
			hp -= damage;
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
			player.SetPlayerSkill (player.database.GetSkill (3));
		Destroy(gameObject);
	}

	// Método chamado ao inimigo colidir com o personagem
	protected void OnCollisionEnter2D(Collision2D other) {
		Player player = other.gameObject.GetComponent<Player>();
		if (player != null) {
			if (redBuffOn) {
				player.TakeDamage ((attack * 2)); // Causa dano ao personagem
				if (!player.redBuffSpell)
					player.SetPlayerSpell (player.database.GetSpell (2));
			} else
				player.TakeDamage (attack);
			Vector2 kbForce = new Vector2(knockback.x * (playerDistance.x / Mathf.Abs(playerDistance.x)), knockback.y);
			player.GetComponent<Rigidbody2D>().AddForce(kbForce, ForceMode2D.Impulse); // Empurra o personagem uma determinada distância
		}
	}
}
