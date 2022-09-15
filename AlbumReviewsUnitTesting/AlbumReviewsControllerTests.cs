using AlbumReviewsAPI.Controllers;
using Domain.Data;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json.Linq;
using Service.IServices;

namespace AlbumReviewsUnitTesting
{
    [TestFixture]
    public class AlbumReviewsControllerTests
    {
        private Mock<HttpMessageHandler>? mockHttpMessageHandler;
        private DbContextOptions<DatabaseContext> dbContextOptions;
        private Mock<ICustomService<AlbumReview>> mockAlbumReviewsService;
        private Mock<IDeezerService> mockDeezerService;
        private AlbumReviewsController controller;
        private DatabaseContext dbContext;
        private List<AlbumReview> sampleData;

        [SetUp]
        public void Setup()
        {
            mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            HttpClient httpClient = new HttpClient(mockHttpMessageHandler.Object);
            mockAlbumReviewsService = new Mock<ICustomService<AlbumReview>>();
            mockDeezerService = new Mock<IDeezerService>();
            controller = new AlbumReviewsController(mockAlbumReviewsService.Object, mockDeezerService.Object);

            dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "sampleData")
                .Options;

            dbContext = new DatabaseContext(dbContextOptions);

            dbContext.AlbumReviews.Add(new AlbumReview()
            {
                Id = new Guid("853de498-0b0a-4801-9bf7-9b8497150e21"),
                ArtistName = "Keshi",
                AlbumName = "GABRIEL",
                ReleaseDate = "2022-03-25",
                Genre = "Pop",
                NumTracks = 12,
                Review = "7/10 some really good songs on here"
            });
            dbContext.AlbumReviews.Add(new AlbumReview()
            {
                Id = new Guid("dd071c29-f3ea-4e63-8438-807f3eb9031b"),
                ArtistName = "Tyler the Creator",
                AlbumName = "Call Me If You Get Lost",
                ReleaseDate = "2021-06-25",
                Genre = "Rap/Hip Hop",
                NumTracks = 16,
                Review = "8/10 Solid Project"
            });
            dbContext.AlbumReviews.Add(new AlbumReview()
            {
                Id = new Guid("f24a38fe-9480-466a-8897-6d7f5d425f4a"),
                ArtistName = "Giveon",
                AlbumName = "Give or Take",
                ReleaseDate = "2022-06-24",
                Genre = "R&B",
                NumTracks = 15,
                Review = "10/10 RnB Soul music"
            });
            dbContext.AlbumReviews.Add(new AlbumReview()
            {
                Id = new Guid("9e865c3a-73b0-49f6-baf7-fa31f40090e4"),
                ArtistName = "Olivia Rodrigo",
                AlbumName = "SOUR",
                ReleaseDate = "2021-05-21",
                Genre = "Pop",
                NumTracks = 11,
                Review = "9/10 enjoy most of the music on here"
            });
            dbContext.SaveChangesAsync();

            sampleData = dbContext.Set<AlbumReview>().ToList();
        }

        [Test]
        public void Get_ShouldReturnTypeList_WhenCalled()
        {
            mockAlbumReviewsService.Setup(service => service.GetAll())
                .ReturnsAsync(sampleData);

            var result = controller.GetAllAlbumReviews().Result as OkObjectResult;

            result.Should().NotBeNull();
            result?.Value.Should().BeOfType<List<AlbumReview>>();
        }

        [Test]
        public void Get_ShouldReturnCorrectReviewsCount_WhenCalled()
        {
            mockAlbumReviewsService.Setup(service => service.GetAll())
                .ReturnsAsync(sampleData);

            var result = mockAlbumReviewsService.Object.GetAll().Result;

            result.Should().NotBeNull();
            result.Should().BeOfType<List<AlbumReview>>();
            result.Count().Should().Be(sampleData.Count());
        }


        [Test]
        public void Get_ShouldReturnTypeAlbumReview_WhenValidId()
        {
            Guid Id = new Guid("f24a38fe-9480-466a-8897-6d7f5d425f4a");

            mockAlbumReviewsService.Setup(service => service.Get(Id))
                .ReturnsAsync(sampleData.Find(review => review.Id.Equals(Id))!);

            var result = controller.GetAlbumReview(Id).Result as OkObjectResult;

            result.Should().NotBeNull();
            result?.Value.Should().BeOfType<AlbumReview>();
        }

        [Test]
        public void Get_ShouldReturnNotFound_WhenInvalidId()
        {
            Guid Id = new Guid("11855960-d3b3-4500-810d-266c7b35ef83");

            mockAlbumReviewsService.Setup(service => service.Get(Id))
                .ReturnsAsync(default(AlbumReview));

            var result = controller.GetAlbumReview(Id).Result;

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Test]
        public void Get_ShouldReturnAlbumReviewDetails_WhenValidId()
        {
            Guid Id = new Guid("f24a38fe-9480-466a-8897-6d7f5d425f4a");

            mockAlbumReviewsService.Setup(service => service.Get(Id))
                .ReturnsAsync(sampleData.Find(review => review.Id.Equals(Id))!);

            var result = controller.GetAlbumReview(Id).Result as OkObjectResult;

            result.Should().NotBeNull();
            result?.Value.Should().BeEquivalentTo(sampleData.Find(review => review.Id.Equals(Id)));
        }

        [Test]
        public void Add_ShouldReturnTypeAlbumReview_WhenValidDetails()
        {
            var artistName = "The Weeknd";
            var albumName = "Dawn FM";
            var review = "8/10 great album";

            JObject deezerAlbumDetails = new JObject(
                new JProperty("release-date", "2022-01-07"),
                new JProperty("nb_tracks", 16),
                new JProperty("genres", 
                    new JObject(
                        new JProperty("data",
                            new JArray(
                                new JObject(
                                    new JProperty("name", "R&B")
                                )
                            )
                        )
                    )
                )
            );

            var albumReview = new AlbumReview()
            {
                Id = Guid.NewGuid(),
                ArtistName = artistName,
                AlbumName = albumName,
                ReleaseDate = (string)deezerAlbumDetails["release_date"]!,
                Genre = (string)deezerAlbumDetails["genres"]!["data"]![0]!["name"]!,
                NumTracks = (int)deezerAlbumDetails["nb_tracks"]!,
                Review = review
            };

            mockDeezerService.Setup(service => service.GetAlbumFromDeezer(artistName, albumName))
                .ReturnsAsync(deezerAlbumDetails);

            mockAlbumReviewsService.Setup(service => service.Insert(albumReview));

            var result = controller.AddAlbumReview(artistName, albumName, review).Result as OkObjectResult;

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result?.Value.Should().BeOfType<AlbumReview>();
        }

        [Test]
        public void Add_ShouldReturnBadRequest_WhenInvalidDetails()
        {
            var artistName = "This is an artist name";
            var albumName = "That is an album name";
            var review = "It's alright";

            mockDeezerService.Setup(service => service.GetAlbumFromDeezer(artistName, albumName))
                .ReturnsAsync(default(JObject));

            var result = controller.AddAlbumReview(artistName, albumName, review).Result;

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Test]
        public void Update_ShouldReturnNotFound_WhenInvalidId()
        {
            Guid Id = new Guid("11855960-d3b3-4500-810d-266c7b35ef83");
            var artistName = "Giveon";
            var albumName = "Give or Take";
            var review = "9/10 great RnB and Soul";

            mockAlbumReviewsService.Setup(service => service.Get(Id))
                .ReturnsAsync(default(AlbumReview));

            var result = controller.UpdateAlbumReview(Id, artistName, albumName, review).Result;

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Test]
        public void Update_ShouldReturnBadRequest_WhenInvalidDetails()
        {
            Guid Id = new Guid("f24a38fe-9480-466a-8897-6d7f5d425f4a");
            var artistName = "This is an artist name";
            var albumName = "That is an album name";
            var review = "It's alright";


            mockAlbumReviewsService.Setup(service => service.Get(Id))
                .ReturnsAsync(sampleData.Find(review => review.Id.Equals(Id))!);

            mockDeezerService.Setup(service => service.GetAlbumFromDeezer(artistName, albumName))
                .ReturnsAsync(default(JObject));

            var result = controller.UpdateAlbumReview(Id, artistName, albumName, review).Result;

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Test]
        public void Update_ShouldReturnTypeAlbumReview_WhenValidDetails()
        {
            Guid Id = new Guid("f24a38fe-9480-466a-8897-6d7f5d425f4a");
            var artistName = "Giveon";
            var albumName = "Give or Take";
            var review = "9/10 great RnB and Soul";

            JObject deezerAlbumDetails = new JObject(
                new JProperty("release-date", "2022-06-24"),
                new JProperty("nb_tracks", 15),
                new JProperty("genres",
                    new JObject(
                        new JProperty("data",
                            new JArray(
                                new JObject(
                                    new JProperty("name", "R&B")
                                )
                            )
                        )
                    )
                )
            );

            mockAlbumReviewsService.Setup(service => service.Get(Id))
                .ReturnsAsync(sampleData.Find(review => review.Id.Equals(Id))!);

            mockDeezerService.Setup(service => service.GetAlbumFromDeezer(artistName, albumName))
                .ReturnsAsync(deezerAlbumDetails);

            var result = controller.UpdateAlbumReview(Id, artistName, albumName, review).Result as OkObjectResult;

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result?.Value.Should().BeOfType<AlbumReview>();
        }

        [Test]
        public void Delete_ShouldReturnNotFound_WhenInvalidId()
        {
            Guid Id = new Guid("11855960-d3b3-4500-810d-266c7b35ef83");

            mockAlbumReviewsService.Setup(service => service.Get(Id))
                .ReturnsAsync(default(AlbumReview));

            var result = controller.DeleteAlbumReview(Id).Result;

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Test]
        public void Delete_ShouldReturnTypeAlbumReview_WhenValidId()
        {
            Guid Id = new Guid("f24a38fe-9480-466a-8897-6d7f5d425f4a");

            mockAlbumReviewsService.Setup(service => service.Get(Id))
                .ReturnsAsync(sampleData.Find(review => review.Id.Equals(Id))!);

            var result = controller.DeleteAlbumReview(Id).Result as OkObjectResult;

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result?.Value.Should().BeOfType<AlbumReview>();
        }
    }
}
