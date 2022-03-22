using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XxDefinitions.XDebugger.UI;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace XxDefinitions.XDebugger
{
	/// <summary>
	/// 用于调试
	/// </summary>
	public partial class XDebugger
	{
		/// <summary>
		/// 用于默认操作的Xdebugger
		/// </summary>
		public static StaticRefWithFunc<XDebugger> DefXDebugger = new StaticRefWithFunc<XDebugger>(()=>new XDebugger("XDebugger.XDebugger",false,true));
		/// <summary>
		/// 所有公开XDebugger的
		/// </summary>
		public static StaticRefWithNew<List<TryGetXDebugger>> AnnouncedDebuggers=new StaticRefWithNew<List<TryGetXDebugger>>();
		private static bool Loaded = false;
		private static bool debugMode = false;
		/// <summary>
		/// 关闭调试
		/// </summary>
		public static void CloseDebugMode() {
			debugMode = false;
			EndDebugMode();
		}
		/// <summary>
		/// 开关调试，控制所有XDebugger
		/// 只要有DebugMode=true 都会开启
		/// </summary>
		public static bool DebugMode {
			get => debugMode;
			set {
				if (!debugMode && value) StartDebugMode();
				debugMode = debugMode || value;
			}
		}
		private static void StartDebugMode() {
			CustomDraw.DrawerList?.Clear();
		}
		private static void EndDebugMode() {
			CustomDraw.DrawerList?.Clear();
		}
		internal static void Update() {
		}
		internal static void PostSetupContent() {
			//Utils.AddGetNPCDebugDataFunc(ModContent.NPCType<Test.NPCs.E3____Hover>(), (Func<NPC, string>)Test.NPCs.E3____Hover.XDebuggerDebugF);
			//foreach (var i in Terraria.ModLoader.ModLoader.Mods)
			//{
			//	List<AnnouncedXDebugger> announcedXDebuggers = new List<AnnouncedXDebugger>(i.GetType().GetCustomAttributes<AnnouncedXDebugger>());
			//	foreach (var j in announcedXDebuggers)
			//	{
			//		AnnouncedDebuggers.Value.Add(j.tryGetXDebugger);
			//	}
			//}
			//foreach (var i in xDebuggers.Value) { 
			//	if(i.Value.GetType().GetCustomAttribute<AnnounceAttribute>()!=null)
			//		AnnouncedDebuggers.Value.Add(TryGetXDebugger.GetTryGetXDebugger(i.Value.FullName));
			//}
		}
		internal static UI.CustomDraw customDraw;
		internal static UserInterface customDrawInterface;
		internal static void Load() {
			if (Loaded) return;
			Loaded = true;
				customDraw = new UI.CustomDraw();
				customDraw.Activate();
				customDrawInterface = new UserInterface();
				customDrawInterface.SetState(customDraw);
			UI.CustomDraw.DrawerList = new List<CustomDraw.IDrawer>();

			DefXDebugger.Value.Using = true;
		}
		internal static void Unload() {
			if (!Loaded) return;
			Loaded = false;
			customDraw = null;
			customDrawInterface = null;
			UI.CustomDraw.DrawerList = null;
		}
		internal static void UpdateUI(GameTime gameTime)
		{
			if (UI.CustomDraw.Visible)
			{
				customDrawInterface?.Update(gameTime);
			}
		}
		internal static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int InventoryTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (InventoryTextIndex != -1)
			{
				layers.Insert(InventoryTextIndex, new LegacyGameInterfaceLayer(
				"XxDefinitions.XDebugger : customDraw",
				 delegate
				{
					if (UI.CustomDraw.Visible)
					customDrawInterface.Draw(Main.spriteBatch, new GameTime());
					return true;
				},
		   InterfaceScaleType.UI)
				);
			}
			
		}
		/// <summary>
		/// 弱引用操作
		/// </summary>
		public static object Call(params object[] args)
		{
			string CTypeS = (string)args[0];
			//switch (CTypeS)
			//{
				
			//}
			return null;
		}
		//static List<XDebugger> xDebuggers=new List<XDebugger>();
		internal static StaticRefWithFunc<Dictionary<string,XDebugger>> xDebuggers = new StaticRefWithFunc<Dictionary<string, XDebugger>>(()=> new Dictionary<string, XDebugger>());
		/// <summary>
		/// 是否使用，如果为false，所有与之相关的操作都将无效
		/// </summary>
		public bool Using=false;
		/// <summary>
		/// 名字
		/// </summary>
		public readonly string FullName;
		/// <summary>
		/// 为mod生成XDebugger
		/// 其全名为 mod.Name+"."+ Name
		/// </summary>
		public XDebugger(Mod mod,string Name, bool Using = false,bool Announce=false) {
			this.FullName = mod.Name+"."+ Name;
			this.Using = Using;
			xDebuggers.Value.Add(FullName, this);
			if (TryGetXDebugger.items.Value.TryGetValue(FullName, out TryGetXDebugger tryGetXDebugger)) {
				tryGetXDebugger.xDebugger_ = this;
			}
			if (Announce) {
				AnnouncedDebuggers.Value.Add(TryGetXDebugger.GetTryGetXDebugger(FullName));
			}
		}
		/// <summary>
		/// 生成XDebugger
		/// 不建议使用
		/// </summary>
		public XDebugger(string FullName, bool Using = false, bool Announce = false)
		{
			this.FullName = FullName;
			this.Using = Using;
			xDebuggers.Value.Add(this.FullName, this);
			if (TryGetXDebugger.items.Value.TryGetValue(this.FullName, out TryGetXDebugger tryGetXDebugger))
			{
				tryGetXDebugger.xDebugger_ = this;
			}
			if (Announce)
			{
				AnnouncedDebuggers.Value.Add(TryGetXDebugger.GetTryGetXDebugger(this.FullName));
			}
		}

		/// <summary>
		/// 获取对应XDebugger
		/// </summary>
		public static XDebugger GetXDebugger(Mod mod, string Name) {
			return xDebuggers.Value[mod.Name + "." + Name];
		}
		/// <summary>
		/// 获取对应XDebugger
		/// </summary>
		public static XDebugger GetXDebugger(string FullName) {
			return xDebuggers.Value[FullName];
			
		}
		/// <summary>
		/// 使得可以直接用if(xDebugger){...}
		/// </summary>
		/// <param name="xDebugger"></param>
		public static implicit operator bool(XDebugger xDebugger) => xDebugger.Using;
		public static TryGetXDebugger GetTryGetXDebugger(string FullName) => TryGetXDebugger.GetTryGetXDebugger(FullName);
	}
	/// <summary>
	/// 尝试获取XDebugger，在生成XDebugger时自动获取
	/// </summary>
	public class TryGetXDebugger {
		/// <summary>
		/// XDebugger的全名
		/// </summary>
		public readonly string FullName;
		internal XDebugger xDebugger_;
		/// <summary>
		/// 目标XDebugger
		/// </summary>
		public XDebugger xDebugger { get => xDebugger_; }
		internal static StaticRefWithNew< Dictionary<string,TryGetXDebugger>> items=new StaticRefWithNew<Dictionary<string, TryGetXDebugger>>();
		/// <summary>
		/// 获取尝试获取XDebugger名为FullName的TryGetXDebugger
		/// </summary>
		public static TryGetXDebugger GetTryGetXDebugger(string FullName) {
			TryGetXDebugger tryGetXDebugger;
			if (items.Value.TryGetValue(FullName,out tryGetXDebugger)) return tryGetXDebugger;
			else return new TryGetXDebugger(FullName);
		}
		internal TryGetXDebugger(string FullName) {
			this.FullName = FullName;
			XDebugger.xDebuggers.Value.TryGetValue(FullName, out xDebugger_);
			items.Value.Add(FullName, this);
		}
		/// <summary>
		/// 如果xDebugger不存在，返回0；如果xDebugger禁用，返回1；否则返回2；
		/// </summary>
		public int XDebuggerMode { get {
				if (xDebugger == null) return 0;
				if (xDebugger.Using == false) return 1;
				else return 2;
			} }
		public static implicit operator bool(TryGetXDebugger xDebugger) => xDebugger.XDebuggerMode==2;
	}
}
