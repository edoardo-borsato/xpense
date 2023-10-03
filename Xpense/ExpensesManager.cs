using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xpense
{
    internal class ExpensesManager
    {
        private readonly ISettingsManager _settingsManager;

        public ExpensesManager(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public async Task<IEnumerable<Expense>> GetAllAsync(string fromDate, string toDate, string inDate)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = _settingsManager.GetExpensesServiceUri();
            var request = CreateGetExpensesRequest(fromDate, toDate, inDate);

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
            httpClient.BaseAddress = _settingsManager.GetExpensesServiceUri();
            var request = CreateGetExpenseRequest(id);

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
            httpClient.BaseAddress = _settingsManager.GetExpensesServiceUri();
            var request = CreateCreateExpenseRequest(expenseDetails);

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
            httpClient.BaseAddress = _settingsManager.GetExpensesServiceUri();
            var request = CreateUpdateExpenseRequest(id, expenseDetails);

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
            httpClient.BaseAddress = _settingsManager.GetExpensesServiceUri();
            var request = CreateDeleteExpenseRequest(id);

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await RaiseException(response);
            }
        }

        #region Utility Methods

        private void AddUsernameHeader(HttpRequestMessage request)
        {
            request.Headers.Add("Username", _settingsManager.GetUsername());
        }

        private HttpRequestMessage CreateGetExpensesRequest(string fromDate, string toDate, string inDate)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/expenses?from={fromDate}&to={toDate}&in={inDate}");
            AddUsernameHeader(request);
            return request;
        }

        private HttpRequestMessage CreateGetExpenseRequest(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/expenses/{id}");
            AddUsernameHeader(request);
            return request;
        }

        private HttpRequestMessage CreateCreateExpenseRequest(ExpenseDetails expenseDetails)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/expenses");
            request.Content = JsonContent.Create(expenseDetails, null, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            });
            AddUsernameHeader(request);
            return request;
        }

        private HttpRequestMessage CreateUpdateExpenseRequest(Guid id, ExpenseDetails expenseDetails)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/expenses/{id}");
            request.Content = JsonContent.Create(expenseDetails, null, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            });
            AddUsernameHeader(request);
            return request;
        }

        private HttpRequestMessage CreateDeleteExpenseRequest(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/expenses/{id}");
            AddUsernameHeader(request);
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

        [JsonPropertyName("category")]
        public Category Category { get; set; }
    }

    internal enum Category
    {
        Others = 0,
        HousingAndSupplies = 1,
        HealthAndPersonalCare = 2,
        Sport = 3,
        Transportation = 4,
        Clothing = 5,
        Entertainment = 6,
        BillsAndUtilities = 7,
        Pets = 8,
        Insurance = 9,
        Gifts = 10
    }
}