﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace XxDefinitions.XDebugger
{
	//[XDebuggerInfo()]
	/// <summary><![CDATA[
	/// 用于显示ProjDebugInfo，调用ModProjectile[ModProjInfoString]与GlobalProjectile[GlobalProjInfoString]]]>
	/// </summary>
	[GlobalProjInfoString("XDebugger.XDebugger", "DefShowProjDebugInfo")]

	public class ShowProjDebugInfo:GlobalProjectile
	{

		/// <summary>
		/// 设为true使ShowNPCDebugInfo总是显示
		/// DrawTimeLeft=1;
		/// </summary>
		public static bool ShowAlways = false;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public static Action<Projectile, List<(string, string)>> DefShowProjDebugInfo => (proj, l) =>
		{
			l.Add(("DefShowProjDebugInfo",
				$"Type:{proj.type} aiStyle:{proj.aiStyle}\n" +
				$"ais:{proj.ai[0]},{proj.ai[1]}\n" +
				$"localAIs:{proj.localAI[0]},{proj.localAI[1]}\n" +
				$"friendly:{proj.friendly} hostile:{proj.hostile} trap:{proj.trap}\n"
				));
		};

		public override bool InstancePerEntity => true;
		List<Action<Projectile, List<(string, string)>>> StringActions = new List<Action<Projectile, List<(string, string)>>>();
		List<TryGetXDebugger> StringActionsUsing = new List<TryGetXDebugger>();
		List<Func<Projectile, SpriteBatch, bool>> DrawActions = new List<Func<Projectile, SpriteBatch, bool>>();
		List<TryGetXDebugger> DrawActionsUsing = new List<TryGetXDebugger>();
		public override void SetDefaults(Projectile proj)
		{
			if (proj == null) return;
			StringActions.Clear();//解决CloneDefaults重复
			StringActionsUsing.Clear();
			DrawActions.Clear();
			DrawActionsUsing.Clear();
			if (proj.modProjectile != null)
			{
				System.Reflection.MemberInfo memberInfo = proj.modProjectile.GetType();
				List<ModProjInfoString> xDebuggerModNPCInfos = new List<ModProjInfoString>((ModProjInfoString[])memberInfo.GetCustomAttributes(typeof(ModProjInfoString), true));
				if (xDebuggerModNPCInfos.Count > 0)
				{
					foreach (var i in xDebuggerModNPCInfos)
					{
						Action<List<(string, string)>> action = i.GetInfoStringMethod(proj.modProjectile);
						if (action != null)
						{
							StringActions.Add((n, l) => { action.Invoke(l); });
							StringActionsUsing.Add(i.tryGetXDebugger);
						}
					}
				}

				List<ModProjInfoDraw> xDebuggerModNPCDraw = new List<ModProjInfoDraw>((ModProjInfoDraw[])memberInfo.GetCustomAttributes(typeof(ModProjInfoDraw), true));
				if (xDebuggerModNPCDraw.Count > 0)
				{
					foreach (var i in xDebuggerModNPCDraw)
					{
						Func<SpriteBatch, bool> action = i.GetInfoStringMethod(proj.modProjectile);
						if (action != null)
						{
							DrawActions.Add((n, sb) => { return action(sb); });
							DrawActionsUsing.Add(i.tryGetXDebugger);
						}
					}
				}
			}
			List<GlobalProjInfoString> xDebuggerGlobalNPCInfos;
			foreach (var j in globalProjs)
			{
				xDebuggerGlobalNPCInfos = new List<GlobalProjInfoString>((GlobalProjInfoString[])j.Instance(proj).GetType().GetCustomAttributes(typeof(GlobalProjInfoString), true));
				if (xDebuggerGlobalNPCInfos.Count > 0)
				{
					foreach (var i in xDebuggerGlobalNPCInfos)
					{
						Action<Projectile, List<(string, string)>> action = i.GetInfoStringMethod(j);
						if (action != null)
						{
							StringActions.Add(action);
							StringActionsUsing.Add(i.tryGetXDebugger);
						}
					}
				}
			}


			List<GlobalProjInfoDraw> xDebuggerGlobalNPCDraws;
			foreach (var j in globalProjs)
			{
				xDebuggerGlobalNPCDraws = new List<GlobalProjInfoDraw>((GlobalProjInfoDraw[])j.Instance(proj).GetType().GetCustomAttributes(typeof(GlobalProjInfoDraw), true));
				if (xDebuggerGlobalNPCDraws.Count > 0)
				{
					foreach (var i in xDebuggerGlobalNPCDraws)
					{
						Func<Projectile, SpriteBatch, bool> action = i.GetInfoStringMethod(j);
						if (action != null)
						{
							DrawActions.Add(action);
							DrawActionsUsing.Add(i.tryGetXDebugger);
						}
					}
				}
			}
		}
		internal static IList<GlobalProjectile> globalProjs;
		static ShowProjDebugInfo()
		{
			Type type = typeof(Terraria.ModLoader.ProjectileLoader);
			var fieldinfo = type.GetField("globalProjectiles", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
			globalProjs = (IList<GlobalProjectile>)fieldinfo.GetValue(null);
		}


		public int DrawTimeLeft = 0;
		public override void PostAI(Projectile proj)
		{
			if (DrawTimeLeft > 0) DrawTimeLeft -= 1;
		}
		public override void PostDraw(Projectile proj, SpriteBatch spriteBatch, Color drawColor)
		{
			if (!XDebugger.DebugMode) return;
			if (ShowAlways) DrawTimeLeft = 1;
			if (DrawTimeLeft <= 0) return;
			bool ShowString = true;
			for (int i = 0; i < DrawActions.Count; i++)
			{
				if (DrawActionsUsing[i].XDebuggerMode == 2)
				{
					ShowString = ShowString && DrawActions[i](proj, spriteBatch);
				}
			}

			Vector2 Pos = proj.position + new Vector2(proj.width, proj.height / 2) - Main.screenPosition;
			List<(string, string)> tooltips = new List<(string, string)>();
			for (int i = 0; i < StringActions.Count; ++i)
			{
				if (StringActionsUsing[i].XDebuggerMode == 2)
				{
					StringActions[i](proj, tooltips);
				}
			}
			string info = "";
			foreach (var i in tooltips)
			{
				info += i.Item2;
			}
			Pos -= new Vector2(0, Main.fontMouseText.MeasureString(info).Y / 2f);
			Terraria.Utils.DrawBorderString(spriteBatch, info, Pos, Color.White);
		}
	}
}
