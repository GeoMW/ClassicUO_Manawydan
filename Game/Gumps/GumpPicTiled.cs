﻿using ClassicUO.Game.Renderer;
using Microsoft.Xna.Framework;

namespace ClassicUO.Game.Gumps
{
    public class GumpPicTiled : GumpControl
    {
        private readonly Graphic _graphic;


        public GumpPicTiled(in Graphic graphic) : base()
        {
            _graphic = graphic;
        }

        public GumpPicTiled(in string[] parts) : this(Graphic.Parse(parts[5]))
        {
            X = int.Parse(parts[1]);
            Y = int.Parse(parts[2]);
            Width = int.Parse(parts[3]);
            Height = int.Parse(parts[4]);
        }


        public override void Update(in double frameMS)
        {
            if (Texture == null)
            {
                Texture = TextureManager.GetOrCreateGumpTexture(_graphic);
            }
            base.Update(frameMS);
        }

        public override bool Draw(in SpriteBatch3D spriteBatch, in Vector3 position)
        {
            spriteBatch.Draw2DTiled(Texture, new Rectangle((int)position.X, (int)position.Y, Width, Height), Vector3.Zero);
            return base.Draw(spriteBatch, position);
        }

    }
}
