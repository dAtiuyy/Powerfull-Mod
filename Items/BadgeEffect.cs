using System;
using Terraria;
using med.Buffs;
using Terraria.ModLoader;

namespace med.Items
{
	public class BadgeEffect : ModPlayer
	{
		public override void ResetEffects()
		{
			this.critOnHit = false;
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (this.critOnHit)
			{
				base.Player.AddBuff(ModContent.BuffType<ImperialCourage>(), 240, true, false);
			}
		}

		public bool critOnHit;
	}
}
