using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Routing;
using PokerTime.API.Controllers;
using PokerTime.API.Data;
using System;
using Xunit;

namespace PokerTime.Testing
{
    public class UserTesting
    {
        [Fact]
        public void GetUsers_Returns_The_Correct_Number_Of_Users()
        {
            //Arrange
            var dataStore = A.Fake<IPTRepository>();
            var mapper = A.Fake<IMapper>();
            var linkGenerator = A.Fake<LinkGenerator>();
            var controller = new HostsController(dataStore, mapper, linkGenerator);

            //Act


            //Assert

        }
    }
}
