using System;
using Lunarse.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace Lunarse.Items.Melee
{
	public class Spear : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.Item.width = (base.Item.height = 52);
			base.Item.damage = 132;
			base.Item.noMelee = true;
			base.Item.noUseGraphic = true;
			base.Item.useAnimation = (base.Item.useTime = 10);
			base.Item.useStyle = ItemUseStyleID.Swing;
			base.Item.knockBack = 7f;
			base.Item.UseSound = new SoundStyle?(Spear.ThrowSound);
			base.Item.autoReuse = true;
			base.Item.value = 30000;
			 base.Item.shoot = ModContent.ProjectileType<Projectiles.Spear>();
			base.Item.shootSpeed = 10f;
			base.Item.DamageType = DamageClass.Melee;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return true;
		}
		static Spear()
		{
			SoundStyle throwSound;
            throwSound = new SoundStyle("Lunarse/Sounds/SpearThrow", 0);
			throwSound.Volume = 0.1f;
			throwSound.PitchVariance = 0.2f;
			Spear.ThrowSound = throwSound;
		}
		public static readonly SoundStyle ThrowSound;
	}
}
