using Fr.CloseLoopController.Parameter;
using UnityEngine;
using UnityEngine.Assertions;

namespace Fr
{

    public class TestScript : MonoBehaviour
    {
        
        public Camera Camera;

        public Pid Parameter;

        public GameObject Plant;

        [Range(-10f, 10f)]
        public float SetPoint;

        [Range(0f, 2f)]
        public float Kp;

        [Range(0f, 2f)]
        public float Ki;

        [Range(0f, 2f)]
        public float Kd;

        Pid _parameter;

        CloseLoopController.Controller.Pid _controller;

        void Start()
        {
            Assert.IsNotNull(Plant);
            _parameter = Instantiate(Parameter);
            _controller = new CloseLoopController.Controller.Pid(_parameter);
        }

        void Update()
        {
            var isVerbose = Time.frameCount % 101 == 0;

            _parameter.Kp = Kp;
            _parameter.Ki = Ki;
            _parameter.Kd = Kd;
            _controller.ChangeParameter(_parameter);

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
            newPoint.x = newPoint.x + (float)newValue;
            Plant.transform.position = newPoint;
        }

    }

}
