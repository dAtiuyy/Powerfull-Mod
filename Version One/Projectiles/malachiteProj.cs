using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace med.Projectiles
{
    public class malachiteProj : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "med/Projectiles/malachiteProj";
            }
        }

        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Malachite");
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            base.Projectile.width = 12;
            base.Projectile.height = 12;
            base.Projectile.alpha = 255;
            base.Projectile.friendly = true;
            base.Projectile.ignoreWater = true;
            base.Projectile.penetrate = 2;
            base.Projectile.alpha = 255;
            base.Projectile.extraUpdates = 10;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 10;
            base.Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) + 1.5707964f;
            base.Projectile.alpha -= 3;
            if (base.Projectile.alpha < 100)
            {
                base.Projectile.alpha = 100;
            }
            base.Projectile.localAI[1] += 1f;
            if (base.Projectile.localAI[1] > 4f)
            {
                for (int num468 = 0; num468 < 3; num468++)
                {
                    int num469 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 107, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 0.75f);
                    Main.dust[num469].noGravity = true;
                    Main.dust[num469].velocity *= 0f;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(Main.DiscoR, 203, 103, base.Projectile.alpha));
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (base.Projectile.penetrate <= 1)
            {
                base.Projectile.position = base.Projectile.Center;
                base.Projectile.width = (base.Projectile.height = 160);
                base.Projectile.position.X = base.Projectile.position.X - (float)(base.Projectile.width / 2);
                base.Projectile.position.Y = base.Projectile.position.Y - (float)(base.Projectile.height / 2);
                base.Projectile.usesLocalNPCImmunity = true;
                base.Projectile.localNPCHitCooldown = 10;
                base.Projectile.Damage();
                for (int num621 = 0; num621 < 70; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 107, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Utils.NextBool(Main.rand, 2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 40; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 107, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.7f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 107, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            base.Projectile.position = base.Projectile.Center;
            base.Projectile.width = (base.Projectile.height = 16);
            base.Projectile.position.X = base.Projectile.position.X - (float)(base.Projectile.width / 2);
            base.Projectile.position.Y = base.Projectile.position.Y - (float)(base.Projectile.height / 2);
            for (int num621 = 0; num621 < 7; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 107, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.2f);
                Main.dust[num622].velocity *= 3f;
                if (Utils.NextBool(Main.rand, 2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 3; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 107, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.7f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 107, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1f);
                Main.dust[num624].velocity *= 2f;
            }
        }
    }
}
