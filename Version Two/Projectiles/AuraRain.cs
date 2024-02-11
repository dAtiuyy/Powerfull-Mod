using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Lunarse.Projectiles
{
	public class AuraRain : ModProjectile, ILocalizedModType, IModType
	{
		public override void SetDefaults()
		{
			base.Projectile.width = 14;
			base.Projectile.height = 14;
			base.Projectile.aiStyle = 45;
			base.Projectile.friendly = true;
			base.Projectile.penetrate = -1;
			base.Projectile.tileCollide = true;
			base.Projectile.timeLeft = 300;
			base.Projectile.alpha = 255;
			base.Projectile.scale = 1.1f;
			base.Projectile.DamageType = DamageClass.Magic;
			base.Projectile.extraUpdates = 1;
			base.AIType = 239;
		}
		public override void AI()
		{
			base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) - 1.5707964f;
			Dust dust = Main.dust[Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, DustID.Demonite, base.Projectile.velocity.X, base.Projectile.velocity.Y, 100, default(Color), 1f)];
			dust.velocity = Vector2.Zero;
			dust.position -= base.Projectile.velocity / 5f;
			dust.noGravity = true;
			dust.scale = 0.8f;
			dust.noLight = true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>(this.Texture).Value;
			Main.EntitySpriteDraw(tex, base.Projectile.Center - Main.screenPosition, default(Rectangle?), base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, Utils.Size(tex) / 2f, base.Projectile.scale, 0, 0f);
			return false;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {	
			target.AddBuff(ModContent.BuffType<Buffs.LunDebuff>(), 1200); // Adjust the duration (1200) as needed
        }
    }
}
