using System;
using System.Data;

namespace Extensions
{
    public static class DataRowExtensions
    {
        public static bool GetBool(this DataRow row, int index) => bool.Parse(row[index].ToString());

        public static bool GetBool(this DataRow row, string colName) => bool.Parse(row[colName].ToString());

        public static bool? GetBoolNullable(this DataRow row, int index) => row[index] != DBNull.Value
                                                                                          ? bool.Parse(row[index].ToString())
                                                                                          : default(bool?);

        public static bool? GetBoolNullable(this DataRow row, string colName) => row[colName] != DBNull.Value
                                                                                              ? bool.Parse(row[colName].ToString())
                                                                                              : default(bool?);

        public static DateTime GetDateTime(this DataRow row, int index) => DateTime.Parse($"{row[index]:yyyy.MM.dd HH:mm:ss.fff}");

        public static DateTime GetDateTime(this DataRow row, string colName) => DateTime.Parse($"{row[colName]:yyyy.MM.dd HH:mm:ss.fff}");


        public static DateTime? GetDateTimeNullable(this DataRow row, int index) => row[index] != DBNull.Value
                                                                                    ? DateTime.Parse($"{row[index]:yyyy.MM.dd HH:mm:ss.fff}")
                                                                                    : default(DateTime?);

        public static DateTime? GetDateTimeNullable(this DataRow row, string colName) => row[colName] != DBNull.Value
                                                                                         ? DateTime.Parse($"{row[colName]:yyyy.MM.dd HH:mm:ss.fff}")
                                                                                         : default(DateTime?);


        public static Guid GetGuid(this DataRow row, int index) => Guid.Parse(row[index].ToString());

        public static Guid GetGuid(this DataRow row, string colName) => Guid.Parse(row[colName].ToString());


        public static Guid? GetGuidNullable(this DataRow row, int index) => row[index] != DBNull.Value
                                                                                    ? Guid.Parse($"{row[index]}")
                                                                                    : default(Guid?);

        public static Guid? GetGuidNullable(this DataRow row, string colName) => row[colName] != DBNull.Value
                                                                                         ? Guid.Parse($"{row[colName]}")
                                                                                         : default(Guid?);


        public static int GetInt(this DataRow row, int index) => int.Parse(row[index].ToString());

        public static int GetInt(this DataRow row, string colName) => int.Parse(row[colName].ToString());


        public static long GetLong(this DataRow row, int index) => long.Parse(row[index].ToString());

        public static long GetLong(this DataRow row, string colName) => long.Parse(row[colName].ToString());


        public static string GetString(this DataRow row, int index) => row[index] != DBNull.Value
                                                                                     ? row[index].ToString()
                                                                                     : null;

        public static string GetString(this DataRow row, string colName) => row[colName] != DBNull.Value
                                                                                         ? row[colName].ToString()
                                                                                         : null;


        public static TimeSpan GetTimeSpan(this DataRow row, int index) => TimeSpan.Parse(row[index].ToString());

        public static TimeSpan GetTimeSpan(this DataRow row, string colName) => TimeSpan.Parse(row[colName].ToString());
    }
}
