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
                // Ӧ�ó���������ִ��
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());

                // ��Ӧ�ó������ʱ�ͷŻ�����
                mutex.ReleaseMutex();
            }
            else
            {
                ShowRunningApplication(process);
            }
        }

        ///<summary>
        /// ��ȡ�����еĽ��̣�δ���з��� null
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
        /// ��ʾ�������е�Ӧ�ó���
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
        /// ���ô���Ϊǰ������
        ///</summary>
        ///<param name="handle"></param>
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
    }
}