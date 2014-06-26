namespace Idecom.Host.Interfaces
{
    public interface IWantToStartAfterServiceStarts
    {
        void AfterStart();
        void BeforeStop();
    }

    public interface IWantToInitializeAfterServiceStarts
    {
        void Initialize();
        void Destroy();
    }
}