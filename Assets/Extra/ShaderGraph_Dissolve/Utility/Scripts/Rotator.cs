using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DissolveExample
{



    public class Rotator : MonoBehaviour
    {
        public float Speed;
        


        public void Update()
        {
            transform.Rotate(Vector3.right, Speed * Time.deltaTime);
        }
    }
}