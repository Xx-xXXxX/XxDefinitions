using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace XxDefinitions.XDebugger.Commands
{
	public class ShowNPCData:ModCommand
	{
		public override CommandType Type
			=> CommandType.World;

		public override string Command
			=> "ShowNPCData";

		public override string Usage
			=> "/ShowNPCData BuffTime";

		public override string Description
			=> "Add Buff ShowDataBuff On All NPC";
		public override void Action(CommandCaller caller, string input, string[] args)
		{
			int time = 1000;
			if (args.Length != 0) time = int.Parse(args[0]);
			//throw new NotImplementedException();
			foreach (var npc in Main.npc) {
				if(npc.active)
					npc.AddBuff(ModContent.BuffType< Buffs.ShowDataBuff>(), time);
			}
		}
	}
}
