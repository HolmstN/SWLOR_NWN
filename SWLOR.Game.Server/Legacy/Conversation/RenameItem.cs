﻿using SWLOR.Game.Server.Core.NWScript;
using SWLOR.Game.Server.Legacy.GameObject;
using SWLOR.Game.Server.Legacy.ValueObject.Dialog;
using SWLOR.Game.Server.Service;

namespace SWLOR.Game.Server.Legacy.Conversation
{
    public class RenameItem: ConversationBase
    {
        public override PlayerDialog SetUp(NWPlayer player)
        {
            var dialog = new PlayerDialog("MainPage");
            var mainPage = new DialogPage("<SET LATER>",
                ColorToken.Green("Refresh"),
                "Change Name",
                "Reset Name");

            dialog.AddPage("MainPage", mainPage);
            return dialog;
        }

        public override void Initialize()
        {
            LoadHeader();
        }

        private void LoadHeader()
        {
            var player = GetPC();
            player.SetLocalBool("ITEM_RENAMING_LISTENING", true);

            NWItem item = player.GetLocalObject("ITEM_BEING_RENAMED");
            var originalName = item.GetLocalString("RENAMED_ITEM_ORIGINAL_NAME");
            if (string.IsNullOrWhiteSpace(originalName))
                originalName = item.Name;
            var currentName = item.Name;
            var renamingName = player.GetLocalString("RENAMED_ITEM_NEW_NAME");
            if (string.IsNullOrWhiteSpace(renamingName))
                renamingName = "{UNSPECIFIED}";

            var header = "You are renaming an item.\n\n";
            header += ColorToken.Green("Original Name: ") + originalName + "\n";
            header += ColorToken.Green("Current Name: ") + currentName + "\n";
            header += ColorToken.Green("New Name: ") + renamingName + "\n";
            header += "Type in a new name, click 'Refresh', and then select 'Change Name' to make the changes. Click 'Reset Name' to switch back to the item's original name.";

            SetPageHeader("MainPage", header);
        }


        public override void DoAction(NWPlayer player, string pageName, int responseID)
        {
            switch (responseID)
            {
                case 1: // Refresh
                    LoadHeader();
                    break;
                case 2: // Change Name
                    ChangeName();
                    LoadHeader();
                    break;
                case 3: // Reset Name
                    ResetName();
                    LoadHeader();
                    break;
            }
        }

        public override void Back(NWPlayer player, string beforeMovePage, string afterMovePage)
        {
        }

        private void ChangeName()
        {
            var player = GetPC();

            // Player hasn't specified a new name for this item.
            var newName = player.GetLocalString("RENAMED_ITEM_NEW_NAME");
            if(string.IsNullOrWhiteSpace(newName))
            {
                player.FloatingText("Enter a new name into the chat box, select 'Refresh' then select 'Change Name'.");
                return;
            }
            NWItem item = player.GetLocalObject("ITEM_BEING_RENAMED");

            // Item isn't in player's inventory.
            if (NWScript.GetItemPossessor(item) != player.Object)
            {
                player.FloatingText("Item must be in your inventory in order to rename it.");
                return;
            }

            // Item's original name isn't being stored. Do that now.
            if(string.IsNullOrWhiteSpace(item.GetLocalString("RENAMED_ITEM_ORIGINAL_NAME")))
                item.SetLocalString("RENAMED_ITEM_ORIGINAL_NAME", item.Name);

            item.Name = newName;

            player.FloatingText("Item renamed to '" + newName + "'.");
            EndConversation();
        }

        private void ResetName()
        {
            var player = GetPC();
            NWItem item = player.GetLocalObject("ITEM_BEING_RENAMED");
            if (string.IsNullOrWhiteSpace(item.GetLocalString("RENAMED_ITEM_ORIGINAL_NAME")))
                item.SetLocalString("RENAMED_ITEM_ORIGINAL_NAME", item.Name);

            item.Name = item.GetLocalString("RENAMED_ITEM_ORIGINAL_NAME");
        }

        public override void EndDialog()
        {
            var player = GetPC();
            player.DeleteLocalObject("ITEM_BEING_RENAMED");
            player.DeleteLocalInt("ITEM_RENAMING_LISTENING");
            player.DeleteLocalString("RENAMED_ITEM_NEW_NAME");
        }
    }
}