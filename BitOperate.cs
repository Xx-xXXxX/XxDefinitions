using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	/// <summary>
	/// 位操作
	/// </summary>
	public static class BitOperate
	{
		/// <summary>
		/// Make 1 Bits
		/// example:MakeBit1s(4)==0xf(1111)
		/// </summary>
		/// <param name="s">Length of 1</param>
		/// <returns></returns>
		public static int MakeBit1s(int s)
		{
			return (int)((1 << (s)) - 1);
		}
		/// <summary>
		/// Sub bits at l to l+s
		/// <code>example:GetBits(101101110,2,3)
		///_101101110
		///_    [ ]
		///_    011(return)</code>
		/// </summary>
		/// <param name="d"></param>
		/// <param name="l"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static int GetBits(int d, int l, int s)
		{
			return ((d >> l) & MakeBit1s(s));
		}
		/// <summary>
		/// <code>Set bits in [l,l+s] by 0
		/// example:ClearBits(101101110,2,3)
		///_101101110
		///_    [ ]
		///_101100010(return)</code>
		/// </summary>
		/// <param name="d"></param>
		/// <param name="l"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static int ClearBits(int d, int l, int s)
		{
			return d & ~(MakeBit1s(s) << l);
		}
		/// <summary>
		/// <code>Set bits outside [l,l+s] by 0
		/// example:ClearBits(101101110,2,3)
		///_101101110
		///_    [ ]
		///_000001100(return)</code>
		/// </summary>
		/// <param name="d"></param>
		/// <param name="l"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static int ClearOutsideBits(int d, int l, int s)
		{
			return d & (MakeBit1s(s) << l);
		}
		/// <summary>
		/// <para>Set bits in [l,l+s] with v</para>
		/// <code>example:SetBits(101101110,1110,2,3)
		///_101001110
		///_    [ ]
		///_   1110
		///_101011010(return)</code>
		/// </summary>
		/// <param name="d"></param>
		/// <param name="v"></param>
		/// <param name="l"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static int SetBits(int d, int v, int l, int s)
		{
			d = ClearBits(d, l, s);
			v = ClearOutsideBits(v, 0, s);
			d |= v << l;
			return d;
		}
		/// <summary>
		/// Make string by bits
		/// Note that left is low
		/// example:ToBitString(IToBytes(0xf))
		/// 0xf(int)
		/// 11110000000000000000000000000000
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string ToBitString(byte[] data)
		{
			string R = "";
			for (int i = 0; i < data.Length; ++i)
			{
				for (int j = 0; j < 8; ++j)
				{
					R += (((data[i] >> j) & 1) == 1) ? '1' : '0';
				}
			}
			return R;
		}

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public static string ToBitString(int data)
		{
			return ToBitString(BitConverter.GetBytes(data));
		}
		[Obsolete]
		public static int FloatToIntBits(float d)
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(d), 0);
		}
		[Obsolete]
		public static float IntToFloatBits(int d)
		{
			return BitConverter.ToSingle(BitConverter.GetBytes(d), 0);
		}
		[Obsolete]
		public static long Int2ToLongBits(int d1, int d2) {
			return ((long)d1 << 32 )| (long)d2;
		}
		[Obsolete]
		public static uint IntToUIntBits(int d) {
			return BitConverter.ToUInt32(BitConverter.GetBytes(d),0);
		}
		[Obsolete]
		public static int IntToUIntBits(uint d)
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(d), 0);
		}
		public static int ToInt(float d) {
			return BitConverter.ToInt32(BitConverter.GetBytes(d), 0);
		}
		public static int ToInt(uint d)
		{
			return (int)d;
		}
		public static uint ToUInt(int d) {
			return (uint)d;
		}
		public static uint ToUInt(float d)
		{
			return BitConverter.ToUInt32(BitConverter.GetBytes(d), 0);
		}
		public static float ToFloat(int d) {
			return BitConverter.ToSingle(BitConverter.GetBytes(d),0);
		}
		public static float ToFloat(uint d)
		{
			return BitConverter.ToSingle(BitConverter.GetBytes(d), 0);
		}
		public static ulong ToULong(long d) {
			return (ulong)d;
		}
		public static ulong ToULong(double d)
		{
			return (ulong)BitConverter.DoubleToInt64Bits(d);
		}
		public static ulong ToULong(int d1,int d2)
		{
			byte[] vs=new byte[8];
			Array.Copy(BitConverter.GetBytes(d1), 0, vs, 0, 4);
			Array.Copy(BitConverter.GetBytes(d2), 0, vs, 4, 4);
			return BitConverter.ToUInt64(vs, 0);
		}
		public static ulong ToULong(float d1, float d2)
		{
			byte[] vs = new byte[8];
			Array.Copy(BitConverter.GetBytes(d1), 0, vs, 0, 4);
			Array.Copy(BitConverter.GetBytes(d2), 0, vs, 4, 4);
			return BitConverter.ToUInt64(vs, 0);
		}
		public static ulong ToULong(uint d1, uint d2)
		{
			byte[] vs = new byte[8];
			Array.Copy(BitConverter.GetBytes(d1), 0, vs, 0, 4);
			Array.Copy(BitConverter.GetBytes(d2), 0, vs, 4, 4);
			return BitConverter.ToUInt64(vs, 0);
		}
		public static long ToLong(ulong d)
		{
			return (long)d;
		}
		public static long ToLong(double d)
		{
			return BitConverter.DoubleToInt64Bits(d);
		}
		public static long ToLong(int d1, int d2)
		{
			byte[] vs = new byte[8];
			Array.Copy(BitConverter.GetBytes(d1), 0, vs, 0, 4);
			Array.Copy(BitConverter.GetBytes(d2), 0, vs, 4, 4);
			return BitConverter.ToInt64(vs, 0);
		}
		public static long ToLong(float d1, float d2)
		{
			byte[] vs = new byte[8];
			Array.Copy(BitConverter.GetBytes(d1), 0, vs, 0, 4);
			Array.Copy(BitConverter.GetBytes(d2), 0, vs, 4, 4);
			return BitConverter.ToInt64(vs, 0);
		}
		public static long ToLong(uint d1, uint d2)
		{
			byte[] vs = new byte[8];
			Array.Copy(BitConverter.GetBytes(d1), 0, vs, 0, 4);
			Array.Copy(BitConverter.GetBytes(d2), 0, vs, 4, 4);
			return BitConverter.ToInt64(vs, 0);
		}
		public static double ToDouble(long d) {
			return BitConverter.Int64BitsToDouble(d);
		}
		public static double ToDouble(ulong d)
		{
			return BitConverter.Int64BitsToDouble((long)d);
		}
		public static double ToDouble(int d1, int d2)
		{
			byte[] vs = new byte[8];
			Array.Copy(BitConverter.GetBytes(d1), 0, vs, 0, 4);
			Array.Copy(BitConverter.GetBytes(d2), 0, vs, 4, 4);
			return BitConverter.ToDouble(vs, 0);
		}
		public static double ToDouble(float d1, float d2)
		{
			byte[] vs = new byte[8];
			Array.Copy(BitConverter.GetBytes(d1), 0, vs, 0, 4);
			Array.Copy(BitConverter.GetBytes(d2), 0, vs, 4, 4);
			return BitConverter.ToDouble(vs, 0);
		}
		public static double ToDouble(uint d1, uint d2)
		{
			byte[] vs = new byte[8];
			Array.Copy(BitConverter.GetBytes(d1), 0, vs, 0, 4);
			Array.Copy(BitConverter.GetBytes(d2), 0, vs, 4, 4);
			return BitConverter.ToDouble(vs, 0);
		}
		public static (int, int) ToInt2(long d) {
			byte[] vs = BitConverter.GetBytes(d);
			return (BitConverter.ToInt32(vs, 0),BitConverter.ToInt32(vs,4));
		}
		public static (int, int) ToInt2(ulong d)
		{
			byte[] vs = BitConverter.GetBytes(d);
			return (BitConverter.ToInt32(vs, 0), BitConverter.ToInt32(vs, 4));
		}
		public static (int, int) ToInt2(double d)
		{
			byte[] vs = BitConverter.GetBytes(d);
			return (BitConverter.ToInt32(vs, 0), BitConverter.ToInt32(vs, 4));
		}
		public static (uint, uint) ToUInt2(long d)
		{
			byte[] vs = BitConverter.GetBytes(d);
			return (BitConverter.ToUInt32(vs, 0), BitConverter.ToUInt32(vs, 4));
		}
		public static (uint, uint) ToUInt2(ulong d)
		{
			byte[] vs = BitConverter.GetBytes(d);
			return (BitConverter.ToUInt32(vs, 0), BitConverter.ToUInt32(vs, 4));
		}
		public static (uint, uint) ToUInt2(double d)
		{
			byte[] vs = BitConverter.GetBytes(d);
			return (BitConverter.ToUInt32(vs, 0), BitConverter.ToUInt32(vs, 4));
		}
		public static (float, float) ToFloat2(long d)
		{
			byte[] vs = BitConverter.GetBytes(d);
			return (BitConverter.ToSingle(vs, 0), BitConverter.ToSingle(vs, 4));
		}
		public static (float, float) ToFloat2(ulong d)
		{
			byte[] vs = BitConverter.GetBytes(d);
			return (BitConverter.ToSingle(vs, 0), BitConverter.ToSingle(vs, 4));
		}
		public static (float, float) ToFloat2(double d)
		{
			byte[] vs = BitConverter.GetBytes(d);
			return (BitConverter.ToSingle(vs, 0), BitConverter.ToSingle(vs, 4));
		}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
	}
	/// <summary>
	/// 将int按位分离操作
	/// </summary>
	[DebuggerDisplay("[ToString()]")]
	public class BitSeparator
	{
		/// <summary>
		/// 显示所有分离结果值
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string I = Get(0).ToString();
			for (int i = 1; i < SeparateIndex.Length; ++i) {
				I += $",{Get(i)}";
			}
			return I;
		}
		/// <summary>
		/// 按位分离长度
		/// </summary>
		public readonly int[] SeparateDistance;
		/// <summary>
		/// 按位分离位置
		/// </summary>
		public readonly int[] SeparateIndex;
		/// <summary>
		/// 被分离的值
		/// </summary>
		public readonly IGetSetValue<int> SeparatedNumber;
		/// <summary>
		/// 初始化，自动生成SeparateIndex
		/// </summary>
		public BitSeparator(IGetSetValue<int> SeparatedNumber, params int[] SeparateDistance) {
			this.SeparatedNumber = SeparatedNumber;
			this.SeparateDistance = SeparateDistance;
			int n = 0;
			SeparateIndex = new int[SeparateDistance.Length];
			for (int i = 0; i < SeparateDistance.Length; ++i)
			{
				SeparateIndex[i] = n;
				n += SeparateDistance[i];
			}
		}
		/// <summary>
		/// 初始化
		/// </summary>
		public BitSeparator(IGetSetValue<int> SeparatedNumber, int[] SeparateDistance,int[] SeparateIndex)
		{
			this.SeparatedNumber = SeparatedNumber;
			this.SeparateDistance = SeparateDistance;
			this.SeparateIndex = SeparateIndex;
		}
		/// <summary>
		/// 获取分离的值
		/// </summary>
		public int Get(int index) {
			return BitOperate.GetBits(SeparatedNumber.Value,SeparateIndex[index], SeparateDistance[index]);
		}
		/// <summary>
		/// 设置分离的值
		/// </summary>
		public int Set(int index, int value) {
			return SeparatedNumber.Value=BitOperate.SetBits(SeparatedNumber.Value, value, SeparateIndex[index], SeparateDistance[index]);
		}
		/// <summary>
		/// 
		/// </summary>
		public int this[int index] {
			get => Get(index);
			set => Set(index, value);
		}
	}
	/// <summary>
	/// 按位分离的工厂
	/// </summary>
	public class BitSeparatorFactory
	{
		/// <summary>
		/// 按位分离长度
		/// </summary>
		public readonly int[] SeparateDistance;
		/// <summary>
		/// 按位分离位置
		/// </summary>
		public readonly int[] SeparateIndex;
		/// <summary>
		/// 初始化，自动生成SeparateIndex
		/// </summary>
		public BitSeparatorFactory(params int[] SeparateDistance)
		{
			this.SeparateDistance = SeparateDistance;
			int n = 0;
			SeparateIndex = new int[SeparateDistance.Length];
			for (int i = 0; i < SeparateDistance.Length; ++i)
			{
				SeparateIndex[i] = n;
				n += SeparateDistance[i];
			}
		}
		/// <summary>
		/// 创建BitSeparator
		/// </summary>
		public BitSeparator Build(IGetSetValue<int> I) {
			return new BitSeparator(I, SeparateDistance, SeparateIndex);
		}
		/// <summary>
		/// 获取分离的值
		/// </summary>
		public int Get(IGetSetValue<int> SeparatedNumber, int index)
		{
			return BitOperate.GetBits(SeparatedNumber.Value, SeparateIndex[index], SeparateDistance[index]);
		}
		/// <summary>
		/// 设置分离的值
		/// </summary>
		public int Set(IGetSetValue<int> SeparatedNumber,int index, int value)
		{
			return SeparatedNumber.Value = BitOperate.SetBits(SeparatedNumber.Value, value, SeparateIndex[index], SeparateDistance[index]);
		}
	}
	/// <summary>
	/// 将uint按值分离操作
	/// </summary>
	[DebuggerDisplay("[ToString()]")]
	public class UIntSeparator
	{
		/// <summary>
		/// 显示所有分离结果值
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string I = Get(0).ToString();
			for (int i = 1; i < SeparateIndex.Length; ++i)
			{
				I += $",{Get(i)}";
			}
			return I;
		}
		/// <summary>
		/// 分离长度
		/// </summary>
		public readonly uint[] SeparateDistance;
		/// <summary>
		/// 分离位置
		/// </summary>
		public readonly uint[] SeparateIndex;
		/// <summary>
		/// 被分离的值
		/// </summary>
		public readonly IGetSetValue<uint> SeparatedNumber;
		/// <summary>
		/// 初始化，自动生成SeparateIndex
		/// </summary>
		public UIntSeparator(IGetSetValue<uint> SeparatedNumber, params uint[] SeparateDistance)
		{
			this.SeparatedNumber = SeparatedNumber;
			this.SeparateDistance = SeparateDistance;
			uint n = 1;
			SeparateIndex = new uint[SeparateDistance.Length];
			for (uint i = 0; i < SeparateDistance.Length; ++i)
			{
				SeparateIndex[i] = n;
				n *= SeparateDistance[i];
			}
		}
		/// <summary>
		/// 初始化
		/// </summary>
		public UIntSeparator(IGetSetValue<uint> SeparatedNumber, uint[] SeparateDistance, uint[] SeparateIndex)
		{
			this.SeparatedNumber = SeparatedNumber;
			this.SeparateDistance = SeparateDistance;
			this.SeparateIndex = SeparateIndex;
		}
		/// <summary>
		/// 获取分离的值
		/// </summary>
		public uint Get(int index)
		{
			return (SeparatedNumber.Value/SeparateIndex[index])%SeparateDistance[index];
		}
		/// <summary>
		/// 设置分离的值
		/// </summary>
		public uint Set(int index, uint value)
		{
			Check(index, value);
			SeparatedNumber.Value -= ((SeparatedNumber.Value / SeparateIndex[index]) % SeparateDistance[index]) * SeparateIndex[index];
			SeparatedNumber.Value += value* SeparateIndex[index];
			return SeparatedNumber.Value;
		}
		/// <summary>
		/// 
		/// </summary>
		public uint this[int index]
		{
			get => Get(index);
			set => Set(index, value);
		}/// <summary>
		 /// 检查值是否在范围内
		 /// </summary>
		public void Check(int index, uint value)
		{
			if (value < 0 || value >= SeparateDistance[index]) throw new ArgumentOutOfRangeException("value", $"value:{value} 不在index:{index} 范围 [0,{SeparateDistance[index]})内");
		}
	}
	/// <summary>
	/// uint按值分离的工厂
	/// </summary>
	public class UIntSeparatorFactory
	{
		/// <summary>
		/// 分离长度
		/// </summary>
		public readonly uint[] SeparateDistance;
		/// <summary>
		/// 分离位置
		/// </summary>
		public readonly uint[] SeparateIndex;
		/// <summary>
		/// 初始化，自动生成SeparateIndex
		/// </summary>
		public UIntSeparatorFactory(params uint[] SeparateDistance)
		{
			this.SeparateDistance = SeparateDistance;
			uint n = 1;
			SeparateIndex = new uint[SeparateDistance.Length];
			for (uint i = 0; i < SeparateDistance.Length; ++i)
			{
				SeparateIndex[i] = n;
				n *= SeparateDistance[i];
			}
		}
		/// <summary>
		/// 创建UIntSeparator
		/// </summary>
		public UIntSeparator Build(IGetSetValue<uint> I)
		{
			return new UIntSeparator(I, SeparateDistance, SeparateIndex);
		}
		/// <summary>
		/// 获取分离的值
		/// </summary>
		public uint Get(IGetValue<uint> SeparatedNumber, int index)
		{
			return (SeparatedNumber.Value / SeparateIndex[index]) % SeparateDistance[index];
		}
		/// <summary>
		/// 获取分离的值
		/// </summary>
		public uint Get(uint SeparatedNumber, int index)
		{
			return (SeparatedNumber / SeparateIndex[index]) % SeparateDistance[index];
		}
		/// <summary>
		/// 设置分离的值
		/// </summary>
		public uint Set(IGetSetValue<uint> SeparatedNumber, int index, uint value)
		{
			Check(index, value);
			SeparatedNumber.Value -= ((SeparatedNumber.Value / SeparateIndex[index]) % SeparateDistance[index]) * SeparateIndex[index];
			SeparatedNumber.Value += value * SeparateIndex[index];
			return SeparatedNumber.Value;
		}
		/// <summary>
		/// 设置分离的值
		/// </summary>
		public uint Set(ref uint SeparatedNumber, int index, uint value)
		{
			Check(index, value);
			SeparatedNumber -= ((SeparatedNumber / SeparateIndex[index]) % SeparateDistance[index]) * SeparateIndex[index];
			SeparatedNumber += value * SeparateIndex[index];
			return SeparatedNumber;
		}
		/// <summary>
		/// 设置分离的值
		/// </summary>
		public uint Set(uint SeparatedNumber, int index, uint value)
		{
			Check(index,value);
			SeparatedNumber -= ((SeparatedNumber / SeparateIndex[index]) % SeparateDistance[index]) * SeparateIndex[index];
			SeparatedNumber += value * SeparateIndex[index];
			return SeparatedNumber;
		}

		/// <summary>
		/// 检查值是否在范围内
		/// </summary>
		public void Check(int index, uint value)
		{
			if (value < 0 || value >= SeparateDistance[index]) throw new ArgumentOutOfRangeException("value", $"value:{value} 不在index:{index} 范围 [0,{SeparateDistance[index]})内");
		}
	}
	/// <summary>
	/// 将int按值分离操作
	/// 但不会有负数，设置为负数会出错
	/// </summary>
	[DebuggerDisplay("[ToString()]")]
	public class IntSeparator
	{
		/// <summary>
		/// 显示所有分离结果值
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string I = Get(0).ToString();
			for (int i = 1; i < SeparateIndex.Length; ++i)
			{
				I += $",{Get(i)}";
			}
			return I;
		}
		/// <summary>
		/// 分离长度
		/// </summary>
		public readonly int[] SeparateDistance;
		/// <summary>
		/// 分离位置
		/// </summary>
		public readonly int[] SeparateIndex;
		/// <summary>
		/// 被分离的值
		/// </summary>
		public readonly IGetSetValue<int> SeparatedNumber;
		/// <summary>
		/// 初始化，自动生成SeparateIndex
		/// </summary>
		public IntSeparator(IGetSetValue<int> SeparatedNumber, params int[] SeparateDistance)
		{
			this.SeparatedNumber = SeparatedNumber;
			this.SeparateDistance = SeparateDistance;
			int n = 1;
			SeparateIndex = new int[SeparateDistance.Length];
			for (int i = 0; i < SeparateDistance.Length; ++i)
			{
				SeparateIndex[i] = n;
				n *= SeparateDistance[i];
			}
		}
		/// <summary>
		/// 初始化
		/// </summary>
		public IntSeparator(IGetSetValue<int> SeparatedNumber, int[] SeparateDistance, int[] SeparateIndex)
		{
			this.SeparatedNumber = SeparatedNumber;
			this.SeparateDistance = SeparateDistance;
			this.SeparateIndex = SeparateIndex;
		}
		/// <summary>
		/// 获取分离的值
		/// </summary>
		public int Get(int index)
		{
			return (int)(((uint)SeparatedNumber.Value / (uint)SeparateIndex[index]) % (uint)SeparateDistance[index]);
		}
		/// <summary>
		/// 设置分离的值，使用负数会报错
		/// </summary>
		public int Set(int index, int value)
		{
			Check(index, value);
			SeparatedNumber.Value -= ((SeparatedNumber.Value / SeparateIndex[index]) % SeparateDistance[index]) * SeparateIndex[index];
			SeparatedNumber.Value += value * SeparateIndex[index];
			return SeparatedNumber.Value;
		}
		/// <summary>
		/// 
		/// </summary>
		public int this[int index]
		{
			get => Get(index);
			set => Set(index, value);
		}
		/// <summary>
		/// 检查值是否在范围内
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Check(int index, int value)
		{
			if (value < 0 || value >= SeparateDistance[index]) throw new ArgumentOutOfRangeException("value", $"value:{value} 不在index:{index} 范围 [0,{SeparateDistance[index]})内");
		}
	}
	/// <summary>
	/// int按值分离的工厂
	/// 但不会有负数，设置为负数会出错
	/// </summary>
	public class IntSeparatorFactory
	{
		/// <summary>
		/// 分离长度
		/// </summary>
		public readonly int[] SeparateDistance;
		/// <summary>
		/// 分离位置
		/// </summary>
		public readonly int[] SeparateIndex;
		/// <summary>
		/// 初始化，自动生成SeparateIndex
		/// </summary>
		public IntSeparatorFactory(params int[] SeparateDistance)
		{
			this.SeparateDistance = SeparateDistance;
			int n = 1;
			SeparateIndex = new int[SeparateDistance.Length];
			for (int i = 0; i < SeparateDistance.Length; ++i)
			{
				SeparateIndex[i] = n;
				n *= SeparateDistance[i];
			}
		}
		/// <summary>
		/// 创建IntSeparator
		/// </summary>
		public IntSeparator Build(IGetSetValue<int> I)
		{
			return new IntSeparator(I, SeparateDistance, SeparateIndex);
		}
		/// <summary>
		/// 获取分离的值
		/// </summary>
		public int Get(IGetValue<int> SeparatedNumber, int index)
		{
			return (int)(((uint)SeparatedNumber.Value / (uint)SeparateIndex[index]) % (uint)SeparateDistance[index]);
		}
		/// <summary>
		/// 获取分离的值
		/// </summary>
		public int Get(int SeparatedNumber, int index)
		{
			return (int)(((uint)SeparatedNumber / (uint)SeparateIndex[index]) % (uint)SeparateDistance[index]);
		}
		/// <summary>
		/// 检查值是否在范围内
		/// </summary>
		public void Check(int index, int value) {
			if (value < 0 || value >= SeparateDistance[index]) throw new ArgumentOutOfRangeException("value",$"value:{value} 不在index:{index} 范围 [0,{SeparateDistance[index]})内");
		}
		/// <summary>
		/// 设置分离的值
		/// </summary>
		public int Set(IGetSetValue<int> SeparatedNumber, int index, int value)
		{
			Check(index, value);
			SeparatedNumber.Value -= ((SeparatedNumber.Value / SeparateIndex[index]) % SeparateDistance[index]) * SeparateIndex[index];
			SeparatedNumber.Value += value * SeparateIndex[index];
			return SeparatedNumber.Value;
		}
		/// <summary>
		/// 设置分离的值
		/// </summary>
		public int Set(ref int SeparatedNumber, int index, int value)
		{
			Check(index, value);
			SeparatedNumber -= ((SeparatedNumber / SeparateIndex[index]) % SeparateDistance[index]) * SeparateIndex[index];
			SeparatedNumber += value * SeparateIndex[index];
			return SeparatedNumber;
		}
		/// <summary>
		/// 设置分离的值
		/// </summary>
		public int Set(int SeparatedNumber, int index, int value)
		{
			Check(index, value);
			SeparatedNumber -= ((SeparatedNumber / SeparateIndex[index]) % SeparateDistance[index])* SeparateIndex[index];
			SeparatedNumber += value * SeparateIndex[index];
			return SeparatedNumber;
		}
	}
}
