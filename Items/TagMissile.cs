using System;
using med;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using med.Dusts;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace med.Items
{
    public class TagMissile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Tag Missile");
        }

        public override void SetDefaults()
        {
            base.Projectile.width = (base.Projectile.height = 6);
            base.Projectile.penetrate = -1;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = -1;
            base.Projectile.timeLeft = 480;
            base.Projectile.friendly = true;
        }

        private void explode()
        {
            if (!this.exploded)
            {
                this.exploded = true;
                base.Projectile.timeLeft = 5;
                base.Projectile.width = 15;
                base.Projectile.height = 15;
                base.Projectile.position -= Vector2.One * 12f;
                base.Projectile.tileCollide = false;
                base.Projectile.velocity = Vector2.Zero;
                for (int i = 0; i < 30; i++)
                {
                    float rot = 6.2831855f * ((float)i / 30f);
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (base.Projectile.timeLeft > 450)
            {
                return new bool?(false);
            }
            return default(bool?);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            this.explode();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            this.explode();
            return false;
        }


        // Token: 0x06000827 RID: 2087 RVA: 0x0004D544 File Offset: 0x0004B744
        public override bool PreDraw(ref Color lightColor)
        {
            if (this.exploded)
            {
                return false;
            }
            Texture2D texture = TextureAssets.Projectile[base.Projectile.type].Value;
            return false;
        }

        private bool exploded;
        private bool runOnce = true;
    }
}

