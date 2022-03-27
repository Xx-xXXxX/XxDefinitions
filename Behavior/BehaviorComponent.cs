using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.ID;

namespace XxDefinitions.Behavior
{
	/// <summary>
	/// 通过组合模式操作behavior
	/// Add应在Initialize前完成，否则应该报错
	/// 否则可能出现联机同步错误
	/// </summary>
	public interface IBehaviorComponent<in RealBehaviorType> : IBehavior
	{
		/// <summary>
		/// 加入成员，应在Initialize前完成
		/// </summary>
		void Add(RealBehaviorType behavior, bool Using);
	}
	/// <summary>
	/// 通过组合模式操作behavior
	/// </summary>
	public abstract class BehaviorComponent<RealBehaviorType> : Behavior, IEnumerable<RealBehaviorType>, IBehaviorComponent<RealBehaviorType>//, IBehaviorComponent<RealBehaviorType>
		where RealBehaviorType : IBehavior
	{
		/// <summary>
		/// 装有Behavior的容器
		/// </summary>
		protected List<RealBehaviorType> BehaviorsList = new List<RealBehaviorType>();
		/// <summary>
		/// 表示Behavior是否正在使用
		/// </summary>
		protected List<bool> BehaviorsUsing = new List<bool>();
		/// <summary>
		/// 下一个可用ID
		/// </summary>
		public int NextID => BehaviorsList.Count;
		/// <summary>
		/// 获取Behavior
		/// </summary>
		public RealBehaviorType this[int id] => BehaviorsList[id];
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public void Add(RealBehaviorType behavior, bool Using = false)
		{
			if (Initialized) throw new InvalidOperationException();
			int id = NextID;
			BehaviorsList.Add(behavior);
			BehaviorsUsing.Add(Using);
			OnAdd(behavior, id);
		}
		/// <summary>
		/// 在加入成员时（已经加入BehaviorsList）
		/// </summary>
		public virtual void OnAdd(RealBehaviorType behavior, int id)
		{

		}
		public bool BehaviorUsing(int id) => BehaviorsUsing[id];
		public bool BehaviorUse(int id)
		{
			BehaviorsUsing[id] = true;
			if (Active) return BehaviorsList[id].TryActivate();
			return false;
		}
		public bool BehaviorUnUse(int id)
		{
			BehaviorsUsing[id] = false;
			if (Active) return BehaviorsList[id].TryPause();
			return false;
		}
		public override void Update()
		{
			base.Update();
			foreach (var i in this)
			{
				i.Update();
			}
		}

		public override bool CanPause()
		{
			bool can = base.CanPause();
			foreach (var i in GetUsings())
			{
				can &= (!i.Active | i.CanPause());
				if (!can) return false;
			}
			return true;
		}

		public override void OnPause()
		{
			foreach (var i in GetUsings())
			{
				i.TryPause();
			}
		}

		public override bool CanActivate()
		{
			bool can = base.CanActivate();
			foreach (var i in GetUsings())
			{
				can &= (i.Active | i.CanActivate());
				if (!can) return false;
			}
			return true;
		}

		public override void OnActivate()
		{
			foreach (var i in GetUsings())
			{
				i.TryActivate();
			}
		}

		public override void OnInitialize()
		{
			foreach (var i in BehaviorsList)
			{
				i.Initialize();
			}
		}

		public override void OnDispose()
		{
			foreach (var i in BehaviorsList)
			{
				i.Dispose();
			}
		}

		public override void NetUpdateSend(BinaryWriter writer)
		{
			base.NetUpdateSend(writer);
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

			writer.Write(BehaviorsList.Count);
			for (int i = 0; i < BehaviorsList.Count; ++i)
			{
				RealBehaviorType behavior = BehaviorsList[i];
				Terraria.BitsByte bits = 0;
				bits[0] = behavior.Active;
				bool NetUpdate = bits[1] = All || behavior.NetUpdate;
				writer.Write(bits);
				if (NetUpdate)
					behavior.NetUpdateSend(writer);
			}
		}

		public override void NetUpdateReceive(BinaryReader reader)
		{
			base.NetUpdateReceive(reader);
			int Count = reader.ReadInt32();
			for (int i = 0; i < Count; ++i)
			{
				int id = i;
				Terraria.BitsByte bits = reader.ReadByte();
				bool active = bits[0];
				bool NetUpdate = bits[1];
				IBehavior behavior = BehaviorsList[id];
				behavior.TrySetActive(active);
				if (NetUpdate)
					behavior.NetUpdateReceive(reader);
			}
		}

		public override object Call(params object[] vs)
		{
			return null;
		}

		public IEnumerator<RealBehaviorType> GetEnumerator()
		{
			foreach (var i in BehaviorsList)
			{
				RealBehaviorType b = i;
				if (b.Active) yield return b;
			}
		}
		public IEnumerable<RealBehaviorType> GetUsings()
		{
			for (int i = 0; i < BehaviorsList.Count; ++i)
			{
				if (BehaviorsUsing[i]) yield return BehaviorsList[i];
			}
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<RealBehaviorType>)this).GetEnumerator();
		}
	}
}
