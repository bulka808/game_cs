using ConsoleGameEngine;
namespace console_game;


public class Player
{
    public int Health;
    public Point Pos = new Point(0, 0);
    public long atk = DateTimeOffset.Now.ToUnixTimeMilliseconds()-500;
    
    public Player(int health)
    {
        this.Health = health;
    }

    public Point[] Attack()
    {
        atk = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        var points = new Point[8];
        points[0] = Pos + new Point(-1,  0);
        points[1] = Pos + new Point(-1,  1);
        points[2] = Pos + new Point( 0,  1);
        points[3] = Pos + new Point( 1,  1);
        points[4] = Pos + new Point( 1,  0);
        points[5] = Pos + new Point( 1, -1);
        points[6] = Pos + new Point( 0, -1);
        points[7] = Pos + new Point(-1, -1);
        return points;
    }
}