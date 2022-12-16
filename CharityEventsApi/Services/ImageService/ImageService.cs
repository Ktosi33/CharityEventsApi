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
            return await addOneImageAsync(image);
        }
        public async Task<List<int>> SaveImagesAsync(List<IFormFile> images)
        {
            checkifPathExistAsync();

            List<int> ids = new();
            foreach (var image in images)
            {
               ids.Add(await addOneImageAsync(image));
            }
            return ids;
        }
        private async Task<int> addOneImageAsync(IFormFile image)
        {
                if(!isValidImage(image.ContentType, image.Length))
                {
                throw new BadRequestException("Image is not valid, bad content type or too long");
                }
                Image img = new() { Path = "Temporary null", ContentType = image.ContentType };
                dbContext.Images.Add(img);
                dbContext.SaveChanges();
                int id = img.IdImages;
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
        private bool isValidImage(string contentType, long fileSize)
        {
            if(contentType.StartsWith("image"))
            {
                
            if ((fileSize / 1048576.0) > 5)
            {
                return false;
            }

                return true;
            }
            else
            {
                return false;
            }
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

            return await createImageDtoByObjectAsync(img);
        }
        private async Task<ImageDto> createImageDtoByObjectAsync(Image img)
        {
            Byte[] content;
            var pathimg = Path.Combine(path, img.IdImages.ToString());
            content = await File.ReadAllBytesAsync(pathimg);
            var image = new ImageDto
            {
                IdImages = img.IdImages,
                Content = Convert.ToBase64String(content),
                ContentType = img.ContentType
            };
            return image;
        }
        public async Task<IEnumerable<ImageDto>> GetImagesInRangeAsync(List<int> ids)
        {
            List<ImageDto> images = new();
            foreach (var id in ids)
            {
                images.Add(await GetImageAsync(id));
            }

            return images;
        }
        public async Task<IEnumerable<ImageDto>> GetImagesDtoByImages(ICollection<Image> images)
        {
            List<ImageDto> imagesDto = new();
            foreach (var image in images)
            {
                imagesDto.Add(await GetImageAsync(image.IdImages));
            }

            return imagesDto;
        }
        public List<Image> getImageObjectsByIds(List<int> ids)
        {
             return dbContext.Images.Where(img => ids.Contains(img.IdImages)).ToList();
           
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
