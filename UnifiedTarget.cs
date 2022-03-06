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
	/// [1,256] player的ID-1
	/// >301 npc的ID+301
	/// 0 null
	/// ]]></code>
	/// </summary>
	public struct UnifiedTarget
	{
		/// <summary>
		/// 目标的值
		/// </summary>
		public short data;
		/// <summary>
		/// 目标是否为npc
		/// </summary>
		public bool IsNPC {
			get => data>=301;
		}
		/// <summary>
		/// 目标是否为player
		/// </summary>
		public bool IsPlayer {
			get => data>=1&&data<=256;
		}
		/// <summary>
		/// 目标是否为空
		/// </summary>
		public bool IsNull {
			get => data == 0;
			set => data = 0;
		}
		/// <summary>
		/// 所表示的npc的id
		/// </summary>
		public int NPCID {
			get {
				if (IsNPC) return data - 301;
				else return -1;
			}
			set => data = (short)(value + 301);
		}
		/// <summary>
		/// 获取所表示的NPC
		/// </summary>
		public NPC GetNPC()
		{
			int id = NPCID;
			if (id == -1) return null;
			return Main.npc[id];
		}
		/// <summary>
		/// 所表示的NPC
		/// </summary>
		public NPC npc {
			get => GetNPC();
			set => Set(value);
		}
		/// <summary>
		/// 获取所表示的player
		/// </summary>
		public Player GetPlayer()
		{
			int id = PlayerID;
			if (id == -1) return null;
			return Main.player[id];
		}
		/// <summary>
		/// 所表示的player
		/// </summary>
		public Player player {
			get => GetPlayer();
			set => Set(value);
		}
		/// <summary>
		/// 所表示的player的id
		/// </summary>
		public int PlayerID{
			get {
				if (IsPlayer) return data+1;
				else return -1;
			}
			set =>data=(short)(value+1);
		}
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public UnifiedTarget(short data=0) { this.data = data; }
		public static explicit operator short(UnifiedTarget A) { return A.data; }
		public static explicit operator UnifiedTarget(short A) { return new UnifiedTarget(A); }
		public static explicit operator UnifiedTarget(int A) { return new UnifiedTarget((short)A); }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		//public void WriteBinary(BinaryWriter writer) { writer.Write(data); }
		//public void ReadBinary(BinaryReader reader) { data = reader.ReadInt16(); }
		/// <summary>
		/// 将UnifiedTarget写入流
		/// </summary>
		public static void WriteBinary(BinaryWriter writer, UnifiedTarget i) { writer.Write(i.data); }
		/// <summary>
		/// 从流读取UnifiedTarget
		/// </summary>
		public static UnifiedTarget ReadBinary(BinaryReader reader) {
			return new UnifiedTarget(reader.ReadInt16());
		}
		/// <summary>
		/// 生成对应NPC的id的UnifiedTarget
		/// </summary>
		public static UnifiedTarget MakeNPCID(int NPCID) {
			return new UnifiedTarget() { NPCID = NPCID };
		}
		/// <summary>
		/// 生成对应player的id的UnifiedTarget
		/// </summary>
		public static UnifiedTarget MakePlayerID(int PlayerID)
		{
			return new UnifiedTarget() { PlayerID = PlayerID };
		}
		/// <summary>
		/// 设置表示npc
		/// </summary>
		public void Set(NPC npc) { NPCID = npc.whoAmI; }
		/// <summary>
		/// 设置表示player
		/// </summary>
		public void Set(Player player) { PlayerID = player.whoAmI; }
		/// <summary>
		/// 与npc.target对应
		/// </summary>
		public int NPCTarget {
			get {
				return data - 1;
			}
			set {
				data = (short)(value + 1);
			}
		}
	}


	/// <summary><code><![CDATA[
	/// 用short表示玩家或NPC,或无
	/// 值data
	/// >0:data-1 为NPC的ID
	/// <0:data+1 为Player的ID
	/// ==0:为空
	/// ]]></code>
	/// </summary>
	public struct UnifiedTarget2
	{
		/// <summary>
		/// 目标的值
		/// </summary>
		public short data;
		/// <summary>
		/// 目标是否为npc
		/// </summary>
		public bool IsNPC
		{
			get => data > 0;
			set { if (data < 0) data = (short)-data; }
		}
		public bool IsPlayer
		{
			get => data < 0;
			set { if (data > 0) data = (short)-data; }
		}
		public bool IsNull
		{
			get => data == 0;
			set => data = 0;
		}
		public int NPCID
		{
			get
			{
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
		public NPC npc
		{
			get => GetNPC();
			set => Set(value);
		}
		public Player GetPlayer()
		{
			int id = PlayerID;
			if (id == -1) return null;
			return Main.player[id];
		}
		public Player player
		{
			get => GetPlayer();
			set => Set(value);
		}
		public int PlayerID
		{
			get
			{
				if (data < 0) return -(data + 1);
				else return -1;
			}
			set => data = (short)-(value + 1);
		}
		public UnifiedTarget2(short data = 0) { this.data = data; }
		public static explicit operator short(UnifiedTarget2 A) { return A.data; }
		public static explicit operator UnifiedTarget2(short A) { return new UnifiedTarget2(A); }
		public static explicit operator UnifiedTarget2(int A) { return new UnifiedTarget2((short)A); }
		//public void WriteBinary(BinaryWriter writer) { writer.Write(data); }
		//public void ReadBinary(BinaryReader reader) { data = reader.ReadInt16(); }
		public static void WriteBinary(BinaryWriter writer, UnifiedTarget i) { writer.Write(i.data); }
		public static UnifiedTarget ReadBinary(BinaryReader reader)
		{
			return new UnifiedTarget(reader.ReadInt16());
		}
		public static UnifiedTarget MakeNPCID(int NPCID)
		{
			return new UnifiedTarget() { NPCID = NPCID };
		}
		public static UnifiedTarget MakePlayerID(int PlayerID)
		{
			return new UnifiedTarget() { PlayerID = PlayerID };
		}
		public void Set(NPC npc) { NPCID = npc.whoAmI; }
		public void Set(Player player) { PlayerID = player.whoAmI; }
		public int NPCTarget
		{
			get
			{
				if (IsNPC) return NPCID + 300;
				else if (IsPlayer) return PlayerID;
				else return -1;
			}
			set
			{
				if (value >= 300 && value < 500) NPCID = value - 300;
				else if (value >= 0 && value <= 255) PlayerID = value;
				else IsNull = true;
			}
		}
	}
}
