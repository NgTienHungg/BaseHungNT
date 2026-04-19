using Cysharp.Threading.Tasks;

namespace WingsMob.HungNT
{
    public interface IManager
    {
        bool IsInitialized { get; }

        UniTask Initialize();
    }
}