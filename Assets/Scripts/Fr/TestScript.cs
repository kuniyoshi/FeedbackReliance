using UnityEngine;
using UnityEngine.Assertions;

namespace Fr
{

    public class TestScript : MonoBehaviour
    {
        
        #region InternalClass

        class Controller
        {

            class SystemConstant
            {

                public float D { get; private set; }

                public float I { get; private set; }

                public float P { get; private set; }

                public SystemConstant(float p, float i, float d)
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

            readonly float _alpha;

            readonly SystemConstant _constant;

            float _i;

            float _d;

            float _previousError;

            public Controller(float kp,
                              float ki,
                              float kd,
                              float alpha)
            {
                _constant = new SystemConstant(kp, ki, kd);
                _alpha = alpha;
            }

            public void Update(float p, float i, float d)
            {
                _constant.Update(p, i, d);
            }

            public float Work(float error, float deltaTime)
            {
                _i = _i + error * deltaTime;
                _d = _alpha * (error - _previousError) / deltaTime
                     + (1f - _alpha) * _d;

                var output = _constant.P * error
                             + _constant.I * _i
                             + _constant.D * _d;

                _previousError = error;

                return output;
            }

        }
        
        #endregion

        public Camera Camera;

        public GameObject Plant;

        public float SetPoint;

        public float Alpha;

        public float Kp;

        public float Ki;

        public float Kd;

        Controller _controller;

        void Start()
        {
            Assert.IsNotNull(Plant);
            _controller = new Controller(Kp, Ki, Kd, Alpha);
        }

        void Update()
        {
            var isVerbose = Time.frameCount % 101 == 0;

            _controller.Update(Kp, Ki, Kd);

            var output = Plant.transform.position;

            if (isVerbose)
            {
                Debug.Log($"output: {output}");
            }

            var mousePosition = UnityEngine.Input.mousePosition;
            mousePosition.z = Plant.transform.position.z;

            if (isVerbose)
            {
                Debug.Log($"mouse position: {mousePosition}");
            }

            var worldPosition = Camera.ScreenToWorldPoint(
                mousePosition
            );

            if (isVerbose)
            {
                Debug.Log($"world position: {worldPosition}");
            }

            var delta = output.x - worldPosition.x;

            if (isVerbose)
            {
                Debug.Log($"delta: {delta}");
            }

            var reference = SetPoint - delta;

            if (isVerbose)
            {
                Debug.Log($"reference: {reference}");
            }

            var newValue = _controller.Work(reference, Time.deltaTime);

            if (isVerbose)
            {
                Debug.Log($"new value: {newValue}");
            }

            var newPoint = Plant.transform.position;
            newPoint.x = newPoint.x + newValue;
            Plant.transform.position = newPoint;
        }

    }

}
