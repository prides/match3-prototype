using UnityEngine;

namespace Battle
{
    public abstract class BattleManagerBase : MonoBehaviour
    {
        public delegate void EventDelegateWithInt(BattleManagerBase sender, int value);
        public event EventDelegateWithInt OnPlayerLoseEvent;
        public abstract void Init();
        public abstract void Deinit();
        public abstract void Action(ActionType type, float attackStrength, SimpleCallbackDelegate attackOverCallback);
        public abstract void NextTurn();

        protected void OnPlayerLose(BattleManagerBase sender, int value)
        {
            if (null != OnPlayerLoseEvent)
            {
                OnPlayerLoseEvent(sender, value);
            }
        }
    }
}
