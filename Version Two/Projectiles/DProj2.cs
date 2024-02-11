using System;
using Lunarse.Items.Accs;
using Lunarse.Items.DEBUG;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Lunarse.Projectiles
{
	// Token: 0x0200028A RID: 650
	public class DProj2 : GlobalProjectile
	{
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600115F RID: 4447 RVA: 0x0000CC09 File Offset: 0x0000AE09
		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00076118 File Offset: 0x00074318
		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			EntitySource_ItemUse_WithAmmo item = source as EntitySource_ItemUse_WithAmmo;
			if (item != null && item.Item.type == ModContent.ItemType<DEBUG>())
			{
				this.predatorDarts = true;
			}
			if (item != null && item.Item.type == ModContent.ItemType<TestAcc>())
			{
				this.predatorDarts = true;
			}
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00076148 File Offset: 0x00074348
		public override void AI(Projectile projectile)
		{
			this.predatorCooldown--;
			if (this.predatorCooldown <= 0)
			{
				this.predatorCooldown = 0;
			}
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x00076168 File Offset: 0x00074368
		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.myPlayer == projectile.owner && this.predatorDarts && this.predatorCooldown <= 0)
			{
				this.predatorCooldown = 30;
				int dartCount = 3 + Main.rand.Next(3);
				for (int i = 0; i < dartCount; i++)
				{
					Vector2 posOffset = Utils.NextVector2CircularEdge(Main.rand, 1f, 1f);
					posOffset *= 180f;
					Vector2 spawnPos = target.Center + posOffset;
					Vector2 dartVelocity = Utils.SafeNormalize(target.Center - spawnPos, Vector2.Zero) * 7f;
					Projectile.NewProjectile(projectile.GetSource_FromThis(null), spawnPos, dartVelocity, ModContent.ProjectileType<CrackedDaggerProj>(), (int)((float)projectile.originalDamage * 0.35f), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
					for (int j = 0; j < 10; j++)
					{
						int dustIndex = Dust.NewDust(new Vector2(spawnPos.X - (float)(projectile.width / 2), spawnPos.Y - (float)(projectile.height / 2)), projectile.width, projectile.height, 217, 0f, 0f, 0, default(Color), 1f);
						Main.dust[dustIndex].noGravity = true;
						Main.dust[dustIndex].velocity *= 1.2f;
					}
					for (int k = 0; k < 5; k++)
					{
						int dustIndex2 = Dust.NewDust(new Vector2(spawnPos.X - (float)(projectile.width / 2), spawnPos.Y - (float)(projectile.height / 2)), projectile.width, projectile.height, 217, dartVelocity.X, dartVelocity.Y, 0, default(Color), 0.9f);
						Main.dust[dustIndex2].noGravity = true;
						Main.dust[dustIndex2].velocity *= 0.6f;
					}
				}
			}
		}

		// Token: 0x040001FE RID: 510
		private bool predatorDarts;

		// Token: 0x040001FF RID: 511
		public int predatorCooldown;
	}
}
