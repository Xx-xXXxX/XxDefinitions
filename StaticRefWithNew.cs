using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{

	internal static class StaticRefHolder
	{
		internal static LinkedList<object> sRitems;
		public static LinkedList<object> SRitems
		{
			get
			{
				if (sRitems == null) sRitems = new LinkedList<object>();
				return sRitems;
			}
		}
		internal static LinkedList<Action> sRs;
		public static LinkedList<Action> SRs
		{
			get
			{
				if (sRs == null) sRs = new LinkedList<Action>();
				return sRs;
			}
		}
		internal static bool Loaded = false;
		public static void Load()
		{
			Loaded = true;
		}
		public static void Unload()
		{

			XxDefinitions.Logv1.Debug($"StaticRefHolder Call Unload {Loaded}");
			if (Loaded)
			{
				Loaded = false;
				XxDefinitions.Logv1.Debug("StaticRefHolder Unload");
				sRitems = null;
				foreach (var i in SRs)
				{
					i?.Invoke();
				}
				sRs = null;

				XxDefinitions.Logv1.Debug("StaticRefHolder pre GC.Collect()");
				GC.Collect(); GC.WaitForPendingFinalizers();
				XxDefinitions.Logv1.Debug("StaticRefHolder post GC.Collect()");
			}
		}
	}
	#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	/// <summary><![CDATA[
	/// 通过一个弱引用和一个列表引用完成可以在XxDefinitions.Unload自动释放的引用
	/// 通过new T()创建对象
	/// 在内存中剩下一个WeakReference<T>]]>
	/// </summary>
	public struct StaticRefWithNew<T> : IGetValue<T>
		where T : class, new()
	{
		private WeakReference<T> item;
	
		public T CreateItem1()
		{
			T V = new T();
			item = new WeakReference<T>(V);
			StaticRefHolder.SRitems.AddLast(V);
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} CreateItem1{V}");
			StaticRefHolder.SRs.AddLast(Unload);
			return V;
		}
		public T CreateItem2()
		{
			T J = new T();
			item.SetTarget(J);
			StaticRefHolder.SRitems.AddLast(J);
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} CreateItem2{J}");
			StaticRefHolder.SRs.AddLast(Unload);
			return J;
		}
		public bool ItemAvaliable()
		{
			if (item == null) return false;
			if (!item.TryGetTarget(out var I)) return false;
			return true;
		}
		public T CheckItem()
		{
			if (item == null) return CreateItem1();
			if (item.TryGetTarget(out var I)) { return I; }
			else
			{
				return CreateItem2();
			}
		}
		public StaticRefWithNew(bool CreateNow = false)
		{
			if (CreateNow)
			{
				T V = new T();
				item = new WeakReference<T>(V);
				StaticRefHolder.SRitems.AddLast(V);
				StaticRefHolder.SRs.AddLast(Unload);
			}
			else item = null;
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} ctor {((Action)(Unload)).Method}");
		}
		public T Value
		{
			get => CheckItem();
		}
		public static implicit operator T(StaticRefWithNew<T> I) => I.Value;
		public void Unload()
		{
			item.SetTarget(null);
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} Run Unload");
		}

	}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	/// <summary>
	/// 在使用时自动用F()创建对象
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class CtorByF<T> : IGetValue<T>
		where T : class
	{
		private T item;
		public abstract T F();
		public CtorByF()
		{
			item = F();
			XxDefinitions.Logv1.Debug($"CtorByF{this.GetType().FullName} ctor {item}");
		}
		public T Value
		{
			get => item;
			set => item = Value;
		}
		public static implicit operator T(CtorByF<T> I) => I.Value;
		~CtorByF()
		{
			XxDefinitions.Logv1.Debug($"CtorByF{this.GetType().FullName} ~");
			item = null;
		}
	}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释


#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	/// <summary><![CDATA[
	/// 通过一个弱引用和一个列表引用完成可以在XxDefinitions.Unload自动释放的引用
	/// 通过CtorF创建对象
	/// 在内存中剩下一个WeakReference<T>和委派构造函数]]>
	/// </summary>
	public struct StaticRefWithFunc<T>:IGetValue<T>
		where T : class
	{
		private WeakReference<T> item;
		public readonly Func<T> CtorF;
		public T CreateItem1()
		{
			T V = CtorF();
			item = new WeakReference<T>(V);
			StaticRefHolder.SRitems.AddLast(V);
			XxDefinitions.Logv1.Debug($"StaticRefWithFunc{this.GetType().FullName} CreateItem1{V}");
			StaticRefHolder.SRs.AddLast(Unload);
			return V;
		}
		public T CreateItem2()
		{
			T J = CtorF();
			item.SetTarget(J);
			StaticRefHolder.SRitems.AddLast(J);
			XxDefinitions.Logv1.Debug($"StaticRefWithFunc{this.GetType().FullName} CreateItem2{J}");
			StaticRefHolder.SRs.AddLast(Unload);
			return J;
		}
		public bool ItemAvaliable()
		{
			if (item == null) return false;
			if (!item.TryGetTarget(out var I)) return false;
			return true;
		}
		public T CheckItem()
		{
			if (item == null) return CreateItem1();
			if (item.TryGetTarget(out var I)) { return I; }
			else
			{
				return CreateItem2();
			}
		}
		public StaticRefWithFunc(Func<T> I)
		{
			CtorF = I;
			item = null;
			XxDefinitions.Logv1.Debug($"StaticRefWithFunc{this.GetType().FullName} ctor {((Action)(Unload)).Method}");
		}
		public T Value
		{
			get => CheckItem();
		}
		public static explicit operator T(StaticRefWithFunc<T> I) => I.Value;
		public void Unload()
		{
			item.SetTarget(null);
			XxDefinitions.Logv1.Debug($"StaticRefWithFunc{this.GetType().FullName} Run Unload");
		}

	}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public class MemoryCheck {
		public string name = "def";
		public MemoryCheck() {
			XxDefinitions.Logv1.Debug($"{this.GetType().FullName} {name} ctor");
		}
		public MemoryCheck(string Name)
		{
			name = Name;
			XxDefinitions.Logv1.Debug($"{this.GetType().FullName} {name} ctor");
		}
		~ MemoryCheck()
		{
			XxDefinitions.Logv1.Debug($"{this.GetType().FullName} {name} ~");
		}
	}
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
