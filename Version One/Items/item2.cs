using System;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace med.Items
{
	public class item2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Life From Dust");
			base.Tooltip.SetDefault("+1 Minion per 100 life\n10% Increased summon damage per 10 life\nHits have a 8% chance of inflicting cosmic burn\nHits deals additional damage, witch scales with\nmaximum minions.\n");
		}

		public override void SetDefaults()
		{
			base.Item.width = 40;
			base.Item.height = 40;
			base.Item.accessory = true;
			base.Item.value = Item.buyPrice(5, 0, 0, 0);
			base.Item.rare = 10;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions += player.statLifeMax / 10;
			player.GetDamage(DamageClass.Summon) += 30.1f * (float)(player.statLifeMax / 10);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
