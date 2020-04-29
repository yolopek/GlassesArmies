﻿using System.Drawing;

namespace GlassesArmies
{
    public class Level
    {
        public Creature[] Enemies;
        public Wall[] Walls;
        public Creature StartCharacter;

        public Level(Creature[] enemies, Wall[] walls, Creature startCharacter)
        {
            Enemies = enemies;
            Walls = walls;
            StartCharacter = startCharacter;
        }

        public static Bitmap PlayerSoldierTexture = new Bitmap(@"..\..\Resources\Textures\soldier.png");
        public static Bitmap EnemySoldierTexture = new Bitmap(@"..\..\Resources\Textures\enemySoldier.png");
    }
}