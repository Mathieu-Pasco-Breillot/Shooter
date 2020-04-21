using System;

namespace Com.Kearny.Shooter.Guns
{
    public class Pistol : Gun
    {
        protected override FireMode FireMode { get; set; } = FireMode.SemiAuto;

        /// <summary>
        /// Vérifie que l'arme peut utiliser le mode de tir : <paramref name="fireModeToCheck"/>
        /// </summary>
        /// <param name="fireModeToCheck">Le mode de tir à appliquer à l'arme.</param>
        /// <returns>Vrai si le mode demandé est <see cref="FireMode.SemiAuto"/>; Faux sinon.</returns>
        protected override bool IsFireModeAllow(FireMode fireModeToCheck)
        {
            return fireModeToCheck == FireMode.SemiAuto;
        }
    }
}