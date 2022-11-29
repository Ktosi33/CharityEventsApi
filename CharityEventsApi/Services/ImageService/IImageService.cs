using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;

namespace CharityEventsApi.Services.ImageService
{
    public interface IImageService
    {
        public Task<ImageDto> GetImageAsync(int id);
        public Task<IEnumerable<ImageDto>> GetImagesInRangeAsync(List<int> ids);
        public Task<IEnumerable<ImageDto>> GetImagesDtoByImages(ICollection<Image> images);
        public Task<List<int>> SaveImagesAsync(List<IFormFile> images);
        public Task<int> SaveImageAsync(IFormFile image);
        public Task DeleteImageByIdAsync(int idImage);
        public Task DeleteImageByObjectAsync(Image image);
        public List<Image> getImageObjectsByIds(List<int> ids);

    }
}
