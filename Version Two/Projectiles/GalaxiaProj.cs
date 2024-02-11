using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Lunarse.Projectiles
{
	// Token: 0x02000189 RID: 393
	public class GalaxiaProj : ModProjectile
	{
		protected virtual float HoldoutRangeMin
		{
			get
			{
				return 100f;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x000488D3 File Offset: 0x00046AD3
		protected virtual float HoldoutRangeMax
		{
			get
			{
				return 160f;
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00043A87 File Offset: 0x00041C87
		public override void SetDefaults()
		{
			base.Projectile.CloneDefaults(49);
			base.Projectile.scale = 1f;
			base.Projectile.penetrate = 20;
			base.Projectile.maxPenetrate = 100;
			base.Projectile.ArmorPenetration = 200;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x000488DC File Offset: 0x00046ADC
		public override bool PreAI()
		{
			Player player = Main.player[base.Projectile.owner];
			int duration = player.itemAnimationMax;
			player.heldProj = base.Projectile.whoAmI;
			if (base.Projectile.timeLeft > duration)
			{
				base.Projectile.timeLeft = duration;
				Vector2 Vec = base.Projectile.velocity * 0.75f;
				base.Projectile.netUpdate = true;
				Projectile.NewProjectileDirect(base.Projectile.GetSource_FromAI(null), base.Projectile.Center, Vec, ModContent.ProjectileType<GalaxiaProj2>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, 0f, 0f, 0f);
				if ((float)player.statLife >= (float)player.statLifeMax2 * 0.50f) {
					Projectile.NewProjectile(base.Projectile.GetSource_FromAI(null), base.Projectile.Center, Vec, ModContent.ProjectileType<GalaxiaProj2>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, 0f, 0f, 0f);
				}
			}
			base.Projectile.velocity = Vector2.Normalize(base.Projectile.velocity);
			float halfDuration = (float)duration * 0.5f;
			float progress;
			if ((float)base.Projectile.timeLeft < halfDuration)
			{
				progress = (float)base.Projectile.timeLeft / halfDuration;
			}
			else
			{
				progress = (float)(duration - base.Projectile.timeLeft) / halfDuration;
			}
			base.Projectile.Center = player.MountedCenter + Vector2.SmoothStep(base.Projectile.velocity * this.HoldoutRangeMin, base.Projectile.velocity * this.HoldoutRangeMax, progress);
			if (base.Projectile.spriteDirection == -1)
			{
				base.Projectile.rotation += MathHelper.ToRadians(45f);
			}
			else
			{
				base.Projectile.rotation += MathHelper.ToRadians(135f);
			}
			return false;
		}
		 public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {	
			target.AddBuff(ModContent.BuffType<Buffs.LunDebuff>(), 1200); // Adjust the duration (1200) as needed
        }
	}
}
