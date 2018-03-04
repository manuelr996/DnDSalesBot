using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using DnDSalesBot.Data_Access_Layer;

namespace DnDSalesBot.Object_Layer
{
    class Item
    {
        public string itemName { get; set; }
        public decimal itemPrice { get; set; }
        
        #region Constructors
        public Item(string name, decimal price = 0)
        {
            itemName = name;
            itemPrice = price;
        }

        public Item()
        {
            itemName = string.Empty;
            itemPrice = 0;
        }

        public Item(Item copyItem)
        {
            itemName = copyItem.itemName;
            itemPrice = copyItem.itemPrice;
        }
        #endregion

        public static Item GetFromDatabase(string itemName)
        {
            Item result = new Item();
            DatabaseHandler db = new DatabaseHandler();
            SQLiteDataReader values = db.GetItemPrice(itemName);

            if (values.HasRows && values.Read())
            {
                result.itemName = itemName;
                result.itemPrice = ((decimal)values["itemPrice"]);
            }
            else
                result = null;

            if (!values.IsClosed)
                values.Close();
            
            return result;
        }
    }
}
