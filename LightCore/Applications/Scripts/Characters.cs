using MySql.Data.MySqlClient;
using Dapper;

namespace LightCore.Applications.Scripts
{
	/// <summary>
	/// Provides operations related to Character Database. 
	/// </summary>
	public class Characters
	{
#region Operations
		/// <summary>
		/// Retrieves the total number of characters from the characters database.
		/// </summary>
		/// <remarks>
		/// This method updates the <see cref="CardServerStatistics.Characters"/> property with the retrieved count.
		/// </remarks>
		public static void GetCharacterCount()
		{
			Database._construct();
			using (MySqlConnection connection = Database.GetConnection(ConnectionStrings.characters))
			{
				Database.Open(connection);
				String Query = "SELECT COUNT(*) FROM `characters`";

				using (MySqlCommand command = new MySqlCommand(Query, connection))
				{
					CardServerStatistics.characters = Convert.ToInt32(command.ExecuteScalar());
				}
			}
		}

		/// <summary>
		/// Retrieves the count of online characters from the characters database.
		/// </summary>
		/// <remarks>
		/// This method updates the <see cref="CardServerStatistics.Online"/> property with the retrieved count.
		/// </remarks>
		public static void GetOnlineCharacters()
		{
			Database._construct();
			using (MySqlConnection connection = Database.GetConnection(ConnectionStrings.characters))
			{
				Database.Open(connection);
				String Query = "SELECT COUNT(*) FROM `characters` WHERE online = 1";

				using (MySqlCommand command = new MySqlCommand(Query, connection))
				{
					CardServerStatistics.online = Convert.ToInt32(command.ExecuteScalar());
				}
			}
		}

#region CardPvPData Operation
		/// <summary>
		/// Retrieves the top 5 player card data based on the total kills from the characters database.
		/// </summary>
		/// <remarks>
		/// This method populates the <see cref="CardPvPData.data"/> list with the retrieved player card data.
		/// </remarks>
		public static void GetTopPlayerCard()
		{
			Database._construct();

			CardPvPData.data = new List<CardPvPData>();

			using (MySqlConnection connection = Database.GetConnection(ConnectionStrings.characters))
			{
				String Query = "SELECT name, class, level, totalKills FROM `characters` ORDER BY totalKills DESC LIMIT 5";

				using (MySqlCommand command = new MySqlCommand(Query, connection))
				{
					Database.Open(connection);

					using (MySqlDataReader _reader = command.ExecuteReader())
					{
						while (_reader.Read())
						{
#pragma warning disable CS8601 // Possible null reference assignment.
							CardPvPData temp = new CardPvPData
							{
								Name = _reader["name"].ToString(),
								Class = Convert.ToInt32(_reader["class"]),
								Level = Convert.ToInt32(_reader["level"]),
								Kills = Convert.ToInt32(_reader["totalKills"])
							};
#pragma warning restore CS8601 // Possible null reference assignment.

							CardPvPData.data.Add(temp);
						}
					}
				}
			}
		}
#endregion

#region OnlinePlayerCharacterData Operation

		/// <summary>
		/// Retrieves a list of online player character data from the characters database.
		/// </summary>
		/// <returns>
		/// A list of <see cref="OnlinePlayerCharacterData"/> representing online player characters.
		/// If no online players are found, the method returns an empty list.
		/// </returns>
		public static List<OnlinePlayerCharacterData> GetOnlinePlayer()
		{
			// Create a temporary list to store the retrieved data
			List<OnlinePlayerCharacterData> temp = new List<OnlinePlayerCharacterData>();

			// Establish a connection to the characters database
			using (MySqlConnection connection = Database.GetConnection(ConnectionStrings.characters))
			{
				// Open the database connection
				Database.Open(connection);

				// Query online player character data from the characters table
				var data = connection.Query<OnlinePlayerCharacterData>("SELECT name, race, class, gender, level FROM `characters` WHERE online = 1 ORDER BY level DESC LIMIT 49");

				// Check if data is not null and contains any elements
				if (data != null && data.Any())
					temp.AddRange(data); // Add the retrieved data to the temporary list

				// Close the database connection
				Database.Close(connection);
			}

#pragma warning disable CS8603 // Possible null reference return.
#if DEBUG
			return temp.Count > 0 ? temp : null;
#else
			// Return the temporary list of online player character data
			return temp;
#endif
#pragma warning restore CS8603 // Possible null reference return.
		}

#endregion

#endregion
	}

	#region OnlinePlayerCharacterData
	/// <summary>
	/// Class representing online player character data.
	/// </summary>
	public class OnlinePlayerCharacterData : Characters
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		/// <summary>
		/// Gets or sets the name of the online player character.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the race ID of the online player character.
		/// </summary>
		public int Race { get; set; }

		/// <summary>
		/// Gets or sets the class ID of the online player character.
		/// </summary>
		public int Class { get; set; }

		/// <summary>
		/// Gets or sets the gender ID of the online player character.
		/// </summary>
		public int Gender { get; set; }

		/// <summary>
		/// Gets or sets the level of the online player character.
		/// </summary>
		public int Level { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	}
#endregion

#region CardPvPData
	/// <summary>
	/// Represents player card data used in PvP scenarios.
	/// </summary>
	public class CardPvPData : Characters
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		/// <summary>
		/// Gets or sets the list of CardPvPData instances.
		/// </summary>
		public static List<CardPvPData> data { get; set; }

		/// <summary>
		/// Gets or sets the player's name.
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// Gets or sets the player's class.
		/// </summary>
		public int Class { get; set; }

		/// <summary>
		/// Gets or sets the player's level.
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// Gets or sets the total kills achieved by the player.
		/// </summary>
		public int Kills { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	}
#endregion
}
