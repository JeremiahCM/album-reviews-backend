using Domain.Models;
using Repository.IRepository;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AlbumReviewsService : ICustomService<AlbumReview>
    {
        private readonly IRepository<AlbumReview> albumReviewsRepository;

        public AlbumReviewsService(IRepository<AlbumReview> albumReviewsRepository)
        {
            this.albumReviewsRepository = albumReviewsRepository;
        }

        

        public async Task<AlbumReview> Get(Guid id)
        {
            try
            {
                var albumReview = await albumReviewsRepository.Get(id);
                if (albumReview != null)
                {
                    return albumReview;
                } else
                {
                    return null!;
                }
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AlbumReview>> GetAll()
        {
            try
            {
                var albumReviews = await albumReviewsRepository.GetAll();
                if (albumReviews != null)
                {
                    return albumReviews;
                } else
                {
                    return null!;
                }
            } catch (Exception)
            {
                throw;
            }
        }

        public void Insert(AlbumReview albumReview)
        {
            try
            {
                if (albumReview != null)
                {
                    albumReviewsRepository.Insert(albumReview);
                    albumReviewsRepository.SaveChanges();
                }
            } catch (Exception)
            {
                throw;
            }
        }

        public void Update(AlbumReview albumReview)
        {
            try
            {
                if (albumReview != null)
                {
                    albumReviewsRepository.Update(albumReview);
                    albumReviewsRepository.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(AlbumReview albumReview)
        {
            try
            {
                if (albumReview != null)
                {
                    albumReviewsRepository.Delete(albumReview);
                    albumReviewsRepository.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Remove(AlbumReview albumReview)
        {
            try
            {
                if (albumReview != null)
                {
                    albumReviewsRepository.Remove(albumReview);
                    albumReviewsRepository.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
