namespace RagnarokHotKeyInWinforms.Model
{
    //Used in the AutoPotForm
    public interface Action
    {
        void Start();
        void Stop();
        string GetConfiguration();
        string GetActionName();
    }
}
