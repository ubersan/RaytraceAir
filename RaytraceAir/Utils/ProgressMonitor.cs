using System.Diagnostics;

namespace RaytraceAir
{
    public class ProgressMonitor
    {
        private readonly int _total;
        private int _current;
        private Stopwatch _stopwatch;

        public ProgressMonitor(int total)
        {
            _total = total;

            _stopwatch = new Stopwatch();
        }

        public void Advance()
        {
            ++_current;
        }

        public string PrintPercentage()
        {
            var percentage = 100.0 / _total * _current;
            return $"{percentage:0.00}%";
        }

        public string PrintTimeElapsed()
        {
            var seconds = _stopwatch.ElapsedMilliseconds / 1000.0;
            return $"{seconds:0} sec";
        }

        public void Start()
        {
            _stopwatch.Start();
        }
    }
}