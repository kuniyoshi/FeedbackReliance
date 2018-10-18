using Fr.CloseLoopController.Input;

namespace Fr.CloseLoopController.Controller
{

    public class Pid
    {

        readonly Delta _d;

        readonly Integrator _i;

        Parameter.Pid _parameter;

        public Pid(Parameter.Pid parameter)
        {
            _parameter = parameter;
            _i = new Integrator();
            _d = new Delta();
        }

        public void ChangeParameter(Parameter.Pid parameter)
        {
            _parameter = parameter;
        }

        public double Work(double input, double deltaTime)
        {
            var i = _i.Work(input * deltaTime, deltaTime);
            var d = _d.Work(input, deltaTime);
//            var d = _d.Work(input, deltaTime) / deltaTime;

            var output = _parameter.P * input
                         + _parameter.I * i
                         + _parameter.D * d;

            return output;
        }

        public override string ToString()
        {
            return $"{nameof(Pid)}{{"
                   + $"Parameter: {_parameter}"
                   + $", Delta: {_d}"
                   + $", Integrator: {_i}"
                   + $"}}";
        }

    }

}
