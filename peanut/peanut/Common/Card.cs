using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peanut
{
    class Card
    {
        public enum Suit
        {
            NotFound = 0,
            Diamonds = 1,
            Hearts = 2,
            Clubs = 3,
            Spades = 4,
        }

        public enum Rank
        {
            NotFound = 0,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13,
            Ace = 14,
        }


        public Suit suit { get; private set; }
        public Rank rank { get; private set; }

        public Card(Suit suit_arg, Rank rank_arg)
        {
            suit = suit_arg;
            rank = rank_arg;
        }

        public string ToString() {
            return ConvertRankToString(rank) + ConvertSuitToString(suit);
        }

        public static Card.Rank ConvertStringToRank(string stringRank) {
            Card.Rank cardRank;

            //Figure out which Card.Rank the found card was
            switch (stringRank) {
                case "2":
                    cardRank = Card.Rank.Two;
                    break;
                case "3":
                    cardRank = Card.Rank.Three;
                    break;
                case "4":
                    cardRank = Card.Rank.Four;
                    break;
                case "5":
                    cardRank = Card.Rank.Five;
                    break;
                case "6":
                    cardRank = Card.Rank.Six;
                    break;
                case "7":
                    cardRank = Card.Rank.Seven;
                    break;
                case "8":
                    cardRank = Card.Rank.Eight;
                    break;
                case "9":
                    cardRank = Card.Rank.Nine;
                    break;
                case "10":
                    cardRank = Card.Rank.Ten;
                    break;
                case "J":
                    cardRank = Card.Rank.Jack;
                    break;
                case "Q":
                    cardRank = Card.Rank.Queen;
                    break;
                case "K":
                    cardRank = Card.Rank.King;
                    break;
                case "A":
                    cardRank = Card.Rank.Ace;
                    break;
                default:
                    cardRank = Card.Rank.NotFound;
                    break;
            }

            return cardRank;
        }

        public static Card.Suit ConvertStringToSuit( string s) {
            switch (s.ToLower()) {
                case "s": return Card.Suit.Spades;
                case "h": return Card.Suit.Hearts;
                case "d": return Card.Suit.Diamonds;
                case "c": return Card.Suit.Clubs;
            }
            return Card.Suit.NotFound;
        }

        public static string ConvertSuitToString( Card.Suit suit) {
            switch( suit ) {
                case Suit.Clubs: return "c";
                case Suit.Diamonds: return "d";
                case Suit.Hearts: return "h";
                case Suit.Spades: return "s";
            }
            return "n";
        }

        public static string ConvertRankToString( Card.Rank rank ) {
            switch( rank ) {
                case Rank.Ace: return "A";
                case Rank.King: return "K";
                case Rank.Queen: return "Q";
                case Rank.Jack: return "J";
                case Rank.Ten: return "T";
                case Rank.NotFound: return "0";
                default:
                    return ((int)rank).ToString();
            }
            return "0";
        }
    }
}
