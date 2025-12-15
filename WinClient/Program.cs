using WinClient.Sources.Managers;

namespace WinClient
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ControllerManager.CreateControllers();
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
            ControllerManager.DestroyControllers();
        }
    }
}