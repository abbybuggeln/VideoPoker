using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace VideoPoker
{
	//-//////////////////////////////////////////////////////////////////////
	///
	/// Manages UI including button events and updates to text fields
	/// 
	public class UIManager : MonoBehaviour
	{
		[SerializeField]
		private  Text currentBalanceText = null;

		[SerializeField]
		private Text winningText = null;

		[SerializeField]
		private Button betButton = null;

		[SerializeField]
		private Button[] cardButtons = null;



		//Variables declared by me: dictionary for holding cardImages, array for holding
		//the playerHandImages specifically, and some class declarations. 
		public Dictionary<string, Sprite> cardImages= new Dictionary<string, Sprite>();

		public Image[] playerHandImages = new Image[5];

		private GameManager gameManager;

		private CardComparisonManager compareCards;

		private GameObject[] holdObjects = new GameObject[5];



		/// Initialize variables, managers, load up the card images so the game logic
		/// can use the card dictionary.
		void Awake()
		{
			gameManager = FindObjectOfType<GameManager>();
			compareCards = FindObjectOfType<CardComparisonManager>();
			Transform Cards = GameObject.Find("Cards").transform;
			for(int i=0; i<holdObjects.Length; i++)
            {
				holdObjects[i] = Cards.GetChild(i).GetChild(1).gameObject;
			}
			LoadImagesFromFolder();
		}


		/// Add listeners for UI buttons
		void Start()
		{
			betButton.onClick.AddListener(OnBetButtonPressed);
			foreach (var button in cardButtons)
			{
				button.onClick.AddListener(() => OnHoldCardButtonPressed(button.transform.GetChild(1)));
			}
		}


		/// Load in all the card images and create a dictionary out of them. 
		private void LoadImagesFromFolder()
		{
			int imageIndex = 0;
			Sprite[] cardSprites = (Resources.LoadAll<Sprite>("Art/Cards"));
			foreach (var cardSprite in cardSprites)
			{
				Card card = gameManager.cardDeck.deck[imageIndex];
				string imageName = card.rank.ToString() + card.suit.ToString();
				cardImages.Add(imageName, cardSprite);
				imageIndex++;
			}
		}


		/// Update the visual cards in the players hand. 
		public void UpdateCards(List<Card> playerHand)
        {
			for(int i=0; i<playerHand.Count; i++)
            {
				var imageName = playerHand[i].rank.ToString() + playerHand[i].suit.ToString();
				playerHandImages[i].GetComponent<Image>().sprite = cardImages[imageName];
            }
			
        }

		/// Update the text boxes to show player winnings.
		public void UpdateText(int justWon, int totalBalance, string payoutName)
        {
			currentBalanceText.text = ("Balance: " + totalBalance + " credits");
			winningText.text = (payoutName +  " You won " + justWon + " credits.");
		}


		/// Let the player know when the deck is out of cards, game over. 
		private void GameOver()
        {
			winningText.text = ("Game Over!");
		}

		public void TriggerDealEvents(int numOfCards)
        {
			var playerHand = gameManager.DealCards(gameManager.cardDeck.deck, gameManager.player.playerHand, numOfCards);
			UpdateCards(playerHand);
			compareCards.CheckForPayouts(playerHand);
		}


		/// Event that triggers when bet button is pressed.
		private void OnBetButtonPressed()
		{
			int numOfCards = gameManager.player.playerHand.Count(card => !card.hold);
			if (gameManager.cardDeck.deck.Count == 0)
			{
				GameOver();
				throw new InvalidOperationException("There are 0 cards left in the deck.");
			}
			else if (gameManager.cardDeck.deck.Count < numOfCards)
			{
				throw new InvalidOperationException("Cannot deal " + numOfCards + " cards. There are only " + gameManager.cardDeck.deck.Count + " cards left in the deck.");
			}
			if (compareCards.payoutFound)
            {
				compareCards.payoutFound = false;
				TriggerDealEvents(5);
            }
            else
            {
				TriggerDealEvents(numOfCards);
			}
			
		}


		/// Event that triggers when OnHoldCardButton is pressed.
		private void OnHoldCardButtonPressed(Transform holdObject)
		{
			holdObject.gameObject.SetActive(!holdObject.gameObject.activeSelf);
			gameManager.player.Hold(gameManager.player.playerHand, holdObject.parent.GetSiblingIndex());

		}

		/// Reset all holds if player wins a hand
		public  void ResetAllHolds()
		{
			foreach(var hold in holdObjects){
				hold.SetActive(false);
			}
		}
	}
}