﻿using SWLOR.Game.Server.Messaging;
using SWLOR.Game.Server.NWN.Events.Creature;


// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    public class crea_on_rested
#pragma warning restore IDE1006 // Naming Styles
    {
        public static void Main()
        {
            MessageHub.Instance.Publish(new OnCreatureRested());
        }
    }
}
