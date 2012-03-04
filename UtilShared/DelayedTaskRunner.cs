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
            this.timer = new Timer(delay);
            this.timer.BeginInit();
            this.timer.AutoReset = false;
            this.timer.Elapsed += new ElapsedEventHandler(this.TimerElapsedHandler);
            this.timer.EndInit();
        }

        private void TimerElapsedHandler(object sender, ElapsedEventArgs e)
        {
            #region    Precondition

            lock (lockObj)
            {
                if (this.taskRunning || this.currentArgs == null)
                {
                    return;
                }
                else
                {
                    this.taskRunning = true;
                }
            }

            #endregion Precondition

            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            this.timer.Stop();
            this.OnRunTaskTrigger(this.currentArgs);
        }

        /// <summary>
        /// Delay of delayed task in [ms]
        /// </summary>
        public int Delay
        {
            get { return this.delay; }
            set
            {
                this.delay = value;
                this.timer.Interval = this.delay;
            }
        }

        public event RunDelayedTaskHandler<TArgs> RunDelayedTask;

        /// <summary>
        /// Triggers a new delayed task, any pending task is cancelled
        /// </summary>
        /// <param name="args">arguments for delayed tasks</param>
        public void RunTask(TArgs args)
        {
            this.cultureInfo = Thread.CurrentThread.CurrentUICulture;

            if (this.timer.Enabled)
            {
                this.timer.Stop();
            }
            this.currentArgs = args;

            this.timer.Start();
        }

        protected virtual void OnRunTaskTrigger(TArgs args)
        {
            if (this.RunDelayedTask != null)
            {
                this.RunDelayedTask(this, args);
            }

            #region    Postcondition

            lock (lockObj)
            {
                this.taskRunning = false;
            }

            #endregion Postcondition
        }

        /// <summary>
        /// Cancels pending tasks
        /// </summary>
        public void CancelTask()
        {
            if (this.timer.Enabled)
            {
                this.timer.Stop();
            }

            #region    Postcondition

            lock (lockObj)
            {
                this.taskRunning = false;
            }

            #endregion Postcondition
        }
    }
}