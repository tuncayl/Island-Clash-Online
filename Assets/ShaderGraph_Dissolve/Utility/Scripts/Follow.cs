using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DissolveExample
{


    public class Follow : MonoBehaviour
    {
        [Range(0, 5f)]
        public float speed;
        public float height;
        Vector3 pos;
        // Start is called before the first frame update
        void Start()
        {
            pos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            var value = Mathf.PingPong(Time.time * speed, height);
            transform.position = (pos + value * Vector3.up);
        }
    }
}