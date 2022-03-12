using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XxDefinitions.NPCs.NPCBehaviors;
using System.IO;
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
		public readonly ListWithID_Index<string, RealBehaviorType> BehaviorsList = new ListWithID_Index<string, RealBehaviorType>();
		/// <summary>
		/// 用id获取行为
		/// </summary>
		public RealBehaviorType this[int id] => GetBehavior(id);
		/// <summary>
		/// 用索引获取行为
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public RealBehaviorType this[IndexList<string> index] => GetBehavior(index);
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
				if (!i.Value.Pausing) yield return i.Key;
			}
		}
		/// <summary>
		/// 只枚举正在运行的ModNPCBehavior,按注册顺序
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		/// <summary>
		/// 正在进行的主要的行为的id
		/// </summary>
		public int BehaviorMainID;
		/// <summary>
		/// 正在进行的主要的行为
		/// </summary>
		public RealBehaviorType BehaviorMain => this[BehaviorMainID];
		/// <summary>
		/// 改变主要行为
		/// </summary>
		public bool ChangeBehavior(int NewBehaviorID)
		{
			RealBehaviorType NewBehavior = GetBehavior(NewBehaviorID);
			if (NewBehavior.Pausing && !NewBehavior.CanContinue()) return false;
			BehaviorMain.Pausing = true;
			if (!BehaviorMain.Pausing) return false;
			NewBehavior.Continue();
			BehaviorMainID = NewBehaviorID;
			return true;
		}
		/// <summary>
		/// 注册行为
		/// </summary>
		public int RegisterBehavior(RealBehaviorType behavior, IndexList<string> index) {
			int NewID = BehaviorsList.NextID;
			BehaviorsList.Add(behavior);
			return NewID;
		}
		/// <summary>
		/// 联机同步发送
		/// </summary>
		public void NetUpdateSend(BinaryWriter writer) {
			writer.Write(BehaviorMainID);
			List<int> enumed = new List<int>();
			foreach (var i in this)
			{
				//writer.Write(i);
				//BehaviorsList[i].NetUpdateSend(writer);
				enumed.Add(i);
			}
			writer.Write(enumed.Count);
			foreach (var i in enumed)
			{
				writer.Write(i);
				BehaviorsList[i].NetUpdateSend(writer);
			}
		}
		/// <summary>
		/// 联机同步接收
		/// </summary>
		public void NetUpdateReceive(BinaryReader reader) {
			ChangeBehavior(reader.ReadInt32());
			int Count = reader.ReadInt32();
			for (int i = 0; i < Count; ++i) {
				int id = reader.ReadInt32();
				BehaviorsList[id].NetUpdateReceive(reader);
			}
		}
		/// <summary>
		/// 先执行主要的行为，在执行全部进行的行为
		/// </summary>
		public void Update() {
			BehaviorMain.Update();
			foreach (var i in this) {
				if (i != BehaviorMainID) GetBehavior(i).Update();
			}
		}
	}
}
