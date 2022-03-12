using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public interface IGetValue<out T> {
		T Value { get; }
	}
	public interface ISetValue<in T> { 
		T Value { set; }
	}
	public interface IGetSetValue<T>
	{
		T Value { get; set; }
	}
	/// <summary>
	/// 通过委派函数实现引用
	/// </summary>
	/// <typeparam name="T">引用的对象的类型</typeparam>
	public class RefByDelegate<T>:IGetValue<T>,ISetValue<T>,IGetSetValue<T>
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
		public RefByDelegate(Action<T> SetF, Func<T> GetF)
		{
			this.GetF = GetF;
			this.SetF = SetF;
		}
		/// <summary>
		/// 值
		/// </summary>
		public T Value
		{
			get => GetF();
			set => SetF(value);
		}
	}
	/// <summary>
	/// 使用类表示值
	/// </summary>
	/// <typeparam name="T">类型</typeparam>
	public class ClassValue<T> : IGetValue<T>, ISetValue<T>, IGetSetValue<T>
	{
		private T value;
		/// <summary>
		/// 值
		/// </summary>
		public T Value {
			get=>value;set=>this.value=value;
		}
		public ClassValue() { }
		public ClassValue(T value) { this.value = value; }
	}
}
