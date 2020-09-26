namespace BakeTest.Mod
{
    using System;
    using System.Diagnostics;

    internal struct TimeDif
    {
        internal TimeDif(TimeDif old, TimeDif cur)
        {
            this.ticks = cur.ticks - old.ticks;
        }
        internal TimeDif(Stopwatch timer)
        {
            this.ticks = timer.ElapsedTicks;
        }

        public override String ToString() => $"{this.ticks} ticks, {this.seconds} seconds";

        internal Int64 ticks { get; }
        internal Double seconds => ((Double)this.ticks) / (Double)Stopwatch.Frequency;
    }
}
