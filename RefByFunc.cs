using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	public class RefByFunc<T>
	{
		public readonly Action<T> SetF;
		public readonly Func<T> GetF;
		public RefByFunc(Action<T> SetF, Func<T> GetF) {
			this.GetF = GetF;
			this.SetF = SetF;
		}
		public T Value {
			get => GetF();
			set => SetF(value);
		}
	}
}
