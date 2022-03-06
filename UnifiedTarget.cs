using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Terraria;
namespace XxDefinitions
{
	/// <summary><code><![CDATA[
	/// 用short表示玩家或NPC,或无
	/// 值data
	/// >0:data-1 为NPC的ID
	/// <0:data+1 为Player的ID
	/// ==0:为空
	/// ]]></code>
	/// </summary>
	public struct UnifiedTarget
	{
		public short data;
		public bool IsNPC {
			get => data > 0;
			set { if (data < 0) data = (short)-data; }
		}
		public bool IsPlayer {
			get => data < 0;
			set { if (data > 0) data = (short)-data; }
		}
		public bool IsNull {
			get => data == 0;
			set => data = 0;
		}
		public int NPCID {
			get {
				if (data > 0) return data - 1;
				else return -1;
			}
			set => data = (short)(value + 1);
		}
		public NPC GetNPC()
		{
			int id = NPCID;
			if (id == -1) return null;
			return Main.npc[id];
		}
		public NPC npc {
			get => GetNPC();
			set => Set(value);
		}
		public Player GetPlayer()
		{
			int id = PlayerID;
			if (id == -1) return null;
			return Main.player[id];
		}
		public Player player {
			get => GetPlayer();
			set => Set(value);
		}
		public int PlayerID{
			get {
				if (data < 0) return -(data + 1);
				else return -1;
			}
			set =>data= (short)-(value + 1);
		}
		public UnifiedTarget(short data=0) { this.data = data; }
		public static explicit operator short(UnifiedTarget A) { return A.data; }
		public static explicit operator UnifiedTarget(short A) { return new UnifiedTarget(A); }
		public static explicit operator UnifiedTarget(int A) { return new UnifiedTarget((short)A); }
		//public void WriteBinary(BinaryWriter writer) { writer.Write(data); }
		//public void ReadBinary(BinaryReader reader) { data = reader.ReadInt16(); }
		public static void WriteBinary(BinaryWriter writer, UnifiedTarget i) { writer.Write(i.data); }
		public static UnifiedTarget ReadBinary(BinaryReader reader) {
			return new UnifiedTarget(reader.ReadInt16());
		}
		public static UnifiedTarget MakeNPCID(int NPCID) {
			return new UnifiedTarget() { NPCID = NPCID };
		}
		public static UnifiedTarget MakePlayerID(int PlayerID)
		{
			return new UnifiedTarget() { PlayerID = PlayerID };
		}
		public void Set(NPC npc) { NPCID = npc.whoAmI;}
		public void Set(Player player) { PlayerID = player.whoAmI; }
		public int NPCTarget {
			get {
				if (IsNPC) return NPCID + 300;
				else if (IsPlayer) return PlayerID;
				else return -1;
			}
			set {
				if (value >= 300 && value < 500) NPCID = value - 300;
				else if (value >= 0 && value <= 255) PlayerID = value;
				else IsNull = true;
			}
		}
	}
}
