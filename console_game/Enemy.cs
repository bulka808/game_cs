using ConsoleGameEngine;
namespace console_game;

public class Enemy(Point pos)
{
    public Point Pos = pos;

    public void MoveOrAttack(Player player, Point offset)
    {   
        var playerPos = player.Pos+offset;
        var dist = Math.Floor(Point.Distance(playerPos, Pos));
        if (Math.Abs(Pos.X - playerPos.X) == 1 && Math.Abs(Pos.Y - playerPos.Y) == 1)
        {
            if (playerPos.X < Pos.X) { Pos.X--; }
            else if (playerPos.X > Pos.X) { Pos.X++; }
            else if (playerPos.Y < Pos.Y) { Pos.Y--; }
            else if (playerPos.Y > Pos.Y) { Pos.Y++; }
        }
        else if (dist > 1)
        {
            if (Pos.X > playerPos.X) { Pos.X -= 1; }
            else if (Pos.X < playerPos.X) { Pos.X += 1; }
            if (Pos.Y < playerPos.Y) { Pos.Y += 1; }
            else if (Pos.Y > playerPos.Y) { Pos.Y -= 1; }
        }
        else { player.Health -= 1; }
    }
}