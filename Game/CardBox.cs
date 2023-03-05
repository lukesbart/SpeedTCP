using System;
namespace Game
{
	public class CardBox
	{
		static Dictionary<string, int> defaultCards = new Dictionary<string, int>() {
			{"Ace of Hearts", 0},
			{"2 of Hearts", 1},
			{"3 of Hearts", 2},
			{"4 of Hearts", 3},
			{"5 of Hearts", 4},
			{"6 of Hearts", 5},
			{"7 of Hearts", 6},
			{"8 of Hearts", 7},
			{"9 of Hearts", 8},
			{"10 of Hearts", 9},
			{"Jack of Hearts", 10},
			{"Queen of Hearts", 11},
			{"King of Hearts", 12},

            {"Ace of Clubs", 0},
            {"2 of Clubs", 1},
            {"3 of Clubs", 2},
            {"4 of Clubs", 3},
            {"5 of Clubs", 4},
            {"6 of Clubs", 5},
            {"7 of Clubs", 6},
            {"8 of Clubs", 7},
            {"9 of Clubs", 8},
            {"10 of Clubs", 9},
            {"Jack of Clubs", 10},
            {"Queen of Clubs", 11},
            {"King of Clubs", 12},

            {"Ace of Diamonds", 0},
            {"2 of Diamonds", 1},
            {"3 of Diamonds", 2},
            {"4 of Diamonds", 3},
            {"5 of Diamonds", 4},
            {"6 of Diamonds", 5},
            {"7 of Diamonds", 6},
            {"8 of Diamonds", 7},
            {"9 of Diamonds", 8},
            {"10 of Diamonds", 9},
            {"Jack of Diamonds", 10},
            {"Queen of Diamonds", 11},
            {"King of Diamonds", 12},

            {"Ace of Spades", 0},
            {"2 of Spades", 1},
            {"3 of Spades", 2},
            {"4 of Spades", 3},
            {"5 of Spades", 4},
            {"6 of Spades", 5},
            {"7 of Spades", 6},
            {"8 of Spades", 7},
            {"9 of Spades", 8},
            {"10 of Spades", 9},
            {"Jack of Spades", 10},
            {"Queen of Spades", 11},
            {"King of Spades", 12}
        };

        public static List<string> GetNameCards()
        {
            return new List<string>(defaultCards.Keys);
        }

        public static Dictionary<string, int> GetCards()
        {
            return defaultCards;
        }
    }
}

