﻿using SWLOR.Game.Server.Core.NWScript;
using SWLOR.Game.Server.Core.NWScript.Enum;
using SWLOR.Game.Server.Core.NWScript.Enum.Item;
using SWLOR.Game.Server.Legacy.Enumeration;
using SWLOR.Game.Server.Legacy.GameObject;
using SWLOR.Game.Server.Legacy.Service;
using PerkType = SWLOR.Game.Server.Legacy.Enumeration.PerkType;

namespace SWLOR.Game.Server.Legacy.Perk.OneHanded
{
    public class Knockdown: IPerkHandler
    {
        public PerkType PerkType => PerkType.Knockdown;

        public string CanCastSpell(NWCreature oPC, NWObject oTarget, int spellTier)
        {
            var weapon = oPC.RightHand;
            if (weapon.CustomItemType != CustomItemType.Baton && weapon.BaseItemType != BaseItem.Club)
                return "You must be equipped with a baton weapon to use that ability.";

            return string.Empty;
        }
        
        public int FPCost(NWCreature oPC, int baseFPCost, int spellTier)
        {
            return baseFPCost;
        }

        public float CastingTime(NWCreature oPC, float baseCastingTime, int spellTier)
        {
            return baseCastingTime;
        }

        public float CooldownTime(NWCreature oPC, float baseCooldownTime, int spellTier)
        {
            return baseCooldownTime;
        }

        public int? CooldownCategoryID(NWCreature creature, int? baseCooldownCategoryID, int spellTier)
        {
            return baseCooldownCategoryID;
        }

        public void OnImpact(NWCreature creature, NWObject target, int perkLevel, int spellTier)
        {
            int damage;
            float length;

            switch (perkLevel)
            {
                case 1:
                    damage = SWLOR.Game.Server.Service.Random.D4(1);
                    length = 6.0f;
                    break;
                case 2:
                    damage = SWLOR.Game.Server.Service.Random.D4(2);
                    length = 6.0f;
                    break;
                case 3:
                    damage = SWLOR.Game.Server.Service.Random.D6(2);
                    length = 6.0f;
                    break;
                case 4:
                    damage = SWLOR.Game.Server.Service.Random.D6(2);
                    length = 9.0f;
                    break;
                case 5:
                    damage = SWLOR.Game.Server.Service.Random.D6(3);
                    length = 9.0f;
                    break;
                case 6:
                    damage = SWLOR.Game.Server.Service.Random.D8(3);
                    length = 9.0f;
                    break;
                default: return;
            }

            NWScript.ApplyEffectToObject(DurationType.Temporary, AbilityService.EffectKnockdown(target, length), target.Object, length);
            NWScript.ApplyEffectToObject(DurationType.Instant, NWScript.EffectDamage(damage, DamageType.Bludgeoning), target);
        }

        public void OnPurchased(NWCreature creature, int newLevel)
        {
        }

        public void OnRemoved(NWCreature creature)
        {
        }

        public void OnItemEquipped(NWCreature creature, NWItem oItem)
        {
        }

        public void OnItemUnequipped(NWCreature creature, NWItem oItem)
        {
        }

        public void OnCustomEnmityRule(NWCreature creature, int amount)
        {
        }

        public bool IsHostile()
        {
            return false;
        }

        public void OnConcentrationTick(NWCreature creature, NWObject target, int perkLevel, int tick)
        {
            
        }
    }
}