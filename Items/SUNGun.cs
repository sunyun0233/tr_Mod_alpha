﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MyMod.Items
{
	public class SUNGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("烈阳枪"); //为你的武器命名
			Tooltip.SetDefault("三足金乌之魂凝练而成,用特殊弹药生效");//这是武器的提示语，利用\n进行换行
		}
		public override void SetDefaults()
		{
			//以下是武器物品的基本属性
			Item.damage = 100;//物品的基础伤害
			Item.crit = 96;//物品的暴击率
			Item.DamageType = DamageClass.Ranged;//物品的伤害类型
			Item.width = 40;//物品以掉落物形式存在的碰撞箱宽度
			Item.height = 40;//物品以掉落物形式存在的碰撞箱高度
			Item.useTime = 5;//物品一次使用所经历的时间（以帧为单位）(正常情况1秒60帧)
			Item.shoot = ProjectileID.DD2PhoenixBowShot;//物品发射的弹幕ID(玛瑙炮)
			Item.shootSpeed = 24f;//物品发射的弹幕速度（像素/帧）（一个物块长16像素）
			Item.useAnimation = 5;//物品播放使用动画所经历的时间
			Item.useStyle = ItemUseStyleID.Shoot;//使用动作 swing为挥舞 shoot为射击
			Item.knockBack = 2;//物品击退
			Item.value = Item.buyPrice(1,22,0,0);//价值  buyprice方法可以直接设置成直观的钱币数
			Item.rare = ItemRarityID.Pink;//稀有度
			Item.UseSound = SoundID.Item22;//使用时的声音
			Item.autoReuse = true;//自动连发
            Item.useAmmo = AmmoID.Bullet;
            Item.noUseGraphic = false;//为true时会隐藏物品使用动画
			Item.noMelee = true;//为true时会取消物品近战判定
			
			Item.mana = 0;//为大于零的数时每次使用会消耗魔力值
			Item.scale = 1.2f;//物品使用动画的大小
		}
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
			//本函数是武器近战挥舞时触发的操作，通常为生成粒子
			//Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Torch, 0, 0, 0, default, 2);
            base.MeleeEffects(player, hitbox);
        }
        
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -48; i <= 48; i++)
            {
                float rotation = (float)i * (MathHelper.Pi / 48);
                Projectile p = Projectile.NewProjectileDirect(source, position, velocity,
                ProjectileID.Flamelash//烈焰火鞭是魔法伤害弹幕，我们需要把它改成远程
                , damage, knockback, player.whoAmI);//NewProjectileDirect方法可以直接定义生成出来的这个弹幕
				 
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(0.2f), type == ProjectileID.Bullet ? ProjectileID.DD2PhoenixBowShot : type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(-0.2f), type == ProjectileID.Bullet ? ProjectileID.DD2PhoenixBowShot : type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(0.4f), type == ProjectileID.Bullet ? ProjectileID.Flamelash : type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(-0.4f), type == ProjectileID.Bullet ? ProjectileID.Flamelash : type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(0.6f), type == ProjectileID.Bullet ? ProjectileID.DD2PhoenixBowShot : type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(-0.6f), type == ProjectileID.Bullet ? ProjectileID.DD2PhoenixBowShot : type, damage, knockback, player.whoAmI);
                p.hostile = false;
                p.friendly = true;
                p.DamageType = DamageClass.Ranged;
                return true;
            }
            return false;
         }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			//本函数为近战攻击到NPC时进行的操作，通常为产生弹幕或者BUFF
			target.AddBuff(BuffID.OnFire, 120);//addbuff方法第一个参数为要上的BUFFID，第二个为持续时间（帧）
			player.AddBuff(BuffID.NebulaUpLife3, 30);//为玩家添加半秒星云回复BUFF
			//远程武器就不需要了
            base.OnHitNPC(player, target, damage, knockBack, crit);
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();//创建一个配方
			recipe.AddIngredient(ItemID.Torch, 10);//加入材料(10火把)
			recipe.AddIngredient(ItemID.Wood, 10);//添加第二种材料（10木材）
			recipe.AddTile(TileID.Campfire);//加入合成站(这里为了有趣我改成了篝火)
			recipe.Register();
		}
	}
}