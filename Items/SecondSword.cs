using Microsoft.Xna.Framework;
using MyMod.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MyMod.Items
{
	public class SecondSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("���齣"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("���׵�Ļ����ʾ���Լ�buffʾ��");
		}

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.shoot = ModContent.ProjectileType<ModProj.LIG>();//������׵�Ļ
			Item.shootSpeed = 9.8f;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			target.AddBuff(ModContent.BuffType<NPCdebuff>(), 100);//��NPC��Debuff
            base.OnHitNPC(player, target, damage, knockBack, crit);
        }
    }
}