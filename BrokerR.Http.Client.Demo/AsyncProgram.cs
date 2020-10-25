using System;
using System.Threading;
using System.Threading.Tasks;

namespace BrokerR.Http.Client.Demo
{
    public static class AsyncProgram
    {
        public static async Task RunAsync(string[] args, Func<string[], CancellationToken, Task<Func<Task>>> program)
        {
            var cts = new CancellationTokenSource();

            var waitHandle = new ManualResetEventSlim();

            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                waitHandle.Set();
            };

            Func<Task> dispose;
            try
            {
                dispose = await program.Invoke(args, cts.Token);
            }
            catch (OperationCanceledException)
            {
                dispose = null;
                // Nothing
            }

            waitHandle.Wait(cancellationToken: default);

            if (dispose != null)
            {
                await dispose();
            }
        }
        
        public static async Task RunAsync(string[] args, Func<string[], CancellationToken, Task> program)
        {
            await RunAsync(args, async (a, t) =>
            {
                await program(a, t);
                return () => Task.CompletedTask;
            });
        }
    }
}