using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace med.Items
{
	public class SkywardHiltEffect : ModPlayer
	{
		public override void ResetEffects()
		{
			this.effect = false;
		}

		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			
		}

		public bool effect;
	}
}
