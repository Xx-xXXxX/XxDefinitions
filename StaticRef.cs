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
	public struct StaticRefWithNew<T> :IGetValue<T>, ISetValue<T>, IGetSetValue<T>
		where T : class, new()
	{
		private WeakReference<T> itemWR;
		private WeakReference<LinkedListNode<object>> itemNodeWR;
		private void SetItemNode(LinkedListNode<object> Node) {
			if (itemNodeWR == null) itemNodeWR = new WeakReference<LinkedListNode<object>>(Node);
			else {
				if (itemNodeWR.TryGetTarget(out LinkedListNode<object> ON)) {
					StaticRefHolder.sRitems.Remove(ON);
				}
				itemNodeWR.SetTarget(Node);
			}
		}
		private T CreateItem1()
		{
			T V = new T();
			itemWR = new WeakReference<T>(V);
			SetItemNode(StaticRefHolder.SRitems.AddLast(V));
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} CreateItem1 {V}");
			StaticRefHolder.SRs.AddLast(Unload);
			return V;
		}
		private T CreateItem2()
		{
			T J = new T();
			itemWR.SetTarget(J);
			SetItemNode(StaticRefHolder.SRitems.AddLast(J));
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} CreateItem2 {J}");
			StaticRefHolder.SRs.AddLast(Unload);
			return J;
		}
		public bool ItemAvaliable()
		{
			if (itemWR == null) return false;
			if (!itemWR.TryGetTarget(out var I)) return false;
			return true;
		}
		private T CheckItem()
		{
			if (itemWR == null) return CreateItem1();
			if (itemWR.TryGetTarget(out var I)) { return I; }
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
				itemWR = new WeakReference<T>(V);
				itemNodeWR=new WeakReference<LinkedListNode<object>>(StaticRefHolder.SRitems.AddLast(V));
				StaticRefHolder.SRs.AddLast(Unload);
			}
			else { 
				itemWR = null;
				itemNodeWR = null;
			}
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} ctor {((Action)(Unload)).Method}");
		}
		public T Value
		{
			get => CheckItem();
			set => SetItem(value);
		}
		private void SetItem1(T Item) {
			T V = Item;
			itemWR = new WeakReference<T>(V);
			SetItemNode(StaticRefHolder.SRitems.AddLast(V));
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} SetItem1 {V}");
			StaticRefHolder.SRs.AddLast(Unload);
		}
		private void SetItem2(T item) {
			T J = item;
			this.itemWR.SetTarget(J);
			SetItemNode(StaticRefHolder.SRitems.AddLast(J));
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} SetItem2 {J}");
		}
		private void SetItem(T item) {
			if (itemWR == null) SetItem1(item);
			else SetItem2(item);
		}
		public static implicit operator T(StaticRefWithNew<T> I) => I.Value;
		private void Unload()
		{
			itemWR.SetTarget(null);
			itemNodeWR.SetTarget(null);
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} Run Unload");
		}

	}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	/// <summary>
	/// 在使用时自动用F()创建对象
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class CtorByF<T> : IGetValue<T>, ISetValue<T>, IGetSetValue<T>
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
	public struct StaticRefWithFunc<T>:IGetValue<T>,ISetValue<T>,IGetSetValue<T>
		where T : class
	{
		private WeakReference<T> itemWR;
		private WeakReference<LinkedListNode<object>> itemNodeWR;
		private void SetItemNode(LinkedListNode<object> Node)
		{
			if (itemNodeWR == null) itemNodeWR = new WeakReference<LinkedListNode<object>>(Node);
			else
			{
				if (itemNodeWR.TryGetTarget(out LinkedListNode<object> ON))
				{
					StaticRefHolder.sRitems.Remove(ON);
				}
				itemNodeWR.SetTarget(Node);
			}
		}
		public readonly Func<T> CtorF;
		private T CreateItem1()
		{
			T V = CtorF();
			itemWR = new WeakReference<T>(V);
			SetItemNode(StaticRefHolder.SRitems.AddLast(V));
			XxDefinitions.Logv1.Debug($"StaticRefWithFunc{this.GetType().FullName} CreateItem1 {V}");
			StaticRefHolder.SRs.AddLast(Unload);
			return V;
		}
		private T CreateItem2()
		{
			T J = CtorF();
			itemWR.SetTarget(J);
			SetItemNode(StaticRefHolder.SRitems.AddLast(J));
			XxDefinitions.Logv1.Debug($"StaticRefWithFunc{this.GetType().FullName} CreateItem2 {J}");
			StaticRefHolder.SRs.AddLast(Unload);
			return J;
		}
		public bool ItemAvaliable()
		{
			if (itemWR == null) return false;
			if (!itemWR.TryGetTarget(out var I)) return false;
			return true;
		}
		private T CheckItem()
		{
			if (itemWR == null) return CreateItem1();
			if (itemWR.TryGetTarget(out var I)) { return I; }
			else
			{
				return CreateItem2();
			}
		}
		public StaticRefWithFunc(Func<T> I)
		{
			CtorF = I;
			itemWR = null;
			itemNodeWR = null;
			XxDefinitions.Logv1.Debug($"StaticRefWithFunc{this.GetType().FullName} ctor {((Action)(Unload)).Method}");
		}
		public T Value
		{
			get => CheckItem();
			set => SetItem(value);
		}
		private void SetItem1(T Item)
		{
			T V = Item;
			itemWR = new WeakReference<T>(V);
			SetItemNode(StaticRefHolder.SRitems.AddLast(V));
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} SetItem1 {V}");
			StaticRefHolder.SRs.AddLast(Unload);
		}
		private void SetItem2(T item)
		{
			T J = item;
			this.itemWR.SetTarget(J);
			SetItemNode(StaticRefHolder.SRitems.AddLast(J));
			XxDefinitions.Logv1.Debug($"StaticRefWithNew{this.GetType().FullName} SetItem2 {J}");
		}
		private void SetItem(T item)
		{
			if (itemWR == null) SetItem1(item);
			else SetItem2(item);
		}
		public static explicit operator T(StaticRefWithFunc<T> I) => I.Value;
		private void Unload()
		{
			itemWR.SetTarget(null);
			itemNodeWR.SetTarget(null);
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
