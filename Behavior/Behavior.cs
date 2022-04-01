using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions.Behavior
{
	#region
	/*
	public class BehaviorControler<ExType>: IBehavior
		where ExType: IBehavior
	{
		public ExType item;
		public bool pausing;
		/// <summary>
		/// 尝试暂停
		/// </summary>
		/// <returns>true代表操纵成功，!item.CanPause()与pausing都返回false</returns>
		public bool TryPause() {
			if (pausing) return false;
			if (item.CanPause())
			{
				item.Pause();
				pausing = true;
				return true;
			}
			else return false;
		}
		/// <summary>
		/// 尝试继续
		/// </summary>
		/// <returns>true代表操纵成功，!item.CanContinue()与!pausing都返回false</returns>
		public bool TryContinue() {
			if (!pausing) return false;
			if (item.CanContinue())
			{
				item.Continue();
				pausing = false;
				return true;
			}
			else return false;
		}
		public bool Pausing {
			get => pausing;
			set {
				if (value) TryPause();
				else TryContinue();
			}
		}
		public void Update() { if (!Pausing) item.Update(); }
		public object Call(params object[] vs)=>item.Call(vs);
		public string BehaviorName => item.BehaviorName;
		public void Start() => item.Start(); 
		public void End()=>item.End(); 
		public bool CanPause() => !pausing;
		public bool CanContinue() => pausing;
		public void Pause() => TryPause();
		public void Continue() => TryContinue();
		public void NetUpdateSend(BinaryWriter writer) => item.NetUpdateSend(writer);
		public void NetUpdateReceive(BinaryReader reader) => item.NetUpdateReceive(reader);
	}*/
	#endregion
	/// <summary>
	/// 行为，用于自动机
	/// </summary>
	public interface IBehavior
	{
		/// <summary>
		/// 是否正在暂停，初始值应为true暂停
		/// 在CanPause()时Pausing必须能在Pause()后set为true，Continue同理
		/// 不要在set_Active中套用TryPause等
		/// </summary>
		bool Active { get;}
		/// <summary>
		/// 执行
		/// </summary>
		void Update();
		/// <summary>
		/// 它是否能暂停,true会
		/// </summary>
		bool CanPause();
		/// <summary>
		/// 进行暂停时
		/// </summary>
		void Pause();
		/// <summary>
		/// 它能否激活,true会
		/// </summary>
		bool CanActivate();
		/// <summary>
		/// 进行激活时
		/// </summary>
		void Activate();
		/// <summary>
		/// 开始(注册)时
		/// </summary>
		void Initialize();
		/// <summary>
		/// 终止时
		/// 因为某些原因，不保证执行
		/// </summary>
		void Dispose();
		/// <summary>
		/// 用于联机同步发送，如果需要完整同步(Server存在需要同步世界的客户端)会全部使用，否则在NetUpdate时使用
		/// </summary>
		/// <param name="writer"></param>
		void NetUpdateSend(BinaryWriter writer);
		/// <summary>
		/// 用于联机同步接收，如果需要完整同步(Server存在需要同步世界的客户端)会全部使用，否则在NetUpdate时使用
		/// </summary>
		/// <param name="reader"></param>
		void NetUpdateReceive(BinaryReader reader);
		/// <summary>
		/// 用于通用的数据传输
		/// </summary>
		object Call(params object[] vs);
		/// <summary>
		/// 行为的名称
		/// </summary>
		string BehaviorName { get; }
		/// <summary>
		/// 是否进行同步
		/// </summary>
		bool NetUpdate { get; }
	}
	/// <summary>
	/// 行为的基类
	/// </summary>

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public abstract class Behavior : IBehavior
	{
		private bool initialized = false;
		public bool Initialized => initialized;
		private bool active = false;
		public virtual bool Active { get=>active;}
		public virtual string BehaviorName { get { return this.GetType().FullName; } }
		public virtual object Call(params object[] vs) { return null; }
		public virtual bool CanActivate() { return !Active; }
		public virtual bool CanPause() { return Active; }
		public void Dispose() { initialized = false; }
		public virtual void OnDispose() { }
		public void Activate() { if (!initialized) Initialize(); active = true; OnActivate(); }
		public virtual void OnActivate() { }
		public void Pause() { active = false; OnPause(); }
		public virtual void OnPause() { }
		public void Initialize() { initialized = true; OnInitialize(); }
		public virtual void OnInitialize() { }
		public virtual void Update() { }
		public abstract bool NetUpdate { get; }
		public virtual void NetUpdateReceive(BinaryReader reader) { }
		public virtual void NetUpdateSend(BinaryWriter writer) { }
	}
	public static class BehaviorUtils {
		/// <summary>
		/// 尝试暂停
		/// </summary>
		/// <returns>如果执行了Pause返回true</returns>
		public static bool TryPause(this IBehavior behavior)
		{
			if (!behavior.Active) return false;
			if (behavior.CanPause())
			{
				behavior.Pause();
				return true;
			}
			else return false;
		}
		/// <summary>
		/// 尝试激活
		/// </summary>
		/// <returns>如果执行了Activate返回true</returns>
		public static bool TryActivate(this IBehavior behavior)
		{
			if (behavior.Active) return false;
			if (behavior.CanActivate())
			{
				behavior.Activate();
				return true;
			}
			else return false;
		}
		public static bool TrySetActive(this IBehavior behavior, bool active) {
			if (active) return behavior.TryActivate();
			else return behavior.TryPause();
		}
	}
}
