using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using highrisehavoc.Source.Entities;
using highrisehavoc.Source.Renderers;
using Java.Util.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace highrisehavoc.Source.Controllers
{
    [Serializable]
    public class HighScoreData
    {
        public List<int> HighScores { get; set; }

        public HighScoreData()
        {
            HighScores = new List<int>();
        }

        public void AddScore(int score)
        {
            HighScores.Add(score);
            HighScores.Sort((a, b) => b.CompareTo(a)); // Sort in descending order
            if (HighScores.Count > 5)
            {
                HighScores.RemoveAt(5); // Keep only the top 5 scores
            }
        }
    }

    [Serializable]
    public class Settings
    {
        public bool MusicEnabled { get; set; } = true;
        public bool SoundEnabled { get; set; } = true;
        public int MusicVolume { get; set; } = 100;
    }

    [Serializable]
    public class Checkpoint
    {
        public int PlayerScore { get; set; }
        public int Hour { get; set; }
        public int WaveNumber { get; set; }
        public int PlayerMonies { get; set; }

        public CollisionController CollisionController { get; set; }
        public Highrise HighriseController { get; set; }
        public List<SoldierController> Soldiers { get; set; }
        public List<SoldierSpawnPointController> soldierSpawnPoints { get; set; }
        public List<ObstacleController> ObstacleControllers { get; set; }
        public List<MineController> MineControllers { get; set; }

        public Checkpoint(int playerScore, int hour, int waveNumber, int playerMonies, Highrise highrise, List<SoldierController> soldiers, List<SoldierSpawnPointController> soldierSpawnPoints, CollisionController collisionController, List<ObstacleController> obstacleControllers, List<MineController> mineControllers)
        {
            PlayerScore = playerScore;
            Hour = hour;
            WaveNumber = waveNumber;
            PlayerMonies = playerMonies;
            HighriseController = highrise;
            Soldiers = soldiers;
            this.soldierSpawnPoints = soldierSpawnPoints;
            CollisionController = collisionController;
            ObstacleControllers = obstacleControllers;
            MineControllers = mineControllers;
        }
    }


    public class GameController
    {
        public List<Enemy> Enemies { get; set; }
        public List<EnemyRenderer> EnemyRenderers { get; set; }
        public List<EnemyController> EnemyControllers { get; set; }

        public MenuController menuController;

        public SoundController soundController;

        private string _highScoreFilename => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "highscores.json");

        private string _settingsFileName => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "settings.json");

        private HighScoreData highScoreData;

        private Settings settings;



        private int PlayerScore { get; set; }

        private int Hour { get; set; }
        private int WaveNumber { get; set; }
        private int PlayerMonies { get; set; }

        public UIController UIController { get; set; }

        private int _goldIntervalTimer = 0;
        private int _hourIntervalTimer = 0;

        private float scaleY;
        private float scaleX;
        private Vector2 textureScale;

        private InputController _inputController;

        Texture2D spritesheet;

        Texture2D spritesheet2;

        Texture2D backgroundSprite;

        Texture2D menuBackgroundSprite;

        Texture2D roadSprite;

        Texture2D buttonSprite;

        Texture2D pauseButtonSprite;

        Texture2D gameOverSprite;

        Texture2D deploymentBorderSprite;

        Texture2D spawnPointSprite;

        Texture2D emptyTexture;

        Rectangle highRiseSpriteRectangle = new(1500, 0, 795, 710);

        Rectangle soldierBodySpriteRectangle = new(3289, 0, 104, 174);
        Rectangle soldierHeadSpriteRectangle = new(1387, 0, 56, 57);
        Rectangle soldierGunSpriteRectangle = new(926, 0, 159, 93);
        Rectangle aaSoldierBackArmSpriteRectangle = new(0, 0, 40, 50);
        Rectangle aaSoldierArmSpriteRectangle = new(40, 0, 79, 60);
        Rectangle aaSoldierLauncherSpriteRectangle = new(127, 0, 190, 30);
        Rectangle aaRocketSpriteRectangle = new(326, 0, 128, 20);
        Rectangle aaSoldierHeadSpriteRectangle = new(448, 0, 56, 57);
        Rectangle mineSpriteRectangle = new(514, 0, 108, 23);
        Rectangle obstacleSpriteRectangle = new(614, 0, 78, 123);
        Rectangle aaSoldierTorsoSpriteRectangle = new(690, 0, 82, 174);


        Rectangle enemyBodySpriteRectangle = new(3538, 0, 90, 175);
        Rectangle enemyBodySpriteRectangle1 = new(3630, 0, 120, 175);
        Rectangle enemyBodySpriteRectangle2 = new(3751, 0, 90, 175);
        Rectangle enemyHeadSpriteRectangle = new(1443, 0, 56, 57);
        Rectangle enemyGunSpriteRectangle = new(1279, 0, 105, 51);

        Rectangle planeSpriteRectangle = new(309, 0, 318, 220);

        Rectangle missileSpriteRectangle = new(3004, 0, 129, 45);

        Rectangle explosionSpriteRectangle = new(3083, 0, 186, 168);

        int screenWidth = 2560;
        int screenHeight = 1440;

        Highrise highrise;
        HighriseRenderer highriseRenderer;
        HighriseController highriseController;

        HighriseAttachmentRenderer highriseAttachmentRenderer;
        HighriseLevelRenderer highriseLevelRenderer;

        List<EnemyController> enemyControllers;
        List<SoldierController> soldierControllers;
        List<ObstacleController> obstacleControllers;
        List<MineController> mineControllers;
        List<EnemyPlaneController> planeControllers;
        List<SoldierSpawnPointController> soldierSpawnPoints;

        Checkpoint Checkpoint;

        DistanceCheckController distanceCheckController;

        CollisionController collisionController;

        OverseerController overseerController;

        SpriteBatch _spriteBatch;

        ContentManager Content;

        GraphicsDevice GraphicsDevice_;

        SpriteFont font;

        Texture2D plusSignSprite;


        enum GameState
        {
            StartMenu,
            Playing,
            Tutorial,
            OptionsPlaying,
            OptionsStart,
            HighScores,
            Paused,
            GameOver
        }

        GameState gameState;



        public GameController(SpriteBatch spriteBatch, GraphicsDeviceManager _graphics)
        {

            Enemies = new List<Enemy>();
            EnemyRenderers = new List<EnemyRenderer>();
            EnemyControllers = new List<EnemyController>();

            this._spriteBatch = spriteBatch;

            /*
            _graphics.PreferredBackBufferWidth = 1920; // Set your target resolution
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            */

            Hour = 6;
            WaveNumber = 1;
            PlayerMonies = 1250;

            highScoreData = this.LoadHighScore();
            settings = this.LoadSettings();
        }

        public void Initialize(GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            this.Content = Content;
            this.GraphicsDevice_ = GraphicsDevice;

            _inputController = new InputController();
            _inputController.OnTap += HandleTap;
            _inputController.OnDrag += HandleDrag;
            _inputController.OnRelease += HandleRelease;

            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;

            if(1920.00 < (float)screenWidth)
            {
                scaleX = (float)(1920.00 / (float)screenWidth) * 0.7f;
            }
            else
            {
                scaleX = (float) ((float)screenWidth / 1920.00) * 0.7f;
            }

            if(1080.00 < (float)screenHeight)
            {
                scaleY = (float)(1080.00 / (float)screenHeight) * 0.65f;
            }
            else
            {
                scaleY = (float) ((float)screenHeight / 1080.00) * 0.65f;
            }
            
            textureScale = new Vector2(scaleX, scaleY);

            highrise = new Highrise(highRiseSpriteRectangle, new Vector2(-20, screenHeight - (int)(highRiseSpriteRectangle.Height * textureScale.Y)));

            soundController = new SoundController(Content);

            collisionController = new CollisionController();
            collisionController.AddHighrise(highrise);

            distanceCheckController = new DistanceCheckController(highrise);

            enemyControllers = new List<EnemyController>();

            soldierControllers = new List<SoldierController>();

            obstacleControllers = new List<ObstacleController>();

            mineControllers = new List<MineController>();

            planeControllers = new List<EnemyPlaneController>();

            soldierSpawnPoints = new List<SoldierSpawnPointController>();

            overseerController = new(highrise, collisionController, distanceCheckController);

            // subscribe to spawn enemy event

            float scale = 1f;
            menuController = new MenuController(screenWidth, screenHeight, textureScale);

            int attackRange = 100;
            int x = 15;

            overseerController.subscribeToSpawnEnemyEvent((sender, e) =>
            {
                Console.WriteLine("Spawn Enemy Event triggered");
                Console.WriteLine("With type: " + e.Type);
                if (e.SpawnEnemy && e.Type == "Soldier")
                {
                    Console.WriteLine("Spawn Enemy Event triggered");
                    // spawn at random location on right side of screen equal to screenwidth + 100 + random number between 0 and 100
                    int numOfEnemies = enemyControllers.Count;
                    Console.WriteLine("Number of enemies that overseer detects: " + numOfEnemies);
                    CreateEnemy(new Vector2(screenWidth + 100 + (float)(new System.Random().NextDouble() * 100 + 98), screenHeight), (int)((100 * textureScale.X + (numOfEnemies * 100)) * textureScale.X));
                }
                else if(e.SpawnEnemy && e.Type == "Plane")
                {
                    CreateEnemyPlane(new Vector2(screenWidth + 100 + (float)(new System.Random().NextDouble() * 100 + 98), screenHeight / 2));
                }
            });

            this.gameState = GameState.StartMenu;
        }



        public void LoadContent(SpriteBatch spriteBatch)
        {
            this._spriteBatch = spriteBatch;

            spritesheet = Content.Load<Texture2D>("spritesheet");

            spritesheet2 = Content.Load<Texture2D>("spritesheet2");

            plusSignSprite = Content.Load<Texture2D>("plus-sign");

            font = Content.Load<SpriteFont>("Arial");

            backgroundSprite = Content.Load<Texture2D>("War");

            menuBackgroundSprite = Content.Load<Texture2D>("HighRiseHavocTitle");

            roadSprite = Content.Load<Texture2D>("road");

            buttonSprite = Content.Load<Texture2D>("simple_button");

            pauseButtonSprite = Content.Load<Texture2D>("pause_button");

            gameOverSprite = Content.Load<Texture2D>("game-over");

            deploymentBorderSprite = Content.Load<Texture2D>("deployment-border");

            spawnPointSprite= Content.Load<Texture2D>("plus-sign-soldier");

            // create a grey texture for the empty bar
            emptyTexture = new Texture2D(GraphicsDevice_, 1, 1);
            emptyTexture.SetData(new Color[] { Color.Gray });

            highriseRenderer = new HighriseRenderer(_spriteBatch, spritesheet, highrise, textureScale);
            highriseAttachmentRenderer = new HighriseAttachmentRenderer(_spriteBatch, spritesheet, plusSignSprite, textureScale);
            highriseLevelRenderer = new HighriseLevelRenderer(_spriteBatch, spritesheet, plusSignSprite, textureScale, spawnPointSprite);

            highriseController = new HighriseController(highrise, highriseRenderer, highriseAttachmentRenderer, highriseLevelRenderer);

            menuController.LoadContent(_spriteBatch, font, buttonSprite, pauseButtonSprite, textureScale);

            soundController.LoadContent();

            UIController = new(font, textureScale);

            int attackRange = 1200;
            int soldierSpawnRange =(int) (highrise.SourceRectangle.Width * textureScale.X) + 95;
            for (int i = 0; i < 5; i++)
            {

                // spawn at random location on right side of screen equal to screenwidth + 100 + random number between 0 and 100
                // CreateEnemy(new Vector2(screenWidth + 100 + (float)(new System.Random().NextDouble() * 100 + 98), screenHeight), attackRange);
                // CreateSoldier(new Vector2(soldierSpawnRange, screenHeight), attackRange);
                SoldierSpawnPoint soldierSpawnPoint = new SoldierSpawnPoint(new Vector2(soldierSpawnRange, screenHeight - spawnPointSprite.Height*2));
                SoldierSpawnPointRenderer soldierSpawnPointRenderer = new SoldierSpawnPointRenderer(_spriteBatch, spawnPointSprite);
                soldierSpawnPoints.Add(new SoldierSpawnPointController(soldierSpawnPoint, soldierSpawnPointRenderer));
                // CreateEnemy(new Vector2(screenWidth/2, screenHeight), attackRange);
                attackRange += 100;
                soldierSpawnRange += 85;
            }
        }

        public void Update(GameTime gameTime)
        {
            
            if (highrise.HitPoints <= 0 && this.gameState != GameState.OptionsStart && this.gameState != GameState.OptionsPlaying)
            {
                this.gameState = GameState.GameOver;
            }

            // TODO: Add your update logic here
            // TEST for highrise movement -> MOVES TO THE RIGHT, will eventually check HP and num of upgrades
            if (gameState == GameState.Playing)
            {
                highriseController.Update(gameTime);
                collisionController.Update(gameTime);
                distanceCheckController.Update(gameTime);
                overseerController.Update(gameTime);

                int enemyControllersCount = enemyControllers.Count;
                int soldierControllersCount = soldierControllers.Count;
                int planeControllersCount = planeControllers.Count;
                int obstacleControllersCount = obstacleControllers.Count;
                // Console.WriteLine("There are currently " + enemyControllers.Count + " enemies and " + soldierControllers.Count + " soldiers.");
                for (int i = 0; i < enemyControllersCount; i++)
                {
                    if (enemyControllers[i].Enemy.IsDead)
                    {
                        distanceCheckController.removeEnemyController(enemyControllers[i]);
                        collisionController.RemoveEnemyController(enemyControllers[i]);
                        enemyControllers.Remove(enemyControllers[i]);
                        increaseMonies(15);
                        // Console.WriteLine("There are now " + enemyControllers.Count + " enemies left.");
                        enemyControllersCount--;
                        for (int j = 0; j < enemyControllersCount; j++)
                        {
                            // decrease attackRange of each enemy by 100
                            enemyControllers[j].Enemy.AttackRange -= 100;
                        }
                        continue;
                    }
                    enemyControllers[i].Update(gameTime);
                }

                for(int i = 0; i< planeControllersCount; i++)
                {
                    planeControllers[i].Update(gameTime);
                    if (planeControllers[i]._enemyPlane.isDead)
                    {
                        distanceCheckController.removeEnemyPlaneController(planeControllers[i]);
                        if (planeControllers[i]._enemyPlaneRenderer.animationFrame<0)
                        {
                            planeControllers.Remove(planeControllers[i]);
                            planeControllersCount--;
                        }
                    }
                }

                for (int i = 0; i < soldierControllersCount; i++)
                {
                    soldierControllers[i].Update(gameTime);
                    if (soldierControllers[i].Soldier.IsDead)
                    {
                        distanceCheckController.removeSoldierController(soldierControllers[i]);
                        for (int j = 0; j < soldierSpawnPoints.Count; j++)
                        {
                            if (soldierSpawnPoints[j].HasSoldier && soldierSpawnPoints[j].SoldierController == soldierControllers[i])
                            {
                                soldierSpawnPoints[j].HasSoldier = false;
                                soldierSpawnPoints[j].SoldierController = null;
                            }
                        }
                        collisionController.RemoveSoldierController(soldierControllers[i]);
                        soldierControllers.Remove(soldierControllers[i]);


                        soldierControllersCount--;
                        continue;
                    } 

                }

                for (int i = 0; i < obstacleControllersCount; i++)
                {
                    obstacleControllers[i].Update();

                    if (!obstacleControllers[i].obstacle.IsSolid)
                    {
                        for(int j = 0; j < soldierSpawnPoints.Count; j++)
                        {
                            if (soldierSpawnPoints[j].HasSoldier && soldierSpawnPoints[j].ObstacleController  == obstacleControllers[i])
                            {
                                soldierSpawnPoints[j].HasSoldier = false;
                                soldierSpawnPoints[j].ObstacleController = null;
                            }
                        }
                        collisionController.RemoveObstacleController(obstacleControllers[i]);
                        distanceCheckController.removeObstacleController(obstacleControllers[i]);
                        obstacleControllers.Remove(obstacleControllers[i]);
                        obstacleControllersCount--;
                    }

                }
                int mineControllersCount = mineControllers.Count;

                for(int i = 0; i < mineControllersCount; i++)
                {
                    mineControllers[i].Update(gameTime);

                    if (mineControllers[i].Mine.IsExploded)
                    {
                        for(int j = 0; j < soldierSpawnPoints.Count; j++)
                        {
                            if (soldierSpawnPoints[j].HasSoldier && soldierSpawnPoints[j].MineController == mineControllers[i])
                            {
                                soldierSpawnPoints[j].HasSoldier = false;
                                soldierSpawnPoints[j].MineController = null;
                            }
                        }
                        collisionController.RemoveMineController(mineControllers[i]);
                        if (mineControllers[i].MineRenderer.animationFrame < 1)
                        {
                            mineControllers.Remove(mineControllers[i]);
                            mineControllersCount--;
                        }
                    }
                }


                _goldIntervalTimer += gameTime.ElapsedGameTime.Milliseconds;
                _hourIntervalTimer += gameTime.ElapsedGameTime.Milliseconds;
                // every 10 seconds, add 25 to player gold
                if (_goldIntervalTimer > 10000)
                {
                    PlayerMonies += 25;
                    // wait 1 second 
                    _goldIntervalTimer = 0;

                    Hour += 1;
                    // wait 1 second
                    _hourIntervalTimer = 0;

                    PlayerScore += 10;
                }

                // every 60 seconds, add minor incentive to player score
                if (_hourIntervalTimer > 60000)
                {
                    PlayerScore += (int)(60 * 1.75);
                }

                // If hour is 24, reset to 6, increase wave number, and spawn enemies
                if (Hour == 10)
                {
                    Hour = 6;
                    WaveNumber += 1;
                    _hourIntervalTimer = 0;

                    PlayerScore += (int)(24 * 1.95);
                    this.gameState = GameState.Tutorial;
                    // create a checkpoint which will save the game state and allow the player to continue from that point
                    // create copy of highrise, soldiers, and enemies

                    CreateCheckpoint();

                    foreach (var enemyController in enemyControllers)
                    {
                        enemyController.Enemy.IsDead = true;
                    }

                    foreach (var enemyPlaneController in planeControllers)
                    {
                        enemyPlaneController._enemyPlane.isDead = true;
                    }

                    foreach (var soldierController in soldierControllers)
                    {
                        foreach (var projectileController in soldierController.Projectiles)
                        {
                            projectileController._outOfBounds = true;
                        }
                    }

                    collisionController.ClearProjectiles();


                }

                if (PlayerMonies < 50) { this.highriseController._canBuildAttachment = false; this.highriseController._canBuildLevel = false; }
                if (PlayerMonies < 100) { this.highriseController._canBuildLevel = false; }
                if (PlayerMonies >= 50) { this.highriseController._canBuildAttachment = true; this.highriseController._canBuildLevel = false; }
                if (PlayerMonies >= 100) { this.highriseController._canBuildLevel = true; this.highriseController._canBuildAttachment = true; }

                if (highrise.HitPoints <= 0)
                {
                    // Game over
                    UpdateHighScore(PlayerScore);
                    Console.WriteLine("Achieved score of : " + PlayerScore);
                    Console.WriteLine("High score is : " + highScoreData.HighScores[0]);
                    Console.WriteLine("Game over");
                }
            }

            if(this.gameState == GameState.Tutorial)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.X))
                {
                    this.gameState = GameState.Playing;
                }
            }

            if (this.gameState == GameState.StartMenu)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.X))
                {
                    this.gameState = GameState.GameOver;
                }
            }

            if (this.gameState == GameState.GameOver)
            {
                // soldierControllers.Clear();
                enemyControllers.Clear();
                planeControllers.Clear();
                obstacleControllers.Clear();
                mineControllers.Clear();
            }

            _inputController.Update(gameTime);
        }

        public void CreateCheckpoint()
        {
            List<SoldierSpawnPointController> copySpawnPoints = new List<SoldierSpawnPointController>();
            foreach (var spawnPoint in soldierSpawnPoints)
            {
                copySpawnPoints.Add(spawnPoint.ReturnCopy());
            }

            Highrise copyHighrise = highriseController.GetHighrise().ReturnCopy();

            CollisionController copyCollisionController = collisionController.ReturnCopy(copyHighrise);

            List<SoldierController> copySoldiers = new List<SoldierController>();
            foreach (var soldierController in soldierControllers)
            {
                copySoldiers.Add(soldierController.ReturnCopy(copyCollisionController));
            }

            List<ObstacleController> copyObstacles = new List<ObstacleController>();
            foreach (var obstacleController in obstacleControllers)
            {
                copyObstacles.Add(obstacleController.ReturnCopy(copyCollisionController));
            }

            List<MineController> copyMines = new List<MineController>();
            foreach (var mineController in mineControllers)
            {
                copyMines.Add(mineController.ReturnCopy(copyCollisionController));
            }

            int score = PlayerScore;
            int hour = Hour;
            int waveNumber = WaveNumber;
            int monies = PlayerMonies;

            Checkpoint = new Checkpoint(score, hour, waveNumber, monies, copyHighrise, copySoldiers, copySpawnPoints, copyCollisionController, copyObstacles, copyMines);
        }

        public void SaveHighScore()
        {
            string jsonString = JsonSerializer.Serialize(highScoreData);
            System.IO.File.WriteAllText(_highScoreFilename, jsonString);
        }

        public HighScoreData LoadHighScore()
        {
            if (File.Exists(_highScoreFilename))
            {
                string jsonString = File.ReadAllText(_highScoreFilename);
                return JsonSerializer.Deserialize<HighScoreData>(jsonString);
            }

            return new HighScoreData();
        }
        public void UpdateHighScore(int playerScore)
        {
            highScoreData.AddScore(playerScore);
            SaveHighScore();
        }

        public void SaveSettings()
        {
            string jsonString = JsonSerializer.Serialize(settings);
            File.WriteAllText(_settingsFileName, jsonString);
        }

        public Settings LoadSettings()
        {
            if (File.Exists(_settingsFileName))
            {
                string jsonString = File.ReadAllText(_settingsFileName);
                return JsonSerializer.Deserialize<Settings>(jsonString);
            }

            return new Settings();
        }

        public void reduceMonies(int amount)
        {
            PlayerMonies -= amount;
            PlayerScore += (int)(amount * 3);
        }

        public void increaseMonies(int amount)
        {
            PlayerMonies += amount;
        }

        public void Draw(GraphicsDevice GraphicsDevice, SpriteBatch _spriteBatch)
        {
            this._spriteBatch = _spriteBatch;
            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (this.gameState == GameState.StartMenu)
            {

                float backgroundScaleY = (1f / ((float)menuBackgroundSprite.Height / (float)screenHeight));
                if (backgroundScaleY < 1.0f)
                {
                    backgroundScaleY = 1.0f;
                }
                float backgroundScaleX = (1f / ((float)menuBackgroundSprite.Width / (float)screenWidth));
                _spriteBatch.Draw(menuBackgroundSprite, new Vector2(0, 0), new Rectangle(0, 0, (int)(menuBackgroundSprite.Width), (int)(screenHeight)), Color.White, 0f, Vector2.Zero, new Vector2((float)backgroundScaleX, backgroundScaleY), SpriteEffects.None, 0f);
                // gameController.DrawStartMenu(screenWidth, screenHeight);
                menuController.DrawStartMenu();
            }

            if (this.gameState == GameState.Paused)
            {
                float backgroundScaleY = (1f / ((float)menuBackgroundSprite.Height / (float)screenHeight));
                if (backgroundScaleY < 1.0f)
                {
                    backgroundScaleY = 1.0f;
                }
                float backgroundScaleX = (1f / ((float)menuBackgroundSprite.Width / (float)screenWidth));
                _spriteBatch.Draw(menuBackgroundSprite, new Vector2(0, 0), new Rectangle(0, 0, (int)(menuBackgroundSprite.Width), (int)(screenHeight)), Color.White, 0f, Vector2.Zero, new Vector2((float)backgroundScaleX, backgroundScaleY), SpriteEffects.None, 0f); ;
                menuController.DrawPauseMenu();
            }

            if(this.gameState == GameState.Tutorial)
            {
                // _spriteBatch.Draw(menuBackgroundSprite, new Rectangle(-22, 0, (int)(backgroundSprite.Width * 1.22), (int)(backgroundSprite.Height * 1.15)), Color.White);
                DrawTutorialMenu(screenWidth, screenHeight);
            }

            if (this.gameState == GameState.HighScores)
            {
                float backgroundScaleY = (1f / ((float)menuBackgroundSprite.Height / (float)screenHeight));
                if (backgroundScaleY < 1.0f)
                {
                    backgroundScaleY = 1.0f;
                }
                float backgroundScaleX = (1f / ((float)menuBackgroundSprite.Width / (float)screenWidth));
                _spriteBatch.Draw(menuBackgroundSprite, new Vector2(0, 0), new Rectangle(0, 0, (int)(menuBackgroundSprite.Width), (int)(screenHeight)), Color.White, 0f, Vector2.Zero, new Vector2((float)backgroundScaleX, backgroundScaleY), SpriteEffects.None, 0f);
                DrawHighScoresMenu(screenWidth, screenHeight);
            }

            if (this.gameState == GameState.OptionsPlaying || this.gameState == GameState.OptionsStart)
            {
                float backgroundScaleY = (1f / ((float)menuBackgroundSprite.Height / (float)screenHeight));
                if (backgroundScaleY < 1.0f)
                {
                    backgroundScaleY = 1.0f;
                }
                float backgroundScaleX = (1f / ((float)menuBackgroundSprite.Width / (float)screenWidth));
                _spriteBatch.Draw(menuBackgroundSprite, new Vector2(0, 0), new Rectangle(0, 0, (int)menuBackgroundSprite.Width, (int)(screenHeight)), Color.White, 0f, Vector2.Zero, new Vector2((float)backgroundScaleX, backgroundScaleY), SpriteEffects.None, 0f);
                DrawOptionsScreen(screenWidth, screenHeight);
            }

            if (this.gameState == GameState.GameOver)
            {
                float backgroundScaleY = (1f / ((float)gameOverSprite.Height / (float)screenHeight));
                if (backgroundScaleY < 1.0f)
                {
                    backgroundScaleY = 1.0f;
                }
                float backgroundScaleX = (1f / ((float)gameOverSprite.Width / (float)screenWidth));
                _spriteBatch.Draw(gameOverSprite, new Vector2(0, 0), new Rectangle(0, 0, (int)(gameOverSprite.Width), (int)(screenHeight)), Color.White, 0f, Vector2.Zero, new Vector2((float)backgroundScaleX, backgroundScaleY), SpriteEffects.None, 0f);
                if(Checkpoint != null && Checkpoint.WaveNumber >= 1) menuController.DrawEndMenu(true);
                else menuController.DrawEndMenu(false);

            }

            if (this.gameState == GameState.Playing)
            {
                float backgroundScaleY = (1f / ((float)backgroundSprite.Height / (float)screenHeight));
                if (backgroundScaleY < 1.0f)
                {
                    backgroundScaleY = 1.0f;
                }
                float backgroundScaleX = (1f / ((float)backgroundSprite.Width / (float)screenWidth));
                // draw background sprite at top left corner with 1.5 times width and height and move it down by 100 pixels
                _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), new Rectangle(0, 0, (int)backgroundSprite.Width, (int)(backgroundSprite.Height)), Color.White, 0f, Vector2.Zero, new Vector2(backgroundScaleX, 0.6f), SpriteEffects.None, 0f);

                UIController.DrawHeaderBar(_spriteBatch, screenWidth, screenHeight, emptyTexture);
                UIController.DrawDeploymentBar(_spriteBatch, screenWidth, screenHeight, soldierHeadSpriteRectangle, aaSoldierHeadSpriteRectangle, obstacleSpriteRectangle, mineSpriteRectangle ,spritesheet, spritesheet2, deploymentBorderSprite);

                // draw roead sprite at bottom of screen with half height
                _spriteBatch.Draw(roadSprite, new Rectangle(0, screenHeight - (roadSprite.Height / 2), GraphicsDevice.Viewport.Width, roadSprite.Height / 2), Color.White);

                // draw highrise sprite in lower left corner

                int drawnEnemies = 0;
                int drawnSoldiers = 0;

                foreach (var obstacleController in obstacleControllers)
                {
                    obstacleController.Draw();
                }
                foreach (var mineController in mineControllers)
                {
                    mineController.Draw();
                }
                // draw plane sprite in center of screen
                foreach (var enemyController in enemyControllers)
                {
                    enemyController.Draw();
                    if (enemyController.EnemyRenderer.isBeingDrawn) drawnEnemies++;
                }

                foreach (var soldierController in soldierControllers)
                {
                    soldierController.Draw();
                    if (soldierController.SoldierRenderer.isBeingDrawn) drawnSoldiers++;
                }



                foreach (var planeController in planeControllers)
                {
                    planeController.Draw();
                }

                foreach (var soldierSpawnPoint in soldierSpawnPoints)
                {
                    soldierSpawnPoint.Draw();
                }

                menuController.DrawPauseButton();

                DrawCurrentlyBeingDraw(_spriteBatch, screenWidth, screenHeight, enemyControllers.Count + soldierControllers.Count, drawnEnemies + drawnSoldiers);

                highriseController.Draw();

                UIController.Draw(this._spriteBatch, WaveNumber, PlayerMonies, Hour, highrise.HitPoints, highrise.HitPoints <= 0);

                if (_inputController.isHoldingArmy && _inputController.lastKnownDragPosition != null)
                {
                    if(_inputController.deployType == InputController.DeployType.Soldier)
                    {
                        UIController.DrawSoldierSquare(_spriteBatch, (Vector2)_inputController.lastKnownDragPosition, soldierHeadSpriteRectangle, spritesheet, deploymentBorderSprite);
                    }
                    else if(_inputController.deployType == InputController.DeployType.AASoldier)
                    {
                        UIController.DrawAASoldierSquare(_spriteBatch, (Vector2)_inputController.lastKnownDragPosition, aaSoldierHeadSpriteRectangle, spritesheet2, deploymentBorderSprite);
                    }
                    else if(_inputController.deployType == InputController.DeployType.Obstacle)
                    {
                        UIController.DrawObstacleSquare(_spriteBatch, (Vector2)_inputController.lastKnownDragPosition, obstacleSpriteRectangle, spritesheet2, deploymentBorderSprite);
                    }
                    else if(_inputController.deployType == InputController.DeployType.Mine)
                    {
                        UIController.DrawMineSquare(_spriteBatch, (Vector2)_inputController.lastKnownDragPosition, mineSpriteRectangle, spritesheet2, deploymentBorderSprite);
                    }


                }
            }

            _spriteBatch.End();
        }

        public void toggleMusic()
        {
            if (settings.MusicEnabled)
            {
                settings.MusicEnabled = false;
                // soundController.StopMusic();
            }
            else
            {
                settings.MusicEnabled = true;
                // soundController.PlayMusic();
            }
            SaveSettings();
        }

        public void toggleSound()
        {
            if (settings.SoundEnabled)
            {
                settings.SoundEnabled = false;
                soundController.StopSound();
                Console.WriteLine("Can be played: " + soundController.canPlaySound.ToString() + "Should be false");
            }
            else
            {
                settings.SoundEnabled = true;
                soundController.EnableSound();
                Console.WriteLine("Can be played: " + soundController.canPlaySound.ToString() + "Should be true");
            }
            SaveSettings();
        }

        public void DrawStartMenu(int screenWidth, int screenHeight)
        {
            UIController.DrawStartScreen(this._spriteBatch, screenWidth, screenHeight);
        }

        public void DrawTutorialMenu(int screenWidth, int screenHeight)
        {
            menuController.DrawTutorialControls();
            UIController.DrawTutorialScreen(this._spriteBatch, screenWidth, screenHeight);
        }

        public void DrawHighScoresMenu(int screenWidth, int screenHeight)
        {
            UIController.DrawHighScores(highScoreData,this._spriteBatch, screenWidth, screenHeight);
            menuController.DrawHighScoresMenu();
        }

        public void DrawOptionsScreen(int screenWidth, int screenHeight)
        {
            UIController.DrawOptionsScreen(this._spriteBatch, screenWidth, screenHeight);
            menuController.DrawOptionsMenu(settings);
        }

        public void DrawCurrentlyBeingDraw(SpriteBatch spriteBatch, int screenWidth, int screenHeight, int total_entities, int drawn_entities)
        {
            UIController.DrawCurrentlyBeingDraw(spriteBatch,screenWidth, screenHeight, total_entities, drawn_entities);
        }

        private void HandleDrag(Vector2 tapPosition, Vector2 dragPosition)
        {

            System.Console.WriteLine("Dragged from " + tapPosition + " to " + dragPosition);
        }

        private void HandleRelease(Vector2 tapPosition)
        {
            System.Console.WriteLine("Released from " + tapPosition);
            if(_inputController.isHoldingArmy) {                 
                _inputController.isHoldingArmy = false;
                _inputController.lastKnownDragPosition = null;
                for(int i = 0; i < highrise.AttachmentPoints.Count; i++)
                {
                    Console.WriteLine("Checking attachment point " + i + " for tap");
                    Console.WriteLine("The rectangle is " + new Rectangle((int)highrise.AttachmentPoints[i].PlusSignPosition.X, (int)highrise.AttachmentPoints[i].PlusSignPosition.Y, highrise.AttachmentPoints[i].PlusSignSourceRectangle.Width, highrise.AttachmentPoints[i].PlusSignSourceRectangle.Height));
                    Console.WriteLine("The tap position is " + tapPosition);
                    if (highrise.AttachmentPoints[i].isBuilt && !highrise.AttachmentPoints[i].hasSoldier && _inputController.IsTapNearObject(tapPosition,new Rectangle((int)highrise.AttachmentPoints[i].SoldierPosition.X - 200, (int)highrise.AttachmentPoints[i].SoldierPosition.Y-150, 400, 300)))
                    {
                        if(PlayerMonies >= 150 && !highrise.AttachmentPoints[i].hasSoldier)
                        {
                            Console.WriteLine("Creating soldier at attachment point " + i);
                            if (_inputController.deployType == InputController.DeployType.Soldier)
                                CreateSoldier(highrise.AttachmentPoints[i].SoldierPosition, 1200, null);
                            if (_inputController.deployType == InputController.DeployType.AASoldier)
                                CreateAASoldier(highrise.AttachmentPoints[i].SoldierPosition, 1200, null);
                            highrise.AttachmentPoints[i].hasSoldier = true;
                            break;
                        }
                    }
                }

                for(int i = 0; i < highrise.Levels.Count; i++)
                {
                    if (highrise.Levels[i].IsBuilt && !highrise.Levels[i].hasSoldier && _inputController.IsTapNearObject(tapPosition, new Rectangle((int)highrise.Levels[i].SoldierPosition.X - 250, (int)highrise.Levels[i].SoldierPosition.Y -50, 500, 100)))
                    {
                        if (PlayerMonies >= 150 && !highrise.Levels[i].hasSoldier)
                        {
                            if(_inputController.deployType == InputController.DeployType.Soldier)
                               CreateSoldier(highrise.Levels[i].SoldierPosition, 1200, null);
                            if(_inputController.deployType == InputController.DeployType.AASoldier)
                                CreateAASoldier(highrise.Levels[i].SoldierPosition, 1200, null);
                            highrise.Levels[i].hasSoldier = true;
                            break;
                        }
                    }
                }

                for (int i = 0; i<soldierSpawnPoints.Count; i++)
                {
                    if (_inputController.IsTapNearObject(tapPosition, new Rectangle((int)soldierSpawnPoints[i].SoldierSpawnPoint.Position.X - 50, (int)soldierSpawnPoints[i].SoldierSpawnPoint.Position.Y - 50, 150, 150)))
                    {
                        if(_inputController.deployType == InputController.DeployType.Soldier)
                        {
                            if (PlayerMonies >= 150 && !soldierSpawnPoints[i].HasSoldier)
                            {
                                CreateSoldier(new Vector2(soldierSpawnPoints[i].SoldierSpawnPoint.Position.X, soldierSpawnPoints[i].SoldierSpawnPoint.Position.Y), 1200, soldierSpawnPoints[i]);
                                break;
                            }
                        }

                        if(_inputController.deployType == InputController.DeployType.AASoldier)
                        {
                            if (PlayerMonies >= 150 && !soldierSpawnPoints[i].HasSoldier)
                            {
                                CreateAASoldier(new Vector2(soldierSpawnPoints[i].SoldierSpawnPoint.Position.X, soldierSpawnPoints[i].SoldierSpawnPoint.Position.Y), 1200, soldierSpawnPoints[i]);
                                break;
                            }
                        }

                        if(_inputController.deployType == InputController.DeployType.Obstacle)
                        {
                            if (PlayerMonies >= 150 && !soldierSpawnPoints[i].HasSoldier)
                            {
                                Console.WriteLine("Creating obstacle at spawn point " + i);
                                CreateObstacle(new Vector2(soldierSpawnPoints[i].SoldierSpawnPoint.Position.X, soldierSpawnPoints[i].SoldierSpawnPoint.Position.Y + 50), soldierSpawnPoints[i]);
                                break;
                            }
                        }

                        if (_inputController.deployType == InputController.DeployType.Mine)
                        {
                            if (PlayerMonies >= 150 && !soldierSpawnPoints[i].HasSoldier)
                            {
                                Console.WriteLine("Creating obstacle at spawn point " + i);
                                CreateMine(new Vector2(soldierSpawnPoints[i].SoldierSpawnPoint.Position.X, soldierSpawnPoints[i].SoldierSpawnPoint.Position.Y + 50), soldierSpawnPoints[i]);
                                break;
                            }
                        }

                    }
                }
                // overseerController.deployArmy(tapPosition);
            }
        }

        private void HandleTap(Vector2 tapPosition)
        {

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(highrise.SpritePosition.ToPoint(), highrise.SourceRectangle.Size)))
            {
                System.Console.WriteLine("Tapped highrise");
            }
            foreach (var attachment in highrise.AttachmentPoints)
            {
                if (_inputController.IsTapNearObject(tapPosition, new Rectangle(attachment.PlusSignPosition.ToPoint(), attachment.PlusSignSourceRectangle.Size)))
                {
                    attachment.isBuilt = true;
                    if (attachment.canBeTapped) { reduceMonies(50); }
                    attachment.canBeTapped = false;
                    System.Console.WriteLine("Tapped attachment");
                }
            }

            foreach (var level in highrise.Levels)
            {
                if (_inputController.IsTapNearObject(tapPosition, new Rectangle(level.PlusSignPosition.ToPoint(), level.PlusSignSourceRectangle.Size)))
                {
                    level.IsBuilt = true;
                    if (level.canBeTapped) { reduceMonies(100); }
                    level.canBeTapped = false;
                }
            }


            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.startGameButton.Position.ToPoint(), menuController.startGameButton.Bounds.Size)) && (this.gameState == GameState.Paused))
            {
                this.gameState = GameState.Playing;
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.startGameButton.Position.ToPoint(), menuController.startGameButton.Bounds.Size)) && (this.gameState == GameState.GameOver))
            {
                this.RestartGame(null);
                this.gameState = GameState.Playing;
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.startGameButton.Position.ToPoint(), menuController.startGameButton.Bounds.Size)) && (this.gameState == GameState.StartMenu))
            {
                this.gameState = GameState.Tutorial;
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.stopGameButton.Position.ToPoint(), menuController.stopGameButton.Bounds.Size)) && (this.gameState == GameState.Tutorial))
            {
                this.gameState = GameState.Playing;
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.stopGameButton.Position.ToPoint(), menuController.stopGameButton.Bounds.Size)) && (this.gameState == GameState.StartMenu || this.gameState == GameState.Paused || this.gameState == GameState.OptionsPlaying || this.gameState == GameState.OptionsStart || this.gameState == GameState.HighScores || this.gameState == GameState.GameOver))
            {
                Console.WriteLine("current game state before press: " + this.gameState);
                if (this.gameState == GameState.OptionsPlaying) { this.gameState = GameState.Paused; }
                else if (this.gameState == GameState.OptionsStart) { this.gameState = GameState.StartMenu; }
                else if (this.gameState == GameState.HighScores) { this.gameState = GameState.StartMenu; }
                else
                {
                    this.gameState = GameState.GameOver;
                }

                Console.WriteLine("current game state after press: " + this.gameState);

            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(new Vector2(screenWidth / 2 - 150, 7).ToPoint(), new Rectangle(0,0, (int)(400 * textureScale.X), (int)(400 * textureScale.Y)).Size)) && this.gameState == GameState.Playing)
            {
                Console.WriteLine("Pressed on the army deploy");
                _inputController.isHoldingArmy = true;
                _inputController.deployType = InputController.DeployType.Soldier;
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(new Vector2(screenWidth / 2 - 50 + 6, 12).ToPoint(), new Rectangle(0, 0, (int)(400 * textureScale.X), (int)(400 * textureScale.Y)).Size)) && this.gameState == GameState.Playing)
            {
                Console.WriteLine("Pressed on the aaarmy deploy");
                _inputController.isHoldingArmy = true;
                _inputController.deployType = InputController.DeployType.AASoldier;

            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(new Vector2(screenWidth / 2 + 50 + 20, 12).ToPoint(), new Rectangle(0, 0, (int)(400 * textureScale.X), (int)(400 * textureScale.Y)).Size)) && this.gameState == GameState.Playing)
            {
                Console.WriteLine("Pressed on the obstacle deploy");
                _inputController.isHoldingArmy = true;
                _inputController.deployType = InputController.DeployType.Obstacle;
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(new Vector2(screenWidth / 2 + 150 + 20, 12).ToPoint(), new Rectangle(0, 0, (int)(400 * textureScale.X), (int)(400 * textureScale.Y)).Size)) && this.gameState == GameState.Playing)
            {
                Console.WriteLine("Pressed on the Mine deploy");
                _inputController.isHoldingArmy = true;
                _inputController.deployType = InputController.DeployType.Mine;
            }


            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.scoresButton.Position.ToPoint(), menuController.scoresButton.Bounds.Size)) && (this.gameState == GameState.StartMenu))
            {
                Console.WriteLine("current game state before press: " + this.gameState);
                this.gameState = GameState.HighScores;
                Console.WriteLine("current game state after press: " + this.gameState);
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.scoresButton.Position.ToPoint(), menuController.scoresButton.Bounds.Size)) && (this.gameState == GameState.GameOver))
            {
                // load with checkpoint
                this.gameState = GameState.Playing;
                this.RestartGame(Checkpoint);
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.optionsButton.Position.ToPoint(), menuController.optionsButton.Bounds.Size)) && (this.gameState == GameState.StartMenu || this.gameState == GameState.GameOver))
            {
                Console.WriteLine("current game state before press: " + this.gameState);
                this.gameState = GameState.OptionsStart;
                Console.WriteLine("current game state after press: " + this.gameState);
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.musicToggleButton.Position.ToPoint(), menuController.startGameButton.Bounds.Size)) && (this.gameState == GameState.OptionsStart || this.gameState == GameState.OptionsPlaying))
            {
                toggleMusic();
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.soundToggleButton.Position.ToPoint(), menuController.startGameButton.Bounds.Size)) && (this.gameState == GameState.OptionsStart || this.gameState == GameState.OptionsPlaying))
            {
                toggleSound();
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.optionsButton.Position.ToPoint(), menuController.optionsButton.Bounds.Size)) && this.gameState == GameState.Paused)
            {
                Console.WriteLine("current game state before press: " + this.gameState);
                this.gameState = GameState.OptionsPlaying;
                Console.WriteLine("current game state after press: " + this.gameState);
            }

            if (_inputController.IsTapNearObject(tapPosition, new Rectangle(menuController.pauseGameButton.Position.ToPoint(), menuController.pauseGameButton.Bounds.Size)) && this.gameState == GameState.Playing)
            {
                Console.WriteLine("current game state before press: " + this.gameState);
                this.gameState = GameState.Paused;
                Console.WriteLine("current game state after press: " + this.gameState);
            }
        }

        private void RestartGame(Checkpoint? checkpoint)
        {

            highScoreData = this.LoadHighScore();
            settings = this.LoadSettings();

            // reinitialize everything

            _inputController = new InputController();
            _inputController.OnTap += HandleTap;
            _inputController.OnDrag += HandleDrag;
            _inputController.OnRelease += HandleRelease;

            screenWidth = this.GraphicsDevice_.Viewport.Width;
            screenHeight = this.GraphicsDevice_.Viewport.Height;

            if (checkpoint == null)
            {
                Hour = 6;
                WaveNumber = 1;
                PlayerMonies = 1250;

                highrise = new Highrise(highRiseSpriteRectangle, new Vector2(-20, screenHeight - (int)(highRiseSpriteRectangle.Height * textureScale.Y)));

                enemyControllers = new List<EnemyController>();

                soldierControllers = new List<SoldierController>();

                planeControllers = new List<EnemyPlaneController>();

                soldierSpawnPoints = new List<SoldierSpawnPointController>();

                obstacleControllers = new List<ObstacleController>();

                mineControllers = new List<MineController>();

                int soldierattackRange = 1200;
                int soldierSpawnRange = (int)(highrise.SourceRectangle.Width * textureScale.X) + 95;
                for (int i = 0; i < 5; i++)
                {

                    // spawn at random location on right side of screen equal to screenwidth + 100 + random number between 0 and 100
                    // CreateEnemy(new Vector2(screenWidth + 100 + (float)(new System.Random().NextDouble() * 100 + 98), screenHeight), attackRange);
                    // CreateSoldier(new Vector2(soldierSpawnRange, screenHeight), attackRange);
                    SoldierSpawnPoint soldierSpawnPoint = new SoldierSpawnPoint(new Vector2(soldierSpawnRange, screenHeight - spawnPointSprite.Height * 2));
                    SoldierSpawnPointRenderer soldierSpawnPointRenderer = new SoldierSpawnPointRenderer(_spriteBatch, spawnPointSprite);
                    soldierSpawnPoints.Add(new SoldierSpawnPointController(soldierSpawnPoint, soldierSpawnPointRenderer));
                    // CreateEnemy(new Vector2(screenWidth/2, screenHeight), attackRange);
                    soldierattackRange += 100;
                    soldierSpawnRange += 85;
                }


                highriseRenderer = new HighriseRenderer(_spriteBatch, spritesheet, highrise,textureScale);
                highriseAttachmentRenderer = new HighriseAttachmentRenderer(_spriteBatch, spritesheet, plusSignSprite, textureScale);
                highriseLevelRenderer = new HighriseLevelRenderer(_spriteBatch, spritesheet, plusSignSprite, textureScale, spawnPointSprite);

                highriseController = new HighriseController(highrise, highriseRenderer, highriseAttachmentRenderer, highriseLevelRenderer);
            }
            else
            {
                Hour = 6;
                WaveNumber = checkpoint.WaveNumber;
                PlayerMonies = checkpoint.PlayerMonies;

                highrise = checkpoint.HighriseController;

                highriseRenderer = new HighriseRenderer(_spriteBatch, spritesheet, highrise, textureScale);
                highriseAttachmentRenderer = new HighriseAttachmentRenderer(_spriteBatch, spritesheet, plusSignSprite, textureScale);
                highriseLevelRenderer = new HighriseLevelRenderer(_spriteBatch, spritesheet, plusSignSprite, textureScale, spawnPointSprite);
                highriseController = new HighriseController(highrise, highriseRenderer, highriseAttachmentRenderer, highriseLevelRenderer);

                soldierControllers = checkpoint.Soldiers;
                soldierSpawnPoints = checkpoint.soldierSpawnPoints;
                obstacleControllers = checkpoint.ObstacleControllers;
                mineControllers = checkpoint.MineControllers;
                PlayerScore = checkpoint.PlayerScore;
            }

            soundController = new SoundController(Content);

            if(checkpoint != null && checkpoint.CollisionController != null)
            {
                collisionController = checkpoint.CollisionController;
            }
            else
            {
                collisionController = new CollisionController();
                collisionController.AddHighrise(highrise);
            }

            distanceCheckController = new DistanceCheckController(highrise);

            overseerController = new(highrise, collisionController, distanceCheckController);

            // subscribe to spawn enemy event

            menuController = new MenuController(screenWidth, screenHeight, textureScale);

            overseerController.subscribeToSpawnEnemyEvent((sender, e) =>
            {
                Console.WriteLine("Spawn Enemy Event triggered");
                Console.WriteLine("With type: " + e.Type);
                if (e.SpawnEnemy && e.Type == "Soldier")
                {
                    Console.WriteLine("Spawn Enemy Event triggered");
                    // spawn at random location on right side of screen equal to screenwidth + 100 + random number between 0 and 100
                    int numOfEnemies = enemyControllers.Count;
                    Console.WriteLine("Number of enemies that overseer detects: " + numOfEnemies);
                    CreateEnemy(new Vector2(screenWidth + 100 + (float)(new System.Random().NextDouble() * 100 + 98), screenHeight), (int)((100 * textureScale.X + (numOfEnemies * 100)) * textureScale.X));
                }
                else if (e.SpawnEnemy && e.Type == "Plane")
                {
                    CreateEnemyPlane(new Vector2(screenWidth + 100 + (float)(new System.Random().NextDouble() * 100 + 98), screenHeight /2 ));
                }
            });


            menuController.LoadContent(_spriteBatch, font, buttonSprite, pauseButtonSprite, textureScale);

            soundController.LoadContent();

            UIController = new(font, textureScale);

            CreateCheckpoint();

        }

        private void _inputController_OnDrag(Vector2 arg1, Vector2 arg2)
        {
            throw new NotImplementedException();
        }

        private void CreateEnemy(Vector2 position, int attackRange)
        {
            float bodyScaleY = 1.8f;
            float headScaleY = 1.5f;
            float gunScaleY = 2f;
            if (screenHeight < 1920)
            {
                bodyScaleY = 1.2f;
                headScaleY = 1f;
                gunScaleY = 1.35f;
            }
            Enemy enemy = new(

            enemyBodySpriteRectangle,
                new Vector2(position.X - (int)(enemyBodySpriteRectangle.Width * textureScale.X) - 5, position.Y - (float)((int)(enemyBodySpriteRectangle.Height * textureScale.Y)) / bodyScaleY),
                enemyBodySpriteRectangle1,
                new Vector2(position.X - (int)(enemyBodySpriteRectangle1.Width * textureScale.X) + 2, position.Y - (int)(enemyBodySpriteRectangle1.Height * textureScale.Y) / bodyScaleY),
                enemyBodySpriteRectangle2,
                new Vector2(position.X - (int)(enemyBodySpriteRectangle2.Width * textureScale.X) - 3, position.Y - (int)(enemyBodySpriteRectangle2.Height * textureScale.Y) / bodyScaleY),
                enemyHeadSpriteRectangle,
                new Vector2(position.X - (enemyBodySpriteRectangle.Width * textureScale.X) - 2, position.Y - (int)(enemyBodySpriteRectangle.Height * textureScale.Y) / headScaleY + 2),
                enemyGunSpriteRectangle,
                new Vector2(position.X - (enemyBodySpriteRectangle.Width * textureScale.X) - (float)(enemyGunSpriteRectangle.Width * textureScale.X) / 3f, position.Y - (int)(enemyBodySpriteRectangle.Height * textureScale.Y) / gunScaleY),
                5, // Set appropriate values for hit points, damage, and speed
                10,
                (int)(new System.Random().NextDouble() * 300 + 10),
                attackRange
            );

            EnemyRenderer enemyRenderer = new(this._spriteBatch, spritesheet, screenWidth, textureScale);
            EnemyController enemyController = new(enemy, enemyRenderer, highrise, collisionController, soundController, textureScale);

            enemyControllers.Add(enemyController);
            distanceCheckController.addEnemyController(enemyController);
        }
        private void CreateEnemyPlane(Vector2 position)
        {
            EnemyPlane enemyPlane = new(planeSpriteRectangle, new Vector2(position.X - planeSpriteRectangle.Width, position.Y - (int)(planeSpriteRectangle.Height * 0.35)), 3, 10, 560);
            EnemyPlaneRenderer enemyPlaneRenderer = new(this._spriteBatch, spritesheet, screenWidth, textureScale);
            EnemyPlaneController enemyPlaneController = new(enemyPlane, enemyPlaneRenderer, highrise, collisionController);

            Console.WriteLine("Created plane at " + enemyPlane.BodySpritePosition+ " Adding to planeControllers list");
            planeControllers.Add(enemyPlaneController);
            distanceCheckController.addEnemyPlaneController(enemyPlaneController);
        }

        private void CreateSoldier(Vector2 position, int attackRange, SoldierSpawnPointController? soldierSpawnPointController)
        {
            Console.WriteLine("Position is " + position.ToString());
            Console.WriteLine("Head sprite position is: " + (position.X - (soldierBodySpriteRectangle.Width * textureScale.X) - (float)(soldierHeadSpriteRectangle.Width * textureScale.X)));
            Soldier soldier = new(
                soldierBodySpriteRectangle,
                new Vector2(position.X - (int)(soldierBodySpriteRectangle.Width * textureScale.X * 0.6f) / 2, position.Y - (int)(soldierBodySpriteRectangle.Height * textureScale.Y * 0.6) / 4),
                soldierHeadSpriteRectangle,
                new Vector2(position.X - (int)((soldierHeadSpriteRectangle.Width*textureScale.X * 0.6f )/ 2.5), position.Y - (float)(soldierHeadSpriteRectangle.Height * textureScale.Y * 0.6) * 1.4f),
                soldierGunSpriteRectangle,
                new Vector2(position.X - (int)(soldierGunSpriteRectangle.Width * textureScale.X * 0.6f / 8.5), position.Y - (float)(soldierGunSpriteRectangle.Height * textureScale.Y * 0.6) / 3.5f),
                2, // Set appropriate values for hit points, damage, and speed
                10,
                (int)(new System.Random().NextDouble() * 300 + 10),
                attackRange,
                200,
                new Rectangle(76,20,23,5)
            );

            reduceMonies(150);
            PlayerScore += (int)(150 * 1.35);
            // SoldierRenderer soldierRenderer = new(this._spriteBatch, spritesheet);
            SoldierRenderer soldierRenderer = new(this._spriteBatch, spritesheet, textureScale);
            SoldierController soldierController = new(soldier, soldierRenderer, highrise, collisionController, soundController);

            if(soldierSpawnPointController != null)
            {
                soldierSpawnPointController.HasSoldier = true;
                soldierSpawnPointController.AddSoldierController(soldierController);
            }

            soldierControllers.Add(soldierController);
            distanceCheckController.addSoldierController(soldierController);

        }

        private void CreateAASoldier(Vector2 position, int attackRange, SoldierSpawnPointController? soldierSpawnPointController)
        {
            AASoldier soldier = new(
                aaSoldierTorsoSpriteRectangle,
                new Vector2(position.X - (int)(aaSoldierTorsoSpriteRectangle.Width * textureScale.X * 0.6), position.Y - (int)(aaSoldierTorsoSpriteRectangle.Height * textureScale.Y * 0.6) / 4),
                aaSoldierHeadSpriteRectangle,
                new Vector2(position.X - (int)(aaSoldierTorsoSpriteRectangle.Width * textureScale.X * 0.6) + 2, position.Y - (int)(aaSoldierHeadSpriteRectangle.Height * textureScale.Y * 0.6) * 1.5f),
                aaSoldierArmSpriteRectangle,
                new Vector2(position.X - (int)(aaSoldierTorsoSpriteRectangle.Width * textureScale.X * 0.6) + 2, position.Y - (float)(int)(aaSoldierArmSpriteRectangle.Height * textureScale.Y * 0.6) / 1.4f),
                aaSoldierBackArmSpriteRectangle,
                new Vector2(position.X - (int)(aaSoldierTorsoSpriteRectangle.Width * textureScale.X * 0.6), position.Y - (float)(int)(aaSoldierBackArmSpriteRectangle.Height * textureScale.Y * 0.6) / 2f),
                aaSoldierLauncherSpriteRectangle,
                new Vector2(position.X - (int)(aaSoldierTorsoSpriteRectangle.Width * textureScale.X * 0.6), position.Y - ((int)(aaSoldierLauncherSpriteRectangle.Height * textureScale.Y * 0.6)) * 2f),
                2, // Set appropriate values for hit points, damage, and speed
                10,
                (int)(new System.Random().NextDouble() * 300 + 10),
                attackRange,
                200,
                aaRocketSpriteRectangle
            );
            reduceMonies(150);
            PlayerScore += (int)(150 * 1.35);
            // SoldierRenderer soldierRenderer = new(this._spriteBatch, spritesheet);
            SoldierRenderer soldierRenderer = new(this._spriteBatch, spritesheet2, textureScale);
            SoldierController soldierController = new(soldier, soldierRenderer, highrise, collisionController, soundController);

            if (soldierSpawnPointController != null)
            {
                soldierSpawnPointController.HasSoldier = true;
                soldierSpawnPointController.AddSoldierController(soldierController);
            }

            soldierControllers.Add(soldierController);
            distanceCheckController.addSoldierController(soldierController);
        }

        public void CreateObstacle(Vector2 position, SoldierSpawnPointController soldierSpawnPointController)
        {
            Obstacle obstacle = new(new Vector2(position.X - obstacleSpriteRectangle.Width*textureScale.X + 16 , position.Y - (int)(obstacleSpriteRectangle.Height * textureScale.Y) + 4), obstacleSpriteRectangle, true, 5);
            ObstacleRenderer obstacleRenderer = new(this._spriteBatch, spritesheet2, textureScale);
            ObstacleController obstacleController = new(obstacle, obstacleRenderer, collisionController, soundController);

            reduceMonies(150);

            PlayerScore += (int)(150 * 1.35);

            if(soldierSpawnPointController != null)
            {
                soldierSpawnPointController.HasSoldier = true;
                soldierSpawnPointController.AddObstacleController(obstacleController);
            }

            distanceCheckController.addObstacleController(obstacleController);

            obstacleControllers.Add(obstacleController);

        }

        public void CreateMine(Vector2 position, SoldierSpawnPointController soldierSpawnPointController)
        {
            Mine mine = new(new Vector2(position.X - mineSpriteRectangle.Width * textureScale.X + 32, position.Y - (int)(mineSpriteRectangle.Height * textureScale.Y) + 5) , mineSpriteRectangle, false);
            MineRenderer mineRenderer = new(this._spriteBatch, spritesheet2, spritesheet, textureScale);
            MineController mineController = new(mine, mineRenderer, collisionController, soundController);

            reduceMonies(250);

            PlayerScore += (int)(150 * 1.35);

            if (soldierSpawnPointController != null)
            {
                soldierSpawnPointController.HasSoldier = true;
                soldierSpawnPointController.AddMineController(mineController);
            }

            // distanceCheckController.addObstacleController(mineController);

            mineControllers.Add(mineController);

        }
    }
}
