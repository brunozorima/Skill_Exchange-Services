using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SkillExchange.AccessService.Models.ImageModel;
using SkillExchange.AccessService.Repository.ImageUpload;

namespace SkillExchange.AccessService.Services.ImageService
{
    public class ImageService : IImageService
    {
        public readonly IImageRepository _imageRepository;
        public readonly IIdentityService _identityService;
        public ImageService(IImageRepository imageRepository, IIdentityService identityService)
        {
            this._imageRepository = imageRepository;
            this._identityService = identityService;
        }
        public async Task<ImageModel> DownloadImageAsync(int imageId, CancellationToken cancellationToken)
        {
            var getImageById = await this._imageRepository.DownloadImage(imageId, cancellationToken);
            return getImageById;
        }

        public async Task<int> UploadImageAsync(ImageModel image, CancellationToken cancellationToken)
        {
            var UploadImageById = await this._imageRepository.UploadImage(image, cancellationToken);
            //update users table with a new photo id
            return UploadImageById;
        }
    }
}
