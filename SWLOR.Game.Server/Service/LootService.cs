﻿using System.Linq;
using System.Numerics;
using SWLOR.Game.Server.NWN;
using SWLOR.Game.Server.Data.Entity;
using SWLOR.Game.Server.GameObject;
using SWLOR.Game.Server.Messaging;
using SWLOR.Game.Server.NWN.Enum;
using SWLOR.Game.Server.NWN.Events.Creature;
using SWLOR.Game.Server.ValueObject;
using static SWLOR.Game.Server.NWN._;

namespace SWLOR.Game.Server.Service
{
    public static class LootService
    {
        public static void SubscribeEvents()
        {
            MessageHub.Instance.Subscribe<OnCreatureDeath>(message => OnCreatureDeath());
        }

        public static ItemVO PickRandomItemFromLootTable(int lootTableID)
        {
            if (lootTableID <= 0) return null;
            var lootTableItems = DataService.LootTableItem.GetAllByLootTableID(lootTableID).ToList();

            if (lootTableItems.Count <= 0) return null;
            int[] weights = new int[lootTableItems.Count];
            for (int x = 0; x < lootTableItems.Count; x++)
            {
                weights[x] = lootTableItems.ElementAt(x).Weight;
            }

            int randomIndex = RandomService.GetRandomWeightedIndex(weights);
            LootTableItem itemEntity = lootTableItems.ElementAt(randomIndex);
            int quantity = RandomService.Random(itemEntity.MaxQuantity) + 1;
            ItemVO result = new ItemVO
            {
                Quantity = quantity,
                Resref = itemEntity.Resref,
                SpawnRule = itemEntity.SpawnRule
            };
            
            return result;
        }

        private static void OnCreatureDeath()
        {
            ProcessLoot();
            ProcessCorpse();
        }

        private static void ProcessLoot()
        {
            NWCreature creature = _.OBJECT_SELF;
            
            // Single loot table (without an index)
            int singleLootTableID = creature.GetLocalInt("LOOT_TABLE_ID");
            if (singleLootTableID > 0)
            {
                int chance = creature.GetLocalInt("LOOT_TABLE_CHANCE");
                int attempts = creature.GetLocalInt("LOOT_TABLE_ATTEMPTS");

                RunLootAttempt(creature, singleLootTableID, chance, attempts);
            }

            // Multiple loot tables (with an index)
            int lootTableNumber = 1;
            int lootTableID = creature.GetLocalInt("LOOT_TABLE_ID_" + lootTableNumber);
            while (lootTableID > 0)
            {
                int chance = creature.GetLocalInt("LOOT_TABLE_CHANCE_" + lootTableNumber);
                int attempts = creature.GetLocalInt("LOOT_TABLE_ATTEMPTS_" + lootTableNumber);

                RunLootAttempt(creature, lootTableID, chance, attempts);

                lootTableNumber++;
                lootTableID = creature.GetLocalInt("LOOT_TABLE_ID_" + lootTableNumber);
            }
        }

        private static void RunLootAttempt(NWCreature target, int lootTableID, int chance, int attempts)
        {
            if (chance <= 0)
                chance = 75;
            else if (chance > 100) chance = 100;

            if (attempts <= 0)
                attempts = 1;

            for (int a = 1; a <= attempts; a++)
            {
                if (RandomService.Random(100) + 1 <= chance)
                {
                    ItemVO model = PickRandomItemFromLootTable(lootTableID);
                    if (model == null) continue;

                    int spawnQuantity = model.Quantity > 1 ? RandomService.Random(1, model.Quantity) : 1;

                    for (int x = 1; x <= spawnQuantity; x++)
                    {
                        var item = CreateItemOnObject(model.Resref, target);
                        if (!string.IsNullOrWhiteSpace(model.SpawnRule))
                        {
                            var rule = SpawnService.GetSpawnRule(model.SpawnRule);
                            rule.Run(item);
                        }
                    }
                }
            }
        }


        private static void ProcessCorpse()
        {
            SetIsDestroyable(false);

            NWObject self = _.OBJECT_SELF;
            if (self.Tag == "spaceship_copy") return;

            Vector3 lootPosition = Vector3(self.Position.X, self.Position.Y, self.Position.Z - 0.11f);
            Location spawnLocation = Location(self.Area, lootPosition, self.Facing);

            NWPlaceable container = CreateObject(ObjectType.Placeable, "corpse", spawnLocation);
            container.SetLocalObject("CORPSE_BODY", self);
            container.Name = self.Name + "'s Corpse";

            container.AssignCommand(() =>
            {
                TakeGoldFromCreature(self.Gold, self);
            });

            // Dump equipped items in container
            for (var slot = 0; slot < NumberOfInventorySlots; slot++)
            {
                var inventorySlot = (InventorySlot) slot;
                if (inventorySlot == InventorySlot.CreatureArmor ||
                    inventorySlot == InventorySlot.CreatureBite ||
                    inventorySlot == InventorySlot.CreatureRight ||
                    inventorySlot == InventorySlot.CreatureLeft)
                    continue;

                NWItem item = GetItemInSlot(inventorySlot, self);
                if (item.IsValid && !item.IsCursed && item.IsDroppable)
                {
                    NWItem copy = CopyItem(item, container, true);

                    if (inventorySlot == InventorySlot.Head  ||
                        inventorySlot == InventorySlot.Chest)
                    {
                        copy.SetLocalObject("CORPSE_ITEM_COPY", item);
                    }
                    else
                    {
                        item.Destroy();
                    }
                }
            }

            foreach (var item in self.InventoryItems)
            {
                if (item.IsValid && !item.IsCursed && item.IsDroppable)
                {
                    CopyItem(item, container, true);
                    item.Destroy();
                }
            }

            DelayCommand(360.0f, () =>
            {
                if (!container.IsValid) return;

                NWObject body = container.GetLocalObject("CORPSE_BODY");
                body.AssignCommand(() => SetIsDestroyable(true));
                body.DestroyAllInventoryItems();
                body.Destroy();

                container.DestroyAllInventoryItems();
                container.Destroy();
            });

        }

    }
}
