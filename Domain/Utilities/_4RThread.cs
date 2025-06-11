using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace RagnarokHotKeyInWinforms.Utilities
{
    public class _4RThread
    {
        private Thread monitorThread;
        private CancellationTokenSource cancellationTokenSource;
        public _4RThread(Action<CancellationToken> toRun)
        {
            StartNewThread(toRun);
        }

        private void StartNewThread(Action<CancellationToken> toRun)
        {
            cancellationTokenSource = new CancellationTokenSource();
            monitorThread = new Thread(() =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        toRun(cancellationTokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[4RThread Exception] Error while executing thread: " + ex.Message);
                    }

                    Thread.Sleep(50);
                }
            });
            monitorThread.SetApartmentState(ApartmentState.STA); // ✅ Set before starting
            monitorThread.IsBackground = true;
            monitorThread.Start();
        }

        public void Start(Action<CancellationToken> toRun)
        {
            if (monitorThread == null || !monitorThread.IsAlive)
            {
                Console.WriteLine("Starting new thread...");
                StartNewThread(toRun); // ✅ Creates a fresh thread instance
            }
            else
            {
                Console.WriteLine("Thread is already running.");
            }
        }

        public void Stop()
        {
            if (monitorThread != null && monitorThread.IsAlive)
            {
                Console.WriteLine("Stopping thread...");
                cancellationTokenSource.Cancel(); // ✅ Signals thread to exit
                monitorThread.Join(); // ✅ Ensures full termination
                Console.WriteLine("Thread successfully stopped.");
            }
            else
            {
                Console.WriteLine("No active thread found to stop.");
            }
        }
    }
}
