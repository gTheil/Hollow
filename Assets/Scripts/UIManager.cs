using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public GameObject pauseMenu; // Referência ao menu principal
	public Transform cursor; // Referência ao cursor para a navegação no menu
	public Transform[] menuOptions; // Array de opções selecionáveis no menu principal
	public GameObject optionPanel; // Painel que contém as opções selecionáveis no menu principal
	public GameObject itemPanel; // Painel que contém a lista de itens que o personagem possui
	public GameObject itemList; // Lista de itens que o personagem possui
	public RectTransform content; // Referência ao conteúdo das opções do menu
	public List<ItemList> items; // Referência à classe ItemList para manipulação
	public Text descriptionText; // Referência ao texto de descrição de um item

	private bool menuActive = false; // Determina se o jogador está com o menu aberto
	private int optionID = 0; // Posição do cursor no array de opções selecionáveis
	private Inventory inventory; // Referência ao inventário do jogador
	private bool itemListActive = false; // Determina se a lista de itens está aberta

	// Atribui a variável inventory o inventário do jogador
	void Start () {
		inventory = Inventory.inventory;
	}
	
	// Atualizado a cada frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.P)) { // Ao jogador pressionar o botão de menu
			menuActive = !menuActive; // Abre o menu se o mesmo estiver fechado, fecha-o se estiver aberto
			optionID = 0; // Reseta a posição do cursor para a primeira opção selecionável
			optionPanel.SetActive(true); // Ativa o painel de opções
			itemPanel.SetActive(false); // Desativa o painel de itens
			itemListActive = false; // Desativa a lista de itens
			descriptionText.text = ""; // Limpa o campo de descrição no menu
			if (menuActive) {
				pauseMenu.SetActive(true);
			} else {
				pauseMenu.SetActive(false);
			}
		}

		if (menuActive) { // Se o menu estiver aberto
			Vector3 cursorPosition = new Vector3(); // Variável que determina a posição do cursor
			if (!itemListActive) { // Caso a lista de itens não esteja ativa
				cursorPosition = menuOptions [optionID].position; // Determina a posição das opções selecionáveis
				cursor.position = new Vector3 (cursorPosition.x - 100, cursorPosition.y, cursorPosition.z); // Determina a posição do cursor em relação às opções do menu
			} else if (itemListActive && items.Count > 0) {
				cursorPosition = items[optionID].transform.position; // Determina a posição dos itens na lista
				cursor.position = new Vector3(cursorPosition.x - 75, cursorPosition.y, cursorPosition.z); // Determina a posição do cursor em relação aos itens da lista
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) { // Ao jogar pressionar a seta para baixo
				optionID++; // Move o cursor para a próxima opção
				if (optionID > (menuOptions.Length - 1)) { // Caso seja feito na última opção,
					optionID = 0; // move o cursor para a primeira opção
				}
			} else if (Input.GetKeyDown(KeyCode.UpArrow)) { // Ao jogador pressionar a seta para cima
				optionID--; // Move o cursor para a opção anterior
				if (optionID < 0) { // Caso seja feito na primeira opção,
					optionID = (menuOptions.Length - 1); // move o cursor para a última opção
				}
			}

			if (Input.GetButtonDown ("Submit") && !itemListActive) { // Ao jogador selecionar uma das opções
				optionPanel.SetActive(false); // Desativa o painel de opções do menu
				itemPanel.SetActive(true); // Ativa o painel de itens no menu
				RefreshItemList(); // Limpa a lista de itens
				UpdateItemList(optionID); // Gera a os itens dentro da lista de acordo com a opção selecionada
				optionID = 0; // Posiciona o cursor no primeiro item da lista
				if (items.Count > 0) // Caso a lista de itens não esteja vazia
				UpdateDescription(); // Exibe o texto de descrição do item selecionado
				itemListActive = true; // Ativa a lista de itens
			}
		}

	}

	void UpdateDescription(){
		if (items[optionID].weapon != null) // Caso hajam armas na lista de itens
			descriptionText.text = items[optionID].weapon.description; // Atribui ao campo de descrição o texto de descrição da arma selecionada
		else if (items[optionID].consumable != null) // Caso hajam consumíveis na lista de itens
			descriptionText.text = items[optionID].consumable.description; // Atribui ao campo de descrição o texto de descrição do consumível selecionado
		else if (items[optionID].key != null) // Caso hajam chaves na lista de itens
			descriptionText.text = items[optionID].key.description; // Atribui ao campo de descrição o texto de descrição da chave selecionada
	}

	// Método que exclui todos os itens de dentro da lista de itens
	void RefreshItemList () {
		for (int i = 0; i < items.Count; i++) {
			Destroy(items[i].gameObject);
		}
		items.Clear();
	}

	// Método que gera os itens dentro da lista de acordo com a opção selecionada
	void UpdateItemList (int option) {
		if (option == 0) { // Caso seja a primeira opção
			for (int i = 0; i < inventory.weapons.Count; i++) { // Para cada arma no inventário
				GameObject tempItem = Instantiate(itemList, content.transform); // Instancia a lista de itens para que possa ser manipulada
				tempItem.GetComponent<ItemList>().SetUpWeapon(inventory.weapons[i]); // Adiciona a arma do inventário à lista de itens
				items.Add(tempItem.GetComponent<ItemList>());
			}
		}
		else if (option == 2) { // Caso seja a terceira opção
			for (int i = 0; i < inventory.consumables.Count; i++) { // Para cada consumível no inventário
				GameObject tempItem = Instantiate(itemList, content.transform); // Instancia a lista de itens para que possa ser manipulada
				tempItem.GetComponent<ItemList>().SetUpConsumable(inventory.consumables[i]); // Adiciona o consumível do inventário à lista de itens
				items.Add(tempItem.GetComponent<ItemList>());
			}
		}
		else if (option == 3) { // Caso seja a quarta opção
			for (int i = 0; i < inventory.keys.Count; i++) { // Para cada chave no inventário
				GameObject tempItem = Instantiate(itemList, content.transform); // Instancia a lista de itens para que possa ser manipulada
				tempItem.GetComponent<ItemList>().SetUpKey(inventory.keys[i]); // Adiciona a chave do inventário à lista de itens
				items.Add(tempItem.GetComponent<ItemList>());
			}
		}
	}
}
