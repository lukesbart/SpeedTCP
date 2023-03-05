using System;
namespace Game
{
	public class PlayerState
	{
        public string? Name { get; set; }
        //Move['Ace of Spades', 'Draw1']
        public String[]? Move { get; set; }
        public List<string>? Hand { get; set; }
    }
}