using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using log4net;
using log4net.Core;

namespace XxDefinitions
{
	public class LogWithUsing : ILog
	{
		private ILog log;
		public bool Using = false;

		public bool IsDebugEnabled => log.IsDebugEnabled;

		public bool IsInfoEnabled => log.IsInfoEnabled;

		public bool IsWarnEnabled => log.IsWarnEnabled;

		public bool IsErrorEnabled => log.IsErrorEnabled;

		public bool IsFatalEnabled => log.IsFatalEnabled;

		public ILogger Logger => log.Logger;

		public LogWithUsing(string Name, bool Using = false)
		{
			log = LogManager.GetLogger(Name);
			this.Using = Using;
		}
		public void Debug(object message)
		{
			if (Using)
				log.Debug(message);
		}

		public void Debug(object message, Exception exception)
		{
			if (Using)
				log.Debug(message, exception);
		}

		public void DebugFormat(string format, params object[] args)
		{
			if (Using)
				log.DebugFormat(format, args);
		}

		public void DebugFormat(string format, object arg0)
		{
			if (Using)
				log.DebugFormat(format, arg0);
		}

		public void DebugFormat(string format, object arg0, object arg1)
		{
			if (Using)
				log.DebugFormat(format, arg0, arg1);
		}

		public void DebugFormat(string format, object arg0, object arg1, object arg2)
		{
			if (Using)
				log.DebugFormat(format, arg0, arg1, arg2);
		}

		public void DebugFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (Using)
				log.DebugFormat(provider, format, args);
		}

		public void Info(object message)
		{
			if (Using)
				log.Info(message);
		}

		public void Info(object message, Exception exception)
		{
			if (Using)
				log.Info(message, exception);
		}

		public void InfoFormat(string format, params object[] args)
		{
			if (Using)
				log.InfoFormat(format, args);
		}

		public void InfoFormat(string format, object arg0)
		{
			if (Using)
				log.InfoFormat(format, arg0);
		}

		public void InfoFormat(string format, object arg0, object arg1)
		{
			if (Using)
				log.InfoFormat(format, arg0, arg1);
		}

		public void InfoFormat(string format, object arg0, object arg1, object arg2)
		{
			if (Using)
				log.InfoFormat(format, arg0, arg1, arg2);
		}

		public void InfoFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (Using)
				log.InfoFormat(provider, format, args);
		}

		public void Warn(object message)
		{
			if (Using)
				log.Warn(message);
		}

		public void Warn(object message, Exception exception)
		{
			if (Using)
				log.Warn(message, exception);
		}

		public void WarnFormat(string format, params object[] args)
		{
			if (Using)
				log.WarnFormat(format, args);
		}

		public void WarnFormat(string format, object arg0)
		{
			if (Using)
				log.WarnFormat(format, arg0);
		}

		public void WarnFormat(string format, object arg0, object arg1)
		{
			if (Using)
				log.WarnFormat(format, arg0, arg1);
		}

		public void WarnFormat(string format, object arg0, object arg1, object arg2)
		{
			if (Using)
				log.WarnFormat(format, arg0, arg1, arg2);
		}

		public void WarnFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (Using)
				log.WarnFormat(provider, format, args);
		}

		public void Error(object message)
		{
			if (Using)
				log.Error(message);
		}

		public void Error(object message, Exception exception)
		{
			if (Using)
				log.Error(message, exception);
		}

		public void ErrorFormat(string format, params object[] args)
		{
			if (Using)
				log.ErrorFormat(format, args);
		}

		public void ErrorFormat(string format, object arg0)
		{
			if (Using)
				log.ErrorFormat(format, arg0);
		}

		public void ErrorFormat(string format, object arg0, object arg1)
		{
			if (Using)
				log.ErrorFormat(format, arg0, arg1);
		}

		public void ErrorFormat(string format, object arg0, object arg1, object arg2)
		{
			if (Using)
				log.ErrorFormat(format, arg0, arg1, arg2);
		}

		public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (Using)
				log.ErrorFormat(provider, format, args);
		}

		public void Fatal(object message)
		{
			if (Using)
				log.Fatal(message);
		}

		public void Fatal(object message, Exception exception)
		{
			if (Using)
				log.Fatal(message, exception);
		}

		public void FatalFormat(string format, params object[] args)
		{
			if (Using)
				log.FatalFormat(format, args);
		}

		public void FatalFormat(string format, object arg0)
		{
			if (Using)
				log.FatalFormat(format, arg0);
		}

		public void FatalFormat(string format, object arg0, object arg1)
		{
			if (Using)
				log.FatalFormat(format, arg0, arg1);
		}

		public void FatalFormat(string format, object arg0, object arg1, object arg2)
		{
			if (Using)
				log.FatalFormat(format, arg0, arg1, arg2);
		}

		public void FatalFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (Using)
				log.FatalFormat(provider, format, args);
		}
	}
}
