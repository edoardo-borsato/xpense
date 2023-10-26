using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xpense
{
    internal class ServiceClient
    {
        private readonly ISettingsManager _settingsManager;

        public ServiceClient(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync(string fromDate, string toDate, string inDate)
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

        public async Task<Expense> GetExpenseAsync(Guid id)
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

        public async Task<Expense> CreateExpenseAsync(ExpenseDetails expenseDetails)
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

        public async Task<Expense> UpdateExpenseAsync(Guid id, ExpenseDetails expenseDetails)
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

        public async Task DeleteExpenseAsync(Guid id)
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

        public async Task<IEnumerable<Income>> GetAllIncomesAsync(string fromDate, string toDate, string inDate)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = _settingsManager.GetExpensesServiceUri();
            var request = CreateGetIncomesRequest(fromDate, toDate, inDate);

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await RaiseException(response);
            }

            return await GetIncomesFromResponseAsync(response);
        }

        public async Task<Income> GetIncomeAsync(Guid id)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = _settingsManager.GetExpensesServiceUri();
            var request = CreateGetIncomeRequest(id);

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await RaiseException(response);
            }

            return await GetIncomeFromResponseAsync(response);
        }

        public async Task<Income> CreateIncomeAsync(IncomeDetails incomeDetails)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = _settingsManager.GetExpensesServiceUri();
            var request = CreateCreateIncomeRequest(incomeDetails);

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await RaiseException(response);
            }

            return await GetIncomeFromResponseAsync(response);
        }

        public async Task<Income> UpdateIncomeAsync(Guid id, IncomeDetails incomeDetails)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = _settingsManager.GetExpensesServiceUri();
            var request = CreateUpdateIncomeRequest(id, incomeDetails);

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await RaiseException(response);
            }

            return await GetIncomeFromResponseAsync(response);
        }

        public async Task DeleteIncomeAsync(Guid id)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = _settingsManager.GetExpensesServiceUri();
            var request = CreateDeleteIncomeRequest(id);

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

        private HttpRequestMessage CreateGetIncomesRequest(string fromDate, string toDate, string inDate)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/incomes?from={fromDate}&to={toDate}&in={inDate}");
            AddUsernameHeader(request);
            return request;
        }

        private HttpRequestMessage CreateGetIncomeRequest(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/incomes/{id}");
            AddUsernameHeader(request);
            return request;
        }

        private HttpRequestMessage CreateCreateIncomeRequest(IncomeDetails incomeDetails)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/incomes");
            request.Content = JsonContent.Create(incomeDetails, null, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            });
            AddUsernameHeader(request);
            return request;
        }

        private HttpRequestMessage CreateUpdateIncomeRequest(Guid id, IncomeDetails incomeDetails)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/incomes/{id}");
            request.Content = JsonContent.Create(incomeDetails, null, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            });
            AddUsernameHeader(request);
            return request;
        }

        private HttpRequestMessage CreateDeleteIncomeRequest(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/incomes/{id}");
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

        private static async Task<IEnumerable<Income>> GetIncomesFromResponseAsync(HttpResponseMessage response)
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var incomes = (await JsonSerializer.DeserializeAsync<IEnumerable<Income>>(responseStream, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            }))?.ToList();

            return incomes;
        }

        private static async Task<Income> GetIncomeFromResponseAsync(HttpResponseMessage response)
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var income = await JsonSerializer.DeserializeAsync<Income>(responseStream, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            });

            return income;
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

    internal record Income
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("details")]
        public IncomeDetails IncomeDetails { get; set; }
    }

    internal record IncomeDetails
    {
        [JsonPropertyName("value")]
        public double Value { get; set; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }
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