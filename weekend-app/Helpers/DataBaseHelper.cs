using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using weekend_app.Models;

namespace weekend_app.Helpers
{
    public static class DataBaseHelper
    {
        private static string _connectionString = "Data Source=./weekend-app-database.db;";
        public static List<DatabaseModel> GetDataFromDataBase()
        {
            List<DatabaseModel> dataList = new List<DatabaseModel>();
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand("SELECT * FROM Weekends", conn);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DatabaseModel dataModel = new DatabaseModel();
                        dataModel.Id = (long)reader[0];
                        dataModel.Country = reader[1].ToString();
                        dataModel.Weekends = reader[2].ToString();
                        dataModel.CountryCode = reader[3].ToString();
                        dataList.Add(dataModel);
                    }
                    reader.Close();
                }
                catch { 
                    return dataList;
                }
            }
            return dataList;
        }
        public static void SaveDataInDatabase(string country, string code, List<Weekend> weekendList)
        {

            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(@"INSERT INTO Weekends (Country, Weekends, CountryCode) VALUES(@country, @list, @code); ", conn);
                    command.Parameters.AddWithValue("@country", country);
                    command.Parameters.AddWithValue("@list", ConvertWeekendListIntoString(weekendList));
                    command.Parameters.AddWithValue("@code", code);
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                catch
                {
                    return;
                }
            }
        }
        private static string ConvertWeekendListIntoString(List<Weekend> weekendList)
        {
            string weekendListString = JsonSerializer.Serialize(weekendList);
            return weekendListString;
        }
    }
}
