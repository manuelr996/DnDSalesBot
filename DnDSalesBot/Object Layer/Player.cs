using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using DnDSalesBot.Data_Access_Layer;

namespace DnDSalesBot.Object_Layer
{
    class Player
    {
		#region Properties
		public string PlayerName { get; set; }

		public int PlayerDescriptor { get; set; }

		public ulong PlayerJournalId { get; set; }

		public Character Character { get; set; }
		#endregion

		public Player()
		{
			PlayerName = string.Empty;
			PlayerDescriptor = 0;
			PlayerJournalId = 0;
			Character = new Character();
		}

		public static Player GetFromDatabase(int descriptor)
		{
			Player result = new Player();
			DatabaseHandler db = new DatabaseHandler();
			SQLiteDataReader values = db.GetPlayer(descriptor);

			if (values.HasRows && values.Read())
			{
				result.Character.Name = ((string)values["characterName"]);
				result.PlayerName = ((string)values["playerName"]);
				result.PlayerJournalId = ulong.Parse((string) values["journalId"]);
				result.PlayerDescriptor = ((int) values["playerDescriptor"]);
			}
			else
				result = null;

			if (!values.IsClosed)
				values.Close();

			return result;
		}
	}
}
