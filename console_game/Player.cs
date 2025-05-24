using ConsoleGameEngine;
namespace console_game;


public class Player
{
    public int Health;
    public Point Pos = new Point(0, 0);
    public long AttackCd = 100;
    public long atk;
    
    public Player(int health)
    {
        this.Health = health;
        this.atk = 0;
    }
    public void Move(int ch)
    {
        switch (ch)
        {
            case 1:
                Pos -= new Point(0, 1);
                break;
            case 2:
                Pos += new Point(0, 1);
                break;
            case 3:
                Pos += new Point(1, 0);
                break;
            case 4:
                Pos -= new Point(1, 0);
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