namespace HobbitSpeedrunTools
{
    // Keeps track of all memory addresses to be used by cheats
    public static class MemoryAddresses
    {
        public const string local = "base+0035BA3C";
        public const string devMode = "7600e8";
        public const string loadTriggers = "00777B18";
        public const string otherTriggers = "00777B04";
        public const string polyCache = "00778078";
        public const string bilboPositionDisplay = "0075FBD4";
        public const string loading = "00798548";
        public const string stamina = local + ",A04";
        public const string invincibility = "0075FBF4";
        public const string health = "0075BDBC";
        public const string sign1 = "0075B548";
        public const string sign2 = "0075B54C";
        public const string sign3 = "0075B550";
        public const string sign4 = "0075B554";
        public const string sign5 = "0075B558";
        public const string memUsage = "0075FBBC";
        public const string memUsageText = "00730FFC";

        public const string warpCoordsX = local + ",5f50";
        public const string warpCoordsY = local + ",5F54";
        public const string warpCoordsZ = local + ",5F58";

        public const string bilboState = local + ",8D8";
        public const string stateTimer = local + ",91C";

        public const string load = "00760358";
        public const string currentLevelID = "00762B5C";
        public const string outOfLevelState = "00762B58";
    }
}
