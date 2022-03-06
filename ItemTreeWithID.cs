using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	/// <summary>
	/// 提供了结合ItemTree和ID来管理Item的类，但是不能移除
	/// </summary>
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
}
