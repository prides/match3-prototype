using UnityEngine;
using Match3Core;

namespace Match3Wrapper
{
    [System.Serializable]
    public class PossibleMoveWrapper
    {
        public Vector2 gemPos;
        public Vector2 movePos;
        private PossibleMove instance;

        public PossibleMoveWrapper(PossibleMove instance)
        {
            this.instance = instance;
            this.gemPos = new Vector2(instance.Key.Position.x, instance.Key.Position.y);
            this.movePos = new Vector2(instance.MatchablePosition.x, instance.MatchablePosition.y);
        }

        public void Move()
        {
            ((GemControllerWrapper)instance.Key.tag).MoveTo(DirectionHelper.GetDirectionByPosition(instance.Key.Position, instance.MatchablePosition));
        }
    } 
}