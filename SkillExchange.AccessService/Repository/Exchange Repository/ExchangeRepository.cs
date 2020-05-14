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
                //add a new message to the list (ExchangeMessage Table) and update the last message time on exchange table.
                await connection.OpenAsync(cancellationToken);
                exchangeMessage.Id =  await connection.QuerySingleOrDefaultAsync<int>
                ($@"INSERT INTO [ExchangeMessage] ([Sender_Id],[Exchange_Id],[Body], [TimeStamp])
                VALUES (@{nameof(exchangeMessage.Sender_Id)},
                    @{nameof(exchangeMessage.Exchange_Id)}, 
                    @{nameof(exchangeMessage.Body)},
                    @{nameof(exchangeMessage.TimeStamp)}); 
                            
                    UPDATE [Exchange]
                    SET [Last_Message_TimeStamp] = @{nameof(exchangeMessage.TimeStamp)}
                    WHERE [Id] = @{nameof(exchangeMessage.Exchange_Id)}
                    SELECT CAST(SCOPE_IDENTITY() as int)", exchangeMessage);
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

        //return all messages in a specific exchange
        public async Task<IEnumerable<ExchangeMessage>> GetAllMessagesInOneExchange(int loggedInUserId, int exchange_id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<ExchangeMessage>($@"
                    SELECT DISTINCT [EM].[ID], [EM].[Sender_Id], [EM].[Exchange_Id], [EM].[Body], [EM].[TimeStamp]
                    FROM [ExchangeMessage] [EM]
                    JOIN [Exchange] [E] ON 
                    [E].[Recipient_Id] = @{nameof(loggedInUserId)} OR [E].[Sender_Id] = @{nameof(loggedInUserId)} 
                    where [EM].[Exchange_Id] = @{nameof(exchange_id)}", new { exchange_id, loggedInUserId });              
                return result.ToList();
            }
        }

        //return all requests sent to this user
        public async Task<IEnumerable<ExchangeResponseModel>> RequestSentTo(int sender_id, CancellationToken cancellationToken, int status)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<ExchangeResponseModel>($@"
                    SELECT [U].[Id] as User_Id, [U].[FirstName], [U].[LastName], [U].[Email], [E].Id as Exchange_Id, [E].Status, [E].Opened_TimeStamp
                    FROM [ApplicationUser] [U]    
                    JOIN [Exchange] [E] ON [U].Id = [E].[Recipient_Id]
                    WHERE [E].[Sender_Id] = @{nameof(sender_id)}
                    AND [E].[Status] =  @{nameof(status)}", new { sender_id, status });
                return result.ToList();
            }
        }
        //return the list of uusers who I recieved requests for skill exchanges from
        public async Task<IEnumerable<ExchangeResponseModel>> RequestRecievedFrom(int recipient_id, CancellationToken cancellationToken, int status)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<ExchangeResponseModel>($@"
                    SELECT [U].[Id] as User_Id, [U].[FirstName], [U].[LastName], [U].[Email], [E].Id as Exchange_Id, [E].Status, [E].Opened_TimeStamp
                    FROM [ApplicationUser] [U]    
                    JOIN [Exchange] [E] ON [U].Id = [E].[Sender_Id]
                    WHERE [E].[Recipient_Id] = @{nameof(recipient_id)}
                    AND [E].[Status] =  @{nameof(status)}", new { recipient_id, status });
                return result.ToList();
            }
        }
        public async Task<ExchangeRequest> GetExchangeRequestById(int request_id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ExchangeRequest>($@"
                SELECT [Id], [Sender_Id], [Recipient_Id], [Status], [Opened_TimeStamp], [Last_Message_TimeStamp] 
                FROM [Exchange] WHERE [Id] = @{nameof(request_id)}", new { request_id });
            }
        }
        public async Task<ExchangeMessage> GetMessageById(int message_id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ExchangeMessage>($@"
                SELECT [Id],[Sender_Id],[Exchange_Id],[Body], [TimeStamp] 
                FROM [ExchangeMessage] WHERE [Id] = @{nameof(message_id)}", new { message_id });
            }
        }

        public async Task<ExchangeRequest> UpdateRequestStatus(int request_id, int status, int recipient, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ExchangeRequest>($@"
                 UPDATE [FYP_CONCEPT].[dbo].[Exchange] 
                 SET [Status] = @{nameof(status)}
                 WHERE [Id] = @{nameof(request_id)} 
                 AND ([Recipient_Id] = @{nameof(recipient)} OR [Sender_Id] = @{nameof(recipient)})", new { request_id, status, recipient });
            }           
        }
        public async Task<int> RejectRequest(int request_id, int user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.QuerySingleOrDefaultAsync<int>($@"
                DELETE FROM [Exchange]
                WHERE [Id] = @{nameof(request_id)} AND [Recipient_Id] = @{nameof(user)} OR [Sender_Id]= @{nameof(user)}", new { request_id, user});
            }
            return request_id;
        }

    }
}