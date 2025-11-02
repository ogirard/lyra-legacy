using System;
using System.Globalization;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Lyra2.UtilShared
{
    /// <summary>
    /// Delayed task delegate
    /// </summary>
    /// <typeparam name="TArgs">Type of arguments for delayed task</typeparam>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void RunDelayedTaskHandler<TArgs>(object sender, TArgs args);

    /// <summary>
    /// Delayed task runner
    /// </summary>
    /// <typeparam name="TArgs">Type of arguments for delayed task</typeparam>
    public class DelayedTaskRunner<TArgs>
        where TArgs : EventArgs
    {
        private int delay;
        private Timer timer;
        private object lockObj = new object();
        private bool taskRunning;
        private CultureInfo cultureInfo;
        private TArgs currentArgs;


        /// <summary>
        /// Initializes a delayed task runner
        /// </summary>
        /// <param name="delay"></param>
        public DelayedTaskRunner(int delay)
        {
            this.delay = delay;
            timer = new Timer(delay);
            timer.BeginInit();
            timer.AutoReset = false;
            timer.Elapsed += new ElapsedEventHandler(TimerElapsedHandler);
            timer.EndInit();
        }

        private void TimerElapsedHandler(object sender, ElapsedEventArgs e)
        {
            #region    Precondition

            lock (lockObj)
            {
                if (taskRunning || currentArgs == null)
                {
                    return;
                }
                else
                {
                    taskRunning = true;
                }
            }

            #endregion Precondition

            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            timer.Stop();
            OnRunTaskTrigger(currentArgs);
        }

        /// <summary>
        /// Delay of delayed task in [ms]
        /// </summary>
        public int Delay
        {
            get { return delay; }
            set
            {
                delay = value;
                timer.Interval = delay;
            }
        }

        public event RunDelayedTaskHandler<TArgs> RunDelayedTask;

        /// <summary>
        /// Triggers a new delayed task, any pending task is cancelled
        /// </summary>
        /// <param name="args">arguments for delayed tasks</param>
        public void RunTask(TArgs args)
        {
            cultureInfo = Thread.CurrentThread.CurrentUICulture;

            if (timer.Enabled)
            {
                timer.Stop();
            }
            currentArgs = args;

            timer.Start();
        }

        protected virtual void OnRunTaskTrigger(TArgs args)
        {
            if (RunDelayedTask != null)
            {
                RunDelayedTask(this, args);
            }

            #region    Postcondition

            lock (lockObj)
            {
                taskRunning = false;
            }

            #endregion Postcondition
        }

        /// <summary>
        /// Cancels pending tasks
        /// </summary>
        public void CancelTask()
        {
            if (timer.Enabled)
            {
                timer.Stop();
            }

            #region    Postcondition

            lock (lockObj)
            {
                taskRunning = false;
            }

            #endregion Postcondition
        }
    }
}