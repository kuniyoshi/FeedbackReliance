namespace Fr.CloseLoopController.Input
{

    public class Delta : Base
    {

        double _previous = 0d;

        public override double Work(double input, double _deltaTime)
        {
            var output = input - _previous;
            _previous = input;

            return output;
        }

        public override string ToString()
        {
            return $"{nameof(Delta)}{{"
                   + $"_previous: {_previous}"
                   + $"}}";
        }

    }

}
