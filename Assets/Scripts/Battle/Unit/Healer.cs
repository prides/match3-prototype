namespace Battle
{
    namespace Unit
    {
        internal class Healer : Attacker
        {
            public override void DoAction(float strength, SquadContainer teammates, SquadContainer enemies, SimpleCallbackDelegate onOverCallback)
            {
                if (teammates == null)
                {
                    Utils.Logger.GetInstance().Error("teammates is null");
                    onOverCallback();
                    return;
                }
                SquadUnitController[] teammatesArray = teammates.GetAllUnits();
                if (teammatesArray == null || teammatesArray.Length <= 0)
                {
                    Utils.Logger.GetInstance().Message("failed to get teammates");
                    onOverCallback();
                    return;
                }
                AttackAnimation.DoAnimation(teammatesArray[0].transform.parent.position, () =>
                {
                    foreach (SquadUnitController teammate in teammatesArray)
                    {
                        if (teammate.Healable != null)
                        {
                            teammate.Healable.ReceiveHeal(strength);
                        }
                    }
                }, () =>
                {
                    onOverCallback();
                });
            }
        }
    }
}