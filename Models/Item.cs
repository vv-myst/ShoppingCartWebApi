using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApi.Models
{
    public class Item : IItem
    {
        /// <inheritdoc />
        /// <summary>
        ///     Inventory id of the item
        /// </summary>
        public int Id { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Name of the item
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     A short description about the item
        /// </summary>
        public string Description { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Cost of the item (in EUR)
        /// </summary>
        public decimal Value { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Quantity of items in the inventory
        /// </summary>
        public int InventoryCount { get; set; }
    }
}