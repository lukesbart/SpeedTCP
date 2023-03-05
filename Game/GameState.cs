using System;
namespace Game
{
    public class GameState
    {
        public string? Status { get; set; }
        public string? Winner { get; set; }
        public List<string>? Player1 { get; set; }
        public List<string>? Player2 { get; set; }
        public List<string>? Discard1 { get; set; }
        public List<string>? Discard2 { get; set; }
        public List<string>? Draw1 { get; set; }
        public List<string>? Draw2 { get; set; }
    }
}

// Use serialize