using UnityEngine;
using System.Collections.Generic;
using Match3Core;
using Match3Wrapper;

namespace ActionQueue
{
    public class BattleActionsQueue : MonoBehaviour
    {
        [SerializeField]
        private BattleActionController battleActionPrefab;

        private Queue<BattleActionController> battleActionsQueue = new Queue<BattleActionController>();

        public void AddGemPoints(GemPoint[] gemPoints)
        {
            Dictionary<GemType, List<GemPoint>> gems = new Dictionary<GemType, List<GemPoint>>();
            foreach (GemPoint gemPoint in gemPoints)
            {
                if (!gems.ContainsKey(gemPoint.type))
                {
                    gems.Add(gemPoint.type, new List<GemPoint>());
                }
                gems[gemPoint.type].Add(gemPoint);
            }

            foreach (GemType key in gems.Keys)
            {
                BattleActionController battleAction = Instantiate(battleActionPrefab);
                battleAction.transform.parent = transform;
                battleAction.SetPosition(battleActionsQueue.Count);
                battleAction.GemPoints = gems[key].ToArray();
                battleAction.Appear(null);
                battleActionsQueue.Enqueue(battleAction);
            }
        }

        public GemPoint[] GetGemPoints()
        {
            BattleActionController battleAction = battleActionsQueue.Count > 0 ? battleActionsQueue.Dequeue() : null;
            if (null == battleAction)
            {
                return null;
            }
            battleAction.Disappear(() =>
            {
                int index = 0;
                foreach (BattleActionController ba in battleActionsQueue)
                {
                    ba.SetPosition(index++);
                }
            });
            return battleAction.GemPoints;
        }

        public int GetQueueLength()
        {
            return battleActionsQueue.Count;
        }
    }
}