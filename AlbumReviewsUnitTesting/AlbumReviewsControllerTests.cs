using AlbumReviewsAPI.Controllers;
using Domain.Data;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Service.IServices;

namespace AlbumReviewsUnitTesting
{
    [TestFixture]
    public class AlbumReviewsControllerTests
    {
        private Mock<HttpMessageHandler>? mockHttpMessageHandler;
        private Mock<ICustomService<AlbumReview>> mockAlbumReviewsService;
        private Mock<IDeezerService> mockDeezerService;
        private AlbumReviewsController controller;
        private List<AlbumReview> sampleData;

        [SetUp]
        public void Setup()
        {
            mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            HttpClient httpClient = new HttpClient(mockHttpMessageHandler.Object);
            mockAlbumReviewsService = new Mock<ICustomService<AlbumReview>>();
            mockDeezerService = new Mock<IDeezerService>();
            controller = new AlbumReviewsController(mockAlbumReviewsService.Object, mockDeezerService.Object);

            sampleData = new List<AlbumReview>() {
                new AlbumReview() {
                    Id = new Guid("853de498-0b0a-4801-9bf7-9b8497150e21"),
                    ArtistName = "Keshi",
                    AlbumName = "GABRIEL",
                    ReleaseDate = "2022-03-25",
                    Genre = "Pop",
                    NumTracks = 12,
                    Review = "7/10 some really good songs on here"
                },
                new AlbumReview() {
                    Id = new Guid("dd071c29-f3ea-4e63-8438-807f3eb9031b"),
                    ArtistName = "Tyler the Creator",
                    AlbumName = "Call Me If You Get Lost",
                    ReleaseDate = "2021-06-25",
                    Genre = "Rap/Hip Hop",
                    NumTracks = 16,
                    Review = "8/10 Solid Project"
                },
                new AlbumReview()
                {
                    Id = new Guid("f24a38fe-9480-466a-8897-6d7f5d425f4a"),
                    ArtistName = "Giveon",
                    AlbumName = "Give or Take",
                    ReleaseDate = "2022-06-24",
                    Genre = "R&B",
                    NumTracks = 15,
                    Review = "10/10 RnB Soul music"
                },
                new AlbumReview()
                {
                    Id = new Guid("9e865c3a-73b0-49f6-baf7-fa31f40090e4"),
                    ArtistName = "Olivia Rodrigo",
                    AlbumName = "SOUR",
                    ReleaseDate = "2021-05-21",
                    Genre = "Pop",
                    NumTracks = 11,
                    Review = "9/10 enjoy most of the music on here"
                }
            };
        }

        [Test]
        public void GetAll_Test_ReturnsTypeList()
        {
            mockAlbumReviewsService.Setup(service => service.GetAll())
                .ReturnsAsync(sampleData);

            var objectResult = controller.GetAllAlbumReviews().Result as OkObjectResult;
            
            Assert.NotNull(objectResult);
            Assert.IsInstanceOf<List<AlbumReview>>(objectResult.Value);
        }

        [Test]
        public void GetAll_Test_ReturnsCorrectReviewsCount()
        {
            mockAlbumReviewsService.Setup(service => service.GetAll())
                .ReturnsAsync(sampleData);

            var result = mockAlbumReviewsService.Object.GetAll().Result;

            Assert.IsInstanceOf<List<AlbumReview>>(result);
            Assert.That(result.Count(), Is.EqualTo(sampleData.Count()));
        }

        [Test]
        public void Get_Test_WithID_ReturnsTypeAlbumReview()
        {
            Guid Id = new Guid("f24a38fe-9480-466a-8897-6d7f5d425f4a");

            mockAlbumReviewsService.Setup(service => service.Get(Id))
                .ReturnsAsync(sampleData.Find(review => review.Id.Equals(Id))!);

            var objectResult = controller.GetAlbumReview(Id).Result as OkObjectResult;

            Assert.NotNull(objectResult);
            Assert.IsInstanceOf<AlbumReview>(objectResult.Value);
        }

        [Test]
        public void Get_Test_WithID_ReturnsAlbumReview()
        {
            Guid Id = new Guid("f24a38fe-9480-466a-8897-6d7f5d425f4a");

            mockAlbumReviewsService.Setup(service => service.Get(Id))
                .ReturnsAsync(sampleData.Find(review => review.Id.Equals(Id))!);

            var objectResult = controller.GetAlbumReview(Id).Result as OkObjectResult;

            Assert.NotNull(objectResult);
            Assert.That(sampleData.Find(review => review.Id.Equals(Id)), Is.EqualTo(objectResult.Value));
        }
    }
}
