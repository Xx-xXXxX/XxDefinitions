﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	/// <summary>
	/// 分步枚举，每次枚举一部分，共subtime次
	/// </summary>
	public class SubstepEnum:IEnumerable<int>
	{
		private int left=0;
		private int right;
		private int step=1;
		private int distance;
		private int enumedTime=0;
		private int placei;
		private int subtime;
		private int enumDistance;
		private int enumDistance2;
		/// <summary>
		/// 重置
		/// </summary>
		public void Reset(int Left, int Right, int Step, int Subtime)
		{
			left = Left;
			right = Right;
			step = Step;
			subtime = Subtime;
			ReCalculate();
		}
		/// <summary>
		/// 重置
		/// </summary>
		public void Reset(int Length, int Subtime)
		{
			left = 0;
			right = Length;
			step = 1;
			subtime = Subtime;
			ReCalculate();
		}
		/// <summary>
		/// 重新计算，同时重置
		/// </summary>
		public void ReCalculate()
		{
			distance = (right - left) / step;
			enumedTime = 0;
			placei = left;
			enumDistance = distance / subtime;
			enumDistance2 = distance % subtime;
		}
		/// <summary>
		/// 枚举[0,Length)，1，分Subtime次
		/// </summary>
		public SubstepEnum(int Length,int Subtime)
		{
			Reset(Length, Subtime);
		}
		/// <summary>
		/// 枚举[Left,Right)，间隔Step，分Subtime次
		/// </summary>
		public SubstepEnum(int Left, int Right, int Step, int Subtime) {
			Reset(Left, Right, Step, Subtime);
		}
		private int GetEnumTime(int i) => enumDistance + ((i < enumDistance2) ? (1) : (0));
		/// <summary>
		/// 本次枚举的次数
		/// </summary>
		public int EnumTime
		{
			get => GetEnumTime(enumedTime);
		}
		/// <summary>
		/// 枚举区间
		/// </summary>
		public IEnumerable<int> Enum() {
			for (int i = 0; i < EnumTime; i+= 1) yield return i * step + placei;
		}
		/// <summary>
		/// 移动到下一个区间，如果到头，重置并返回true
		/// </summary>
		public bool Next() {
			placei += EnumTime*step;
			enumedTime += 1;
			if (enumedTime == subtime)
			{
				placei = left;
				enumedTime = 0;
				return true;
			}
			else return false;
		}
		/// <summary>
		/// 是否在开头
		/// </summary>
		public bool IsBeginning => enumedTime == 0;

		/// <summary>
		/// 已经进行的枚举次数
		/// </summary>
		public int EnumedTime => enumedTime;
		/// <summary>
		/// 枚举区间并移动
		/// </summary>
		public IEnumerator<int> GetEnumerator()
		{
			foreach (var i in Enum()) yield return i;
			Next();
		}
		/// <summary>
		/// 枚举区间并移动
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
