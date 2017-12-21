namespace Battle
{
    namespace Unit
    {
        internal interface IStatistic
        {
            SimpleCallbackDelegate OnHealthOverEvent
            {
                get;
                set;
            }
            SimpleCallbackDelegate OnReviveEvent
            {
                get;
                set;
            }
            float Health
            {
                get;
                set;
            }
            float Protection
            {
                get;
                set;
            }
            bool IsDied
            {
                get;
            }
        }
    }
}