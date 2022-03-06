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
	[DebuggerDisplay("ElementCount = {ElementCount()}")]
	public class ItemTree<IndexType, ItemType> : IEnumerable<KeyValuePair<IndexList<IndexType>, ItemType>>
	{
		public ItemType item;
		public ItemTree<IndexType, ItemType> Father = null;
		public IndexType indexFromFather;
		SortedList<IndexType, ItemTree<IndexType, ItemType>> keyValues = new SortedList<IndexType, ItemTree<IndexType, ItemType>>();
		public ItemTree(ItemType _item)
		{
			item = _item;
		}
		public ItemTree(ItemTree<IndexType, ItemType> n)
		{
			AddClone(n);
		}
		public ItemTree() { }
		public int NodeCount
		{
			get => keyValues.Count;
		}
		public int ElementCount()
		{
			int n = 0;
			if (item!=null) n += 1;
			foreach (var i in keyValues)
			{
				n += i.Value.ElementCount();
			}
			return n;
		}
		public bool Contains(IndexType index) {
			return keyValues.ContainsKey(index);
		}
		public bool Contains(IndexList<IndexType> index) {
			//return Contains_(new IndexList<IndexType>(index));
			var N = this;
			foreach (var i in index) {
				if (!N.Contains(i)) return false;
				N = N[i];
			}
			return true;
		}
		private bool Contains_(IndexList<IndexType> index) {
			if (index.Count <= 0) return true;
			else {
				if (!Contains(index.First)) return false;
				index.RemoveFront();
				return  keyValues[index.First].Contains(index);
			}
		}
		public void AddClone(ItemTree<IndexType, ItemType> groupTree)
		{
			if (groupTree.item != null) item = groupTree.item;
			foreach (var n in groupTree.keyValues)
			{
				if (!Contains(n.Key)) keyValues.Add(n.Key, new ItemTree<IndexType, ItemType>());
				keyValues[n.Key].AddClone(n.Value);
			}
		}
		public void Add(IndexType index, ItemTree<IndexType, ItemType> groupTree)
		{
			keyValues.Add(index, groupTree);
			groupTree.Father = this;
			groupTree.indexFromFather = index;
		}
		public void Add(IndexType index, ItemType item)
		{
			//keyValues.Add(index, new ItemTree<IndexType, ItemType>(item));
			//keyValues[index].Father = this;
			Add(index, new ItemTree<IndexType, ItemType>(item));
		}
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
		public void Add(IndexList<IndexType> index, ItemType item)
		{
			Add(index, new ItemTree<IndexType, ItemType>(item));
		}
		public void Remove(IndexType index)
		{
			keyValues[index].Father = null;
			keyValues.Remove(index);
		}
		public ItemTree<IndexType, ItemType> this[IndexType index] {
			get=> keyValues [index]; 
		}
		public ItemTree<IndexType, ItemType> Get(IndexType index)
		{
			return keyValues[index];
		}
		public ItemTree<IndexType, ItemType> Get(IndexList<IndexType> index)
		{
			ItemTree<IndexType, ItemType> N = this;
			foreach (var i in index) { N = N[i]; }
			return N;
		}
		public ItemTree<IndexType, ItemType> this[IndexList<IndexType> index]
		{
			get => Get(index);
		}
		public IEnumerator<KeyValuePair<IndexList<IndexType>, ItemType>> GetEnumerator()
		{
			if (item != null)
				yield return new KeyValuePair<IndexList<IndexType>, ItemType>(new IndexList<IndexType>(), item);
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
			if (item != null)
				yield return new KeyValuePair<IndexList<IndexType>, ItemType>(new IndexList<IndexType>(), item);
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
		public class ItemTreeEnumer : IEnumerable<KeyValuePair<IndexList<IndexType>, ItemType>>
		{
			public ItemTree<IndexType, ItemType> itemTree;
			public IndexList<IndexType> IndicesFrom;
			public IEnumerator<KeyValuePair<IndexList<IndexType>, ItemType>> GetEnumerator()
			{
				if (itemTree.item != null)
					yield return new KeyValuePair<IndexList<IndexType>, ItemType>(IndicesFrom, itemTree.item);
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
				if (itemTree.item != null)
					yield return new KeyValuePair<IndexList<IndexType>, ItemType>(IndicesFrom, itemTree.item);
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
	}
	[DebuggerDisplay("{ToString()}")]
	public class IndexList<IndexType> : IEnumerable<IndexType>,IComparable<IndexList<IndexType>>
	{
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
		LinkedList<IndexType> Indices = new LinkedList<IndexType>();
		public IndexType First
		{
			get => Indices.First.Value;
			set => Indices.First.Value = value;
		}
		public IndexType Last
		{
			get => Indices.Last.Value;
			set => Indices.Last.Value = value;
		}
		public IndexList(IndexType index)
		{
			Indices.AddLast(index);
		}
		public IndexList()
		{
		}
		public IndexList(IndexList<IndexType> n)
		{
			AddFront(n);
		}
		public IndexList(params IndexType[] ns) {
			//ItemTreeIndex<IndexType>  new 
			//Indices=ns.ToList();
			foreach (var I in ns) {
				Indices.AddLast(I);
			}
		}
		public IndexList(ICollection<IndexType> L) {
			foreach (var i in L) {
				Indices.AddLast(i);
			}
		}
		public void AddLast(IndexType index)
		{
			Indices.AddLast(index);
		}
		public void AddFirst(IndexType index)
		{
			Indices.AddFirst(index);
		}
		public int Count
		{
			get => Indices.Count;
		}
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
		public LinkedListNode<IndexType> FindFirst(IndexType index)
		{
			return Indices.Find(index);
		}
		public LinkedListNode<IndexType> FindLast(IndexType index)
		{
			return Indices.FindLast(index);
		}
		public void RemoveFront(int n = 1)
		{
			for (int i = 0; i < n; i++)
			{
				Indices.RemoveFirst();
			}
		}
		public void RemoveBack(int n = 1)
		{
			for (int i = 0; i < n; i++)
			{
				Indices.RemoveLast();
			}
		}
		public void AddBack(IndexList<IndexType> index)
		{
			foreach (IndexType v in index)
			{
				Indices.AddLast(v);
			}
			//Indices.AddFirst(index.Indices);
		}
		public void AddFront(IndexList<IndexType> index)
		{
			var Il = index.Indices.Last;
			for (int i = 0; i < index.Count; i++)
			{
				Indices.AddFirst(Il.Value);
				Il = Il.Previous;
			}
		}
		public IEnumerator<IndexType> GetEnumerator()
		{
			return Indices.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Indices.GetEnumerator();
		}
		public static IndexList<IndexType> operator +(IndexList<IndexType> a, IndexList<IndexType> b)
		{
			var n = new IndexList<IndexType>(a);
			n.AddBack(b);
			return n;
		}
		public static IndexList<IndexType> operator +(IndexList<IndexType> a, IndexType b)
		{
			var n = new IndexList<IndexType>(a);
			n.AddLast(b);
			return n;
		}
		public static IndexList<IndexType> operator +(IndexType b, IndexList<IndexType> a)
		{
			var n = new IndexList<IndexType>(a);
			n.AddFirst(b);
			return n;
		}
		public override bool Equals(object item)
		{
			return MatchFront((IndexList<IndexType>)item);
		}
		public override int GetHashCode()
		{
			return Indices.GetHashCode();
		}

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

		public static bool operator ==(IndexList<IndexType> a, IndexList<IndexType> b)
		{
			return a.MatchFront(b);
		}
		public static bool operator !=(IndexList<IndexType> a, IndexList<IndexType> b)
		{
			return !a.MatchFront(b);
		}
	}
}
