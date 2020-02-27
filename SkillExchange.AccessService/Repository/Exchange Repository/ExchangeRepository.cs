using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SkillExchange.AccessService.Domain.ExchangeDomain;
using SkillExchange.AccessService.Models;

namespace SkillExchange.AccessService.Repository.Exchange_Repository
{
    public class ExchangeRepository : IExchangeRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;
        public ExchangeRepository(IDbConnectionProvider dbConnectionProvider)
        {
            this._dbConnectionProvider = dbConnectionProvider;
        }
        public async Task<int> SendExchangeMessage(ExchangeMessage exchangeMessage, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                exchangeMessage.Id =  await connection.QuerySingleOrDefaultAsync<int>($@"INSERT INTO [ExchangeMessage] ([Sender_Id],[Recipient_Id],[Body], [TimeStamp])
                VALUES (@{nameof(exchangeMessage.Sender_Id)},
                    @{nameof(exchangeMessage.Recipient_Id)}, 
                    @{nameof(exchangeMessage.MessageBody)},
                    @{nameof(exchangeMessage.TimeStamp)}); SELECT CAST(SCOPE_IDENTITY() as int)", exchangeMessage);
            }
            return exchangeMessage.Id;
        }

        public async Task<int> RequestExchange(ExchangeRequest exchangeRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                exchangeRequest.Id = await connection.QuerySingleOrDefaultAsync<int>($@"INSERT INTO [Exchange] ([Sender_Id],[Recipient_Id],[Opened_TimeStamp],[Last_Message_TimeStamp],[Status])
                VALUES (@{nameof(exchangeRequest.Sender_Id)},
                    @{nameof(exchangeRequest.Recipient_Id)}, 
                    @{nameof(exchangeRequest.Opened_TimeStamp)},
                    @{nameof(exchangeRequest.Last_Message_TimeStamp)},
                    @{nameof(exchangeRequest.Status)}
                    ); SELECT CAST(SCOPE_IDENTITY() as int)", exchangeRequest);
            }
            return exchangeRequest.Id;
        }

        public async Task<IEnumerable<ApplicationUser>> GetRequestList(int recipient_id, int status, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<ApplicationUser>($@"
                    SELECT [U].[Id], [U].[FirstName], [U].[LastName], [U].[Email]
                    FROM [ApplicationUser] [U]    
                    JOIN [Exchange] [E] ON [U].Id = [E].[Sender_Id]
                    WHERE [E].[Recipient_Id] = @{nameof(recipient_id)}
                    AND [E].[Status] = @{nameof(status)}", new { recipient_id, status });
                return result.ToList();
            }
        }

    }
}