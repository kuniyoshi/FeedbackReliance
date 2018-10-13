namespace Fr.CloseLoopController.Input
{

    public class Identity : Base
    {

        public override double Work(double input, double _deltaTime)
        {
            return input;
        }

    }

}
