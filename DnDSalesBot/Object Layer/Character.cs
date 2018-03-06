using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using DnDSalesBot.Data_Access_Layer;

namespace DnDSalesBot.Object_Layer
{
    class Character
    {
		public string Name { get; set; }

		public int Level { get; set; }

		public double CurrentGold { get; set; }

		public Character()
		{
			Name = string.Empty;
			Level = 0;
			CurrentGold = 0;
		}
    }
}
