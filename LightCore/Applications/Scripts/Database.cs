using MySql.Data.MySqlClient;
using System.Data;

namespace LightCore.Applications.Scripts
{
	/// <summary>
	/// Provides database-related operations.
	/// </summary>
	public class Database
	{
#region Constructor

		/// <summary>
		/// Initializes connection strings for the admin, authentication, characters, and world databases.
		/// </summary>
		public static void _construct()
		{
			ConnectionStrings.admin = System.Configuration.ConfigurationManager.ConnectionStrings["admin"].ConnectionString;
			ConnectionStrings.auth = System.Configuration.ConfigurationManager.ConnectionStrings["auth"].ConnectionString;
			ConnectionStrings.characters = System.Configuration.ConfigurationManager.ConnectionStrings["characters"].ConnectionString;
			ConnectionStrings.world = System.Configuration.ConfigurationManager.ConnectionStrings["world"].ConnectionString;
		}
#endregion
#region Operations

		/// <summary>
		/// Creates a new MySqlConnection instance using the provided connection string.
		/// </summary>
		/// <param name="_s">The connection string.</param>
		/// <returns>A new MySqlConnection instance.</returns>
		public static MySqlConnection GetConnection(String _s) { return new  MySqlConnection(_s); }

		/// <summary>
		/// Opens the specified MySqlConnection if it is in a closed state.
		/// </summary>
		/// <param name="_c">The MySqlConnection to open.</param>
		public static void Open(MySqlConnection _c)
		{
			if (_c.State == ConnectionState.Closed)
				_c.Open();
		}

		/// <summary>
		/// Closes the specified MySqlConnection if it is in an open state.
		/// </summary>
		/// <param name="_c">The MySqlConnection to close.</param>
		public static void Close(MySqlConnection _c)
		{
			if (_c.State == ConnectionState.Open)
				_c.Close();
		}
#endregion
	}

	/// <summary>
	/// Holds connection strings for the admin, authentication, characters, and world databases.
	/// </summary>
	public class ConnectionStrings : Database
	{
#region Connection String
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		/// <summary>
		/// Gets or sets the connection string for the admin database.
		/// </summary>
		public static String admin { get; set; }

		/// <summary>
		/// Gets or sets the connection string for the authentication database.
		/// </summary>
		public static String auth { get; set; }

		/// <summary>
		/// Gets or sets the connection string for the characters database.
		/// </summary>
		public static String characters { get; set; }

		/// <summary>
		/// Gets or sets the connection string for the world database.
		/// </summary>
		public static String world { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		#endregion
	}
}
