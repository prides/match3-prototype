namespace Utils
{
    public class Randomizer
    {
        private static System.Random random = new System.Random();

        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }

        public static float Range(float min, float max)
        {
            float diff = max - min;
            return min + (float)random.NextDouble() * diff;
        }
    }
}