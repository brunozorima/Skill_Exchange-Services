using SkillExchange.AccessService.Models.ImageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Repository.ImageUpload
{
    public interface IImageRepository
    {
        public Task<int> UploadImage(ImageModel image, CancellationToken cancellationToken);
        public Task<ImageModel> DownloadImage(int imageId, CancellationToken cancellationToken);
    }
}
