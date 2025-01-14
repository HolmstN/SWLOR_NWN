using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Quest;

namespace SWLOR.Game.Server.Scripts.Quest.GuildTasks.WeaponsmithGuild
{
    public class BatonRepairKitIV: AbstractQuest
    {
        public BatonRepairKitIV()
        {
            CreateQuest(327, "Weaponsmith Guild Task: 1x Baton Repair Kit IV", "wpn_tsk_327")
                .IsRepeatable()

                .AddObjectiveCollectItem(1, "bt_rep_4", 1, true)

                .AddRewardGold(420)
                .AddRewardGuildPoints(GuildType.WeaponsmithGuild, 88);
        }
    }
}
