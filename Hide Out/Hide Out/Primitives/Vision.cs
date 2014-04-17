using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using HideOut.Entities;

namespace HideOut.Primitives
{
    class Vision
    {
        private static double maxViewAngle = MathHelper.PiOver4;

        public float viewDistance { get; set; }
        public float maxViewDistance { get; set; }

        private double _viewAngle { get; set; }
        public double viewAngle
        {
            get
            {
                return this._viewAngle;
            }
            set
            {
                if (value < 0) 
                    _viewAngle = 0;
                else if (value > maxViewAngle) 
                    _viewAngle = maxViewAngle;
                else 
                    _viewAngle = value;
            }
        }

        public Rectangle parentLocation { get; set; }

        private Vector2 _viewDirection { get; set; }
        public Vector2 viewDirection
        {
            get
            {
                return this._viewDirection;
            }
            set
            {
                this._viewDirection = Normalize(value);
            }
        }

        public Color viewColor { get; set; }

        public Vision(Rectangle pLoc, float vDis, double vAng, Vector2 vDir, Color vCol)
        {
            parentLocation = pLoc;
            viewDistance = vDis;
            maxViewDistance = vDis;
            viewAngle = vAng;
            viewDirection = vDir;
            viewColor = vCol;
        }

        public bool OnScreen()
        {
            VertexPositionColor[] fieldOfVIsion = GetFieldOfViewTriangle();
            foreach (VertexPositionColor v in fieldOfVIsion)
            {
                if (v.Position.X > HideOutGame.SCREEN_OFFSET_X - 100 && v.Position.X < HideOutGame.SCREEN_OFFSET_X + HideOutGame.SCREEN_WIDTH + 100 &&
                    v.Position.Y > HideOutGame.SCREEN_OFFSET_Y - 100 && v.Position.Y < HideOutGame.SCREEN_OFFSET_Y + HideOutGame.SCREEN_HEIGHT + 100)
                    return true;
            }
            return false;
        }

        public VertexPositionColor[] GetFieldOfViewTriangle()
        {
            VertexPositionColor[] fieldOfVision = new VertexPositionColor[3];

            fieldOfVision[0].Color = viewColor;
            fieldOfVision[1].Color = viewColor;
            fieldOfVision[2].Color = viewColor;

            //Index 0 is located at the center of the rectangle
            fieldOfVision[0].Position = new Vector3(parentLocation.X + parentLocation.Width / 2, parentLocation.Y + parentLocation.Height / 2, 0);

            //Indices 1 and 2 indicate the farthest points of view on the left and right sides, respectively, of the Searcher
            double theta = GetTheta();
            fieldOfVision[1].Position = new Vector3((float)(viewDistance * Math.Cos(theta + viewAngle)) + fieldOfVision[0].Position.X,
                (float)(viewDistance * Math.Sin(theta + viewAngle)) + fieldOfVision[0].Position.Y,
                0);
            fieldOfVision[2].Position = new Vector3((float)(viewDistance * Math.Cos(theta - viewAngle)) + fieldOfVision[0].Position.X,
                (float)(viewDistance * Math.Sin(theta - viewAngle)) + fieldOfVision[0].Position.Y
                , 0);

            return fieldOfVision;
        }

        public VertexPositionColor[] GetFieldOfViewTriangleToDraw()
        {
            VertexPositionColor[] fieldOfVision = new VertexPositionColor[3];

            fieldOfVision[0].Color = viewColor;
            fieldOfVision[1].Color = viewColor;
            fieldOfVision[2].Color = viewColor;

            //Index 0 is located at the center of the rectangle
            fieldOfVision[0].Position = new Vector3(parentLocation.X + parentLocation.Width / 2 - HideOutGame.SCREEN_OFFSET_X, parentLocation.Y + parentLocation.Height / 2 - HideOutGame.SCREEN_OFFSET_Y, 0);

            //Indices 1 and 2 indicate the farthest points of view on the left and right sides, respectively, of the Searcher
            double theta = GetTheta();
            fieldOfVision[1].Position = new Vector3((float)(viewDistance * Math.Cos(theta + viewAngle)) + fieldOfVision[0].Position.X,
                (float)(viewDistance * Math.Sin(theta + viewAngle)) + fieldOfVision[0].Position.Y,
                0);
            fieldOfVision[2].Position = new Vector3((float)(viewDistance * Math.Cos(theta - viewAngle)) + fieldOfVision[0].Position.X,
                (float)(viewDistance * Math.Sin(theta - viewAngle)) + fieldOfVision[0].Position.Y
                , 0);

            return fieldOfVision;
        }

        public void Rotate(double angle) //positive to turn left, negative to turn right
        {
            double theta = GetTheta();
            viewDirection = new Vector2((float)Math.Cos(theta + angle), (float)Math.Sin(theta + angle));
        }

        public static Vector2 Intersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            Vector2 b = a2 - a1;
            Vector2 d = b2 - b1;
            var bDotDPerp = b.X * d.Y - b.Y * d.X;

            // if b dot d == 0, it means the lines are parallel so have infinite intersection points
            if (bDotDPerp == 0)
                return new Vector2(-1000, -1000);

            Vector2 c = b1 - a1;
            var t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
            if (t < 0 || t > 1)
            {
                return new Vector2(-1000, -1000);
            }

            var u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
            if (u < 0 || u > 1)
            {
                return new Vector2(-1000, -1000);
            }

            return a1 + t * b;
        }

        public float DistanceSquared(Vector2 p1, Vector2 p2)
        {
            return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
        }

        public void Update(List<Obstacle> obstacles)
        {
            double theta = GetTheta();
            Vector2 p1 = new Vector2(parentLocation.X + parentLocation.Width / 2, parentLocation.Y + parentLocation.Height / 2);
            Vector2 p2 = new Vector2((float)(maxViewDistance * Math.Cos(theta + viewAngle)) + p1.X,
                (float)(maxViewDistance * Math.Sin(theta + viewAngle)) + p1.Y);
         
            float closestDistance = maxViewDistance*maxViewDistance;
            foreach (Obstacle obs in obstacles)
            {
                Rectangle rec = obs.collisionRectangle;
                Vector2 v1 = new Vector2(rec.X, rec.Y);
                Vector2 v2 = new Vector2(rec.X + rec.Width, rec.Y);
                Vector2 v3 = new Vector2(rec.X + rec.Width, rec.Y + rec.Height);
                Vector2 v4 = new Vector2(rec.X, rec.Y + rec.Height);

                Vector2 top = Intersects(p1, p2, v1, v2);
                Vector2 right = Intersects(p1, p2, v2, v3);
                Vector2 bottom = Intersects(p1, p2, v3, v4);
                Vector2 left = Intersects(p1, p2, v4, v1);

                closestDistance = Math.Min(closestDistance, DistanceSquared(p1, top));
                closestDistance = Math.Min(closestDistance, DistanceSquared(p1, right));
                closestDistance = Math.Min(closestDistance, DistanceSquared(p1, bottom));
                closestDistance = Math.Min(closestDistance, DistanceSquared(p1, left));
            }
            viewDistance = (float) Math.Sqrt(closestDistance);
        }

        public bool IntersectsWithPlayer(Player player, List<Obstacle> obstacles)
        {
            double theta = GetTheta();
            Vector2 p1 = new Vector2(parentLocation.X + parentLocation.Width / 2, parentLocation.Y + parentLocation.Height / 2);
            Vector2 p2 = new Vector2((float)((viewDistance - 10) * Math.Cos(theta + viewAngle)) + p1.X,
                (float)((viewDistance-10) * Math.Sin(theta + viewAngle)) + p1.Y);

            Rectangle rec = player.collisionRectangle;
            Vector2 v1 = new Vector2(rec.X, rec.Y);
            Vector2 v2 = new Vector2(rec.X + rec.Width, rec.Y);
            Vector2 v3 = new Vector2(rec.X + rec.Width, rec.Y + rec.Height);
            Vector2 v4 = new Vector2(rec.X, rec.Y + rec.Height);

            Vector2 top = Intersects(p1, p2, v1, v2);
            Vector2 right = Intersects(p1, p2, v2, v3);
            Vector2 bottom = Intersects(p1, p2, v3, v4);
            Vector2 left = Intersects(p1, p2, v4, v1);

            if (top.X >= 0 || right.X >= 0 || bottom.X >= 0 || left.X >= 0)
            {
                return true;
            }
            return false;
        }

        public bool CanSeePlayer(Player player, List<Obstacle> visibleObstacles)
        {
            if (!player.isVisible || !CanSee(player.collisionRectangle)) return false;

            VertexPositionColor[] viewTriangle = GetFieldOfViewTriangle();

            double theta = GetTheta();
            double minFOVAngle = theta - viewAngle;
            double maxFOVAngle = theta + viewAngle;

            while (minFOVAngle < 0) minFOVAngle += MathHelper.TwoPi;
            while (minFOVAngle > MathHelper.TwoPi) minFOVAngle -= MathHelper.TwoPi;
            while (maxFOVAngle < 0) maxFOVAngle += MathHelper.TwoPi;
            while (maxFOVAngle > MathHelper.TwoPi) maxFOVAngle -= MathHelper.TwoPi;

            double topLeftAngle = Math.Tan((player.collisionRectangle.Y - viewTriangle[0].Position.Y) / (player.collisionRectangle.X - viewTriangle[0].Position.X));
            double topRightAngle = Math.Tan((player.collisionRectangle.Y - viewTriangle[0].Position.Y) / ((player.collisionRectangle.X + player.collisionRectangle.Width) - viewTriangle[0].Position.X));
            double bottomLeftAngle = Math.Tan(((player.collisionRectangle.Y + player.collisionRectangle.Height) - viewTriangle[0].Position.Y) / (player.collisionRectangle.X - viewTriangle[0].Position.X));
            double bottomRightAngle = Math.Tan(((player.collisionRectangle.Y + player.collisionRectangle.Height) - viewTriangle[0].Position.Y) / ((player.collisionRectangle.X + player.collisionRectangle.Width) - viewTriangle[0].Position.X));

            double minPlayerAngle = Math.Min(Math.Min(topLeftAngle, topRightAngle), Math.Min(bottomLeftAngle, bottomRightAngle));
            double maxPlayerAngle = Math.Max(Math.Max(topLeftAngle, topRightAngle), Math.Max(bottomLeftAngle, bottomRightAngle));

            if (minPlayerAngle < MathHelper.PiOver4 && maxPlayerAngle > MathHelper.TwoPi - MathHelper.PiOver4)
            {
                if (topLeftAngle > MathHelper.TwoPi - MathHelper.PiOver4) topLeftAngle -= MathHelper.TwoPi;
                if (topRightAngle > MathHelper.TwoPi - MathHelper.PiOver4) topRightAngle -= MathHelper.TwoPi;
                if (bottomLeftAngle > MathHelper.TwoPi - MathHelper.PiOver4) bottomLeftAngle -= MathHelper.TwoPi;
                if (bottomRightAngle > MathHelper.TwoPi - MathHelper.PiOver4) bottomRightAngle -= MathHelper.TwoPi;

                minPlayerAngle = Math.Min(Math.Min(topLeftAngle, topRightAngle), Math.Min(bottomLeftAngle, bottomRightAngle));
                maxPlayerAngle = Math.Max(Math.Max(topLeftAngle, topRightAngle), Math.Max(bottomLeftAngle, bottomRightAngle));
            }

            while (minPlayerAngle < 0) minPlayerAngle += MathHelper.TwoPi;
            while (minPlayerAngle > MathHelper.TwoPi) minPlayerAngle -= MathHelper.TwoPi;
            while (maxPlayerAngle < 0) maxPlayerAngle += MathHelper.TwoPi;
            while (maxPlayerAngle > MathHelper.TwoPi) maxPlayerAngle -= MathHelper.TwoPi;

            if (minPlayerAngle < minFOVAngle) minPlayerAngle = minFOVAngle;
            if (maxPlayerAngle > maxFOVAngle) maxPlayerAngle = maxFOVAngle;

            foreach (Obstacle obs in visibleObstacles)
            {
                topLeftAngle = Math.Tan((obs.worldRectangle.Y - viewTriangle[0].Position.Y) / (obs.worldRectangle.X - viewTriangle[0].Position.X));
                topRightAngle = Math.Tan((obs.worldRectangle.Y - viewTriangle[0].Position.Y) / ((obs.worldRectangle.X + obs.worldRectangle.Width) - viewTriangle[0].Position.X));
                bottomLeftAngle = Math.Tan(((obs.worldRectangle.Y + obs.worldRectangle.Height) - viewTriangle[0].Position.Y) / (obs.worldRectangle.X - viewTriangle[0].Position.X));
                bottomRightAngle = Math.Tan(((obs.worldRectangle.Y + obs.worldRectangle.Height) - viewTriangle[0].Position.Y) / ((obs.worldRectangle.X + obs.worldRectangle.Width) - viewTriangle[0].Position.X));

                double minObstacleAngle = Math.Min(Math.Min(topLeftAngle, topRightAngle), Math.Min(bottomLeftAngle, bottomRightAngle));
                double maxObstacleAngle = Math.Max(Math.Max(topLeftAngle, topRightAngle), Math.Max(bottomLeftAngle, bottomRightAngle));

                if (minObstacleAngle < MathHelper.PiOver4 && maxObstacleAngle > MathHelper.TwoPi - MathHelper.PiOver4)
                {
                    if (topLeftAngle > MathHelper.TwoPi - MathHelper.PiOver4) topLeftAngle -= MathHelper.TwoPi;
                    if (topRightAngle > MathHelper.TwoPi - MathHelper.PiOver4) topRightAngle -= MathHelper.TwoPi;
                    if (bottomLeftAngle > MathHelper.TwoPi - MathHelper.PiOver4) bottomLeftAngle -= MathHelper.TwoPi;
                    if (bottomRightAngle > MathHelper.TwoPi - MathHelper.PiOver4) bottomRightAngle -= MathHelper.TwoPi;

                    minObstacleAngle = Math.Min(Math.Min(topLeftAngle, topRightAngle), Math.Min(bottomLeftAngle, bottomRightAngle));
                    maxObstacleAngle = Math.Max(Math.Max(topLeftAngle, topRightAngle), Math.Max(bottomLeftAngle, bottomRightAngle));
                }

                while (minObstacleAngle < 0) minObstacleAngle += MathHelper.TwoPi;
                while (minObstacleAngle > MathHelper.TwoPi) minObstacleAngle -= MathHelper.TwoPi;
                while (maxObstacleAngle < 0) maxObstacleAngle += MathHelper.TwoPi;
                while (maxObstacleAngle > MathHelper.TwoPi) maxObstacleAngle -= MathHelper.TwoPi;

                if (minObstacleAngle < minFOVAngle) minObstacleAngle = minFOVAngle;
                if (maxObstacleAngle > maxFOVAngle) maxObstacleAngle = maxFOVAngle;

                //Check mindistance between player and obstacle; onward to false condition if obstacle is closer
                if (minObstacleAngle <= minPlayerAngle && maxObstacleAngle >= maxPlayerAngle)
                {
                    double topLeftPlayerDistance = Distance(new Vector2(player.collisionRectangle.X, player.collisionRectangle.Y), new Vector2(viewTriangle[0].Position.X, viewTriangle[0].Position.Y));
                    double topRightPlayerDistance = Distance(new Vector2(player.collisionRectangle.X + player.collisionRectangle.Width, player.collisionRectangle.Y), new Vector2(viewTriangle[0].Position.X, viewTriangle[0].Position.Y));
                    double bottomLeftPlayerDistance = Distance(new Vector2(player.collisionRectangle.X, player.collisionRectangle.Y + player.collisionRectangle.Height), new Vector2(viewTriangle[0].Position.X, viewTriangle[0].Position.Y));
                    double bottomRightPlayerDistance = Distance(new Vector2(player.collisionRectangle.X + player.collisionRectangle.Width, player.collisionRectangle.Y + player.collisionRectangle.Height), new Vector2(viewTriangle[0].Position.X, viewTriangle[0].Position.Y));
                    double minPlayerDistance = Math.Min(Math.Min(topLeftPlayerDistance, topRightPlayerDistance), Math.Min(bottomLeftPlayerDistance, bottomRightPlayerDistance));

                    double topLeftObstacleDistance = Distance(new Vector2(obs.worldRectangle.X, obs.worldRectangle.Y), new Vector2(viewTriangle[0].Position.X, viewTriangle[0].Position.Y));
                    double topRightObstacleDistance = Distance(new Vector2(obs.worldRectangle.X + obs.worldRectangle.Width, obs.worldRectangle.Y), new Vector2(viewTriangle[0].Position.X, viewTriangle[0].Position.Y));
                    double bottomLeftObstacleDistance = Distance(new Vector2(obs.worldRectangle.X, obs.worldRectangle.Y + obs.worldRectangle.Height), new Vector2(viewTriangle[0].Position.X, viewTriangle[0].Position.Y));
                    double bottomRightObstacleDistance = Distance(new Vector2(obs.worldRectangle.X + obs.worldRectangle.Width, obs.worldRectangle.Y + obs.worldRectangle.Height), new Vector2(viewTriangle[0].Position.X, viewTriangle[0].Position.Y));
                    double minObstacleDistance = Math.Min(Math.Min(topLeftObstacleDistance, topRightObstacleDistance), Math.Min(bottomLeftObstacleDistance, bottomRightObstacleDistance));

                    if (minPlayerDistance > minObstacleDistance) return false;
                }
            }

            return true;
        }

        public bool CanSee(Rectangle being)
        {
            VertexPositionColor[] viewTriangle = GetFieldOfViewTriangle();

            for (int i = 0; i < 3; i++)
            {
                if (viewTriangle[i].Position.X > being.X && viewTriangle[i].Position.X < (being.X + being.Width) && viewTriangle[i].Position.Y > being.Y && viewTriangle[i].Position.Y < (being.Y + being.Height))
                {
                    return true;
                }
            }

            if (IsPointInViewTriangle(being.Location) ||
                IsPointInViewTriangle(new Point(being.X, being.Y + being.Height)) ||
                IsPointInViewTriangle(new Point(being.X + being.Width, being.Y)) ||
                IsPointInViewTriangle(new Point(being.X + being.Width, being.Y + being.Height)))
            {
                return true;
            }

            Vector2 t0 = new Vector2(viewTriangle[0].Position.X, viewTriangle[0].Position.Y);
            Vector2 t1 = new Vector2(viewTriangle[1].Position.X, viewTriangle[1].Position.Y);
            Vector2 t2 = new Vector2(viewTriangle[2].Position.X, viewTriangle[2].Position.Y);

            Vector2 s0 = new Vector2(being.X, being.Y);
            Vector2 s1 = new Vector2(being.X, being.Y + being.Height);
            Vector2 s2 = new Vector2(being.X + being.Width, being.Y);
            Vector2 s3 = new Vector2(being.X + being.Width, being.Y + being.Height);

            if (Intersects(t0, t1, s0, s1).X > 0 || Intersects(t0, t1, s0, s2).X > 0 || Intersects(t0, t1, s1, s3).X > 0 || Intersects(t0, t1, s2, s3).X > 0 ||
                Intersects(t1, t2, s0, s1).X > 0 || Intersects(t1, t2, s0, s2).X > 0 || Intersects(t1, t2, s1, s3).X > 0 || Intersects(t1, t2, s2, s3).X > 0 ||
                Intersects(t0, t2, s0, s1).X > 0 || Intersects(t0, t2, s0, s2).X > 0 || Intersects(t0, t2, s1, s3).X > 0 || Intersects(t0, t2, s2, s3).X > 0) return true;

            return false;
        }

        private bool IsPointInViewTriangle(Point p)
        {
            VertexPositionColor[] viewTriangle = GetFieldOfViewTriangle();

            float px = p.X;
            float py = p.Y;
            float t1x = viewTriangle[0].Position.X;
            float t1y = viewTriangle[0].Position.Y;
            float t2x = viewTriangle[1].Position.X;
            float t2y = viewTriangle[1].Position.Y;
            float t3x = viewTriangle[2].Position.X;
            float t3y = viewTriangle[2].Position.Y;

            //barycentric coordinates
            float lambda1 = (((t2y - t3y) * (px - t3x)) + ((t3x - t2x) * (py - t3y))) / (((t2y - t3y) * (t1x - t3x)) + ((t3x - t2x) * (t1y - t3y)));
            float lambda2 = (((t3y - t1y) * (px - t3x)) + ((t1x - t3x) * (py - t3y))) / (((t2y - t3y) * (t1x - t3x)) + ((t3x - t2x) * (t1y - t3y)));
            float lambda3 = 1 - lambda1 - lambda2;

            //If all are greater than 0 and less than 1, then the point is in the triangle
            if (lambda1 > 0 && lambda1 < 1 &&
                lambda2 > 0 && lambda2 < 1 &&
                lambda3 > 0 && lambda3 < 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private double GetTheta()
        {
            //Vector2 unitV = Normalize(viewDirection); //not necessary if we know it's normalized every time it's set

            float x = viewDirection.X;
            float y = viewDirection.Y;

            if (x == 0 || y == 0)
            {
                if (x == 0 && y == 0)
                {
                    return -1; //Invalid flag, as this function should only return values within [0,2pi)
                }
                else if (x == 0)
                {
                    if (y > 0) return MathHelper.PiOver2;
                    if (y < 0) return 3 * MathHelper.PiOver2;
                }
                else if (y == 0)
                {
                    if (x > 0) return 0;
                    if (x < 0) return MathHelper.Pi;
                }
            }
            else if (y > 0)
            {
                return Math.Acos(x);
            }
            else if (y < 0)
            {
                return MathHelper.TwoPi - Math.Acos(x);
            }

            return -1; //Invalid flag, as this function should only return values within [0,2pi)
        }

        private Vector2 Normalize(Vector2 v)
        {
            float distance = Distance(v, new Vector2(0, 0));
            return new Vector2(v.X / distance, v.Y / distance);
        }

        private float Distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }


        private double BoundAngle(double angle)
        {
            while (angle < 0) angle += MathHelper.TwoPi;
            while (angle >= MathHelper.TwoPi) angle -= MathHelper.TwoPi;
            return angle;
        }

        public override string ToString()
        {
            string retVal = base.ToString() +
                "Distance: " + this.viewDistance + "\n" +
                "Angle: " + this.viewAngle + "\n" +
                "Direction: X - " + this.viewDirection.X + " Y - " + this.viewDirection.Y + "\n";
            return retVal;
        }
    }
}