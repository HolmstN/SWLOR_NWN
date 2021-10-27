﻿using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.GameObject;

using SWLOR.Game.Server.NWN;
using SWLOR.Game.Server.NWN.Enum;
using SWLOR.Game.Server.NWN.Enum.Item;
using SWLOR.Game.Server.NWNX;
using SWLOR.Game.Server.Service;

namespace SWLOR.Game.Server.Perk.Weapons
{
    public abstract class WeaponFocusBase : IPerkHandler
    {
        public abstract PerkType PerkType { get; }

        public string CanCastSpell(NWCreature oPC, NWObject oTarget, int spellTier)
        {
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
        }

        public void OnPurchased(NWCreature creature, int newLevel)
        {
            ApplyFeatChanges(creature, null);
        }

        public void OnRemoved(NWCreature creature)
        {
            ApplyFeatChanges(creature, null);
        }

        public void OnItemEquipped(NWCreature creature, NWItem oItem)
        {
            ApplyFeatChanges(creature, null);
        }

        public void OnItemUnequipped(NWCreature creature, NWItem oItem)
        {
            ApplyFeatChanges(creature, oItem);
        }

        public void OnCustomEnmityRule(NWCreature creature, int amount)
        {
        }

        private void ApplyFeatChanges(NWCreature creature, NWItem oItem)
        {
            NWItem equipped = oItem ?? creature.RightHand;
            RemoveAllFeats(creature);

            // Unarmed check
            NWItem mainHand = creature.RightHand;
            NWItem offHand = creature.LeftHand;
            if (oItem != null && Equals(oItem, mainHand))
            {
                mainHand = _.OBJECT_INVALID;
            }
            else if (oItem != null && Equals(oItem, offHand))
            {
                offHand = _.OBJECT_INVALID;
            }

            if (!mainHand.IsValid && !offHand.IsValid) 
            {
                int martialArtsLevel = PerkService.GetCreaturePerkLevel(creature, PerkType.WeaponFocusMartialArts);
                if (martialArtsLevel >= 1)
                {
                    CreaturePlugin.AddFeat(creature, Feat.WeaponFocus_UnarmedStrike);
                }
                if (martialArtsLevel >= 2)
                {
                    CreaturePlugin.AddFeat(creature, Feat.WeaponSpecialization_UnarmedStrike);
                }

                return;
            }

            if (oItem != null && Equals(oItem, equipped)) return;

            // All other weapon types
            PerkType perkType;
            switch (equipped.CustomItemType)
            {
                case CustomItemType.Vibroblade: perkType = PerkType.WeaponFocusVibroblades; break;
                case CustomItemType.FinesseVibroblade: perkType = PerkType.WeaponFocusFinesseVibroblades; break;
                case CustomItemType.Baton: perkType = PerkType.WeaponFocusBatons; break;
                case CustomItemType.HeavyVibroblade: perkType = PerkType.WeaponFocusHeavyVibroblades; break;
                case CustomItemType.Polearm: perkType = PerkType.WeaponFocusPolearms; break;
                case CustomItemType.TwinBlade: perkType = PerkType.WeaponFocusTwinVibroblades; break;
                case CustomItemType.MartialArtWeapon: perkType = PerkType.WeaponFocusMartialArts; break;
                case CustomItemType.BlasterPistol: perkType = PerkType.WeaponFocusBlasterPistols; break;
                case CustomItemType.BlasterRifle: perkType = PerkType.WeaponFocusBlasterRifles; break;
                case CustomItemType.Throwing: perkType = PerkType.WeaponFocusThrowing; break;
                case CustomItemType.Lightsaber: perkType = PerkType.WeaponFocusLightsaber; break;
                case CustomItemType.Saberstaff: perkType = PerkType.WeaponFocusSaberstaff; break;
                default: return;
            }

            if (equipped.GetLocalBool("LIGHTSABER") == true)
            {
                perkType = PerkType.WeaponFocusLightsaber;
            }
            
            var perkLevel = PerkService.GetCreaturePerkLevel(creature, perkType);
            var type = equipped.BaseItemType;
            if (perkLevel >= 1)
            {
                AddFocusFeat(creature, type);
            }
            if (perkLevel >= 2)
            {
                AddSpecializationFeat(creature, type);
            }
        }

        private void RemoveAllFeats(NWCreature creature)
        {
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_BattleAxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Club);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Dagger);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Dart);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_DireMace);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_DoubleAxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Dwaxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_GreatAxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_GreatSword);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Halberd);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_HandAxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_HeavyCrossbow);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_HeavyFlail);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Kama);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Katana);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Kukri);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_LightCrossbow);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_LightFlail);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_LightHammer);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_LightMace);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Longbow);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_LongSword);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_MorningStar);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Rapier);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Scimitar);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Scythe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Shortbow);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_ShortSword);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Shuriken);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Sickle);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Sling);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Spear);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Staff);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_ThrowingAxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Trident);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_TwoBladedSword);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_UnarmedStrike);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_WarHammer);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponFocus_Whip);


            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_BastardSword);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_BattleAxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Club);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Dagger);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Dart);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_DireMace);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_DoubleAxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Dwaxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_GreatAxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_GreatSword);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Halberd);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_HandAxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_HeavyCrossbow);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_HeavyFlail);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Kama);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Katana);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Kukri);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_LightCrossbow);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_LightFlail);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_LightHammer);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_LightMace);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Longbow);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_LongSword);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_MorningStar);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Rapier);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Scimitar);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Scythe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Shortbow);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_ShortSword);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Shuriken);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Sickle);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Sling);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Spear);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Staff);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_ThrowingAxe);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Trident);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_TwoBladedSword);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_UnarmedStrike);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_WarHammer);
            CreaturePlugin.RemoveFeat(creature, Feat.WeaponSpecialization_Whip);

        }

        private void AddFocusFeat(NWCreature creature, BaseItem type)
        {
            Feat feat;

            switch (type)
            {
                case (BaseItem.BastardSword): feat = Feat.WeaponFocus_BastardSword; break;
                case (BaseItem.BattleAxe): feat = Feat.WeaponFocus_BattleAxe; break;
                case (BaseItem.Club): feat = Feat.WeaponFocus_Club; break;
                case (BaseItem.Dagger): feat = Feat.WeaponFocus_Dagger; break;
                case (BaseItem.Dart): feat = Feat.WeaponFocus_Dart; break;
                case (BaseItem.DireMace): feat = Feat.WeaponFocus_DireMace; break;
                case (BaseItem.DoubleAxe): feat = Feat.WeaponFocus_DoubleAxe; break;
                case (BaseItem.DwarvenWarAxe): feat = Feat.WeaponFocus_Dwaxe; break;
                case (BaseItem.GreatAxe): feat = Feat.WeaponFocus_GreatAxe; break;
                case (BaseItem.GreatSword): feat = Feat.WeaponFocus_GreatSword; break;
                case (BaseItem.Halberd): feat = Feat.WeaponFocus_Halberd; break;
                case (BaseItem.HandAxe): feat = Feat.WeaponFocus_HandAxe; break;
                case (BaseItem.HeavyCrossbow): feat = Feat.WeaponFocus_HeavyCrossbow; break;
                case (BaseItem.HeavyFlail): feat = Feat.WeaponFocus_HeavyFlail; break;
                case (BaseItem.Kama): feat = Feat.WeaponFocus_Kama; break;
                case (BaseItem.Katana): feat = Feat.WeaponFocus_Katana; break;
                case (BaseItem.Kukri): feat = Feat.WeaponFocus_Kukri; break;
                case (BaseItem.LightCrossbow): feat = Feat.WeaponFocus_LightCrossbow; break;
                case (BaseItem.LightFlail): feat = Feat.WeaponFocus_LightFlail; break;
                case (BaseItem.LightHammer): feat = Feat.WeaponFocus_LightHammer; break;
                case (BaseItem.LightMace): feat = Feat.WeaponFocus_LightMace; break;
                case (BaseItem.Longbow): feat = Feat.WeaponFocus_Longbow; break;
                case (BaseItem.Longsword): feat = Feat.WeaponFocus_LongSword; break;
                case (BaseItem.MorningStar): feat = Feat.WeaponFocus_MorningStar; break;
                case (BaseItem.Rapier): feat = Feat.WeaponFocus_Rapier; break;
                case (BaseItem.Scimitar): feat = Feat.WeaponFocus_Scimitar; break;
                case (BaseItem.Scythe): feat = Feat.WeaponFocus_Scythe; break;
                case (BaseItem.ShortBow): feat = Feat.WeaponFocus_Shortbow; break;
                case (BaseItem.ShortSword): feat = Feat.WeaponFocus_ShortSword; break;
                case (BaseItem.Shuriken): feat = Feat.WeaponFocus_Shuriken; break;
                case (BaseItem.Sickle): feat = Feat.WeaponFocus_Sickle; break;
                case (BaseItem.Sling): feat = Feat.WeaponFocus_Sling; break;
                case (BaseItem.ShortSpear): feat = Feat.WeaponFocus_Spear; break;
                case (BaseItem.QuarterStaff): feat = Feat.WeaponFocus_Staff; break;
                case (BaseItem.ThrowingAxe): feat = Feat.WeaponFocus_ThrowingAxe; break;
                case (BaseItem.Trident): feat = Feat.WeaponFocus_Trident; break;
                case (BaseItem.TwoBladedSword): feat = Feat.WeaponFocus_TwoBladedSword; break;
                case (BaseItem.Invalid): feat = Feat.WeaponFocus_UnarmedStrike; break;
                case (BaseItem.WarHammer): feat = Feat.WeaponFocus_WarHammer; break;
                case (BaseItem.Whip): feat = Feat.WeaponFocus_Whip; break;
                case (BaseItem.Lightsaber): feat = Feat.WeaponFocus_LongSword; break;
                case (BaseItem.Saberstaff): feat = Feat.WeaponFocus_TwoBladedSword; break;
                default: return;
            }

            CreaturePlugin.AddFeat(creature, (Feat)feat);
        }


        private void AddSpecializationFeat(NWCreature creature, BaseItem type)
        {
            Feat feat;

            switch (type)
            {
                case (BaseItem.BastardSword): feat = Feat.WeaponSpecialization_BastardSword; break;
                case (BaseItem.BattleAxe): feat = Feat.WeaponSpecialization_BattleAxe; break;
                case (BaseItem.Club): feat = Feat.WeaponSpecialization_Club; break;
                case (BaseItem.Dagger): feat = Feat.WeaponSpecialization_Dagger; break;
                case (BaseItem.Dart): feat = Feat.WeaponSpecialization_Dart; break;
                case (BaseItem.DireMace): feat = Feat.WeaponSpecialization_DireMace; break;
                case (BaseItem.DoubleAxe): feat = Feat.WeaponSpecialization_DoubleAxe; break;
                case (BaseItem.DwarvenWarAxe): feat = Feat.WeaponSpecialization_Dwaxe; break;
                case (BaseItem.GreatAxe): feat = Feat.WeaponSpecialization_GreatAxe; break;
                case (BaseItem.GreatSword): feat = Feat.WeaponSpecialization_GreatSword; break;
                case (BaseItem.Halberd): feat = Feat.WeaponSpecialization_Halberd; break;
                case (BaseItem.HandAxe): feat = Feat.WeaponSpecialization_HandAxe; break;
                case (BaseItem.HeavyCrossbow): feat = Feat.WeaponSpecialization_HeavyCrossbow; break;
                case (BaseItem.HeavyFlail): feat = Feat.WeaponSpecialization_HeavyFlail; break;
                case (BaseItem.Kama): feat = Feat.WeaponSpecialization_Kama; break;
                case (BaseItem.Katana): feat = Feat.WeaponSpecialization_Katana; break;
                case (BaseItem.Kukri): feat = Feat.WeaponSpecialization_Kukri; break;
                case (BaseItem.LightCrossbow): feat = Feat.WeaponSpecialization_LightCrossbow; break;
                case (BaseItem.LightFlail): feat = Feat.WeaponSpecialization_LightFlail; break;
                case (BaseItem.LightHammer): feat = Feat.WeaponSpecialization_LightHammer; break;
                case (BaseItem.LightMace): feat = Feat.WeaponSpecialization_LightMace; break;
                case (BaseItem.Longbow): feat = Feat.WeaponSpecialization_Longbow; break;
                case (BaseItem.Longsword): feat = Feat.WeaponSpecialization_LongSword; break;
                case (BaseItem.MorningStar): feat = Feat.WeaponSpecialization_MorningStar; break;
                case (BaseItem.Rapier): feat = Feat.WeaponSpecialization_Rapier; break;
                case (BaseItem.Scimitar): feat = Feat.WeaponSpecialization_Scimitar; break;
                case (BaseItem.Scythe): feat = Feat.WeaponSpecialization_Scythe; break;
                case (BaseItem.ShortBow): feat = Feat.WeaponSpecialization_Shortbow; break;
                case (BaseItem.ShortSword): feat = Feat.WeaponSpecialization_ShortSword; break;
                case (BaseItem.Shuriken): feat = Feat.WeaponSpecialization_Shuriken; break;
                case (BaseItem.Sickle): feat = Feat.WeaponSpecialization_Sickle; break;
                case (BaseItem.Sling): feat = Feat.WeaponSpecialization_Sling; break;
                case (BaseItem.ShortSpear): feat = Feat.WeaponSpecialization_Spear; break;
                case (BaseItem.QuarterStaff): feat = Feat.WeaponSpecialization_Staff; break;
                case (BaseItem.ThrowingAxe): feat = Feat.WeaponSpecialization_ThrowingAxe; break;
                case (BaseItem.Trident): feat = Feat.WeaponSpecialization_Trident; break;
                case (BaseItem.TwoBladedSword): feat = Feat.WeaponSpecialization_TwoBladedSword; break;
                case (BaseItem.Invalid): feat = Feat.WeaponSpecialization_UnarmedStrike; break;
                case (BaseItem.WarHammer): feat = Feat.WeaponSpecialization_WarHammer; break;
                case (BaseItem.Whip): feat = Feat.WeaponSpecialization_Whip; break;
                case (BaseItem.Lightsaber): feat = Feat.WeaponSpecialization_LongSword; break;
                case (BaseItem.Saberstaff): feat = Feat.WeaponSpecialization_TwoBladedSword; break;
                default: return;
            }

            CreaturePlugin.AddFeat(creature, (Feat)feat);
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
