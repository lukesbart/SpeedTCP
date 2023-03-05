// Make data type for sending cards of different piles/players to client

using System;
namespace Game
{
	public class Pile
	{
		List<string> Cards;
        string Name;
		Dictionary<string, int> Deck;

		public Pile(string name, List<string> cards)
		{
			Name = name;
			Cards = cards;
			Deck = CardBox.GetCards();
		}

		public int HandSize()
		{
			return Cards.Count();
		}

		public void AddCards(List<String> cards)
		{
			Cards.AddRange(cards);
		}

		public List<string> ListCards()
		{
			return Cards;
		}

		public void addCard(string card)
		{
			Cards.Insert(0, card);
		}

		public void appendCard(string card)
		{
			Cards.Add(card);
		}

		public int TopValue()
		{
			return Deck[Cards[0]];
		}

		public string TopCard()
		{
			return Cards[0];
		}

		public string removeTopCard()
		{
			string card = TopCard();
			Cards.Remove(card);
			return card;
		}

		public string removeBottomCard()
		{
			string card = Cards[Cards.Count-1];
			Cards.Remove(card);
			return card;
		}

		public List<string> removeAllBottomCards()
		{
			List<string> removedCards = new List<string>();

			int pileSize = HandSize();

			removedCards.AddRange(Cards.GetRange(1, pileSize - 1));
			Cards.RemoveRange(1, pileSize - 1);

			return removedCards;
		}

		public class PileState
		{
			public List<string> cards;
		}
	}
}

