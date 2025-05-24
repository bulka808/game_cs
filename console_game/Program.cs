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
        private Player _player = new(10);
        public List<Point> PlayerAttack  = [];
        public List<Enemy> Enemies = [];
        readonly Point _offset = new Point(1, 2);

        public void CreateEnemy(Point pos) { Enemies.Add(new Enemy(pos)); }
        public void CreateEnemy()
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
           Enemies.Add(new Enemy(pos));
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
            if ((Engine.GetKeyDown(ConsoleKey.UpArrow   ) | Engine.GetKeyDown(ConsoleKey.W)) && _player.Pos.Y != 0)          { _player.Move(1); }
            if ((Engine.GetKeyDown(ConsoleKey.DownArrow ) | Engine.GetKeyDown(ConsoleKey.S)) && _player.Pos.Y != Height - 4) { _player.Move(2); }
            if ((Engine.GetKeyDown(ConsoleKey.RightArrow) | Engine.GetKeyDown(ConsoleKey.D)) && _player.Pos.X != Width  - 3) { _player.Move(3); }
            if ((Engine.GetKeyDown(ConsoleKey.LeftArrow ) | Engine.GetKeyDown(ConsoleKey.A)) && _player.Pos.X != 0)          { _player.Move(4); }
        
            if (Engine.GetMouseLeft() && DateTimeOffset.Now.ToUnixTimeMilliseconds() >= _player.atk+_player.AttackCd)
            {
                var points = _player.Attack();
                points.ForEach(p => PlayerAttack.Add(p));
            
                Enemies.RemoveAll(enemy => {
                    return PlayerAttack.Any((p) =>  p.X == enemy.Pos.X && p.Y == enemy.Pos.Y );
                });
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

            if (this.FrameCounter % 5 == 0)
            {
                Enemies.ForEach(enemy => enemy.MoveOrAttack(_player, _offset));
            }


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

            Enemies.ForEach(enemy => { 
                try { Engine.SetPixel(enemy.Pos+_offset, (int)ConsoleColor.DarkRed, (ConsoleCharacter)'@'); }
                catch (Exception ) { /* ignored */ } 
            });
            PlayerAttack.ForEach((point) => {
                try { Engine.SetPixel(point+_offset, (int)ConsoleColor.DarkCyan, ConsoleCharacter.Medium); }
                catch (Exception) { /* ignored */ }
            });
            PlayerAttack.Clear();


            Engine.WriteText(new Point(0,0), $"{Math.Round(this.GetFramerate())}",2 );
            Engine.WriteText(new Point(3,0), $"{Engine.GetMousePos()}",2 );
            Engine.WriteText(new Point(12,0), $"{_player.Pos}",2 );
            Engine.WriteText(new Point(21,0), $"{_player.Health}",2 );
            
            Engine.SetPixel(_player.Pos + _offset, _map[_player.Pos.Y, _player.Pos.X] == 4 ? (int)ConsoleColor.Cyan: (int)ConsoleColor.DarkGray, (ConsoleCharacter)'@');
            
            Engine.DisplayBuffer();
        }
    }
}
