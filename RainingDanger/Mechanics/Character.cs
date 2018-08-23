using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroLibrary.Xna.Graphic.Sprites;
using ZeroLibrary.Xna.Interfaces;

using ZeroLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RainingDanger.Mechanics
{
    public enum CharacterState
    {
        Idle = 0,
        Moving = 1,
        Death = 2
    }

    public class Character : ISprite
    {
        Dictionary<CharacterState, ISprite> _characterStates;

        Dictionary<CharacterState, ISprite>[] _characterAnimation;
        
        CharacterState _currentState;

        public Character(params ISprite[] sprites)
        {
            _characterStates = new Dictionary<CharacterState, ISprite>();
            _currentState = CharacterState.Idle;

            for(int i = 0; i < sprites.Length; i++)
            {
                _characterStates.Add(i.Cast<CharacterState>(), sprites[i]);
            }
            _characterAnimation = new Dictionary<CharacterState, ISprite>[1];
            _characterAnimation[0] = _characterStates;
        }



        public CharacterState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
                if(_currentState != CharacterState.Idle)
                {
                    foreach(SpriteSheet sheet in _characterStates.Values.OfType<SpriteSheet>())
                    {
                        sheet.CurrentFrameIndex = 0;
                    }
                }
            }
        }

        public Microsoft.Xna.Framework.Vector2 Position
        {
            get
            {
                return _characterStates[_currentState].Position;
            }
            set
            {
                foreach(ISprite sprite in _characterStates.Values)
                {
                    sprite.Position = value;
                }
            }
        }

        public Microsoft.Xna.Framework.Color Color
        {
            get
            {
                return _characterStates[_currentState].Color;
            }
            set
            {
                foreach(ISprite sprite in _characterStates.Values)
                {
                    sprite.Color = value;
                }
            }
        }

        public Microsoft.Xna.Framework.Vector2 Origin
        {
            get
            {
                return _characterStates[_currentState].Origin;
            }
            set
            {
                foreach(ISprite sprite in _characterStates.Values)
                {
                    sprite.Origin = value;
                }
            }
        }

        public Microsoft.Xna.Framework.Vector2 Scale
        {
            get
            {
                return _characterStates[_currentState].Scale;
            }
            set
            {
                foreach(ISprite sprite in _characterStates.Values)
                {
                    sprite.Scale = value;
                }
            }
        }

        public Microsoft.Xna.Framework.Graphics.SpriteEffects Effect
        {
            get
            {
                return _characterStates[_currentState].Effect;
            }
            set
            {
                foreach(ISprite sprite in _characterStates.Values)
                {
                    sprite.Effect = value;
                }
            }
        }

        public float Layer
        {
            get
            {
                return _characterStates[_currentState].Layer;
            }
            set
            {
                foreach(ISprite sprite in _characterStates.Values)
                {
                    sprite.Layer = value;
                }
            }
        }

        public ZeroLibrary.Xna.Utils.Rotation Rotation
        {
            get
            {
                return _characterStates[_currentState].Rotation;
            }
            set
            {
                foreach(ISprite sprite in _characterStates.Values)
                {
                    sprite.Rotation = value;
                }
            }
        }

        public Microsoft.Xna.Framework.Rectangle BoundingBox
        {
            get 
            {
                if (_characterStates[_currentState].BoundingBox.Y > 1503)
                {
                    
                }
                return _characterStates[_currentState].BoundingBox; 
            }
        }

        public string Name
        {
            get 
            {
                return _characterStates[_currentState].Name;
            }
        }

        public float X
        {
            get
            {
                return Position.X;
            }
            set
            {
                foreach(ISprite sprite in _characterStates.Values)
                {
                    sprite.X = value;
                }
            }
        }

        public float Y
        {
            get
            {
                return Position.Y;
            }
            set
            {
                foreach(ISprite sprite in _characterStates.Values)
                {
                    sprite.Y = value;
                }
            }
        }

        public object Tag
        {
            get
            {
                return _characterStates[_currentState].Tag;
            }
            set
            {
                foreach(ISprite sprite in _characterStates.Values)
                {
                    sprite.Tag = value;
                }
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            _characterStates[_currentState].Draw(spriteBatch);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _characterStates[_currentState].Update(gameTime);
        }


        public IScreen Parent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public float Width
        {
            get { throw new NotImplementedException(); }
        }

        public float Height
        {
            get { throw new NotImplementedException(); }
        }


        public bool IsVisible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public bool IsUpdatable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
