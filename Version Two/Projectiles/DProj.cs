using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lunarse.Projectiles
{
	public class DProj : ModProjectile, ILocalizedModType, IModType
	{
		public override void SetDefaults()
		{
            base.Projectile.width = (base.Projectile.height = 15);
			base.Projectile.friendly = true;
			base.Projectile.ignoreWater = true;
			base.Projectile.penetrate = 2;
			base.Projectile.timeLeft = 900;
			base.Projectile.aiStyle = 0;
			base.Projectile.DamageType = DamageClass.Melee;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 10;
		}
		public override void AI()
		{
			LunarseUtils.HomeInOnNPC(base.Projectile, false, 450f, 50f, 26f);
			base.Projectile.rotation = Utils.ToRotation(base.Projectile.velocity) + 0.7853982f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>(this.Texture, (ReLogic.Content.AssetRequestMode)2).Value;
			Main.EntitySpriteDraw(tex, base.Projectile.Center - Main.screenPosition, default(Rectangle?), base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, Utils.Size(tex) / 2f, base.Projectile.scale, 0, 0f);
			return false;
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return default(bool?);
		}
		public override void OnKill(int timeLeft)
		{
            for (int i = 0; i <= 9; i++)
			{
				Dust.NewDust(base.Projectile.position + base.Projectile.velocity, base.Projectile.width, base.Projectile.height, Main.rand.Next(169, 170), base.Projectile.oldVelocity.X * 0.3f, base.Projectile.oldVelocity.Y * 0.3f, 0, default(Color), Utils.NextFloat(Main.rand, 1.2f, 1.6f));
			}
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (base.Projectile.owner == Main.myPlayer)
			{
				Vector2 v = Utils.NextVector2CircularEdge(Main.rand, 200f, 200f);
				if (v.Y < 0f)
				{
					v.Y *= -1f;
				}
				v.Y += 100f;
				Vector2 vector = Utils.SafeNormalize(v, v) * 6f;
				
                Projectile.NewProjectile(base.Projectile.GetSource_FromThis(null), target.Center - vector * 15f, vector, ModContent.ProjectileType<PhaserLaserMultihit>(), (int)((float)base.Projectile.damage * 0.5f), 0f, base.Projectile.owner, 0f, target.Center.Y, 0f);
                if ((Main.rand.NextFloat() < 0.7f) ? false : true)
			    {
				    Projectile rain = LunarseUtils.ProjectileRain(base.Projectile.GetSource_FromThis(null), target.Center, 400f, 100f, 500f, 800f, 22f, ModContent.ProjectileType<AuraRain>(), base.Projectile.damage, 2f, base.Projectile.owner);
                    rain.penetrate = 2;
                    rain.DamageType = DamageClass.Generic;
				    rain.tileCollide = false;
                }
            }
            Vector2 positionInWorld = Utils.NextVector2FromRectangle(Main.rand, target.Hitbox);
			ParticleOrchestraSettings settings = new ParticleOrchestraSettings
			{
    			PositionInWorld = positionInWorld
			};
			ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.Excalibur, settings, new int?(base.Projectile.owner));
            target.AddBuff(ModContent.BuffType<Buffs.LunDebuff>(), 1200);
			base.OnHitNPC(target, hit, damageDone);
        }
    }
}
