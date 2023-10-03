using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xpense.Settings;

namespace Xpense
{
    internal class ExpensesManager
    {
        private readonly ExpensesServiceSettings _settings;

        public ExpensesManager(ISettingsManager configuration)
        {
            _settings = configuration.Get();
        }

        public async Task<IEnumerable<Expense>> GetAllAsync(string fromDate, string toDate, string inDate)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_settings.Url);
            var request = CreateGetExpensesRequest(fromDate, toDate, inDate, _settings.Credentials);

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await RaiseException(response);
            }

            return await GetExpensesFromResponseAsync(response);
        }

        public async Task<Expense> GetAsync(Guid id)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_settings.Url);
            var request = CreateGetExpenseRequest(id, _settings.Credentials);

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await RaiseException(response);
            }

            return await GetExpenseFromResponseAsync(response);
        }

        public async Task<Expense> CreateAsync(ExpenseDetails expenseDetails)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_settings.Url);
            var request = CreateCreateExpenseRequest(expenseDetails, _settings.Credentials);

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await RaiseException(response);
            }

            return await GetExpenseFromResponseAsync(response);
        }

        public async Task<Expense> UpdateAsync(Guid id, ExpenseDetails expenseDetails)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_settings.Url);
            var request = CreateUpdateExpenseRequest(id, expenseDetails, _settings.Credentials);

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await RaiseException(response);
            }

            return await GetExpenseFromResponseAsync(response);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_settings.Url);
            var request = CreateDeleteExpenseRequest(id, _settings.Credentials);

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await RaiseException(response);
            }
        }

        #region Utility Methods

        private static void AddBasicAuthentication(HttpRequestMessage request, Credentials credentials)
        {
            var authenticationString = GetAuthenticationString(credentials);
            request.Headers.Add("Authorization", $"Basic {authenticationString}");
        }

        private static HttpRequestMessage CreateGetExpensesRequest(string fromDate, string toDate, string inDate, Credentials credentials)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/expenses?from={fromDate}&to={toDate}&in={inDate}");
            AddBasicAuthentication(request, credentials);
            return request;
        }

        private static HttpRequestMessage CreateGetExpenseRequest(Guid id, Credentials credentials)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/expenses/{id}");
            AddBasicAuthentication(request, credentials);
            return request;
        }

        private static HttpRequestMessage CreateCreateExpenseRequest(ExpenseDetails expenseDetails, Credentials credentials)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/expenses");
            request.Content = JsonContent.Create(expenseDetails, null, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            });
            AddBasicAuthentication(request, credentials);
            return request;
        }

        private static HttpRequestMessage CreateUpdateExpenseRequest(Guid id, ExpenseDetails expenseDetails, Credentials credentials)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/expenses/{id}");
            request.Content = JsonContent.Create(expenseDetails, null, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            });
            AddBasicAuthentication(request, credentials);
            return request;
        }

        private static HttpRequestMessage CreateDeleteExpenseRequest(Guid id, Credentials credentials)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/expenses/{id}");
            AddBasicAuthentication(request, credentials);
            return request;
        }

        private static async Task RaiseException(HttpResponseMessage response)
        {
            var statusCode = response.StatusCode;
            var reason = response.ReasonPhrase;
            var responseContent = await response.Content.ReadAsStringAsync();
            var message = $"An error occurred. Status code: {statusCode}; Reason: {reason}; Content: {responseContent}";
            throw new Exception(message);
        }

        private static async Task<Expense> GetExpenseFromResponseAsync(HttpResponseMessage response)
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var expense = await JsonSerializer.DeserializeAsync<Expense>(responseStream, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            });

            return expense;
        }

        private static async Task<IEnumerable<Expense>> GetExpensesFromResponseAsync(HttpResponseMessage response)
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var expenses = (await JsonSerializer.DeserializeAsync<IEnumerable<Expense>>(responseStream, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            }))?.ToList();

            return expenses;
        }

        private static string GetAuthenticationString(Credentials credentials)
        {
            if (credentials != null)
            {
                var authenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{credentials.Username}:{credentials.Password}"));

                return authenticationString;
            }

            return null;
        }

        #endregion
    }

    internal record Expense
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("details")]
        public ExpenseDetails ExpenseDetails { get; set; }
    }

    internal record ExpenseDetails
    {
        [JsonPropertyName("value")]
        public double Value { get; set; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }

        [JsonPropertyName("paymentMethod")]
        public PaymentMethod PaymentMethod { get; set; }
    }

    internal enum PaymentMethod
    {
        Undefined = 0,
        Cash = 1,
        DebitCard = 2,
        CreditCard = 3
    }
}