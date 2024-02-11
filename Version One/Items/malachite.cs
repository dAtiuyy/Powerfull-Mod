using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using med;
using med.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace med.Items
{
	public class malachite : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Malachite");
			base.Tooltip.SetDefault("Throws a stream of kunai that stick to enemies and explode\nRight click to throw a single kunai that pierces, after piercing an enemy it emits a massive explosion on the next enemy hit\nStealth strikes fire three kunai that home in, stick to enemies, and explode");
		}
		public override void SetDefaults()
		{
			base.Item.width = 28;
			base.Item.damage = 2500;
			base.Item.noMelee = true;
			base.Item.noUseGraphic = true;
			base.Item.useTime = (base.Item.useAnimation = 14);
			base.Item.useStyle = 1;
			base.Item.knockBack = 1.25f;
			base.Item.UseSound = new SoundStyle?(SoundID.Item1);
			base.Item.autoReuse = true;
			base.Item.height = 58;
			base.Item.shoot = ProjectileType<malachiteProj>();
			base.Item.shootSpeed = 5f;
           		base.Item.DamageType = DamageClass.Melee;
          		base.Item.rare = 8;
          	        base.Item.value = 10000;
        }

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
				for (float i = -6.5f; i <= 6.5f; i += 6.5f)
				{
					Vector2 perturbedSpeed = Utils.RotatedBy(velocity, (double)MathHelper.ToRadians(i), default(Vector2));
				}
				return true;
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