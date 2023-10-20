using System;
using System.Data.SQLite;
using System.Web;

namespace Glosor
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=glosor.db;";
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string createTableSql = "CREATE TABLE IF NOT EXISTS FortniteRarity (ID INTEGER PRIMARY KEY, Name TEXT, Rarity TEXT)";
            SQLiteCommand createTableCommand = new SQLiteCommand(createTableSql, connection);
            createTableCommand.ExecuteNonQuery();

            string insertDataSql = "INSERT INTO FortniteRarity (Name, Rarity) VALUES (@Name, @Rarity)";
            SQLiteCommand insertDataCommand = new SQLiteCommand(insertDataSql, connection);

            ListOptions();
            // Add parameters for the values you want to insert
            void ListOptions()
            {
                Console.WriteLine("1. Add skin");
                Console.WriteLine("2. Delete skin");
                Console.WriteLine("3. Show skins");
                Console.WriteLine("4. Exit");
                Console.WriteLine("5. Practice");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.WriteLine("----Add a new skin----");
                    Console.WriteLine("Enter Skin: ");
                    string skinName = Console.ReadLine();
                    Console.WriteLine("Enter Rarity: ");
                    string skinRarity = Console.ReadLine();

                    insertDataCommand.Parameters.AddWithValue("@Name", skinName);

                    insertDataCommand.Parameters.AddWithValue("@Rarity", skinRarity);

                    insertDataCommand.ExecuteNonQuery();

                    Console.Clear();
                    ListOptions();
                }

                if (choice == "2")
                {
                    string deleteAllDataSql = "DELETE FROM FortniteRarity";
                    SQLiteCommand deleteDataCommand = new SQLiteCommand(deleteAllDataSql, connection);
                    deleteDataCommand.ExecuteNonQuery();

                    Console.WriteLine("All skins removed");

                    Thread.Sleep(2000);

                    Console.Clear();
                    ListOptions();
                }

                if (choice == "3")
                {

                    string selectDataSql = "SELECT * FROM FortniteRarity";
                    SQLiteCommand selectDataCommand = new SQLiteCommand(selectDataSql, connection);

                    using (SQLiteDataReader reader = selectDataCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string rarity = reader.GetString(2);
                            Console.WriteLine($"ID: {id}, Name: {name}, Rarity: {rarity}");
                        }
                    }

                    Console.WriteLine("Press any key to exit: ");
                    Console.ReadKey();

                    Console.Clear();
                    ListOptions();
                }

                if (choice == "4")
                {
                    connection.Close(); // Close the connection when done
                    System.Environment.Exit(1);
                }

                if (choice == "5")
                {
                    Practice();
                    void Practice()
                    {
                        Console.Clear();
                        Console.WriteLine("---Guess the rarity---");

                        // Select a random object (skin) from the database
                        string getRandomObjectSql = "SELECT Name, Rarity FROM FortniteRarity ORDER BY RANDOM() LIMIT 1";
                        SQLiteCommand getRandomObjectCommand = new SQLiteCommand(getRandomObjectSql, connection);

                        using (SQLiteDataReader reader = getRandomObjectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string randomObjectName = reader.GetString(0);
                                string correctRarity = reader.GetString(1);

                                Console.WriteLine($"Guess the rarity of: {randomObjectName}");
                                Console.Write("Enter your guess: ");
                                string userGuess = Console.ReadLine();
                                Console.WriteLine("");

                                if (userGuess.ToLower() == correctRarity.ToLower())
                                {
                                    Console.WriteLine("Correct!");
                                }
                                else
                                {
                                    Console.WriteLine($"Wrong. The correct rarity is: {correctRarity}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No objects found in the database.");
                            }
                        }

                        // Allow the user to continue practicing
                        Console.WriteLine("Want to keep practising? y/n");
                        if (Console.ReadKey().KeyChar == 'y')
                        {
                            Practice();
                        }
                        else
                        {
                            Console.Clear();
                            ListOptions();
                        }
                    }
                    
                }
            }
        }
    }
}