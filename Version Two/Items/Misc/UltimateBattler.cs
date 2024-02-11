using System;
using Lunarse.LunPlayer;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Lunarse.Items.Misc
{
	public class UltimateBattler : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			base.Item.rare = ItemRarityID.Red;
		}
		public override void UpdateInventory(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				if (base.Item.favorited)
				{
					player.GetModPlayer<LunPlayer.LunarsePlayer>().UltBattler = true;
					player.GetModPlayer<LunPlayer.LunarsePlayer>().MaxSpawnsMultiplier = 500f;
					player.GetModPlayer<LunPlayer.LunarsePlayer>().SpawnRateMultiplier = 100f;
					player.enemySpawns = true;
					return;
				}
			}
		}
		public override void AddRecipes()
		{
		}
	}
}
