﻿namespace wServer.realm.worlds
{
    public class DeadwaterDocks : World
    {
        public DeadwaterDocks()
        {
            Name = "DeadWater Docks";
            ClientWorldName = "DeadWater Docks";
            Background = 0;
            Difficulty = 5;
            AllowTeleport = true;
        }

        public override bool NeedsPortalKey => true;

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.ddocks.jm", MapType.Json);
        }
    }
}