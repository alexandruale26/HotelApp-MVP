using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace HotelAppLibrary.Databases;

public class SQLDataAccess : ISQLDataAccess
{
    private readonly IConfiguration config;

    public SQLDataAccess(IConfiguration config)
    {
        this.config = config;
    }

    public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName, bool isStoredProcedure = false)
    {
        string connectionString = this.config.GetConnectionString(connectionStringName);
        CommandType commandType = CommandType.Text;

        if (isStoredProcedure == true)
        {
            commandType = CommandType.StoredProcedure;
        }

        using IDbConnection connection = new SqlConnection(connectionString);
        List<T> rows = connection.Query<T>(sqlStatement, parameters, commandType: commandType).ToList();

        return rows;
    }

    public void SaveData<T>(string sqlStatement, T parameters, string connectionStringName, bool isStoredProcedure = false)
    {
        string connectionString = this.config.GetConnectionString(connectionStringName);
        CommandType commandType = CommandType.Text;

        if (isStoredProcedure == true)
        {
            commandType = CommandType.StoredProcedure;
        }

        using IDbConnection connection = new SqlConnection(connectionString);
        connection.Execute(sqlStatement, parameters, commandType: commandType);
    }
}
