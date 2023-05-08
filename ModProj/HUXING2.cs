
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MyMod.ModProj
{
    public class HUXING2 : ModProjectile //基础运动教学
    {   
        Player player => Main.player[Projectile.owner];
        //定义生成弹幕时传入的owner参数对应的玩家
        public override void SetStaticDefaults()//本函数每次加载模组时执行一次，用于分配静态属性
        {
            Main.projFrames[Type] = 1;//你的帧图有多少帧就填多少
            ProjectileID.Sets.TrailingMode[Type] = 2;//这一项赋值2可以记录运动轨迹和方向（用于制作拖尾）
            ProjectileID.Sets.TrailCacheLength[Type] = 10;//这一项代表记录的轨迹最多能追溯到多少帧以前(注意最大值取不到)
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;//这一项代表弹幕超过屏幕外多少距离以内可以绘制
                                                                //用于长条形弹幕绘制
            
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;//长宽为两格物块长度
            //注意细长形弹幕千万不能照葫芦画瓢把长宽按贴图设置因为碰撞箱是固定的，不会随着贴图的旋转而旋转
            Projectile.friendly = true;//友方弹幕                                          
            Projectile.tileCollide = false;//false就能让他穿墙
            Projectile.timeLeft = 1220;//消散时间
            Projectile.aiStyle = -1;//不使用原版AI
            Projectile.DamageType = DamageClass.Ranged;//魔法伤害
            Projectile.penetrate = 1;//表示能穿透几次
            Projectile.ignoreWater = true;//无视液体
            base.SetDefaults();
        }
        Vector2 coord = Vector2.Zero;//定义一个坐标字段
        float timer = 0;//定义一个计时器字段
        float startVel = 0;//预存速度方向字段
        public override void AI()//本次重点在于AI
        {
            //AI内的东西在非暂停状态下，每帧执行一次，是弹幕表现行为的基础
            Projectile.rotation = Projectile.velocity.ToRotation();
            //首先是匀速运动,在无任何AI的情况下，弹幕速度是不会变的，所以匀速运动不需要修改速度
            if (coord == Vector2.Zero)//弹幕生成的那一刻，这个字段默认为0向量，我们赋予它弹幕此时的坐标
            {
                startVel = Projectile.velocity.ToRotation();//先预存这个速度方向
                Projectile.velocity *= 0;//但是我们不能让弹幕有初速度了
                coord = Projectile.Center;
            }
            else //之后coord已经被赋予弹幕的初始坐标了，此时就可以让弹幕动起来了
            {
                timer++;//我们还是需要一个计时器的，用来控制角度
                float rotation = timer * 0.15f;//随便乘以个小数作为角度使用
                float Range = timer * 5; //圆的半径
                Vector2 originPos = player.Center;//（以玩家中心为原点为例）
                Vector2 circlePos = rotation.ToRotationVector2() * Range;
                originPos += circlePos;
                Projectile.velocity = originPos - Projectile.Center;
                //至此，基础运动就都介绍完了
                //下面，我将介绍游戏中更广泛使用的一些运动方法
            }
        }


        public override bool PreDraw(ref Color lightColor)//predraw返回false即可禁用原版绘制
        {
            //同时，需要进行的绘制在这里面写就好
            
            Texture2D texture = TextureAssets.Projectile[Type].Value;//声明本弹幕的材质
            Rectangle rectangle = new Rectangle(//因为手动绘制需要自己填写帧图框,所以要先算出来
                0,//这个框的左上角的水平坐标(填0就好)
                texture.Height / Main.projFrames[Type] * Projectile.frame,//框的左上角的纵向坐标 
                texture.Width, //框的宽度(材质宽度即可)
                texture.Height / Main.projFrames[Type]//框的高度（用材质高度除以帧数得到单帧高度）
                );

            //要制作拖尾，首先要建立一个for循环语句，从0一直走到轨迹末端
            //这里我们介绍一个能产生高亮叠加绘制的办法（A=0）
            Color MyColor = Color.White; MyColor.A = 0;//让A=0是为了能直接叠加颜色
            for (int i = 0;i < ProjectileID.Sets.TrailCacheLength[Type]; i++)//循环上限小于轨迹长度
            {
                float factor = 1 - (float)i / ProjectileID.Sets.TrailCacheLength[Type];
                //定义一个从新到旧由1逐渐减少到0的变量，比如i = 0时，factor = 1
                Vector2 oldcenter = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                //由于轨迹只能记录弹幕碰撞箱左上角位置，我们要手动加上弹幕宽高一半来获取中心
                Main.EntitySpriteDraw(texture, oldcenter, rectangle, MyColor * factor,//颜色逐渐变淡
                    Projectile.oldRot[i],//弹幕轨迹上的曾经的方向
                    new Vector2(texture.Width / 2, texture.Height / 2 / Main.projFrames[Type]),
                     new Vector2(1, 1),
                     SpriteEffects.None, 0);
            }
            //由于tr绘制是先执行的先绘制，所以要想残影不覆盖到本体上面，就要先写残影绘制

            Main.EntitySpriteDraw(  //entityspritedraw是弹幕，NPC等常用的绘制方法
                texture,//第一个参数是材质
                Projectile.Center - Main.screenPosition,//注意，绘制时的位置是以屏幕左上角为0点
                //因此要用弹幕世界坐标减去屏幕左上角的坐标
                rectangle,//第三个参数就是帧图选框了
                Color.White,//第四个参数是颜色，这里我们用自带的lightcolor，可以受到自然光照影响
                Projectile.rotation,//第五个参数是贴图旋转方向
                new Vector2(texture.Width / 2, texture.Height / 2 / Main.projFrames[Type]),
                //第六个参数是贴图参照原点的坐标，这里写为贴图单帧的中心坐标，这样旋转和缩放都是围绕中心
                new Vector2(1, 1),//第七个参数是缩放，X是水平倍率，Y是竖直倍率
                SpriteEffects.None,
                //第八个参数是设置图片翻转效果，需要手动判定并设置spriteeffects
                0//第九个参数是绘制层级，但填0就行了，不太好使
                );
            
            return false;//return false阻止自动绘制
        }
    }
   
}