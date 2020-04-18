using System.Collections;
using UnityEngine;

namespace Com.Kearny.Shooter.Guns
{
    public class Shell : MonoBehaviour
    {
        public Rigidbody myRigidbody;
        public float forceMin;
        public float forceMax;

        private float lifetime = 4;
        private float fadetime = 2;

        // Start is called before the first frame update
        private void Start()
        {
            var force = Random.Range(forceMin, forceMax);
            myRigidbody.AddForce(transform.right * force);
            myRigidbody.AddTorque(Random.insideUnitSphere * force);

            StartCoroutine(Fade());
        }

        // Update is called once per frame 
        private void Update()
        {
        }

        IEnumerator Fade()
        {
            yield return new WaitForSeconds(lifetime);

            float percent = 0;
            float fadeSpeed = 1 / fadetime;
            Material material = GetComponent<Renderer>().material;
            Color initialColor = material.color;

            while (percent < 1)
            {
                percent += Time.deltaTime * fadeSpeed;
                material.color = Color.Lerp(initialColor, Color.clear, percent);
                yield return null;
            }
            
            Destroy(gameObject);
        }
    }
}