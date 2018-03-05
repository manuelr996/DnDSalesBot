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
			catch(SQLiteException e)
			{
				connection.Close();
				throw new Exception(e.Message);
			}

			return rtn;
        }

		public SQLiteDataReader GetPlayer(int playerDescriptor)
		{
			SQLiteDataReader rtn;
			SQLiteParameter param = new SQLiteParameter("@playerDescriptor");

			param.Value = playerDescriptor;

			string sql = "SELECT * FROM Players WHERE playerDescriptor LIKE @playerDescriptor";

			SQLiteCommand command = new SQLiteCommand(sql, connection);
			command.Parameters.Add(param);

			try
			{
				connection.Open();
				rtn = command.ExecuteReader();
			}
			catch(SQLiteException e)
			{
				connection.Close();
				throw new Exception(e.Message);
			}

			return rtn;
		}
	}
}
