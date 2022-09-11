using Domain.Data;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Service.IServices;

namespace AlbumReviewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumReviewsController : ControllerBase
    {
        private readonly ICustomService<AlbumReview> albumReviewsService;
        private readonly IDeezerService deezerService;

        public AlbumReviewsController(ICustomService<AlbumReview> albumReviewsService, IDeezerService deezerService)
        {
            this.albumReviewsService = albumReviewsService;
            this.deezerService = deezerService;
        }

        /// <summary>
        /// READ operation - all:
        /// Retrieves all album reviews
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAlbumReviews()
        {
            return Ok(await albumReviewsService.GetAll());
        }

        /// <summary>
        /// READ operation - specific:
        /// Search for an existing album review in the database by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetAlbumReview([FromRoute] Guid id)
        {
            var albumReview = await albumReviewsService.Get(id);

            if (albumReview == null)
            {
                return NotFound();
            }

            return Ok(albumReview);
        }

        /// <summary>
        /// CREATE operation:
        /// Make a new album review for the database
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="albumName"></param>
        /// <param name="review"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAlbumReview(string artistName, string albumName, string review)
        {
            var detailsContent = await deezerService.GetAlbumFromDeezer(artistName, albumName);

            var detailsJson = JObject.Parse(detailsContent!);
            var detailsReleaseDate = (string)detailsJson["release_date"]!;
            var detailsGenre = (string)detailsJson["genres"]!["data"]![0]!["name"]!;
            var detailsNumTracks = (int)detailsJson["nb_tracks"]!;

            var albumReview = new AlbumReview()
            {
                Id = Guid.NewGuid(),
                ArtistName = artistName,
                AlbumName = albumName,
                ReleaseDate = detailsReleaseDate,
                Genre = detailsGenre,
                NumTracks = detailsNumTracks,
                Review = review
            };

            albumReviewsService.Insert(albumReview);

            return Ok(albumReview);
        }

        /// <summary>
        /// UPDATE Operation:
        /// Checks if ID exists first before attempting album review update in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="artistName"></param>
        /// <param name="albumName"></param>
        /// <param name="review"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateAlbumReview([FromRoute] Guid id, string artistName, string albumName, string review)
        {
            var albumReview = await albumReviewsService.Get(id);

            if (albumReview == null)
            {
                return NotFound();
            }

            var detailsContent = await deezerService.GetAlbumFromDeezer(artistName, albumName);

            var detailsJson = JObject.Parse(detailsContent!);
            var detailsReleaseDate = (string)detailsJson["release_date"]!;
            var detailsGenre = (string)detailsJson["genres"]!["data"]![0]!["name"]!;
            var detailsNumTracks = (int)detailsJson["nb_tracks"]!;

            albumReview.ArtistName = artistName;
            albumReview.AlbumName = albumName;
            albumReview.ReleaseDate = detailsReleaseDate;
            albumReview.Genre = detailsGenre;
            albumReview.NumTracks = detailsNumTracks;
            albumReview.Review = review;

            albumReviewsService.Update(albumReview);

            return Ok(albumReview);
        }

        /// <summary>
        /// DELETE operation
        /// Check if ID exists first before attempting album review deletion from the database
        /// Passing delete album review back in case user would like to use it for something
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAlbumReview(Guid id)
        {
            var albumReview = await albumReviewsService.Get(id);

            if (albumReview == null)
            {
                return NotFound();
            }

            albumReviewsService.Remove(albumReview);

            return Ok(albumReview);
        }
    }
}
