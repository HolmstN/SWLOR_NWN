using SWLOR.Game.Server.Event.Conversation.Quest.FinishQuest;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    public class fin_qst5_rule6
#pragma warning restore IDE1006 // Naming Styles
    {
        public static int Main()
        {
            return QuestComplete.Check(5, 6) ? 1 : 0;
        }
    }
}
