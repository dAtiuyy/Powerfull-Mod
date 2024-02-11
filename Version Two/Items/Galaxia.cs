using System;
using Lunarse.Projectiles;
using Lunarse.Globals;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lunarse.Items
{
	public class Galaxia : ModItem
	{
		public override void SetDefaults()
		{
			base.Item.rare = ItemRarityID.Purple;
			base.Item.value = Item.sellPrice(0, 26, 50, 0);
			base.Item.useStyle = ItemUseStyleID.Shoot;
			base.Item.useAnimation = 4;
			base.Item.useTime = 4;
			base.Item.UseSound = new SoundStyle?(SoundID.Item1);
			base.Item.autoReuse = true;
			base.Item.damage = 20;
			base.Item.knockBack = 2f;
			base.Item.noUseGraphic = true;
			base.Item.DamageType = DamageClass.Melee;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 10f;
			base.Item.shoot = ModContent.ProjectileType<GalaxiaProj>();
			base.Item.ArmorPenetration = 200;
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[base.Item.shoot] < 1;
		}
		public override bool? UseItem(Player player)
		{
			if (!Main.dedServ && base.Item.UseSound != null)
			{
				SoundStyle value = base.Item.UseSound.Value;
				SoundEngine.PlaySound(in value, new Vector2?(player.Center), null);
			}
			int damage = 180;
			if (NPC.downedSlimeKing && NPC.downedBoss1 && NPC.downedBoss2 && NPC.downedBoss3 && NPC.downedQueenBee && NPC.downedDeerclops)
			{
				base.Item.damage = damage * 2;
				if (Main.hardMode && NPC.downedQueenSlime && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && NPC.downedPlantBoss && NPC.downedGolemBoss && NPC.downedEmpressOfLight && NPC.downedEmpressOfLight && NPC.downedFishron && NPC.downedAncientCultist && NPC.downedMoonlord)
				{
					base.Item.damage = damage * 3;
					if (NPC.downedPirates && NPC.downedMartians && NPC.downedHalloweenKing && NPC.downedChristmasIceQueen)
					{
						base.Item.damage = damage * 5;
					}
				}
			}
			return default(bool?);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float rot = Utils.ToRotation(velocity);
			Vector2 perturbedSpeed = Utils.RotatedByRandom(velocity, (double)MathHelper.ToRadians(1f));
			Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<GalaxiaProj>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
			if ((float)player.statLife >= (float)player.statLifeMax2 * 0.10f) {
				Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<GalaxiaProj>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
				if ((float)player.statLife >= (float)player.statLifeMax2 * 0.25f) {
					Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<GalaxiaProj>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
					if ((float)player.statLife >= (float)player.statLifeMax2 * 0.50f) {
						Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<GalaxiaProj>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
			}
			}}
			return false;
		}
	}
}
