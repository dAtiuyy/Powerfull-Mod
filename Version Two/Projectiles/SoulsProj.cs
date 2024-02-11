using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lunarse.Projectiles
{
	// Token: 0x0200022C RID: 556
	public class SoulsProj : ModProjectile
	{
		// Token: 0x06000DE5 RID: 3557 RVA: 0x00058760 File Offset: 0x00056960
		public override void SetStaticDefaults()
		{
			Main.projFrames[base.Projectile.type] = 12;
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00058778 File Offset: 0x00056978
		public override void SetDefaults()
		{
			base.Projectile.width = 14;
			base.Projectile.height = 14;
			base.Projectile.friendly = true;
			base.Projectile.penetrate = 4;
			base.Projectile.DamageType = DamageClass.Magic;
			base.Projectile.timeLeft = 24;
			base.Projectile.light = 0.5f;
			base.Projectile.tileCollide = false;
			base.Projectile.scale = 1.5f;
			base.DrawOffsetX = 0;
			base.DrawOriginOffsetY = 0;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 10;
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00058828 File Offset: 0x00056A28
		public override void AI()
		{
			this.AnimateProjectile();
			base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) + 1.57f;
			if (base.Projectile.ai[1] == 0f)
			{
				this.dusttype = 170;
				base.DrawOffsetX = 4;
			}
			else if (base.Projectile.ai[1] == 1f)
			{
				this.dusttype = 110;
				base.DrawOffsetX = 4;
			}
			else if (base.Projectile.ai[1] == 2f)
			{
				this.dusttype = 56;
				base.DrawOffsetX = 2;
			}
			Dust dust = Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, this.dusttype, 0f, 0f, 0, default(Color), 1f);
			dust.velocity *= 0.5f;
			dust.noGravity = true;
			if (base.Projectile.timeLeft >= 24)
			{
				for (int i = 0; i < 15; i++)
				{
					Dust dust2 = Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, this.dusttype, 0f, 0f, 0, default(Color), 1f);
					dust2.velocity *= 2f;
					dust2.noGravity = true;
				}
			}
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x000042BB File Offset: 0x000024BB
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x000589C4 File Offset: 0x00056BC4
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int i = 0; i < 10; i++)
			{
				if (base.Projectile.ai[1] == 0f)
				{
					Dust dust = Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, this.dusttype, base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 0, default(Color), 1f);
					dust.scale = 1.25f;
					dust.noGravity = true;
				}
				else if (base.Projectile.ai[1] == 1f)
				{
					Dust dust2 = Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, this.dusttype, base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 0, default(Color), 1f);
					dust2.scale = 0.75f;
					dust2.noGravity = true;
				}
				else if (base.Projectile.ai[1] == 2f)
				{
					Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, this.dusttype, base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 0, default(Color), 1f).noGravity = true;
				}
			}
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00058B88 File Offset: 0x00056D88
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 15; i++)
			{
				Dust dust = Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, this.dusttype, 0f, 0f, 0, default(Color), 1f);
				dust.velocity *= 2f;
				dust.noGravity = true;
			}
			SoundStyle npcdeath = SoundID.NPCDeath6;
			npcdeath.Volume = 0.5f;
			npcdeath.Pitch = 0f;
			SoundEngine.PlaySound(in npcdeath, new Vector2?(base.Projectile.Center), null);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00058C3C File Offset: 0x00056E3C
		public void AnimateProjectile()
		{
			base.Projectile.frameCounter++;
			if (base.Projectile.ai[1] == 0f)
			{
				if (base.Projectile.frame >= 4)
				{
					base.Projectile.frame = 0;
				}
			}
			else if (base.Projectile.ai[1] == 1f)
			{
				if (base.Projectile.frame <= 3 || base.Projectile.frame >= 8)
				{
					base.Projectile.frame = 4;
				}
			}
			else if (base.Projectile.ai[1] == 2f && (base.Projectile.frame <= 7 || base.Projectile.frame >= 12))
			{
				base.Projectile.frame = 8;
			}
			if (base.Projectile.frameCounter >= 8)
			{
				base.Projectile.frame++;
				base.Projectile.frameCounter = 0;
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00058D38 File Offset: 0x00056F38
		public override Color? GetAlpha(Color lightColor)
		{
			Color color = Color.White;
			color.A = 150;
			return new Color?(color);
		}

		// Token: 0x040000B0 RID: 176
		private int dusttype;
	}
}
