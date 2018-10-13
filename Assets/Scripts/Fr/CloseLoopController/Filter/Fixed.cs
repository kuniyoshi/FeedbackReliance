using System.Collections.Generic;
using System.Linq;

namespace Fr.CloseLoopController.Filter
{

    public class Fixed
    {

        int _size;

        List<double> _values;

        public Fixed(int size)
        {
            _size = size;
            _values = new List<double>();
        }

        public double Work(double value)
        {
            _values.Add(value);
            _values = _values
                .Take(_size)
                .ToList();

            var average = _values.Average();

            return average;
        }

    }

}
