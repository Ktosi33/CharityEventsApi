using CharityEventsApi.Entities;
using CharityEventsApi.Exceptions;
using CharityEventsApi.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace CharityEventsApi.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly CharityEventsDbContext dbContext;
        private const string path = "wwwroot\\images";

        public ImageService(CharityEventsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> SaveImageAsync(IFormFile image)
        {
            checkifPathExistAsync();

            long size = image.Length;

            return await addOneImageAsync(image);
            
        }
        public async Task<List<int>> SaveImagesAsync(List<IFormFile> images)
        {
            checkifPathExistAsync();

            long size = images.Sum(f => f.Length);
            List<int> ids = new List<int>();
            foreach (var image in images)
            {
               ids.Add(await addOneImageAsync(image));
            }
            return ids;
        }
        private async Task<int> addOneImageAsync(IFormFile image)
        {
            int id = -1;
            
                Image img = new Image { Path = "Temporary null", ContentType = image.ContentType };
                dbContext.Images.Add(img);
                dbContext.SaveChanges();
                id = img.IdImages;
                var pathimg = Path.Combine(path, id.ToString());

                if (image.Length > 0)
                {
                    using (var stream = File.Create(pathimg))
                    {
                        await image.CopyToAsync(stream);
                    }
                    img.Path = pathimg;
                    await dbContext.SaveChangesAsync();
                
                }
            return id;
        }
        private void checkifPathExistAsync()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
     
        public async Task<ImageDto> GetImageAsync(int id)
        {
            var img = dbContext.Images.SingleOrDefault(i => i.IdImages == id);
            if (img is null)
            {
                throw new NotFoundException("Image with given id doesn't exist");
            }

            Byte[] content;
            var pathimg = Path.Combine(path, id.ToString());
            content = await File.ReadAllBytesAsync(pathimg);
            var image = new ImageDto
            {
                IdImages = id,
                Content = Convert.ToBase64String(content),
                ContentType = img.ContentType
            };
            return image;
        }
        
        public async Task<IEnumerable<ImageDto>> GetImagesInRangeAsync(List<int> ids)
        {
            List<ImageDto> images = new List<ImageDto>();
            foreach (var id in ids)
            {
                images.Add(await GetImageAsync(id));
            }

            return images;
        }
        public async Task<IEnumerable<ImageDto>> GetImagesDtoByImages(ICollection<Image> images)
        {
            List<ImageDto> imagesDto = new List<ImageDto>();
            foreach (var image in images)
            {
                imagesDto.Add(await GetImageAsync(image.IdImages));
            }

            return imagesDto;
        }
        public async Task DeleteImageByIdAsync(int idImage)
        {
            var image = await dbContext.Images.FirstOrDefaultAsync(i => i.IdImages == idImage);
            if(image is null)
            {
                throw new NotFoundException("Image with given id doesn't exist");
            }
            dbContext.Images.Remove(image);
            await dbContext.SaveChangesAsync();
            File.Delete(image.Path);
        }
       
        public async Task DeleteImageByObjectAsync(Image image)
        {
            dbContext.Images.Remove(image);
            await dbContext.SaveChangesAsync();
            File.Delete(image.Path);
        }

    }
}
