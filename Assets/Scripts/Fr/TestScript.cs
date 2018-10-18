using System.Collections.Generic;
using System.Linq;
using Fr.CloseLoopController.Parameter;
using UnityEngine;
using UnityEngine.Assertions;

namespace Fr
{

    public class TestScript : MonoBehaviour
    {

        public Camera Camera;

        public GameObject Plant;

        [Range(-10f, 10f)]
        public float SetPoint;

        [Range(0f, 2f)]
        public float Kp;

        [Range(0f, 2f)]
        public float Ki;

        [Range(0f, 0.1f)]
        public float Kd;

        [SerializeField]
        public List<ControllerParameter> Parameters = new List<ControllerParameter>();

        List<CloseLoopController.Controller.Pid> _controllers
            = new List<CloseLoopController.Controller.Pid>();

        void Start()
        {
            Assert.IsNotNull(Plant);
        }

        void Update()
        {
            var isVerbose = Time.frameCount % 101 == 0;

            if (Parameters.Count != _controllers.Count)
            {
                SyncParametersAndControllers();
            }

            Assert.AreEqual(Parameters.Count, _controllers.Count);

            _controllers.Zip(
                    Parameters,
                    (controller, parameter) => new
                    {
                        Controller = controller,
                        Parameter = parameter
                    }
                )
                .ToList()
                .ForEach(d =>
                    {
                        var pidParameter = new Pid(
                            d.Parameter.Kp,
                            d.Parameter.Ki,
                            d.Parameter.Kd
                        );

                        d.Controller.ChangeParameter(pidParameter);
                    }
                );

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

            var setPoints = Parameters.Select(p => p.SetPoint);

            var newValue = _controllers
                .Zip(
                    setPoints,
                    (controller, setPoint) => new
                    {
                        Controller = controller,
                        SetPont = setPoint
                    }
                )
                .Aggregate(
                    (double) delta,
                    (acc, pair) =>
                    {
                        if (isVerbose)
                        {
                            Debug.Log($"acc: {acc}");
                            Debug.Log($"setPoint: {pair.SetPont}");
                            Debug.Log($"controller: {pair.Controller}");
                        }
                        
                        var currentReference = pair.SetPont - acc;
                        var worked = pair.Controller.Work(currentReference, Time.deltaTime);

                        return worked;
                    }
                );

            if (isVerbose)
            {
                Debug.Log($"new value: {newValue}");
            }

            var newPoint = Plant.transform.position;
            newPoint.x = newPoint.x + (float) newValue;

            if (newPoint.x > 100f || newPoint.x < -100f)
            {
                Debug.LogError($"drop unsettled value: {newPoint.x} (delta: {newValue})");

                return;
            }

            Plant.transform.position = newPoint;
        }

        void SyncParametersAndControllers()
        {
            if (Parameters.Count == _controllers.Count)
            {
                return;
            }

            if (Parameters.Count < _controllers.Count)
            {
                _controllers = _controllers
                    .Take(Parameters.Count)
                    .ToList();
            }

            if (Parameters.Count > _controllers.Count)
            {
                var newControllers = Parameters
                    .Skip(_controllers.Count)
                    .Select(parameter =>
                    {
                        var newParameter = new Pid(
                            parameter.Kp,
                            parameter.Ki,
                            parameter.Kd
                        );

                        return new CloseLoopController.Controller.Pid(newParameter);
                    });

                _controllers.AddRange(newControllers);
            }
        }

    }

}
