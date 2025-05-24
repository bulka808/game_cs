using ConsoleGameEngine;
namespace console_game;

public class Bullet(Point startPoint, Point endPoint)
{
    public readonly List<Point> Points = GenerateTrajectory(startPoint, endPoint);
    public bool IsActive = true;

    private static List<Point> GenerateTrajectory(Point sp, Point ep)
    {
        List<Point> points = [];
        var x0 = sp.X; var y0 = sp.Y;
        var x1 = ep.X; var y1 = ep.Y;
        
        var sx = x0 < x1 ? 1 : -1;
        var sy = y0 < y1 ? 1 : -1;
        
        var dx = Math.Abs(x1 - x0);
        var dy = Math.Abs(y1 - y0);
        
        var err = dx - dy;

        while (true)
        {
            points.Add(new Point(x0, y0));
            if (x0 == x1 && y0 == y1) { break; }
            var e2 = 2 * err;
            
            if (e2 > -dy) { err -= dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }
        
        return points;
    }
}