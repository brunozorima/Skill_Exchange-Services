using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
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
        public async Task<ExchangeMessage> AddExchangeMessage(ExchangeMessage exchangeMessage, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ExchangeMessage>($@"INSERT INTO [Person_Has_Skill] ([Person_Id], [Skill_Id])
                VALUES (@{nameof(exchangeMessage.Sender_Id)},
                    @{nameof(exchangeMessage.Recipient_Id)}, 
                    @{nameof(exchangeMessage.MessageBody)},
                    @{nameof(exchangeMessage.TimeStamp)})", 
                new {
                    exchangeMessage
                });
            }
        }

        public async Task<ExchangeRequest> AddExchangeRequest(ExchangeRequest exchangeRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ExchangeRequest>($@"INSERT INTO [Person_Has_Skill] ([Person_Id], [Skill_Id])
                VALUES (@{nameof(exchangeRequest.Sender_Id)},
                    @{nameof(exchangeRequest.Recipient_Id)}, 
                    @{nameof(exchangeRequest.Opened_TimeStamp)},
                    @{nameof(exchangeRequest.Last_Message_TimeStamp)},
                    @{nameof(exchangeRequest.Status)}
                    )",
                new
                {
                    exchangeRequest
                });
            }
        }
    }
}
