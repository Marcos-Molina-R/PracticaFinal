using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq; //array.Contains(variable);
//Array.IndexOf(stringArray, value); si el resultado es -1, esta value en la stringarray
/*
 * 
 * El problema de las 8 reinas
 * El objetivo de este problema es posicionar 8 reinas en un tablero de ajedrez sin que estas se amenazcan entre ellas
 * Usaremos un paradijma probabilista para resolverlo, haciendo uso de un factor aleatorio para encontrar la solución más rapidamente.
 * Aunque se use este paradijma, el algoritomo se apoyará en él, realmente será de tipo backtracking con un factor de aleatoriedad
 * 
 */ 
namespace PracticaFinal
{
    class Program
    {
        // Esta variable global nos va a delimitar cuantas reinas se van a colocar de manera aleatoria
        static int NumeroPosicionesAleatorias = 2;
        
        static void Main(string[] args)
        {
            List<Tuple <int, int>> CoordenadasReinas = new List<Tuple<int,int>>();
            //las dos primeras reinas se colocarán de manera aleatoria, tal y como el paradijma probabilista nos pide
            var random = new Random();
            Console.WriteLine("Posiciones de las reinas introducidas aleatoriamente:");
            for (int i = 0; i < NumeroPosicionesAleatorias; i++)
            {
                int x = random.Next(0, 7);
                int y = random.Next(0, 7);
                // Si es el primer elemento o no está siendo amenaza por una reina y posicionada añade la nueva coordenada
                if (CoordenadasReinas.Count == 0)
                {
                    Console.WriteLine("\t({0},{1})",x,y);
                    CoordenadasReinas.Add(Tuple.Create(x,y));
                }else if (!ComprobarPosicion(CoordenadasReinas, x, y) )
                {
                    Console.WriteLine("\t({0},{1})",x,y);
                    CoordenadasReinas.Add(Tuple.Create(x,y));
                }
                else
                {
                    i--;
                    //reduzco la i en uno para que se vuelva a ejecutar hasta que se haya introducido una nueva coordenada
                    //ya que choca con otra reina introducida
                }
            }
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
            if (reina.Item1 == x & reina.Item2 == y)
            {
                return true;
            }
            return false;
        }

        static bool ComprobarDiagonales(Tuple<int, int> reina, int x, int y)
        {
            // Se llama desde ComprobarPosicion()
            //diagonal superior izquierda
            int i = reina.Item1;
            int j = reina.Item2;
            for (; i <= 0 && j <= 0; i--, j--)
            {
                if (x == i & y == j)
                {
                    return true;
                }
            }
            //diagonal inferior izquierda
            i = reina.Item1;
            j = reina.Item2;
            for (; i <= 0 && j > 8; i--, j++)
            {
                if (x == i & y == j)
                {
                    return true;
                }
            }
            //diagonal superior derecha
            i = reina.Item1;
            j = reina.Item2;
            for (; i > 8 && j <= 0; i++, j--)
            {
                if (x == i & y == j)
                {
                    return true;
                }
            }
            //diagonal superior izquierda
            i = reina.Item1;
            j = reina.Item2;
            for (; i > 8 && j > 8; i++, j++)
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
