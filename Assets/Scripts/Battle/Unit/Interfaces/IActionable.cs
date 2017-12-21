namespace Battle
{
    namespace Unit
    {
        internal interface IActionable
        {
            void DoAction(float strength, SquadContainer teammates, SquadContainer enemies, SimpleCallbackDelegate onOverCallback);
        }
    }
}