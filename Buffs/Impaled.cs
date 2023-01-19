using System;
using Terraria;
using Terraria.ModLoader;

namespace med.Buffs
{
	public class Impaled : ModBuff
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Impaled");
			base.Description.SetDefault("Ouch!");
			Main.debuff[base.Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.lifeRegen > 0)
			{
				npc.lifeRegen = 0;
			}
			int JavelinCount = 0;
			int impaleDamage = 0;
			for (int i = 0; i < 1000; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].GetGlobalProjectile<ImplaingProjectile>(true).CanImpale && Main.projectile[i].ai[0] == 1f && Main.projectile[i].ai[1] == (float)npc.whoAmI)
				{
					impaleDamage += Main.projectile[i].GetGlobalProjectile<ImplaingProjectile>(true).damagePerImpaler;
					JavelinCount++;
				}
			}
			npc.lifeRegen -= impaleDamage * 2;
			npc.lifeRegenExpectedLossPerSecond = impaleDamage;
		}
	}
}
