using Microsoft.EntityFrameworkCore;
using RPG.DAL;
using RPG.DAL.Models.Items;


namespace RPGBot.Core.Services.Items
{
    public interface IItemService
    {
        Task<Item> GetItemByName(string itemsName);
    }
    public class ItemService : IItemService
    {
        private readonly RPGContext _context;

        public ItemService(RPGContext context)
        {
            _context = context;
        }

        public async Task<Item> GetItemByName(string itemName)
        {
            itemName = itemName.ToLower();
            return await _context.Item.FirstOrDefaultAsync(x => x.Name.ToLower() == itemName);
        }
    }
}
