using System;
namespace Game
{
	public class Player
	{
		public string PlayerName;
		List<string> hand = new List<string>();

		public Player(string player)
		{
			PlayerName = player;
		}

		public void GetHand(List<string> givenCards)
		{
			hand = givenCards;
		}

		/// <summary>
		/// Return list of strings with player's cards
		/// </summary>
		/// <returns>Player's Hand</returns>
		public List<string> ListCards()
		{
			if (hand.Count < 5)
			{
				while (hand.Count < 5)
				{
					hand.Add("Joker");
				}
			}
			return hand;
		}

		public int HandSize()
		{
			int cardCount = 0;

			if (ListCards().Count() <= 5) {
				foreach (string card in ListCards())
				{
					if (card != "Joker")
					{
						cardCount++;
					}
				}
				return cardCount;
			}

			return 5;
		}

        public void RemoveCard(string cardName)
		{
			hand.Remove(cardName);
		}

		/// <summary>
		/// Return true if the players hand is empty
		/// </summary>
		/// <returns>Bool</returns>
		public bool PlayerWinner()
		{
            if (HandSize() == 0)
            {
                return true;
            }
            return false;
        }

		public class PlayerState
		{
			public string name;
			public List<string> hand;
		}


	}
}

