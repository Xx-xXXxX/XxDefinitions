using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	/// <summary>
	/// 用于管理id,但是不能用于检查
	/// </summary>
	public class IDManager:IEnumerable<int>
	{
		private readonly SortedSet<int> IDSection=new SortedSet<int>();
		/// <summary>
		/// 
		/// </summary>
		public IDManager() {
			IDSection.Add(0);
		}
		/// <summary>
		/// 改变该id的使用状态
		/// </summary>
		public void ChangeUsing(int id) {
			if (!IDSection.Remove(id)) IDSection.Add(id);
			if (!IDSection.Remove(id+1)) IDSection.Add(id+1);
		}
		/// <summary>
		/// 添加id使用
		/// 实为ChangeUsing
		/// 不检查
		/// </summary>
		public void Add(int id) => ChangeUsing(id);
		/// <summary>
		/// 移除id使用
		/// 实为ChangeUsing
		/// 不检查
		/// </summary>
		public void Remove(int id) => ChangeUsing(id);
		/// <summary>
		/// 获取下一个可用的ID
		/// </summary>
		public int NextID() {
			return IDSection.First();
		}
		/// <summary>
		/// 获取比所有已用ID都大的第一个可用的ID
		/// </summary>
		public int LastID()
		{
			return IDSection.Last();
		}
		/// <summary>
		/// 枚举所有正在使用的id
		/// </summary>
		public IEnumerator<int> GetEnumerator()
		{
			int now=0;
			bool Having=true;
			foreach (var i in IDSection) {
				if (Having)
				{
					while (now < i)
					{
						yield return now;
						now += 1;
					}
					Having = false;
				}
				else {
					now = i;Having = true;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		/// <summary>
		/// 重置
		/// </summary>
		public void Clear() {
			IDSection.Clear(); IDSection.Add(0);
		}

		/// <summary>
		/// 返回对该id的下一个可用ID(如果id在原范围内)复杂度O(n)
		/// </summary>
		[Obsolete("但凡SortedList开放FindNode再写一个NextNode都可以有O(log n)的复杂度",true)]
		public int NextID(int id)
		{
			//IDSection.ElementAt(id);
			if (!IDSection.Contains(id + 1)) return id + 1;
			else
			{
				bool found = false;
				foreach (var i in IDSection) {
					if (found) return i;
					if (i == id) found = true;
				}
				return id + 1;
			}
		}
		//public StaticRefWithFunc<System.Reflection.MethodInfo> FindNode = new StaticRefWithFunc<System.Reflection.MethodInfo>(() =>
		//	typeof(SortedSet<int>).GetMethod("FindNode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
		//);
		/// <summary>
		/// 所有使用的ID
		/// </summary>\
		public List<int> ToArray() {
			List<int> vs = new List<int>();
			foreach (var i in this) vs.Add(i);
			return vs;
		}
	}
}
