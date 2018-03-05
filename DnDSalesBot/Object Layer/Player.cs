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
        public string Name { get; set; }

        public ushort Discriminator { get; set; }

        public ulong JournalId { get; set; }

        public Character Character { get; set; }

        public bool IsDm { get; set; }
		#endregion

		public Player()
		{
			Name = string.Empty;
			Discriminator = 0;
			JournalId = 0;
			Character = new Character();
		}

		public static Player GetFromDatabase(ushort descriptor)
		{
			Player result = new Player();
			DatabaseHandler db = new DatabaseHandler();
			SQLiteDataReader values = db.GetPlayer(descriptor);

			if (values.HasRows && values.Read())
			{
				result.Character.Name = ((string)values["characterName"]);
				result.Name = ((string)values["playerName"]);
				result.JournalId = ulong.Parse((string) values["journalId"]);
				result.Discriminator = Convert.ToUInt16((values["playerDiscriminator"]));
                result.IsDm = ((bool)values["isDM"]);
			}
			else
				result = null;

			if (!values.IsClosed)
				values.Close();

			return result;
		}

		public static bool AddToDatabase(Player newPlayer)
		{
			DatabaseHandler db = new DatabaseHandler();
			bool result = db.InsertPlayer(newPlayer.Name, newPlayer.Character.Name, newPlayer.Discriminator, newPlayer.JournalId);

			return result;
		}
	}
}
