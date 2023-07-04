using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DissolveExample
{
    public class RotatorDissolveDir : MonoBehaviour
    {
        public Vector3 Speed;
        List<Material> materials = new List<Material>();
        // Start is called before the first frame update
        void Start()
        {
            materials.AddRange(GetComponent<Renderer>().materials);
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < materials.Count; i++)
            {
                var value = materials[i].GetVector("_DissolveDirection");
                var delta = Speed * Time.deltaTime;
                value += new Vector4(delta.x,delta.y,delta.z,0f);
                materials[i].SetVector("_DissolveDirection", value);
            }
        }
    }
}