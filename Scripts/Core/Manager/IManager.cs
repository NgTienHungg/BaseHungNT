using Cysharp.Threading.Tasks;

namespace HungNT
{
    public interface IManager
    {
        bool IsInitialized { get; }

        UniTask Initialize();
    }
}