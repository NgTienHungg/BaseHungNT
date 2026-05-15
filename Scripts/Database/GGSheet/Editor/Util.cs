namespace HungNT.Database.Editor
{
    public static class Util
    {
        public static uint FNVHash(string input)
        {
            uint hash = 2166136261;
            for (int ix = 0; ix < input.Length; ix++)
            {
                hash ^= input[ix];
                hash *= 16777619;
            }

            return hash;
        }

        public const string SymbolIgnore = "#";
    }
}
