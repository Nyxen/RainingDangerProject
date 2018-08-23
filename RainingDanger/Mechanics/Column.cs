using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroLibrary;
using ZeroLibrary.Xna.Graphic.Sprites;
using ZeroLibrary.Xna.Interfaces;

namespace RainingDanger.Mechanics
{
    public class Column
    {
        public static int PoolSize = 10;

        private static Random _random = new Random();

        private Stack<FallingItem> _pool;

        private FallingItem _lastItem;

        public ICollection<FallingItem> Pool 
        {
            get
            {
                return _pool.ToList();
            }
        }

  

        public FallingItem LastItem
        {
            get
            {
                return _lastItem;
            }
        }

        private List<FallingItem> _fallingItems;// make into a queue Kevin.
        public List<FallingItem> FallingItems// make into a queue Kevin.
        {
            get
            {
                return _fallingItems;
            }
        }

        public void Reset()
        {
            while (_fallingItems.Count != 0)
            {
                Push(_fallingItems[0]);
            }
        }

        private List<Column> _nextColumns;

        public List<Column> NextColumns
        {
            get
            {
                return _nextColumns;
            }
        }

        public bool CanSpawn
        {
            get
            {
                if (_pool.Count == 0)
                {
                    return false;
                }

                return LastItem.Y > OffSet; //should know the player height and some offset
            }
        }

        public static float OffSet;

        private int _screenHeight;

        public static List<Item> BadItems;

        public static Item CoinItem;

        public Column(int xCoord, int screenHeight)
        {
            _screenHeight = screenHeight;
            _pool = new Stack<FallingItem>(PoolSize);
            _nextColumns = new List<Column>();

            for (int i = 0; i < PoolSize; i++)
            {
                FallingItem item = new FallingItem("object", BadItems[0], new Microsoft.Xna.Framework.Vector2(xCoord, screenHeight), null);
                item.FadeComplete += item_FadeComplete;
                _lastItem = item;
                _pool.Push(item);
            }

            _fallingItems = new List<FallingItem>();
            _nextColumns.Add(this);
        }

        void item_FadeComplete(object sender, EventArgs e)
        {
            FallingItem item = sender.Cast<FallingItem>();
            Push(item);
        }

        public void Pop()
        {
            FallingItem item = _pool.Pop();


            item.Item = BadItems[_random.Next(BadItems.Count)];

            item.IsVisible = true;
            item.Fade = false;

            item.Origin = new Microsoft.Xna.Framework.Vector2(item.Image.Width / 2, item.Image.Height / 2);
            item.Y = 0;

            _lastItem = item;
            _fallingItems.Add(_lastItem);
        }

        public void Push(FallingItem item)
        {
            item.Y = _screenHeight;
            item.IsVisible = false;
            item.Fade = false;
            _fallingItems.Remove(item);
            _pool.Push(item);
        }

        public bool CheckHit(ISprite player)
        {

            if (_fallingItems.Count > 0)
            {
                FallingItem item = _fallingItems[0];

                if(item.Fade)
                {
                    return false;
                }

                if (item.BoundingBox.Bottom > player.BoundingBox.Top && item.BoundingBox.Bottom < player.BoundingBox.Top + player.Origin.Y)
                {
                    if (player.BoundingBox.Intersects(item.BoundingBox))
                    {
                        //Push(item);
                        if (item.Item.ItemType == ItemType.Bad)
                        {
                            item.Fade = false;
                            return true;
                        }
                    }
                }

            }
            return false;
        }


        public bool ItemBelong(FallingItem item)
        {
            return _lastItem.X == item.X;
        }
    }
}
