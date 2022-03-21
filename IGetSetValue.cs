﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;

namespace XxDefinitions
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public interface IGetValue<out T>
	{
		T Value { get; }
	}
	public interface ISetValue<in T>
	{
		T Value { set; }
	}
	public interface IGetSetValue<T> : IGetValue<T>, ISetValue<T>
	{
		new T Value { get; set; }
	}
	/// <summary>
	/// 引用的抽象类
	/// </summary>
	public abstract class Ref<T> : IGetValue<T>, ISetValue<T>, IGetSetValue<T> {
		/// <summary>
		/// 设置值的函数
		/// </summary>
		public abstract void SetFunc(T value);
		/// <summary>
		/// 获取值的函数
		/// </summary>
		public abstract T GetFunc();
		/// <summary>
		/// 值
		/// </summary>
		public T Value
		{
			get => GetFunc();
			set => SetFunc(value);
		}
	}

	public abstract class Get<T> : IGetValue<T>
	{
		/// <summary>
		/// 获取值的函数
		/// </summary>
		public abstract T GetFunc();
		/// <summary>
		/// 值
		/// </summary>
		public T Value
		{
			get => GetFunc();
		}
	}
	public abstract class Set<T> :ISetValue<T>
	{
		/// <summary>
		/// 设置值的函数
		/// </summary>
		public abstract void SetFunc(T value);
		/// <summary>
		/// 值
		/// </summary>
		public T Value
		{
			set => SetFunc(value);
		}
	}
	/// <summary>
	/// 通过委派函数实现引用
	/// </summary>
	/// <typeparam name="T">引用的对象的类型</typeparam>
	public class RefByDelegate<T> : Ref<T>
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
		public RefByDelegate(Func<T> GetF, Action<T> SetF)
		{
			this.GetF = GetF;
			this.SetF = SetF;
		}
		public override T GetFunc() => GetF();
		public override void SetFunc(T value) => SetF(value);
		public static explicit operator GetByDelegate<T>(RefByDelegate<T> I) => new GetByDelegate<T>(I.GetF);
		public static explicit operator SetByDelegate<T>(RefByDelegate<T> I) => new SetByDelegate<T>(I.SetF);
	}
	public class GetByDelegate<T> : Get<T>
	{
		/// <summary>
		/// 获取值的函数
		/// </summary>
		public readonly Func<T> GetF;
		/// <summary>
		/// 通过函数创建引用
		/// </summary>
		public GetByDelegate(Func<T> GetF)
		{
			this.GetF = GetF;
		}
		public override T GetFunc() => GetF();
		public static implicit operator GetByDelegate<T>(Func<T> func) => new GetByDelegate<T>(func);
	}
	public class SetByDelegate<T> : Set<T>
	{
		/// <summary>
		/// 设置值的函数
		/// </summary>
		public readonly Action<T> SetF;
		/// <summary>
		/// 通过函数创建引用
		/// </summary>
		public SetByDelegate(Action<T> SetF)
		{
			this.SetF = SetF;
		}
		public override void SetFunc(T value) => SetF(value);
		public static implicit operator SetByDelegate<T>(Action<T> func) => new SetByDelegate<T>(func);
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
		public T Value
		{
			get => value; set => this.value = value;
		}
		public ClassValue() { }
		public ClassValue(T value) { this.value = value; }
	}
}
