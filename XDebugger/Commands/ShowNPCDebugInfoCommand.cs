using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace XxDefinitions.XDebugger.Commands
{
	public class ShowNPCDebugInfoCommand:ModCommand
	{
		public override CommandType Type => CommandType.Chat;
		public override string Command => "ShowNPCDebugInfo";
		public override string Description => "ShowNPCDebugInfo.DrawTimeLeft=time";
		public override string Usage => "/ShowNPCDebugInfo ([time]=300)|On|Off\n";
		public override void Action(CommandCaller caller, string input, string[] args)
		{

			int time;
			if (args.Length > 0)
			{
				if (args[0] == "On")
				{
					ShowNPCDebugInfo.ShowAlways = true;
					return;
				}
				else if (args[0] == "Off") {
					ShowNPCDebugInfo.ShowAlways = false;
					return;
				}
				else
				if (!int.TryParse(args[0], out time))
				{
					time = 300;
				}
			}
			else time = 300;
			action(time);
		}
		public static void action(int time) {
			int c = 0;
			foreach (var i in Main.npc)
			{
				if (i.active)
				{
					i.GetGlobalNPC<ShowNPCDebugInfo>().DrawTimeLeft = time;
					c += 1;
				}
			}
		}
	}
}
