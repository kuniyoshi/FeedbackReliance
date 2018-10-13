using Fr.CloseLoopController.Input;

namespace Fr.CloseLoopController.Controller
{

    public class Pid
    {
        
        class SystemParameter
        {

            public float D { get; private set; }

            public float I { get; private set; }

            public float P { get; private set; }

            public SystemParameter(float p, float i, float d)
            {
                P = p;
                I = i;
                D = d;
            }

            public void Update(float p, float i, float d)
            {
                P = p;
                I = i;
                D = d;
            }

        }

        readonly Delta _d;

        readonly Integrator _i;

        readonly SystemParameter _parameter;

        public Pid(Parameter.Pid parameter)
        {
            _parameter = new SystemParameter(
                parameter.Kp,
                parameter.Ki,
                parameter.Kd
            );
            _i = new Integrator();
            _d = new Delta();
        }

        public void ChangeParameter(Parameter.Pid parameter)
        {
            _parameter.Update(
                parameter.Kp,
                parameter.Ki,
                parameter.Kd
            );
        }

        public double Work(double input, double deltaTime)
        {
            var i = _i.Work(input * deltaTime, deltaTime);
            var d = _d.Work(input, deltaTime) / deltaTime;

            var output = _parameter.P * input
                         + _parameter.I * i
                         + _parameter.D * d;

            return output;
        }

    }

}
