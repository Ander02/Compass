using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Compass
{
    class RelationPosition
    {
        public double LatDiff { get; set; }
        public double LongDiff { get; set; }
        public double InclinationX { get; set; } = 0;
        public double InclinationY { get; set; } = 0;
        public double Distance { get; set; }

        public RelationPosition(double lat1, double long1, double lat2, double long2)
        {
            double factor = 111;
            this.LatDiff = Math.Abs(lat1 - lat2) * factor;
            this.LongDiff = Math.Abs(long1 - long2) * factor;
            this.Distance = Math.Sqrt((LatDiff * LatDiff) + (LongDiff * LongDiff));
            this.InclinationX = this.RadianToDegree(Math.Atan(LatDiff / LongDiff));
            this.InclinationY = this.RadianToDegree(Math.Atan(LongDiff / LatDiff));
            var a = this.InclinationX + InclinationY;
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180;
        }

        private double RadianToDegree(double radian)
        {
            return radian * 180 / Math.PI;
        }

        public override string ToString()
        {
            return "LatDiff: " + this.LatDiff + "\nLongDiff: " + this.LongDiff + "\nInclinationX: " + this.InclinationX + "\nInclinationY: " + this.InclinationY + "\nDistance: " + this.Distance;
        }
    }
}