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
        public const string loading = "00798548";
        public const string load = "00760358";
        public const string loadFinished = "0076035C";
        public const string loadStarted = "00760360";
        public const string continueRunLevel = "00760354";
        public const string currentLevelID = "00762B5C";
        public const string outOfLevelState = "00762B58";

        public const string onCutscene = "0075CCE4";
        public const string cutsceneID = "0075CD00";

        public const string health = "0075BDBC";
        public const string level = "0075BE14";
        public const string stamina = local + ",A04";
        public const string invincibility = "0075FBF4";
        public const string rocks = "0075BDB4";
        public const string keys = "0075BE08";
        public const string healthPotions = "0075BE6C";
        public const string antidotes = "0075BE70";
        public const string ringMeter = "0075EBC8";

        public const string bilboCoordsX = local + ",14";
        public const string bilboCoordsY = local + ",18";
        public const string bilboCoordsZ = local + ",1C";

        public const string bilboState = local + ",8D8";
        public const string bilboStateTimer = local + ",91C";

        public const string bilboNewCoordsX = local + ",464";

        public const string bilboNewCoordsY = local + ",468";
        public const string bilboNewCoordsZ = local + ",46C";

        public const string bilboYawRad = local + ",614";

        public const string warpCoordsX = local + ",5f50";
        public const string warpCoordsY = local + ",5F54";
        public const string warpCoordsZ = local + ",5F58";

        public const string sign1 = "0075B548";
        public const string sign2 = "0075B54C";
        public const string sign3 = "0075B550";
        public const string sign4 = "0075B554";
        public const string sign5 = "0075B558";

        public const string memUsage = "0075FBBC";
        public const string memUsageText = "00730FFC";
    }
}
