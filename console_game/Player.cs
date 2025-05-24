using ConsoleGameEngine;
namespace console_game;


public class Player
{
    public enum Direction { Up = 1, Down, Left, Right }
    
    public int Health;
    public Point Pos = new Point(0, 0);
    public long AttackCd = 100;
    public long atk;
    
    public Player(int health)
    {
        this.Health = health;
        this.atk = 0;
    }
    public void Move(Direction ch)
    {
        switch (ch)
        {
            case Direction.Up:
                Pos -= new Point(0, 1);
                break;
            case Direction.Down:
                Pos += new Point(0, 1);
                break;
            case Direction.Left:
                Pos -= new Point(1, 0);
                break;
            case Direction.Right:
                Pos += new Point(1, 0);
                break;
        }
    }


    public List<Point> Attack()
    {
        atk = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        List<Point> points = [];
        // points.Add(Pos + new Point(-1,  0));
        // points.Add(Pos + new Point(-1,  1));
        // points.Add(Pos + new Point( 0,  1));
        // points.Add(Pos + new Point( 1,  1));
        // points.Add(Pos + new Point( 1,  0));
        // points.Add(Pos + new Point( 1,  1));
        // points.Add(Pos + new Point( 0, -1));
        // points.Add(Pos + new Point(-1, -1));
        for (var i = -3; i <= 3; i++)
        for (var j = -3; j <= 3; j++)
        {
            if ((Math.Abs(i) == 3 && Math.Abs(j) == 3) || (Math.Abs(i) == 2 && Math.Abs(j) == 3) ||
                (Math.Abs(i) == 3 && Math.Abs(j) == 2))
            {
                continue;
            }

            points.Add(Pos + new Point(i, j));
            
        }

        return points;
    }
}