using System.Security.Cryptography;
using System.Numerics;
using System.Text;

namespace LightCore.Applications.Scripts
{
	/// <summary>
	/// Provides utility methods for various functionalities.
	/// </summary>
	public class Utils
	{
#region Date and Time
		/// <summary>
		/// Gets or sets the current year.
		/// </summary>
		public static int year { get; set; }

		/// <summary>
		/// Gets or sets the current month.
		/// </summary>
		public static int month { get; set; }

		/// <summary>
		/// Gets or sets the current day.
		/// </summary>
		public static int day { get; set; }

		/// <summary>
		/// Updates the static properties <see cref="Year"/>, <see cref="Month"/>, and <see cref="Day"/> with the current date.
		/// </summary>
		public static void Date()
		{
			DateTime date = DateTime.Now;
			year = date.Year;
			month = date.Month;
			day = date.Day;
		}

		/// <summary>
		/// Converts a duration in seconds to a human-readable time format (days:hours:minutes).
		/// </summary>
		/// <param name="seconds">The duration in seconds.</param>
		/// <returns>A string representation of the duration in days, hours, and minutes.</returns>
		public static String GetHumanTimeFromSeconds(int seconds)
		{
			// Convert the duration in seconds to a TimeSpan object
			TimeSpan interval = TimeSpan.FromSeconds(seconds);

			// Get the current UTC time
			DateTime now = DateTime.UtcNow;

			// Calculate the future time by adding the duration to the current time
			DateTime futureTime = now.Add(interval);

			// Calculate the difference between the future time and the current time
			TimeSpan difference = futureTime - now;

			// Format the result as a string in the "days:hours:minutes" format and return
			return $"{(int)difference.TotalDays}:{difference.Hours}:{difference.Minutes}";
		}
#endregion
#region Character Stuff

		/// <summary>
		/// Gets the class name based on the provided class ID.
		/// </summary>
		/// <param name="_c">The class ID.</param>
		/// <returns>The corresponding class name as a string.</returns>
		public static String GetClassName(int _c)
		{
			switch (_c)
			{
				case 1:
					return "WARRIOR";
				case 2:
					return "PALADIN";
				case 3:
					return "HUNTER";
				case 4:
					return "ROGUE";
				case 5:
					return "PRIEST";
				case 6:
					return "DEATHKNIGHT";
				case 7:
					return "SHAMAN";
				case 8:
					return "MAGE";
				case 9:
					return "WARLOCK";
				case 11:
					return "DRUID";
			}
			return "";
		}

		/// <summary>
		/// Gets the race name based on the provided race ID.
		/// </summary>
		/// <param name="_r">The race ID.</param>
		/// <returns>The corresponding race name as a string.</returns>
		public static String GetRaceName(int _r)
		{
			switch (_r)
			{
				case 1:
					return "HUMAN";
				case 2:
					return "ORC";
				case 3:
					return "DWARF";
				case 4:
					return "NIGHTELF";
				case 5:
					return "SCOURGE";
				case 6:
					return "TAUREN";
				case 7:
					return "GNOME";
				case 8:
					return "TROLL";
				case 10:
					return "BLOODELF";
				case 11:
					return "DRAENEI";
			}
			return "";
		}
#endregion

		/// <summary>
		/// Generates a random string composed of alphanumeric characters.
		/// </summary>
		/// <param name="length">The length of the random string. Default is 10.</param>
		/// <returns>A randomly generated string.</returns>
		public static string GenerateRandomString(int length = 10)
		{
			// The set of characters from which the random string will be generated
			const string characters = "0123456789abcdefghijklmnopqrstuvwxyz";

			// The total number of characters in the character set
			int charactersLength = characters.Length;

			// Initialize a random number generator
			Random random = new Random();

			// Create an array to store the characters of the random string
			char[] randomString = new char[length];

			// Populate the array with random characters
			for (int i = 0; i < length; i++)
			{
				randomString[i] = characters[random.Next(0, charactersLength)];
			}

			// Convert the character array to a string and return the result
			return new string(randomString);
		}
	}

	/// <summary>
	/// Provides methods related to the Secure Remote Password (SRP-6) protocol for cryptography.
	/// </summary>
	public class SRPCypto : Utils
	{
#region Crypto For Login and Register
		/// <summary>
		/// The generator constant 'g' for SRP-6.
		/// </summary>
		private static BigInteger g = 7;

		/// <summary>
		/// The modulus constant 'N' for SRP-6, parsed from hexadecimal representation.
		/// </summary>
		private static BigInteger N = BigInteger.Parse("894B645E89E1535BBDAD5B8B290650530801B18EBFBF5E8FAB3C82872A3E9BB7", System.Globalization.NumberStyles.HexNumber);

		/// <summary>
		/// Calculates the SRP-6 verifier based on the provided username, password, and salt.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="salt">The salt value.</param>
		/// <returns>The SRP-6 verifier as a byte array.</returns>
		public static byte[] CalculateSRP6Verifier(String username, String password, byte[] salt)
		{
			// Step 1: Calculate h1 = SHA-1(username:password)
			byte[] h1 = SHA1(Encoding.UTF8.GetBytes(username.ToUpper() + ":" + password));

			// Step 2: Calculate h2 = SHA-1(salt || h1)
			byte[] h2 = SHA1(salt.Concat(h1).ToArray());

			// Convert h2 to a BigInteger
			BigInteger h2Integer = new BigInteger(h2.Reverse().ToArray());

			// Step 3: Calculate the verifier = g^h2 mod N
			BigInteger verifier = BigInteger.ModPow(g, h2Integer, N);

			// Convert verifier to bytes, reverse, and resize to 32 bytes
			byte[] verifierBytes = verifier.ToByteArray().Reverse().ToArray();
			Array.Resize(ref verifierBytes, 32);

			return verifierBytes;
		}

		/// <summary>
		/// Generates random salt and calculates the SRP-6 verifier for user registration.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns>A tuple containing the salt and verifier as strings.</returns>
		public static Tuple<string, string> GetRegistrationData(string username, string password)
		{
			// Generate random salt
			byte[] salt = GenerateRandomBytes(32); // <-- diara imuhang asin art

			// Calculate verifier based on username, password, and salt
			byte[] verifier = CalculateSRP6Verifier(username, password, salt);

			return Tuple.Create(Encoding.UTF8.GetString(salt), Encoding.UTF8.GetString(verifier));
		}

		/// <summary>
		/// Verifies SRP-6 using the provided username, password, salt, and verifier.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="salt">The salt value.</param>
		/// <param name="verifier">The verifier value.</param>
		/// <returns>True if verification is successful, otherwise false.</returns>
		public static bool VerifySRP6(string username, string password, byte[] salt, byte[] verifier)
		{
			// Step 1: Calculate x = SHA-1(salt || SHA-1(username:password))
			BigInteger x = new BigInteger(SHA1(salt.Concat(SHA1(Encoding.UTF8.GetBytes(username.ToUpper() + ':' + password))).ToArray()).Reverse().ToArray(), true);

			// Step 2: Calculate v = g^x mod N
			BigInteger v = BigInteger.ModPow(g, x, N);

			// Convert v to bytes, reverse, and resize to 32 bytes
			byte[] vBytes = v.ToByteArray().Reverse().ToArray();
			Array.Resize(ref vBytes, 32);

			// Step 3: Verify if calculated verifier matches the provided verifier
			return verifier.SequenceEqual(vBytes);
		}

		/// <summary>
		/// Calculates the SHA-1 hash of input bytes using a deprecated SHA1Managed class.
		/// </summary>
		/// <param name="input">The input bytes.</param>
		/// <returns>The SHA-1 hash as a byte array.</returns>
		private static byte[] SHA1(byte[] input)
		{
#pragma warning disable SYSLIB0021 // Type or member is obsolete
			using (SHA1Managed sha1 = new SHA1Managed())
			{
				return sha1.ComputeHash(input);
			}
#pragma warning restore SYSLIB0021 // Type or member is obsolete
		}

		/// <summary>
		/// Generates an array of random bytes of the specified length.
		/// </summary>
		/// <param name="length">The length of the random byte array.</param>
		/// <returns>The generated random byte array.</returns>
		private static byte[] GenerateRandomBytes(int length)
		{
			using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
			{
				byte[] data = new byte[length];
				rng.GetBytes(data);
				return data;
			}
		}
		#endregion
	}
}
