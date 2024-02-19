using MySql.Data.MySqlClient;

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

			using (MySqlConnection _connection = Database.GetConnection(ConnectionStrings.characters))
			{
				String _query = "SELECT name, class, level, totalKills FROM `characters` ORDER BY totalKills DESC LIMIT 5";

				using (MySqlCommand _command = new MySqlCommand(_query, _connection))
				{
					Database.Open(_connection);

					using (MySqlDataReader _reader = _command.ExecuteReader())
					{
						while (_reader.Read())
						{
#pragma warning disable CS8601 // Possible null reference assignment.
							CardPvPData _tmp = new CardPvPData
							{
								Name = _reader["name"].ToString(),
								Class = Convert.ToInt32(_reader["class"]),
								Level = Convert.ToInt32(_reader["level"]),
								Kills = Convert.ToInt32(_reader["totalKills"])
							};
#pragma warning restore CS8601 // Possible null reference assignment.

							CardPvPData.data.Add(_tmp);
						}
					}
				}
			}
		}
#endregion
#endregion
	}

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
