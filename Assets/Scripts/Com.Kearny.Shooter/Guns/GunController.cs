using UnityEngine;

namespace Com.Kearny.Shooter.Guns
{
    public class GunController : MonoBehaviour
    {
        public Transform weaponHold;
        public Gun equippedGun;
        private bool _isGunEquipped;

        private void Start()
        {
            if (equippedGun != null)
            {
                EquipGun(equippedGun);
            }
        }

        private void Update()
        {
            // Si aucune arme n'est �quip�e alors on ne fait rien.
            if (!_isGunEquipped) return;

            if (Input.GetButton("Fire1"))
            {
                OnTriggerHold();
            }

            if (Input.GetButtonUp("Fire1"))
            {
                OnTriggerRelease();
            }
        }

        /// <summary>
        /// Equipe une arme 
        /// </summary>
        /// <param name="gunToEquip"></param>
        public void EquipGun(Gun gunToEquip)
        {
            // Ici tu pourrais rajouter une propri�t� dans ton entit� Gun afin de ne pas recr�er ton objet Gun syst�matiquement.
            // Ce que je vois ici, c'est que si tu as un pistolet d'�quiper et que tu changes pour un fusil d'assaut c'est ok.
            // Mais si tu changes d'armes pour en fait s�lectionner la m�me, est-ce vraiment utile de forcer le jeu � la recharger ?
            // Donc tu pourrais rajouter une identifiant unique sur chaque arme que tu cr��eras pour que si le joueur essaye d'�quiper uen arme qu'il a d�j� en main �a ne la r�instancie pas.
            if (equippedGun != null /* && equippedGun.uniqueIdentifier != gunToEquip.uniqueIdentifier*/)
            {
                Destroy(equippedGun.gameObject);
            }

            equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation, weaponHold);
            _isGunEquipped = true;
        }

        private void OnTriggerHold()
        {
            equippedGun.OnTriggerHold();
        }

        private void OnTriggerRelease()
        {
            equippedGun.OnTriggerRelease();
        }
    }
}