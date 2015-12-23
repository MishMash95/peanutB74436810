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
    }
}
