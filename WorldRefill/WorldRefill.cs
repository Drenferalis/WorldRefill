﻿using System;
using System.Collections.Generic;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;

// using TShockAPI.DB;
//using Mono.Data.Sqlite;
using MySql.Data.MySqlClient;

namespace WorldRefill
{
    [ApiVersion(1, 14)]
    public class WorldRefill : TerrariaPlugin
    {
        public WorldRefill(Main game)
            : base(game)
        {
        }
        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoCrystals, "gencrystals")); //Life Crystals
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoPots, "genpots"));         //Pots
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoOrbs, "genorbs"));         //Orbs
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoAltars, "genaltars"));     //Demon Altars
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoTraps, "gentraps"));       //Traps
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoStatues, "genstatues"));   //Statues
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoOres, "genores"));         //ores
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoWebs, "genwebs"));         //webs
            //Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoMineHouse, "genhouse"));   //mine house
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoTrees, "gentrees"));       //trees
            //Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoIsland, "genisland"));     //floating island
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoShrooms, "genpatch"));     //mushroom patch
            //Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoLake, "genlake"));         //lake
            //Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoMountain, "genmountain")); //mountain
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", CountEmpties, "genchests"));    //chests
            //Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoIslandHouse, "genihouse"));    //island house
            Commands.ChatCommands.Add(new Command("tshock.world.causeevents", DoHV, "hellavator"));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }

        public override Version Version
        {
            get { return new Version("1.2"); }
        }
        public override string Name
        {
            get { return "World Refill Plugin"; }
        }
        public override string Author
        {
            get { return "by k0rd / Updated by IcyPhoenix and Enerdy"; }
        }
        public override string Description
        {
            get { return "Refill your world!"; }
        }

        //Updating all players
        public static void InformPlayers(bool hard = false)
        {
            foreach (TSPlayer person in TShock.Players)
            {
                if ((person != null) && (person.Active))
                {
                    //Spams too much if set on a timer and all of them fire at the sametime
                    //person.SendMessage("The server is sending you map data due to world restock...");
                    if (hard)
                    {
                        var myX = person.TileX;
                        var myy = person.TileY;
                        person.SendTileSquare(person.TileX, person.TileY, 150);
                        int count;
                        for (count = person.TileY; count < Main.maxTilesY; count += 150)
                        {
                            person.Teleport(person.TileX + 1, count);
                        }
                        person.Teleport(myX, myy);
                    }

                    else
                        person.SendTileSquare(person.TileX, person.TileY, 150);
                }
            }

        }


        private void DoCrystals(CommandArgs args)
        {

            if (args.Parameters.Count == 1)
            {
                var mCry = Int32.Parse(args.Parameters[0]);
                var surface = Main.worldSurface;
                var trycount = 0;
                //maxtries = retry amounts if generation of object fails
                const int maxtries = 1000000;
                //realcount = actual amount of objects generated
                var realcount = 0;

                //Attempting to generate Objects
                while (trycount < maxtries)
                {
                    if (WorldGen.AddLifeCrystal(WorldGen.genRand.Next(1, Main.maxTilesX), WorldGen.genRand.Next((int)(surface + 20.0), Main.maxTilesY)))
                    {
                        realcount++;
                        //Determine if enough Objects have been generated
                        if (realcount == mCry) break;
                    }
                    trycount++;
                }
                //Notify user on success
                args.Player.SendMessage(string.Format("Generated and hid {0} Life Crystals.", realcount), Color.Green);

                InformPlayers();
            }
            else
            {
                //notify user of command failure
                args.Player.SendMessage(string.Format("Usage: /gencrystals (number of crystals to generate)"), Color.Green);
            }

        }

        private void DoPots(CommandArgs args)
        {
            if (args.Parameters.Count == 1)
            {

                var mPot = Int32.Parse(args.Parameters[0]);
                var surface = Main.worldSurface;
                var trycount = 0;
                const int maxtries = 1000000;
                var realcount = 0;
                while (trycount < maxtries)
                {
                    var tryX = WorldGen.genRand.Next(1, Main.maxTilesX);
                    var tryY = WorldGen.genRand.Next((int)surface - 10, Main.maxTilesY);
                    if (WorldGen.PlacePot(tryX, tryY, 28))
                    {
                        realcount++;
                        if (realcount == mPot)
                            break;
                    }
                    trycount++;

                }
                args.Player.SendMessage(string.Format("Generated and hid {0} Pots.", realcount), Color.Green);
                InformPlayers();
            }
            else
            {
                args.Player.SendMessage(string.Format("Usage: /genpots (number of pots to generate)"), Color.Green);
            }
        }

        private void DoOrbs(CommandArgs args)
        {
            if (args.Parameters.Count == 1)
            {

                var mOrb = Int32.Parse(args.Parameters[0]);
                var surface = Main.worldSurface;
                var trycount = 0;
                const int maxtries = 1000000;
                var realcount = 0;
                while (trycount < maxtries)
                {
                    var tryX = WorldGen.genRand.Next(1, Main.maxTilesX);
                    var tryY = WorldGen.genRand.Next((int)surface + 20, Main.maxTilesY);

                    if ((!Main.tile[tryX, tryY].active()) && (Main.tile[tryX, tryY].wall == (byte)3))
                    {
                        WorldGen.AddShadowOrb(tryX, tryY);
                        if (Main.tile[tryX, tryY].type == 31)
                        {
                            realcount++;
                            if (realcount == mOrb)
                                break;
                        }
                    }
                    trycount++;
                }
                InformPlayers();
                args.Player.SendMessage(string.Format("Generated and hid {0} Orbs.", realcount), Color.Green);
            }
            else
            {
                args.Player.SendMessage(string.Format("Usage: /genorbs (number of orbs to generate)"), Color.Green);
            }
        }
        private void DoAltars(CommandArgs args)
        {
            if (args.Parameters.Count == 1)
            {

                var mAltar = Int32.Parse(args.Parameters[0]);
                var surface = Main.worldSurface;
                var trycount = 0;
                const int maxtries = 1000000;
                var realcount = 0;
                while (trycount < maxtries)
                {
                    var tryX = WorldGen.genRand.Next(1, Main.maxTilesX);
                    var tryY = WorldGen.genRand.Next((int)surface + 10, Main.maxTilesY);

                    WorldGen.Place3x2(tryX, tryY, 26);
                    if (Main.tile[tryX, tryY].type == 26)
                    {
                        realcount++;
                        if (realcount == mAltar)
                            break;
                    }

                    trycount++;
                }
                InformPlayers();
                args.Player.SendMessage(string.Format("Generated and hid {0} Demon Altars.", realcount), Color.Green);
            }
            else
            {
                args.Player.SendMessage(string.Format("Usage: /genaltars (number of Demon Altars to generate)"), Color.Green);
            }
        }
        private void DoTraps(CommandArgs args)
        {
            if (args.Parameters.Count == 1)
            {
                args.Player.SendMessage("Generating traps.. this may take a while..", Color.Green);
                var mTrap = Int32.Parse(args.Parameters[0]);
                var surface = Main.worldSurface;
                var trycount = 0;
                const int maxtries = 100000;
                var realcount = 0;
                while (trycount < maxtries)
                {
                    var tryX = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
                    var tryY = WorldGen.genRand.Next((int)surface, Main.maxTilesY - 300);


                    if (Main.tile[tryX, tryY].wall == 0 && WorldGen.placeTrap(tryX, tryY, -1))
                    {
                        realcount++;
                        if (realcount == mTrap)
                            break;
                    }

                    trycount++;
                }
                InformPlayers();
                args.Player.SendMessage(string.Format("Generated and hid {0} traps.", realcount), Color.Green);
            }
            else
            {
                args.Player.SendMessage(string.Format("Usage: /gentraps (number of Traps to generate)"), Color.Green);
            }
        }
        private void DoStatues(CommandArgs args)
        {
            if (args.Parameters.Count == 1)
            {
                args.Player.SendMessage("Generating statues.. this may take a while..", Color.Green);
                var mStatue = Int32.Parse(args.Parameters[0]);
                var surface = Main.worldSurface;
                var trycount = 0;
                const int maxtries = 100000;
                var realcount = 0;
                while (trycount < maxtries)
                {
                    var tryX = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
                    var tryY = WorldGen.genRand.Next((int)surface + 20, Main.maxTilesY - 300);
                    var tryType = WorldGen.genRand.Next((int)2, 44);

                    while (!Main.tile[tryX, tryY].active())
                    {
                        tryY++;
                    }
                    tryY--;

                    if (tryY < Main.maxTilesY - 300)
                    {

                        WorldGen.PlaceTile(tryX, tryY, 105, true, true, -1, tryType);

                        if (Main.tile[tryX, tryY].type == 105)
                        {
                            realcount++;
                            if (realcount == mStatue)
                                break;
                        }
                    }
                    trycount++;
                }
                args.Player.SendMessage(string.Format("Generated and hid {0} Statues.", realcount), Color.Green);
                InformPlayers();
            }
            else if (args.Parameters.Count == 2)
            {
                List<string> types = new List<string>
                                         {
                                             "Armor",
                                             "Angel",
                                             "Star",
                                             "Sword",
                                             "Slime",
                                             "Goblin",
                                             "Shield",
                                             "Bat",
                                             "Fish",
                                             "Bunny",
                                             "Skeleton",
                                             "Reaper",
                                             "Woman",
                                             "Imp",
                                             "Gargoyle",
                                             "Gloom",
                                             "Hornet",
                                             "Bomb",
                                             "Crab",
                                             "Hammer",
                                             "Potion",
                                             "Spear",
                                             "Cross",
                                             "Jellyfish",
                                             "Bow",
                                             "Boomerang",
                                             "Boot",
                                             "Chest",
                                             "Bird",
                                             "Axe",
                                             "Corrupt",
                                             "Tree",
                                             "Anvil",
                                             "Pickaxe",
                                             "Mushroom",
                                             "Eyeball",
                                             "Pillar",
                                             "Heart",
                                             "Pot",
                                             "Sunflower",
                                             "King",
                                             "Queen",
                                             "Piranha",
                                             "Ukown"
                                         };

                string mReqs = args.Parameters[1].ToLower();
                var mStatue = Int32.Parse(args.Parameters[0]);
                var surface = Main.worldSurface;
                var trycount = 0;
                const int maxtries = 100000;
                var realcount = 0;
                int stid = 0;
                string found = "unknown type!";
                foreach (string ment in types)
                {
                    found = ment.ToLower();
                    if (found.StartsWith(mReqs))
                    {
                        break;
                    }
                    stid++;
                }
                if (stid < 44)
                {

                    args.Player.SendMessage(string.Format("Generating {0} statues.. this may take a while..", found), Color.Green);
                    while (trycount < maxtries)
                    {
                        var tryX = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
                        var tryY = WorldGen.genRand.Next((int)surface + 20, Main.maxTilesY - 300);

                        while (!Main.tile[tryX, tryY].active())
                        {
                            tryY++;
                        }
                        tryY--;

                        if (tryY < Main.maxTilesY - 300)
                        {

                            WorldGen.PlaceTile(tryX, tryY, 105, true, true, -1, stid);

                            if (Main.tile[tryX, tryY].type == 105)
                            {
                                realcount++;
                                if (realcount == mStatue)
                                    break;
                            }
                        }
                        trycount++;
                    }
                    args.Player.SendMessage(string.Format("Generated and hid {0} {1} ({2})Statues.", realcount, found, stid), Color.Green);
                    InformPlayers();
                }
                else
                {
                    args.Player.SendMessage(string.Format("Couldn't find a match for {0}.", mReqs), Color.Green);
                }
            }
            else
            {
                args.Player.SendMessage(string.Format("Usage: /genstatues (number of statues to generate) [(optional)name of statue]"), Color.Green);
            }
        }

        private static void DoOres(CommandArgs args)
        {
            if (WorldGen.genRand == null)
                WorldGen.genRand = new Random();

            TSPlayer ply = args.Player;
            int oreType;
            float oreAmts = 100f;
            //Sigh why did I bother checking what this does...gets completely overwritten
            //Mod altarcount divide 3 - gives number between: 0 to 2
            //int num = WorldGen.altarCount % 3;
            //Altarcount divide 3 + 1 - gives number between: 1 to infinity
            //int num2 = WorldGen.altarCount / 3 + 1;
            //4200 = small world size
            //6400 = medium world size
            //8400 = large world size
            //returns value: - 0 , 1.523809523809524, 2
            //float num3 = (float)(Main.maxTilesX / 4200);
            //Gives number between: -1 to 1
            //int num4 = 1 - num;
            //num3 * 310f - returns value: 0, 472.3809523809524, 620
            //(float)(85 * num) - gives number between 0 to 170
            //Returns value: -170, 302.3809523809524, 450
            //num3 = num3 * 310f - (float)(85 * num);
            //Returns Value: -144.5, 257.0238095238095, 382.5
            //num3 *= 0.85f;
            //gives number between: -144.5 to 382.5
            //num3 /= (float)num2;

            if (args.Parameters.Count < 1)
            {
                ply.SendMessage("Usage: /genores (type) (amount)", Color.Red);    //should this be a help message instead?
                return;
            }
            else if (args.Parameters[0].ToLower() == "cobalt")
            {
                oreType = 107;
            }
            else if (args.Parameters[0].ToLower() == "mythril")
            {
                oreType = 108;
            }
            else if (args.Parameters[0].ToLower() == "copper")
            {
                oreType = 7;
            }
            else if (args.Parameters[0].ToLower() == "iron")
            {
                oreType = 6;
            }
            else if (args.Parameters[0].ToLower() == "silver")
            {
                oreType = 9;
            }
            else if (args.Parameters[0].ToLower() == "gold")
            {
                oreType = 8;
            }
            else if (args.Parameters[0].ToLower() == "demonite")
            {
                oreType = 22;
            }
            else if (args.Parameters[0].ToLower() == "sapphire")
            {
                oreType = 63;
            }
            else if (args.Parameters[0].ToLower() == "ruby")
            {
                oreType = 64;
            }
            else if (args.Parameters[0].ToLower() == "emerald")
            {
                oreType = 65;
            }
            else if (args.Parameters[0].ToLower() == "topaz")
            {
                oreType = 66;
            }
            else if (args.Parameters[0].ToLower() == "amethyst")
            {
                oreType = 67;
            }
            else if (args.Parameters[0].ToLower() == "diamond")
            {
                oreType = 68;
            }
            else if (args.Parameters[0].ToLower() == "adamantite")
            {
                oreType = 111;
            }
            else if (args.Parameters[0].ToLower() == "hellstone")
            {
                oreType = 58;
            }

            // New Ores
            else if (args.Parameters[0].ToLower() == "tin")
            {
                oreType = 166;
            }
            else if (args.Parameters[0].ToLower() == "lead")
            {
                oreType = 167;
            }
            else if (args.Parameters[0].ToLower() == "tungsten")
            {
                oreType = 168;
            }
            else if (args.Parameters[0].ToLower() == "platinum")
            {
                oreType = 169;
            }

            // 1.2 Hardmode Ores
            else if (args.Parameters[0].ToLower() == "palladium")
            {
                oreType = 221;
            }
            else if (args.Parameters[0].ToLower() == "orichalcum")
            {
                oreType = 222;
            }
            else if (args.Parameters[0].ToLower() == "titanium")
            {
                oreType = 223;
            }
            else
            {
                ply.SendMessage("Warning! Typo in Tile name or Tile does not exist", Color.Red);    //should this be a help message instead?
                return;
            }

            //If user specifies how many ores to generate (make sure not over 10000)
            if (args.Parameters.Count > 1)
            {
                float.TryParse(args.Parameters[1], out oreAmts);
                oreAmts = Math.Min(oreAmts, 10000f);
            }
            //oreGened = track amount of ores generated already
            int oreGened = 0;
            int minFrequency;
            int maxFrequency;
            int minSpread;
            int maxSpread;
            while ((float)oreGened < oreAmts)
            {
                //Get random number from 100 tiles each side
                int i2 = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                double worldY = Main.worldSurface;
                //Rare Ores  - Adamantite (Titanium), Demonite, Diamond
                if ((oreType == 111) || (oreType == 22) || (oreType == 223) || (oreType >= 63) && (oreType <= 68))
                {
                    //Some formula created by k0rd for getting somewhere between hell and roughly half way after rock
                    worldY = (Main.rockLayer + Main.rockLayer + (double)Main.maxTilesY) / 3.0;
                    minFrequency = 2;
                    minSpread = 2;
                    maxFrequency = 3;
                    maxSpread = 3;
                }
                //Hellstone Only
                else if (oreType == 58)
                {
                    //roughly where hell is
                    worldY = Main.maxTilesY - 200;
                    minFrequency = 4;
                    minSpread = 4;
                    maxFrequency = 9;
                    maxSpread = 9;
                }
                else
                {
                    worldY = Main.rockLayer;
                    minFrequency = 5;
                    minSpread = 9;
                    maxFrequency = 5;
                    maxSpread = 9;
                }
                //Gets random number based on minimum spawn point to maximum depth of map
                int j2 = WorldGen.genRand.Next((int)worldY, Main.maxTilesY - 150);
                WorldGen.OreRunner(i2, j2, (double)WorldGen.genRand.Next(minSpread, maxSpread), WorldGen.genRand.Next(minFrequency, maxFrequency), oreType);
                oreGened++;
            }
            ply.SendMessage(String.Format("Spawned {0} tiles of {1}", Math.Floor(oreAmts), args.Parameters[0].ToLower()), Color.Green);
            InformPlayers();
        }
        private void DoWebs(CommandArgs args)
        {
            if (args.Parameters.Count == 1)
            {

                var mWeb = Int32.Parse(args.Parameters[0]);
                var surface = Main.worldSurface;
                var trycount = 0;
                const int maxtries = 1000000;
                var realcount = 0;
                while (trycount < maxtries)
                {
                    var tryX = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
                    var tryY = WorldGen.genRand.Next(150, Main.maxTilesY - 300);
                    int direction = WorldGen.genRand.Next(2);
                    if (direction == 0)
                    {
                        direction = -1;
                    }
                    else
                    {
                        direction = 1;
                    }
                    while (!Main.tile[tryX, tryY].active() && tryY > 149)
                    {
                        tryY--;
                    }
                    tryY++;
                    while (!Main.tile[tryX, tryY].active() && tryX > 10 && tryX < Main.maxTilesX - 10)
                    {
                        tryX += direction;
                    }
                    tryX -= direction;

                    if ((tryY < Main.maxTilesY - 300) && (tryX < Main.maxTilesX - 20) && (tryX > 20) && (tryY > 150))
                    {

                        WorldGen.TileRunner(tryX, tryY, (double)WorldGen.genRand.Next(4, 11), WorldGen.genRand.Next(2, 4), 51, true, (float)direction, -1f, false, false);
                        realcount++;
                        if (realcount == mWeb)
                            break;
                    }
                    trycount++;

                }
                args.Player.SendMessage(string.Format("Generated and hid {0} Webs.", realcount), Color.Green);
                InformPlayers();
            }
            else
            {
                args.Player.SendMessage(string.Format("Usage: /genwebs (number of webs to generate)"), Color.Green);
            }
        }

        private void DoTrees(CommandArgs args)
        {
            var counter = 0;
            while ((double)counter < (double)Main.maxTilesX * 0.003)
            {
                int tryX = WorldGen.genRand.Next(50, Main.maxTilesX - 50);
                int tryY = WorldGen.genRand.Next(25, 50);
                for (var tick = tryX - tryY; tick < tryX + tryY; tick++)
                {
                    var offset = 20;
                    while ((double)offset < Main.worldSurface)
                    {
                        WorldGen.GrowEpicTree(tick, offset);
                        offset++;
                    }
                }
                counter++;
            }
            WorldGen.AddTrees();
            args.Player.SendMessage("Enjoy your trees.", Color.Green);
            InformPlayers();
        }
        private void DoShrooms(CommandArgs args)
        {
            int tryX = args.Player.TileX;
            int tryY = args.Player.TileY;
            const int offset = 25;
            WorldGen.ShroomPatch(tryX, tryY + 1);
            for (int z = args.Player.TileX - offset; z < args.Player.TileX + offset; z++)
            {
                for (int y = args.Player.TileY - offset; y < args.Player.TileY + offset; y++)
                {
                    if (Main.tile[z, y].active())
                    {
                        WorldGen.SpreadGrass(z, y, 59, 70, false);

                    }
                }
            }

            InformPlayers();
            args.Player.SendMessage("Mushroom Farm generated.", Color.Green);
        }

        private void DoHV(CommandArgs args)
        {
            int meX = args.Player.TileX;
            int meY = args.Player.TileY;
            const int maxsize = 25;
            const int bump = 4;
            int cx;
            int ypos = 0;
            int start = 0;

            int bottom = Main.maxTilesY - 150;
            int width = 3;
            if (args.Parameters.Count == 1)
                width = Int32.Parse(args.Parameters[0]);
            if (width < 2) width = 2;
            if (width > maxsize) width = maxsize;
            start = meX - (width / 2);
            ypos = meY + bump;
            start--;
            width++;
            for (cx = start; cx < width + start; cx++)
            {
                int xc;
                for (xc = ypos; xc < bottom; xc++)
                {
                    //                   WorldGen.KillTile(cx, xc,false,false,false);
                    if ((cx == start) || (cx == width + start - 1))
                    {
                        Main.tile[cx, xc].type = 121;
                        Main.tile[cx, xc].active(true);
                    }
                    else
                    {
                        WorldGen.KillTile(cx, xc, false, false, false);
                        Main.tile[cx, xc].wall = 25;
                    }
                    //            Log.ConsoleError(string.Format("pos - x: {0} y: {1}",cx,xc));
                }
            }


            InformPlayers(true);
        }

        private void CountEmpties(CommandArgs args)
        {

            //Code stold from InanZen - Not implemented yet
            // -------------- Chests  ----------------------
            //int chests = 0;
            try
            {
                switch (TShock.Config.StorageType.ToLower())
                {
                    case "mysql":
                        string[] host = TShock.Config.MySqlHost.Split(':');
                        ChestDB = new MySqlConnection();
                        {
                            string ConnectionString = string.Format("Server={0}; Port={1}; Database={2}; Uid={3}; Pwd={4};",
                                host[0],
                                host.Length == 1 ? "3306" : host[1],
                                TShock.Config.MySqlDbName,
                                TShock.Config.MySqlUsername,
                                TShock.Config.MySqlPassword);
                        };
                        break;
                    case "sqlite":
                        Log.ConsoleInfo(" -> SQLite is Disabled sorry!");
                        break;
                }
            //    TShock.Utils.Broadcast(" -> generating chests...", Color.DarkMagenta);
            //    Log.ConsoleInfo(" -> generating chests..");
            //    ChestDB.Query("DELETE FROM Chests WHERE X BETWEEN @0 AND @1 AND Y BETWEEN @2 AND @3", startX, startX + startW, startY, startY + startH);
            //    iterations = h / 40;
            //    Main.chest[0] = null;
            //    for (int i = 1; i <= iterations; i++)
            //    {
            //        int y = curY + RandomNumber((int)((i - 1) * ((float)h / iterations)), (int)(i * ((float)h / iterations)));
            //        for (int x = curX; x < curX + w; x++)
            //        {
            //            if (!Main.tile[x, y].active && Main.tile[x, y + 1].active)
            //            {
            //                try
            //                {
            //                    if (WorldGen.PlaceChest(x, y) == 0)
            //                    {
            //                        NetItem[] items = new NetItem[4];
            //                        for (int j = 0; j < 4; j++)
            //                        {
            //                            Item item = new Item();
            //                            item.netDefaults(RandomNumber(1, 364));
            //                            if (item.maxStack > 1)
            //                                item.stack = RandomNumber(1, item.maxStack / 2);
            //                            items[j] = new NetItem { netID = item.netID, stack = item.stack, prefix = 0 };
            //                        }
            //                        ChestDB.Query("INSERT INTO Chests (X, Y, Account, Flags, Items, Password, WorldID) VALUES (@0, @1, @2, @3, @4, \'\', @5)",
            //                            x, y - 1, "", 0, String.Format("{0},{1},0,{2},{3},0,{4},{5},0,{6},{7},0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0", items[0].netID, items[0].stack, items[1].netID, items[1].stack, items[2].netID, items[2].stack, items[3].netID, items[3].stack), Main.worldID);
            //                        Main.chest[0] = null;
            //                        x += 100;
            //                        y = curY + RandomNumber((int)((i - 1) * ((float)h / iterations)), (int)(i * ((float)h / iterations)));
            //                        chests++;
            //                    }
            //                }
            //                catch { }
            //            }
            //        }
            //    }
            //    ChestDB.Dispose();

            ////--------------------------

            if (args.Parameters.Count == 0 || args.Parameters.Count > 2)
            {
                args.Player.SendMessage("Usage: /genchests <amount> [gen mode: default/easy/all]", Color.Green);
            }
            int empty = 0;
            int tmpEmpty = 0;
            int chests = 0;
            string setting = "default";
            if (args.Parameters.Count > 1)
            {
                setting = args.Parameters[1];
            }
            const int maxtries = 100000;
            Int32.TryParse(args.Parameters[0], out chests);
            const int threshold = 100;
            for (int x = 0; x < 1000; x++)
            {
                if (Main.chest[x] != null)
                {
                    tmpEmpty++;
                    bool found = false;
                    foreach (Item itm in Main.chest[x].item)
                        if (itm.netID != 0)
                            found = true;
                    if (found == false)
                    {
                        empty++;
                              //TShock.Utils.Broadcast(string.Format("Found chest {0} empty at x {1} y {2}", x, Main.chest[x].x,
                              //                                     Main.chest[x].y));

                        // destroying
                        WorldGen.KillTile(Main.chest[x].x, Main.chest[x].y, false, false, false);
                        Main.chest[x] = null;

                    }

                }

            }
            args.Player.SendMessage(string.Format("uprooted {0} empty out of {1} chests.", empty, tmpEmpty), Color.Green);
            if (chests + tmpEmpty + threshold > 100000)
                chests = 100000 - tmpEmpty - threshold;
            if (chests > 0)
            {
                int chestcount = 0;
                chestcount = tmpEmpty;
                int tries = 0;
                int newcount = 0;
                while (newcount < chests)
                {
                    int contain;
                    if (setting == "default")
                    {
                        // Default Items: Grenade(168), Copper Bar(20), Iron Bar(22), Wooden Arrow(40), Shuriken(42), Lesser Healing Potion(28), Ironskin Potion(292), Shine Potion (298), 
                        // Night Owl Potion(299), Swiftness Potion(290), Torch(8), Bottle(31), Silver Coin(72), Spear(280), Wooden Boomerang(284), Blowpipe(281), Glowstick(282),
                        // Throwing Knife(279), Aglet(285), Silver Bar(21), Regeneration Potion(289), Archery Potion(303), Gills Potion(291), Hunter Potion(304), Band of Regeneration(49),
                        // Magic Mirror(50), Angel Statue(52), Cloud in a Bottle(53), Hermes Boots(54), Enchanted Boomerang(55), Jester's Arrow(51), Suspicious Looking Eye(43), Dynamite(167),
                        // Healing Potion(188), Featherfall Potion(295), Water Walking Potion(302), Gravitation Potion(305), Gold Coin(73), Thorns Potion(301), Shiny Red Baloon(159), Starfury(65),
                        // Lucky Horseshoe(158), Meteorite Bar(117), Hellfire Arrow(265), Magic Power Potion(294), Obsidian Skin Potion(288), Invisibility Potion(297), Battle Potion(300),
                        // Flamelash (218), Flower of Fire(112), Sunfury(220)
                        // ID's Only: 168, 20, 22, 40, 42, 28, 292, 298, 299, 290, 8, 31, 72, 280, 284, 281, 282, 279, 285, 21, 289, 303, 291, 304, 49, 50, 52, 53, 54, 55, 51, 43, 167, 188, 295, 302, 305, 73, 301, 159, 65, 158, 117, 265, 294, 288, 297, 300, 218, 112, 220

                        // Plugin Version 1.2 - New Items: Rope Coil(985), Guide Voodoo Doll(267), Cobalt Shield(156)

                        int[] itemID = new int[] { 168, 20, 22, 40, 42, 28, 292, 298, 299, 290, 8, 31, 72, 280, 284, 281, 282, 279, 285, 21, 289, 303, 291, 304, 49, 50, 52, 53, 54, 55, 51, 43, 167, 188, 295, 302, 305, 73, 301, 159, 65, 158, 117, 265, 294, 288, 297, 300, 218, 112, 220, 985, 267, 156 };
                        contain = itemID[WorldGen.genRand.Next(0, itemID.GetUpperBound(0))];
                    }
                    else if (setting == "all")
                    {
                        contain = WorldGen.genRand.Next(1, 1603);
                    }
                    else if (setting == "easy")
                    {
                        contain = WorldGen.genRand.Next(1, 363);
                    }
                    else
                    {
                        args.Player.SendMessage(string.Format("Warning! Typo in second argument: {0}", args.Parameters[1]), Color.Red);
                        return;
                    }
                    int tryX = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
                    int tryY = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 200);
                    while (!Main.tile[tryX, tryY].active())
                    {
                        tryY++;
                    }
                    tryY--;
                    WorldGen.KillTile(tryX, tryY, false, false, false);
                    WorldGen.KillTile(tryX + 1, tryY, false, false, false);
                    WorldGen.KillTile(tryX, tryY + 1, false, false, false);
                    WorldGen.KillTile(tryX + 1, tryY, false, false, false);

                    if (WorldGen.AddBuriedChest(tryX, tryY, contain, true, 1))
                    {
                        try
                        {
                            string sql = "INSERT INTO Chests (X, Y, Account, Flags, Items, Password, WorldID) VALUES (" + tryX + ", " + tryY + ", 0, flag, " + contain + ", \"\", 2093487259)";
                            MySqlCommand cmd = new MySqlCommand(sql, ChestDB);
                            cmd.ExecuteNonQuery();
                            chestcount++;
                            newcount++;
                        }
                        catch (MySqlException ex)
                        {
                            Log.ConsoleError(ex.ToString());
                        }
                    }
                    if (tries + 1 >= maxtries)
                        break;

                    tries++;
                }
                args.Player.SendMessage(string.Format("generated {0} new chests - {1} total", newcount, chestcount), Color.Green);
                InformPlayers();
            }
            }
            catch (Exception ex)
            {
                Log.ConsoleError(ex.ToString());
            }
        }

        public MySqlConnection ChestDB { get; set; }
    }
}