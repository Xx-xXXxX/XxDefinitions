using Microsoft.Xna.Framework.Input;

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
		public int GetID(IndexList<IndexType> index) => IndexToID[index].value.Value;
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
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public class ListWithIDandIndex<IndexType, ItemType> : IEnumerable<KeyValuePair<int, ItemType>>, IEnumerable<KeyValuePair<IndexList<IndexType>, ItemType>>, IEnumerable<ItemType>, IDictionary<int, ItemType>, IDictionary<IndexList<IndexType>, ItemType>
	{
		/// <summary>
		/// Index->ID
		/// </summary>
		ItemTree<IndexType, int?> IndexToID = new ItemTree<IndexType, int?>();
		//List<ItemType> IDToItem = new List<ItemType>();
		/// <summary>
		/// ID->Index
		/// </summary>
		List<IndexList<IndexType>> IDToIndex = new List<IndexList<IndexType>>();
		/// <summary>
		/// ID->Item
		/// </summary>
		ListWithID<ItemType> IDToItem = new ListWithID<ItemType>();
		/// <summary>
		/// 下一个可用的ID
		/// </summary>
		public int NextID => IDToItem.NextID;

		public ICollection<int> Keys => ((IDictionary<int, ItemType>)IDToItem).Keys;
		ICollection<IndexList<IndexType>> IDictionary<IndexList<IndexType>, ItemType>.Keys {
			get {
				List<IndexList<IndexType>> indices=new List<IndexList<IndexType>>();
				foreach (var i in IDToItem) {
					indices.Add(GetIndex(i.Key));
				}
				return indices;
			}
		}

		public ICollection<ItemType> Values => ((IDictionary<int, ItemType>)IDToItem).Values;

		public int Count => ((ICollection<KeyValuePair<int, ItemType>>)IDToItem).Count;

		public bool IsReadOnly => ((ICollection<KeyValuePair<int, ItemType>>)IDToItem).IsReadOnly;

		/// <summary>
		/// 用索引获取元素的id
		/// </summary>
		public int GetID(IndexList<IndexType> index) => IndexToID[index].value.Value;
		/// <summary>
		/// 用id获取元素的索引
		/// </summary>
		public IndexList<IndexType> GetIndex(int id) => IDToIndex[id];
		/// <summary>
		/// 用index获取元素
		/// </summary>
		public ItemType Get(IndexList<IndexType> index) => IDToItem[GetID(index)];
		/// <summary>
		/// 用index获取元素更改
		/// </summary>
		public void Set(IndexList<IndexType> index, ItemType value) => IDToItem[GetID(index)] = value;
		/// <summary>
		/// 用index获取元素
		/// </summary>
		public ItemType this[IndexList<IndexType> index] {get=> Get(index);set=> Set(index, value); }
		/// <summary>
		/// 用ID获取元素
		/// </summary>
		public ItemType Get(int id) => IDToItem[id];
		/// <summary>
		/// 用ID获取元素
		/// </summary>
		public void Set(int id, ItemType value) => IDToItem[id]=value;
		/// <summary>
		/// 用index获取元素
		/// </summary>
		public ItemType this[int id] { get => Get(id);set => Set(id, value); }
		/// <summary>
		/// 加入元素
		/// </summary>
		/// <returns>元素的id</returns>
		public int Add(ItemType item, IndexList<IndexType> index=null,int id=-1)
		{
			if (id == -1) id = NextID;
			ReSizeUp(id);
			IDToItem.Add(id,item);
			if (index != null)
			{
				IndexToID.Add(index, id);
			}
			IDToIndex[id]=index;
			return id;
		}
		public void Add(int id, ItemType item) => Add(item,null,id);
		public void Add(IndexList<IndexType> index, ItemType item) => Add(item, index);
		public void ReSizeUp(int AbleId)
		{
			IDToItem.ReSizeUp(AbleId);
			while (IDToIndex.Count <= AbleId) IDToIndex.Add(null);
		}
		public void ReSizeDown(int AbleId)
		{
			IDToItem.ReSizeDown(AbleId);
			while (IDToIndex.Count > AbleId + 1)
			{
				if (IDToIndex.Last()==null) IDToIndex.RemoveAt(IDToIndex.Count - 1);
				else break;
			}
		}
		public void ReSize(int AbleId)
		{
			ReSizeUp(AbleId);
			ReSizeDown(AbleId);
		}
		/// <summary>
		/// 在第一个可用的id加入Item,该Item无index
		/// </summary>
		/// <returns>Item的id</returns>
		public int Add(ItemType Item)
		{
			int id = IDToItem.Add(Item);
			IDToIndex[id]=null;
			return id;
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return IDToItem.GetEnumerator();
		}
		IEnumerator<KeyValuePair<IndexList<IndexType>, ItemType>> IEnumerable<KeyValuePair<IndexList<IndexType>, ItemType>>.GetEnumerator()
		{
			foreach (var i in IDToItem) {
				yield return new KeyValuePair<IndexList<IndexType>, ItemType>(IDToIndex[i.Key],IDToItem[i.Key]);
			}
		}
		IEnumerator<ItemType> IEnumerable<ItemType>.GetEnumerator()
		{
			foreach (var i in IDToItem) {
				yield return IDToItem[i.Key];
			}
		}
		/// <summary>
		/// 枚举ID,ItemType
		/// </summary>
		public IEnumerator<KeyValuePair<int, ItemType>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<int, ItemType>>)IDToItem).GetEnumerator();
		}
		public bool ContainsKey(int id) => IDToItem.ContainsKey(id);
		public bool ContainsKey(IndexList<IndexType> index) => IndexToID.ContainsValue(index);
		/// <summary>
		/// 移除位于id的元素
		/// </summary>
		public bool Remove(int id) {
			if (!IDToItem.ContainsKey(id)) return false;
			IndexToID.RemoveItem(IDToIndex[id]);
			IDToIndex[id] = null;
			IDToItem.Remove(id);
			return true;
		}
		public bool Remove(IndexList<IndexType> index) {
			if (!ContainsKey(index)) return false;
			return Remove(GetID(index)); 
		}
		public bool TryGetValue(int id, out ItemType item) => IDToItem.TryGetValue(id,out item);
		public bool TryGetValue(IndexList<IndexType> index, out ItemType item) {
			if (ContainsKey(index))
				return IDToItem.TryGetValue(GetID(index), out item);
			else item = default;return false;
		}
		public void Add(KeyValuePair<int, ItemType> item)
		{
			//((ICollection<KeyValuePair<int, ItemType>>)IDToItem).Add(item);
			Add(item.Key, item.Value);
		}

		public void Clear()
		{
			//((ICollection<KeyValuePair<int, ItemType>>)IDToItem).Clear();
			IDToItem.Clear();
			IDToIndex.Clear();
			IndexToID.Clear();
		}

		public bool Contains(KeyValuePair<int, ItemType> item)
		{
			return IDToItem.Contains(item);
		}

		public void CopyTo(KeyValuePair<int, ItemType>[] array, int arrayIndex)
		{
			IDToItem.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<int, ItemType> item)
		{
			if (TryGetValue(item.Key, out ItemType value)) {
				if ((value?.Equals(item.Value)).IsTrue())
				{
					Remove(item.Key);
					return true;
				}
			}
			return false;
		}

		public bool Remove(KeyValuePair<IndexList<IndexType>, ItemType> item)
		{
			if (TryGetValue(item.Key, out ItemType value))
			{
				if ((value?.Equals(item.Value)).IsTrue())
				{
					Remove(item.Key);
					return true;
				}
			}
			return false;
		}

		public void Add(KeyValuePair<IndexList<IndexType>, ItemType> item)
		{
			Add(item.Key,item.Value);
		}

		public bool Contains(KeyValuePair<IndexList<IndexType>, ItemType> item)
		{
			if (IndexToID.TryGetValue(item.Key,out ItemTree<IndexType,int?> value))
			{
				if (value.Value.HasValue) {
					return Contains(new KeyValuePair<int, ItemType>( value.Value.Value, item.Value));
				}
			}
			return false;
		}

		public void CopyTo(KeyValuePair<IndexList<IndexType>, ItemType>[] array, int index)
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
				throw new ArgumentException($"array 的可用长度 {array.Length - index} 小于元素数量 {Count}", "array,index");
			}
			foreach (var i in IDToItem) {
				array[index++] = new KeyValuePair<IndexList<IndexType>, ItemType>(GetIndex(i.Key),i.Value);
			}
		}
	}
}
