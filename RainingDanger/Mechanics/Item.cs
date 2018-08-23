using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZeroLibrary.Xna.Graphic.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace RainingDanger.Mechanics
{
    public struct Item
    {
        private Texture2D _image;
        private ItemType _itemType;
        private bool _canRotate;
        
        public Texture2D Image
        {
            get
            {
                return _image;
            }
        }

        public ItemType ItemType
        {
            get
            {
                return _itemType;
            }
        }

        public bool CanRotate
        {
            get
            {
                return _canRotate;
            }
        }

        public Item(Texture2D image, ItemType itemType, bool canRotate)
        {
            _image = image;
            _itemType = itemType;
            _canRotate = canRotate;
        }
    }
}