using System.Runtime.InteropServices;

namespace MyMusicPlayer;

static class Program
{
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool AllocConsole();

    [STAThread]
    static void Main()
    {
        // Enable for debugging
        // AllocConsole();
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }    
}