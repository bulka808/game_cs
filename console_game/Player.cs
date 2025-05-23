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
        List<Point> points = new();
        // points.Add(Pos + new Point(-1,  0));
        // points.Add(Pos + new Point(-1,  1));
        // points.Add(Pos + new Point( 0,  1));
        // points.Add(Pos + new Point( 1,  1));
        // points.Add(Pos + new Point( 1,  0));
        // points.Add(Pos + new Point( 1,  1));
        // points.Add(Pos + new Point( 0, -1));
        // points.Add(Pos + new Point(-1, -1));
        for (int i = -2; i <= 2; i++)
        for (int j = -2; j <= 2; j++)
        {
            points.Add(Pos + new Point(i, j));
        }

        return points;
    }
}