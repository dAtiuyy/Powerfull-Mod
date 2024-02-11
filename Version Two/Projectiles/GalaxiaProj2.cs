using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Lunarse.Projectiles;

namespace Lunarse.Projectiles
{
	public class GalaxiaProj2 : ModProjectile
	{
		private Vector2 projvelocity;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.Projectile.width = 250;
			base.Projectile.height = 30;
			base.Projectile.scale = 1f;
			base.Projectile.timeLeft = 10;
			base.Projectile.penetrate = 20;
			base.Projectile.aiStyle = -1;
			base.Projectile.DamageType = DamageClass.Melee;
			base.Projectile.tileCollide = false;
			base.Projectile.friendly = true;
			base.Projectile.extraUpdates = 1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = -1;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float num = 0f;
			return new bool?(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), base.Projectile.Center - Vector2.Normalize(base.Projectile.velocity) * ((float)(base.Projectile.width / 2) * base.Projectile.scale), base.Projectile.Center + Vector2.Normalize(base.Projectile.velocity) * ((float)(base.Projectile.width / 2) * base.Projectile.scale), (float)base.Projectile.height * base.Projectile.scale, ref num));
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00048C04 File Offset: 0x00046E04
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[base.Projectile.type].Value;
			Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
			Color color = Main.DiscoColor;
			color *= 1f;
			color.A = 0;
			for (int i = 0; i < 1; i++)
			{
				Vector2 Scale = new Vector2(base.Projectile.scale * 7f, base.Projectile.scale * 0.4f);
				Vector2 vector = base.Projectile.Center - Main.screenPosition + new Vector2(0f, base.Projectile.gfxOffY);
				Main.spriteBatch.Draw(texture, vector, new Rectangle?(rectangle), color, Utils.ToRotation(base.Projectile.velocity), Utils.Size(rectangle) / 2f, Scale, 0, 0f);
			}
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{	
			Player player = Main.player[base.Projectile.owner];
        	int newDamage = (Main.rand.NextFloat() < 0.7f) ? 1 : 10;
			base.Projectile.damage += newDamage;
			base.Projectile.CritChance += base.Projectile.damage;
			target.AddBuff(ModContent.BuffType<Buffs.LunDebuff>(), 1200); // Adjust the duration (1200) as needed
			double deg = (double)Main.rand.Next(0, 360);
    		for (int i = 0; i < 8; i++)
    		{
        		float projspeed = 5f;
            	this.projvelocity = new Vector2(target.position.X, target.position.Y) * projspeed;
        		Projectile.NewProjectile(target.GetSource_FromThis(null), target.Center, new Vector2(this.projvelocity.Y, this.projvelocity.X), ModContent.ProjectileType<CrackedDaggerProj>(), newDamage, 0, player.whoAmI, 0f, 0f, 0f);
			}
		}
		protected virtual float HoldoutRangeMin
		{
			get
			{
				return 6f;
			}
		}
		protected virtual float HoldoutRangeMax
		{
			get
			{
				return 30f;
			}
		}
		public override void AI()
		{
			float progress = 0.1f;
			Player player = Main.player[base.Projectile.owner];
			base.Projectile.rotation = Utils.ToRotation(base.Projectile.velocity);
			base.Projectile.velocity *= 0.98f;
			base.Projectile.Center = player.MountedCenter + Vector2.SmoothStep(base.Projectile.velocity * this.HoldoutRangeMin, base.Projectile.velocity * this.HoldoutRangeMax, progress);
		}
	}
}
