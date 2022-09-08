using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using weekend_app.Models;

namespace weekend_app.Helpers
{
    public static class RestAPIHelper
    {
        public static async Task<WeekendResponseObject?> GetAllWeekendsForCountryAndYear(string countryCode, string year)
        {
            List<Weekend> weekends = new List<Weekend>();
            WeekendResponseObject responseObj = new WeekendResponseObject();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{Constants.AllWeekendsAdress}/{year}/{countryCode}");
                    client.Timeout = TimeSpan.FromSeconds(10);
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await client.GetAsync(client.BaseAddress);
                    responseObj.Code = response.StatusCode.ToString();
                    responseObj.Status = response.IsSuccessStatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        JArray jsonArray = JArray.Parse(result);
                        foreach (var jsonArrayObject in jsonArray)
                        {
                            string weekendName = jsonArrayObject["localName"].ToString();
                            string weekendDateString = jsonArrayObject["date"].ToString();
                            DateTime weekendDate = DateTime.ParseExact(weekendDateString, "yyyy-MM-dd",null);
                            Weekend weekend = new Weekend();
                            weekend.Name = weekendName;
                            weekend.Date = weekendDate;
                            weekends.Add(weekend);
                        }
                        responseObj.WeekendList = weekends;
                    }
                    else
                    {
                        responseObj.FailMessage = "Request failed";
                    }
                    response.Dispose();
                }
            }
            catch(Exception ex)
            {
                responseObj.FailMessage = ex.Message;
                return responseObj;
            }
            return responseObj;
        }
        public static async Task<CountryResponseObject?> GetAllCountries()
        {
            List<Country> countries = new List<Country>();
            CountryResponseObject responseObj = new CountryResponseObject();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constants.AllCountryAdress);
                    client.Timeout = TimeSpan.FromSeconds(10);
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await client.GetAsync(client.BaseAddress);
                    responseObj.Code = response.StatusCode.ToString();
                    responseObj.Status = response.IsSuccessStatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        JArray jsonArray = JArray.Parse(result);
                        foreach (var jsonArrayObject in jsonArray)
                        {
                            string countryCode = jsonArrayObject["cca2"].ToString();
                            string countryName = jsonArrayObject["name"]["common"].ToString();
                            Country country = new Country();
                            country.Name = countryName;
                            country.Code = countryCode;
                            countries.Add(country);
                        }
                        responseObj.CountryList = countries;
                    }
                    else
                    {
                        responseObj.FailMessage = "Request failed";
                    }
                    response.Dispose();
                }
            }
            catch(Exception ex)
            {
                responseObj.FailMessage = ex.Message;
                return responseObj;
            }
            return responseObj;
        }
    }
}
