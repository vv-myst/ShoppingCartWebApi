#region Auto generated information. Please do not modify

// ShoppingCartWebApi ShoppingCartWebApi Item.cs
// bila007 Bilangi, Vivek-Vardhan
// 2018-02-15 11:06 
// 2018-02-15 11:03 

#endregion

namespace ShoppingCartWebApi.Models
{
    public class Item
    {
        public Item(int id, string name, decimal value, int inventoryCount,
                    string description = "")
        {
            Id = id;
            Name = name;
            Description = description;
            Value = value;
            InventoryCount = inventoryCount;
        }

        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Value { get; }
        public int InventoryCount { get; }
    }
}
