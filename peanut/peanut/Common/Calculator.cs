using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HoldemHand;

namespace peanut.Common
{
    /*
        Abstraction of the HoldemHand API
        http://www.codeproject.com/Articles/19091/More-Texas-Holdem-Analysis-in-C-Part-1
    */
    class Calculator
    {
        public double getEquity(HoldemHand.Hand hero, List<Villain> opponents, HoldemHand.Hand board) {
            ulong heroMask = hero.MaskValue;
            ulong boardMask = board.MaskValue;
            string mergedVillainRange = "";

            // Will need a more sophisticated method for merging all the villain ranges
            foreach (Villain v in opponents) {
                mergedVillainRange += v.range;
            }

            // A Pocket Query Returns an array of all
            // hands that meet the criterion.
            ulong[] opposingRange = PocketHands.Query(mergedVillainRange);
            //ulong[] hero = PocketHands.Query("Connected Offsuit");

            // Holds stats
            long heroWins = 0, villainsWin = 0,
                ties = 0, count = 0;

            // Iterate through 10000 trials.            
            for (int trials = 0; trials < 10000; trials++) {
                // Pick a random pocket hand out of 
                // player1's query set                
                //ulong player1Mask = Hand.RandomHand(player1, 0UL, 2);

                // Pick a random pocket hand for player2                
                ulong villainMask = HoldemHand.Hand.RandomHand(opposingRange, heroMask, 2);

                // Pick a random board         
                // Need to write a method for creating a random entire board if preflop, just turn/river on flop etc
                // Based on the board given parameter       
                //ulong boardMask
                //    = Hand.RandomHand(player1Mask | player2Mask, 5);

                // Create a hand value for each player
                uint heroHandValue =
                    HoldemHand.Hand.Evaluate(boardMask | heroMask, 7);
                uint villainHandValue =
                    HoldemHand.Hand.Evaluate(boardMask | villainMask, 7);

                // Calculate Winners                
                if (heroHandValue > villainHandValue) {
                    heroWins++;
                } else if (heroHandValue < villainHandValue) {
                    villainsWin++;
                } else {
                    ties++;
                }
                count++;
            }

            return (heroWins + ties / 2.0) / ((double)count) * 100.0;
        }

        // Return a hand mask of the cards that improve our hand
        static ulong getOuts(ulong pocket, ulong board) {
            ulong retval = 0UL;
            ulong hand = pocket + board; // May be wrong

            // Get original hand value
            uint playerOrigHandVal = HoldemHand.Hand.Evaluate(hand);

            // Look ahead one card
            foreach (ulong card in HoldemHand.Hand.Hands(0UL, hand, 1)) {
                // Get new hand value
                uint playerNewHandVal = HoldemHand.Hand.Evaluate(hand + card);

                // If the hand improved then we have an out
                if (playerNewHandVal > playerOrigHandVal) {
                    // Add card to outs mask
                    retval = card;
                }
            }

            // return outs as a hand mask
            return retval;
        }

        public double getPotOdds(int priceToCall, int potSize) { 
            // potSize must be the total size of the pot which includes the raise made by the villain
            return (priceToCall / potSize) * 100;
        }

        public double getImpliedOdds() {
            return 0;
        }

        public double getCardOdds() {
            /* Hit odds are not calculated, they are instead searched in the hitOdds table */
            Console.WriteLine("Card odds are accessible in the database class");
            return 0;
        }

 
    }
}
