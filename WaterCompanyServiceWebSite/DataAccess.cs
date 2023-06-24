using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using ModelLibrary;
using WaterCompanyServiceWebSite.Models;

namespace WaterCompanyServiceWebSite
{
    public static class DataAccess
    {
        
        private static string live = "http://WCSAPI23.somee.com/";
        private static string local = "https://localhost:7186/";
        private static string newsrc = "http://wcsapi23-001-site1.btempurl.com/";
        public static User CurrentUser = null;
        private static string BaseURL = newsrc;
        public static void log(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }
        public static User Login(User user)
        {
            User result = null;
            using (var httpClient = new HttpClient())
            {
                String json = JsonConvert.SerializeObject(user);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{BaseURL}user/login"),
                    Content = new StringContent(json, Encoding.UTF8, "application/json"),
                };

                using (var response = httpClient.SendAsync(request))
                {
                    if (response.Result.IsSuccessStatusCode)
                    {
                        result = response.Result.Content.ReadFromJsonAsync<User>().Result;
                    }
                }
                return result;
            }
        }

        public static bool UserNameExists(string userName)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{BaseURL}user/exists/{userName}")
                };

                using (var response = httpClient.SendAsync(request))
                {
                    if (response.Result.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<bool>(response.Result.Content.ReadAsStringAsync().Result);
                    }
                }
                return true;
            }
        }

        public static void AddConsumer(Consumer consumer)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    String json = JsonConvert.SerializeObject(consumer);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{BaseURL}consumer"),
                        Content = new StringContent(json, Encoding.UTF8, "application/json"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void AddSubscription(Subscription sub)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    String json = JsonConvert.SerializeObject(sub);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{BaseURL}subscription"),
                        Content = new StringContent(json, Encoding.UTF8, "application/json"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<User> GetUsers()
        {
            List<User> result = new List<User>();
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{BaseURL}user"),
                };

                using (var response = httpClient.SendAsync(request))
                {
                    if (response.Result.IsSuccessStatusCode)
                    {
                        result = response.Result.Content.ReadFromJsonAsync<List<User>>().Result;
                    }
                }
                return result;
            }
        }

        public static List<Department> GetDepartments()
        {
            List<Department> result = new List<Department>();
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{BaseURL}department"),
                };

                using (var response = httpClient.SendAsync(request))
                {
                    if (response.Result.IsSuccessStatusCode)
                    {
                        result = response.Result.Content.ReadFromJsonAsync<List<Department>>().Result;
                    }
                }
                return result;
            }
        }

        public static User GetUser(int id)
        {
            User result = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}user/{id}"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            result = response.Result.Content.ReadFromJsonAsync<User>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            return result;
        }
       
        public static void UpdateUser(User user)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    String json = JsonConvert.SerializeObject(user);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Put,
                        RequestUri = new Uri($"{BaseURL}user/{user.Id}"),
                        Content = new StringContent(json, Encoding.UTF8, "application/json"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }


        public static Subscription GetSubscriptionByBarcode(string barcode)
        {
            Subscription result = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}subscription/getbybarcode/{barcode}"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            result = response.Result.Content.ReadFromJsonAsync<Subscription>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            return result;
        }

        public static Subscription GetSubscription(int sid)
        {
            Subscription result = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}subscription/{sid}"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            result = response.Result.Content.ReadFromJsonAsync<Subscription>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            return result;
        }

        public static List<Subscription> GetConsumerSubscription()
        {
            List<Subscription> result = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}subscription/getConsumerSubscriptions/{GetCurrentConsumer().Id}"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            result = response.Result.Content.ReadFromJsonAsync<List<Subscription>>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            return result;
        }

        public static List<Subscription> GetSubscriptions()
        {
            List<Subscription> result = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}subscription"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            result = response.Result.Content.ReadFromJsonAsync<List<Subscription>>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            return result;
        }

        public static List<Invoice> GetUnpaidInvoices(string barcode)
        {
            List<Invoice> result = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}invoice/getunpaidbybarcode/{barcode}"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            result = response.Result.Content.ReadFromJsonAsync<List<Invoice>>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            return result;
        }

        public static void AddEmployee(Employee employee)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    String json = JsonConvert.SerializeObject(employee);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{BaseURL}employee"),
                        Content = new StringContent(json, Encoding.UTF8, "application/json"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Employee GetCurrentEmployee()
        {
            Employee result = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}employee/getbyuser/{CurrentUser.Id}"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            result = response.Result.Content.ReadFromJsonAsync<Employee>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            return result;
        }

        public static Consumer GetCurrentConsumer()
        {
            Consumer result = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}consumer/getbyuser/{CurrentUser.Id}"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            result = response.Result.Content.ReadFromJsonAsync<Consumer>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            return result;
        }

        public static Request AddRequest(Request req)
        {
            Request result = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    String json = JsonConvert.SerializeObject(req);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{BaseURL}request"),
                        Content = new StringContent(json, Encoding.UTF8, "application/json"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            result = response.Result.Content.ReadFromJsonAsync<Request>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public static List<Request> GetPendingRequests()
        {
            List<Request> result = new List<Request>();
            using (var httpClient = new HttpClient())
            {
                int id = GetCurrentEmployee().Department.Id;
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{BaseURL}request/getpending/{id}"),
                };

                using (var response = httpClient.SendAsync(request))
                {
                    if (response.Result.IsSuccessStatusCode)
                    {
                        result = response.Result.Content.ReadFromJsonAsync<List<Request>>().Result;
                    }
                }
                return result;
            }
        }

        public static Request GetRequest(int id)
        {
            Request result = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}request/{id}"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            result = response.Result.Content.ReadFromJsonAsync<Request>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            return result;
        }

        public static bool AcceptRequest(int rid, string notes = "")
        {
            try
            {
                int eid = GetCurrentEmployee().Id;
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}request/accept/{rid}/{eid}/{notes}"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            return response.Result.Content.ReadFromJsonAsync<bool>().Result;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool RejectRequest(int rid, string notes = "")
        {
            try
            {
                int eid = GetCurrentEmployee().Id;
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}request/reject/{rid}/{eid}/{notes}"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            return response.Result.Content.ReadFromJsonAsync<bool>().Result;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public static List<Invoice> GetInvoices(string barcode)
        {
            List<Invoice> result = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{BaseURL}invoice/getbybarcode/{barcode}"),
                    };

                    using (var response = httpClient.SendAsync(request))
                    {
                        if (response.Result.IsSuccessStatusCode)
                        {
                            result = response.Result.Content.ReadFromJsonAsync<List<Invoice>>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            return result;
        }

        public static List<RequestResult> GetRequestsResults()
        {
            List<RequestResult> result = new List<RequestResult>();
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{BaseURL}requestresult"),
                };

                using (var response = httpClient.SendAsync(request))
                {
                    if (response.Result.IsSuccessStatusCode)
                    {
                        result = response.Result.Content.ReadFromJsonAsync<List<RequestResult>>().Result;
                    }
                }
                return result;
            }
        }

        public static List<RequestVM> GetConsumerRequests()
        {
            List<RequestVM> result = new List<RequestVM>();
            using (var httpClient = new HttpClient())
            {
                int cid = GetCurrentConsumer().Id;
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{BaseURL}request/getbyconsumer/{cid}"),
                };

                using (var response = httpClient.SendAsync(request))
                {
                    if (response.Result.IsSuccessStatusCode)
                    {
                        result = response.Result.Content.ReadFromJsonAsync<List<RequestVM>>().Result;
                    }
                }
                return result;
            }
        }

        public static RequestVM GetConsumerRequest(int rid)
        {
            RequestVM result = new RequestVM();
            using (var httpClient = new HttpClient())
            {
                int cid = GetCurrentConsumer().Id;
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{BaseURL}request/getconsumerrequest/{rid}"),
                };

                using (var response = httpClient.SendAsync(request))
                {
                    if (response.Result.IsSuccessStatusCode)
                    {
                        result = response.Result.Content.ReadFromJsonAsync<RequestVM>().Result;
                    }
                }
                return result;
            }
        }
    }
}
