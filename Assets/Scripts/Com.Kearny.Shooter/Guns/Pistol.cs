using System;

namespace Com.Kearny.Shooter.Guns
{
    public class Pistol : Gun
    {
        protected override FireMode FireMode { get; set; } = FireMode.SemiAuto;

        /// <summary>
        /// V�rifie que l'arme peut utiliser le mode de tir : <paramref name="fireModeToCheck"/>
        /// </summary>
        /// <param name="fireModeToCheck">Le mode de tir � appliquer � l'arme.</param>
        /// <returns>Vrai si le mode demand� est <see cref="FireMode.SemiAuto"/>; Faux sinon.</returns>
        protected override bool IsFireModeAllow(FireMode fireModeToCheck)
        {
            return fireModeToCheck == FireMode.SemiAuto;
        }
    }
}