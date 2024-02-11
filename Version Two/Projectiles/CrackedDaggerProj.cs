using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lunarse.Projectiles
{
	// Token: 0x020001B4 RID: 436
	public class CrackedDaggerProj : ModProjectile
	{
		// Token: 0x06000A19 RID: 2585 RVA: 0x0003A99B File Offset: 0x00038B9B
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 5;
		}
		public override void SetDefaults()
		{
			base.Projectile.width = 22;
			base.Projectile.height = 22;
			base.Projectile.friendly = true;
			base.Projectile.penetrate = 1;
			base.Projectile.DamageType = DamageClass.Magic;
			base.Projectile.timeLeft = 40;
			base.Projectile.light = 0.5f;
			base.Projectile.tileCollide = false;
			base.Projectile.extraUpdates = 1;
			base.Projectile.scale = 1f;
			base.DrawOffsetX = 0;
			base.DrawOriginOffsetY = 0;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 10;
			base.Projectile.penetrate = 3;
		}
		public override void OnSpawn(IEntitySource source)
		{
			for (int i = 0; i < 15; i++)
			{
				Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, DustID.EnchantedNightcrawler, 0f, 0f, 0, default(Color), 1f).noGravity = true;
			}
		}
		public override void AI()
		{
			base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) + 1.57f;
			Dust dust = Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, DustID.EnchantedNightcrawler, 0f, 0f, 0, default(Color), 1f);
			dust.velocity *= 0.5f;
			dust.scale = 0.75f;
			dust.noGravity = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, DustID.EnchantedNightcrawler, base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 0, default(Color), 1f);
				dust.scale = 1.25f;
				dust.noGravity = true;
			}
			target.AddBuff(ModContent.BuffType<Buffs.LunDebuff>(), 1200); // Adjust the duration (1200) as needed
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 15; i++)
			{
				Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, DustID.EnchantedNightcrawler, 0f, 0f, 0, default(Color), 1f).noGravity = true;
			}
			SoundStyle item = SoundID.Item27;
			item.Volume = 0.5f;
			item.Pitch = -0.2f;
			SoundEngine.PlaySound(in item, new Vector2?(base.Projectile.Center), null);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0003ACC8 File Offset: 0x00038EC8
		public override Color? GetAlpha(Color lightColor)
		{
			Color color = Color.White;
			color.A = 100;
			return new Color?(color);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0003ACEC File Offset: 0x00038EEC
		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(base.Projectile.type);
			Texture2D texture = TextureAssets.Projectile[base.Projectile.type].Value;
			Vector2 drawOrigin;
            drawOrigin = new Vector2((float)texture.Width * 0.5f, (float)base.Projectile.height * 0.5f);
			for (int i = 0; i < base.Projectile.oldPos.Length; i++)
			{
				Vector2 drawPos = base.Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(2f, 0f);
				Color color = base.Projectile.GetAlpha(lightColor) * ((float)(base.Projectile.oldPos.Length - i) / (float)base.Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, default(Rectangle?), color, base.Projectile.rotation, drawOrigin, base.Projectile.scale, 0, 0f);
			}
			return true;
		}
	}
}
