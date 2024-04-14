using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Cocr
{
    internal static class Program
    {
        private static Mutex mutex = new Mutex(true, "{CUIKEYAO-B9A1-45fd-A8CF-I6GTD5TY9G}");

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process process = GetRunningProcess();
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                // 应用程序启动并执行
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());

                // 在应用程序结束时释放互斥体
                mutex.ReleaseMutex();
            }
            else
            {
                ShowRunningApplication(process);
            }
        }

        ///<summary>
        /// 获取已运行的进程，未运行返回 null
        ///</summary>
        ///<returns></returns>
        private static Process GetRunningProcess()
        {
            Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (processes.Length > 0)
            {
                return processes[0];
            }
            else
            {
                return null;
            }
        }

        ///<summary>
        /// 显示正在运行的应用程序
        ///</summary>
        ///<param name="process"></param>
        private static void ShowRunningApplication(Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            if (handle != IntPtr.Zero)
            {
                SwitchToThisWindow(handle, true);
            }
        }

        ///<summary>
        /// 设置窗口为前景窗口
        ///</summary>
        ///<param name="handle"></param>
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
    }
}