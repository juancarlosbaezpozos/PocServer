using Principal.Server.Objects;
using System;
using System.Collections;
using System.Reflection;

namespace Principal.Server.Controller.Helpers;

public static class StiConnectionBuilderHelper
{
    public static class MySql
    {
        public static bool CanBuildConnectionString()
        {
            return false;
        }

        public static string EditConnectionString(string connectionString)
        {
            return connectionString;
        }
    }

    public static class MsSql
    {
        public static bool CanBuildConnectionString()
        {
            return Universal.CanBuildConnectionString("Sql");
        }

        public static string EditConnectionString(string connectionString)
        {
            return Universal.EditConnectionString(connectionString, "Sql");
        }
    }

    public static class Odbc
    {
        public static bool CanBuildConnectionString()
        {
            return Universal.CanBuildConnectionString("Odbc");
        }

        public static string EditConnectionString(string connectionString)
        {
            return Universal.EditConnectionString(connectionString, "Odbc");
        }
    }

    public static class OleDb
    {
        public static bool CanBuildConnectionString()
        {
            return Universal.CanBuildConnectionString("OleDb");
        }

        public static string EditConnectionString(string connectionString)
        {
            return Universal.EditConnectionString(connectionString, "OleDb");
        }
    }

    public static class Firebird
    {
        public static bool CanBuildConnectionString()
        {
            return false;
        }

        public static string EditConnectionString(string connectionString)
        {
            return connectionString;
        }
    }

    public static class PostgreSql
    {
        public static bool CanBuildConnectionString()
        {
            return false;
        }

        public static string EditConnectionString(string connectionString)
        {
            return connectionString;
        }
    }

    public static class Oracle
    {
        public static bool CanBuildConnectionString()
        {
            return false;
        }

        public static string EditConnectionString(string connectionString)
        {
            return connectionString;
        }
    }

    private static class Universal
    {
        private static Hashtable connectionHelpers = new();

        public static bool CanBuildConnectionString(string ident)
        {
            return GetConnectionHelper(ident) != null;
        }

        public static string EditConnectionString(string connectionString, string ident)
        {
            try
            {
                Type connectionHelper = GetConnectionHelper(ident);
                if (connectionHelper != null)
                {
                    return connectionHelper.GetMethod("EditConnectionString", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[1] { connectionString }) as string;
                }
            }
            catch
            {
            }
            return connectionString;
        }

        private static Type GetConnectionHelper(string ident)
        {
            try
            {
                Type type = connectionHelpers[ident] as Type;
                if (type != null)
                {
                    return type;
                }
            }
            catch
            {
            }
            return null;
        }
    }
}
