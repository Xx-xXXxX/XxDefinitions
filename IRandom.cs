using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	/// <summary>
	/// 对随机数的接口
	/// </summary>
	public interface IRandom
	{
		/// <summary>
		/// 获取随机值
		/// </summary>
		int Sample();
	}
	/// <summary>
	/// 通过委派函数实现接口
	/// </summary>
	public class IRandomByDelegate:IRandom
	{
		readonly Func<int> sample;
		/// <summary>
		/// 获取随机值
		/// </summary>
		public int Sample() => sample();
		/// <summary>
		/// 通过委派函数实现接口
		/// </summary>
		public IRandomByDelegate(Func<int> f) { sample = f; }
	}

}
