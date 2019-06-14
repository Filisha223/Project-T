#region

using wServer.networking;

#endregion

namespace wServer.realm.worlds
{
    public class Moon : World
    {
        public Moon()
        {
            Name = "Moon";
            ClientWorldName = "Moon";
            Background = 2;
            IsLimbo = true;
            AllowTeleport = true;
        }


        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.Moon.jm", MapType.Json);
        }

        public override World GetInstance(Client psr) => Manager.AddWorld(new Moon());
    }
}