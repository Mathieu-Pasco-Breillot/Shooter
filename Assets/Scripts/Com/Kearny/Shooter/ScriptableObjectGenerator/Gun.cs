using UnityEngine;

namespace Com.Kearny.Shooter.ScriptableObjectGenerator
{
    [CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
    public class Gun : ScriptableObject
    {
        public string name;
        public float fireRate;
        public GameObject prefab;
    }
}
