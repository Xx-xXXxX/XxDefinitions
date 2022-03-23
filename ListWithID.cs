using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">元素类型，认为default(T)为空</typeparam>
	public class ListWithID<T>:IEnumerable<KeyValuePair<int,T>>
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
		/// 从ID索引元素
		/// </summary>
		protected readonly List<T> IDToItem = new List<T>();
		/// <summary>
		/// 
		/// </summary>
		public ListWithID(){
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
			while (IDToItem.Count > size+1) {
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
			return id;
		}
		/// <summary>
		/// 在id处加入Item
		/// </summary>
		public virtual void Add(T Item, int id)
		{
			ReSizeUp(id + 1);
			if (!IDToItem[id].IsDef())
				throw new ArgumentException($"位于 {id} 的元素已存在 {IDToItem[id]}");
			IDToItem[id] = Item;
			iDManager.Add(id);
			return;
		}
		/// <summary>
		/// ID是否使用
		/// </summary>
		public virtual bool ContiansID(int id) {
			if (IDToItem.Count <= id) return false;
			if (IDToItem[id].IsDef()) return false;
			return true;
		}
		/// <summary>
		/// 安全的在id加入Item,如果存在则在第一个用的id处加入Item
		/// </summary>
		/// <param name="Item"></param>
		/// <param name="id"></param>
		/// <returns>Item的id</returns>
		public virtual int SafelyAdd(T Item, int id) {
			if (ContiansID(id)) { Add(Item, id); return id; }
			else {
				Add(Item);return id;
			}
		}
		/// <summary>
		/// 移除位于id处的元素
		/// </summary>
		public virtual void RemoveAt(int id) {
			if (!ContiansID(id)) throw new ArgumentException($"位于 {id} 的元素不存在");
			IDToItem[id] = default(T);
			iDManager.Remove(id);
			ReSizeDown(0);
		}
		/// <summary>
		/// 安全的移除位于id处的元素
		/// </summary>
		/// <param name="id"></param>
		/// <returns>是否成功被移除</returns>
		public virtual bool SafelyRemove(int id) {
			if (!ContiansID(id)) return false;
			IDToItem[id] = default(T);
			iDManager.Remove(id);
			ReSizeDown(0);
			return true;
		}
		/// <summary>
		/// 获取在id处的元素
		/// </summary>
		public T GetItem(int id) => IDToItem[id];
		/// <summary>
		/// 获取在id处的元素
		/// </summary>
		public T Get(int id) => IDToItem[id];
		/// <summary>
		/// 获取在id处的元素
		/// </summary>
		public T this[int id] => GetItem(id);
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


	}
}
