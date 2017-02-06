class Tags
{
    public static readonly string Player = "Player";

    public class Enemy
    {
        public static readonly string PufferFish = "PufferFish";
        public static readonly string Eel = "Eel";
        public static readonly string MantaRay = "MantaRay";

        public static bool IsEnemyTag(string tag)
        {
            return tag == PufferFish ||
                   tag == Eel ||
                   tag == MantaRay; 
        }
    }
}
