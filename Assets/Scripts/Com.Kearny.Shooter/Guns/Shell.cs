using System.Collections;
using UnityEngine;

namespace Com.Kearny.Shooter.Guns
{
    public class Shell : MonoBehaviour
    {
        /// <summary>
        /// Lifetime unit is second
        /// </summary>
        private const float LIFETIME = 4;
        /// <summary>
        /// // Fade time unit is second
        /// </summary>
        private const float FADE_TIME = 2;

        public float ForceMin { get; set; }
        public float ForceMax { get; set; }
        public Rigidbody Rigidbody { get; set; }
        public Renderer Renderer { get; set; }

        // Start is called before the first frame update
        private void Start()
        {
            Renderer = GetComponent<Renderer>();
            
            var force = Random.Range(ForceMin, ForceMax);
            Rigidbody.AddForce(transform.right * force);
            Rigidbody.AddTorque(Random.insideUnitSphere * force);

            StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            yield return new WaitForSeconds(LIFETIME);

            float percent = 0;
            const float fadeSpeed = 1 / FADE_TIME;
            Material material = Renderer.material;
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