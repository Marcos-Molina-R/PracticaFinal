using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaFinalConTablero
{
    
    public partial class Form1 : Form
    {
        public Form1(List<Tuple<int, int>> coordenadasReinas)
        {
            if (coordenadasReinas.Count == 8)
            {
                InitializeComponent();
                Form_Load(coordenadasReinas);
            }
            else
            {
                Console.WriteLine("Fallo en las coordenadas: no se pinta el tablero");
            }
        }

        // class member array of Panels to track chessboard tiles
        // initialize the "chess board"
        private Panel[,] _chessBoardPanels = new Panel[GridSize, GridSize];
        
        // Constantes de tablero
        private const int TileSize = 40;
        private const int GridSize = 8;

        public Panel[,] ChessBoardPanels
        {
            get => _chessBoardPanels;
            set => _chessBoardPanels = value;
        }

        // event handler of Form Load... init things here
        private void Form_Load(List<Tuple<int, int>> coordenadasReinas)
        {
            var clr1 = Color.DarkGray;
            var clr2 = Color.White;


            // double for loop to handle all rows and columns
            var flag = false;
            for (var n = 0; n < GridSize; n++)
            {
                for (var m = 0; m < GridSize; m++)
                {
                    // create new Panel control which will be one 
                    // chess board tile
                    var agregar = new Control();
                    foreach (var (item1, item2) in coordenadasReinas)
                    {
                        if (n != item1 || m != item2) continue;
                        flag = true;
                        agregar = new PictureBox
                        {
                            Size = new Size(TileSize, TileSize),
                            Location = new Point(TileSize * n, TileSize * m),
                            Image = Properties.Resources.queen,
                            SizeMode = PictureBoxSizeMode.StretchImage
                        };
                        continue;
                    }
                    if (!flag)
                    {
                        agregar = new Panel
                        {
                            Size = new Size(TileSize, TileSize),
                            Location = new Point(TileSize * n, TileSize * m)
                        };
                    }
                    else
                    {
                        flag = false;
                    }

                    // add to Form's Controls so that they show up
                    Controls.Add(agregar);

                    // add to our 2d array of panels for future use
                    // _chessBoardPanels[n, m] = agregar;

                    // color the backgrounds
                    if (n % 2 == 0)
                        agregar.BackColor = m % 2 != 0 ? clr1 : clr2; 
                    else
                        agregar.BackColor = m % 2 != 0 ? clr2 : clr1;
                }
            }
        }
    }
}