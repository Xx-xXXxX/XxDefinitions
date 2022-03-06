using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions.Behavior
{
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
	/// <summary>
	/// 行为，用于自动机
	/// </summary>
	public interface IBehavior
	{
		/// <summary>
		/// 是否正在暂停，初始值应为true暂停
		/// 在CanPause时Pausing必须能在Pause后set为true，Continue同理
		/// </summary>
		bool Pausing { get; set; }
		/// <summary>
		/// 它会做什么
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
		/// 它是否能继续,true会
		/// </summary>
		bool CanContinue();
		/// <summary>
		/// 进行继续时
		/// </summary>
		void Continue();
		/// <summary>
		/// 开始(注册)时
		/// </summary>
		void Start();
		/// <summary>
		/// 终止时
		/// </summary>
		void End();
		/// <summary>
		/// 用于联机同步发送，一般不在暂停时使用
		/// </summary>
		/// <param name="writer"></param>
		void NetUpdateSend(BinaryWriter writer);
		/// <summary>
		/// 用于联机同步接收，一般不在暂停时使用
		/// </summary>
		/// <param name="reader"></param>
		void NetUpdateReceive(BinaryReader reader);
		/// <summary>
		/// 用于默认的数据传输
		/// </summary>
		object Call(params object[] vs);
		/// <summary>
		/// 行为的名称
		/// </summary>
		string BehaviorName { get; }
	}
	public class Behavior : IBehavior
	{
		public bool pause = true;
		public bool Pausing { get; set; }
		public virtual string BehaviorName { get; }
		public virtual object Call(params object[] vs) { return null; }
		public virtual bool CanContinue() { return true; }
		public virtual bool CanPause() { return true; }
		public virtual void Continue() { }
		public virtual void End() { }
		public virtual void NetUpdateReceive(BinaryReader reader) { }
		public virtual void NetUpdateSend(BinaryWriter writer) { }
		public virtual void Pause() { }
		public virtual void Start() { }
		public virtual void Update() { }
	}
	public static class BehaviorUtils {

		public static bool TryPause(this IBehavior behavior)
		{
			if (behavior.Pausing) return false;
			if (behavior.CanPause())
			{
				behavior.Pause();
				behavior.Pausing = true;
				return true;
			}
			else return false;
		}
		public static bool TryContinue(this IBehavior behavior)
		{
			if (!behavior.Pausing) return false;
			if (behavior.CanContinue())
			{
				behavior.Continue();
				behavior.Pausing = false;
				return true;
			}
			else return false;
		}
	}
}
