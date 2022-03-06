using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	/// <summary>
	/// 通过函数实现引用
	/// </summary>
	/// <typeparam name="T">引用的对象的类型</typeparam>
	public class RefByFunc<T>
	{
		/// <summary>
		/// 设置值的函数
		/// </summary>
		public readonly Action<T> SetF;
		/// <summary>
		/// 获取值的函数
		/// </summary>
		public readonly Func<T> GetF;
		/// <summary>
		/// 通过函数创建引用
		/// </summary>
		public RefByFunc(Action<T> SetF, Func<T> GetF) {
			this.GetF = GetF;
			this.SetF = SetF;
		}
		/// <summary>
		/// 值
		/// </summary>
		public T Value {
			get => GetF();
			set => SetF(value);
		}
	}
}
