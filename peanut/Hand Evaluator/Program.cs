using System;
using HoldemHand;

// A first try at calculating outs
namespace HoldemHand
{
    class Program
    {
        static void Main(string[] args)
        {
            string pocket = "As Ac";
            string board = "Kd 8h 9c";
            // Calcuate the outs
            ulong outsmask =
                Outs(Hand.ParseHand(pocket), Hand.ParseHand(board));

            Console.WriteLine("[{0}] {1} : Outs Count {2}",
                    pocket, board, Hand.BitCount(outsmask));

            // List the cards
            foreach (string card in Hand.Cards(outsmask))
            {
                Console.Write("{0} ", card);
            }
            Console.WriteLine();
        }

        // Return a hand mask of the cards that improve our hand
        static ulong Outs(
            ulong pocket, ulong board, params ulong[] opponents)
        {
            ulong retval = 0UL;

            // Get original hand value
            uint playerOrigHandVal = Hand.Evaluate(pocket  board);

            // Look ahead one card
            foreach (ulong card in Hand.Hands(0UL, board  pocket, 1))
            {
                // Get new hand value
                uint playerNewHandVal = Hand.Evaluate(pocket  board  card);

                // Get new board value
                uint boardHandVal = Hand.Evaluate(board  card);

                // Is the new hand better than the old one?
                bool handImproved =
                    playerNewHandVal > playerOrigHandVal &&
                    Hand.HandType(playerNewHandVal) > Hand.HandType(
                    playerOrigHandVal);

                // This compare ensures we move up in hand type.
                bool handStrongerThanBoard =
                    Hand.HandType(playerNewHandVal) > Hand.HandType(
                    boardHandVal);

                // Check against opponents cards
                bool handBeatAllOpponents = true;
                if (handImproved && handStrongerThanBoard &&
                    opponents != null && opponents.Length > 0)
                {
                    foreach (ulong opponent in opponents)
                    {
                        uint opponentHandVal =
                            Hand.Evaluate(opponent  board  card);
                        if (opponentHandVal > playerNewHandVal)
                        {
                            handBeatAllOpponents = false;
                            break;
                        }
                    }
                }

                // If the hand improved then we have an out
                if (handImproved && handStrongerThanBoard &&
                    handBeatAllOpponents)
                {
                    // Add card to outs mask
                    retval = card;
                }
            }

            // return outs as a hand mask
            return retval;
        }
    }
}