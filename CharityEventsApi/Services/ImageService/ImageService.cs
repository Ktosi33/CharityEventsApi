namespace CharityEventsApi.Services.ImageService
{
    public class ImageService
    {
        private string filePath = "\\uploads\\images\\";
        public ImageService()
        {

        }
        public async Task SaveImageAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            if(Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath); 

            }
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }


        }


    }
}
