using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	/// <summary>
	/// 在Unload时自动执行
	/// </summary>
	public static class UnloadDo
	{
		private static List<Action> actions;
		private static List<Action> Actions {
			get { if (actions == null) actions = new List<Action>();return actions; }
		}
		/// <summary>
		/// 加入方法
		/// </summary>
		public static void Add(Action action) => Actions.Add(action);
		internal static void Unload() {
			if (actions == null) return;
			foreach (var i in actions) i?.Invoke();
			actions = null;
		}
	}
}