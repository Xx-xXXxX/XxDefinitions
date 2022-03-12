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
	}
}
