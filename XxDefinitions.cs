using log4net;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using System;
using On.Terraria;

namespace XxDefinitions
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public class XxDefinitions : Mod
	{
        private static XxDefinitions instance;
        public static XxDefinitions Instance => instance;
        public XxDefinitions()
        {
            instance = this;
        }
        ~XxDefinitions() {
            LogManager.GetLogger("XxDefinitions").Debug($"~XxDefinitions {this.GetType().FullName}");
            GC.Collect(); GC.WaitForPendingFinalizers();
        }
        //public static Mod XDebugger;
        //public static Func<NPC, string> XDebuggerDefaultGetNPCDebugData;
        /*
		public override void PostSetupContent()
		{
			XDebugger = ModLoader.GetMod("XDebugger");
			if (XDebugger != null)
			{
				XDebugger.Call("AddGetNPCDebugDataFunc", ModContent.NPCType<Test.NPCs.E3____Hover>(), (Func<NPC, string>)Test.NPCs.E3____Hover.XDebuggerDebugF);
				XDebuggerDefaultGetNPCDebugData = (Func<NPC, string>)XDebugger.Call("GetDefaultNPCDebugDataFunc");
				Log.Debug($"E3____Hover.type:{ModContent.NPCType<Test.NPCs.E3____Hover>()}");
			}
		}*/
        public override void PostSetupContent()
        {
            if (XDebugger.XDebugger.DebugMode)
            {
                XDebugger.XDebugger.PostSetupContent();//XDebuggerDefaultGetNPCDebugData = (Func<NPC, string>)XDebugger.Call("GetDefaultNPCDebugDataFunc");
            }
        }
        public override void Load()
        {
            StaticRefHolder.Load();
            Logv1.Using = true;
            Logv1.Debug(mc.Value.name);
        }
        public override void Unload()
        {
            instance = null;
            Logv1 = null;
            //UnloadDo.Unload();
            WaitForRelease();
            XDebugger.XDebugger.CloseDebugMode();
            StaticRefHolder.Unload();
        }
        public void WaitForRelease() {
            XxDefinitions.Logv1.Debug("WaitForRelease pre GC.Collect()");
            GC.Collect(); GC.WaitForPendingFinalizers();
            XxDefinitions.Logv1.Debug("WaitForRelease post GC.Collect()");
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (XDebugger.XDebugger.DebugMode)
            {
                XDebugger.XDebugger.UpdateUI(gameTime);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (XDebugger.XDebugger.DebugMode)
            {
                XDebugger.XDebugger.ModifyInterfaceLayers(layers);
            }
        }
        public override object Call(params object[] args)
        {
            string CTypeS = (string)args[0];
            object[] Nargs = new object[args.Length - 1];
            for (int i = 1; i < args.Length; i++)
            {
                Nargs[i - 1] = args[i];
            }
            switch (CTypeS)
            {
                case "Debug":
                    if (XDebugger.XDebugger.DebugMode)
                    {
                        return XDebugger.XDebugger.Call(Nargs);
                    }
                    else
                        return null;
            }
            return null;
        }
        //      public class XLWA : CtorByF<LogWithUsing> {
        //          public override LogWithUsing F() => new LogWithUsing("XxDefinitions");
        //}
        //      public static StaticRef<XLWA> Log;
        private static LogWithUsing logv1;
        public static bool Uselogv1 = true;
        public static LogWithUsing Logv1{
            get { 
                if(logv1==null)logv1 = new LogWithUsing("XxDefinitions", Uselogv1);
                logv1.Using = Uselogv1;
                return logv1;
            }
            set => logv1 = value;
        }
        public static StaticRefWithFunc<MemoryCheck> mc = new StaticRefWithFunc<MemoryCheck>(()=>new MemoryCheck());
    }
}