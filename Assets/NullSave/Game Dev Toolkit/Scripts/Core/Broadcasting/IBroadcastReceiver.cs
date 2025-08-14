namespace NullSave.GDTK
{
    public interface IBroadcastReceiver
    {

        #region Methods

        void BroadcastReceived(object sender, string channel, string message, object[] args);

        void PublicBroadcastReceived(object sender, string message);

        #endregion

    }
}