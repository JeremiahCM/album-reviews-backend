using AlbumReviewsAPI.Controllers;
using AlbumReviewsAPI.Data;
using AlbumReviewsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

// reference: https://chaitanyasuvarna.wordpress.com/2020/09/06/nunit-test-for-httpclient-moq/

namespace AlbumReviewsUnitTesting
{
    /// <summary>
    /// Class for testing the AlbumReviewAPI
    /// Contains a single test for checking that all album review are retrieved when calling ' GetAllAlbumReviews() '
    /// 
    /// Utilizes mock database with test data
    /// </summary>
    [TestFixture]
    public class Tests
    {
        private DeezerService deezerService;
        private Mock<HttpMessageHandler>? httpMessageHandlerMock;

        [SetUp]
        public void Setup()
        {
            httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            HttpClient httpClient = new HttpClient(httpMessageHandlerMock.Object);
            deezerService = new DeezerService(httpClient);

        }

        /// <summary>
        /// Test if all album reviews are being retrieved with the get all request
        /// 
        /// Passes if the number of reviews in the database matches the number of reviews retrieved by the API operation
        /// </summary>
        [Test]
        public void GetAll_ReturnsAllReviews()
        {
            //Arrange - Set up requirements for the test
            var options = new DbContextOptionsBuilder<AlbumReviewsAPIDbContext>()
                .UseInMemoryDatabase(databaseName: "AlbumReviewsDb")
                .Options;
            
            using (var dbContext = new AlbumReviewsAPIDbContext(options))
            {
                dbContext.AlbumReviews.Add(new AlbumReview()
                {
                    Id = new Guid("853de498-0b0a-4801-9bf7-9b8497150e21"),
                    ArtistName = "Keshi",
                    AlbumName = "Gabriel",
                    ReleaseDate = "2022-03-25",
                    Genre = "Pop",
                    NumTracks = 12,
                    Review = "10/10 sad"
                });
                dbContext.AlbumReviews.Add(new AlbumReview()
                {
                    Id = new Guid("dd071c29-f3ea-4e63-8438-807f3eb9031b"),
                    ArtistName = "Tyler the Creator",
                    AlbumName = "Call Me If You Get Lost",
                    ReleaseDate = "2021-06-25",
                    Genre = "Rap/Hip Hop",
                    NumTracks = 16,
                    Review = "9/10 solid project"
                });
                dbContext.AlbumReviews.Add(new AlbumReview()
                {
                    Id = new Guid("f24a38fe-9480-466a-8897-6d7f5d425f4a"),
                    ArtistName = "Giveon",
                    AlbumName = "Give or Take",
                    ReleaseDate = "2022-06-24",
                    Genre = "R&B",
                    NumTracks = 15,
                    Review = "Hurting 10/10"
                });
                dbContext.SaveChanges();
            }

            //Act - Run the test
            using (var dbContext = new AlbumReviewsAPIDbContext(options))
            {
                AlbumReviewsController controller = new AlbumReviewsController(dbContext, deezerService);
                var actionResult = controller.GetAllAlbumReviews();
                var okResult = actionResult.Result as OkObjectResult;
                List<AlbumReview> data = (List<AlbumReview>)okResult.Value;

                //Assert - Verify the outcome
                Assert.That(dbContext.AlbumReviews.Count, Is.EqualTo(data.Count()));
            }
        }
    }
}