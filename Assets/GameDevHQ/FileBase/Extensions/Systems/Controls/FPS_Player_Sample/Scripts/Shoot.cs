using System.Collections;
using System.Collections.Generic;
using Com.Kearny.Shooter.Enemy;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private AudioSource source;
    private Animator anim;
    [SerializeField]
    private ParticleSystem smokeParticle, muzzleFlash, tracer;
    [SerializeField]
    private bool canFire = true;
    [SerializeField]
    private GameObject bloodSplat;
    
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        //if left click
        if (Input.GetMouseButtonDown(0) && canFire)
        {
            canFire = false;

            anim.SetTrigger("Fire");
            source.Play();
            smokeParticle.Emit(5);
            tracer.Emit(10);
            muzzleFlash.Emit(1);
            StartCoroutine(WeaponCoolDownRoutine());

            //cast ray from center of the screen through the radicule 
            var rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            RaycastHit hitInfo; //Store information about the object we hit

            //cast a ray
            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                var enemy = hitInfo.collider.GetComponent<EnemyIntelligence>();

                if (enemy != null)
                {
                    var blood = Instantiate(bloodSplat, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(blood, 5.0f);
                    enemy.Damage(20);
                }
            }
        }
    }

    private IEnumerator WeaponCoolDownRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        canFire = true;
    }
}
