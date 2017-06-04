using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtsSystems
{
    public class PlayerController
    {
        Vector3 _position;
        public Point OldMouseState { get; set; }

        public PlayerController(Vector3 startPosition)
        {
            _position = startPosition;
        }
        public void Update(GameTime gameTime, KeyboardState kState, MouseState mState)
        {
            if (mState.Position != OldMouseState)
            {
                float xDiff = OldMouseState.X - mState.Position.X;
                float yDiff = OldMouseState.Y - mState.Position.Y;

            }
            Vector3 updateOffset = new Vector3(0,0,0);
            int rotation = 0;
            if (kState.IsKeyDown(Keys.W))
            {
                updateOffset = new Vector3(0, 1f, 0);
            }
            if (kState.IsKeyDown(Keys.A))
            {
                updateOffset = new Vector3(-1f, 0, 0);
            }
            if (kState.IsKeyDown(Keys.S))
            {
                updateOffset = new Vector3(0, -1f, 0);
            }
            if (kState.IsKeyDown(Keys.D))
            {
                updateOffset = new Vector3(1f, 0, 0);
            }
            if (kState.IsKeyDown(Keys.Q))
            {
                updateOffset = new Vector3(0, 0, 1f);
            }
            if (kState.IsKeyDown(Keys.E))
            {
                updateOffset = new Vector3(0, 0, -1f);
            }
            _position += updateOffset;
            Main.camTarget += updateOffset;
            Main.camPosition += updateOffset;
        }
    }
}
