
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
    public class SIN : ModProjectile //�����˶���ѧ
    {
        Player player => Main.player[Projectile.owner];
        //�������ɵ�Ļʱ�����owner������Ӧ�����
        public override void SetStaticDefaults()//������ÿ�μ���ģ��ʱִ��һ�Σ����ڷ��侲̬����
        {
            Main.projFrames[Type] = 4;//���֡ͼ�ж���֡�������
            ProjectileID.Sets.TrailingMode[Type] = 2;//��һ�ֵ2���Լ�¼�˶��켣�ͷ�������������β��
            ProjectileID.Sets.TrailCacheLength[Type] = 10;//��һ������¼�Ĺ켣�����׷�ݵ�����֡��ǰ(ע�����ֵȡ����)
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;//��һ�����Ļ������Ļ����پ������ڿ��Ի���
                                                                //���ڳ����ε�Ļ����

            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;//����Ϊ������鳤��
            //ע��ϸ���ε�Ļǧ�����պ�«��ư�ѳ�����ͼ������Ϊ��ײ���ǹ̶��ģ�����������ͼ����ת����ת
            Projectile.friendly = true;//�ѷ���Ļ                                          
            Projectile.tileCollide = false;//false����������ǽ
            Projectile.timeLeft = 1220;//��ɢʱ��
            Projectile.aiStyle = -1;//��ʹ��ԭ��AI
            Projectile.DamageType = DamageClass.Ranged;//ħ���˺�
            Projectile.penetrate = 1;//��ʾ�ܴ�͸����
            Projectile.ignoreWater = true;//����Һ��
            base.SetDefaults();
        }
        Vector2 coord = Vector2.Zero;//����һ�������ֶ�
        float timer = 0;//����һ����ʱ���ֶ�
        float startVel = 0;//Ԥ���ٶȷ����ֶ�
        public override void AI()//�����ص�����AI
        {
            //AI�ڵĶ����ڷ���ͣ״̬�£�ÿִ֡��һ�Σ��ǵ�Ļ������Ϊ�Ļ���

            //�����������˶�,�����κ�AI������£���Ļ�ٶ��ǲ����ģ����������˶�����Ҫ�޸��ٶ�
            Projectile.rotation = Projectile.velocity.ToRotation();//����ͼ��������ٶȷ���

            if (coord == Vector2.Zero)//��Ļ���ɵ���һ�̣�����ֶ�Ĭ��Ϊ0���������Ǹ�������Ļ��ʱ������
            {
                startVel = Projectile.velocity.ToRotation();//��Ԥ������ٶȷ���
                Projectile.velocity *= 0;//�������ǲ����õ�Ļ�г��ٶ���
                coord = Projectile.Center;
            }
            else //֮��coord�Ѿ������赯Ļ�ĳ�ʼ�����ˣ���ʱ�Ϳ����õ�Ļ��������
            {
                timer++;//��ʱ��ÿ֡+1
                float height = 120;//���������˶����°ڶ�����
                float timeLoop = 30f;//���������˶�������
                float Xspeed = 15f;//���嵯Ļ�ں�����ٶ�
                float w = MathHelper.TwoPi / timeLoop;//��2���������ڵõ����ٶ�

                Vector2 pos = new Vector2(timer * Xspeed,//����ʱ��X�����겻������
                    height * (float)Math.Sin(timer * -w));//Y����������ʱ�����°ڶ�
                pos = pos.RotatedBy(startVel);//�ص㣺����������ͼ��ƫת�ɳ��ٶȵķ���
                Vector2 position = coord + pos;//�ټӻ�Ԥ��ĳ�ʼλ��
                Projectile.velocity = position - Projectile.Center;//������õ�Ļ�ٶ�һֱ���ź���ͼ����
            }
            #region �ٶȸ�������
            //�������ֱ�Ӷ��ٶȸ���������Ч��
            /* if (Projectile.timeLeft == 90)
             {
                 Projectile.velocity = new Vector2(0, 10);//������ʱʣ1.5s��ʱ���ٶȱ�Ϊ��ֱ����10����/֡
             }
             if (Projectile.timeLeft == 60)
             {
                 Projectile.velocity = new Vector2(-10, 0);//������ʱʣ1s��ʱ���ٶȱ�Ϊˮƽ����10����/֡
             }
             if (Projectile.timeLeft == 30)
             {
                 Projectile.velocity = new Vector2(10, -10);//������ʱʣ0.5s��ʱ���ٶȱ�Ϊ���Ϸ�45�㣬10�����Ŷ�����ÿ֡
             }*/
            #endregion
            #region �Ӽ����˶�
            //������ܼ����˶��ͼ����˶�
            //������ֱ�ӳɱ��ʵļӼ���
            //Projectile.velocity = Projectile.velocity * 0.95f;//�ٶ�ÿ֡����5%��һ��֮���ٶ�ֻʣԭ����4%��
            //����Ǿ���������ֵ�ļӼ��٣��������������һ��
            //Projectile.velocity = (Projectile.velocity.Length() + 0.1f) * Projectile.velocity.SafeNormalize(Vector2.Zero);
            //������ֵ���ٶ�����.length()�������Ȼ�ȡ�����ټ�һ���ٳ��Ե�λ�ٶ�,����������ÿ֡ + 1

            #endregion
            #region �����˶�
            //���ˣ�������ܵ�Ļ�ٶȽ���ƫת������������˶���
            // Projectile.velocity = Projectile.velocity.RotatedBy(0.034f);//�ٶ�ÿ֡˳ʱ��ƫת0.034����

            #endregion
            #region �����˶�
            //�������ֻ���ض���ʱ��ڵ�ƫת�ٶȣ���ô���ܳ��������˶�
            if (Projectile.timeLeft % 20 == 9)//ÿ20֡�еĵ�ʮִ֡�У�0�ǵ�һ֡��
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(-0.8f);//˳ʱ��ƫת0.8����
            }
            if (Projectile.timeLeft % 20 == 19)//ÿ20֡�еĵڶ�ʮִ֡�У�0�ǵ�һ֡��
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(0.8f);//��ʱ��ƫת0.8����
            }
            #endregion
            #region ���Һ����˶�
            //���ں����˶���Ҫһ��ԭ����Ϊ���գ����������½�һ����ά�����ֶ�
            //�����õ����ĺ����˶����������˶���
            //����дһ���򵥵�ˮƽ���ҵ������˶�
            /* if(coord == Vector2.Zero)//��Ļ���ɵ���һ�̣�����ֶ�Ĭ��Ϊ0���������Ǹ�������Ļ��ʱ������
             {
                 Projectile.velocity *= 0;//��ʱ�����ǲ���Ҫ�ٶ�
                 coord = Projectile.Center;
             }
             else //֮��coord�Ѿ������赯Ļ�ĳ�ʼ�����ˣ���ʱ�Ϳ����õ�Ļ��������
             {
                 timer++;//��ʱ��ÿ֡+1
                 float height = 250;//���������˶����°ڶ�����
                 float timeLoop = 30f;//���������˶�������
                 float Xspeed = 15f;//���嵯Ļ�ں�����ٶ�
                 float w = MathHelper.TwoPi / timeLoop;//��2���������ڵõ����ٶ�
                 Vector2 position = coord + //math.sin��������double������ǿ��ת����float
                     new Vector2(timer * Xspeed,//����ʱ��X�����겻������
                     height * (float)Math.Sin(timer * w));//Y����������ʱ�����°ڶ�
                 Projectile.velocity = position - Projectile.Center;//������õ�Ļ�ٶ�һֱ���ź���ͼ����
             }*/
            #endregion
            #region ������˼��
            //����������˶��ܺã������Ҳ�����ֻ����ˮƽ���ң��Ǹ���ô����?
            /*if (coord == Vector2.Zero)//��Ļ���ɵ���һ�̣�����ֶ�Ĭ��Ϊ0���������Ǹ�������Ļ��ʱ������
            {
                startVel = Projectile.velocity.ToRotation();//��Ԥ������ٶȷ���
                Projectile.velocity *= 0;//�������ǲ����õ�Ļ�г��ٶ���
                coord = Projectile.Center;
            }
            else //֮��coord�Ѿ������赯Ļ�ĳ�ʼ�����ˣ���ʱ�Ϳ����õ�Ļ��������
            {
                timer++;//��ʱ��ÿ֡+1
                float height = 60;//���������˶����°ڶ�����
                float timeLoop = 30f;//���������˶�������
                float Xspeed = 15f;//���嵯Ļ�ں�����ٶ�
                float w = MathHelper.TwoPi / timeLoop;//��2���������ڵõ����ٶ�

                Vector2 pos = new Vector2(timer * Xspeed,//����ʱ��X�����겻������
                    height * (float)Math.Sin(timer * w));//Y����������ʱ�����°ڶ�
                pos = pos.RotatedBy(startVel);//�ص㣺����������ͼ��ƫת�ɳ��ٶȵķ���
                Vector2 position = coord + pos;//�ټӻ�Ԥ��ĳ�ʼλ��
                Projectile.velocity = position - Projectile.Center;//������õ�Ļ�ٶ�һֱ���ź���ͼ����
            }*/

            #endregion
            #region Բ���˶�����Բ�˶�
            //��ô��ѧϰ�꼫���꣬�����Զ�ά������һ�����õ���ʶ��
            //�����������ҽ����������Χ��ĳ���Բ���˶���
            /*
            timer++;//���ǻ�����Ҫһ����ʱ���ģ��������ƽǶ�
            float rotation = timer * 0.25f;//�����Ը�С����Ϊ�Ƕ�ʹ��
            float Range = 150; //Բ�İ뾶
            Vector2 originPos = player.Center;//�����������Ϊԭ��Ϊ����
            Vector2 circlePos = rotation.ToRotationVector2() * Range;//ToRotVec2�������Խ��Ƕ�ת��λ�������ٳ��Գ���
                                                                     //�Ϳ��Եõ�Բ���ϵĵ�������
           //��ô��Բ�أ���ʵ���õ��ǲ������̵�˼�룬����Բ�ܵ�������X����Y����һ������
              //�ٽ�������ƫת������ԭ��������ӣ����ɵõ����ⷽ����Բ
            circlePos.Y *= 0.5f;//�����ｫԲ��Y�����50%���γ�һ����Բ
            circlePos = circlePos.RotatedBy(-0.5f);//����ƫת0.5�������Կ�
            Projectile.velocity = circlePos + originPos - Projectile.Center;//�ٶ��ٶȸ�ֵ
            */
            #endregion
            //���ˣ������˶��Ͷ���������
            //���棬�ҽ�������Ϸ�и��㷺ʹ�õ�һЩ�˶�����
            #region ׷���˶�
            //׷���˶���ָ��Ļ��ĳ�����꣨���겻ȷ��������׷�ϵ��˶�
            //����׷���˶�һ�㲻�������˶���ʱ�䣬����ֻ���Ծ�����ٶ���Ϊ�ж�����
            //׷���˶���ʵ�ַ����кܶ࣬�����ҽ������ֳ����İ취
            //��һ�֣�ֱ��׷��
            /*Vector2 targetPos = Main.MouseWorld;//Ϊ�˸�����ʾ����ѡ�������Ϊ��׷Ŀ��
            if ((targetPos - Projectile.Center).Length() > 8)//�����Ŀ��ľ��볬��һ��ֵ���˶�����ֹ�鴤
            {
                Vector2 vel = Projectile.DirectionTo(targetPos);
                //directionto��̩��ʵ���Դ��Ļ�ȡ��Ŀ�굥λ�����ķ�����
                //ֱ��׷�����ŵ����ٶȿɿأ������Լ��ã�ֱ��Ŀ�꣬ȱ�����ǽӽ�Ŀ������׳鴤,���ʺ���ΪNPC��׷���˶�
                vel *= MathHelper.Min(16, Projectile.Distance(targetPos));//�鴤������Ҫ���ٶ���Minѹ�����뻺��
                Projectile.velocity = vel;//��������ٶȽ�����Ļ
            }
            else
            {
                Projectile.velocity *= 0;//������ͣ�°�
            }
            */
            //�ڶ���:��������׷��   ��������׷����ԭ��ʹ�ý϶��һ��׷���ֶΣ��˶�ʮ����Ȼ���ʺϷ��е�λʹ��
            /*Vector2 targetPos = Main.MouseWorld;//Ϊ�˸�����ʾ����ѡ�������Ϊ��׷Ŀ��
            float MaxSpeed = 20f;//�趨����������ٶ�
            float accSpeed = 0.5f;//�趨��������ٶ�
            //ԭ���Ƚ�Ŀ����Լ��ĺ��������������Ȼ����Լ����ٶȼ������Ų�ֵ��Сǰ���ļ��ٶ�
            //����Լ����ٶ������һ����˵���Լ�����ԭ��Ŀ�꣬��Ҫ����ļ��ٶȣ��������趨����2��
            if (Projectile.Center.X - targetPos.X < 0f)
                Projectile.velocity.X += Projectile.velocity.X < 0 ? 2 * accSpeed : accSpeed;
            else
                Projectile.velocity.X -= Projectile.velocity.X > 0 ? 2 * accSpeed : accSpeed;

            if (Projectile.Center.Y - targetPos.Y < 0f)
                Projectile.velocity.Y += Projectile.velocity.Y < 0 ? 2 * accSpeed : accSpeed;
            else
                Projectile.velocity.Y -= Projectile.velocity.Y > 0 ? 2 * accSpeed : accSpeed;
            if (Math.Abs(Projectile.velocity.X) > MaxSpeed)//��������ٶȳ�Խ���ֵ����ص����ֵ
                Projectile.velocity.X = MaxSpeed * Math.Sign(Projectile.velocity.X);
            if (Math.Abs(Projectile.velocity.Y) > MaxSpeed)//��������ٶȳ�Խ���ֵ����ص����ֵ
                Projectile.velocity.Y = MaxSpeed * Math.Sign(Projectile.velocity.Y);*/

            //�����֣���ֵ�����˶�   ��ֵ�����˶���һ���ڶ�ʱ���ڿ����ȶ��ƶ���Ŀ�괦���˶����ʺ��ٻ���ǿ�ƻع����Ϊ

            #endregion
        }


        public override bool PreDraw(ref Color lightColor)//predraw����false���ɽ���ԭ�����
        {
            //ͬʱ����Ҫ���еĻ�����������д�ͺ�

            Texture2D texture = TextureAssets.Projectile[Type].Value;//��������Ļ�Ĳ���
            Rectangle rectangle = new Rectangle(//��Ϊ�ֶ�������Ҫ�Լ���д֡ͼ��,����Ҫ�������
                0,//���������Ͻǵ�ˮƽ����(��0�ͺ�)
                texture.Height / Main.projFrames[Type] * Projectile.frame,//������Ͻǵ��������� 
                texture.Width, //��Ŀ��(���ʿ�ȼ���)
                texture.Height / Main.projFrames[Type]//��ĸ߶ȣ��ò��ʸ߶ȳ���֡���õ���֡�߶ȣ�
                );

            //Ҫ������β������Ҫ����һ��forѭ����䣬��0һֱ�ߵ��켣ĩ��
            //�������ǽ���һ���ܲ����������ӻ��Ƶİ취��A=0��
            Color MyColor = Color.White; MyColor.A = 0;//��A=0��Ϊ����ֱ�ӵ�����ɫ
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type]; i++)//ѭ������С�ڹ켣����
            {
                float factor = 1 - (float)i / ProjectileID.Sets.TrailCacheLength[Type];
                //����һ�����µ�����1�𽥼��ٵ�0�ı���������i = 0ʱ��factor = 1
                Vector2 oldcenter = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                //���ڹ켣ֻ�ܼ�¼��Ļ��ײ�����Ͻ�λ�ã�����Ҫ�ֶ����ϵ�Ļ���һ������ȡ����
                Main.EntitySpriteDraw(texture, oldcenter, rectangle, MyColor * factor,//��ɫ�𽥱䵭
                    Projectile.oldRot[i],//��Ļ�켣�ϵ������ķ���
                    new Vector2(texture.Width / 2, texture.Height / 2 / Main.projFrames[Type]),
                     new Vector2(1, 1),
                     SpriteEffects.None, 0);
            }
            //����tr��������ִ�е��Ȼ��ƣ�����Ҫ���Ӱ�����ǵ��������棬��Ҫ��д��Ӱ����

            Main.EntitySpriteDraw(  //entityspritedraw�ǵ�Ļ��NPC�ȳ��õĻ��Ʒ���
                texture,//��һ�������ǲ���
                Projectile.Center - Main.screenPosition,//ע�⣬����ʱ��λ��������Ļ���Ͻ�Ϊ0��
                                                        //���Ҫ�õ�Ļ���������ȥ��Ļ���Ͻǵ�����
                rectangle,//��������������֡ͼѡ����
                Color.White,//���ĸ���������ɫ�������������Դ���lightcolor�������ܵ���Ȼ����Ӱ��
                Projectile.rotation,//�������������ͼ��ת����
                new Vector2(texture.Width / 2, texture.Height / 2 / Main.projFrames[Type]),
                //��������������ͼ����ԭ������꣬����дΪ��ͼ��֡���������꣬������ת�����Ŷ���Χ������
                new Vector2(1, 1),//���߸����������ţ�X��ˮƽ���ʣ�Y����ֱ����
                SpriteEffects.None,
                //�ڰ˸�����������ͼƬ��תЧ������Ҫ�ֶ��ж�������spriteeffects
                0//�ھŸ������ǻ��Ʋ㼶������0�����ˣ���̫��ʹ
                );

            return false;//return false��ֹ�Զ�����
        }
    }

}