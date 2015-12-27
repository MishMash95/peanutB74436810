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
        public double getEquity(HoldemHand.Hand hero, List<Villain> villains, HoldemHand.Hand boardH) {
            // static double WinOddsMonteCarlo(ulong pocket, ulong board, ulong dead, int nopponents, double duration)
            ulong pocket = hero.MaskValue;
            ulong board = boardH.MaskValue;
            int nopponents = villains.Count;
            int duration = 5;
            System.Diagnostics.Debug.Assert(nopponents > 0 && nopponents <= 9);
            System.Diagnostics.Debug.Assert(duration > 0.0);
            System.Diagnostics.Debug.Assert(HoldemHand.Hand.BitCount(pocket) == 2);
            System.Diagnostics.Debug.Assert(HoldemHand.Hand.BitCount(board) >= 0 && HoldemHand.Hand.BitCount(board) <= 5);

            // Keep track of stats
            double win = 0.0, counta = 0.0;

            // Keep track of time
            double start = HoldemHand.Hand.CurrentTime;

            // Loop for specified time duration
            while ((HoldemHand.Hand.CurrentTime - start) < duration) {
                // Player and board info
                ulong boardmask = HoldemHand.Hand.RandomHand(board | pocket, 5);
                uint playerHandVal = HoldemHand.Hand.Evaluate(pocket | boardmask);

                // Ensure that dead, board, and pocket cards are not
                // available to opponent hands.
                ulong deadmask = boardmask | pocket;

                // Comparison Results
                bool greaterthan = true;
                bool greaterthanequal = true;

                long player1Wins = 0, player2Wins = 0,
                    ties = 0, countb = 0;

                // Get random opponent hand values
                for (int i = 0; i < nopponents; i++) {
                    // Get Opponent hand info
                    //ulong oppmask = HoldemHand.Hand.RandomHand(deadmask, 2);
                    ulong[] oppmask = PocketHands.Query("Connected Offsuit"); // Needs to be changed
                    /* ulong[] oppmask PocketHands.Query(Villain[i].query...); */


                    // Iterate through 10000 trials.            
                    for (int trials = 0; trials < 10000; trials++) {
                        // Pick a random pocket hand for player2                
                        ulong player2Mask = HoldemHand.Hand.RandomHand(oppmask, pocket, 2);

                        // Pick a random board                
                        ulong boardMask
                            = HoldemHand.Hand.RandomHand(pocket | player2Mask, 5);

                        // Create a hand value for each player
                        uint player1HandValue =
                            HoldemHand.Hand.Evaluate(boardMask | pocket, 7);
                        uint player2HandValue =
                            HoldemHand.Hand.Evaluate(boardMask | player2Mask, 7);

                        // Calculate Winners                
                        if (player1HandValue > player2HandValue) {
                            player1Wins++;
                        } else if (player1HandValue < player2HandValue) {
                            player2Wins++;
                        } else {
                            ties++;
                        }
                        countb++;
                    }

                    // For debugging
                    Console.WriteLine("Player1: {0:0.0}%",
                                        (player1Wins + ties / 2.0) / ((double)countb) * 100.0);

                    //uint oppHandVal = HoldemHand.Hand.Evaluate(oppmask | boardmask);

                    // Remove these opponent cards from future opponents
                    //deadmask |= oppmask;

                    // Determine compare status
                    /*if (playerHandVal < player2HandValue) {
                        greaterthan = greaterthanequal = false;
                        break;
                    } else if (playerHandVal <= player2HandValue) {
                        greaterthan = false;
                    }*/
                }

                // Calculate stats
                if (greaterthan)
                    win += 1.0;
                else if (greaterthanequal)
                    win += 0.5;

                counta += 1.0;
            }

            // Return stats
            return (counta == 0.0 ? 0.0 : win / counta);
        }

        // Return a hand mask of the cards that improve our hand
        static ulong Outs(ulong pocket, ulong board) {
            ulong retval = 0UL;
            ulong hand = pocket + board;

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
