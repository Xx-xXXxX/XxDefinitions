using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XxDefinitions.NPCs.NPCBehaviors;
using System.IO;
using Terraria.ID;
using XxDefinitions.Behavior;

namespace XxDefinitions.Behavior
{
	/// <summary>
	/// 用于操作行为集的类
	/// </summary>
	public class BehaviorSet<RealBehaviorType>: IEnumerable<int>
		where RealBehaviorType:IBehavior
	{
		//ItemTree<string, int?> BehaviorsIndex = new ItemTree<string, int?>();
		//List<RealBehaviorType> BehaviorsList = new List<RealBehaviorType>();
		/// <summary>
		/// 装行为的容器
		/// </summary>
		public readonly ListWithIDandIndex<string, RealBehaviorType> BehaviorsList = new ListWithIDandIndex<string, RealBehaviorType>();
		/// <summary>
		/// 用id获取行为
		/// </summary>
		public RealBehaviorType this[int id] => GetBehavior(id);
		/// <summary>
		/// 用索引获取行为
		/// </summary>
		public RealBehaviorType this[IndexList<string> index] => GetBehavior(index);
		/// <summary>
		/// 用索引获取id
		/// </summary>
		public int GetID(IndexList<string> index) {
			return BehaviorsList.GetID(index);
		}
		/// <summary>
		/// 用id获取行为
		/// </summary>
		public RealBehaviorType GetBehavior(int id)
		{
			return BehaviorsList[id];
		}
		/// <summary>
		/// 用索引获取行为
		/// </summary>
		public RealBehaviorType GetBehavior(IndexList<string> index)
		{
			return BehaviorsList[index];
		}
		/// <summary>
		/// 用索引获取行为的id
		/// </summary>
		public int GetBehaviorID(IndexList<string> index)
		{
			return BehaviorsList.GetID(index);
		}
		/// <summary>
		/// 只枚举正在运行的ModNPCBehavior的ID,按注册顺序
		/// </summary>
		public IEnumerator<int> GetEnumerator()
		{
			foreach(var i in BehaviorsList)
			{
				if (i.Value.Active) yield return i.Key;
			}
		}
		/// <summary>
		/// 只枚举正在运行的ModNPCBehavior,按注册顺序
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		private int behaviorMainID=-1;
		/// <summary>
		/// 正在进行的主要的行为的id
		/// </summary>
		public int BehaviorMainID=> behaviorMainID;
		/// <summary>
		/// 正在进行的主要的行为
		/// </summary>
		public RealBehaviorType BehaviorMain => this[behaviorMainID];
		/// <summary>
		/// 改变主要行为或设置初始主要行为
		/// </summary>
		public bool SetBehaviorMain(int NewBehaviorID)
		{
			RealBehaviorType NewBehavior = GetBehavior(NewBehaviorID);
			if (!NewBehavior.Active && NewBehavior.CanActivate()) return false;
			if (behaviorMainID != -1)
			{
				if (!BehaviorMain.CanPause()) return false;
				BehaviorMain.TryPause();
			}
			NewBehavior.TryActivate();
			behaviorMainID = NewBehaviorID;
			return true;
		}
		/// <summary>
		/// 注册行为
		/// </summary>
		public int RegisterBehavior(RealBehaviorType behavior, IndexList<string> index) {
			int NewID = BehaviorsList.NextID;
			index.AddBack(behavior.BehaviorName);
			BehaviorsList.Add(behavior,index);
			index.RemoveBack();
			behavior.Initialize();
			return NewID;
		}
		///// <summary>
		///// 设置初始的BehaviorMain
		///// </summary>
		//public void SetBehaviorMain(int id) {
		//	if (behaviorMainID == -1) behaviorMainID = id;
		//	throw new InvalidOperationException($"不能重复设置BehaviorMain 目前BehaviorMain ID:{behaviorMainID} Index:{BehaviorsList.GetIndex(behaviorMainID)}");
		//}
		///// <summary>
		///// 设置初始的BehaviorMain
		///// </summary>
		//public void SetBehaviorMain(IndexList<string> index) => SetBehaviorMain(GetID(index));
		/// <summary>
		/// 联机同步发送
		/// </summary>
		public void NetUpdateSend(BinaryWriter writer) {
			bool All = false;
			if (Terraria.Main.netMode == NetmodeID.Server)
			{
				foreach (var i in Terraria.Netplay.Clients)
				{
					if (i != null && i.IsActive && i.State == 3)
					{
						All = true; break;//存在需要同步的端
					}
				}
			}
			List<int> enumed = new List<int>();
			foreach (var i in BehaviorsList)
			{
				enumed.Add(i.Key);
			}
			writer.Write(enumed.Count);
			foreach (var i in enumed)
			{
				writer.Write(i);
				IBehavior behavior = BehaviorsList[i];
				Terraria.BitsByte bits = (byte)0;
				bits[0] = behavior.Active;
				bool NetUpdate = bits[1] = All || behavior.NetUpdate;
				writer.Write(bits);
				if (NetUpdate)
					behavior.NetUpdateSend(writer);
			}
		}
		/// <summary>
		/// 联机同步接收
		/// </summary>
		public void NetUpdateReceive(BinaryReader reader) {
			int Count = reader.ReadInt32();
			for (int i = 0; i < Count; ++i)
			{
				int id = reader.ReadInt32();
				Terraria.BitsByte bits = reader.ReadByte();
				bool active = bits[0];
				bool NetUpdate = bits[1];
				IBehavior behavior = BehaviorsList[id];
				behavior.TrySetActive(active);
				if (NetUpdate)
					behavior.NetUpdateReceive(reader);
			}
		}
		/// <summary>
		/// 先执行主要的行为，在执行全部进行的行为
		/// </summary>
		public void Update() {
			if(BehaviorMainID!=-1)
			BehaviorMain.Update();
			foreach (var i in this) {
				if (i != BehaviorMainID) GetBehavior(i).Update();
			}
		}
		/// <summary>
		/// 释放
		/// </summary>
		public void Dispose() {
			foreach (var i in BehaviorsList) i.Value.Dispose();
		}
	}
}
