using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Quest;

namespace SWLOR.Game.Server.Scripts.Quest.GuildTasks.WeaponsmithGuild
{
    public class BasicVibrobladeK: AbstractQuest
    {
        public BasicVibrobladeK()
        {
            CreateQuest(238, "Weaponsmith Guild Task: 1x Basic Vibroblade K", "wpn_tsk_238")
                .IsRepeatable()

                .AddObjectiveCollectItem(1, "katana_b", 1, true)

                .AddRewardGold(35)
                .AddRewardGuildPoints(GuildType.WeaponsmithGuild, 9);
        }
    }
}
