namespace RaytraceAir
{
    public class ProgressMonitor
    {
        private readonly int _total;
        private int _current = 0;

        public ProgressMonitor(int total)
        {
            _total = total;
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
    }
}