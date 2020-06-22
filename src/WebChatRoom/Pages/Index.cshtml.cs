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

        public IndexModel(IMemoryCache memoryCache, ILogger<IndexModel> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public void OnGet()
        {
            Rooms = _memoryCache.Get<List<Room>>(ROOMCACHEKEY);
            if (Rooms == null)
            {
                Rooms = new List<Room>();
                SetRoomCache(Rooms);
            }
        }

        public IActionResult OnPostAddRoom(string roomName)
        {
            if (ModelState.IsValid)
            {
                // Get rooms
                this.Rooms = _memoryCache.Get<List<Room>>(ROOMCACHEKEY);
                if (this.Rooms.Count > 9) return Page(); // limit to 10 rooms only

                this.Rooms.Add(new Room() { Id = Guid.NewGuid(), Name = roomName, Participants = new List<Participant>() });
                SetRoomCache(this.Rooms);
            }
            return Page();
        }

        public IActionResult OnPostJoinRoom(Guid roomId, string Name)
        {
            if (ModelState.IsValid)
            {
                // Get rooms
                this.Rooms = _memoryCache.Get<List<Room>>(ROOMCACHEKEY);
                var theRoom = this.Rooms.FirstOrDefault(r => r.Id == roomId);
                if (theRoom != null)
                {
                    if (theRoom.Participants.Count > 9) return Page(); // limit to 10 partisipants
                    var participantId = Guid.NewGuid();
                    theRoom.Participants.Add(new Participant() { Id = participantId, Name = Name });
                    SetRoomCache(this.Rooms);
                    return new RedirectToPageResult("Room",new { roomId, participantId });
                }
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
