using MySql.Data.MySqlClient;

namespace LightCore.Applications.Scripts
{
	/// <summary>
	/// Provides Operation method for Auth Database.
	/// </summary>
	public class Auth
	{
#region Operations
		/// <summary>
		/// Retrieves the total number of accounts from the authentication database.
		/// </summary>
		/// <remarks>
		/// This method updates the <see cref="CardServerStatistics.Accounts"/> property with the retrieved count.
		/// </remarks>
		public static void GetAccountCount()
		{
			// Initialize and configure the database connection
			Database._construct();

			// Establish a connection to the authentication database
			using (MySqlConnection _connection = Database.GetConnection(ConnectionStrings.auth))
			{
				// Open the database connection
				Database.Open(_connection);

				// SQL query to retrieve the count of accounts
				String _query = "SELECT COUNT(*) FROM `account`";

				// Execute the query and update the CardServerStatistics.accounts property
				using (MySqlCommand _command = new MySqlCommand(_query, _connection))
				{
					CardServerStatistics.accounts = Convert.ToInt32(_command.ExecuteScalar());
				}
			}
		}

#region Bool Checks

		/// <summary>
		/// Checks if an email already exists in the authentication database.
		/// </summary>
		/// <param name="email">The email to check.</param>
		/// <returns>True if the email does not exist, otherwise false.</returns>
		public static bool CheckEmailExists(String email)
		{
			if(!string.IsNullOrEmpty(email))
			{
				using (MySqlConnection connection = Database.GetConnection(ConnectionStrings.auth))
				{
					Database.Open(connection);

					using (MySqlCommand command = new MySqlCommand("SELECT id FROM `account` WHERE email = UPPER(@email)", connection))
					{
						command.Parameters.AddWithValue("@email", email);

						using(MySqlDataReader reader = command.ExecuteReader())
						{
							if(!reader.Read())
							{
								// No results found, email does not exist
								Database.Close(connection);
								return true;
							}
						}
					}
					Database.Close(connection);
				}
			}

			return false;
		}

		/// <summary>
		/// Checks if a username already exists in the authentication database.
		/// </summary>
		/// <param name="username">The username to check.</param>
		/// <returns>True if the username does not exist, otherwise false.</returns>
		public static bool CheckUsernameExists(String username)
		{
			if (!string.IsNullOrEmpty(username))
			{
				using (MySqlConnection connection = Database.GetConnection(ConnectionStrings.auth))
				{
					Database.Open(connection);

					using (MySqlCommand command = new MySqlCommand("SELECT id FROM `account` WHERE username = UPPER(@username)", connection))
					{
						command.Parameters.AddWithValue("@username", username);

						using (MySqlDataReader reader = command.ExecuteReader())
						{
							if (!reader.Read())
							{
								// No results found, username does not exist
								Database.Close(connection);
								return true;
							}
						}
					}
					Database.Close(connection);
				}
			}

			return false;
		}
#endregion
#endregion
	}
}