using Android.App;
using Android.Graphics.Fonts;
using Android.Renderscripts;
using highrisehavoc.Source.Controllers;
using highrisehavoc.Source.Entities;
using highrisehavoc.Source.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Runtime;

namespace highrisehavoc;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameController gameController;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);

        _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        gameController = new GameController(_spriteBatch, _graphics);

        // Enable full-screen mode
        _graphics.IsFullScreen = true;

        _graphics.ApplyChanges();

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag | GestureType.DoubleTap;

        gameController.Initialize(GraphicsDevice, Content);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        gameController.LoadContent(_spriteBatch);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        gameController.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        gameController.Draw(GraphicsDevice, _spriteBatch);

        base.Draw(gameTime);
    }


}
