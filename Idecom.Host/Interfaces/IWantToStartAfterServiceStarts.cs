namespace Idecom.Host.Interfaces
{
    public interface IWantToStartAfterServiceStarts
    {
        void AfterStart();
        void BeforeStop();
    }
}