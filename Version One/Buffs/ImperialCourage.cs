using System;
using Terraria;
using Terraria.ModLoader;

namespace med.Buffs

{
    public class ImperialCourage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Imperial Courage");
            base.Description.SetDefault("25% increased critical chance");
            Main.debuff[base.Type] = false;
            Main.pvpBuff[base.Type] = false;
            Main.buffNoSave[base.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 25f;
        }
    }
}
