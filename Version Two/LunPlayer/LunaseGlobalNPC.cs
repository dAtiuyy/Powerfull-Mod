using System;
using Lunarse.Items.Misc;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Lunarse;
using Lunarse.LunPlayer;

namespace LuiAFKUtl.LuiAFKCFG
{
	public class LunarseGlobalNPC : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			int type = npc.type;
			if (type <= 262)
			{
				if (type == 113)
				{
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LootMagnet>(), 2, 1, 1));
					return;
				}
				if (type != 262)
				{
					return;
				}
				npcLoot.Add(ItemDropRule.Common(947, 2, 100, 150));
				return;
			}
		}
		public override void ModifyShop(NPCShop shop)
		{
		}
		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
            LunarsePlayer lunarsePlayer = player.Lunarse();
			if (lunarsePlayer.BattleCry)
			{
				spawnRate = (int)((double)spawnRate * 0.1);
				maxSpawns = (int)((float)maxSpawns * 100f);
			}
			if (lunarsePlayer.CalmingCry)
			{
				spawnRate = (int)((float)spawnRate * 10f);
				maxSpawns = (int)((double)maxSpawns * 0.1);
			}
        }	
	}
}
