using System.Collections.Generic;
using SWLOR.Game.Server.Core.Bioware;
using SWLOR.Game.Server.Core.NWScript.Enum;
using SWLOR.Game.Server.Core.NWScript.Enum.Item;
using SWLOR.Game.Server.Service.CraftService;
using static SWLOR.Game.Server.Core.NWScript.NWScript;

namespace SWLOR.Game.Server.Feature.ItemModDefinition
{
    public class KillerItemModDefinition: IItemModListDefinition
    {
        private readonly ItemModBuilder _builder = new ItemModBuilder();

        public Dictionary<string, ItemModDetail> BuildItemMods()
        {
            CreateMod("mod_w_humankill", "Human Killer", RacialType.Human);
            CreateMod("mod_w_animalkill", "Animal Killer", RacialType.Animal);
            CreateMod("mod_w_beastkill", "Beast Killer", RacialType.Beast);
            CreateMod("mod_w_verminkill", "Vermin Killer", RacialType.Vermin);
            CreateMod("mod_w_undeadkill", "Undead Killer", RacialType.Undead);
            CreateMod("mod_w_robotkill", "Robot Killer", RacialType.Robot);
            CreateMod("mod_w_bothankill", "Bothan Killer", RacialType.Bothan);
            CreateMod("mod_w_chisskill", "Chiss Killer", RacialType.Chiss);
            CreateMod("mod_w_zabrakkill", "Zabrak Killer", RacialType.Zabrak);
            CreateMod("mod_w_wookieekil", "Wookiee Killer", RacialType.Wookiee);
            CreateMod("mod_w_twilekkill", "Twi'lek Killer", RacialType.Twilek);
            CreateMod("mod_w_cyborgkill", "Cyborg Killer", RacialType.Cyborg);
            CreateMod("mod_w_catharkill", "Cathar Killer", RacialType.Cathar);
            CreateMod("mod_w_trandokill", "Trandoshan Killer", RacialType.Trandoshan);
            CreateMod("mod_w_mirialanki", "Mirialan Killer", RacialType.Mirialan);
            CreateMod("mod_w_echanikill", "Echani Killer", RacialType.Echani);
            CreateMod("mod_w_moncalkill", "Mon Calamari Killer", RacialType.MonCalamari);
            CreateMod("mod_w_uggykill", "Ugnaught Killer", RacialType.Ugnaught);

            return _builder.Build();

        }

        private void CreateMod(string tag, string name, RacialType racialType)
        {
            _builder.Create(tag, ItemModType.Weapon)
                .Name(name)
                .ApplyAction((user, mod, item) =>
                {
                    var amount = 1;

                    for (var ip = GetFirstItemProperty(item); GetIsItemPropertyValid(ip); ip = GetNextItemProperty(item))
                    {
                        if (GetItemPropertyType(ip) == ItemPropertyType.AttackBonusVsRacialGroup)
                        {
                            var existingRacialType = (RacialType)GetItemPropertySubType(ip);
                            if (existingRacialType == racialType)
                            {
                                var existingBonus = GetItemPropertyCostTableValue(ip);
                                amount += existingBonus;
                            }
                        }
                    }

                    var newIP = ItemPropertyAttackBonusVsRace(racialType, amount);
                    BiowareXP2.IPSafeAddItemProperty(item, newIP, 0.0f, AddItemPropertyPolicy.ReplaceExisting, true, false);
                });
        }
    }
}
