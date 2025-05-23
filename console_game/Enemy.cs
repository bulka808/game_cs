using ConsoleGameEngine;
namespace console_game;

public class Enemy(Point pos)
{
    public Point Pos = pos;

    public void Attack(Player player, Point offset)
    {   
        var playerPos = player.Pos + offset;
        int dist = Convert.ToInt32(Math.Ceiling(Point.Distance(player.Pos, Pos)));
        if (dist > 1)
        {
            if (Pos.X > playerPos.X) { Pos.X -= 1; }
            else if (Pos.X < playerPos.X) { Pos.X += 1; }
            if (Pos.Y > playerPos.Y) { Pos.Y -= 1; }
            else if (Pos.Y < playerPos.Y) { Pos.Y += 1; }
        }
        else if (dist <= 1) { player.Health -= 1; }
    }
}