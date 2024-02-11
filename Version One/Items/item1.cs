using System;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace med.Items
{
	public class item1 : ModItem
	{
        public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Nebula Predator");
			base.Tooltip.SetDefault("130% melee damage\n20% melee critical strike chance\n200% melee attack speed\nMelee critical hits grants attack speed\nMelee damage accumulates 10 times on enemies\nAfter 10 hits, the accumulated damage explodeand strikes the enemy\napplying cosmic burn by half of this damage.");
		}

		public override void SetDefaults()
		{
			base.Item.width = 40;
			base.Item.height = 40;
			base.Item.accessory = true;
			base.Item.value = Item.buyPrice(50, 0, 0, 0);
			base.Item.rare = 11;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Melee) += 4.3f;
            player.GetDamage(DamageClass.Magic) += 1.3f;
			player.GetDamage(DamageClass.Ranged) += 1.3f;
            player.GetCritChance(DamageClass.Melee) += 200f;
			player.GetAttackSpeed(DamageClass.Melee) += 1.2f;
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
