using System;

namespace HungNT.Database.Editor
{
    public interface ILogger
    {
        void LogError(string error);
        void LogError(Exception error);
        void LogWarning(string warning);
    }
}
