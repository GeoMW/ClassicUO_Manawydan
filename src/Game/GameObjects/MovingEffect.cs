﻿#region license

//  Copyright (C) 2019 ClassicUO Development Community on Github
//
//	This project is an alternative client for the game Ultima Online.
//	The goal of this is to develop a lightweight client considering 
//	new technologies.  
//      
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

#endregion

using System;

using ClassicUO.Configuration;

using Microsoft.Xna.Framework;

namespace ClassicUO.Game.GameObjects
{
    internal sealed partial class MovingEffect : GameEffect
    {
        private uint _lastMoveTime;
        private int _distance;
        private Vector2 _velocity;

        private MovingEffect(Graphic graphic, Hue hue)
        {
            Hue = hue;
            Graphic = graphic;
            Load();
        }

        public MovingEffect(Serial src, Serial trg, int xSource, int ySource, int zSource, int xTarget, int yTarget, int zTarget, Graphic graphic, Hue hue, bool fixedDir, byte speed) : this(graphic, hue)
        {
            FixedDir = fixedDir;
            MovingDelay = (byte) (20 - speed);

            Entity source = World.Get(src);

            if (src.IsValid && source != null)
                SetSource(source);
            else
                SetSource(xSource, ySource, zSource);


            Entity target = World.Get(trg);

            if (trg.IsValid && target != null)
                SetTarget(target);
            else
                SetTarget(xTarget, yTarget, zTarget);



            int playerX = World.Player.X;
            int playerY = World.Player.Y;
            int playerZ = World.Player.Z;

            int screenCenterX = ProfileManager.Current.GameWindowPosition.X;
            int screenCenterY = ProfileManager.Current.GameWindowPosition.Y;


            (int sX, int sY, int sZ) = GetSource();
            int offsetSourceX = sX - playerX;
            int offsetSourceY = sY - playerY;
            int offsetSourceZ = sZ - playerZ;

            int screenSourceX = (ProfileManager.Current.GameWindowSize.X >> 1) + (offsetSourceX - offsetSourceY) * 22;
            int screenSourceY = (ProfileManager.Current.GameWindowSize.Y >> 1) + (offsetSourceX + offsetSourceY) * 22 - offsetSourceZ * 4;
            screenSourceX += screenCenterX;
            screenSourceY += screenCenterY;


            (int tX, int tY, int tZ) = GetTarget();
            int offsetTargetX = tX - playerX;
            int offsetTargetY = tY - playerY;
            int offsetTargetZ = tZ - playerZ;

            int screenTargetX = (ProfileManager.Current.GameWindowSize.X >> 1) + (offsetTargetX - offsetTargetY) * 22;
            int screenTargetY = (ProfileManager.Current.GameWindowSize.Y >> 1) + (offsetTargetX + offsetTargetY) * 22 - offsetTargetZ * 4;
            screenTargetX += screenCenterX;
            screenTargetY += screenCenterY;



            _distance = (int) Math.Sqrt(Math.Pow(screenSourceX - screenTargetX, 2) + Math.Pow(screenSourceY - screenTargetY, 2));

            _velocity.X = (screenTargetX - screenSourceX) * (MovingDelay / (float) _distance);
            _velocity.Y = (screenTargetY - screenSourceY) * (MovingDelay / (float) _distance);

            Vector2.Normalize(ref _velocity, out _velocity);

            AngleToTarget = (float) -Math.Atan2(screenTargetY - screenSourceY, screenTargetX - screenSourceX);
        }

        public float AngleToTarget;
        public bool Explode;
        public readonly bool FixedDir;
        public byte MovingDelay = 10;


        private void UpdateEx(double totalMS, double frameMS)
        {
            //if (_lastMoveTime > Time.Ticks)
            //    return;

            //_lastMoveTime = Time.Ticks + MovingDelay;

            if (Target != null && Target.IsDestroyed)
            {
                Destroy();
                return;
            }

            
            int playerX = World.Player.X;
            int playerY = World.Player.Y;
            int playerZ = World.Player.Z;

            int screenCenterX = ProfileManager.Current.GameWindowPosition.X;
            int screenCenterY = ProfileManager.Current.GameWindowPosition.Y;


            (int sX, int sY, int sZ) = GetSource();
            int offsetSourceX = sX - playerX;
            int offsetSourceY = sY - playerY;
            int offsetSourceZ = sZ - playerZ;

            int screenSourceX = (ProfileManager.Current.GameWindowSize.X >> 1) + (offsetSourceX - offsetSourceY) * 22;
            int screenSourceY = (ProfileManager.Current.GameWindowSize.Y >> 1) + (offsetSourceX + offsetSourceY) * 22 - offsetSourceZ * 4;
            screenSourceX += screenCenterX;
            screenSourceY += screenCenterY;


            (int tX, int tY, int tZ) = GetTarget();
            int offsetTargetX = tX - playerX;
            int offsetTargetY = tY - playerY;
            int offsetTargetZ = tZ - playerZ;

            int screenTargetX = (ProfileManager.Current.GameWindowSize.X >> 1) + (offsetTargetX - offsetTargetY) * 22;
            int screenTargetY = (ProfileManager.Current.GameWindowSize.Y >> 1) + (offsetTargetX + offsetTargetY) * 22 - offsetTargetZ * 4;
            screenTargetX += screenCenterX;
            screenTargetY += screenCenterY;


            int offX = (screenSourceX - screenTargetX) + (int) Math.Floor(Offset.X);
            int offY = (screenSourceY - screenTargetY) + (int) Math.Floor(Offset.Y);

            //int tileX = _distance + offX;
            //tileX /= 44;

            //int tileY = _distance + offY;
            //tileY /= 44;


            //Console.WriteLine("TILE: {0},{1}", tileX, tileY);

            int startX = screenSourceX + offX;
            int startY = screenSourceY + offY;

            int distanceNow = (int) Math.Sqrt(Math.Pow(startX - screenTargetX, 2) + Math.Pow(startY - screenTargetY, 2));

            if (distanceNow < _distance)
            //if (tileX <= 0 && tileY <= 0)
            {
                Destroy();
                return;
            }

            Offset.X += _velocity.X * (float) frameMS;
            Offset.Y += _velocity.Y * (float) frameMS;
        }

        public override void Update(double totalMS, double frameMS)
        {
            base.Update(totalMS, frameMS);
            UpdateEx(totalMS, frameMS);
            //Standard();
          
            //base.Update(totalMS, frameMS);
            //(int sx, int sy, int sz) = GetSource();
            //(int tx, int ty, int tz) = GetTarget();

            //if (_timeUntilHit == 0.0f)
            //{
            //    _timeActive = 0f;

            //    _timeUntilHit = (float) Math.Sqrt(
            //                                      Math.Pow((tx - sx), 2) +
            //                                      Math.Pow((ty - sy), 2) +
            //                                      Math.Pow((tz - sz), 2)) * 20;
            //}
            //else
            //{
            //    _timeActive += (float) frameMS;
            //}

            //if (_timeActive >= _timeUntilHit)
            //{
            //    Destroy();
            //    return;
            //}
            //else
            //{
            //    float x, y, z;
            //    x = (sx + (_timeActive / _timeUntilHit) * (float) (tx - sx));
            //    y = (sy + (_timeActive / _timeUntilHit) * (float) (ty - sy));
            //    z = (sz + (_timeActive / _timeUntilHit) * (float) (tz - sz));
            //    Position = new Position((ushort) x, (ushort) y, (sbyte) z);
            //    AddToTile();
            //    Offset.X = x % 1;
            //    Offset.Y = y % 1;
            //    Offset.Z = z % 1;
            //    AngleToTarget = (float) -((float) Math.Atan2((ty - sy), (tx - sx)) + (float) (Math.PI) * (1f / 4f));
            //}


            
        }

        private void Standard()
        {
            if (_lastMoveTime > Time.Ticks)
                return;

            _lastMoveTime = Time.Ticks + MovingDelay;

            (int sx, int sy, int sz) = GetSource();
            (int tx, int ty, int tz) = GetTarget();
            int screenCenterX = ProfileManager.Current.GameWindowPosition.X + (ProfileManager.Current.GameWindowSize.X >> 1);
            int screenCenterY = ProfileManager.Current.GameWindowPosition.Y + (ProfileManager.Current.GameWindowSize.Y >> 1);
            int playerX = World.Player.X;
            int playerY = World.Player.Y;
            int playerZ = World.Player.Z;
            int offsetX = sx - playerX;
            int offsetY = sy - playerY;
            int offsetZ = sz - playerZ;
            int drawX = screenCenterX + (offsetX - offsetY) * 22;
            int drawY = screenCenterY + ((offsetX + offsetY) * 22 - offsetZ * 4);
            int realDrawX = drawX + (int) Offset.X;
            int realDrawY = drawY + (int) Offset.Y;
            int offsetDestX = tx - playerX;
            int offsetDestY = ty - playerY;
            int offsetDestZ = tz - playerZ;
            int drawDestX = screenCenterX + (offsetDestX - offsetDestY) * 22;
            int drawDestY = screenCenterY + ((offsetDestX + offsetDestY) * 22 - offsetDestZ * 4);

            int[] deltaXY =
            {
                Math.Abs(drawDestX - realDrawX), Math.Abs(drawDestY - realDrawY)
            };
            int x = 0;

            if (deltaXY[0] < deltaXY[1])
            {
                x = 1;
                int temp = deltaXY[0];
                deltaXY[0] = deltaXY[1];
                deltaXY[1] = temp;
            }

            int delta = deltaXY[0];
            int stepXY = 0;
            const int EFFECT_SPEED = 5;

            int[] tempXY =
            {
                EFFECT_SPEED, 0
            };

            for (int j = 0; j < EFFECT_SPEED; j++)
            {
                stepXY += deltaXY[1];

                if (stepXY >= delta)
                {
                    tempXY[1]++;
                    stepXY -= deltaXY[0];
                }
            }

            if (realDrawX < drawDestX)
            {
                realDrawX += tempXY[x];

                if (realDrawX > drawDestX)
                    realDrawX = drawDestX;
            }
            else
            {
                realDrawX -= tempXY[x];

                if (realDrawX < drawDestX)
                    realDrawX = drawDestX;
            }

            if (realDrawY < drawDestY)
            {
                realDrawY += tempXY[(x + 1) % 2];

                if (realDrawY > drawDestY)
                    realDrawY = drawDestY;
            }
            else
            {
                realDrawY -= tempXY[(x + 1) % 2];

                if (realDrawY < drawDestY)
                    realDrawY = drawDestY;
            }

            int newOffsetX = (realDrawX - screenCenterX) / 22;
            int newOffsetY = (realDrawY - screenCenterY) / 22;
            int newCoordX = 0;
            int newCoordY = 0;
            TileOffsetOnMonitorToXY(ref newOffsetX, ref newOffsetY, ref newCoordX, ref newCoordY);
            int newX = playerX + newCoordX;
            int newY = playerY + newCoordY;

            if ((newX == tx && newY == ty && sz == tz) || (Target != null && Target.IsDestroyed))
            {
                if (Explode)
                {
                    //Position = new Position(Position.X, Position.Y, (sbyte) tz);
                    //Tile = World.Map.GetTile(tx, ty);
                }

                Destroy();
            }
            else
            {
                int newDrawX = screenCenterX + (newCoordX - newCoordY) * 22;
                int newDrawY = screenCenterY + ((newCoordX + newCoordY) * 22 - (offsetZ * 4));
                IsPositionChanged = true;
                Offset.X = realDrawX - newDrawX;
                Offset.Y = realDrawY - newDrawY;
                bool wantUpdateInRenderList = false;
                int countX = drawDestX - (newDrawX + (int) Offset.X);
                int countY = drawDestY - (newDrawY + (int) Offset.Y);

                if (sz != tz)
                {
                    //int stepsCountX = countX / (tempXY[x] + 1);
                    //int stepsCountY = countY / (tempXY[(x + 1) % 2] + 1);

                    //if (stepsCountX < stepsCountY)
                    //    stepsCountX = stepsCountY;

                    //if (stepsCountX <= 0)
                    //    stepsCountX = 1;
                    //int totalOffsetZ = 0;
                    //bool incZ = sz < tz;

                    //if (incZ)
                    //    totalOffsetZ = (tz - sz) << 2;
                    //else
                    //    totalOffsetZ = (sz - tz) << 2;
                    //totalOffsetZ /= stepsCountX;

                    //if (totalOffsetZ == 0)
                    //    totalOffsetZ = 1;
                    //Offset.Z += totalOffsetZ;
                    //if (Offset.Z >= 4)
                    //{
                    //    const int COUNT_Z = 1;

                    //    if (incZ)
                    //        sz += COUNT_Z;
                    //    else
                    //        sz -= COUNT_Z;

                    //    if (sz == tz)
                    //        Offset.Z = 0;
                    //    else
                    //        Offset.Z %= 8;
                    //    wantUpdateInRenderList = true;
                    //}
                }

                //countY -= (int) Offset.Z + ((tz - sz) << 2);
                if (!FixedDir)
                {
                    AngleToTarget = -(float) Math.Atan2(countY, countX);
                }

                if (sx != newX || sy != newY)
                {
                    sx = newX;
                    sy = newY;

                    wantUpdateInRenderList = true;
                }

                if (wantUpdateInRenderList)
                    SetSource(sx, sy, sz);
            }
        }

        private static void TileOffsetOnMonitorToXY(ref int ofsX, ref int ofsY, ref int x, ref int y)
        {
            if (ofsX == 0)
                x = y = ofsY >> 1;
            else if (ofsY == 0)
            {
                x = ofsX >> 1;
                y = -x;
            }
            else
            {
                int absX = Math.Abs(ofsX);
                int absY = Math.Abs(ofsY);
                x = ofsX;

                if (ofsY > ofsX)
                {
                    if (ofsX < 0 && ofsY < 0)
                        y = absX - absY;
                    else if (ofsX > 0 && ofsY > 0)
                        y = absY - absX;
                }
                else if (ofsX > ofsY)
                {
                    if (ofsX < 0 && ofsY < 0)
                        y = -(absY - absX);
                    else if (ofsX > 0 && ofsY > 0)
                        y = -(absX - absY);
                }

                if (y == 0 && ofsY != ofsX)
                {
                    if (ofsY < 0)
                        y = -(absX + absY);
                    else
                        y = absX + absY;
                }

                y /= 2;
                x += y;
            }
        }
    }
}