using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WebChatRoom.Models;

namespace WebChatRoom.Pages
{
    public class RoomModel : PageModel
    {
        private const string ROOMCACHEKEY = "RoomCacheKey";
        private readonly ILogger<RoomModel> _logger;
        private readonly IMemoryCache _memoryCache;

        [BindProperty]
        public Room Room { get; set; }

        [BindProperty]
        public Participant Participant { get; set; }



        public RoomModel(IMemoryCache memoryCache, ILogger<RoomModel> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;


        }
        public void OnGet(Guid roomId, Guid participantId)
        {
            var rooms = _memoryCache.Get<List<Room>>(ROOMCACHEKEY);
            if (rooms == null)
            {
                this.Room = new Room() { Participants = new List<Participant>() };
                return;
            }
            this.Room = rooms.FirstOrDefault(r => r.Id == roomId);
            if (this.Room != null)
            {
                this.Participant = this.Room.Participants.FirstOrDefault(p => p.Id == participantId);
            }
        }
    }
}