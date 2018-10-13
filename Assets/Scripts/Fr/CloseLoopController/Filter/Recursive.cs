namespace Fr.CloseLoopController.Filter
{

    public class Recursive
    {

        double _alpha;
        
        double _previous;

        public Recursive(double alpha)
        {
            _alpha = alpha;
        }

        public double Work(double value)
        {
            var output = _alpha * value + (1d - _alpha) * _previous;
            _previous = value;

            return output;
        }

    }

}
