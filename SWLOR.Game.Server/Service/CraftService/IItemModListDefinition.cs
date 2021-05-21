using System.Collections.Generic;

namespace SWLOR.Game.Server.Service.CraftService
{
    public interface IItemModListDefinition
    {
        public Dictionary<string, ItemModDetail> BuildItemMods();
    }
}
