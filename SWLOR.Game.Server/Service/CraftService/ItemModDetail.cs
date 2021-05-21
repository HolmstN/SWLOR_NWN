namespace SWLOR.Game.Server.Service.CraftService
{
    public delegate void ApplyItemModDelegate(uint user, uint mod, uint item);
    public class ItemModDetail
    {
        public string Name { get; set; }
        public ItemModType ModType { get; set; }
        public ApplyItemModDelegate ApplyItemModAction { get; set; }

        public ItemModDetail()
        {
            Name = string.Empty;
            ModType = ItemModType.None;
        }
    }
}
