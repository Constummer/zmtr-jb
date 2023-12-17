using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class LRStats
    {
        public LRStats()
        {
            for (int i = 0; i < 64; i++)
            {
                lr_players[i] = new PlayerStat();
            }
        }

        public void win(CCSPlayerController? player, LastRequest.LRType type)
        {
            var lr_player = lr_player_from_player(player);

            if (lr_player != null && type != LastRequest.LRType.NONE && player != null && player.is_valid())
            {
                int idx = (int)type;
                lr_player.win[idx] += 1;
                inc_db(player, type, true);
                Lib.announce(LastRequest.LR_PREFIX, $"{player.PlayerName} won {LastRequest.LR_NAME[idx]} win {lr_player.win[idx]} : loss {lr_player.loss[idx]}");
            }
        }

        public void loss(CCSPlayerController? player, LastRequest.LRType type)
        {
            var lr_player = lr_player_from_player(player);

            if (lr_player != null && type != LastRequest.LRType.NONE && player != null && player.is_valid())
            {
                int idx = (int)type;
                lr_player.loss[idx] += 1;
                inc_db(player, type, false);

                Lib.announce(LastRequest.LR_PREFIX, $"{player.PlayerName} lost {LastRequest.LR_NAME[idx]} win {lr_player.win[idx]} : loss {lr_player.loss[idx]}");
            }
        }

        private PlayerStat? lr_player_from_player(CCSPlayerController? player)
        {
            if (player == null || !player.is_valid())
            {
                return null;
            }

            var slot = player.slot();

            if (slot == null)
            {
                return null;
            }

            return lr_players[slot.Value];
        }

        private void print_stats(CCSPlayerController? invoke, CCSPlayerController? player)
        {
            if (invoke == null || !invoke.is_valid())
            {
                return;
            }

            var lr_player = lr_player_from_player(player);

            if (lr_player != null && player != null && player.is_valid())
            {
                invoke.PrintToChat($"{LastRequest.LR_PREFIX} lr stats for {player.PlayerName}");

                for (int i = 0; i < LastRequest.LR_SIZE; i++)
                {
                    invoke.PrintToChat($"{LastRequest.LR_PREFIX} {LastRequest.LR_NAME[i]} win {lr_player.win[i]} : loss {lr_player.loss[i]}");
                }
            }
        }

        public void lr_stats_cmd(CCSPlayerController? player, CommandInfo command)
        {
            // just do own player for now
            print_stats(player, player);
        }

        public void purge_player(CCSPlayerController? player)
        {
            var lr_player = lr_player_from_player(player);

            if (lr_player != null)
            {
                for (int i = 0; i < LastRequest.LR_SIZE; i++)
                {
                    lr_player.win[i] = 0;
                    lr_player.loss[i] = 0;
                }

                lr_player.cached = false;
            }
        }

        private class PlayerStat
        {
            public int[] win = new int[LastRequest.LR_SIZE];
            public int[] loss = new int[LastRequest.LR_SIZE];
            public bool cached = false;
        }

        private async void insert_player(String steam_id, String player_name)
        {
            var database = connect_db();

            if (database == null)
            {
                return;
            }

            // insert new player
            var insert_player = new MySqlCommand("INSERT IGNORE INTO stats (steamid,name) VALUES (@steam_id, @name)", database);
            insert_player.Parameters.AddWithValue("@steam_id", steam_id);
            insert_player.Parameters.AddWithValue("@name", player_name);

            try
            {
                await insert_player.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async void inc_db(CCSPlayerController? player, LastRequest.LRType type, bool win)
        {
            if (player == null || !player.is_valid() || type == LastRequest.LRType.NONE)
            {
                return;
            }

            var database = connect_db();

            if (database == null)
            {
                return;
            }

            String name = LastRequest.LR_NAME[(int)type].Replace(" ", "_");

            if (win)
            {
                name += "_win";
            }
            else
            {
                name += "_loss";
            }

            String steam_id = new SteamID(player.SteamID).SteamId2;

            var inc_stat = new MySqlCommand($"UPDATE stats SET {name} = {name} + 1 WHERE steamid = @steam_id", database);
            inc_stat.Parameters.AddWithValue("@steam_id", steam_id);

            try
            {
                Console.WriteLine($"increment {player.PlayerName} : {steam_id} : {name} : {win}");
                await inc_stat.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void read_stats(ulong id, String steam_id, String player_name)
        {
            var database = connect_db();

            if (database == null)
            {
                return;
            }

            // repull player from steamid if they are still around
            CCSPlayerController? player = Utilities.GetPlayerFromSteamId(id);
            int? slot_opt = player.slot();

            if (slot_opt == null)
            {
                return;
            }

            int slot = slot_opt.Value;

            // allready cached we dont care
            if (lr_players[slot].cached)
            {
                return;
            }

            // query steamid
            var query_steam_id = new MySqlCommand("SELECT * FROM stats WHERE steamid = @steam_id", database);
            query_steam_id.Parameters.AddWithValue("@steam_id", steam_id);

            try
            {
                var reader = await query_steam_id.ExecuteReaderAsync();

                if (reader.Read())
                {
                    if (player == null || !player.is_valid() || slot_opt == null)
                    {
                        return;
                    }

                    //Console.WriteLine($"reading out lr stats {player.PlayerName}");

                    for (int i = 0; i < LastRequest.LR_SIZE; i++)
                    {
                        String name = LastRequest.LR_NAME[i].Replace(" ", "_");

                        lr_players[slot].win[i] = (int)reader[name + "_win"];
                        lr_players[slot].loss[i] = (int)reader[name + "_loss"];
                    }

                    lr_players[slot].cached = true;
                }

                // failed to pull player stats
                // insert a new entry
                else
                {
                    //Console.WriteLine("insert new entry");

                    insert_player(steam_id, player_name);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void connect(CCSPlayerController? player)
        {
            if (player == null || !player.is_valid())
            {
                return;
            }

            // attempt to cache player stats
            String name = player.PlayerName;
            String steam_id = new SteamID(player.SteamID).SteamId2;

            read_stats(player.SteamID, steam_id, name);
        }

        public void setup_db(MySqlConnection? database)
        {
            if (database == null)
            {
                return;
            }

            // Make sure Table exists
            var table_cmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS stats (steamid varchar(64) PRIMARY KEY,name varchar(64))", database);
            table_cmd.ExecuteNonQuery();

            // Check table size to see if we have the right number of LR's
            // if we dont make the extra tables
            var col_cmd = new MySqlCommand("SHOW COLUMNS FROM stats", database);
            var col_reader = col_cmd.ExecuteReader();

            int row_count = 0;
            while (col_reader.Read())
            {
                row_count++;
            }
            col_reader.Close();

            // NOTE: both win and lose i.e * 2 + steamid and name
            if (row_count != (LastRequest.LR_SIZE * 2) + 2)
            {
                for (int i = 0; i < LastRequest.LR_SIZE; i++)
                {
                    String name = LastRequest.LR_NAME[i].Replace(" ", "_");

                    try
                    {
                        // NOTE: could use NOT Exists put old sql versions dont play nice
                        // ideally we would use an escaped statement but these strings aernt user controlled anyways
                        var insert_table_win = new MySqlCommand($"ALTER TABLE stats ADD COLUMN {name + "_win"} int DEFAULT 0", database);
                        insert_table_win.ExecuteNonQuery();

                        var insert_table_loss = new MySqlCommand($"ALTER TABLE stats ADD COLUMN {name + "_loss"} int DEFAULT 0", database);
                        insert_table_loss.ExecuteNonQuery();
                    }
                    catch { }
                }
            }
        }

        public MySqlConnection? connect_db()
        {
            try
            {
                MySqlConnection? database = new MySqlConnection(
                    $"Server={config.server};User ID={config.username};Password={config.password};Database={config.database};Port={config.port}");

                database.Open();

                return database;
            }
            catch
            {
                //Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public JailConfig config = new JailConfig();

        private PlayerStat[] lr_players = new PlayerStat[64];
    }
}