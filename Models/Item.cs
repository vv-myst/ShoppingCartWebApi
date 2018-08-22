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

        /// <summary>
        ///     Inventory id of the item
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Name of the item
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     A short description about the item
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Cost of the item (in EUR)
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        ///     Quantity of items in the inventory
        /// </summary>
        public int InventoryCount { get; }
    }
}