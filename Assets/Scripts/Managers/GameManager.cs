using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace VideoPoker
{
	//-//////////////////////////////////////////////////////////////////////
	/// Custom class that contains an individual card's info: Suite & card rank
	public class Card
	{
		public Rank rank;
		public Suit suit;
		public bool hold = false;
		public enum Rank { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };
		public enum Suit {Clubs, Diamonds, Hearts, Spades };

		public Card(Rank chosenRank, Suit chosenSuit)
		{
			rank = chosenRank;
			suit = chosenSuit;
		}

	}

	//-//////////////////////////////////////////////////////////////////////
	/// Custom class that contains the filled card deck & functions relating 
	/// to the card deck.
	public class CardDeck
	{
		public List<Card> deck = new List<Card>();
		public CardDeck() {

			//Each card needs its own unique rank & suit pairing
			foreach (Card.Suit suit in Enum.GetValues(typeof(Card.Suit)))
			{	
				foreach (Card.Rank rank in Enum.GetValues(typeof(Card.Rank)))
				{
					deck.Add(new Card(rank, suit));
				}
			}
		}
		public List<Card> Shuffle(List<Card> cardDeck)
        {
			//The deck will be a fixed 52 cards to shuffle, I won't worry about time complexity here for now.
            System.Random random = new System.Random();
			return cardDeck.OrderBy(x => random.Next()).ToList();
        }

		public List<Card> Deal(List<Card> cardDeck, List<Card> playerHand, int numOfCards)
		{
			for (int j = 0; j < playerHand.Count; j++)
			{
				if (!playerHand[j].hold)
				{
					Card card = cardDeck[0];
					playerHand[j] = card;
					cardDeck.RemoveAt(0);
				}
			}
            if (playerHand.Count == 0)
            {
				for (int i = 0; i < numOfCards; i++)
				{
					Card card = cardDeck[0];
					playerHand.Add(card);
					cardDeck.RemoveAt(0);
				}
			}
			return playerHand;
		}
	}


	//-//////////////////////////////////////////////////////////////////////
	/// Custom class that contains the player's hand list & the function to hold
	/// cards.
	public class Player{

		public List<Card> playerHand = new List<Card>();
		public void Hold(List<Card> hand, int cardIndex)
        {
			hand[cardIndex].hold = !hand[cardIndex].hold;
		}

		public void ResetHolds(List<Card> hand)
		{
			foreach(var card in hand)
            {
				card.hold = false;
			}
			
		}

	}


	//-//////////////////////////////////////////////////////////////////////
	/// The main game manager
	public class GameManager : MonoBehaviour
	{
		private UIManager uiManager;
		public Player player;
		public CardDeck cardDeck;
		public Dictionary<string, Image> cardImages;


		/// Initialize class objects
		void Awake()
		{
			player = new Player();
			cardDeck = new CardDeck();
			uiManager = FindObjectOfType<UIManager>();
		}


		/// Start game logic, want to shuffle, deal and update the cards first
		void Start()
		{
			cardDeck.deck = cardDeck.Shuffle(cardDeck.deck);
			cardDeck.Deal(cardDeck.deck, player.playerHand,  5);   //Our player always gets 5 cards delt initially so I'll just hard code this 5
			uiManager.UpdateCards(player.playerHand);
		}

		public List<Card> DealCards(List<Card> deck, List<Card> playerHand, int numOfCards)
		{
			if(numOfCards == 5)
            {
				player.ResetHolds(playerHand);
				uiManager.ResetAllHolds();
            }
			var newHand = cardDeck.Deal(deck, playerHand, numOfCards);
			return newHand;

		}
	}
}