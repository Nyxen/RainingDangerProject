using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroLibrary.Xna.Graphic.Sprites;
using Microsoft.Xna.Framework;
using ZeroLibrary;
using ZeroLibrary.Xna.Utils;

namespace RainingDanger.Mechanics
{
    public class FallingItem : Sprite
    {
        public static Rotation RotationSpeed = Rotation.FromDegree(3);
        public static float FallingSpeed;
        public static float FadingSpeed = 0.1f;
        private float ammount;


        private Item _item;

        public Item Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                Image = _item.Image;
                _rotation = Rotation.FromDegree(0);
                Origin = new Vector2(Image.Width / 2, Image.Height / 2);
            }
        }

        private Action<Sprite> _playerHit;
        public event EventHandler FadeComplete;

        private bool _fade;
        public bool Fade
        {
            get
            {
                return _fade;
            }
            set
            {
                _fade = value;
                if (_fade == false)
                {
                    _color = Color.White;
                    ammount = 0;
                }
            }
        }

        public FallingItem(string name, Item item, Vector2 position, Action<Sprite> playerHit) :
            base(name, item.Image, position, Color.White, Rotation.FromRadian(0), new Vector2(item.Image.Width / 2, item.Image.Height / 2))
        {
            ammount = 0;
            _fade = false;
            _item = item;
            _playerHit = playerHit;
            FadeComplete += FallingItem_FadeComplete;
        }

        void FallingItem_FadeComplete(object sender, EventArgs e)
        {
            //a pass through event
        }

        public override void Update(GameTime gameTime)
        {
            if (_isVisible)
            {
                if (_item.CanRotate)
                {
                    _rotation += RotationSpeed;
                }

                if (Fade)
                {
                    ammount += FadingSpeed;
                    Color = Color.Lerp(Color.White, Color.Transparent, ammount);//new Color(Color.R, Color.G, Color.B, Color.A - FadingSpeed);
                    if (Color == Color.Transparent)
                    {
                        FadeComplete(this, EventArgs.Empty);
                    }
                }
                Y += FallingSpeed;
                base.Update(gameTime);
            }
        }

        public void PlayerHit(Sprite player)
        {
            _playerHit(player);
        }
    }
}