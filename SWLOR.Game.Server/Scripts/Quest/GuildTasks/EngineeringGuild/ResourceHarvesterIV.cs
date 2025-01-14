using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Quest;

namespace SWLOR.Game.Server.Scripts.Quest.GuildTasks.EngineeringGuild
{
    public class ResourceHarvesterIV: AbstractQuest
    {
        public ResourceHarvesterIV()
        {
            CreateQuest(545, "Engineering Guild Task: 1x Resource Harvester IV", "eng_tsk_545")
                .IsRepeatable()

                .AddObjectiveCollectItem(1, "harvest_r_4", 1, true)

                .AddRewardGold(410)
                .AddRewardGuildPoints(GuildType.EngineeringGuild, 85);
        }
    }
}
