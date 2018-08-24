namespace ShoppingCartWebApi.Models.Interfaces
{
    public interface IItem
    {
        /// <summary>
        ///     Inventory id of the item
        /// </summary>
        int Id { get; }

        /// <summary>
        ///     Name of the item
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     A short description about the item
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Cost of the item (in EUR)
        /// </summary>
        decimal Value { get; set; }

        /// <summary>
        ///     Quantity of items in the inventory
        /// </summary>
        int InventoryCount { get; }
    }
}