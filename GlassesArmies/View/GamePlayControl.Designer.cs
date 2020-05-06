﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GlassesArmies.View
{
    partial class GamePlayControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer _components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._timer = new Timer();
            this._timer.Interval = 20;
            // this._timer.Interval = 1000;

            this._isPaused = false;

            this.SuspendLayout();
            // 
            // GamePlayControl
            // 
            
            
            // 
            // pauseMenu
            //
            
            this._pauseMenu = new TableLayoutPanel();
            this._pauseMenu.Dock = DockStyle.Fill;
            this._pauseMenu.BackColor = Color.Transparent;
            
            
            //this.pauseMenu.BackgroundImage = coolDog;
            this._pauseMenu.BackColor = Color.FromArgb(50, Color.Gray);

            this._pauseMenu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            this._pauseMenu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90F));

            //resume
            //restart -> confirm
            //settings
            //exit
            
            var resume = new MainMenuButton("Resume");
            resume.Click += (sender, args) => ManagePauseMenu();
            
            var restart = new MainMenuButton("Restart");
            
            var settings = new MainMenuButton("Settings");
            settings.Click += (sender, args) => this._controller.ChangeState(Controller.State.Settings);
            
            var exit = new MainMenuButton("Exit");
            exit.Click += (sender, args) => this._controller.ChangeState(Controller.State.MainMenu);
            
            var pauseMenuButtons = new List<MainMenuButton>
            {
                resume,
                restart,
                settings,
                exit
            };
            
            pauseMenuButtons.For((button, index) =>
            {
                button.Anchor = AnchorStyles.Left;
                button.AutoSize = true;
                button.BackColor = Color.Transparent;
                
                //button.BackColor = Color.Aqua;
                //button.BackgroundImage = coolDog;
                
                this._pauseMenu.Controls.Add(button, 1, index);
                this._pauseMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            });

            this.Controls.Add(_pauseMenu);
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "GamePlayControl";

            this.KeyDown += OnKeyDown;
            this.Click += OnClick;
            this._timer.Tick += OnTimerTick;
            
            HidePauseMenu();
            Invalidate();
            this.ResumeLayout(false);
        }

        #endregion

        private Timer _timer;

        private TableLayoutPanel _pauseMenu;

        public void StopGame() => this._timer.Stop();

        public void ResumeGame()
        {
            if (!this._isPaused)
                this._timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs eventArgs)
        {
            //Console.WriteLine("Tick");
            _controller.TurnGame();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs eventArgs)
        {
            foreach (var projectile in _controller.GetProjectiles())
            {
                eventArgs.Graphics.FillRectangle(Brushes.Crimson, projectile);
            }
            foreach (var textureLocation in _controller.GetAliveCreature())
            {
                eventArgs.Graphics.DrawImage(textureLocation.Item1, textureLocation.Item2);
            }

            foreach (var gameWall in _controller.GetWalls())
            {
                eventArgs.Graphics.FillRectangle(Brushes.Silver, gameWall);
            }

            var playerData = _controller.GetPlayerData();
            eventArgs.Graphics.DrawImage(playerData.Item1, playerData.Item2);
            var trinagleCenter = new Point(playerData.Item2.X + playerData.Item1.Height / 2, playerData.Item2.Y - 15);
            eventArgs.Graphics.FillPolygon(Brushes.Red, new []
            {
                new Point(trinagleCenter.X, trinagleCenter.Y + 2),
                new Point(trinagleCenter.X - 3, trinagleCenter.Y - 3), 
                new Point(trinagleCenter.X + 3, trinagleCenter.Y - 3), 
            });
        }
        
        private Random _rng = new Random(1729);

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Console.WriteLine(e.KeyChar);
            // Console.WriteLine((int)e.KeyChar);
            //Enter is 13
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    ManagePauseMenu();
                    break;
                case Keys.D:
                    _controller.SetTurn(Turn.MoveRight);
                    break;
                case Keys.A:
                    _controller.SetTurn(Turn.MoveLeft);
                    break;
                case Keys.Space:
                    _controller.SetTurn(Turn.Jump);
                    break;
            }
        }

        private void OnClick(object sender, EventArgs eventArgs)
        {
            var mouseEventArgs = (MouseEventArgs) eventArgs;
            //Console.WriteLine(mouseEventArgs.Location);
            _controller.ShootInGame(mouseEventArgs.Location);
        }

        private bool _isPaused;
        
        private void ManagePauseMenu()
        {
            _isPaused = !_isPaused;
            if (!_isPaused)
            {
                HidePauseMenu();
                ResumeGame();
                //Console.WriteLine("Resumed");
            }
            else
            {
                StopGame();
                ShowPauseMenu();
            }
        }
        
        private void ShowPauseMenu()
        {
            this._pauseMenu.Show();
            this._pauseMenu.Enabled = true;
        }
        
        private void HidePauseMenu()
        {
            this._pauseMenu.Enabled = false;
            this._pauseMenu.Hide();
        }
    }
}