using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankWars
{
    public partial class TankWarsForm : Form
    {
        //handles updates from the "server"
        private Controller theController;

        // World is a simple container for Players and Powerups
        // The controller owns the world, but we have a reference to it
        private World theWorld;

        private int playerID;

        // This simple form only has two components
        DrawingPanel drawingPanel;
        Button connectButton;
        Label serverLabel;
        TextBox serverText;
        Label playerLabel;
        TextBox playerText;

        private const int viewSize = 900;
        private const int menuSize = 40;

        public TankWarsForm(Controller ctrl)
        {
            InitializeComponent();
            theController = ctrl;
            theWorld = theController.GetWorld();

            theController.UpdateArrived += OnFrame;
            theController.PlayerIdArrived += setID;
            theController.ShotArrived += PlayShot;
            theController.BeamArrived += PlayBeam;
            theController.PlayerDied += PlayDeath;

            ClientSize = new Size(viewSize, viewSize + menuSize);

            //GUI 
            {
                Font font = new Font("Gadugi", 10);
                // Place and add the button
                connectButton = new Button();
                connectButton.Location = new Point(400, 5);
                connectButton.Size = new Size(80, 27);
                connectButton.Text = "Connect";
                connectButton.Font = font;
                connectButton.Click += StartClick;
                this.Controls.Add(connectButton);

                // Place and add the name label
                serverLabel = new Label();
                serverLabel.Text = "Server:";
                serverLabel.Font = font;
                serverLabel.Location = new Point(5, 10);
                serverLabel.Size = new Size(50, 30);
                this.Controls.Add(serverLabel);

                // Place and add the name textbox
                serverText = new TextBox();
                serverText.Text = "localhost";
                serverText.Font = font;
                serverText.Location = new Point(55, 5);
                serverText.Size = new Size(100, 30);
                this.Controls.Add(serverText);

                //player Label
                playerLabel = new Label();
                playerLabel.Text = "Name: ";
                playerLabel.Font = font;
                playerLabel.Location = new Point(200, 10);
                playerLabel.Size = new Size(50, 30);
                this.Controls.Add(playerLabel);

                //player text
                playerText = new TextBox();
                playerText.Text = "player";
                playerText.Font = font;
                playerText.Location = new Point(250, 5);
                playerText.Size = new Size(100, 30);
                this.Controls.Add(playerText);



                // Place and add the drawing panel
                drawingPanel = new DrawingPanel(theWorld);
                drawingPanel.Location = new Point(0, menuSize);
                drawingPanel.Size = new Size(viewSize, viewSize);
          
                //drawingPanel.Paint += new PaintEventHandler(OnPaint);
                this.Controls.Add(drawingPanel);

                // Set up key and mouse handlers
                this.KeyDown += HandleKeyDown;
                this.KeyUp += HandleKeyUp;
                drawingPanel.MouseDown += HandleMouseDown;
                drawingPanel.MouseUp += HandleMouseUp;
                drawingPanel.MouseMove += HandleMouseMove;
            }
        }

        private void StartClick(object sender, EventArgs e)
        {
            if (serverText.Text == "")
            {
                MessageBox.Show("Please enter a server address");
                return;
            }
            connectButton.Enabled = false;
            serverText.Enabled = false;
            playerText.Enabled = false;
            KeyPreview = true;
            try
            {
                theController.Connect(serverText.Text, playerText.Text);
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("../../../Resources/Sprites/arcadeMusic.wav");
                player.Play();
            }
            catch
            {
                MessageBox.Show("Unable to connect to server.");
            }
        }
        
        /// <summary>
        /// Invalidates the current form
        /// </summary>
        private void OnFrame()
        {
            this.Invoke(new MethodInvoker(() => Invalidate(true)));

        }

        /// <summary>
        /// Key down handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
            if (e.KeyCode == Keys.W)
                theController.HandleMoveRequest("up");
            if (e.KeyCode == Keys.A)
                theController.HandleMoveRequest("left");
            if (e.KeyCode == Keys.S)
                theController.HandleMoveRequest("down");
            if (e.KeyCode == Keys.D)
                theController.HandleMoveRequest("right");

            // Prevent other key handlers from running
            e.SuppressKeyPress = true;
            e.Handled = true;
        }


        /// <summary>
        /// Key up handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                theController.CancelMoveRequest("up");
            if (e.KeyCode == Keys.A)
                theController.CancelMoveRequest("left");
            if (e.KeyCode == Keys.S)
                theController.CancelMoveRequest("down");
            if (e.KeyCode == Keys.D)
                theController.CancelMoveRequest("right");
        }


        /// <summary>
        /// Handle mouse down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                theController.HandleMouseRequest();
        }

        /// <summary>
        /// Handle mouse up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                theController.CancelMouseRequest();
        }

        /// <summary>
        /// Handler to update the angle of the turret as the mouse is moved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            Vector2D aimDir = new Vector2D(e.X - 450, e.Y - 450);
            aimDir.Normalize();
            theController.turretMoved(aimDir);
        }

        private void setID(int i)
        {
            playerID = i;
            drawingPanel.setPlayerID(i);
        }

        private void PlayShot()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("../../../Resources/Sprites/laserSound.wav");
            player.Play();
        }

        private void PlayDeath()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("../../../Resources/Sprites/deathSound.wav");
            player.Play();
        }

        private void PlayBeam()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("../../../Resources/Sprites/beamSound.wav");
            player.Play();
        }
       
    }
}
