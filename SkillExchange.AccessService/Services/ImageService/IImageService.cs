using SkillExchange.AccessService.Models.ImageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Services.ImageService
{
    public interface IImageService
    {
        public Task<int> UploadImageAsync(ImageModel image, CancellationToken cancellationToken);
        public Task<ImageModel> DownloadImageAsync(int imageId, CancellationToken cancellationToken);
    }
}
