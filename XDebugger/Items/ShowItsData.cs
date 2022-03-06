using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using XxDefinitions.XDebugger.Buffs;

namespace XxDefinitions.XDebugger.Items
{
	public class ShowItsData : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Hold to see Its data!");
		}

		public override void SetDefaults()
		{
			item.holdStyle = 4;
			item.width = 40;
			item.height = 40;
			item.value = 1;
			item.rare = ItemRarityID.Green;
		}

		public override void HoldStyle(Player player)
		{
			Vector2 position = GetLightPosition(player);
			if (position.Y >= player.Center.Y == (player.gravDir == 1))
			{
				player.itemLocation.X = player.Center.X + 6f * player.direction;
				player.itemLocation.Y = player.position.Y + 21f + 23f * player.gravDir + player.mount.PlayerOffsetHitbox;
			}
			else
			{
				player.itemLocation.X = player.Center.X;
				player.itemLocation.Y = player.position.Y + 21f - 3f * player.gravDir + player.mount.PlayerOffsetHitbox;
			}
			player.itemRotation = 0f;
		}

		public override bool HoldItemFrame(Player player)
		{
			Vector2 position = GetLightPosition(player);
			if (position.Y >= player.Center.Y == (player.gravDir == 1))
			{
				player.bodyFrame.Y = player.bodyFrame.Height * 3;
			}
			else
			{
				player.bodyFrame.Y = player.bodyFrame.Height * 2;
			}
			return true;
		}

		public override void HoldItem(Player player)
		{
			Vector2 Pos = GetLightPosition(player) + new Vector2(-16, -16);
			Rectangle Rect = new Rectangle((int)Pos.X, (int)Pos.Y, 32,32);
			//Utils.AddDraw.AddDrawRect(Rect);
			Utils.AddDraw.AddDrawEllipse(Pos,32,16);
			foreach (NPC npc in Main.npc) {
				if (Rect.Intersects(npc.Hitbox)) {
					npc.AddBuff(type: ModContent.BuffType<Buffs.ShowDataBuff>(),150);
				}
			}
		}

		private Vector2 GetLightPosition(Player player)
		{
			Vector2 position = Main.screenPosition;
			position.X += Main.mouseX;
			position.Y += player.gravDir == 1 ? Main.mouseY : Main.screenHeight - Main.mouseY;
			return position;
		}
	}
}
