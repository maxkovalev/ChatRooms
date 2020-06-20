using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WebChatRoom.Models;

namespace WebChatRoom.Pages
{
    public class IndexModel : PageModel
    {
        private const string ROOMCACHEKEY = "RoomCacheKey";
        private readonly ILogger<IndexModel> _logger;
        private readonly IMemoryCache _memoryCache;

        [BindProperty]
        public List<Room> Rooms { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Room Name")]
            public string RoomName { get; set; }
        }

        public IndexModel(IMemoryCache memoryCache, ILogger<IndexModel> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public void OnGet()
        {
            if(!_memoryCache.TryGetValue(ROOMCACHEKEY,out List<Room> Rooms))
            {
                this.Rooms = new List<Room>();
                SetRoomCache(this.Rooms);
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                // Get rooms
                this.Rooms = _memoryCache.Get<List<Room>>(ROOMCACHEKEY);
                this.Rooms.Add(new Room() { Name = Input.RoomName });
                SetRoomCache(this.Rooms);
                this.Input.RoomName = ""; //rest room name
            }
            return Page();
        }

        private void SetRoomCache(List<Room> rooms)
        {
            var cacheExpirationOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(5),
                Priority = CacheItemPriority.Normal,
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };
            _memoryCache.Set(ROOMCACHEKEY, Rooms, cacheExpirationOptions);
        }
    }
}
