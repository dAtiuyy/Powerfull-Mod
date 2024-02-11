using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lunarse.Projectiles
{
	// Token: 0x020006A0 RID: 1696
	public class Spear : ModProjectile, ILocalizedModType, IModType
	{
		public override void SetDefaults()
		{
			base.Projectile.width = 10;
			base.Projectile.height = 10;
			base.Projectile.friendly = true;
			base.Projectile.ignoreWater = true;
			base.Projectile.penetrate = 6;
			base.Projectile.timeLeft = 900;
			base.Projectile.aiStyle = 0;
			base.Projectile.DamageType = DamageClass.Melee;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 50;
		}
		public override void AI()
		{
			this.framesInAir++;
			base.Projectile.scale = 1.2f;
			if (Utils.NextBool(Main.rand) && !this.posthit)
			{
				Vector2 position = base.Projectile.Center + Vector2.Normalize(base.Projectile.velocity);
				Dust dust = Main.dust[Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, Main.rand.Next(169, 170), 0f, 0f, 0, default(Color), Utils.NextFloat(Main.rand, 1.2f, 1.5f))];
				dust.position = position;
				dust.velocity = Utils.RotatedBy(base.Projectile.velocity, 1.9707963705062865, default(Vector2)) * 0.1f + base.Projectile.velocity / 8f;
				dust.position += Utils.RotatedBy(base.Projectile.velocity, 0.3, default(Vector2));
				dust.fadeIn = 0.5f;
				dust.noGravity = true;
				Dust dust2 = Main.dust[Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, Main.rand.Next(169, 170), 0f, 0f, 0, default(Color), Utils.NextFloat(Main.rand, 1.2f, 1.5f))];
				dust2.position = position;
				dust2.velocity = Utils.RotatedBy(base.Projectile.velocity, -1.9707963705062865, default(Vector2)) * 0.1f + base.Projectile.velocity / 8f;
				dust2.position += Utils.RotatedBy(base.Projectile.velocity, -0.3, default(Vector2));
				dust2.fadeIn = 0.5f;
				dust2.noGravity = true;
			}
			base.Projectile.rotation = Utils.ToRotation(base.Projectile.velocity) + 0.7853982f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>(this.Texture, (ReLogic.Content.AssetRequestMode)2).Value;
			Main.EntitySpriteDraw(tex, base.Projectile.Center - Main.screenPosition, default(Rectangle?), base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, Utils.Size(tex) / 2f, base.Projectile.scale, 0, 0f);
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Vector2 bloodSpawnPosition = target.Center + Utils.NextVector2Circular(Main.rand, (float)target.width, (float)target.height) * 0.04f;
			Utils.SafeNormalize(base.Projectile.Center - bloodSpawnPosition, Vector2.UnitY);
			int sparkCount = Main.rand.Next(4, 6);
			for (int i = 0; i < sparkCount; i++)
			{
				Vector2 sparkVelocity = Utils.RotatedByRandom(base.Projectile.velocity, 0.20000000298023224) * Utils.NextFloat(Main.rand, 0.6f, 1.1f);
				int sparkLifetime = Main.rand.Next(23, 25);
				float sparkScale = Utils.NextFloat(Main.rand, 0.8f, 1f) * 0.955f;
				Color sparkColor = Color.Lerp(Color.Gold, Color.Goldenrod, Utils.NextFloat(Main.rand, 0.7f));
				sparkColor = Color.Lerp(sparkColor, Color.Gold, Utils.NextFloat(Main.rand));
				}
			SoundEngine.PlaySound(in Spear.Hitsound, new Vector2?(base.Projectile.position), null);
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (base.Projectile.numHits > 0)
			{
				base.Projectile.damage = (int)((float)base.Projectile.damage * 0.9f);
			}
			if (base.Projectile.damage < 1)
			{
				base.Projectile.damage = 1;
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			return default(bool?);
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i <= 9; i++)
			{
				Dust.NewDust(base.Projectile.position + base.Projectile.velocity, base.Projectile.width, base.Projectile.height, Main.rand.Next(169, 170), base.Projectile.oldVelocity.X * 0.3f, base.Projectile.oldVelocity.Y * 0.3f, 0, default(Color), Utils.NextFloat(Main.rand, 1.2f, 1.6f));
			}
		}

		static Spear()
		{
			SoundStyle hitsound;
			hitsound = new SoundStyle("Lunarse/Sounds/Hit", 0);
			hitsound.PitchVariance = 0.3f;
			hitsound.Volume = 0.5f;
			Spear.Hitsound = hitsound;
		}
		public static readonly SoundStyle Hitsound;
		public bool posthit;
		public int framesInAir;
	}
}
