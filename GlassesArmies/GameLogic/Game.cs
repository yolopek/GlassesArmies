﻿using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace GlassesArmies
{
    public class Game
    {
        private List<Creature> _enemies;
        public IEnumerable<Creature> Alive => _aliveCretures; // tuples with location and texture
        private HashSet<Creature> _players;
        //players locations to allow enemies aim
        private HashSet<Creature> _aliveCretures; // probably not set
        public IEnumerable<Wall> Walls => _walls;
        private List<Wall> _walls;
        //ordered walls by borders
        
        private HashSet<Projectile> _projectiles;
        public IEnumerable<Projectile> Projectiles => _projectiles;
        public Creature Player;
        public Turn PlayersTurn { get; set; }

        //camera location
        //score

        //Turn
        // enemies -> ai
        // past_me -> list of actions

        public Game(Level level)
        {
            _enemies = new List<Creature>(level.Enemies.Length);
            _players = new HashSet<Creature>();
            _aliveCretures = new HashSet<Creature>();
            _projectiles = new HashSet<Projectile>();
            foreach (var enemy in level.Enemies)
            {
                //copy enemy
                enemy.Game = this;
                _enemies.Add(enemy);
                _aliveCretures.Add(enemy);
            }
            _walls = new List<Wall>(level.Walls);
            
            //player = level.StartCharacter.Copy();
            PlayersTurn = Turn.None;
            Player = level.StartCharacter;
            Player.Game = this;
        }

        public void MakeTurn()
        {
            foreach (var creature in _aliveCretures)
            {
                creature.MakeAutoTurn();
            }
            
            foreach (var projectile in _projectiles)
            {
                //check for hit
                projectile.Move();
                var projectileRect = projectile.ToRectangle();
                foreach (var creature in _aliveCretures.Where(creature => Geometry.CheckRectangleIntersection(projectileRect, creature.ToRectangle())))
                {
                    creature.TakeDamage(projectile.Damage);
                    projectile.Collide();
                }

                foreach (var dummy in _walls.Where(wall => Geometry.CheckRectangleIntersection(wall.ToRectangle(), projectileRect)))
                {
                    projectile.Collide();
                }
            }

            _projectiles.RemoveWhere(p => p.Live <= 0);
            _aliveCretures.RemoveWhere(c => c.HealthPoints <= 0);
            PlayersTurn.Action(Player);
            PlayersTurn = Turn.None;
            //Console.WriteLine("Wow");
        }

        public void AddProjectile(Projectile projectile)
        {
            _projectiles.Add(projectile);
        }
        // public void MakePlayersCreatureTurn(action)
        // {
        //     
        // }
    }
}