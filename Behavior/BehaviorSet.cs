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
	public class BehaviorSet<RealBehaviorType>: IEnumerable<int>
		where RealBehaviorType:IBehavior
	{
		//ItemTree<string, int?> BehaviorsIndex = new ItemTree<string, int?>();
		//List<RealBehaviorType> BehaviorsList = new List<RealBehaviorType>();
		public readonly ItemTreeWithID<string, RealBehaviorType> BehaviorsList = new ItemTreeWithID<string, RealBehaviorType>();
		public RealBehaviorType this[int id] => GetBehavior(id);
		public int this[IndexList<string> index] => GetBehaviorID(index);
		public RealBehaviorType GetBehavior(int id)
		{
			return BehaviorsList[id];
		}
		public RealBehaviorType GetBehavior(IndexList<string> index)
		{
			return BehaviorsList[index];
		}
		public int GetBehaviorID(IndexList<string> index)
		{
			return BehaviorsList.GetID(index);
		}
		/// <summary>
		/// 只枚举正在运行的ModNPCBehavior的ID,无序
		/// </summary>
		public IEnumerator<int> GetEnumerator()
		{
			foreach(var i in BehaviorsList)
			{
				if (!i.Value.Pausing) yield return i.Key;
			}
		}
		/// <summary>
		/// 只枚举正在运行的ModNPCBehavior,无序
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		public int BehaviorNowID;
		public RealBehaviorType BehaviorNow => this[BehaviorNowID];
		public bool ChangeBehavior(int NewBehaviorID)
		{
			RealBehaviorType NewBehavior = GetBehavior(NewBehaviorID);
			if (NewBehavior.Pausing && !NewBehavior.CanContinue()) return false;
			BehaviorNow.Pausing = true;
			if (!BehaviorNow.Pausing) return false;
			NewBehavior.Continue();
			BehaviorNowID = NewBehaviorID;
			return true;
		}
		public int RegisterBehavior(RealBehaviorType behavior, IndexList<string> index) {
			int NewID = BehaviorsList.Count;
			BehaviorsList.Add(behavior);
			return NewID;
		}
		public void NetUpdateSend(BinaryWriter writer) {
			writer.Write(BehaviorNowID);
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
		public void NetUpdateReceive(BinaryReader reader) {
			ChangeBehavior(reader.ReadInt32());
			int Count = reader.ReadInt32();
			for (int i = 0; i < Count; ++i) {
				int id = reader.ReadInt32();
				BehaviorsList[id].NetUpdateReceive(reader);
			}
		}
		public void Update() {
			BehaviorNow.Update();
		}
	}
}
