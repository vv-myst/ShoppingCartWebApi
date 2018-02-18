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

        /// <inheritdoc />
        /// <summary>
        ///     Inventory id of the item
        /// </summary>
        public int Id { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Name of the item
        /// </summary>
        public string Name { get; }

        /// <inheritdoc />
        /// <summary>
        ///     A short description about the item
        /// </summary>
        public string Description { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Cost of the item (in EUR)
        /// </summary>
        public decimal Value { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Quantity of items in the inventory
        /// </summary>
        public int InventoryCount { get; }
    }
}