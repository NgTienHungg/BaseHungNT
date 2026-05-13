using System;

namespace HungNT
{
    /// <summary>Lưu số coin (soft currency) của người chơi.</summary>
    [Serializable]
    public class CoinSave : BaseUserData
    {
        public long Amount = 0;

        public override void OnAfterLoad()
        {
            if (Amount < 0) Amount = 0;
        }
    }

    /// <summary>Lưu số gem (hard currency / premium currency) của người chơi.</summary>
    [Serializable]
    public class GemSave : BaseUserData
    {
        public long Amount = 0;

        public override void OnAfterLoad()
        {
            if (Amount < 0) Amount = 0;
        }
    }
}
