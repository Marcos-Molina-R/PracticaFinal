using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaFinalConTablero
{
    static class Program
    {
        [DllImport( "kernel32.dll" )]
        static extern bool AttachConsole( int dwProcessId );
        private const int ATTACH_PARENT_PROCESS = -1;
        
        //
        // Esta variable global nos va a delimitar cuantas reinas se van a colocar de manera aleatoria
        static int NumeroPosicionesAleatorias = 2; // 2 reinas aleatorias es la opcion óptima
        //
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // redirect console output to parent process;
            // must be before any calls to Console.WriteLine()
            AttachConsole( ATTACH_PARENT_PROCESS );
            Console.WriteLine("prueba");
            
            //
            List<Tuple<int, int>> CoordenadasReinas = new List<Tuple<int, int>>();
            Console.WriteLine("EL PROBLEMA DE LAS REINAS");
            Console.Write("Primero, dado que uso el paradigma de las vegas, elige cuantas reinas serán asignadas de forma aleatoria (se recomiendan 2): ");
            int pos = Convert.ToInt32(Console.ReadLine());
            if(pos < 8 && pos >= 0)
            {
                NumeroPosicionesAleatorias = pos;
            }
            //las primeras reinas se colocarán de manera aleatoria, tal y como el paradijma probabilista nos pide
            var random = new Random();
            Console.WriteLine("Posiciones de las reinas introducidas aleatoriamente:");
            for (int i = 0; i < NumeroPosicionesAleatorias; i++)
            {
                int x = random.Next(0, 7);
                int y = random.Next(0, 7);
                // Si es el primer elemento o no está siendo amenaza por una reina y posicionada añade la nueva coordenada
                if (CoordenadasReinas.Count == 0)
                {
                    Console.WriteLine("\t({0},{1})", x, y);
                    CoordenadasReinas.Add(Tuple.Create(x, y));
                }
                else if (!ComprobarPosicion(CoordenadasReinas, x, y))
                {
                    Console.WriteLine("\t({0},{1})", x, y);
                    CoordenadasReinas.Add(Tuple.Create(x, y));
                }
                else
                {
                    i--;
                    //reduzco la i en uno para que se vuelva a ejecutar hasta que se haya introducido una nueva coordenada
                    //ya que choca con otra reina introducida
                }
            }
            Console.WriteLine("Posiciones de las reinas dadas por backtracking: (no definitivas)");
            //Ahora, una vez hecha la asignación aleatoria, empieza el backtracking
            CoordenadasReinas = Bacotraco(CoordenadasReinas, 0);
            PrintCoordenadas(CoordenadasReinas);
            //
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(CoordenadasReinas));
        }

        static List<Tuple<int, int>> Bacotraco(List<Tuple<int, int>> coordenadas, int x) {
            // Recorremos el tablero si no hay 8 reinas colocadas
            if (coordenadas.Count() < 8)
            {
                for (int i = x; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (!ComprobarPosicion(coordenadas, i, j))
                        {
                            //guardo las coordenadas sin introducir las nuevas coordenadas
                            List<Tuple<int, int>> antiguasCoordenadas = coordenadas;
                            Console.WriteLine("\t({0},{1})", i, j);
                            //añado el nuevo elemento a las coordenadas
                            coordenadas.Add(Tuple.Create(i, j));
                            // hacemos return en caso de encontrar la octaba reina
                            if(coordenadas.Count == 8)
                            {
                                return coordenadas; 
                            }
                            //hago la llamada recursiva para encontrar el siguiente
                            List<Tuple<int, int>> nuevaLista = Bacotraco(coordenadas, i + 1);
                            //si la nueva lista es igual a la antigua, significa que el algoritmo no ha encontrado las 8 reinas con esta nueva posicion
                            if (nuevaLista == antiguasCoordenadas) {
                                coordenadas = antiguasCoordenadas;
                            }
                            // si la nueva lista esta completa, con las 8 reinas, hacemos un return
                            if(nuevaLista.Count() == 8)
                            {
                                return nuevaLista;
                            }
                        }
                    }
                }
                if (coordenadas.Count != 8)
                {
                    //Si ya he recorrido completamente el tablero y no ha encontrado todas las coordenadas devuelve la lista que le han pasado menos el elemento anterior
                    coordenadas.RemoveAt(coordenadas.Count - 1);
                    return coordenadas;
                }
            }
            return coordenadas;
        }
        static void PrintCoordenadas(List<Tuple<int, int>> coordenadas)
        {
            Console.WriteLine("Coordenadas finales de las reinas posicionadas:");
            int incremento = 0;
            Console.WriteLine("\tX  Y");
            Console.WriteLine("       ------");
            foreach(Tuple<int,int> coordenada in coordenadas)
            {
                Console.WriteLine("{2}\t{0}  {1}", coordenada.Item1, coordenada.Item2,incremento+1);
                incremento++;
            }
            if(incremento != 8) { Console.WriteLine("[-] No se han podido encontrar las 8 reinas con las posiciones aleatorias dadas"); }
        }
        static bool ComprobarPosicion(List<Tuple<int, int>> CoordenadasReinas, int x, int y)
        {
            //Aqui voy a comprobar si las coordenadas x e y están amenazadas por alguna de las reinas ya posicionadas
            // True = si ha detectado que la posicion x e y es amenazada por alguna reina
            // False = no ha detectado que nunguna reina amenace a esta posición
            foreach (Tuple<int, int> reina in CoordenadasReinas)
            {
                if (ComprobarReinas(reina, x, y))//Comprueba si las coordenadas coinciden con las reinas
                {
                    return true;
                }
                if (ComprobarDiagonales(reina, x, y)) // si las coordenadas coinciden con las diagonales hace return true
                {
                    return true;
                }
                if (ComprobarFilasYColumnas(reina, x, y))//se fija en las filas y columnas para saber si amenaza a la nueva posición
                {
                    return true;
                }
            }
            return false;
        }
        static bool ComprobarReinas(Tuple<int, int> reina, int x, int y)
        {
            // Se llama desde ComprobarPosicion()
            return reina.Item1 == x & reina.Item2 == y;
        }
        static bool ComprobarDiagonales(Tuple<int, int> reina, int x, int y)
        {
            // Se llama desde ComprobarPosicion()
            //diagonal superior izquierda
            int i = reina.Item1;
            int j = reina.Item2;
            for (; i >= 0 && j >= 0; i--, j--)
            {
                if (x == i & y == j)
                {
                    return true;
                }
            }
            //diagonal inferior izquierda
            i = reina.Item1;
            j = reina.Item2;
            for (; i >= 0 && j < 8; i--, j++)
            {
                if (x == i & y == j)
                {
                    return true;
                }
            }
            //diagonal superior derecha
            i = reina.Item1;
            j = reina.Item2;
            for (; i < 8 && j >= 0; i++, j--)
            {
                if (x == i & y == j)
                {
                    return true;
                }
            }
            //diagonal superior izquierda
            i = reina.Item1;
            j = reina.Item2;
            for (; i < 8 && j < 8; i++, j++)
            {
                if (x == i & y == j)
                {
                    return true;
                }
            }
            //si no ha coincidido, no está amenazado por la diagonal por esta reina
            return false;
        }
        static bool ComprobarFilasYColumnas(Tuple<int, int> reina, int x, int y)
        {
            // Se llama desde ComprobarPosicion()
            if (reina.Item1 == x || reina.Item2 == y)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}