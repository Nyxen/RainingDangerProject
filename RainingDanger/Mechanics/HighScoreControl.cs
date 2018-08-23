using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroLibrary.Xna.Graphic.Sprites;

namespace RainingDanger.Mechanics
{
    public class HighScoreControl : Sprite
    {
        public static Dictionary<ControlTypes, Texture2D> Images;

        private ControlTypes _control;

        public ControlTypes Control
        {
            get
            {
                return _control;
            }
            set
            {
                _control = value;
                _isVisible = _control != ControlTypes.NoControl;
                if(_control != ControlTypes.NoControl)
                {
                    Image = Images[_control];
                }
            }
        }

        public HighScoreControl(string name, ControlTypes control, Vector2 position):
            base(name, Images[ControlTypes.Tap], position, Color.White)
        {
            Control = control;
        }
    }
}
