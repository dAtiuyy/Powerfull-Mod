using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Lunarse.LunPlayer;
using Lunarse.Items;
using Lunarse.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.Prefixes;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;
using Terraria.UI.Chat;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Lunarse
{
	public static class LunarseUtils
	{
		public static void PrintText(string text)
		{
			LunarseUtils.PrintText(text, Color.White);
		}
		public static void PrintText(string text, Color color)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.NewText(text, new Color?(color));
				return;
			}
			if (Main.netMode == NetmodeID.Server)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color, -1);
			}
		}
		public static void PrintText(string text, int r, int g, int b)
		{
			LunarseUtils.PrintText(text, new Color(r, g, b));
		}
		public static bool WithinBounds(this int index, int cap)
		{
			return index >= 0 && index < cap;
		}
        public static LunarsePlayer Lunarse(this Player player)
		{
			return player.GetModPlayer<LunarsePlayer>();
		}
		public static NPC MinionHoming(this Vector2 origin, float maxDistanceToCheck, Player owner, bool ignoreTiles = true, bool checksRange = false)
		{
			if (owner == null || !owner.whoAmI.WithinBounds(255) || !owner.MinionAttackTargetNPC.WithinBounds(Main.maxNPCs))
			{
				return origin.ClosestNPCAt(maxDistanceToCheck, ignoreTiles, false);
			}
			NPC npc = Main.npc[owner.MinionAttackTargetNPC];
			bool canHit = true;
			if (!ignoreTiles)
			{
				canHit = Collision.CanHit(origin, 1, 1, npc.Center, 1, 1);
			}
			float extraDistance = (float)(npc.width / 2 + npc.height / 2);
			bool distCheck = Vector2.Distance(origin, npc.Center) < maxDistanceToCheck + extraDistance || !checksRange;
			if (owner.HasMinionAttackTargetNPC && canHit && distCheck)
			{
				return npc;
			}
			return origin.ClosestNPCAt(maxDistanceToCheck, ignoreTiles, false);
		}
		// Token: 0x060002C9 RID: 713 RVA: 0x0002B3F8 File Offset: 0x000295F8
		public static void HomeInOnNPC(Projectile projectile, bool ignoreTiles, float distanceRequired, float homingVelocity, float N)
		{
			if (!projectile.friendly)
			{
				return;
			}
			Vector2 destination = projectile.Center;
			bool locatedTarget = false;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				float extraDistance = (float)(Main.npc[i].width / 2 + Main.npc[i].height / 2);
				if (Main.npc[i].CanBeChasedBy(projectile, false) && projectile.WithinRange(Main.npc[i].Center, distanceRequired + extraDistance) && (ignoreTiles || Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1)))
				{
					destination = Main.npc[i].Center;
					locatedTarget = true;
					break;
				}
			}
			if (locatedTarget)
			{
				Vector2 homeDirection = Utils.SafeNormalize(destination - projectile.Center, Vector2.UnitY);
				projectile.velocity = (projectile.velocity * N + homeDirection * homingVelocity) / (N + 1f);
				return;
			}
		}
		public static NPC ClosestNPCAt(this Vector2 origin, float maxDistanceToCheck, bool ignoreTiles = true, bool bossPriority = false)
		{
			NPC closestTarget = null;
			float distance = maxDistanceToCheck;
			if (bossPriority)
			{
				bool bossFound = false;
				for (int index = 0; index < Main.npc.Length; index++)
				{
					if ((!bossFound || Main.npc[index].boss || Main.npc[index].type == NPCID.WallofFleshEye) && Main.npc[index].CanBeChasedBy(null, false))
					{
						float extraDistance = (float)(Main.npc[index].width / 2 + Main.npc[index].height / 2);
						bool canHit = true;
						if (extraDistance < distance && !ignoreTiles)
						{
							canHit = Collision.CanHit(origin, 1, 1, Main.npc[index].Center, 1, 1);
						}
						if (Vector2.Distance(origin, Main.npc[index].Center) < distance + extraDistance && canHit)
						{
							if (Main.npc[index].boss || Main.npc[index].type == NPCID.WallofFleshEye)
							{
								bossFound = true;
							}
							distance = Vector2.Distance(origin, Main.npc[index].Center);
							closestTarget = Main.npc[index];
						}
					}
				}
			}
			else
			{
				for (int index2 = 0; index2 < Main.npc.Length; index2++)
				{
					if (Main.npc[index2].CanBeChasedBy(null, false))
					{
						float extraDistance2 = (float)(Main.npc[index2].width / 2 + Main.npc[index2].height / 2);
						bool canHit2 = true;
						if (extraDistance2 < distance && !ignoreTiles)
						{
							canHit2 = Collision.CanHit(origin, 1, 1, Main.npc[index2].Center, 1, 1);
						}
						if (Vector2.Distance(origin, Main.npc[index2].Center) < distance + extraDistance2 && canHit2)
						{
							distance = Vector2.Distance(origin, Main.npc[index2].Center);
							closestTarget = Main.npc[index2];
						}
					}
				}
			}
			return closestTarget;
		}
		public static T ModProjectile<T>(this Projectile projectile) where T : ModProjectile
		{
			return projectile.ModProjectile as T;
		}
		public static Projectile ProjectileRain(IEntitySource source, Vector2 targetPos, float xLimit, float xVariance, float yLimitLower, float yLimitUpper, float projSpeed, int projType, int damage, float knockback, int owner)
		{
			float x = targetPos.X + Utils.NextFloat(Main.rand, -xLimit, xLimit);
			float y = targetPos.Y - Utils.NextFloat(Main.rand, yLimitLower, yLimitUpper);
			Vector2 spawnPosition = new Vector2(x, y);
			Vector2 velocity = targetPos - spawnPosition;
			velocity.X += Utils.NextFloat(Main.rand, -xVariance, xVariance);
			float targetDist = velocity.Length();
			targetDist = projSpeed / targetDist;
			velocity.X *= targetDist;
			velocity.Y *= targetDist;
			return Projectile.NewProjectileDirect(source, spawnPosition, velocity, projType, damage, knockback, owner, 0f, 0f, 0f);
		}
		public static Projectile ProjectileCircle(IEntitySource source, Vector2 playerPos, float radius, float angleOffset, float projSpeed, int projType, int damage, float knockback, int owner)
		{
    	// Calculate the angle based on the current time and an offset
    	float angle = angleOffset + (float)Main.GlobalTimeWrappedHourly * MathHelper.TwoPi;

   		// Calculate the spawn position based on the player's position and the circle's radius
    	float x = playerPos.X + (float)Math.Cos(angle) * radius;
    	float y = playerPos.Y + (float)Math.Sin(angle) * radius;

    	Vector2 spawnPosition = new Vector2(x, y);

    	// Calculate the velocity to make the projectile orbit the player
    	Vector2 velocity = playerPos - spawnPosition;
    	velocity.Normalize();
    	velocity *= projSpeed;

    	// Create and return the projectile
    	return Projectile.NewProjectileDirect(source, spawnPosition, velocity, projType, damage, knockback, owner, 0f, 0f, 0f);
		}


		public static StatModifier GetBestClassDamage(this Player player)
		{
			StatModifier ret = StatModifier.Default;
			StatModifier classless = player.GetTotalDamage<GenericDamageClass>();
			ret.Base = classless.Base;
			ret *= classless.Multiplicative;
			ret.Flat = classless.Flat;
			float best = 1f;
			float melee = player.GetTotalDamage<MeleeDamageClass>().Additive;
			if (melee > best)
			{
				best = melee;
			}
			float ranged = player.GetTotalDamage<RangedDamageClass>().Additive;
			if (ranged > best)
			{
				best = ranged;
			}
			float magic = player.GetTotalDamage<MagicDamageClass>().Additive;
			if (magic > best)
			{
				best = magic;
			}
			float summon = player.GetTotalDamage<SummonDamageClass>().Additive;
			if (summon > best)
			{
				best = summon;
			}
			return ret + (best - 1f);
		}
		public static int ApplyArmorAccDamageBonusesTo(this Player player, float damage)
		{
			return (int)damage;
		}
    }
}