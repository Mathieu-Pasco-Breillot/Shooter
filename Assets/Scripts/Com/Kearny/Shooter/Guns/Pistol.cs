namespace Com.Kearny.Shooter.Guns
{
    public class Pistol : Gun
    {
        public override FireType FireType
        {
            get => FireType.SemiAutomatic;
        }
    }
}