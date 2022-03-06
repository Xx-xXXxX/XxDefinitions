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
		public int GetID(IndexList<IndexType> index) => IndexToID[index].item.Value;
		public IndexList<IndexType> GetIndex(int id) => IDToIndex[id];
		public ItemType GetItem(int id) => IDToItem[id];
		public ItemType GetItem(IndexList<IndexType> index) => IDToItem[GetID(index)];
		public ItemType this[int id]=>GetItem(id);
		public ItemType this[IndexList<IndexType> index]=>GetItem(index);
		public int Count=> IDToItem.Count;
		public int NextID => IDToItem.Count;
		public int Add(ItemType item, IndexList<IndexType> index) {
			int newID = NextID;
			IndexToID.Add(index, newID);
			IDToItem.Add(item);
			IDToIndex.Add(index);
			return newID;
		}
		public int Add(ItemType item) {
			int newID = NextID;
			IDToItem.Add(item);
			IDToIndex.Add(null);
			return newID;
		}

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
