using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DissolveExample
{
    public class DissolveOffest : MonoBehaviour
    {
        // Start is called before the first frame update
        List<Material> materials = new List<Material>();
        bool PingPong = false;
        void Start()
        {
            var renders = GetComponents<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                materials.AddRange(renders[i].materials);
            }
        }

        private void Reset()
        {
            Start();
            SetValue(0);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                //StartCoroutine(enumerator());
                PingPong = true;
            }
            if (PingPong)
            {
                var value = Mathf.PingPong(Time.time * 0.5f, 1.6f);
                value -= 0.5f;
                SetValue(value);
            }
        }

        //IEnumerator enumerator()
        //{

        //    //float value =         while (true)
        //    //{
        //    //    Mathf.PingPong(value, 1f);
        //    //    value += Time.deltaTime;
        //    //    SetValue(value);
        //    //    yield return new WaitForEndOfFrame();
        //    //}
        //}

        public void SetValue(float value)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetVector("_DissolveOffest", new Vector4(0f,value,0f,0f));
            }
        }
    }
}