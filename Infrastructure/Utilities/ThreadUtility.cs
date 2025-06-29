using System.Threading;
using System;

namespace Infrastructure.Utilities
{
    public class ThreadUtility
    {
        private Thread monitorThread;
        private CancellationTokenSource cancellationTokenSource;
        public ThreadUtility(Action<CancellationToken> toRun)
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

                    // ✅ Replace Task.Delay with Thread.Sleep for safer execution inside the thread
                    Thread.Sleep(50);
                }
            });

            monitorThread.SetApartmentState(ApartmentState.STA);
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

                monitorThread.Join(200); // ✅ Ensures full termination with timeout
                if (monitorThread.IsAlive)
                {
                    Console.WriteLine("Warning: Thread did not stop in time.");
                }
                else
                {
                    Console.WriteLine("Thread successfully stopped.");
                }
            }
            else
            {
                Console.WriteLine("No active thread found to stop.");
            }
        }
    }
}
