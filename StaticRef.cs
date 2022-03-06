using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{

	//public static class StaticRefHolder
	//{
	//	internal static LinkedList<object> sRitems;
	//	public static LinkedList<object> SRitems
	//	{
	//		get
	//		{
	//			if (sRitems == null) sRitems = new LinkedList<object>();
	//			return sRitems;
	//		}
	//	}
	//	internal static LinkedList<Action> sRs;
	//	public static LinkedList<Action> SRs
	//	{
	//		get
	//		{
	//			if (sRs == null) sRs = new LinkedList<Action>();
	//			return sRs;
	//		}
	//	}
	//	internal static bool Loaded = false;
	//	public static void Load() {
	//		Loaded = true;
	//	}
	//	public static void Unload()
	//	{

	//		XxDefinitions.Log.Debug($"StaticRefHolder Call Unload {Loaded}");
	//		if (Loaded)
	//		{
	//			Loaded = false;
	//			XxDefinitions.Log.Debug("StaticRefHolder Unload");
	//			sRitems = null;
	//			foreach (var i in SRs)
	//			{
	//				i?.Invoke();
	//			}
	//			sRs = null;

	//			XxDefinitions.Log.Debug("StaticRefHolder pre GC.Collect()");
	//			GC.Collect(); GC.WaitForPendingFinalizers();
	//			XxDefinitions.Log.Debug("StaticRefHolder post GC.Collect()");
	//		}
	//	}
	//}
	//public struct StaticRef<T>
	//	where T : class,new()
	//{
	//	private WeakReference<T> item;
	//	public T CreateItem1() {
	//		T V = new T();
	//		item = new WeakReference<T>(V);
	//		StaticRefHolder.SRitems.AddLast(V);
	//		XxDefinitions.Log.Debug($"StaticRef{this.GetType().FullName} CreateItem1{V}");
	//		StaticRefHolder.SRs.AddLast(Unload);
	//		return V;
	//	}
	//	public T CreateItem2() {
	//		T J = new T();
	//		item.SetTarget(J);
	//		StaticRefHolder.SRitems.AddLast(J);
	//		XxDefinitions.Log.Debug($"StaticRef{this.GetType().FullName} CreateItem2{J}");
	//		StaticRefHolder.SRs.AddLast(Unload);
	//		return J;
	//	}
	//	public bool ItemAvaliable() {
	//		if (item == null) return false;
	//		if (!item.TryGetTarget(out var I)) return false;
	//		return true;
	//	}
	//	public T CheckItem() {
	//		if (item == null) return CreateItem1();
	//		if (item.TryGetTarget(out var I)) { return I; }
	//		else
	//		{
	//			return CreateItem2();
	//		}
	//	}
	//	public StaticRef(bool CreateNow=false)
	//	{
	//		if (CreateNow)
	//		{
	//			T V = new T();
	//			item = new WeakReference<T>(V);
	//			StaticRefHolder.SRitems.AddLast(V);
	//			StaticRefHolder.SRs.AddLast(Unload);
	//		}
	//		else item = null;
	//		XxDefinitions.Log.Debug($"StaticRef{this.GetType().FullName} ctor {((Action)(Unload)).Method}");
	//	}
	//	public T Value
	//	{
	//		get => CheckItem();
	//	}
	//	public static implicit operator T(StaticRef<T> I) => I.Value;
	//	public void Unload()
	//	{
	//		item = null;
	//		XxDefinitions.Log.Debug($"StaticRef{this.GetType().FullName} Run Unload");
	//	}
	//}
	//public abstract class CtorByF<T>
	//	where T : class
	//{
	//	private T item;
	//	public abstract T F();
	//	public CtorByF()
	//	{
	//		item = F();
	//		XxDefinitions.Log.Debug($"CtorByF{this.GetType().FullName} ctor {item}");
	//	}
	//	public T Value
	//	{
	//		get => item;
	//		set => item = Value;
	//	}
	//	public static implicit operator T(CtorByF<T> I) => I.Value;
	//	~CtorByF() {
	//		XxDefinitions.Log.Debug($"CtorByF{this.GetType().FullName} ~");
	//		item = null;
	//	}
	//}


#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	[Obsolete("未完成")]
	public static class UnloadDo
	{
		internal static LinkedList<Action> unloadActions;
		public static LinkedList<Action> UnloadActions
		{
			get
			{
				if (unloadActions == null) unloadActions = new LinkedList<Action>();
				return unloadActions;
			}
		}
		internal static bool Loaded = false;
		public static void Load()
		{
			Loaded = true;
		}
		public static void Add(Action action)
		{
			UnloadActions.AddLast(action);
		}
		public static void Unload()
		{
			XxDefinitions.Logv1.Debug($"StaticRefHolder Call Unload {Loaded}");
			if (Loaded)
			{
				Loaded = false;
				XxDefinitions.Logv1.Debug("StaticRefHolder Unload");
				foreach (var i in UnloadActions)
				{
					i?.Invoke();
				}
				unloadActions = null;

			}
		}
	}
	///<summary><![CDATA[ 一次失败的尝试，试图创建一个可以在Unload时自动解除对class的引用的struct
	///目标:]]>
	///<code>
	///<![CDATA[static StaticRef<T> A=new StaticRef<T>( new T(...));]]>
	///</code>
	///<param>在Unload时A的对象引用会被设成null，从而释放对象</param>
	///<param>因为各种原因失败</param></summary>
	[Obsolete("未完成")]
	public struct StaticRef<T>
		where T : class
	{
		private static int GIDs = 0;
		private int GID;
		private bool SetUnload;
		private T item;
		public bool ItemAvaliable()
		{
			if (item == null) return false;
			return true;
		}
		public StaticRef(T I)
		{
			GID = GIDs++;
			item = I;
			SetUnload = true;
			UnloadDo.Add(Unload);
			XxDefinitions.Logv1.Debug($"StaticRef {this.GID} {this.GetType().FullName} ctor");
		}
		public StaticRef(StaticRef<T> I)
		{
			GID = GIDs++;
			item = I.item;
			SetUnload = false;
			XxDefinitions.Logv1.Debug($"StaticRef {this.GID} {this.GetType().FullName} copy from {I.GID} {I.GetType().FullName}");

		}
		public T Value
		{
			get => item;
			set
			{
				item = value;
				if (!SetUnload)
				{
					SetUnload = true;
					UnloadDo.Add(Unload);
				}
			}
		}
		public void Unload()
		{
			item = null;
			SetUnload = false;
			XxDefinitions.Logv1.Debug($"StaticRef {this.GID} {this.GetType().FullName} Run Unload");
		}
	}

}
