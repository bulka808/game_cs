using System.Numerics;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Threading;

using ConsoleGameEngine;
namespace console_game;



internal class Program: ConsoleGame
{
    private static void Main(string[] args) {
        new Program().Construct(16, 17, 16, 16, FramerateMode.MaxFps);
        Console.Title = "Console Game";
    }

    int[,] map;
    Player player = new Player(0);
    public Point[] player_atck;
    public List<Enemy> enemies = new List<Enemy>();
    public override void Create() {
        Engine.SetPalette(Palettes.Default);
        //Engine.Borderless();
        this.DeltaTime = 1 / 30;
        map = new int[14, 14]
        {
            { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
        };



    }

    public override void Update() {
        
        if(Engine.GetKeyDown(ConsoleKey.UpArrow) && player.Pos.Y != 0) {
            player.Pos -= new Point(0, 1);
        }

        if (Engine.GetKeyDown(ConsoleKey.DownArrow) && player.Pos.Y != map.GetLength(0) - 1)
        {
            player.Pos += new Point(0, 1);
        }

        if (Engine.GetKeyDown(ConsoleKey.RightArrow)  && player.Pos.X != map.GetLength(1) - 1)
        {
            player.Pos += new Point(1, 0);
        }

        if (Engine.GetKeyDown(ConsoleKey.LeftArrow) && player.Pos.X != 0)
        {
            player.Pos -= new Point(1, 0);
        }

        if (Engine.GetMouseLeft() && DateTimeOffset.Now.ToUnixTimeMilliseconds() >= player.atk+500)
        {
            player_atck = player.Attack();
        }

        if (Engine.GetKeyDown(ConsoleKey.Spacebar))
        {   
            var pos = Engine.GetMousePos();
            try
            {
                map[pos.Y-2, pos.X-1] = 4;
            }
            catch (Exception e) { }
        }
        if (Engine.GetKeyDown(ConsoleKey.E))
        {   
            var pos = Engine.GetMousePos();
            try
            {
                

                enemies.Add(new Enemy(pos));
            }
            catch (Exception e) { }
        }

    }

    public override void Render() {
        Engine.ClearBuffer();
        
        Engine.Frame(new Point(0, 1), new Point(15, 16), 7);
        
        Point offset = new Point(1, 2);
        
        for (int y = 0; y < map.GetLength(0); y++) {
            for (int x = 0; x < map.GetLength(1); x++) {
                Point p = new Point(x, y) + offset;

                switch (map[y, x]) {
                    case 1:
                        Engine.SetPixel(p, 7, ConsoleCharacter.Full);
                        break;
                    case 3:
                        Engine.SetPixel(p, 9, ConsoleCharacter.Full);
                        break;
                    case 4:
                        Engine.SetPixel(p, 8, (ConsoleCharacter)'x');
                        break;
                }
            }
        }

        if (player_atck != null)
        {   
            foreach (var point in player_atck)
            {
                try
                {
                    Engine.SetPixel(point + offset, 3, ConsoleCharacter.Medium);
                    if (map[point.Y, point.X] == 4) { map[point.Y, point.X] = 1; }
                    else if (map[point.Y, point.X] == 1) { map[point.Y, point.X] = 4; }
                }
                catch (Exception e) { }
            }
            player_atck = null;
        }

        foreach (var enemy in enemies)
        {
            try
            {
                Engine.SetPixel(enemy.pos, 4, (ConsoleCharacter)'@');
            }
            catch (Exception e) { }
        }
        
        Engine.WriteText(new Point(0,0), $"{Math.Round(this.GetFramerate())}",2 );
        Engine.WriteText(new Point(5,0), $"{Engine.GetMousePos()}",2 );
        Engine.SetPixel(player.Pos + offset, map[player.Pos.Y, player.Pos.X] == 4 ? 11: 8, (ConsoleCharacter)'@');
        
        Engine.DisplayBuffer();
    }
}

