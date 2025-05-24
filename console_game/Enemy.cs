using ConsoleGameEngine;
namespace console_game;

public class Enemy(Point pos)
{
    public Point Pos = pos;

    public void MoveOrAttack(Player player, Point offset)
    {   
       
        var dist = Math.Floor(Point.Distance(player.Pos, Pos));
        if (Math.Abs(Pos.X - player.Pos.X) == 1 && Math.Abs(Pos.Y - player.Pos.Y) == 1)
        {
            player.Health -= 1; 
        }
        else if (dist > 1)
        {
            if (Pos.X > player.Pos.X) { Pos.X -= 1; }
            else if (Pos.X < player.Pos.X) { Pos.X += 1; }
            if (Pos.Y < player.Pos.Y) { Pos.Y += 1; }
            else if (Pos.Y > player.Pos.Y) { Pos.Y -= 1; }
        }
        else { player.Health -= 1; }
    }
}