using System;
using Microsoft.Xna.Framework;
using Lunarse.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lunarse.Items.Mage
{
	public class Dagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.staff[base.Item.type] = true;
			base.Item.ResearchUnlockCount = 1;
			Main.RegisterItemAnimation(base.Item.type, new DrawAnimationVertical(6, 4, false));
			ItemID.Sets.AnimatesAsSoul[base.Item.type] = true;
		}

		public override void SetDefaults()
		{
			base.Item.width = 25;
			base.Item.height = 30;
			base.Item.maxStack = 1;
			base.Item.value = Item.sellPrice(0, 1, 0, 0);
			base.Item.rare = ItemRarityID.Orange;
			base.Item.useStyle = ItemUseStyleID.Thrust;
			base.Item.useTime = 10;
			base.Item.useAnimation = 10;
			base.Item.useTurn = false;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.autoReuse = true;
			Item item = base.Item;
			SoundStyle item2 = SoundID.Item8;
			item2.Volume = 1f;
			item2.Pitch = 0.2f;
			item.UseSound = new SoundStyle?(item2);
			base.Item.damage = 20;
			base.Item.knockBack = 0.1f;
			base.Item.shoot = ModContent.ProjectileType<SoulsProj>();
			base.Item.shootSpeed = 1f;
			base.Item.noMelee = true;
			base.Item.mana = 10;
		}

		public override void PostUpdate()
		{
			if (!Main.dedServ)
			{
				Lighting.AddLight(base.Item.Center, Color.WhiteSmoke.ToVector3() * 0.2f * Main.essScale);
			}
			if (base.Item.lavaWet && base.Item.velocity.Y > -2f)
			{
				Item item = base.Item;
				item.velocity.Y = item.velocity.Y - 0.25f;
			}
		}

		public override bool CanUseItem(Player player)
		{
			return Collision.CanHitLine(Main.MouseWorld, 1, 1, player.Center, 0, 0);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
    		double deg = (double)Main.rand.Next(0, 360);
    		for (int i = 0; i < 8; i++)
    		{
        		deg += 45.0;
        		double rad = deg * 0.017453292519943295;
        		double dist = 150.0;
        		float projspeed = 8f;

        		// Randomly choose between 20 and 70 damage based on probabilities
        		int newDamage = (Main.rand.NextFloat() < 0.7f) ? 30 : 70;

        		if (Vector2.Distance(player.Center, Main.MouseWorld) <= 500f)
        		{
            		position.X = Main.MouseWorld.X - (float)((int)(Math.Cos(rad) * dist));
        			position.Y = Main.MouseWorld.Y - (float)((int)(Math.Sin(rad) * dist));
            		this.projvelocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(position.X, position.Y)) * projspeed;
        		}
        		else
        		{
            		Vector2 perturbedSpeed = Utils.RotatedBy(new Vector2(velocity.X * 500f, velocity.Y * 500f), 0.0, default(Vector2));
            		position.X = player.Center.X + perturbedSpeed.X - (float)((int)(Math.Cos(rad) * dist));
            		position.Y = player.Center.Y + perturbedSpeed.Y - (float)((int)(Math.Sin(rad) * dist));
            		this.projvelocity = Vector2.Normalize(new Vector2(player.Center.X + perturbedSpeed.X, player.Center.Y + perturbedSpeed.Y) - new Vector2(position.X, position.Y)) * projspeed;
        		}
        		Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(this.projvelocity.X, this.projvelocity.Y), ModContent.ProjectileType<CrackedDaggerProj>(), newDamage, knockback, player.whoAmI, 0f, 0f, 0f);
    		}
    	return false;
		}

		private Vector2 projvelocity;
	}
}
