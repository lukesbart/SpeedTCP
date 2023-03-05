using System;
namespace TestClient
{
	public class PlayerMove
	{
			public string? Name { get; set; }
			public (string, string) Move { get; set; }
			public List<string>? Hand { get; set; }
	}
}

