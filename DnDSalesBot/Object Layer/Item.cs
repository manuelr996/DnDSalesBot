using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using DnDSalesBot.Data_Access_Layer;

namespace DnDSalesBot.Object_Layer
{
    class Item
    {
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        
        #region Constructors
        public Item(string name, double price = 0)
        {
            ItemName = name;
            ItemPrice = price;
        }

        public Item()
        {
            ItemName = string.Empty;
            ItemPrice = 0;
        }

        public Item(Item copyItem)
        {
            ItemName = copyItem.ItemName;
            ItemPrice = copyItem.ItemPrice;
        }
        #endregion

        public static Item GetFromDatabase(string itemName)
        {
            Item result = new Item();
            DatabaseHandler db = new DatabaseHandler();
            SQLiteDataReader values = db.GetItemPrice(itemName);

            if (values.HasRows && values.Read())
            {
                result.ItemName = itemName;
                result.ItemPrice = ((double)values["itemPrice"]);
            }
            else
                result = null;

            if (!values.IsClosed)
                values.Close();
            
            return result;
        }
    }
}
