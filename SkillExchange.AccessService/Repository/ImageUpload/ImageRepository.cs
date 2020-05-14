using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SkillExchange.AccessService.Models.ImageModel;

namespace SkillExchange.AccessService.Repository.ImageUpload
{
    public class ImageRepository : IImageRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;
        public ImageRepository(IDbConnectionProvider dbConnectionProvider)
        {
            this._dbConnectionProvider = dbConnectionProvider;
        }
        public async Task<ImageModel> DownloadImage(int imageId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ImageModel>($@"
                SELECT [Name],[Size],[ImageData] 
                FROM [Image] WHERE [Id] = @{nameof(imageId)}", new { imageId });
            }
        }

        public async Task<int> UploadImage(ImageModel image, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                image.Id = await connection.QuerySingleOrDefaultAsync<int>
                ($@"INSERT INTO [Image] ([Name],[Size],[ImageData])
                VALUES (@{nameof(image.Name)}, @{nameof(image.Size)}, @{nameof(image.ImageData)});                            
                SELECT CAST(SCOPE_IDENTITY() as int)", image);
            }
            return image.Id;
        }
    }
}
