using System;
using LuiAFKUtl.LuiAFKCFG;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Lunarse.LunPlayer;

namespace Lunarse.Items.Misc
{
	public class UltimatePeaceful : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			base.Item.rare = ItemRarityID.Pink;
		}
		public override void UpdateInventory(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				if (base.Item.favorited && !player.GetModPlayer<LunarsePlayer>().UltBattler)
				{
					player.GetModPlayer<LunarsePlayer>().UltPeaceful = true;
					player.GetModPlayer<LunarsePlayer>().MaxSpawnsMultiplier = 0.01f;
					player.GetModPlayer<LunarsePlayer>().SpawnRateMultiplier = 0.01f;
					player.enemySpawns = false;
					return;
				}
			}
		}
		public override void AddRecipes()
		{}
	}
}
