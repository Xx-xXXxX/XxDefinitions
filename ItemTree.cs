using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	/// <summary>
	/// 提供以树结构来管理元素的方法
	/// </summary>
	/// <typeparam name="IndexType"></typeparam>
	/// <typeparam name="ItemType"></typeparam>
	[DebuggerDisplay("ElementCount = {ElementCount()}")]
	public class ItemTree<IndexType, ItemType> : IEnumerable<KeyValuePair<IndexList<IndexType>, ItemType>>,IGetValue<ItemType>,ISetValue<ItemType>,IGetSetValue<ItemType>//,IDictionary<IndexList< IndexType >, ItemType>
	{
		/// <summary>
		/// 该节点元素
		/// </summary>
		public ItemType value;
		/// <summary>
		/// 该节点元素
		/// </summary>
		public ItemType Value {
			get => value;
			set => this.value = value;
		}
		/// <summary>
		/// 父亲
		/// </summary>
		public ItemTree<IndexType, ItemType> Father = null;
		/// <summary>
		/// 在父亲中的索引
		/// </summary>
		public IndexType indexFromFather;
		SortedList<IndexType, ItemTree<IndexType, ItemType>> keyValues = new SortedList<IndexType, ItemTree<IndexType, ItemType>>();
		/// <summary>
		/// 创建含有item的ItemTree
		/// </summary>
		public ItemTree(ItemType _item)
		{
			value = _item;
		}
		/// <summary>
		/// 克隆
		/// </summary>
		public ItemTree(ItemTree<IndexType, ItemType> n)
		{
			AddClone(n);
		}
		/// <summary>
		/// 创建空ItemTree
		/// </summary>
		public ItemTree() { }
		/// <summary>
		/// 该节点的孩子数量
		/// </summary>
		public int NodeCount
		{
			get => keyValues.Count;
		}
		/// <summary>
		/// 该节点以下所有元素数量
		/// </summary>
		public int ElementCount()
		{
			int n = 0;
			if (value!=null) n += 1;
			foreach (var i in keyValues)
			{
				n += i.Value.ElementCount();
			}
			return n;
		}
		/// <summary>
		/// 包含该索引指向的ItemTree
		/// </summary>
		public bool Contains(IndexType index) {
			return keyValues.ContainsKey(index);
		}
		/// <summary>
		/// 包含该索引指向的ItemTree
		/// </summary>
		public bool Contains(IndexList<IndexType> index) {
			//return Contains_(new IndexList<IndexType>(index));
			var N = this;
			foreach (var i in index) {
				if (!N.Contains(i)) return false;
				N = N[i];
			}
			return true;
		}
		/// <summary>
		/// 包含该索引指向的ItemTree且该ItemTree有值
		/// </summary>
		public bool ContainsValue(IndexList<IndexType> index)
		{
			//return Contains_(new IndexList<IndexType>(index));
			var N = this;
			foreach (var i in index)
			{
				if (!N.Contains(i)) return false;
				N = N[i];
			}
			return !N.value.IsDef();
		}
		private bool Contains_(IndexList<IndexType> index) {
			if (index.Count <= 0) return true;
			else {
				if (!Contains(index.First)) return false;
				index.RemoveFront();
				return  keyValues[index.First].Contains(index);
			}
		}
		/// <summary>
		/// 克隆整个groupTree加入自己
		/// </summary>
		/// <param name="groupTree"></param>
		public void AddClone(ItemTree<IndexType, ItemType> groupTree)
		{
			if (!groupTree.value.IsDef()) value = groupTree.value;
			foreach (var n in groupTree.keyValues)
			{
				if (!Contains(n.Key)) keyValues.Add(n.Key, new ItemTree<IndexType, ItemType>());
				keyValues[n.Key].AddClone(n.Value);
			}
		}
		/// <summary>
		/// 加入groupTree于index处
		/// </summary>
		public void Add(IndexType index, ItemTree<IndexType, ItemType> groupTree)
		{
			keyValues.Add(index, groupTree);
			groupTree.Father = this;
			groupTree.indexFromFather = index;
		}
		/// <summary>
		/// 加入new ItemTree(item)于index处
		/// </summary>
		public void Add(IndexType index, ItemType item)
		{
			//keyValues.Add(index, new ItemTree<IndexType, ItemType>(item));
			//keyValues[index].Father = this;
			Add(index, new ItemTree<IndexType, ItemType>(item));
		}/// <summary>
		 /// 加入groupTree于index处
		 /// </summary>
		public void Add(IndexList<IndexType> index, ItemTree<IndexType, ItemType> groupTree)
		{
			if (index.Count == 0)
			{
				AddClone(groupTree);
			}
			else
			{
				var N = this;
				var index2 = new IndexList<IndexType>(index);
				index2.RemoveBack();
				foreach (var n in index2)
				{
					if (!keyValues.ContainsKey(n))
					{
						N.Add(n, new ItemTree<IndexType, ItemType>());
					}
					N = keyValues[n];
				}
				//N.AddClone(groupTree);
				N.Add(index.Last, groupTree);
			}
		}
		/// <summary>
		/// 加入new ItemTree(item)于index处
		/// </summary>
		public void Add(IndexList<IndexType> index, ItemType item)
		{
			Add(index, new ItemTree<IndexType, ItemType>(item));
		}
		/// <summary>
		/// 移除于index处的孩子并清理
		/// </summary>
		public void Remove(IndexType index)
		{
			keyValues[index].Father = null;
			keyValues.Remove(index);
			RemoveNulls();
		}
		/// <summary>
		/// 移除于index处的孩子并清理
		/// </summary>
		public void Remove(IndexList<IndexType> index)
		{
			//Remove_(new IndexList<IndexType>(index));
			var N = this;
			IndexType L = index.Last;
			index.RemoveBack();
			foreach (var i in index)
			{
				if (!N.Contains(i)) throw new ArgumentException($"{index} 不存在");
				N = N[i];
			}
			//N.item = default(ItemType);
			N.Remove(L);
		}
		/// <summary>
		/// 移除于index处的元素并清理
		/// </summary>
		public void RemoveItem(IndexList<IndexType> index) {
			//Remove_(new IndexList<IndexType>(index));
			var N = this;
			foreach (var i in index) {
				if (!N.Contains(i)) throw new ArgumentException($"{index} 不存在");
				N = N[i];
			}
			N.value = default(ItemType);
			N.RemoveNulls();
		}
		private void Remove_(IndexList<IndexType> index) {
			if (index.Count == 0) {
				value = default(ItemType);
				return;
			}
			IndexType i = index.First;
			index.RemoveFront();
			keyValues[i].Remove_(index);
		}
		/// <summary>
		/// 该节点为空，可以清理
		/// </summary>
		public bool IsNull() {
			if (value.IsDef() && keyValues.Count == 0) return true;
			return false;
		}
		/// <summary>
		/// 清理孩子
		/// </summary>
		public void RemoveNullSuns() {
			if (!value.IsDef()) return;
			LinkedList<IndexType> Nulls = new LinkedList<IndexType>();
			foreach (var i in keyValues) {
				i.Value.RemoveNullSuns();
				if (i.Value.IsNull()) {
					Nulls.AddLast(i.Key);
				}
			}
			foreach (var i in Nulls) {
				Remove(i);
			}
		}
		/// <summary>
		/// 清理孩子并向上递归
		/// </summary>
		public void RemoveNulls() {
			RemoveNullSuns();
			if (IsNull()) {
				if (Father != null) {
					Father.RemoveNulls();
				}
			}
		}
		/// <summary>
		/// 获取位于index的ItemTree
		/// </summary>
		public ItemTree<IndexType, ItemType> this[IndexType index] {
			get=> keyValues [index];
		}
		/// <summary>
		/// 获取位于index的ItemTree
		/// </summary>
		public ItemTree<IndexType, ItemType> Get(IndexType index)
		{
			return keyValues[index];
		}
		/// <summary>
		/// 获取位于index的ItemTree
		/// </summary>
		public ItemTree<IndexType, ItemType> Get(IndexList<IndexType> index)
		{
			ItemTree<IndexType, ItemType> N = this;
			foreach (var i in index) { N = N[i]; }
			return N;
		}
		/// <summary>
		/// 获取位于index的ItemTree
		/// </summary>
		public ItemTree<IndexType, ItemType> this[IndexList<IndexType> index]
		{
			get => Get(index);
		}
		/// <summary>
		/// 枚举所有元素
		/// </summary>
		public IEnumerator<KeyValuePair<IndexList<IndexType>, ItemType>> GetEnumerator()
		{
			if (!value.IsDef())
				yield return new KeyValuePair<IndexList<IndexType>, ItemType>(new IndexList<IndexType>(), value);
			if (NodeCount > 0)
			{
				foreach (var n in keyValues)
				{
					//index.AddFirst(n.Key);
					foreach (var n2 in n.Value)
					{
						yield return new KeyValuePair<IndexList<IndexType>, ItemType>(n.Key + n2.Key, n2.Value);
					}
				}
			}
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			if (!value.IsDef())
				yield return new KeyValuePair<IndexList<IndexType>, ItemType>(new IndexList<IndexType>(), value);
			if (NodeCount > 0)
			{
				foreach (var n in keyValues)
				{
					//index.AddFirst(n.Key);
					foreach (var n2 in n.Value)
					{
						yield return new KeyValuePair<IndexList<IndexType>, ItemType>(n.Key + n2.Key, n2.Value);
					}
				}
			}
		}
		/// <summary><![CDATA[
		/// 获取从第depth个父亲到自己的索引
		/// 如果到达根节点则终止（返回从根节点到自己的索引）]]>
		/// </summary>
		/// <param name="depth"></param>
		/// <returns></returns>
		public IndexList<IndexType> GetIndex(int depth)
		{
			if (Father == null) {
				return new IndexList<IndexType>();
			}
			else
			if (depth == 0) return new IndexList<IndexType>();
			else
			if (depth < 0)
			{
				throw new ArgumentOutOfRangeException("depth",depth,"参数小于0");
			}
			else//depth>0
			{
				IndexList<IndexType> indices = Father.GetIndex(depth - 1);
				indices.AddLast(indexFromFather);
				return indices;
			}
		}
		/// <summary>
		/// Enum At IndicesAt
		/// The returned item.Key including IndicesAt
		/// which Enum this.Get(IndicesAt) don't have
		/// </summary>
		/// <param name="IndicesAt"></param>
		/// <returns></returns>
		public ItemTreeEnumer EnumAt(IndexList<IndexType> IndicesAt) { return new ItemTreeEnumer(Get(IndicesAt), IndicesAt); }
		/// <summary>
		/// 从某个节点出发枚举在另一个节点的全部元素（在节点的枚举的索引加上一部分）
		/// </summary>
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public class ItemTreeEnumer : IEnumerable<KeyValuePair<IndexList<IndexType>, ItemType>>
		{
			public ItemTree<IndexType, ItemType> itemTree;
			public IndexList<IndexType> IndicesFrom;
			public IEnumerator<KeyValuePair<IndexList<IndexType>, ItemType>> GetEnumerator()
			{
				if (!itemTree.value.IsDef())
					yield return new KeyValuePair<IndexList<IndexType>, ItemType>(IndicesFrom, itemTree.value);
				if (itemTree.NodeCount > 0)
				{
					foreach (var n in itemTree.keyValues)
					{
						//index.AddFirst(n.Key);
						foreach (var n2 in n.Value)
						{
							yield return new KeyValuePair<IndexList<IndexType>, ItemType>(IndicesFrom + n.Key + n2.Key, n2.Value);
						}
					}
				}
			}
			IEnumerator IEnumerable.GetEnumerator()
			{
				if (!itemTree.value.IsDef())
					yield return new KeyValuePair<IndexList<IndexType>, ItemType>(IndicesFrom, itemTree.value);
				if (itemTree.NodeCount > 0)
				{
					foreach (var n in itemTree.keyValues)
					{
						//index.AddFirst(n.Key);
						foreach (var n2 in n.Value)
						{
							yield return new KeyValuePair<IndexList<IndexType>, ItemType>(IndicesFrom + n.Key + n2.Key, n2.Value);
						}
					}
				}
			}
			public ItemTreeEnumer(ItemTree<IndexType, ItemType> itemTree, IndexList<IndexType> IndicesFrom) {
				this.itemTree = itemTree;
				this.IndicesFrom = IndicesFrom;
			}
		}
		public void Clear() {
			value = default;
			keyValues.Clear();
		}
		public bool TryGetValue(IndexList<IndexType> index, out ItemTree<IndexType, ItemType> itemTree) {
			itemTree = null;
			foreach (var i in index)
			{
				if (!itemTree.Contains(i)) { itemTree = null; return false; }
				itemTree = itemTree[i];
			}
			return true;
		}

#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
	}
	/// <summary>
	/// 通过链表管理一串索引
	/// </summary>
	/// <typeparam name="IndexType">索引类型</typeparam>
	[DebuggerDisplay("{ToString()}")]
	public class IndexList<IndexType> : IEnumerable<IndexType>,IComparable<IndexList<IndexType>>
	{
		/// <summary>
		/// 列出全部节点
		/// </summary>
		public override string ToString()
		{
			if (Indices == null) return "Indices == null";
			string N = "";
			if (Count > 0)
			{
				
				foreach (var i in Indices)
				{
					N += i + " ";
				}/*
				var I = Indices.First;
				while ((I = I.Next) == null)
				{
					N += I.Value;
					if (I.Next != null) N += ",";
				}*/
			}
			else N = ".";
			return N;
		}
		/// <summary>
		/// 索引列表
		/// </summary>
		LinkedList<IndexType> Indices = new LinkedList<IndexType>();
		/// <summary>
		/// 第一个索引
		/// </summary>
		public IndexType First
		{
			get => Indices.First.Value;
			set => Indices.First.Value = value;
		}
		/// <summary>
		/// 最后一个索引
		/// </summary>
		public IndexType Last
		{
			get => Indices.Last.Value;
			set => Indices.Last.Value = value;
		}
		/// <summary>
		/// 创建一个只含有一个index的IndexList
		/// </summary>
		/// <param name="index"></param>
		public IndexList(IndexType index)
		{
			Indices.AddLast(index);
		}
		/// <summary>
		/// 创建一个空IndexList
		/// </summary>
		public IndexList()
		{
		}
		/// <summary>
		/// 克隆IndexList
		/// </summary>
		public IndexList(IndexList<IndexType> n)
		{
			AddFront(n);
		}
		/// <summary>
		/// 创建含有ns的IndexList
		/// </summary>
		public IndexList(params IndexType[] ns) {
			//ItemTreeIndex<IndexType>  new 
			//Indices=ns.ToList();
			foreach (var I in ns) {
				Indices.AddLast(I);
			}
		}
		/// <summary>
		/// 克隆IndexList
		/// </summary>
		public IndexList(ICollection<IndexType> L) {
			foreach (var i in L) {
				Indices.AddLast(i);
			}
		}
		/// <summary>
		/// 在最后加入
		/// </summary>
		public void AddLast(IndexType index)
		{
			Indices.AddLast(index);
		}
		/// <summary>
		/// 在开头加入
		/// </summary>
		public void AddFirst(IndexType index)
		{
			Indices.AddFirst(index);
		}
		/// <summary>
		/// 元素数量
		/// </summary>
		public int Count
		{
			get => Indices.Count;
		}
		/// <summary>
		/// 完整匹配
		/// </summary>
		public bool MatchAll(IndexList<IndexType> index) {
			if (index.Count != Count) return false;
			var Ib = index.Indices.First;
			var Ia = Indices.First;
			for (int i = 0; i < index.Count; ++i)
			{
				if (!Ib.Value.Equals(Ia.Value)) return false;
				Ib = Ib.Next;
				Ia = Ia.Next;
			}
			return true;

		}
		/// <summary>
		/// 从开头进行匹配index.Count
		/// </summary>
		public bool MatchFront(IndexList<IndexType> index)
		{
			if (index.Count > Count) return false;
			var Ib = index.Indices.First;
			var Ia = Indices.First;
			for (int i = 0; i < index.Count; ++i)
			{
				if (!Ib.Value.Equals(Ia.Value)) return false;
				Ib = Ib.Next;
				Ia = Ia.Next;
			}
			return true;
		}
		/// <summary>
		/// 从末尾开始向前匹配index.Count
		/// </summary>
		public bool MatchBack(IndexList<IndexType> index)
		{
			if (index.Count > Count) return false;
			var Ib = index.Indices.Last;
			var Ia = Indices.Last;
			for (int i = 0; i < index.Count; ++i)
			{
				if (!Ib.Value.Equals(Ia.Value)) return false;
				Ib = Ib.Previous;
				Ia = Ia.Previous;
			}
			//Indices.F
			return true;
		}
		/// <summary>
		/// 找到第一个与index相同的LinkedListNode
		/// </summary>
		public LinkedListNode<IndexType> FindFirst(IndexType index)
		{
			return Indices.Find(index);
		}
		/// <summary>
		/// 从后往前找到第一个与index相同的LinkedListNode
		/// </summary>
		public LinkedListNode<IndexType> FindLast(IndexType index)
		{
			return Indices.FindLast(index);
		}
		/// <summary>
		/// 移除开头的n个元素
		/// </summary>
		public void RemoveFront(int n = 1)
		{
			for (int i = 0; i < n; i++)
			{
				Indices.RemoveFirst();
			}
		}
		/// <summary>
		/// 移除末尾的n个元素
		/// </summary>
		public void RemoveBack(int n = 1)
		{
			for (int i = 0; i < n; i++)
			{
				Indices.RemoveLast();
			}
		}
		/// <summary>
		/// 在末尾加入index
		/// </summary>
		public void AddBack(IndexList<IndexType> index)
		{
			foreach (IndexType v in index)
			{
				Indices.AddLast(v);
			}
			//Indices.AddFirst(index.Indices);
		}
		/// <summary>
		/// 在末尾加入index
		/// </summary>
		public void AddBack(IndexType index)
		{
			Indices.AddLast(index);
			//Indices.AddFirst(index.Indices);
		}
		/// <summary>
		/// 在开头加入index
		/// </summary>
		public void AddFront(IndexList<IndexType> index)
		{
			var Il = index.Indices.Last;
			for (int i = 0; i < index.Count; i++)
			{
				Indices.AddFirst(Il.Value);
				Il = Il.Previous;
			}
		}
		/// <summary>
		/// 在开头加入index
		/// </summary>
		public void AddFront(IndexType index)
		{
			Indices.AddFirst(index);
		}
		/// <summary>
		/// 枚举
		/// </summary>
		public IEnumerator<IndexType> GetEnumerator()
		{
			return Indices.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Indices.GetEnumerator();
		}
		/// <summary>
		/// 合并（克隆）
		/// </summary>
		public static IndexList<IndexType> operator +(IndexList<IndexType> a, IndexList<IndexType> b)
		{
			var n = new IndexList<IndexType>(a);
			n.AddBack(b);
			return n;
		}
		/// <summary>
		/// 合并（克隆）
		/// </summary>
		public static IndexList<IndexType> operator +(IndexList<IndexType> a, IndexType b)
		{
			var n = new IndexList<IndexType>(a);
			n.AddLast(b);
			return n;
		}
		/// <summary>
		/// 合并（克隆）
		/// </summary>
		public static IndexList<IndexType> operator +(IndexType b, IndexList<IndexType> a)
		{
			var n = new IndexList<IndexType>(a);
			n.AddFirst(b);
			return n;
		}
		/// <summary>
		/// 判断相同
		/// </summary>
		public override bool Equals(object item)
		{
			//return item!=null&&MatchFront((IndexList<IndexType>)item);
			if (item.IsNull()) return false;
			if (item is IndexList<IndexType>)
			{
				return MatchAll((IndexList<IndexType>)item);
			}
			else return false;
		}
		/// <summary>
		/// Indices.GetHashCode();
		/// </summary>
		public override int GetHashCode()
		{
			return Indices.GetHashCode();
		}
		/// <summary><![CDATA[
		/// 通过Comparer<IndexType>.Default.Compare比较]]>
		/// </summary>
		public int CompareTo(IndexList<IndexType> index)
			
		{
			int MinCount = Math.Min(Count, index.Count);
			var Ib = index.Indices.First;
			var Ia = Indices.First;
			int i;
			for (i = 0; i < MinCount; ++i)
			{
				int C = Comparer<IndexType>.Default.Compare(Ia.Value, Ib.Value);
				if (C != 0) return C;
				Ib = Ib.Next;
				Ia = Ia.Next;
			}
			return Comparer<int>.Default.Compare(this.Count-i,index.Count-i);
		}
		/// <summary>
		/// 判断相同
		/// </summary>
		public static bool operator ==(IndexList<IndexType> a, IndexList<IndexType> b)
		{
			if (((object)(a)) == null)
			{
				if ((object)(b) == null)
					return true;
				else
					return false;
			}
			else if ((object)(b) == null) return false;
			else
				return a.MatchAll(b);
		}
		/// <summary>
		/// 判断不同
		/// </summary>
		public static bool operator !=(IndexList<IndexType> a, IndexList<IndexType> b)
		{
			return !(a == b);
		}
	}
}
