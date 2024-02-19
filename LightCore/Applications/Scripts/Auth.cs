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

#region Registration

		/// <summary>
		/// Placeholder method for user registration.
		/// </summary>
		/// <remarks>
		/// This method is intended for handling user registration logic.
		/// </remarks>
		public static void Register()
		{
			// ToDo: Implement user registration logic here.
		}
#endregion
#endregion
	}
}