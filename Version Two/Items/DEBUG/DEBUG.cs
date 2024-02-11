using System;
using Lunarse.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace Lunarse.Items.DEBUG
{
	public class DEBUG : ModItem
	{
		public override void SetDefaults()
		{
			base.Item.width = (base.Item.height = 30);
			base.Item.damage = 1;
			base.Item.noMelee = true;
			base.Item.noUseGraphic = true;
			base.Item.useAnimation = (base.Item.useTime = 20);
			base.Item.useStyle = ItemUseStyleID.Swing;
			base.Item.knockBack = 7f;
			base.Item.autoReuse = true;
			base.Item.value = 30000;
			 base.Item.shoot = ModContent.ProjectileType<Projectiles.DProj>();
			base.Item.shootSpeed = 20f;
			base.Item.DamageType = DamageClass.Melee;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<DProj>(), damage, 0f, 0, 0f, 0f, 0f);
			return false;
		}
        private int numHIT; // Declare numHIT as a field outside the method
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {   
        numHIT++;
        if (numHIT >= 5)
        {
            target.AddBuff(BuffID.Ichor, 1200);
            target.AddBuff(ModContent.BuffType<Buffs.LunDebuff>(), 1200);
			player.AddBuff(BuffID.Sharpened, 120);
            Projectile.NewProjectile(player.GetSource_FromThis(null), player.Center, player.Center * 0.1f, ModContent.ProjectileType<DProj>(), 200, 10f, player.whoAmI, 0f, 0f , 0f);
            numHIT = 0;
        }
            base.OnHitNPC(player, target, hit, damageDone);
        }
    public override Vector2? HoldoutOffset()
		{
			return new Vector2?(new Vector2(-8f, 0f));
		}
	}
}
