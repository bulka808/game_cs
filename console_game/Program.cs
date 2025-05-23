using ConsoleGameEngine;
namespace console_game;

internal class Program: ConsoleGame
{
    private static void Main(string[] args) {
        new Program().Construct(16, 17, 16, 16, FramerateMode.MaxFps);
    }

    private int[,]? _map; //TODO класс Map для карты
    private Player _player = new(0);
    public List<Point> PlayerAtck  = new();
    public List<Enemy> Enemies = new();
    readonly Point _offset = new Point(1, 2);
    public override void Create() {
        Console.Title = "Console Game";
        Engine.SetPalette(Palettes.Default);
        //Engine.Borderless();
        // this.DeltaTime = 1 / 30;
        //14,14
        _map = new[,]
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
        
        if (Engine.GetKeyDown(ConsoleKey.UpArrow   )  && _player.Pos.Y != 0)                               { _player.Move(1); }
        if (Engine.GetKeyDown(ConsoleKey.DownArrow )  && _player.Pos.Y != _map.GetLength(0) - 1) { _player.Move(2); }
        if (Engine.GetKeyDown(ConsoleKey.RightArrow)  && _player.Pos.X != _map.GetLength(1) - 1) { _player.Move(3); }
        if (Engine.GetKeyDown(ConsoleKey.LeftArrow )  && _player.Pos.X != 0)                               { _player.Move(4); }
        
        if (Engine.GetMouseLeft() && DateTimeOffset.Now.ToUnixTimeMilliseconds() >= _player.atk+500)
        {
            var points = _player.Attack();
            points.ForEach(p => PlayerAtck.Add(p));
        }

        if (Engine.GetKeyDown(ConsoleKey.Spacebar))
        {   
            var pos = Engine.GetMousePos();
            try { _map[pos.Y-2, pos.X-1] = 4; }
            catch (Exception e) { }
        }

        if (Engine.GetKeyDown(ConsoleKey.E))
        {
            var pos = Engine.GetMousePos();
            try { Enemies.Add(new Enemy(pos)); }
            catch (Exception e) { }
        }
        
        Enemies.ForEach(Enemy => Enemy.Attack(_player, _offset));
    }

    public override void Render() {
        Engine.ClearBuffer();
        Engine.Frame(new Point(0, 1), new Point(15, 16), 7);

        for (var y = 0; y < _map.GetLength(0); y++)
        for (var x = 0; x < _map.GetLength(1); x++)
        {
            var p = new Point(x, y) + _offset;

            switch (_map[y, x])
            {
                case 1:
                    Engine.SetPixel(p, 7);
                    break;
                case 3:
                    Engine.SetPixel(p, 9);
                    break;
                case 4:
                    Engine.SetPixel(p, 8, (ConsoleCharacter)'x');
                    break;
            }
        }

        Enemies.ForEach(enemy => { 
            try { Engine.SetPixel(enemy.Pos, (int)ConsoleColor.DarkRed, (ConsoleCharacter)'@'); }
            catch (Exception e) { } 
        });
        PlayerAtck.ForEach((point) => {
            try { Engine.SetPixel(point+_offset, (int)ConsoleColor.DarkCyan, ConsoleCharacter.Medium); }
            catch (Exception e) { }
        });
        Enemies.RemoveAll(enemy => {
            return PlayerAtck.Any((p) =>  p.X == (enemy.Pos-_offset).X && p.Y == (enemy.Pos-_offset).Y );
        });
        PlayerAtck.Clear();


        Engine.WriteText(new Point(0,0), $"{Math.Round(this.GetFramerate())}",2 );
        Engine.WriteText(new Point(3,0), $"{Engine.GetMousePos()}",2 );
        Engine.WriteText(new Point(12,0), $"{_player.Health}",2 );
        Engine.SetPixel(_player.Pos + _offset, _map[_player.Pos.Y, _player.Pos.X] == 4 ? 11: 8, (ConsoleCharacter)'@');
        
        Engine.DisplayBuffer();
    }
}
