using System;
namespace Game
{
	public class Game
	{
        List<string> gameDeck;
        public Dictionary<string, int> cardValueDeck;
        private List<Player> players;
        static Random _random = new Random();

        private Player? player1;
        private Player? player2;
        private Pile? discard1;
        private Pile? discard2;
        private Pile? draw1;
        private Pile? draw2;

        private string status = "Setup";
        private string winner = "None";

        public Game()
        {
            var deckCards = CardBox.GetCards();
            var nameCards = CardBox.GetNameCards();

            //Make game deck list of strings
            cardValueDeck = deckCards;
            gameDeck = nameCards;

            nameCards = Shuffle(nameCards);
            nameCards = Shuffle(nameCards);

            player1 = new Player("player1");
            player1.GetHand(nameCards.GetRange(0, 20));

            player2 = new Player("player2");
            player2.GetHand(nameCards.GetRange(20, 20));

            players = new List<Player>() { player1, player2 };

            //Discard piles will initially have no cards
            discard1 = new Pile("discard1", nameCards.GetRange(40, 1));
            discard2 = new Pile("discard2", nameCards.GetRange(41, 1));

            draw1 = new Pile("draw1", nameCards.GetRange(42, 5));
            draw2 = new Pile("draw2", nameCards.GetRange(47, 5));

            status = "Playing";
            CreateAvailableMove();
        }

            

        public GameState ShowGameState()
        {
            var currentGameState = new GameState
            {
                Status = status,
                Winner = winner,
                Player1 = player1.ListCards(),
                Player2 = player2.ListCards(),
                Discard1 = discard1.ListCards(),
                Discard2 = discard2.ListCards(),
                Draw1 = draw1.ListCards(),
                Draw2 = draw2.ListCards()
            };
            return currentGameState;
        }

        public void CheckWinner()
        {
            if (player1.PlayerWinner())
            {
                status = "Won";
                winner= "player1";
            }
            if (player2.PlayerWinner())
            {
                status = "Won";
                winner = "player2";
            }
        }

        public bool ValidateMove(int playerNo, String[] move) 
        {
            if (!players[playerNo].ListCards().Contains(move[0]))
            {
                return false;
            }

            var cardValue = cardValueDeck[move[0]];

            Pile pileMove = move[1] == "discard1" ? discard1 : discard2;

            if (cardValue == pileMove.TopValue() - 1)
            {
                return true;
            }
            if (cardValue == pileMove.TopValue() + 1)
            {
                return true;
            }
            // K -> A
            if (cardValue == 12 && pileMove.TopValue() == 0)
            {
                return true;
            }
            // A -> K
            if (cardValue == 0 && pileMove.TopValue() == 12)
            {
                return true;
            }

            return false;
        }

        public bool Move(String[] move, int playerNumber)
        {
            var card = move[0];

            if (playerNumber == 0)
            {
                player1.RemoveCard(card);
            } else
            {
                player2.RemoveCard(card);
            }

            if (move[1] == "discard1")
            {
                discard1.addCard(card);
            } else
            {
                discard2.addCard(card);
            }

            CreateAvailableMove();

            return true;
        }

        public void CreateAvailableMove()
        {
                if (!CheckForAvailableMoves())
                {
                    while (!CheckForAvailableMoves())
                    {
                        DrawToDiscard();
                    }
                }

        }

        public bool CheckForAvailableMoves()
        {
            List<string> player1Cards = player1.ListCards();
            List<string> player2Cards = player2.ListCards();

            int player1HandSize = player1.HandSize();
            int player2HandSize = player2.HandSize();

            for (int i = 0; i < player1HandSize; i++)
            {
                if (ValidateMove(0, new string[] { player1Cards[i], "discard1" })) {
                    return true;
                }
                if (ValidateMove(0, new string[] { player1Cards[i], "discard2" }))
                {
                    return true;
                }
            }
            for (int i = 0; i < player2HandSize; i++)
            {
                if (ValidateMove(1, new string[] { player2Cards[i], "discard1"})) {
                    return true;
                }
                if (ValidateMove(1, new string[] { player2Cards[i], "discard2" }))
                {
                    return true;
                }
            }
            return false;
        }

        public void DrawToDiscard()
        {
            discard1.addCard(draw1.removeTopCard());
            discard2.addCard(draw2.removeTopCard());

            if (draw1.HandSize() <= 1)
            {
                draw1.appendCard(discard1.removeBottomCard());
            }
            if (draw2.HandSize() <= 1)
            {
                draw2.appendCard(discard2.removeBottomCard());
            }
        }

        static List<string> Shuffle(List<string> deck)
        {
            int n = deck.Count();
            for (int i = 0; i < (n - 1); i++)
            {
                int r = i + _random.Next(n - i);
                (deck[r], deck[i]) = (deck[i], deck[r]);
            }

            return deck;
        }
    }
}

