using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peanut.Common {

    abstract class Action { }

    class ActionFold : Action{ };
    class ActionCheck : Action { };
    class ActionBet : Action {
        Bet bet { get; }

        public ActionBet( Bet bet ) {
            this.bet = bet;
        }
    }

    /*
        A wrapper for an action bound to a villain
    */
    class VillainAction {
        Action action { get; }
        Villain villain { get; }

        public VillainAction( Action action, Villain villain ) {
            this.action  = action;
            this.villain = villain;
        }

    }

    // ****************** ACTION STACK **************** //
    /*
        An action stack represents a series of actions facing 
        the player. It is fundamentally just an array, however
        it consists of VillainActions, which contain reference to a villain and an action.

        The array elements are accessed using the [] operator. 0 refers to the most recent action before the player.
        up to n which represents the 1st action (i.e player to the left of you).

        If you are first to act, the stack will be empty. A size function also exists.

    */
    class ActionStack {
        private ArrayList actions;

        public ActionStack() {
            actions = new ArrayList();
        }

        public void push( VillainAction va ) {
            actions.Add(va);
        }

        public VillainAction this[int i] {
            get { return actions[actions.Count - i - 1] as VillainAction; }
        }

        public int getCount() {
            return actions.Count;
        }

    }

}
