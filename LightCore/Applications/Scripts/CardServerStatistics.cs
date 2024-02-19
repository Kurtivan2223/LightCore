namespace LightCore.Applications.Scripts
{
#region CardServerStatistics
	/// <summary>
	/// Represents statistics related to the Card Server application.
	/// </summary>
	public class CardServerStatistics
	{
		/// <summary>
		/// Gets or sets the total number of accounts.
		/// </summary>
		public static int accounts { get; set; }

		/// <summary>
		/// Gets or sets the total number of characters.
		/// </summary>
		public static int characters { get; set; }

		/// <summary>
		/// Gets or sets the number of currently online users.
		/// </summary>
		public static int online { get; set; }
	}
#endregion
}