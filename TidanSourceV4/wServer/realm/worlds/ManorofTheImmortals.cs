﻿namespace wServer.realm.worlds
{
    public class ManoroftheImmortals : World
    {
        public ManoroftheImmortals()
        {
            Name = "Manor of the Immortals";
            ClientWorldName = "Manor of the Immortals";
            Background = 0;
            Dungeon = true;
            Difficulty = 4;
            AllowTeleport = true;
        }

        public override bool NeedsPortalKey => true;

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.Manor.jm", MapType.Json);
        }
    }
}
