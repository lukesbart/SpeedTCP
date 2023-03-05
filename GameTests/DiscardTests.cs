namespace GameTests;
using Game;

[TestClass]
public class DiscardTests
{
    Pile discard = new Pile("discard1", new List<string> { "Ace of Spades", "2 of Diamonds", "3 of Clubs" });

    [TestMethod]
    public void TestMethod1()
    {
        List<string> bottomCards = new List<string> { "2 of Diamonds", "3 of Clubs" };
        List<string> removedCards = discard.removeAllBottomCards();
        Assert.AreEqual(bottomCards, removedCards);
        Console.WriteLine(discard.removeAllBottomCards());
    }
}
