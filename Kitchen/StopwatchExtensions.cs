using System;
using System.Diagnostics;

namespace Kitchen
{
    public static class StopwatchExtensions
    {
        /// <summary>
        /// Gets the total elapsed time measured by the current instance, including the fractional part.
        /// </summary>
        /// <param name="stopwatch">The stopwatch.</param>
        /// <param name="unit">The time unit to measure the time taken in.</param>
        public static double Elapsed(this Stopwatch stopwatch, StopwatchUnit unit)
        {
            int factor = 1;
            switch (unit)
            {
                case StopwatchUnit.Milliseconds:
                    factor = 1000;
                    break;
                case StopwatchUnit.Microseconds:
                    factor = 1000000;
                    break;
                case StopwatchUnit.Nanoseconds:
                    factor = 1000000000;
                    break;
            }

            return factor * (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;
        }

        /// <summary>
        /// Mesaures the time taken to execute an action.
        /// </summary>
        /// <param name="stopwatch">The stopwatch to time with.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="times">How many times to execute to execute the action.</param>
        /// <param name="unit">The time unit to measure the time taken in.</param>
        public static double TimeAction(this Stopwatch stopwatch, Action action, int times, StopwatchUnit unit)
        {
            stopwatch.Restart();
            for (int i = 0; i < times; i++)
                action();
            stopwatch.Stop();
            return stopwatch.Elapsed(unit);
        }
    }

    public enum StopwatchUnit
    {
        Seconds,
        Milliseconds,
        Microseconds,
        Nanoseconds
    }
}
