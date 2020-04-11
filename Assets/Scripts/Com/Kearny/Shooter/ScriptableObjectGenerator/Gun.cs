using UnityEngine;

namespace Com.Kearny.Shooter.ScriptableObjectGenerator
{
    [CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
    public class Gun : ScriptableObject
    {
        public string name;
        public float fireRate;
        public float bloom;
        public float recoil;
        public float kickBack;
        public float aimSpeed;
        public float impactForce;
        public GameObject prefab;
    }
}
