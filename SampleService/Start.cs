namespace SampleService
{
    using log4net;
    using Idecom.Host.Interfaces;

    public class Start : IWantToStartAfterServiceStarts
    {
        public ILog Log { get; set; }

        public void AfterStart()
        {
            Log.Info("Starting something big");
        }

        public void BeforeStop()
        {
            Log.Info("Stopping something big");
        }
    }
}