using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hide_Out.Primatives
{
    class Vision
    {
        private static double maxViewAngle = MathHelper.PiOver4;

        public float viewDistance { get; set; }

        public double viewAngle
        {
            get
            {
                return this.viewAngle;
            }
            set
            {
                if (value < 0) 
                    this.viewAngle = 0;
                else if (value > maxViewAngle) 
                    this.viewAngle = maxViewAngle;
                else 
                    this.viewAngle = value;
            }
        }

        public Rectangle parentLocation { get; set; }

        public Vector2 viewDirection
        {
            get
            {
                return this.viewDirection;
            }
            set
            {
                this.viewDirection = Normalize(value);
            }
        }
        public Color viewColor { get; set; }

        public Vision(Rectangle pLoc, float vDis, double vAng, Vector2 vDir, Color vCol)
        {
            parentLocation = pLoc;
            viewDistance = vDis;
            viewAngle = vAng;
            viewDirection = vDir;
            viewColor = vCol;
        }

        public VertexPositionColor[] getFieldOfViewTriangle()
        {
            VertexPositionColor[] fieldOfVision = new VertexPositionColor[3];

            fieldOfVision[0].Color = viewColor;
            fieldOfVision[1].Color = viewColor;
            fieldOfVision[2].Color = viewColor;

            //Index 0 is located at the center of the rectangle
            fieldOfVision[0].Position = new Vector3(parentLocation.X + parentLocation.Width / 2, parentLocation.Y + parentLocation.Height / 2, 0);

            //Indices 1 and 2 indicate the farthest points of view on the left and right sides, respectively, of the Searcher
            double theta = getTheta();
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
            double theta = getTheta();
            viewDirection = new Vector2((float)Math.Cos(theta + angle), (float)Math.Sin(theta + angle));
        }

        public bool CanSee(Rectangle being)
        {
            VertexPositionColor[] viewTriangle = getFieldOfViewTriangle();

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

            return false;
        }

        private bool IsPointInViewTriangle(Point p)
        {
            VertexPositionColor[] viewTriangle = getFieldOfViewTriangle();

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

        private double getTheta()
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