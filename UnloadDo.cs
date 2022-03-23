using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions
{
	public static class UnloadDo
	{
		private static List<Action> actions;
		private static List<Action> Actions {
			get { if (actions == null) actions = new List<Action>();return actions; }
		}
		public static void Add(Action action) => Actions.Add(action);
		internal static void Unload() {
			if (actions == null) return;
			foreach (var i in actions) i?.Invoke();
			actions = null;
		}
	}
}