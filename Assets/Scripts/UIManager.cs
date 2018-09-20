using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public GameObject pauseMenu; // Referência ao menu principal
	public GameObject shopMenu; // Referência ao menu principal
	public Transform pauseCursor; // Referência ao cursor para a navegação no menu
	public Transform shopCursor; // Referência ao cursor para a navegação no menu
	public Transform[] menuOptions; // Array de opções selecionáveis no menu principal
	public GameObject optionPanel; // Painel que contém as opções selecionáveis no menu principal
	public GameObject itemPanel; // Painel que contém a lista de itens a serem exibidos
	public GameObject shopPanel; // Painel que contém a lista de itens a serem exibidos
	public GameObject itemList; // Lista de itens a serem exibidos
	public RectTransform pauseContent; // Referência ao conteúdo das opções do menu
	public RectTransform shopContent; // Referência ao conteúdo das opções do menu
	public List<ItemList> playerItems; // Referência à classe ItemList para manipulação
	public List<ItemList> shopItems; // Referência à classe ItemList para manipulação
	public Text descriptionText; // Referência ao texto de descrição de um item
	public Text shopDescriptionText; // Referência ao texto de descrição de um item
	public Scrollbar scrollVertical; // Variável que controla a rolagem vertical da lista de itens
	public Text hpText, mpText, goldText, atkText, msgText; // Variáveis que contêm os textos dos atributos do personagem
	public Text hpUI, mpUI, goldUI, itemUI; // Variáveis que contêm os textos da barra de status
	public Image consumableImage; // Imagem do consumível equipado

	private bool pauseActive = false; // Determina se o jogador está com o menu aberto
	private bool shopActive = false; // Determina se o jogador está com o menu aberto
	private int optionID = 0; // Posição do cursor no array de opções selecionáveis
	private Inventory playerInventory; // Referência ao inventário do jogador
	private Inventory shopInventory; // Referência ao inventário do jogador
	private bool itemListActive = false; // Determina se a lista de itens está aberta
	private bool shopListActive = false; // Determina se a lista de itens está aberta
	private Player player; // Referência ao jogador
	private bool msgActive = false; // Determina se a mensagem na tela está ativa
	private float msgTimer; // Contagem para que a mensagem na tela desapareça

	// Inicialização de personagem e inventário
	void Start () {
		playerInventory = Inventory.playerInventory;
		shopInventory = Inventory.shopInventory;
		player = FindObjectOfType<Player>();
	}
	
	// Atualizado a cada frame
	void Update () {

		UpdateUI();
		UpdateAttributes();

		// Caso a mensagem esteja ativa
		if (msgActive) {
			Color color = msgText.color;
			color.a += 1f * Time.deltaTime; // Aumenta o alpha da mensagem
			msgText.color = color;
			// Quando o alpha estiver maior ou igual a 1
			if (color.a >= 1) {
				msgActive = false;
				msgTimer = 0; // Inicia a contagem para a mensagem desaparecer
			}
		// Após a mensagem ser exibida
		}
		else if (!msgActive) {
			msgTimer += Time.deltaTime; // Incrementa a contagem
			// Quando a contagem ultrapassar dois segundos
			if (msgTimer >= 1f) {
				Color color = msgText.color;
				color.a -= 1f * Time.deltaTime; // Diminui o alpha da mensagem
				msgText.color = color;
				// Quando o alpha for menor ou igual a 0
				if (color.a <= 0) {
					msgText.text = ""; // Apaga o texto da mensagem
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.P)) { // Ao jogador pressionar o botão de menu
			pauseActive = !pauseActive; // Abre o menu se o mesmo estiver fechado, fecha-o se estiver aberto
			optionID = 0; // Reseta a posição do cursor para a primeira opção selecionável
			optionPanel.SetActive(true); // Ativa o painel de opções
			itemPanel.SetActive(false); // Desativa o painel de itens
			itemListActive = false; // Desativa a lista de itens
			descriptionText.text = ""; // Limpa o campo de descrição no menu
			if (pauseActive) {
				pauseMenu.SetActive(true);
				Time.timeScale = 0; // Pausa o jogo
			} else {
				pauseMenu.SetActive(false);
				Time.timeScale = 1; // Despausa o jogo
			}
		}

		if (Input.GetKeyDown(KeyCode.O)) { // Ao jogador pressionar o botão de menu
			shopActive = !shopActive; // Abre o menu se o mesmo estiver fechado, fecha-o se estiver aberto
			optionID = 0; // Reseta a posição do cursor para a primeira opção selecionável
			shopListActive = true; // Desativa a lista de itens
			shopPanel.SetActive (true); // Ativa o painel de itens no menu
			RefreshItemList (); // Limpa a lista de itens
			UpdateItemList (3); // Gera a os itens dentro da lista de acordo com a opção selecionada
			UpdateDescription(1);
			if (shopActive) {
				shopMenu.SetActive(true);
				Time.timeScale = 0; // Pausa o jogo
			} else {
				shopMenu.SetActive(false);
				Time.timeScale = 1; // Despausa o jogo
			}
		}

		if (pauseActive) { // Se o menu estiver aberto
			Vector3 cursorPosition = new Vector3(); // Variável que determina a posição do cursor
			if (!itemListActive) { // Caso a lista de itens não esteja ativa
				cursorPosition = menuOptions[optionID].position; // Determina a posição das opções selecionáveis
				pauseCursor.position = new Vector3 (cursorPosition.x - 100, cursorPosition.y, cursorPosition.z); // Determina a posição do cursor em relação às opções do menu
			} else if (itemListActive && playerItems.Count > 0) {
				cursorPosition = playerItems[optionID].transform.position; // Determina a posição dos itens na lista
				pauseCursor.position = new Vector3(cursorPosition.x - 75, cursorPosition.y, cursorPosition.z); // Determina a posição do cursor em relação aos itens da lista
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) { // Ao jogador pressionar a seta para baixo
				if (!itemListActive && optionID >= menuOptions.Length - 1)
					optionID = menuOptions.Length - 1; // Impede o jogador de descer o cursor abaixo da última opção
				else if (itemListActive && optionID >= playerItems.Count - 1) {
					if (playerItems.Count == 0) // Caso não haja itens
						optionID = 0; // Impede o jogador de mover o cursor
					else
						optionID = playerItems.Count - 1; // Impede o jogador de descer o cursor abaixo do último item
				}
				else
				optionID++; // Move o cursor para a próxima opção
				if (itemListActive && playerItems.Count > 0) { // Caso a lista de itens esteja ativa e não esteja vazia
					scrollVertical.value -= (1f / (playerItems.Count - 1)); // Desce a barra de rolagem da lista
					UpdateDescription(0); // Atualiza o campo de descrição de acordo com o item selecionado
				}
			} else if (Input.GetKeyDown(KeyCode.UpArrow)) { // Ao jogador pressionar a seta para cima
				if (optionID == 0)
					optionID = 0; // Impede o jogador de subir o cursor acima da primeira opção
				else
				optionID--; // Move o cursor para a opção anterior
				if (itemListActive && playerItems.Count > 0) { // Caso a lista de itens esteja ativa e não esteja vazia
					scrollVertical.value += (1f / (playerItems.Count - 1));
					UpdateDescription(0); // Atualiza o campo de descrição de acordo com o item selecionado
				}
			}

			if (Input.GetButtonDown ("Submit") && !itemListActive) { // Ao jogador selecionar uma das opções
				optionPanel.SetActive (false); // Desativa o painel de opções do menu
				itemPanel.SetActive (true); // Ativa o painel de itens no menu
				RefreshItemList (); // Limpa a lista de itens
				UpdateItemList (optionID); // Gera a os itens dentro da lista de acordo com a opção selecionada
				optionID = 0; // Posiciona o cursor no primeiro item da lista
				if (playerItems.Count > 0) // Caso a lista de itens não esteja vazia
				UpdateDescription (0); // Exibe o texto de descrição do item selecionado
				itemListActive = true; // Ativa a lista de itens
			} else if (Input.GetButtonDown ("Submit") && itemListActive) { // Ao jogador selecionar um item dentro da lista
				if (playerItems.Count > 0) {
					UseItem(); // O item é equipado ou utilizado
				}
			}
		}

		if (shopActive) { // Se o menu estiver aberto
			Vector3 cursorPosition = new Vector3(); // Variável que determina a posição do cursor
			if (shopListActive && shopItems.Count > 0) {
				cursorPosition = shopItems[optionID].transform.position; // Determina a posição dos itens na lista
				shopCursor.position = new Vector3(cursorPosition.x - 75, cursorPosition.y, cursorPosition.z); // Determina a posição do cursor em relação aos itens da lista
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) { // Ao jogador pressionar a seta para baixo
				if (shopListActive && optionID >= shopItems.Count - 1) {
					if (shopItems.Count == 0) // Caso não haja itens
						optionID = 0; // Impede o jogador de mover o cursor
					else
						optionID = shopItems.Count - 1; // Impede o jogador de descer o cursor abaixo do último item
				}
				else
					optionID++; // Move o cursor para a próxima opção
				if (shopListActive && shopItems.Count > 0) { // Caso a lista de itens esteja ativa e não esteja vazia
					scrollVertical.value -= (1f / (shopItems.Count - 1)); // Desce a barra de rolagem da lista
					UpdateDescription(1); // Atualiza o campo de descrição de acordo com o item selecionado
				}
			} else if (Input.GetKeyDown(KeyCode.UpArrow)) { // Ao jogador pressionar a seta para cima
				if (optionID == 0)
					optionID = 0; // Impede o jogador de subir o cursor acima da primeira opção
				else
					optionID--; // Move o cursor para a opção anterior
				if (shopListActive && shopItems.Count > 0) { // Caso a lista de itens esteja ativa e não esteja vazia
					scrollVertical.value += (1f / (shopItems.Count - 1));
					UpdateDescription(1); // Atualiza o campo de descrição de acordo com o item selecionado
				}
			}
			/**
			if (Input.GetButtonDown ("Submit") && !itemListActive) { // Ao jogador selecionar uma das opções
				optionPanel.SetActive (false); // Desativa o painel de opções do menu
				itemPanel.SetActive (true); // Ativa o painel de itens no menu
				RefreshItemList (); // Limpa a lista de itens
				UpdateItemList (optionID); // Gera a os itens dentro da lista de acordo com a opção selecionada
				optionID = 0; // Posiciona o cursor no primeiro item da lista
				if (playerItems.Count > 0) // Caso a lista de itens não esteja vazia
					UpdateDescription (1); // Exibe o texto de descrição do item selecionado
				itemListActive = true; // Ativa a lista de itens
			} else if (Input.GetButtonDown ("Submit") && itemListActive) { // Ao jogador selecionar um item dentro da lista
				if (playerItems.Count > 0) {
					UseItem(); // O item é equipado ou utilizado
				}
			} */
		}

	}

	// Método que atualiza o campo de descrição do menu
	void UpdateDescription(int option){
		if (option == 0) {
			if (playerItems [optionID].skill != null) // Caso hajam habilidades na lista de itens
			descriptionText.text = playerItems [optionID].skill.description; // Atribui ao campo de descrição o texto de descrição da habilidade selecionada
			else if (playerItems [optionID].consumable != null) // Caso hajam consumíveis na lista de itens
			descriptionText.text = playerItems [optionID].consumable.description; // Atribui ao campo de descrição o texto de descrição do consumível selecionado
			else if (playerItems [optionID].key != null) // Caso hajam chaves na lista de itens
			descriptionText.text = playerItems [optionID].key.description; // Atribui ao campo de descrição o texto de descrição da chave selecionada
		} else if (option == 1) {
			if (shopItems [optionID].skill != null) // Caso hajam habilidades na lista de itens
				shopDescriptionText.text = shopItems [optionID].skill.description;
		}
	}

	// Método que exclui todos os itens de dentro da lista de itens
	void RefreshItemList () {
		for (int i = 0; i < playerItems.Count; i++) {
			Destroy(playerItems[i].gameObject);
		}
		playerItems.Clear();
		for (int j = 0; j < shopItems.Count; j++) {
			Destroy(shopItems[j].gameObject);
		}
		shopItems.Clear();
	}

	// Método que gera os itens dentro da lista de acordo com a opção selecionada
	void UpdateItemList (int option) {
		scrollVertical.value = 1; // Reseta a posição da barra de rolagem
		if (option == 0) { // Caso seja a primeira opção
			for (int i = 0; i < playerInventory.skills.Count; i++) { // Para cada habilidade no inventário
				GameObject tempItem = Instantiate(itemList, pauseContent.transform); // Instancia a lista de itens para que possa ser manipulada
				tempItem.GetComponent<ItemList>().SetUpSkill(playerInventory.skills[i]); // Adiciona a habilidade do inventário à lista de itens
				playerItems.Add(tempItem.GetComponent<ItemList>());
			}
		}
		else if (option == 1) { // Caso seja a segunda opção
			for (int i = 0; i < playerInventory.consumables.Count; i++) { // Para cada consumível no inventário
				GameObject tempItem = Instantiate(itemList, pauseContent.transform); // Instancia a lista de itens para que possa ser manipulada
				tempItem.GetComponent<ItemList>().SetUpConsumable(playerInventory.consumables[i]); // Adiciona o consumível do inventário à lista de itens
				playerItems.Add(tempItem.GetComponent<ItemList>());
			}
		}
		else if (option == 2) { // Caso seja a terceira opção
			for (int i = 0; i < playerInventory.keys.Count; i++) { // Para cada chave no inventário
				GameObject tempItem = Instantiate(itemList, pauseContent.transform); // Instancia a lista de itens para que possa ser manipulada
				tempItem.GetComponent<ItemList>().SetUpKey(playerInventory.keys[i]); // Adiciona a chave do inventário à lista de itens
				playerItems.Add(tempItem.GetComponent<ItemList>());
			}
		}
		else if (option == 3) { // Caso seja a terceira opção
			for (int i = 0; i < shopInventory.skills.Count; i++) { // Para cada chave no inventário
				GameObject tempItem = Instantiate(itemList, shopContent.transform); // Instancia a lista de itens para que possa ser manipulada
				tempItem.GetComponent<ItemList>().SetUpSkill(shopInventory.skills[i]); // Adiciona a chave do inventário à lista de itens
				shopItems.Add(tempItem.GetComponent<ItemList>());
			}
		}

	}

	// Método que atualiza os textos dos atributos no menu de acordo com os atributos do personagem
	void UpdateAttributes() {
		hpText.text = "Vida: " + player.GetHP() + "/" + player.maxHP;
		mpText.text = "Mana: " + player.GetMP() + "/" + player.maxMP;
		goldText.text = "Ouro: " + player.gold;
		atkText.text = "Ataque: " + player.GetComponentInChildren<Attack>().GetDamage();
	}

	// Método que chama funções de equipar armas/armaduras e utilizar consumíveis
	void UseItem() {
		if (playerItems[optionID].consumable != null) { // Caso hajam consumíveis na lista e um seja selecionado
			player.consumable = playerItems[optionID].consumable; // Equipa no slot de uso rápido o consumível selecionado
			consumableImage.sprite = playerItems[optionID].consumable.image; // Atualiza a imagem do item de uso rápido na barra de status
			pauseActive = false; // Fecha a tela de menu
			pauseMenu.SetActive(false); // Desativa o menu
			Time.timeScale = 1; // Despausa o jogo
		}
	}

	// Método que atualiza os textos dos recursos na barra de status
	void UpdateUI() {
		hpUI.text = player.GetHP().ToString();
		mpUI.text = "Mana: " + player.GetMP();
		goldUI.text = "Ouro: " + player.gold;
		itemUI.text = "x" + playerInventory.CountConsumable(player.consumable);
	}

	// Passa um texto para a mensagem na tela
	public void SetMessage(string msg) {
		msgText.text = msg;
		Color color = msgText.color;
		color.a = 0; // Tira o valor alpha da cor da mensagem
		msgText.color = color;
		msgActive = true;
	}
}
