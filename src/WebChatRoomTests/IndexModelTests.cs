using Xunit;
using WebChatRoom.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using WebChatRoom.Pages;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using WebChatRoom.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace WebChatRoom.Pages.Tests
{
    public class IndexModelTests
    {
        [Fact()]
        public void OnGetTest_GetCahedValue()
        {
            // Assign 
            const string ROOMCACHEKEY = "RoomCacheKey";
            const string ROOMNAME = "RoomCacheKey";
            var resultRoomList = new List<Room>() { new Room() { Name= ROOMNAME } };
            Mock<IMemoryCache> memoryCacheMoq = new Mock<IMemoryCache>();
            memoryCacheMoq.Setup(m => m.TryGetValue(It.IsAny<object>(), out resultRoomList)).Returns(true);

            // act 
            var indexModel = new IndexModel(memoryCacheMoq.Object, NullLogger<IndexModel>.Instance);
            indexModel.OnGet();

            // assert
            Assert.Single(indexModel.Rooms);
            Assert.Equal(ROOMNAME,indexModel.Rooms[0].Name);

        }
    }
}