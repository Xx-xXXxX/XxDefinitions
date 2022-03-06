﻿using System;
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

namespace XxDefinitions.XDebugger
{
	public class XDebugger
	{
		private static bool Loaded = false;
		private static bool debugMode = false;
		public static void CloseDebugMode() {
			debugMode = false;
			Unload();
		}
		public static bool DebugMode {
			get => debugMode;
			set { debugMode = debugMode || value; Load(); }
		}
		public static void Update() { 
			
		}
		public static void PostSetupContent() {
			//Utils.AddGetNPCDebugDataFunc(ModContent.NPCType<Test.NPCs.E3____Hover>(), (Func<NPC, string>)Test.NPCs.E3____Hover.XDebuggerDebugF);
		}
		internal static UI.CustomDraw customDraw;
		internal static UserInterface customDrawInterface;
		public static void Load() {
			if (Loaded) return;
			Loaded = true;
				customDraw = new UI.CustomDraw();
				customDraw.Activate();
				customDrawInterface = new UserInterface();
				customDrawInterface.SetState(customDraw);
			UI.CustomDraw.DrawerList = new List<CustomDraw.IDrawer>();
		}
		public static void Unload() {
			if (!Loaded) return;
			Loaded = false;
			customDraw = null;
			customDrawInterface = null;
			UI.CustomDraw.DrawerList = null;
		}
		public static void UpdateUI(GameTime gameTime)
		{
			if (UI.CustomDraw.Visible)
			{
				customDrawInterface?.Update(gameTime);
			}
		}
		public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
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
		public static object Call(params object[] args)
		{
			string CTypeS = (string)args[0];
			switch (CTypeS)
			{
				case "AddGetNPCDebugDataFunc":
					//npctype,func
					if (NPCStringData.GetNPCDebugDataFuncs.ContainsKey((int)args[1]))
						NPCStringData.GetNPCDebugDataFuncs[(int)args[1]] = (Func<NPC, string>)args[2];
					else
						NPCStringData.GetNPCDebugDataFuncs.Add((int)args[1], (Func<NPC, string>)args[2]);
					return true;
					break;
				case "GetNPCDebugDataFuncDef":
					return (Func<NPC, string>)NPCStringData.GetNPCDebugDataDef;
					break;

				case "AddShowNPCDebugFunc":
					//npctype,func
					if (NPCStringData.ShowNPCDebugFuncs.ContainsKey((int)args[1]))
						NPCStringData.ShowNPCDebugFuncs[(int)args[1]] = (Action<NPC>)args[2];
					else
						NPCStringData.ShowNPCDebugFuncs.Add((int)args[1], (Action<NPC>)args[2]);
					return true;
					break;
				case "GetShowNPCDebugFuncDef":
					return (Action<NPC>)NPCStringData.ShowNPCDebugDef;
					break;
				case "GetAddDrawFunc":
					return (Action<Action<SpriteBatch>>)Utils.AddDraw.AddDrawFunc;
					break;
			}
			return null;
		}
	}
}