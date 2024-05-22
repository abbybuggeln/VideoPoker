using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VideoPoker
{
	//-//////////////////////////////////////////////////////////////////////
	/// 
	/// Card Comparison Manager- Checks all of the cards in the players hand
	/// after each bet. Tracks credits and winnings. 
	/// 

	public class CardComparisonManager : MonoBehaviour
	{
		private UIManager uiManager;
		private int credits = 0;
		public bool payoutFound = false;
		void Awake()
		{
			uiManager = FindObjectOfType<UIManager>();
		}

		public void CheckForPayouts(List<Card> playerHand)
        {
			// bool payoutFound helps simplify my payout function logic,
			// so I don't actually have make sure my Three of a kind isnt already a full house etc.
			payoutFound = RoyalFlush(playerHand);
			payoutFound= StraightFlush(playerHand);
			payoutFound= FourOfAKind(playerHand);
			payoutFound= FullHouse(playerHand);
			payoutFound= Flush(playerHand);
			payoutFound= Straight(playerHand);
			payoutFound= ThreeOfAKind(playerHand);
			payoutFound= TwoPair(playerHand);
			payoutFound= JacksOrBetter(playerHand);
			
		}
		private bool RoyalFlush(List<Card> hand)
		{
            if (payoutFound) { return true; }
			var sortedSuits = hand.GroupBy(card => card.suit);
			foreach( var suit in sortedSuits)
            {
				var sortedRanks= hand.GroupBy(card => card.rank);
				if (sortedRanks.Count() == 5)
                {
					if (sortedRanks.Any(card => card.Key == Card.Rank.Ten) && sortedRanks.Any(card => card.Key == Card.Rank.Jack) &&
					sortedRanks.Any(card => card.Key == Card.Rank.Queen) && sortedRanks.Any(card => card.Key == Card.Rank.King) &&
					sortedRanks.Any(card => card.Key == Card.Rank.Ace))
					{ 
						credits += 250;
						uiManager.UpdateText(250, credits, "Royal Flush!");
						return true;
					}
				}
            }
			return false;

		}
		public bool StraightFlush(List<Card> hand)
		{
			if (payoutFound) { return true; }
			var sortedSuits = hand.GroupBy(card => card.suit);
			if(sortedSuits.Count() > 1)
            {
				return false;
            }
			foreach (var suit in sortedSuits)
			{
					var sortedRanks = hand.OrderBy(card => card.rank).ToList();
					int counter = 0;
					for (int i = 0; i < sortedRanks.Count()-1; i++)
					{
						if (sortedRanks[i].rank + 1 != sortedRanks[i+1].rank)
						{
							break;
						}
						else
						{
							counter++;
						}
					}
					if (counter == 4)
					{
						credits += 50;
						uiManager.UpdateText(50, credits, "Straight Flush!");
						return true;
					}
				
			}
			return false;
		}
		public bool FourOfAKind(List<Card> hand)
		{
			if (payoutFound) { return true; }
			var sortedRanks = hand.GroupBy(card => card.rank);
			foreach( var rank in sortedRanks)
            {
				if(rank.Count() == 4)
                {
					credits += 25;
					uiManager.UpdateText(25, credits, "Four of a Kind!");
					return true;
				}
            }
			return false;

		}
		public bool FullHouse(List<Card> hand)
		{
			if (payoutFound) { return true; }
			var sortedRanks = hand.GroupBy(card => card.rank);
			if (sortedRanks.Count() >= 3) { return false; }
			if (sortedRanks.ElementAt(0).Count() == 3 || sortedRanks.ElementAt(1).Count() == 3
				&& sortedRanks.ElementAt(0).Count() == 2 || sortedRanks.ElementAt(1).Count() == 2)
			{
				credits += 9;
				uiManager.UpdateText(9, credits, "Full House!");
				return true;
			}
			return false;
			
		}
		public bool Flush(List<Card> hand)
		{
			if (payoutFound) { return true; }
			var sortedSuits = hand.GroupBy(card => card.suit);
			foreach (var card in sortedSuits)
			{
				if (card.Count() == 5)
				{
					credits += 6;
					uiManager.UpdateText(6, credits, "Flush!");
					return true;
				}
			}
			return false;
		}
		public bool Straight(List<Card> hand)
		{
			if (payoutFound) { return true; }
			var sortedSuits = hand.GroupBy(card => card.suit);
			if(sortedSuits.Count() == 1){ return false; }
			int counter = 0;
			var sortedRanks = hand.OrderBy(card => card.rank).ToList();
			for (int i = 0; i < sortedRanks.Count() - 1; i++)
			{
				if (sortedRanks[i].rank + 1 != sortedRanks[i + 1].rank)
				{
					break;
				}
				else
				{
					counter++;
				}
			}
			if (counter == 4)
			{
				credits += 4;
				uiManager.UpdateText(4, credits, "Straight!");
				return true;
			}
			return false;
		}
		public bool ThreeOfAKind(List<Card> hand)
		{
			if (payoutFound) { return true; }
			var sortedRank = hand.GroupBy(card => card.rank);
			foreach (var card in sortedRank)
			{
				if (card.Count() == 3)
				{
					credits += 3;
					uiManager.UpdateText(3, credits, "Three Of A Kind!");
					return true;
				}
			}
			return false;
		}

		public bool TwoPair(List<Card> hand)
		{
			if (payoutFound) { return true; }
			var sortedRank = hand.GroupBy(card => card.rank);
			if ((sortedRank.Count(group=> group.Count()== 2)==2)){
				credits += 2;
				uiManager.UpdateText(2, credits, "Two Pair!");
				return true;
			}		
			return false;
		}
		public bool JacksOrBetter(List<Card> hand)
		{
			if (payoutFound) { return true; }
			var sortedRank = hand.GroupBy(card => card.rank);
			foreach (var cardGroup in sortedRank)
			{
				if (cardGroup.Count() == 2 && (cardGroup.Key == Card.Rank.Jack || cardGroup.Key == Card.Rank.Queen || cardGroup.Key == Card.Rank.King || cardGroup.Key == Card.Rank.Ace))
				{
					credits += 1;
					uiManager.UpdateText(1, credits, "Jacks or Better!");
					return true;
				}
			}
			return false;
		}
	}


}
