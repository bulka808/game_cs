using ConsoleGameEngine;




namespace console_game
{
    internal class Program: ConsoleGame
    {
        private const int Width = 30; // на самом деле -3
        private const int Height = Width+1; // на самом деле -4
        // 1 строка под вывод фпс и т.д.
        private static void Main( ) {
            new Program().Construct(Width, Height, 16, 16, FramerateMode.MaxFps);
        }

        private int[,]? _map; //TODO класс Map для карты
        private readonly Player _player = new(10);
        private readonly List<Enemy> _enemies = [];
        private readonly List<Bullet> _bullets = [];
        private readonly Point _offset = new Point(1, 2);

        private void CreateEnemy(Point pos) { _enemies.Add(new Enemy(pos)); }

        private void CreateEnemy()
        {
           var rand = new Random();
           var side =  rand.Next(4);
           int x = 0, y = 0;
           switch (side)
           {
               case 0:
                   y = -2;
                   x = rand.Next(-2, Width+2);
                   break;
               case 1:
                   y =  Height+2;
                   x = rand.Next(-2, Width+2);
                   break;
               case 2:
                   x = -2;
                   y = rand.Next(-2, Height+2);
                   break;
               case 3: 
                   x = Height+2;
                   y = rand.Next(-2, Height+2);
                   break;
           }
           var pos = new Point(x, y);
           _enemies.Add(new Enemy(pos));
        }

        public override void Create() {
            Console.Title = "Console Game";
            Engine.SetPalette(Palettes.Default);
            //Engine.Borderless();
            // this.DeltaTime = 1 / 30;
            //14,14
            //Width-2 для нормальной рамки
            var l = Width - 2;
            _map = new int [l, l];
            for (int y = 0; y < l; y++)
            for (int x = 0; x < l; x++)
            {
                _map[y, x] = 0;
            }
            //крестики в углах
            _map[0, 0] = 4;
            _map[l - 1, 0] = 4;
            _map[l - 1, l - 1] = 4;
            _map[0, l - 1] = 4;
        }

        public override void Update() {
            //-4, -3 для того чтобы не выходил за край
            //вверх
            if ((Engine.GetKeyDown(ConsoleKey.UpArrow   ) | Engine.GetKeyDown(ConsoleKey.W)) && _player.Pos.Y != 0) 
            { _player.Move(Player.Direction.Up); }
            //вниз
            if ((Engine.GetKeyDown(ConsoleKey.DownArrow ) | Engine.GetKeyDown(ConsoleKey.S)) && _player.Pos.Y != Height - 4) 
            { _player.Move(Player.Direction.Down); }
            //влево
            if ((Engine.GetKeyDown(ConsoleKey.LeftArrow ) | Engine.GetKeyDown(ConsoleKey.A)) && _player.Pos.X != 0)          
            { _player.Move(Player.Direction.Left); }
            //вправо
            if ((Engine.GetKeyDown(ConsoleKey.RightArrow) | Engine.GetKeyDown(ConsoleKey.D)) && _player.Pos.X != Width  - 3) 
            { _player.Move(Player.Direction.Right); }

        
            if (Engine.GetMouseLeft() && DateTimeOffset.Now.ToUnixTimeMilliseconds() >= _player.Atk+_player.AttackCd)
            {   /*TODO убрать костыль*////////////////////////////////////
                _player.Atk = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                //////////////////////////////////////////////////////////
                var pos = Engine.GetMousePos();
                _bullets.Add(new Bullet(_player.Pos, pos-_offset ));
            }
            
            if (Engine.GetKeyDown(ConsoleKey.Spacebar))
            {   
                var pos = Engine.GetMousePos();
                try { _map![pos.Y-2, pos.X-1] = 4; }
                catch (Exception) { /* ignored */ }
            }

            if (Engine.GetKeyDown(ConsoleKey.E))
            {
                var pos = Engine.GetMousePos()-_offset;
                try { CreateEnemy(pos);}
                catch (Exception) { /* ignored */ }
            }
            if (Engine.GetKeyDown(ConsoleKey.R))
            {
                try { CreateEnemy();}
                catch (Exception) { /* ignored */ }
            }
            _enemies.RemoveAll(enemy => _bullets.Any(bullet => bullet.IsActive && bullet.Points[0].X == enemy.Pos.X && bullet.Points[0].Y == enemy.Pos.Y));
            if (this.FrameCounter % 5 == 0) { _enemies.ForEach(enemy => enemy.MoveOrAttack(_player, _offset)); }
            
            for (var i = _bullets.Count - 1; i >= 0; i--) { if (!_bullets[i].IsActive) { _bullets.RemoveAt(i); } }
        }

        public override void Render() {
            Engine.ClearBuffer();
            Engine.Frame(new Point(0, 1), new Point(Width-1, Height-1), 7);

            for (var y = 0; y < _map!.GetLength(0); y++)
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

            _enemies.ForEach(enemy => { 
                try { Engine.SetPixel(enemy.Pos+_offset, (int)ConsoleColor.DarkRed, (ConsoleCharacter)'@'); }
                catch (Exception ) { /* ignored */ } 
            });
            
            _bullets.ForEach(bullet =>
            {
                try
                {
                    Engine.SetPixel(bullet.Points[0]+_offset, (int)ConsoleColor.Magenta, (ConsoleCharacter)'*');
                    bullet.Points.RemoveAt(0);
                    if (bullet.Points.Count <= 0) { bullet.IsActive = false; }
                }
                catch (Exception ) { /* ignored */ }
            });


            Engine.WriteText(new Point(0,0), $"{Math.Round(this.GetFramerate())}",2 );
            Engine.WriteText(new Point(3,0), $"{Engine.GetMousePos()}",2 );
            Engine.WriteText(new Point(12,0), $"{_player.Pos}",2 );
            Engine.WriteText(new Point(21,0), $"{_player.Health}",2 );
            
            Engine.SetPixel(_player.Pos + _offset, _map[_player.Pos.Y, _player.Pos.X] == 4 ? (int)ConsoleColor.Cyan: (int)ConsoleColor.DarkGray, (ConsoleCharacter)'@');
            
            Engine.DisplayBuffer();
        }
    }
}
