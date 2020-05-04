﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace GlassesArmies
{
    public abstract class Creature
    {
        protected readonly double _dt = 1d / 60;

        public Game Game;
        
        //ai
        
        protected LinkedList<Turn> Turns;
        public Vector Location { get; protected set; }

        protected Vector Step;
        protected Vector Velocity;
        protected Vector JumpAcceleration;

        protected int HealthPoints;
        public bool IsAlive { get; protected set; } = true;
        
        public Game.CreatureSide Side { get; protected set; }


        public Bitmap Texture { get; protected set; }

        public Creature(Bitmap texture, Vector location)
        {
            StartTexture = texture;
            Texture = texture;
            StartLocation = location.Copy;
            Location = location.Copy;
            Step = new Vector(5, 0);
            Turns = new LinkedList<Turn>();
            Velocity = Vector.Zero;
        }

        protected Vector StartLocation { get; }

        protected Bitmap StartTexture { get; }

        public virtual void Move(Vector movement)
        {
            //check for collisions
            //maybe some acceleration
            Location += movement;
            //_turns.AddLast(Turn.Move(movement));
        }
        
        public virtual void MoveLeft()
        {
            Move(-Step);
        }

        public virtual void MoveRight()
        {
            Move(Step);
        }

        public virtual void Jump()
        {
            //if not in flight
            Velocity += JumpAcceleration;
        }

        public abstract void Shoot(Vector target);

        public abstract void MakeAutoTurn();

        public void MemorizeTurn(Turn turn)
        {
            if (Turns.Last == null)
            {
                Turns.AddLast(turn.Copy());
                return;
            }
            var currentTurn = Turns.Last.Value;
            if (turn.Type != Turn.Types.None && turn.Type != Turn.Types.MoveLeft && turn.Type != Turn.Types.MoveRight
                || currentTurn.Type != turn.Type)
            {
                Turns.AddLast(turn.Copy()); // copy to not increase counter from outside
            }
            else
            {
                currentTurn.Repetitions++;
            }
        }

        public abstract void MakeTurn(Turn turn);

        public void Accelerate(Vector impulse)
        {
            Velocity += impulse;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Location, Step).GetHashCode();
        }
        
        public Rectangle ToRectangle()
        {
            return new Rectangle(Location.ToPoint(), new Size(Texture.Width, -Texture.Height));
        }

        public abstract void TakeDamage(int damage);

        public abstract Creature Copy();
        
        public virtual void Reborn()
        {
            IsAlive = true;
        }
    }
}