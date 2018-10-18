namespace Fr.CloseLoopController.Parameter
{

    public struct Pid
    {

        public double D { get; }

        public double I { get; }

        public double P { get; }

        public Pid(double p, double i, double d)
        {
            P = p;
            I = i;
            D = d;
        }

        public override string ToString()
        {
            return $"{nameof(Pid)}{{"
                   + $"P: {P}"
                   + $", I: {I}"
                   + $", D: {D}"
                   + $"}}";
        }

    }

}
