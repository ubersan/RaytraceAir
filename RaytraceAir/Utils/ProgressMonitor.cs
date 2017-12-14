using System.Diagnostics;

namespace RaytraceAir
{
    public class ProgressMonitor
    {
        private readonly int _total;
        private int _current;
        private readonly Stopwatch _stopwatch;

        public ProgressMonitor(int total)
        {
            _total = total;
            _stopwatch = new Stopwatch();
        }

        public void Advance()
        {
            ++_current;
        }

        private string Percentage()
        {
            var percentage = 100.0 / _total * _current;
            return $"{percentage:0.00}%";
        }

        private string TimeElapsed()
        {
            var seconds = _stopwatch.ElapsedMilliseconds / 1000.0;
            return $"{seconds:0} sec";
        }

        public string Progress()
        {
            return $"{ Percentage() } [{ TimeElapsed() }]";
        }

        public void Start()
        {
            _stopwatch.Start();
        }
    }
}