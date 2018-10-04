﻿using NWN;
using SWLOR.Game.Server.ChatCommand.Contracts;
using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.GameObject;
using static NWN.NWScript;

namespace SWLOR.Game.Server.ChatCommand
{
    [CommandDetails("Plays a scratch head animation.", CommandPermissionType.Player | CommandPermissionType.DM)]
    public class ScratchHead : IChatCommand
    {
        private readonly INWScript _;

        public ScratchHead(INWScript script)
        {
            _ = script;
        }

        public void DoAction(NWPlayer user, params string[] args)
        {
            user.AssignCommand(() =>
            {
                _.ActionPlayAnimation(ANIMATION_FIREFORGET_PAUSE_SCRATCH_HEAD);
            });
        }
    }
}