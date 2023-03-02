﻿
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GestureAnalyzer
{
    public class PointPattern : IPointPattern
    {
        public PointPattern(Point[][] points)
        {
            Points = points;
        }

        public PointPattern(IEnumerable<List<Point>> points)
        {
            Points = points.Select(l => l.ToArray()).ToArray();
        }

        public Point[][] Points { get; set; }
    }
}
