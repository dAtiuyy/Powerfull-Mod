using System;
using System.IO;
using Lunarse.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lunarse.Projectiles
{
	public class SummonProj : ModProjectile, ILocalizedModType, IModType
	{
		public unsafe float BladeHoverOffsetAngle
		{
			get
			{
				float projectileCounts = (float)this.Owner.ownedProjectileCounts[base.Type];
				if (projectileCounts <= 1f)
				{
					projectileCounts = 1f;
				}
				return 6.2831855f * (float)this.BladeIndex / projectileCounts + this.AITimer / 27f;
			}
		}
		public SummonProj.SummonProjAIState CurrentState
		{
			get
			{
				return (SummonProj.SummonProjAIState)base.Projectile.ai[0];
			}
			set
			{
				base.Projectile.ai[0] = (float)value;
			}
		}
		public Player Owner
		{
			get
			{
				return Main.player[base.Projectile.owner];
			}
		}
		public ref float AITimer
		{
			get
			{
				return ref base.Projectile.ai[1];
			}
		}
		public ref float BladeGleamInterpolant
		{
			get
			{
				return ref base.Projectile.localAI[0];
			}
		}
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionTargettingFeature[base.Type] = true;
			ProjectileID.Sets.TrailingMode[base.Type] = 2;
			ProjectileID.Sets.TrailCacheLength[base.Type] = 45;
		}
		public override void SetDefaults()
		{
			base.Projectile.width = 84;
			base.Projectile.height = 84;
			base.Projectile.netImportant = true;
			base.Projectile.friendly = true;
			base.Projectile.ignoreWater = true;
			base.Projectile.timeLeft = 90000;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 14;
			base.Projectile.tileCollide = false;
			base.Projectile.minion = true;
			base.Projectile.minionSlots = 1f;
			base.Projectile.DamageType = DamageClass.Summon;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(this.BladeIndex);
			Utils.WriteVector2(writer, this.ChargeStartingPosition);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			this.BladeIndex = reader.ReadInt32();
			this.ChargeStartingPosition = Utils.ReadVector2(reader);
		}
		public unsafe override void AI()
		{
			this.HandleMinionBools();
			base.Projectile.MaxUpdates = 1;
			this.BladeGleamInterpolant = MathHelper.Lerp(this.BladeGleamInterpolant, 0f, 0.1f);
			if (this.BladeGleamInterpolant <= 0.02f)
			{
				this.BladeGleamInterpolant = 0f;
			}
			NPC potentialTarget = base.Projectile.Center.MinionHoming(1550f, this.Owner, true, false);
			switch (this.CurrentState)
			{
			case SummonProj.SummonProjAIState.CircleOwner:
				this.DoBehavior_CircleOwner(potentialTarget);
				break;
			case SummonProj.SummonProjAIState.HorizontalSlashes:
				this.DoBehavior_HorizontalSlashes(potentialTarget);
				break;
			case SummonProj.SummonProjAIState.VerticalPierceTeleport:
				this.DoBehavior_VerticalPierceTeleport(potentialTarget);
				break;
			case SummonProj.SummonProjAIState.RegularPierceSlashes:
				this.DoBehavior_RegularPierceSlashes(potentialTarget);
				break;
			}
			this.AITimer += 1f;
		}
		public unsafe void DoBehavior_CircleOwner(NPC potentialTarget)
		{
			if (potentialTarget != null)
			{
				this.CurrentState = SummonProj.SummonProjAIState.HorizontalSlashes;
				this.AITimer = 0f;
				base.Projectile.netUpdate = true;
				return;
			}
			Vector2 hoverDestination = this.Owner.Center + Utils.ToRotationVector2(this.BladeHoverOffsetAngle) * 200f;
			base.Projectile.Center = Utils.MoveTowards(Vector2.Lerp(base.Projectile.Center, hoverDestination, 0.04f), hoverDestination, 16f);
			base.Projectile.velocity *= 0.8f;
			if (!base.Projectile.WithinRange(this.Owner.Center, 2500f))
			{
				base.Projectile.Center = hoverDestination;
				base.Projectile.netUpdate = true;
			}
			base.Projectile.rotation = base.Projectile.AngleFrom(this.Owner.Center) + 1.5707964f;
		}
		public unsafe void DoBehavior_HorizontalSlashes(NPC target)
		{
			int hoverTime = 22;
			int chargeTime = 14;
			if (target == null)
			{
				this.ReturnToIdleState();
				return;
			}
			base.Projectile.MaxUpdates = 2;
			float wrappedAttackTimer = this.AITimer % (float)(hoverTime + chargeTime);
			float fadeIn = Utils.GetLerpValue((float)hoverTime - 6f, (float)hoverTime, wrappedAttackTimer, true);
			float fadeOut = Utils.GetLerpValue((float)chargeTime, (float)chargeTime - 6f, wrappedAttackTimer - (float)hoverTime, true);
			this.BladeGleamInterpolant = fadeIn * fadeOut;
			if (wrappedAttackTimer < (float)hoverTime)
			{
				Vector2 hoverDestination = target.Center + Vector2.UnitX * (float)Utils.ToDirectionInt(target.Center.X < base.Projectile.Center.X) * 250f;
				hoverDestination.Y += (float)Math.Cos((double)((float)base.Projectile.identity * 1.7f)) * 67f;
				base.Projectile.Center = Utils.MoveTowards(Vector2.Lerp(base.Projectile.Center, hoverDestination, 0.08f), hoverDestination, 16f);
				base.Projectile.velocity *= 0.5f;
				base.Projectile.rotation = Utils.AngleLerp(base.Projectile.rotation, 0f, 0.08f);
			}
			else
			{
				base.Projectile.rotation += (float)Math.Sign(base.Projectile.velocity.X) * 3.1415927f / (float)chargeTime * 0.36f;
			}
			if (wrappedAttackTimer == (float)hoverTime)
			{
                SoundStyle meatySlashSound = new SoundStyle("Lunarse/Sounds/MeatySlash", 0);
				meatySlashSound.Pitch = 1.6f;
				meatySlashSound.Volume = 0.27f;
				SoundEngine.PlaySound(in meatySlashSound, new Vector2?(base.Projectile.Center), null);
				base.Projectile.oldPos = new Vector2[base.Projectile.oldPos.Length];
				base.Projectile.velocity = Vector2.UnitX * (float)Utils.ToDirectionInt(target.Center.X > base.Projectile.Center.X) * 44f;
				base.Projectile.netUpdate = true;
			}
			if (this.AITimer >= (float)((hoverTime + chargeTime) * 7))
			{
				this.AITimer = 0f;
				this.CurrentState = SummonProj.SummonProjAIState.VerticalPierceTeleport;
				base.Projectile.netUpdate = true;
			}
		}
		public unsafe void DoBehavior_VerticalPierceTeleport(NPC target)
		{
			int hoverTime = 26;
			int chargeTime = 32;
			if (target == null)
			{
				this.ReturnToIdleState();
				return;
			}
			base.Projectile.MaxUpdates = 2;
			float wrappedAttackTimer = (float)(((int)(this.AITimer) + base.Projectile.identity / 2) % (hoverTime + chargeTime));
			float fadeIn = Utils.GetLerpValue((float)hoverTime - 6f, (float)hoverTime, wrappedAttackTimer, true);
			float fadeOut = Utils.GetLerpValue((float)chargeTime, (float)chargeTime - 6f, wrappedAttackTimer - (float)hoverTime, true);
			this.BladeGleamInterpolant = fadeIn * fadeOut;
			if (wrappedAttackTimer < (float)hoverTime)
			{
				Vector2 hoverDestination = target.Center - Vector2.UnitY * 500f;
				hoverDestination.X += (float)Math.Cos((double)((float)base.Projectile.identity * 1.7f)) * 50f;
				base.Projectile.Center = Utils.MoveTowards(Vector2.Lerp(base.Projectile.Center, hoverDestination, 0.08f), hoverDestination, 16f);
				base.Projectile.velocity *= 0.5f;
				base.Projectile.rotation = Utils.AngleLerp(base.Projectile.rotation, base.Projectile.AngleTo(target.Center) + 1.5707964f, 0.24f);
			}
			if (wrappedAttackTimer == (float)hoverTime)
			{
				SoundStyle meatySlashSound = new SoundStyle("Lunarse/Sounds/MeatySlash", 0);
				meatySlashSound.Pitch = 1.6f;
				meatySlashSound.Volume = 0.27f;
				SoundEngine.PlaySound(in meatySlashSound, new Vector2?(base.Projectile.Center), null);
				base.Projectile.oldPos = new Vector2[base.Projectile.oldPos.Length];
				base.Projectile.velocity = Vector2.UnitY * 45f;
				base.Projectile.netUpdate = true;
			}
			if (wrappedAttackTimer >= (float)hoverTime + 5f && base.Projectile.Center.Y > target.Center.Y + 850f)
			{
				base.Projectile.oldPos = new Vector2[base.Projectile.oldPos.Length];
				base.Projectile.Center = target.Center - Vector2.UnitY * 850f * 0.7f;
				base.Projectile.netUpdate = true;
			}
			if (this.AITimer >= (float)((hoverTime + chargeTime) * 7))
			{
				this.AITimer = 0f;
				this.CurrentState = SummonProj.SummonProjAIState.RegularPierceSlashes;
				base.Projectile.netUpdate = true;
			}
		}
		public unsafe void DoBehavior_RegularPierceSlashes(NPC target)
		{
			int attackCycleTime = 44;
			float upwardRiseTimeRatio = 0.4f;
			float pierceTimeRatio = 0.14f;
			if (target == null)
			{
				this.ReturnToIdleState();
				this.ChargeStartingPosition = Vector2.Zero;
				return;
			}
			int num = (int)(this.AITimer) + base.Projectile.identity;
			if ((float)(num % attackCycleTime) == 1f)
			{
				this.ChargeStartingPosition = base.Projectile.Center + Utils.NextVector2Circular(Main.rand, 80f, 80f);
				base.Projectile.netUpdate = true;
			}
			float attackCompletion = (float)num / (float)attackCycleTime % 1f;
			if (attackCompletion < upwardRiseTimeRatio)
			{
				base.Projectile.oldPos = new Vector2[base.Projectile.oldPos.Length];
			}
			base.Projectile.MaxUpdates = 2;
			float offsetDistanceFactor = MathHelper.Lerp(1.61f, 3f, (float)base.Projectile.identity / 7f % 1f);
			Vector2 startingPosition = this.ChargeStartingPosition + Vector2.UnitY * Utils.GetLerpValue(0f, upwardRiseTimeRatio, attackCompletion, true) * -200f;
			Vector2 targetOffset = target.Center - startingPosition;
			Vector2 endingPosition = target.Center + Utils.SafeNormalize(targetOffset, Vector2.Zero) * MathHelper.Clamp(targetOffset.Length(), 60f, 240f) * offsetDistanceFactor;
			float pierceCompletion = Utils.GetLerpValue(upwardRiseTimeRatio, upwardRiseTimeRatio + pierceTimeRatio, attackCompletion, true);
			float throughTargetCompletion = Utils.GetLerpValue(upwardRiseTimeRatio + pierceTimeRatio, 1f, attackCompletion, true);
			base.Projectile.rotation = Utils.AngleTowards(base.Projectile.rotation, Utils.ToRotation(targetOffset) + 1.5707964f, 0.62831855f);
			base.Projectile.Center = Vector2.Lerp(base.Projectile.Center, Vector2.Lerp(startingPosition, target.Center, pierceCompletion), pierceCompletion * 0.5f);
			if (throughTargetCompletion > 0f)
			{
				base.Projectile.Center = Vector2.Lerp(target.Center, endingPosition, throughTargetCompletion);
			}
			base.Projectile.velocity = Vector2.Zero;
			if (num % attackCycleTime == (int)((float)attackCycleTime * upwardRiseTimeRatio))
			{
				SoundStyle meatySlashSound = new SoundStyle("Lunarse/Sounds/MeatySlash", 0);
				meatySlashSound.Pitch = 1.6f;
				meatySlashSound.Volume = 0.27f;
				SoundEngine.PlaySound(in meatySlashSound, new Vector2?(base.Projectile.Center), null);
			}
			if (num >= attackCycleTime * 7)
			{
				this.AITimer = 0f;
				this.CurrentState = SummonProj.SummonProjAIState.HorizontalSlashes;
				base.Projectile.netUpdate = true;
			}
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x00123682 File Offset: 0x00121882
		public unsafe void ReturnToIdleState()
		{
			this.AITimer = 0f;
			this.CurrentState = SummonProj.SummonProjAIState.CircleOwner;
			base.Projectile.netUpdate = true;
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x001236A4 File Offset: 0x001218A4
		public void HandleMinionBools()
		{
			this.Owner.AddBuff(ModContent.BuffType<SummonBuff>(), 3600, true, false);
			if (base.Projectile.type == ModContent.ProjectileType<SummonProj>())
			{
				if (this.Owner.dead)
				{
					this.Owner.Lunarse().SummonBuff = false;
				}
				if (this.Owner.Lunarse().SummonBuff)
				{
					base.Projectile.timeLeft = 2;
				}
			}
		}
		public Color TrailColorFunction(float completionRatio)
		{
			float opacity = (float)Math.Pow((double)Utils.GetLerpValue(1f, 0.45f, completionRatio, true), 4.0) * base.Projectile.Opacity * 0.48f;
			return Color.Lerp(new Color(115, 196, 127), Color.Yellow, MathHelper.Clamp(completionRatio * 1.4f, 0f, 1f)) * opacity;
		}
		public float TrailWidthFunction(float completionRatio)
		{
			return (float)base.Projectile.height * (1f - completionRatio) * 0.8f;
		}
		public unsafe override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = ModContent.Request<Texture2D>(this.Texture).Value;
			Rectangle frame = Utils.Frame(value, 1, Main.projFrames[base.Type], 0, base.Projectile.frame, 0, 0);
			Vector2 origin = Utils.Size(frame) * 0.5f;
			Vector2 drawPosition = base.Projectile.Center - Main.screenPosition;
			//SpriteEffects direction = (base.Projectile.spriteDirection == 1) ? 1 : 0;
			SpriteEffects direction = (base.Projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			if (this.TrailDrawer == null)
			{
				this.TrailDrawer = new VertexStrip();
			}
			GameShaders.Misc["EmpressBlade"].UseImage0("Images/Extra_201");
			GameShaders.Misc["EmpressBlade"].UseImage1("Images/Extra_193");
			GameShaders.Misc["EmpressBlade"].UseShaderSpecificData(new Vector4(1f, 0f, 0f, 0.6f));
			GameShaders.Misc["EmpressBlade"].Apply(default(DrawData?));
			this.TrailDrawer.PrepareStrip(base.Projectile.oldPos, base.Projectile.oldRot, new VertexStrip.StripColorFunction(this.TrailColorFunction), new VertexStrip.StripHalfWidthFunction(this.TrailWidthFunction), base.Projectile.Size * 0.5f - Main.screenPosition, new int?(base.Projectile.oldPos.Length), true);
			this.TrailDrawer.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			Main.EntitySpriteDraw(value, drawPosition, new Rectangle?(frame), base.Projectile.GetAlpha(lightColor), base.Projectile.rotation + 0.2f, origin, base.Projectile.scale, direction, 0f);
			Texture2D shineTex = ModContent.Request<Texture2D>("Lunarse/Particles/HalfStar").Value;
			Vector2 shineScale = new Vector2(1.67f, 3f) * base.Projectile.scale;
			shineScale *= MathHelper.Lerp(0.9f, 1.1f, (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly * 7.4f + (float)base.Projectile.identity)) * 0.5f + 0.5f);
			Vector2 lensFlareWorldPosition = base.Projectile.Center + Utils.ToRotationVector2(base.Projectile.rotation - 1.5707964f) * (float)base.Projectile.width * base.Projectile.scale * 0.88f;
			Color color = Color.Lerp(Color.LimeGreen, Color.Yellow, 0.23f);
			color.A = 0;
			Color lensFlareColor = color * this.BladeGleamInterpolant;
			Main.EntitySpriteDraw(shineTex, lensFlareWorldPosition - Main.screenPosition, default(Rectangle?), lensFlareColor, 0f, Utils.Size(shineTex) * 0.5f, shineScale * 0.6f, 0, 0f);
			Main.EntitySpriteDraw(shineTex, lensFlareWorldPosition - Main.screenPosition, default(Rectangle?), lensFlareColor, 1.5707964f, Utils.Size(shineTex) * 0.5f, shineScale, 0, 0f);
			GameShaders.Misc["EmpressBlade"].UseImage0("Images/Extra_209");
			GameShaders.Misc["EmpressBlade"].UseImage1("Images/Extra_210");
			return false;
		}

		// Token: 0x04000727 RID: 1831
		public int BladeIndex;

		// Token: 0x04000728 RID: 1832
		public VertexStrip TrailDrawer;

		// Token: 0x04000729 RID: 1833
		public Vector2 ChargeStartingPosition;

		// Token: 0x02001AE3 RID: 6883
		public enum SummonProjAIState
		{
			// Token: 0x040023E3 RID: 9187
			CircleOwner,
			HorizontalSlashes,
			VerticalPierceTeleport,
			RegularPierceSlashes
		}
	}
}
