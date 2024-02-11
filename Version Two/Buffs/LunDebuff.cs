using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Lunarse;
using System;

namespace Lunarse.Buffs
{
    public class LunDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Increase the damage rate to make it 10 times stronger
            player.GetModPlayer<LunPlayer.LunarsePlayer>().customBurn = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen *= -100;
            npc.defense *= -100;
            npc.AddBuff(BuffID.OnFire, 1000);
            npc.AddBuff(BuffID.OnFire3, 1000);
            npc.AddBuff(BuffID.Chilled, 1000);
			for (int i = 0; i < 1; i++)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fireworks, 0f, 0f, 0);
				Main.dust[dust].noGravity = true;
			}
            base.Update(npc, ref buffIndex);
        }
    }
}
