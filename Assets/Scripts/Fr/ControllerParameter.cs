using System;
using UnityEngine;

namespace Fr
{

    [Serializable]
    public class ControllerParameter
    {

        [Range(-10f, +10f)]
        public float SetPoint;

        [Range(0f, 2f)]
        public float Kp;

        [Range(0f, 2f)]
        public float Ki;

        [Range(0f, 2f)]
        public float Kd;

    }

}
