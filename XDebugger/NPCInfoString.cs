using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions.XDebugger
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class NPCInfoString: XDebuggerRequires
	{
		public NPCInfoString(string XDebuggerFullName, string MethodName):base(XDebuggerFullName,MethodName)
		{ }
	}
}
