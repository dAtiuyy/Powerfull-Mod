using System;
using Lunarse.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lunarse.Items.Summon
{
	public class Summon : ModItem, ILocalizedModType, IModType
	{
		public override void SetStaticDefaults()
		{
			Item.staff[base.Item.type] = true;
		}
		public override void SetDefaults()
		{
			base.Item.damage = 73;
			base.Item.DamageType = DamageClass.Summon;
			base.Item.mana = 10;
			base.Item.width = 26;
			base.Item.height = 36;
			base.Item.useTime = (base.Item.useAnimation = 14);
			base.Item.useStyle = ItemUseStyleID.HoldUp;
			base.Item.noMelee = true;
			base.Item.knockBack = 5f;
			base.Item.value = 10000;
			base.Item.rare = ItemRarityID.Red;
			base.Item.UseSound = new SoundStyle?(SoundID.Item71);
			base.Item.autoReuse = true;
			base.Item.shoot = ModContent.ProjectileType<SummonProj>();
		}
		public unsafe override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 1f, 0f);
				if (Utils.IndexInRange<Projectile>(Main.projectile, p))
				{
					Main.projectile[p].originalDamage = base.Item.damage;
					Main.projectile[p].ModProjectile<SummonProj>().BladeIndex = player.ownedProjectileCounts[type];
				}
				int bladeIndex = 0;
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					if (Main.projectile[i].type == type && Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI)
					{
						Main.projectile[i].ModProjectile<SummonProj>().BladeIndex = bladeIndex++;
						Main.projectile[i].ModProjectile<SummonProj>().AITimer = 0f;
						Main.projectile[i].netUpdate = true;
					}
				}
			}
			return false;
		}
		

		public const int HorizontalSlashChargeTime = 14;
		public const float HorizontalSlashSpeed = 44f;
		public const int VerticalSlashChargeTime = 32;
		public const float VerticalSlashSpeed = 45f;
		public const float VerticalTeleportOffset = 850f;
		public const int PierceChargeAttackCycleTime = 44;
		public const float MaxTargetingDistance = 1550f;
		public const int ChargesPerAttackCycle = 7;
	}
}
