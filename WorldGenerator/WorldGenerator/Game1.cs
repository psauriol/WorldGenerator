using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WorldGenerator
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D defaultTexture;
        int cellSize;
        Random rand;
        int randomSeedNumber;
        Color tempColor = new Color();
        MouseState currentMouse;
        MouseState oldMouse;
        KeyboardState currentKeyboard;
        KeyboardState oldKeyboard;
        int SystemX, SystemY = 0;
        int SR = 10;

        

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1000;//changes screen size
            graphics.PreferredBackBufferWidth = 1000;//changes screen size
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            rand = new Random();//add seed later
            randomSeedNumber = 1;
            this.IsMouseVisible = true;
            cellSize = 20;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            defaultTexture = Content.Load<Texture2D>(@"tempCell");
            base.Initialize();
            tempColor = Color.Red;

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
        /// all content.
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
            MouseUpdate(spriteBatch);
            KeyboardUpdate(spriteBatch);
            base.Update(gameTime);
        }

        protected void MouseUpdate(SpriteBatch batch)
        {
            currentMouse = Mouse.GetState();
            if (currentMouse.LeftButton == ButtonState.Pressed)// && oldMouse.LeftButton == ButtonState.Released)
            {
                SR++;
            }
            else if (currentMouse.RightButton == ButtonState.Pressed)// && oldMouse.RightButton == ButtonState.Released)
            {
                SR--;
            }
            

            if (oldMouse.ScrollWheelValue < currentMouse.ScrollWheelValue)//zoom out
            {
                cellSize += 1;
            }//if the mouse has been scrolled down && the cell size is greater than 1
            else if (oldMouse.ScrollWheelValue > currentMouse.ScrollWheelValue && cellSize > 1)//zoom in
            {
                cellSize -= 1;
            }

            oldMouse = currentMouse;
        }

        protected void KeyboardUpdate(SpriteBatch batch)
        {
            currentKeyboard = Keyboard.GetState();
            if (currentKeyboard.IsKeyDown(Keys.W))
            {
                SystemY++;
            }
            if (currentKeyboard.IsKeyDown(Keys.S))
            {
                SystemY--;
            }
            if (currentKeyboard.IsKeyDown(Keys.A))
            {
                SystemX++;
            }
            if (currentKeyboard.IsKeyDown(Keys.D))
            {
                SystemX--;
            }
            if (currentKeyboard.IsKeyDown(Keys.C) && !oldKeyboard.IsKeyDown(Keys.C))
            {
                randomSeedNumber++;
            }
            if (currentKeyboard.IsKeyDown(Keys.V) && !oldKeyboard.IsKeyDown(Keys.V))
            {
                randomSeedNumber--;
            }




            oldKeyboard = currentKeyboard;
        }
        double t = 0;//part of comet move code
        protected override void Draw(GameTime gameTime)
        {
            t += 0.01;
            GraphicsDevice.Clear(Color.Navy);

            spriteBatch.Begin();
            //DrawSun(SystemX, SystemY);
            //CreatePlanet(SystemX+15, SystemY+15);

            #region cometMoveCode
            int radius = 100;//how far form the center it orbits
            int orbitDirection = 1;//1 is clockwise | -1 is counterclockwise
            double orbitSpeed = 1.00;//how fast the orbit is
            double eccentricyX = 1;//how squashed the orbit is
            double eccentricyY = 0;//how squashed the orbit is
            Vector2 origen = new Vector2(0, 0);

            Vector2 objectLocation = new Vector2(SystemX, SystemY);

            objectLocation.X = SystemX + (int)(radius / 2 * Math.Cos(t * orbitDirection * orbitSpeed + eccentricyX)) + 0; //+0 is number of pixels over to draw it
            objectLocation.Y = SystemY + (int)(radius / 2 * Math.Sin(t * orbitDirection * orbitSpeed + eccentricyY)) + 0;
            #endregion cometMoveCode
            //CreateComet((int)objectLocation.X, (int)objectLocation.Y);


            CreateAsteroid(SystemX + 25, SystemY + 25);
            //CreateSystem(); //eats ALL memory only run in init
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawCell(SpriteBatch batch, int cellX, int cellY, Color blockType)
        {
            //tempColor.R = (byte)rand.Next(1, 254);
            Rectangle tempCell = new Rectangle(cellX * cellSize, cellY * cellSize, cellSize, cellSize);
            batch.Draw(defaultTexture, tempCell, blockType);

        }

        void CreateSystem()
        {
            int sunRadius = rand.Next(50, 100);
            int numOfPlanets = rand.Next(1, 3);
            int numOfComets = rand.Next(3, 5);
            


            for (int i = 0; i <= numOfPlanets; i++)
            {
                CreatePlanet(SystemX + (i*500), SystemY + (i*0));
            }
            for (int i = 0; i <= numOfComets; i++)
            {
                CreateComet(SystemX + 0, SystemY + 0);
            }


            int systemRadiusVert = 200;
            int systemRadiusHorz = 200;
            for (int Xi = (SystemX - systemRadiusHorz); Xi <= (SystemX + systemRadiusHorz); Xi++)
            //for (int Xi = (planetX + planetRadiusHorz); Xi >= (planetX - planetRadiusHorz); Xi--)//for backwards drawing
            {
                for (int Yi = (SystemY - systemRadiusVert); Yi <= (SystemY + systemRadiusVert); Yi++)
                {
                    double D = ((Math.Pow(Xi - SystemX, 2)) / (Math.Pow(systemRadiusHorz, 2))) + ((Math.Pow(Yi - SystemY, 2)) / (Math.Pow(systemRadiusVert, 2)));
                    if (D < 1)//draw inside the main oval
                    {
                        if (D > 0.2)
                        {
                            if (rand.Next(1, 101) < 20)
                            {
                                CreateAsteroid(Xi, Yi);
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        #region MultiObjectCreation //*********************************************************************************************//


        void CreateAsteroidBelt()
        {
            //for r > 20
            //CreateAsteroid();
        }
        #endregion MultiObjectCreation

        #region CreateSingleObject //*********************************************************************************************//

        void CreateComet(int cometX, int cometY)
        {
            int cometRadiusVert = 10;
            int cometRadiusHorz = 12;
            for (int Xi = (cometX - cometRadiusHorz); Xi <= (cometX + cometRadiusHorz); Xi++)
            //for (int Xi = (planetX + planetRadiusHorz); Xi >= (planetX - planetRadiusHorz); Xi--)//for backwards drawing
            {
                for (int Yi = (cometY - cometRadiusVert); Yi <= (cometY + cometRadiusVert); Yi++)
                {
                    double D = ((Math.Pow(Xi - cometX, 2)) / (Math.Pow(cometRadiusHorz, 2))) + ((Math.Pow(Yi - cometY, 2)) / (Math.Pow(cometRadiusVert, 2)));
                    if (D < 1)//draw inside the main oval
                    {
                        DrawCell(spriteBatch, Xi, Yi, Color.LightBlue);//draws the circle
                        if (D < .3)
                        {
                            DrawCell(spriteBatch, Xi, Yi, Color.Blue);
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        void CreatePlanet(int planetX, int planetY)
        {
            int planetRadiusVert = 100;
            int planetRadiusHorz = 100;
            for (int Xi = (planetX - planetRadiusHorz); Xi <= (planetX + planetRadiusHorz); Xi++)
            //for (int Xi = (planetX + planetRadiusHorz); Xi >= (planetX - planetRadiusHorz); Xi--)//for backwards drawing
            {
                for (int Yi = (planetY - planetRadiusVert); Yi <= (planetY + planetRadiusVert); Yi++)
                {
                    double D = ((Math.Pow(Xi - planetX, 2)) / (Math.Pow(planetRadiusHorz, 2))) + ((Math.Pow(Yi - planetY, 2)) / (Math.Pow(planetRadiusVert, 2)));
                    if (D < 1)//draw inside the main oval
                    {
                        DrawCell(spriteBatch, Xi, Yi, Color.Brown);//draws the circle
                    }
                    else
                    {

                    }
                }
            }

            //CreateCavern(planetX, planetY, planetRadiusHorz, planetRadiusVert);
            //CreateChasm(planetX, planetY, planetRadiusHorz, planetRadiusVert);
            testLine(planetX, planetY, planetRadiusHorz, planetRadiusVert);
        }

        void CreateAsteroid(int asteroidX, int asteroidY)
        {
            //int AstRadiusVert = 5;
            //int AstRadiusHorz = 10;
            rand = new Random(randomSeedNumber);
            int AstRadiusVert = rand.Next(5, 20);
            int AstRadiusHorz = rand.Next(5, 20);

            for (int Xi = (asteroidX - AstRadiusHorz); Xi <= (asteroidX + AstRadiusHorz); Xi++)
            {
                for (int Yi = (asteroidY - AstRadiusVert); Yi <= (asteroidY + AstRadiusVert); Yi++)
                {
                    double D = ((Math.Pow(Xi - asteroidX, 2)) / (Math.Pow(AstRadiusHorz, 2))) + ((Math.Pow(Yi - asteroidY, 2)) / (Math.Pow(AstRadiusVert, 2)));


                    if (D < 1.2)//draw inside the main oval
                    {
                        if (rand.Next(1, 100) < 85)
                        {
                            DrawCell(spriteBatch, Xi, Yi, Color.Brown);//Rock
                        }
                        else
                        {
                            DrawCell(spriteBatch, Xi, Yi, Color.Transparent);//nothing
                        }

                        if (rand.Next(0, 100) < 5)//resource loop draws OVER more common ones
                        {
                             CreateIronPatch(Xi, Yi);
                        }
                        if (rand.Next(0, 100) < 1)//1%
                        {
                            CreateTitaniumPatch(Xi, Yi);
                        }
                        if (rand.Next(0, 10000) < 1)//.001%
                        {
                            DrawCell(spriteBatch, Xi, Yi, Color.Red);
                        }
                    }
                    else//outside oval
                    {
                        if (rand.Next(1, 10) < 2)//Add rock to outside of oval to obfuscate shape
                        {
                            DrawCell(spriteBatch, Xi, Yi, Color.Brown);//rock
                        }
                    }
                }
            }
        }

        void DrawSun(int sunX, int sunY)
        {
            double R = SR;

            //for (int Xi = (sunX - R); Xi <= (sunX + R); Xi++)
            //{
            //    for (int Yi = (sunY - R); Yi <= (sunY + R); Yi++)
            //    {
            //        double D = (Math.Pow(Xi - sunX, 2) + Math.Pow(Yi - sunY, 2));

            //        if (D < Math.Pow(R, 2))
            //        {
            //            DrawCell(spriteBatch, Xi, Yi);
            //        }
            //    }
            //}



            for (; R > 0; R -= .5)
            {
                for (double i = 0; i <= R * .75; i += 0.75)
                {
                    double Y;
                    double X = i;

                    Y = Math.Sqrt(Math.Pow(R, 2) - Math.Pow(X, 2));
                    if (R > 1)
                    {
                        X = Math.Ceiling(X) - 1;//why didn't i just use floor instead of ceiling -1????? look into this at some point
                        Y = Math.Ceiling(Y) - 1;
                    }
                    else
                    {
                        X = 0;
                        Y = 0;
                    }

                    DrawCell(spriteBatch, sunX + (int)X, sunY + (int)Y, tempColor);
                    DrawCell(spriteBatch, sunX - (int)X, sunY + (int)Y, tempColor);
                    DrawCell(spriteBatch, sunX + (int)X, sunY - (int)Y, tempColor);
                    DrawCell(spriteBatch, sunX - (int)X, sunY - (int)Y, tempColor);

                    DrawCell(spriteBatch, sunX + (int)Y, sunY + (int)X, tempColor);
                    DrawCell(spriteBatch, sunX - (int)Y, sunY + (int)X, tempColor);
                    DrawCell(spriteBatch, sunX + (int)Y, sunY - (int)X, tempColor);
                    DrawCell(spriteBatch, sunX - (int)Y, sunY - (int)X, tempColor);
                }
            }
        }

        #endregion CreateSingleObject

        #region ObjectFeatures //*********************************************************************************************//

        void testLine(int objectX, int objectY, int objectRadiusHorz, int objectRadiusVert)
        {
            double offsetX = 2;
            double offsetY = 10;
            double ProtateX = 0;
            double ProtateY = 0;

            double RotY = 1;
            double RotX = 1;

            if (Math.Abs(offsetX) < Math.Abs(offsetY))
            {
                RotX = 1;
                RotY = -(offsetX / offsetY);
            }
            else if (Math.Abs(offsetY) < Math.Abs(offsetX))
            {
                RotY = 1;
                RotX = -(offsetY / offsetX);
            }
            else //X==Y
            {
                if (offsetX * offsetY > 0)
                {
                    RotX = 1;
                    RotY = -1;
                }
                else
                {
                    RotX = 1;
                    RotY = 1;
                }
            }

            double rotateX = ProtateX;
            double rotateY = ProtateY;
            for (int i = 0; i <= 10; i++)
            {
                offsetX += RotX/2;
                offsetY += RotY/2;
                rotateX = ProtateX;
                rotateX = ProtateY;
                for (int e = 0; e <= 10; e++)//chasm width
                {

                    DrawCell(spriteBatch, (int)(objectX + offsetX + Math.Floor(rotateX)), (int)(objectY + offsetY + Math.Floor(rotateY)), Color.White);
                    DrawCell(spriteBatch, (int)(objectX + offsetX - Math.Floor(rotateX)), (int)(objectY + offsetY - Math.Floor(rotateY)), Color.White);

                    rotateX += RotX;
                    rotateY += RotY;
                }
            }
            DrawCell(spriteBatch, objectX, objectY, Color.Red);
        }

        void CreateCavern(int objectX, int objectY, int objectRadiusHorz, int objectRadiusVert)//tweak to starting height and in/decrease algrthm
        {
            int startX = rand.Next(objectX - objectRadiusHorz, objectX + objectRadiusHorz);
            int startY = rand.Next(objectY - objectRadiusVert, objectY + objectRadiusVert);

            double D = ((Math.Pow(startX - objectX, 2)) / (Math.Pow(objectRadiusHorz, 2))) + ((Math.Pow(startY - objectY, 2)) / (Math.Pow(objectRadiusVert, 2)));

            if (D < 0.75)
            {
                //for (int e = 0; e < 30; e++)
                int e = 0;
                int CaveRand;
                int caveHeight = 20;
                while( caveHeight > 0)
                {
                    CaveRand = rand.Next(1, 101);
                    for (int i = 0; i <= caveHeight; i++)
                    {
                        DrawCell(spriteBatch, startX + e, startY + (int)(i / 3), Color.White);
                        DrawCell(spriteBatch, startX + e, startY - i, Color.White);

                    }
                    if (CaveRand < 50)
                    {
                        caveHeight--;
                    }
                    else if (CaveRand < 60)
                    {
                        caveHeight++;
                    }
                    else
                    {
                        //cave hieght stays same
                    }
                    e++;
                }
                caveHeight = 20;
                e = 0;
                while (caveHeight > 0)
                {
                    CaveRand = rand.Next(1, 101);
                    for (int i = 0; i <= caveHeight; i++)
                    {
                        DrawCell(spriteBatch, startX - e, startY + (int)(i / 3), Color.White);
                        DrawCell(spriteBatch, startX - e, startY - i, Color.White);

                    }
                    if (CaveRand < 50)
                    {
                        caveHeight--;
                    }
                    else if (CaveRand < 60)
                    {
                        caveHeight++;
                    }
                    else
                    {
                        //cave hieght stays same
                    }
                    e++;
                }
            }
        }

        void CreateChasm(int objectX, int objectY, int objectRadiusHorz, int objectRadiusVert)//work on direction cavern faces
        {
            int locationDegrees = rand.Next(0, 361);
            int startX = (int)(Math.Sin(locationDegrees) * objectRadiusHorz);//Xoffset
            int startY = (int)(Math.Cos(locationDegrees) * objectRadiusVert);//Yoffset


            if (locationDegrees < 90)
            {
                objectX += startX;
                objectY += startY;
            }
            else if (locationDegrees < 180)
            {
                objectX -= startX;
                objectY += startY;
            }
            else if (locationDegrees < 270)
            {
                objectX -= startX;
                objectY -= startY;
            }
            else//location degrees < 360
            {
                objectX += startX;
                objectY -= startY;
            }




            ////////////////add if depth over %planet radius ends////
            int Cwidth = rand.Next(8, 25);//leave this a flat value NOT one dependent on object size//maybe make growth/shrink % depend on planet size
            int CShift = 0;
            int CShiftBias = 0;
            int Cdepth = 0;
            int CGrowShrink;
            int CShiftWidth;
            while (Cwidth > 0)
            {
                CGrowShrink = rand.Next(1, 101);
                if (CGrowShrink < 25)
                {
                    Cwidth--;
                }
                else if (CGrowShrink < 29)
                {
                    Cwidth++;
                }
                else
                {
                    //no change
                }

                CShiftWidth = rand.Next(1, 101);
                if (CShiftWidth < 20)
                {
                    CShift += -(int)(Cwidth / 10);
                }
                else if (CShiftWidth < 40)
                {
                    CShift += (int)(Cwidth / 10);
                }
                else if (CShiftWidth < 50)
                {
                    CShift += -(int)(Cwidth / 5);
                }
                else if (CShiftWidth < 60)
                {
                    CShift += (int)(Cwidth / 5);
                }
                else if (CShiftWidth < 65)
                {
                    CShift += -(int)(Cwidth / 3);
                }
                else if (CShiftWidth < 70)
                {
                    CShift += (int)(Cwidth / 3);
                }
                else if (CShiftWidth > 98)
                {
                     CShiftBias++;
                }
                else if (CShiftWidth > 96)
                {
                    CShiftBias--;
                }


                for (int e = 0; e <= Cwidth; e++)//chasm width
                {

                    DrawCell(spriteBatch, objectX + (Cdepth), objectY + e + CShift + CShiftBias, Color.White);
                    DrawCell(spriteBatch, objectX + (Cdepth), objectY - (int)(e / 2) + CShift + CShiftBias, Color.White);
                }
                Cdepth++;
            }
        }

        #endregion ObjectFeatures

        #region CreateResourcePatch //*********************************************************************************************//

        void CreateIronPatch(int IrX, int IrY)
        {
            DrawCell(spriteBatch, IrX, IrY, Color.Gray);

            int temp = rand.Next(1, 9);
            switch (temp)
            {
                case 1://3x3
                    DrawCell(spriteBatch, IrX + 1, IrY, Color.Gray);
                    DrawCell(spriteBatch, IrX + 1, IrY + 1, Color.Gray);
                    DrawCell(spriteBatch, IrX + 1, IrY - 1, Color.Gray);
                    DrawCell(spriteBatch, IrX, IrY + 1, Color.Gray);
                    DrawCell(spriteBatch, IrX, IrY - 1, Color.Gray);
                    DrawCell(spriteBatch, IrX - 1, IrY, Color.Gray);
                    DrawCell(spriteBatch, IrX - 1, IrY + 1, Color.Gray);
                    DrawCell(spriteBatch, IrX - 1, IrY - 1, Color.Gray);
                    break;
                case 2://2x3 rect
                    DrawCell(spriteBatch, IrX + 1, IrY, Color.Gray);
                    DrawCell(spriteBatch, IrX + 1, IrY + 1, Color.Gray);
                    DrawCell(spriteBatch, IrX + 1, IrY - 1, Color.Gray);
                    DrawCell(spriteBatch, IrX, IrY + 1, Color.Gray);
                    DrawCell(spriteBatch, IrX, IrY - 1, Color.Gray);
                    break;
                case 3://2x2
                    DrawCell(spriteBatch, IrX + 1, IrY, Color.Gray);
                    DrawCell(spriteBatch, IrX + 1, IrY + 1, Color.Gray);
                    DrawCell(spriteBatch, IrX, IrY + 1, Color.Gray);
                    break;
                case 4://+
                    DrawCell(spriteBatch, IrX + 1, IrY, Color.Gray);
                    DrawCell(spriteBatch, IrX - 1, IrY, Color.Gray);
                    DrawCell(spriteBatch, IrX, IrY + 1, Color.Gray);
                    DrawCell(spriteBatch, IrX, IrY - 1, Color.Gray);
                    break;
                case 5://T
                    DrawCell(spriteBatch, IrX + 1, IrY, Color.Gray);
                    DrawCell(spriteBatch, IrX + 1, IrY + 1, Color.Gray);
                    DrawCell(spriteBatch, IrX + 1, IrY - 1, Color.Gray);
                    DrawCell(spriteBatch, IrX - 1, IrY, Color.Gray);
                    break;
                case 6://missing top right corner
                    DrawCell(spriteBatch, IrX - 1, IrY, Color.Gray);
                    DrawCell(spriteBatch, IrX - 1, IrY + 1, Color.Gray);
                    DrawCell(spriteBatch, IrX - 1, IrY - 1, Color.Gray);
                    DrawCell(spriteBatch, IrX, IrY + 1, Color.Gray);
                    DrawCell(spriteBatch, IrX, IrY - 1, Color.Gray);
                    break;
                case 7://tiny rotated reverse L
                    DrawCell(spriteBatch, IrX - 1, IrY, Color.Gray);
                    DrawCell(spriteBatch, IrX - 1, IrY + 1, Color.Gray);
                    break;
                case 8://tiny rotated L
                    DrawCell(spriteBatch, IrX - 1, IrY, Color.Gray);
                    DrawCell(spriteBatch, IrX - 1, IrY - 1, Color.Gray);
                    break;
            }
        }

        void CreateTitaniumPatch(int TitX, int TitY)
        {
            DrawCell(spriteBatch, TitX, TitY, Color.Silver);
            switch(rand.Next(1, 5))
            {
                case 1:
                    DrawCell(spriteBatch, TitX + 1, TitY, Color.Silver);
                    DrawCell(spriteBatch, TitX + 1, TitY + 1, Color.Silver);
                    DrawCell(spriteBatch, TitX, TitY + 1, Color.Silver);
                    break;
                case 2:
                    DrawCell(spriteBatch, TitX + 1, TitY, Color.Silver);
                    DrawCell(spriteBatch, TitX + 1, TitY - 1, Color.Silver);
                    DrawCell(spriteBatch, TitX, TitY - 1, Color.Silver);
                    break;
                case 3:
                    DrawCell(spriteBatch, TitX - 1, TitY, Color.Silver);
                    DrawCell(spriteBatch, TitX - 1, TitY + 1, Color.Silver);
                    DrawCell(spriteBatch, TitX, TitY + 1, Color.Silver);
                    break;
                case 4:
                    DrawCell(spriteBatch, TitX - 1, TitY, Color.Silver);
                    DrawCell(spriteBatch, TitX - 1, TitY - 1, Color.Silver);
                    DrawCell(spriteBatch, TitX, TitY - 1, Color.Silver);
                    break;
            }
        }

        #endregion CreateResourcePatch
    }
}
