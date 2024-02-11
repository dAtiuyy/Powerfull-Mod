using System;
using Lunarse.LunPlayer;
using Lunarse.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace Lunarse.Buffs
{
	public class SummonBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[base.Type] = true;
			Main.buffNoSave[base.Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			LunarsePlayer modPlayer = player.Lunarse();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SummonProj>()] > 0)
			{
				modPlayer.SummonBuff = true;
			}
			if (!modPlayer.SummonBuff)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			player.buffTime[buffIndex] = 18000;
		}
	}
}
