namespace Fr.CloseLoopController.Input
{

    public class Integrator : Base
    {

        double _data = 0d;

        public override double Work(double input, double deltaTime)
        {
            _data = _data + input;

            return _data * deltaTime;
        }

    }

}
