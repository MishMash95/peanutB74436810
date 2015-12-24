using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peanut.Logic
{
    class Player
    {
        public string name;

        public double stackSize;
        public enum Position
        {
            BTN = 0,
            SB = 1,
            BB = 2,
            UTG = 3,
            MP = 4,
            CO = 5
        }

        public Position position;
        public double vpip;
        public double pfr;
        public double b3;

        public Player(string name, double stackSize, Position pos, double vpip, double pfr, double b3)
        {
            this.name = name;

            this.stackSize = stackSize;
            this.position = pos;

            this.vpip = vpip;
            this.pfr = pfr;
            this.bet3 = bet3;          
        }

    }
}
