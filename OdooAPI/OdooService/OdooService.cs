using RestSharp;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Odoo_Project.Models;

public class OdooService
{
    private readonly string _baseUrl;
    private readonly string _apiKey;
    private readonly string _userId;
    private readonly string _database;

    public OdooService(IConfiguration configuration)
    {
        _baseUrl = configuration["OdooService:BaseUrl"];
        _apiKey = configuration["OdooService:ApiKey"];
        _userId = configuration["OdooService:UserId"];
        _database = configuration["OdooService:Database"];
    }

    public async Task PushInvoiceAsync(Invoice invoice)
    {
        var client = new RestClient(_baseUrl);
        var request = new RestRequest("jsonrpc", Method.Post);
        request.AddHeader("Content-Type", "application/json");

        var parameters = new
        {
            service = "object",
            method = "execute_kw",
            args = new object[]
            {
                _database,
                _userId,
                _apiKey,  
                "account.move",
                "create",
                new object[]
                {
                    new
                    {
                        move_type = "out_invoice",
                        name = invoice.Number,
                        invoice_date = invoice.InvoiceDate.ToString("yyyy-MM-dd")
                    }
                }
            }
        };

        var body = new
        {
            jsonrpc = "2.0",
            method = "call",
            @params = parameters,
            id = 1 
        };

        request.AddJsonBody(body);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var responseContent = JsonConvert.DeserializeObject<JObject>(response.Content);

            if (responseContent.ContainsKey("error"))
            {
                var error = responseContent["error"];
                Console.WriteLine("Failed to push invoice to Odoo. Error: " + error["message"]);
                Console.WriteLine("Error details: " + error["data"]["debug"]);
            }
            else
            {
                var odooInvoiceId = responseContent["result"];
                Console.WriteLine("Invoice pushed to Odoo successfully with ID: " + odooInvoiceId);
            }
        }
        else
        {
            Console.WriteLine("Failed to push invoice to Odoo. Response: " + response.Content);
            // Additional logging to capture detailed error information
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));
            Console.WriteLine("Response Status Code: " + response.StatusCode);
            Console.WriteLine("Response Error Message: " + response.ErrorMessage);
        }
    }


    public async Task DescribeAccountMoveModelAsync()
    {
        var client = new RestClient(_baseUrl);
        var request = new RestRequest("jsonrpc", Method.Post);
        request.AddHeader("Content-Type", "application/json");

        var parameters = new
        {
            service = "object",
            method = "execute_kw",
            args = new object[]
            {
                _database,  
                2,          
                _apiKey,    
                "account.move",
                "fields_get",
                new object[] { },
                new { attributes = new string[] { "string", "help", "type" } }
            }
        };

        var body = new
        {
            jsonrpc = "2.0",
            method = "call",
            @params = parameters,
            id = 1 // Request identifier, can be any unique integer
        };

        request.AddJsonBody(body);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var responseContent = JsonConvert.DeserializeObject<JObject>(response.Content);
            var result = responseContent["result"];

            Console.WriteLine("Fields in account.move model:");
            foreach (JProperty field in result["fields"])
            {
                Console.WriteLine($"{field.Name}: {field.Value["string"]}");
            }
        }
        else
        {
            Console.WriteLine("Failed to describe account.move model. Response: " + response.Content);
        }
    }


    

    public async Task PushClientAsync(Client client)
    {
        var client1 = new RestClient(_baseUrl);
        var request = new RestRequest("jsonrpc", Method.Post);
        request.AddHeader("Content-Type", "application/json");

        string photoBase64 = null;

        if (client.Photo != null)
        {
            photoBase64 = Convert.ToBase64String(client.Photo);
        }

        // Retrieve country ID based on the country name
        int? countryId = await GetCountryIdByNameAsync(client.Country);

        if (!countryId.HasValue)
        {
            Console.WriteLine($"Failed to retrieve country ID for country: {client.Country}");
            return;
        }

        var parameters = new
        {
            service = "object",
            method = "execute_kw",
            args = new object[]
            {
            _database,
            _userId,
            _apiKey,
            "res.partner",
            "create",
            new object[]
            {
                new
                {
                    name = client.Name,
                    email = client.Email,
                    image_medium = photoBase64,
                    country_id = countryId.Value
                }
            }
            }
        };

        var body = new
        {
            jsonrpc = "2.0",
            method = "call",
            @params = parameters,
            id = 1
        };

        request.AddJsonBody(body);

        var response = await client1.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var responseContent = JsonConvert.DeserializeObject<JObject>(response.Content);

            if (responseContent.ContainsKey("error"))
            {
                var error = responseContent["error"];
                Console.WriteLine("Failed to push client to Odoo. Error: " + error["message"]);
                Console.WriteLine("Error details: " + error["data"]["debug"]);
            }
            else
            {
                var odooClientId = responseContent["result"];
                Console.WriteLine("Client pushed to Odoo successfully with ID: " + odooClientId);
            }
        }
        else
        {
            Console.WriteLine("Failed to push client to Odoo. Response: " + response.Content);
            // Additional logging to capture detailed error information
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));
            Console.WriteLine("Response Status Code: " + response.StatusCode);
            Console.WriteLine("Response Error Message: " + response.ErrorMessage);
        }
    }

    public async Task<int?> GetCountryIdByNameAsync(string countryName)
    {
        var client = new RestClient(_baseUrl);
        var request = new RestRequest("jsonrpc", Method.Post);
        request.AddHeader("Content-Type", "application/json");

        var parameters = new
        {
            jsonrpc = "2.0",
            method = "call",
            @params = new
            {
                service = "object",
                method = "execute_kw",
                args = new object[]
                {
               _database,
                _userId,
                _apiKey,
                "res.country",
                "search_read",
                new object[]
                {
                    new object[] { new object[] { "name", "=", countryName } },
                    new object[] { "id" }
                }
                }
            },
            id = 1
        };

        request.AddJsonBody(parameters);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var responseContent = JsonConvert.DeserializeObject<JObject>(response.Content);

            if (responseContent.ContainsKey("result"))
            {
                var results = responseContent["result"].ToObject<JArray>();
                if (results.Count > 0)
                {
                    var countryId = results[0]["id"].ToObject<int>();
                    return countryId;
                }
                else
                {
                    // Country not found
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Result not found in the response.");
            }
        }
        else
        {
            Console.WriteLine("Failed to retrieve country ID. Response: " + response.Content);
        }

        return null; 
    }



    public async Task PushPaymentAsync(Payment payment)
    {
        var client = new RestClient(_baseUrl);
        var request = new RestRequest("jsonrpc", Method.Post);
        request.AddHeader("Content-Type", "application/json");

        var parameters = new
        {
            service = "object",
            method = "execute_kw",
            args = new object[]
            {
            _database,
            _userId,
            _apiKey,
            "account.payment",
            "create",
            new object[]
            {
                new
                {
                    payment_type = "inbound",  // Assuming this is an inbound payment. Change if necessary.
                    partner_type = "customer", // Assuming payment is from a customer. Change if necessary.
                    amount = payment.Amount,
                    payment_date = payment.PaymentDate.ToString("yyyy-MM-dd"),
                    journal_id = 1, // Replace with your journal ID
                    payment_method_id = 1, // Replace with your payment method ID
                    communication = payment.Number,
                    partner_id = 1 // Replace with the actual partner ID
                }
            }
            }
        };

        var body = new
        {
            jsonrpc = "2.0",
            method = "call",
            @params = parameters,
            id = 1
        };

        request.AddJsonBody(body);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var responseContent = JsonConvert.DeserializeObject<JObject>(response.Content);

            if (responseContent.ContainsKey("error"))
            {
                var error = responseContent["error"];
                Console.WriteLine("Failed to push payment to Odoo. Error: " + error["message"]);
                Console.WriteLine("Error details: " + error["data"]["debug"]);
            }
            else
            {
                var odooPaymentId = responseContent["result"];
                Console.WriteLine("Payment pushed to Odoo successfully with ID: " + odooPaymentId);
            }
        }
        else
        {
            Console.WriteLine("Failed to push payment to Odoo. Response: " + response.Content);
            // Additional logging to capture detailed error information
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));
            Console.WriteLine("Response Status Code: " + response.StatusCode);
            Console.WriteLine("Response Error Message: " + response.ErrorMessage);
        }
    }




    public async Task PushCreditNoteAsync(CreditNote creditNote)
    {
        var client = new RestClient(_baseUrl);
        var request = new RestRequest("jsonrpc", Method.Post);
        request.AddHeader("Content-Type", "application/json");

        var parameters = new
        {
            service = "object",
            method = "execute_kw",
            args = new object[]
            {
            _database,
            _userId,
            _apiKey,
            "account.move",
            "create",
            new object[]
            {
                new
                {
                    move_type = "out_refund", 
                    name = creditNote.Number,
                    invoice_date = creditNote.InvoiceDate.ToString("yyyy-MM-dd")
                }
            }
            }
        };

        var body = new
        {
            jsonrpc = "2.0",
            method = "call",
            @params = parameters,
            id = 1
        };

        request.AddJsonBody(body);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var responseContent = JsonConvert.DeserializeObject<JObject>(response.Content);

            if (responseContent.ContainsKey("error"))
            {
                var error = responseContent["error"];
                Console.WriteLine("Failed to push credit note to Odoo. Error: " + error["message"]);
                Console.WriteLine("Error details: " + error["data"]["debug"]);
            }
            else
            {
                var odooCreditNoteId = responseContent["result"];
                Console.WriteLine("Credit note pushed to Odoo successfully with ID: " + odooCreditNoteId);
            }
        }
        else
        {
            Console.WriteLine("Failed to push credit note to Odoo. Response: " + response.Content);
            // Additional logging to capture detailed error information
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));
            Console.WriteLine("Response Status Code: " + response.StatusCode);
            Console.WriteLine("Response Error Message: " + response.ErrorMessage);
        }
    }

}
