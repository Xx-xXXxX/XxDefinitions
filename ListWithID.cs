using ReLogic.Reflection;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">元素类型，认为default(T)为空</typeparam>
	public class ListWithID<T> : IEnumerable<KeyValuePair<int, T>>, IDictionary<int, T>
	{
		/// <summary>
		/// 管理ID
		/// </summary>
		protected readonly IDManager iDManager = new IDManager();
		/// <summary>
		/// 下一个可用的id
		/// </summary>
		public int NextID => iDManager.NextID();

		/// <summary>
		/// 克隆键列表
		/// </summary>
		public ICollection<int> Keys {
			get => iDManager.ToArray();
		}
		/// <summary>
		/// 克隆值列表
		/// </summary>
		public ICollection<T> Values {
			get {
				List<T> ts = new List<T>();
				foreach (var i in this) ts.Add(i.Value);
				return ts;
			}
		}
		private int count=0;
		/// <summary>
		/// 元素数量
		/// </summary>
		public int Count => count;

		/// <summary>
		/// false
		/// </summary>
		public bool IsReadOnly =>false;

		/// <summary>
		/// 从ID索引元素
		/// </summary>
		protected readonly List<T> IDToItem = new List<T>();
		/// <summary>
		/// 
		/// </summary>
		public ListWithID() {
		}
		/// <summary>
		/// 向上调整大小达到size
		/// </summary>
		public virtual void ReSizeUp(int size) {

			while (IDToItem.Count <= size) IDToItem.Add(default(T));
		}
		/// <summary>
		/// 向下调整大小不低于size
		/// </summary>
		public virtual void ReSizeDown(int size)
		{
			while (IDToItem.Count > size + 1) {
				if (IDToItem.Last().IsDef()) IDToItem.RemoveAt(IDToItem.Count - 1);
				else break;
			}
		}
		/// <summary>
		/// 调整大小
		/// </summary>
		public virtual void ReSize(int size) {
			ReSizeUp(size);
			ReSizeDown(size);
		}
		/// <summary>
		/// 在第一个用的id处加入Item
		/// </summary>
		/// <param name="Item"></param>
		/// <returns>Item的id</returns>
		public virtual int Add(T Item) {
			int id = NextID;
			ReSizeUp(id + 1);
			IDToItem[id] = Item;
			iDManager.Add(id);
			count += 1;
			return id;
		}
		/// <summary>
		/// 在id处加入Item
		/// </summary>
		public virtual void Add(int id, T Item)
		{
			ReSizeUp(id + 1);
			if (!IDToItem[id].IsDef())
				throw new ArgumentException($"位于 {id} 的元素已存在 {IDToItem[id]}");
			IDToItem[id] = Item;
			iDManager.Add(id);
			count += 1;
			return;
		}
		/// <summary>
		/// 安全的在id加入Item,如果存在则在第一个用的id处加入Item
		/// </summary>
		/// <param name="Item"></param>
		/// <param name="id"></param>
		/// <returns>Item的id</returns>
		public virtual int SafelyAdd(T Item, int id) {
			if (ContainsKey(id)) { Add(id, Item); return id; }
			else {
				Add(Item); return id;
			}
		}
		/// <summary>
		/// 移除位于id处的元素
		/// </summary>
		public virtual bool Remove(int id) {
			if (!ContainsKey(id)) return false;
			IDToItem[id] = default(T);
			iDManager.Remove(id);
			ReSizeDown(0);
			count -= 1;
			return true;
		}
		/// <summary>
		/// 安全的移除位于id处的元素
		/// </summary>
		/// <param name="id"></param>
		/// <returns>是否成功被移除</returns>
		public virtual bool SafelyRemove(int id) {
			if (!ContainsKey(id)) return false;
			IDToItem[id] = default(T);
			iDManager.Remove(id);
			ReSizeDown(0);
			count -= 1;
			return true;
		}
		/// <summary>
		/// 获取在id处的元素
		/// </summary>
		public T Get(int id) => IDToItem[id];
		/// <summary>
		/// 更改在id处的元素
		/// </summary>
		public void Set(int id, T value) => IDToItem[id] = value;
		/// <summary>
		/// 在id处的元素
		/// </summary>
		public T this[int id] {
			get => Get(id);
			set => Set(id,value);
		}
		/// <summary>
		/// 枚举
		/// </summary>
		public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
		{
			foreach (var i in iDManager) {
				yield return new KeyValuePair<int, T>(i,IDToItem[i]);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public bool ContainsKey(int key)
		{
			if (key < 0) return false;
			if (key > IDToItem.Count) return false;
			return IDToItem[key].IsDef();
		}

		public bool TryGetValue(int key, out T value)
		{
			if (ContainsKey(key)) { value = IDToItem[key]; return true; }
			else { value = default(T);return false; }
		}

		public void Add(KeyValuePair<int, T> item)
		{
			Add(item.Key,item.Value);
		}

		public void Clear()
		{
			iDManager.Clear();
			IDToItem.Clear();
			count = 0;
		}

		public bool Contains(KeyValuePair<int, T> item)
		{
			if (TryGetValue(item.Key, out T v)) {
				if ((item.Value?.Equals(v)).IsTrue())
					return true;
			}
			return false;
		}

		public void CopyTo(KeyValuePair<int, T>[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}

			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", $"{index} 不在array Length:{array.Length}范围内");
			}

			if (array.Length - index < Count)
			{
				throw new ArgumentException($"array 的可用长度 {array.Length - index} 小于元素数量 {Count}","array,index");
			}
			foreach (var i in this) array[index++] = i;
		}

		public bool Remove(KeyValuePair<int, T> item)
		{
			if (Contains(item)) return false;
			Remove(item.Key);
			return true;
		}

	}
}
