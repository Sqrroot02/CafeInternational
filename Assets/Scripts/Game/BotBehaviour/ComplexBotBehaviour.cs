namespace Game.BotBehaviour
{
    /// <summary>
    /// A Singular card that is placed
    /// </summary>
    public class Move
    {
        public int CardId { get; set; }
        public int ChairId { get; set; }

        public Move(int cardId, int chairId)
        {
            CardId = cardId;
            ChairId = chairId;
        }
    }
    
    /// <summary>
    /// A complete move a player can make. So first placed card, followed by a second card
    /// </summary>
    public class Turn
    {
        public Move FirstMove { get; set; }
        public Move SecondMove { get; set; }

        public Turn(Move firstMove, Move secondMove)
        {
            FirstMove = firstMove;
            SecondMove = secondMove;
        }

        public int GetPoints()
        {
            return 0;
        }
        
        public void PlayMove()
        {
            
        }
    }
    
    public class ComplexBotBehaviour
    {
        
        
    }
}