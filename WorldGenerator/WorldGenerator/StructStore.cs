using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldGenerator
{
    class StructStore
    {
        
        struct cell
        {
            int X;
            int Y;
            short cellType;
        }

        struct Station
        {
            int X, Y;
            List<cell> StationList;
        }

        struct PlayerBuilt
        {
            //int X, Y; from sun/center
            List<cell> PlayerList;
        }

        struct Planets
        {
            int X;
            int Y;
            char planetType;
            List<cell> PlanetList;
        }

        struct Asteroids
        {
            int X;
            int Y;
            char asteroidType;
            List<cell> AsteroidList;
        }

        struct Comets
        {
            int X;
            int Y;
            char cometType;
            List<cell> CometList;
        }

        struct Meteors
        {
            int X;
            int Y;
            List<cell> MeteorList;
        }

        struct Sun
        {
            int X;//0
            int Y;//0
        }

        Station station;
        PlayerBuilt userCells;

        StructStore()
        {
            station = new Station();
            userCells = new PlayerBuilt();
          // List<cell> PlayerList = new List<cell>();
        }

        void WriteToFile(String fileName)
        {
         //   file.WriteLine(station.cells.Length());
        }
    }
}
