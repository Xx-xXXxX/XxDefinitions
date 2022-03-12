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
	/// 提供了结合ItemTree和ID来管理Item的类，但是不能移除
	/// </summary>
	[Obsolete("使用ListWithID_Index代替")]
	public class ItemTreeWithID<IndexType, ItemType> : IEnumerable<KeyValuePair<int, ItemType>>, IEnumerable<KeyValuePair<IndexList<IndexType>, ItemType>>,IEnumerable<ItemType>
	{
		ItemTree<IndexType, int?> IndexToID=new ItemTree<IndexType, int?>();
		List<ItemType> IDToItem=new List<ItemType>();
		List<IndexList<IndexType>> IDToIndex=new List<IndexList<IndexType>>();
		/// <summary>
		/// 用索引获取元素的id
		/// </summary>
		public int GetID(IndexList<IndexType> index) => IndexToID[index].item.Value;
		/// <summary>
		/// 用id获取元素的索引
		/// </summary>
		public IndexList<IndexType> GetIndex(int id) => IDToIndex[id];
		/// <summary>
		/// 用id获取元素
		/// </summary>
		public ItemType GetItem(int id) => IDToItem[id];
		/// <summary>
		/// 用index获取元素
		/// </summary>
		public ItemType GetItem(IndexList<IndexType> index) => IDToItem[GetID(index)];
		/// <summary>
		/// 用id获取元素
		/// </summary>
		public ItemType this[int id]=>GetItem(id);
		/// <summary>
		/// 用index获取元素
		/// </summary>
		public ItemType this[IndexList<IndexType> index]=>GetItem(index);
		/// <summary>
		/// 元素的数量
		/// </summary>
		public int Count=> IDToItem.Count;
		/// <summary>
		/// 下一个可用的ID
		/// </summary>
		public int NextID => IDToItem.Count;
		/// <summary>
		/// 加入元素
		/// </summary>
		/// <returns>元素的id</returns>
		public int Add(ItemType item, IndexList<IndexType> index) {
			int newID = NextID;
			IndexToID.Add(index, newID);
			IDToItem.Add(item);
			IDToIndex.Add(index);
			return newID;
		}
		/// <summary>
		/// 在无索引的情况下加入元素，该元素没有索引
		/// </summary>
		public int Add(ItemType item) {
			int newID = NextID;
			IDToItem.Add(item);
			IDToIndex.Add(null);
			return newID;
		}
		/// <summary>
		/// 按ID枚举全部对象
		/// </summary>
		public IEnumerator<KeyValuePair<int, ItemType>> GetEnumerator()
		{
			for (int i = 0; i < IDToItem.Count; ++i)
			{
				yield return new KeyValuePair<int, ItemType>(i, IDToItem[i]);
			}
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		IEnumerator<KeyValuePair<IndexList<IndexType>, ItemType>> IEnumerable<KeyValuePair<IndexList<IndexType>, ItemType>>.GetEnumerator()
		{
			for (int i = 0; i < IDToItem.Count; ++i)
			{
				yield return new KeyValuePair<IndexList<IndexType>, ItemType>(IDToIndex[i], IDToItem[i]);
			}
		}
		IEnumerator<ItemType> IEnumerable<ItemType>.GetEnumerator()
		{
			return ((IEnumerable<ItemType>)IDToItem).GetEnumerator();
		}
	}

	/// <summary>
	/// 提供了结合ItemTree和ID来管理Item的类，可以移除
	/// </summary>
	public class ListWithID_Index<IndexType, ItemType> :ListWithID<ItemType>, IEnumerable<KeyValuePair<int, ItemType>>, IEnumerable<KeyValuePair<IndexList<IndexType>, ItemType>>, IEnumerable<ItemType>
	{
		/// <summary>
		/// 从Index索引ID
		/// </summary>
		ItemTree<IndexType, int?> IndexToID = new ItemTree<IndexType, int?>();
		//List<ItemType> IDToItem = new List<ItemType>();
		/// <summary>
		/// 从ID索引Index
		/// </summary>
		List<IndexList<IndexType>> IDToIndex = new List<IndexList<IndexType>>();
		/// <summary>
		/// 用索引获取元素的id
		/// </summary>
		public int GetID(IndexList<IndexType> index) => IndexToID[index].item.Value;
		/// <summary>
		/// 用id获取元素的索引
		/// </summary>
		public IndexList<IndexType> GetIndex(int id) => IDToIndex[id];
		/// <summary>
		/// 用index获取元素
		/// </summary>
		public ItemType GetItem(IndexList<IndexType> index) => IDToItem[GetID(index)];
		/// <summary>
		/// 用index获取元素
		/// </summary>
		public ItemType this[IndexList<IndexType> index] => GetItem(index);
		/// <summary>
		/// 加入元素
		/// </summary>
		/// <returns>元素的id</returns>
		public int Add(ItemType item, IndexList<IndexType> index=null,int id=-1)
		{
			if (id == -1) id = NextID;
			base.Add(item, id);
			if (index != null)
			{
				IndexToID.Add(index, id);
			}
			IDToIndex[id]=index;
			return id;
		}
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override void ReSizeUp(int size)
		{
			base.ReSizeUp(size);
			while (IDToIndex.Count <= size) IDToIndex.Add(null);
		}
		public override void ReSizeDown(int size)
		{
			base.ReSizeDown(size);
			while (IDToIndex.Count > size + 1)
			{
				if (IDToIndex.Last()==null) IDToIndex.RemoveAt(IDToIndex.Count - 1);
				else break;
			}
		}
		public override void ReSize(int size)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		{
			ReSizeUp(size);
			ReSizeDown(size);
		}
		/// <summary>
		/// 在第一个可用的id加入Item,该Item无index
		/// </summary>
		/// <returns>Item的id</returns>
		public override int Add(ItemType Item)
		{
			int id = base.Add(Item);
			IDToIndex[id]=null;
			return id;
		}
		/// <summary>
		/// 移除位于id的元素
		/// </summary>
		public override void RemoveAt(int id)
		{
			if(!ContiansID(id)) throw new ArgumentException($"位于 {id} 的元素不存在");
			IndexToID.RemoveItem(IDToIndex[id]);
			IDToIndex[id] = null;
			base.RemoveAt(id);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		IEnumerator<KeyValuePair<IndexList<IndexType>, ItemType>> IEnumerable<KeyValuePair<IndexList<IndexType>, ItemType>>.GetEnumerator()
		{
			foreach (var i in iDManager) {
				yield return new KeyValuePair<IndexList<IndexType>, ItemType>(IDToIndex[i],IDToItem[i]);
			}
		}
		IEnumerator<ItemType> IEnumerable<ItemType>.GetEnumerator()
		{
			foreach (var i in iDManager) {
				yield return IDToItem[i];
			}
		}
	}
}
