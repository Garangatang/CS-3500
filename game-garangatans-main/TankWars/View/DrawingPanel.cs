using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TankWars
{
    // Get to a point where can see walls being drawn, and how to do calculations for placing walls
    // How to center screen around tank each time. 
    public class DrawingPanel : Panel
    {
        private World theWorld;
        private Tank ourTank;

        private int worldSize;
        private int ourTankID = -1;

        private Image backgroundImage;
        private Image wallImage;
        private Image powerupImage;
        private Image gravestoneImage;

        private Dictionary<int, Tank> tanks;
        private Dictionary<int, Powerup> powerups;
        private Dictionary<int, Wall> walls;
        private Dictionary<int, Projectile> projectiles;
        private Dictionary<int, Beam> beams;

        private Dictionary<string, Image> images = new Dictionary<string, Image>();

       

        public DrawingPanel(World w)
        {
            this.BackColor = Color.Black;
            DoubleBuffered = true;
            theWorld = w;
            worldSize = theWorld.Size;
            setImages();
            tanks = theWorld.Tanks;
            powerups = theWorld.Powerups;
            walls = theWorld.Walls;
            projectiles = theWorld.Projectiles;
            beams = theWorld.Beams;
        }

        private void setImages()
        {
            //backround image
            Image origBackImage = Image.FromFile("../../../Resources/Sprites/Background.png");
            backgroundImage = resizeImage(origBackImage, new Size(worldSize, worldSize));

            //wall image
            Image origWallImage = Image.FromFile("../../../Resources/Sprites/WallSprite.png");
            wallImage = resizeImage(origWallImage, new Size(50, 50));

            //different colors of tank, turrets, and projectiles
            {
                //blueTankImage
                Image origTankImage = Image.FromFile("../../../Resources/Sprites/BlueTank.png");
                Image tankImageBlue = resizeImage(origTankImage, new Size(60, 60));
                images.Add("blueTank", tankImageBlue);

                //blueTurretImage
                Image origTurretImage = Image.FromFile("../../../Resources/Sprites/BlueTurret.png");
                Image turretImageBlue = resizeImage(origTurretImage, new Size(50, 50));
                images.Add("blueTurret", turretImageBlue);

                //blueProjectile
                Image origProjImage = Image.FromFile("../../../Resources/Sprites/shot-blue.png");
                Image projBlue = resizeImage(origTurretImage, new Size(30, 30));
                images.Add("blueProj", projBlue);

                //redTankImage
                Image origTankImageRed = Image.FromFile("../../../Resources/Sprites/RedTank.png");
                Image tankImageRed = resizeImage(origTankImageRed, new Size(60, 60));
                images.Add("redTank", tankImageRed);

                //redTurretImage
                Image origTurretImageRed = Image.FromFile("../../../Resources/Sprites/RedTurret.png");
                Image turretImageRed = resizeImage(origTurretImageRed, new Size(50, 50));
                images.Add("redTurret", turretImageRed);

                //redProjectile
                Image origProjImageRed = Image.FromFile("../../../Resources/Sprites/shot-red.png");
                Image projRed = resizeImage(origTurretImageRed, new Size(30, 30));
                images.Add("redProj", projRed);

                //greenTankImage
                Image origTankImageGreen = Image.FromFile("../../../Resources/Sprites/GreenTank.png");
                Image tankImageGreen = resizeImage(origTankImageGreen, new Size(60, 60));
                images.Add("greenTank", tankImageGreen);

                //greenTurretImage
                Image origTurretImageGreen = Image.FromFile("../../../Resources/Sprites/GreenTurret.png");
                Image turretImageGreen = resizeImage(origTurretImageGreen, new Size(50, 50));
                images.Add("greenTurret", turretImageGreen);

                //greenProjectile
                Image origProjImageGreen = Image.FromFile("../../../Resources/Sprites/shot-green.png");
                Image projGreen = resizeImage(origProjImageGreen, new Size(30, 30));
                images.Add("greenProj", projGreen);

                //darkTankImage
                Image origTankImageDark = Image.FromFile("../../../Resources/Sprites/DarkTank.png");
                Image tankImageDark = resizeImage(origTankImageDark, new Size(60, 60));
                images.Add("darkTank", tankImageDark);

                //darkTurretImage
                Image origTurretImageDark = Image.FromFile("../../../Resources/Sprites/DarkTurret.png");
                Image turretImageDark = resizeImage(origTurretImageDark, new Size(50, 50));
                images.Add("darkTurret", turretImageDark);

                //darkProjectile
                Image origProjImageDark = Image.FromFile("../../../Resources/Sprites/shot-violet.png");
                Image projDark = resizeImage(origProjImageDark, new Size(30, 30));
                images.Add("darkProj", projDark);

                //orangeTankImage
                Image origTankImageOrange = Image.FromFile("../../../Resources/Sprites/OrangeTank.png");
                Image tankImageOrange = resizeImage(origTankImageOrange, new Size(60, 60));
                images.Add("orangeTank", tankImageOrange);

                //orangeTurretImage
                Image origTurretImageOrange = Image.FromFile("../../../Resources/Sprites/OrangeTurret.png");
                Image turretImageOrange = resizeImage(origTurretImageOrange, new Size(50, 50));
                images.Add("orangeTurret", turretImageOrange);

                //orangeProjectile
                Image origProjImageOrange = Image.FromFile("../../../Resources/Sprites/shot-yellow.png");
                Image projOrange = resizeImage(origProjImageOrange, new Size(30, 30));
                images.Add("orangeProj", projOrange);

                //purpleTankImage
                Image origTankImagePurple = Image.FromFile("../../../Resources/Sprites/PurpleTank.png");
                Image tankImagePurple = resizeImage(origTankImagePurple, new Size(60, 60));
                images.Add("purpleTank", tankImagePurple);

                //purpleTurretImage
                Image origTurretImagePurple = Image.FromFile("../../../Resources/Sprites/PurpleTurret.png");
                Image turretImagePurple = resizeImage(origTurretImagePurple, new Size(50, 50));
                images.Add("purpleTurret", turretImagePurple);

                //purpleProjectile
                Image origProjImagePurple = Image.FromFile("../../../Resources/Sprites/shot-violet.png");
                Image projPurple = resizeImage(origProjImagePurple, new Size(30, 30));
                images.Add("purpleProj", projPurple);

                //LightGreenTankImage
                Image origTankImageLightGreen = Image.FromFile("../../../Resources/Sprites/LightGreenTank.png");
                Image tankImageLightGreen = resizeImage(origTankImageLightGreen, new Size(60, 60));
                images.Add("lightGreenTank", tankImageLightGreen);

                //LightGreenTurretImage
                Image origTurretImageLightGreen = Image.FromFile("../../../Resources/Sprites/LightGreenTurret.png");
                Image turretImageLightGreen = resizeImage(origTurretImageLightGreen, new Size(50, 50));
                images.Add("lightGreenTurret", turretImageLightGreen);

                //LightGreenProjectile
                Image origProjImageLightGreen = Image.FromFile("../../../Resources/Sprites/shot-green.png");
                Image projLightGreen = resizeImage(origProjImageLightGreen, new Size(30, 30));
                images.Add("lightGreenProj", projLightGreen);

                //YellowTankImage
                Image origTankImageYellow = Image.FromFile("../../../Resources/Sprites/YellowTank.png");
                Image tankImageYellow = resizeImage(origTankImageYellow, new Size(60, 60));
                images.Add("yellowTank", tankImageYellow);

                //YellowTurretImage
                Image origTurretImageYellow = Image.FromFile("../../../Resources/Sprites/YellowTurret.png");
                Image turretImageYellow = resizeImage(origTurretImageYellow, new Size(50, 50));
                images.Add("yellowTurret", turretImageYellow);

                //YellowProjectile
                Image origProjImageYellow = Image.FromFile("../../../Resources/Sprites/shot-yellow.png");
                Image projYellow = resizeImage(origProjImageYellow, new Size(30, 30));
                images.Add("yellowProj", projYellow);
            }

            //powerupImage
            Image origPowerImage = Image.FromFile("../../../Resources/Sprites/powerup.png");
            powerupImage = resizeImage(origPowerImage, new Size(30, 30));

            //gravestoneImage
            Image origGravestoneImage = Image.FromFile("../../../Resources/Sprites/gravestone.png");
            gravestoneImage = resizeImage(origGravestoneImage, new Size(50, 50));
        }


        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // "push" the current transform
            System.Drawing.Drawing2D.Matrix oldMatrix = e.Graphics.Transform.Clone();

            e.Graphics.TranslateTransform((int)worldX, (int)worldY);
            e.Graphics.RotateTransform((float)angle);
            drawer(o, e);

            // "pop" the transform
            e.Graphics.Transform = oldMatrix;
        }

        /// <summary>
        /// Called when form is invalidated to paint the form
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (ourTankID != -1 && theWorld.Tanks.ContainsKey(ourTankID))
            {
                ourTank = theWorld.Tanks[ourTankID];

                int viewSize = Size.Width; //view size is same as size length and width

                double playerX = ourTank.Location.GetX();
                double playerY = ourTank.Location.GetY();

                e.Graphics.TranslateTransform((float)-playerX + (viewSize / 2), (float)-playerY + (viewSize / 2)); //centering on player 

                //Drawing the background
                lock (theWorld)
                {
                    e.Graphics.DrawImage(backgroundImage, new Point((-worldSize / 2), (-worldSize / 2))); //Drawing image relative to origin (0, 0)

                    if (!(walls is null))
                    {
                        foreach (Wall w in walls.Values)
                        {
                            WallDrawer(w, e); //drawing each wall
                        }
                    }

                    if (!(tanks is null))
                    {
                        //Console.WriteLine("Tanks: " + tanks.Count);
                        foreach (Tank t in tanks.Values)
                        {
                            if (t.HP != 0)
                            {
                                DrawObjectWithTransform(e, t, t.Location.GetX(), t.Location.GetY(), t.orientation.ToAngle(), TankDrawer);
                                DrawObjectWithTransform(e, t, t.Location.GetX(), t.Location.GetY(), t.aiming.ToAngle(), TurretDrawer);
                                DrawObjectWithTransform(e, t, t.Location.GetX(), t.Location.GetY(), 0, NameAndHPDrawer);

                            }
                            if (t.HP == 0)
                            {
                                DrawObjectWithTransform(e, t, t.Location.GetX(), t.Location.GetY(), 0, DeathDrawer);
                            }
                        }
                    }

                    if (!(projectiles is null) && projectiles.Count != 0)
                    {
                        foreach (Projectile proj in projectiles.Values)
                        {
                            DrawObjectWithTransform(e, proj, proj.loc.GetX(), proj.loc.GetY(), proj.dir.ToAngle(), ProjDrawer);
                        }
                    }

                    if (!(powerups is null) && powerups.Count != 0)
                    {
                        foreach(Powerup power in powerups.Values)
                        {
                            DrawObjectWithTransform(e, power, power.loc.GetX(), power.loc.GetY(), 0, PowerupDrawer);
                        }
                    }

                    if (!(beams is null) && beams.Count != 0)
                    {
                        
                        foreach (Beam beam in beams.Values)
                        {
                            if (!beam.beamDictEmpty)
                            {
                                if (beam.frameCount != 0)
                                {
                                    DrawObjectWithTransform(e, beam, beam.org.GetX(), beam.org.GetY(), beam.dir.ToAngle() + 180, BeamDrawer);
                                    beam.frameCount -= 1;
                                }
                                else
                                {
                                    beam.beamDictEmpty = true;
                                    theWorld.Beams.Clear();
                                    break;
                                }
                            }
                            beam.frameCount -= 1;
                        }
                        
                    }
                }
            }
        }
        /// <summary>
        /// Method to draw the walls onto the map when first connecting to the server.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void WallDrawer(object o, PaintEventArgs e)
        {
            Wall w = o as Wall;

            Point firstPoint = new Point((int)w.p1.GetX(), (int)w.p1.GetY());
            Point secPoint = new Point((int)w.p2.GetX(), (int)w.p2.GetY());

            if (firstPoint.X == secPoint.X)
            {
                int length = CalculateLengthY(w);
                int smallY = FindSmallerY(w);
                int amtOfWalls = length / 50;

                for (int i = 0; i <= amtOfWalls; i++)
                {
                    DrawObjectWithTransform(e, w, w.p1.GetX(), smallY + i * 50, 0, drawWall);
                }
            }

            if (firstPoint.Y == secPoint.Y)
            {
                //Consider case if first y == second y
                int length = CalculateLengthX(w);
                int smallX = FindSmallerX(w);
                int amtOfWalls = length / 50;

                for (int i = 0; i <= amtOfWalls; i++)
                {
                    DrawObjectWithTransform(e, w, smallX + i * 50, w.p1.GetY(), 0, drawWall);
                }
            }

        }

        /// <summary>
        /// Drawing the wall onto the DrawingPanel
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void drawWall(object o, PaintEventArgs e)
        {
            Wall w = o as Wall;

            e.Graphics.DrawImage(wallImage, new PointF(-25, -25));
        }


        /// <summary>
        /// Drawing the tank onto the DrawingPanel at its position.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void TankDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;
            e.Graphics.DrawImage(images[FindTankColor(t) + "Tank"], new PointF(-30, -30));
        }

        /// <summary>
        /// Placing the turret on top of the tank.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void TurretDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;
            e.Graphics.DrawImage(images[FindTankColor(t) + "Turret"], new PointF(-25, -25));
        }

        /// <summary>
        /// Drawing the projectile after a mouse click event has happened.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ProjDrawer(object o, PaintEventArgs e)
        {
            Projectile p = o as Projectile;
            Tank t = theWorld.Tanks[p.owner];

            e.Graphics.DrawImage(images[FindTankColor(t) + "Proj"], new PointF(-15, -15));
        }

         /// <summary>
        /// Drawing tank name below tank and tank's health above the tank.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void NameAndHPDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;
            e.Graphics.DrawString(t.name + ": " + t.ID, new Font("Gadugi", 10), new SolidBrush(Color.White), new Point(-30, 40));
            int HP = t.HP;
            if (HP == 3)
            {
                Rectangle r = new Rectangle(-30, -40, 60, 8);
                e.Graphics.FillRectangle(new SolidBrush(Color.Green), r);
            }
            if (HP == 2)
            {
                Rectangle r = new Rectangle(-30, -40, 40, 8);
                e.Graphics.FillRectangle(new SolidBrush(Color.Yellow), r);
            }
            if (HP == 1)
            {
                Rectangle r = new Rectangle(-30, -40, 20, 8);
                e.Graphics.FillRectangle(new SolidBrush(Color.Red), r);
            }
        }

        /// <summary>
        /// Draws when a tank has died
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void DeathDrawer(object o, PaintEventArgs e)
        {
            e.Graphics.DrawImage(gravestoneImage, new Point(-25, -25));
        }

        /// <summary>
        /// Invalidates the form on frame change.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void OnFrameChanged(object o, EventArgs e)
        {
            //Force a call to the Paint event handler.
            this.Invalidate();
        }

        /// <summary>
        /// Draws the powerup onto the drawing panel.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void PowerupDrawer(object o, PaintEventArgs e)
        {
            e.Graphics.DrawImage(powerupImage, new Point(-15, -15));
        }

        private void BeamDrawer(object o, PaintEventArgs e)
        {
            Beam beam = o as Beam;
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            SolidBrush redBrush = new SolidBrush(Color.Red);
            SolidBrush purpleBrush = new SolidBrush(Color.Purple);
            Rectangle r = new Rectangle();
            if (beam.frameCount == 90)
            {
                r = new Rectangle(0, 0, 10, theWorld.Size);
                e.Graphics.FillRectangle(redBrush, r);
            }
            else if (beam.frameCount == 80)
            {
                r = new Rectangle(0, 0, 9, theWorld.Size); 
                e.Graphics.FillRectangle(purpleBrush, r);
            }
            else if (beam.frameCount == 60)
            {
                r = new Rectangle(0, 0, 8, theWorld.Size);
                e.Graphics.FillRectangle(whiteBrush, r);
            }
            else if (beam.frameCount == 50)
            {
                r = new Rectangle(0, 0, 7, theWorld.Size);
                e.Graphics.FillRectangle(purpleBrush, r);
            }
            else if (beam.frameCount == 40)
            {
                r = new Rectangle(0, 0, 6, theWorld.Size);
                e.Graphics.FillRectangle(redBrush, r);

            }
            else if (beam.frameCount == 30)
            {
                r = new Rectangle(0, 0, 5, theWorld.Size);
                e.Graphics.FillRectangle(redBrush, r);

            }
            else if (beam.frameCount == 20)
            {
                r = new Rectangle(0, 0, 4, theWorld.Size);
                e.Graphics.FillRectangle(purpleBrush, r);

            }
            else
            {
                r = new Rectangle(0, 0, 3, theWorld.Size);
                e.Graphics.FillRectangle(whiteBrush, r);
            }
        }

        /// <summary>
        /// Helper method to find the smaller Y for drawing walls
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private int FindSmallerY(Wall w)
        {
            Point firstPoint = new Point((int)w.p1.GetX(), (int)w.p1.GetY());
            Point secPoint = new Point((int)w.p2.GetX(), (int)w.p2.GetY());
            if (firstPoint.Y < secPoint.Y)
                return firstPoint.Y;

            else
                return secPoint.Y;
        }

        /// <summary>
        /// Helper method to find the smaller X for drawing walls
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private int FindSmallerX(Wall w)
        {
            Point firstPoint = new Point((int)w.p1.GetX(), (int)w.p1.GetY());
            Point secPoint = new Point((int)w.p2.GetX(), (int)w.p2.GetY());
            if (firstPoint.X < secPoint.X)
                return firstPoint.X;

            else
                return secPoint.X;
        }

        /// <summary>
        /// Helper method which finds the color for the tank
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private string FindTankColor (Tank t)
        {
            int colorID = t.ID % 8;
            if (colorID == 0)
                return "blue";
            else if (colorID == 1)
                return "red";
            else if (colorID == 2)
                return "green";
            else if (colorID == 3)
                return "dark";
            else if (colorID == 4)
                return "orange";
            else if (colorID == 5)
                return "purple";
            else if (colorID == 6)
                return "lightGreen";
            else
                return "yellow";
        }
   

        /// <summary>
        /// Get the length of the wall in the y direction
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private int CalculateLengthY(Wall w)
        {
            int firstY = (int)w.p1.GetY();
            int secY = (int)w.p2.GetY();

            if (firstY > secY)
                return firstY - secY;
            else
                return secY - firstY;
        }
        
        /// <summary>
        /// Get the length of the wall in the x direction.
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private int CalculateLengthX(Wall w)
        {
            int firstX = (int)w.p1.GetX();
            int secX = (int)w.p2.GetX();
            if (firstX > secX)
                return firstX - secX;
            else
                return secX - firstX;
        }
          
        /// <summary>
        /// Sprites come in at set sizes when downloaded from the internet,
        /// so they are resized using this image to fit the game parameters.
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        /// <summary>
        /// Set the player ID associated with a given tank.
        /// </summary>
        /// <param name="i"></param>
        public void setPlayerID(int i)
        {
            ourTankID = i;
            Console.WriteLine(ourTankID);
        }
    }
}