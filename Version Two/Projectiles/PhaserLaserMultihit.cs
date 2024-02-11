using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Lunarse.Projectiles
{
	// Token: 0x02000105 RID: 261
	public class PhaserLaserMultihit : ModProjectile
	{
		// Token: 0x060006E7 RID: 1767 RVA: 0x0003EE60 File Offset: 0x0003D060
		public override void SetDefaults()
		{
			base.Projectile.CloneDefaults(729);
			base.Projectile.DamageType = DamageClass.Magic;
			base.Projectile.usesIDStaticNPCImmunity = false;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = -1;
			base.Projectile.alpha = 255;
			base.DrawOriginOffsetY = -20;
			base.DrawOffsetX = -25;
		}
		public override void AI()
		{
			Lighting.AddLight(base.Projectile.Center, 0.3f, 0.03f, 0.3f);
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, 255) * (1f - (float)base.Projectile.alpha / 255f));
		}
	}
}
