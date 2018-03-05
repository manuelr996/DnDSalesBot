using System;
using System.Data;
using System.Data.SQLite;
using System.Configuration;
using System.Collections.Generic;
using System.Text;




namespace DnDSalesBot.Data_Access_Layer
{
	class DatabaseHandler
	{
		private SQLiteConnection connection;

		public DatabaseHandler()
		{
			connection = new SQLiteConnection(ConfigurationManager.AppSettings["connectionString"]);
		}

		#region Item
		public SQLiteDataReader GetItemPrice(string itemName)
		{
			SQLiteDataReader rtn;
			SQLiteParameter param = new SQLiteParameter("@itemName");

			param.Value = itemName;

			string sql = "SELECT itemPrice FROM Items where itemName like @itemName;";
			SQLiteCommand command = new SQLiteCommand(sql, connection);
			command.Parameters.Add(param);

			try
			{
				connection.Open();
				rtn = command.ExecuteReader();
			}
			catch (SQLiteException e)
			{
				connection.Close();
				throw new Exception(e.Message);
			}

			return rtn;
		}

		public bool InsertItem(string itemName, double itemPrice)
		{
			SQLiteParameter name = new SQLiteParameter("@itemName");
			SQLiteParameter price = new SQLiteParameter("@itemPrice");
			bool result;

			name.Value = itemName;
			price.Value = itemPrice;

			string sql = "INSERT INTO items (itemName, itemPrice) VALUES (@itemName, @itemPrice)";

			SQLiteCommand command = new SQLiteCommand(sql, connection);
			command.Parameters.Add(name);
			command.Parameters.Add(price);

			try
			{
				connection.Open();
				result = (command.ExecuteNonQuery() >= 1 ? true : false);
			}
			catch (SQLiteException e)
			{
				connection.Close();
				throw new Exception(e.Message);
			}

			return result;
		}
		#endregion

		#region Player
		public SQLiteDataReader GetPlayer(int playerDiscriminator)
		{
			SQLiteDataReader rtn;
			SQLiteParameter param = new SQLiteParameter("@playerDiscriminator");

			param.Value = playerDiscriminator;

			string sql = "SELECT * FROM Players WHERE playerDiscriminator LIKE @playerDiscriminator";

			SQLiteCommand command = new SQLiteCommand(sql, connection);
			command.Parameters.Add(param);

			try
			{
				connection.Open();
				rtn = command.ExecuteReader();
			}
			catch (SQLiteException e)
			{
				connection.Close();
				throw new Exception(e.Message);
			}

			return rtn;
		}

		public bool InsertPlayer(string playerName, string characterName, ushort playerDiscriminator, ulong journalId)
		{
			bool result;

			SQLiteParameter plrName = new SQLiteParameter("@playerName") { Value = playerName };
			SQLiteParameter chrName = new SQLiteParameter("@characterName") { Value = characterName };
			SQLiteParameter discriminator = new SQLiteParameter("@playerDiscriminator") { Value = Convert.ToInt32(playerDiscriminator) };
			SQLiteParameter jrnlId = new SQLiteParameter("@journalId") { Value = journalId.ToString() };


			string sql = "INSERT INTO Players (playerName, characterName, playerDiscriminator, isDm, journalId)" +
							"VALUES (@playerName , @characterName, @playerDiscriminator, 0, @journalId);";

			SQLiteCommand command = new SQLiteCommand(sql, connection);
			command.Parameters.Add(plrName);
			command.Parameters.Add(chrName);
			command.Parameters.Add(discriminator);
			command.Parameters.Add(jrnlId);

			try
			{
				connection.Open();
				result = ((command.ExecuteNonQuery()) >= 1 ? true : false);
			}
			catch (SQLiteException e)
			{
				connection.Close();
				throw new Exception(e.Message);
			}

			return result;
		}

		public bool UpdateDM(bool isDm, ushort playerDiscriminator)
		{
			bool result;
			SQLiteParameter dmFlag = new SQLiteParameter("@isDm");
			SQLiteParameter discriminator = new SQLiteParameter("@playerDiscriminator");
			dmFlag.Value = isDm;
			discriminator.Value = Convert.ToInt32(playerDiscriminator);

			string sql = "Update Players Set isDm = @isDm where playerDiscriminator like @playerDiscriminator";

			SQLiteCommand command = new SQLiteCommand(sql, connection);

			try
			{
				connection.Open();
				result = (command.ExecuteNonQuery() >= 1 ? true : false);
			}
			catch(SQLiteException e)
			{
				connection.Close();
				throw new Exception(e.Message);
			}

			return result;
		}
		#endregion
	}
}
