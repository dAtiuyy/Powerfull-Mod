using Terraria;
using Terraria.ModLoader;

namespace med.Dusts
{
    public class item3Dust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false;
            dust.noLight = true;
            dust.scale = 1f;
        }
    }
}