using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RtsSystems
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static int SCREEN_RES_X, SCREEN_RES_Y;

        //Camera
        public static Vector3 avatarPosition;
        public static Vector3 camTarget;
        public static Vector3 camPosition;
        public static Matrix projectionMatrix;
        public static Matrix viewMatrix;
        public static Matrix worldMatrix;

        //Orbit
        bool orbit = false;

        //Player controller
        PlayerController player;

        //Model
        Model envModel;

        //Texture
        Texture2D envTexture;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Set screen res params
            SCREEN_RES_X = graphics.GraphicsDevice.Viewport.Bounds.Width;
            SCREEN_RES_Y = graphics.GraphicsDevice.Viewport.Bounds.Height;

            //Setup Camera
            camTarget = new Vector3(100f, 0f, 0f);
            camPosition = new Vector3(-30f, 0f, 0f);
            //var startRotation = Matrix.CreateRotationX(MathHelper.ToRadians(90f));
            //camPosition = Vector3.Transform(camPosition, startRotation);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),
                                                                   GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0f, 1f, 0f));// Y up
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);

            //envModel = Content.Load<Model>(@"Models\Old House 2 3D Models");
            envModel = Content.Load<Model>(@"Models\room");
            envTexture = Content.Load<Texture2D>(@"Textures\House body");

            player = new PlayerController(new Vector3(0f, -10f, -50f));

            Mouse.SetPosition(SCREEN_RES_X / 2, SCREEN_RES_Y / 2);
            player.OldMouseState = Mouse.GetState().Position;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime, Keyboard.GetState(), Mouse.GetState());
            Matrix cameraWorld = Matrix.Invert(viewMatrix);
            Vector3 axis = new Vector3(0, 0, 0);
            bool rotate = false;
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                axis = cameraWorld.Left;
                rotate = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
            {
                axis = cameraWorld.Up;
                rotate = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                axis = cameraWorld.Down;
                rotate = true;
            }
            if (rotate)
            {
                float angle = axis.Length() * 0.01f;
                axis.Normalize();
                camTarget = Vector3.Transform(camTarget - camPosition, Matrix.CreateFromAxisAngle(axis, angle));
            }
            viewMatrix = Matrix.CreateLookAt(camPosition, Vector3.Zero, Vector3.Up);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (ModelMesh mesh in envModel.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.View = viewMatrix;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                    //effect.Texture = envTexture;
                    //effect.TextureEnabled = true;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
