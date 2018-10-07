using UnityEngine;

namespace Fr.Configuration
{

    [CreateAssetMenu(menuName = "Fr/Configuration/Pid")]
    public class Pid : ScriptableObject
    {

        [Range(0f, 10f)]
        public float Kp;

        [Range(0f, 10f)]
        public float Ki;

        [Range(0f, 10f)]
        public float Kd;

        [Range(0f, 1f)]
        public float Alpha;


    }

}
