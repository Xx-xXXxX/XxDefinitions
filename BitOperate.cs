using System;
using System.Collections.Generic;
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

		public static string ToBitString(int data)
		{
			return ToBitString(IToBytes(data));
		}
		public static int FToIBit(float d)
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(d), 0);
		}
		public static float IToFBit(int d)
		{
			return BitConverter.ToSingle(BitConverter.GetBytes(d), 0);
		}
		public static byte[] FToBytes(float d)
		{
			return BitConverter.GetBytes(d);
		}
		public static byte[] IToBytes(int d)
		{
			return BitConverter.GetBytes(d);
		}
	}
	public class BitSeparator
	{
		public readonly int[] SeparateDistance;
		public readonly int[] SeparateIndex;
		public readonly RefByFunc<int> SeparatedNumber;
		public BitSeparator(RefByFunc<int> SeparatedNumber, int[] SeparateDistance) {
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
		public BitSeparator(RefByFunc<int> SeparatedNumber, int[] SeparateDistance,int[] SeparateIndex)
		{
			this.SeparatedNumber = SeparatedNumber;
			this.SeparateDistance = SeparateDistance;
			this.SeparateIndex = SeparateIndex;
		}
		public int Get(int index) {
			return BitOperate.GetBits(SeparatedNumber.Value,SeparateIndex[index], SeparateDistance[index]);
		}
		public int Set(int index, int value) {
			return SeparatedNumber.Value=BitOperate.SetBits(SeparatedNumber.Value, value, SeparateIndex[index], SeparateDistance[index]);
		}
		public int this[int index] {
			get => Get(index);
			set => Set(index, value);
		}
	}
	public class BitSeparatorFactory {
		public readonly int[] SeparateDistance;
		public readonly int[] SeparateIndex;
		public BitSeparatorFactory(int[] SeparateDistance)
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
		public BitSeparator Build(RefByFunc<int> I) {
			return new BitSeparator(I, SeparateDistance, SeparateIndex);
		}
	}
}
