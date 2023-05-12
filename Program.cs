using EntityFrameworkCore.Triggers;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

//public sealed class SqlContext { }
//public sealed class SqlTriggerContext { }
static void main 
(object sender, System.EventArgs e)
{
  //  public sealed class SqlContext { }
//  public sealed class SqlTriggerContext {   }
string sqlInsert;
    string sqlUpdate;
    string sqlDropTriggers;
    string sqlWhere;
    string tableName;
    string sqlAllTables;
    string sqlPrimaryKeys;
    string strConn;

    //Using INFORMATION_SCHEMA to view the metadata of a SQL Server database
    //TABLE view lists all the tables in a particular database
    //KEY_COLUMN_USAGE table lists all the primary keys that exist in a particular database

    //Filter out any tables that Microsoft ships with versions of SQL Server
    sqlAllTables = "SELECT Table_name from INFORMATION_SCHEMA.TABLES " +
                   "WHERE Table_type = 'BASE TABLE' " +
                   "AND OBJECTPROPERTY(OBJECT_ID(TABLE_NAME),'IsMSShipped')=0";

    //Set value of myconnectionstring to your connection string
    string connectionString = "myconnectionstring";
    //SqlConnection con = new SqlConnection(connectionString);
    SqlConnection con = null;
    con.Open();

    //Declare a DataSet
    DataSet dsTrigger = new DataSet();
    DataTable dtTables = new DataTable();
   dtTables.PrimaryKey = new DataColumn[] {dtTables.Columns["CustLName"],
                                         dtTables.Columns["CustFName"]};

    // Or  

    DataColumn[] keyColumn = new DataColumn[2];
    keyColumn[0] = dtTables.Columns["CustLName"];
    keyColumn[1] = dtTables.Columns["CustFName"];
    dtTables.PrimaryKey = keyColumn;
    dsTrigger.Relations.Add("Tables_Keys", keyColumn[0], keyColumn[1]);
   

    //We have a list of tables that need triggers, let's loop through those tables and create the CREATE TRIGGER statements

    // foreach (DataRow childRow in
    // ("Tables_Keys"))
    string str1 = string.Empty;
    foreach (DataRow dr in dtTables.Rows)
    {
        foreach (DataColumn dc in dtTables.Columns)
        {
            str1 = dr[dc].ToString();
            {
                //Connect to the local, default instance of SQL Server.   
                //Server mysrv;
                //mysrv = new Server();
                //Reference the AdventureWorks2012 database.   
               // Database mydb;
               // mydb = mysrv.Databases["AdventureWorks2012"];
                //Declare a Table object variable and reference the Customer table.   
               // Table mytab;
               // mytab = mydb.Tables["Customer", "Sales"];
                //Define a Trigger object variable by supplying the parent table, schema ,and name in the constructor.   
             }
        }
    }
    {
        //Loop through the rows, accessing the column_name property of the childRow object, to build the complete WHERE clause for the trigger.
       
    }
    //Once the complete CREATE TRIGGER statement is built, we simply output the SQL statements to a .sql file
    TextWriter outSql = new StreamWriter("trigger.sql");
    //outSql.Write(content);
    outSql.Write(str1 + "n");
    outSql.Close();
    
 }
//Trigger trigger = null;
//Table triggertable = null;
//triggertable.Name("Stock");
//String strdb = "db1.mdf";
//Database db = strdb[..^];
//trigger.Builder = db;

//triggertable.Triggers.Add(strdb, trigger);

public class Trackable
{
    public DateTime Inserted { get; private set; }
    public DateTime tUpdated { get; private set; }
    public static DateTime Date { get; private set; }

    static Trackable()
    {
        Triggers<Trackable>.Inserting += entry => entry.Entity.Inserted = entry.Entity.Inserted =  Date;
        Triggers<Trackable>.Updating += entry => entry.Entity.tUpdated = entry.Entity.tUpdated = DateTime.UtcNow;

    }
}

public class Product_Id : Trackable
{
    public Int64 Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
}

public class Access_Token : Trackable { 
     public Int64 id { get; private set; }                     
}

public class Context : DbContextWithTriggers
{
    public DbSet<Product_Id> Stock_Name { get; set; }
    public DbSet<Access_Token> Tokens { get; set; }
}

//public class TriggerCollection
//{ 

// public  ServiceCollection.AddSingleton(typeof(ITriggers<>, typeof(Triggers<>));
// public ServiceCollection..AddSingleton(typeof(ITriggers<>), typeof(Triggers<>));
//  public ServiceCollection.AddSingleton(typeof(ITriggers), typeof(Triggers));

//Triggers<Product_Id, Context>().GlobalInserted.Add<IServiceBus>(,
//    entry => entry.Service.Broadcast("Inserted", entry.Entity)
//);

//Triggers<Person, Context>().GlobalInserted.Add<(IServiceBus Bus, IServiceX X)>(
//    entry => {
//        entry.Service.Bus.Broadcast("Inserted", entry.Entity);
//        entry.Service.X.DoSomething();
//    }
//);
//}

///public class ProductContext : DbContext
//{
//  public DbSet<Category> Categories { get; set; }
// public DbSet<Product> Products { get; set; }
//}
public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<Context>();
        services.AddTriggers();
        //  String strdb = "db1.mdf";
        // Database db = strdb[..^];
        //  DbContext db1 = null; 
    }

    //public void Configure app, Microsoft.Extensions.Hosting.IHostEnvironment env)
    public static void ConnDB()
    {
        //  String[] strdb = new string[] { "db1", ".mdf" };
        //  Database db =  null;
        ///  db. = strdb;
        //  DbContext db1 = null;

        String str;
        SqlConnection myConn = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master");

        str = "CREATE DATABASE MyDatabase ON PRIMARY " +
         "(NAME = MyDatabase_Data, " +
         "FILENAME = 'C:\\MyDatabaseData.mdf', " +
         "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)" +
         "LOG ON (NAME = MyDatabase_Log, " +
         "FILENAME = 'C:\\MyDatabaseLog.ldf', " +
         "SIZE = 1MB, " +
         "MAXSIZE = 5MB, " +
         "FILEGROWTH = 10%)";

        SqlCommand myCommand = new SqlCommand(str, myConn);
        try
        {
            myConn.Open();
            myCommand.ExecuteNonQuery();
            //   MessageBox.Show("DataBase is Created Successfully", "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (System.Exception ex)
        {
            // MessageBox.Show(ex.ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        finally
        {


            if (myConn.State == ConnectionState.Open)
            {
                myConn.Close();

            }
        }
        //   Database mydb = null;
        //   Context myContext = null;
        }
        public void ConfigureTriggers(IInsertingTriggerEvent<Product_Id, Context> app2, Microsoft.Extensions.Hosting.IHostEnvironment env)
        {

            bool x = default(bool);
            IInsertingTriggerEvent<Product_Id, Context> app1 = null;
           // IInsertingTriggerEvent<Product_Id, Context> app2 = null;
         //   var builder1 = typeof(EventId);
         ///   var val = default(IInsertingTriggerEvent<Product_Id, Context>);
        /// var methodinfo =     app2.GetType().GetMethod("Field", 1, new Type[] { typeof(IInsertingTriggerEvent<Product_Id, Context>), builder1 });
        //   app2.Add<Product_Id, Context> builder;
       DbContext dbContext = null;


        // EventHandler builder1 = null;

        //   var val1 =   val.GetType();
        //   dynamic val2 = app2.GetType().GetProperty("Value").GetValue(app2
        //   , null);
        //  var val3 = default(val2);
        //     var type1 = typeof(IInsertingTriggerEvent<Product_Id,Context>).MakeGenericType(typeof(Product_Id));
        //    object v2 = Activator.CreateInstance(type1);

        int id = 1;
        Context context = null;
       

        // (app2).Add(System.Action<IInsertingEntry
        Product_Id product_id = null;
        
        IInsertingEntry<Product_Id,Context> builder1 = null;
      //  app2.Add(product_id);
             //  builder1.Service(Triggers.Inserting)
              //   entry => Console.WriteLine(entry.Entity.FirstName)
     

        // receive injected services inside your handler, either with just a single service type or with a value tuple of services
        // builder.Triggers<Product_Id, Context>().GlobalInserted.Add<IServiceBus>(
        //   entry => entry.Service.Broadcast("Inserted", entry.Entity)
        //  );
        // builder.Triggers<Product_Id, Context>().GlobalInserted.Add<(IServiceBus Bus, IServiceX X)>(
        // entry => 
        //   entry.Service.Bus.Broadcast("Inserted", entry.Entity);
        // entry.Service.X.DoSomething();
        //


    }
        

      
    }
//


namespace ConfigureTriggers
{

    class ProgramTriggers
    {

        //   public sealed class SqlContext { }
        //   public sealed class SqlTriggerContext { }
        //  [System.Serializable]
        //   public class TriggerAction : Microsoft.SqlServer.TransactSql.ScriptDom.TSqlFragment { }
       // public Microsoft.SqlServer.Server.TriggerAction TriggerAction { get; }
        static void Main(string[] args)
        {

            //    [SqlTrigger(Name = @"TableAudit", Target = "[dbo].[Users]", Event = "FOR INSERT, DELETE")]
            // Trigger mytrigger = null;
            // mytrigger.GetDatabaseName();





            //SqlCommand command = new SqlCommand();
            String cmd = "CREATE TRIGGER StockTrigger ON { Stock   }        { FOR  } { [INSERT][ ,][UPDATE][ ,][DELETE] } ";
            SqlTriggerContext triggContext =new SqlTriggerContext();
            SqlCommand command = new SqlCommand(cmd);
            SqlDataReader reader;
            reader = command.ExecuteReader();
            SqlConnection connection
                           = new SqlConnection(@"context connection=true");
            connection.Open();
            cmd = "Select * From STOCK";
            command = new SqlCommand(cmd);
            reader = command.ExecuteReader();
            reader.Read();
            // Retrieve data from inserted rows
            reader.Close();

                //  switch (triggContext.TriggerAction)
              //  {
              // Insert.
              //      case TriggerAction.Insert:

                //        using (SqlConnection connection
                //             = new SqlConnection(@"context connection=true"))
                //    {
                //      // Open the context connection.
                //    connection.Open();

                // Get the inserted row.
                //  command = new SqlCommand(@"SELECT * FROM INSERTED;",
                //                                      connection);
                //  reader = command.ExecuteReader();
                //  reader.Read();

                // Retrieve data from inserted row.

                //  reader.Close();
                    }
        //  break;

        // Delete.
        // case TriggerAction.Delete:
        //   using (SqlConnection connection
        //        = new SqlConnection(@"context connection=true"))
        //  {
        // Open the context connection.
        //    connection.Open();

        // Get the deleted rows.
        //  command = new SqlCommand(@"SELECT * FROM DELETED;",
        //                         connection);
        //  reader = command.ExecuteReader();

        //  if (reader.HasRows)
        //  {
        //    while (reader.Read())
        //               {
        //      // Retrieve data from deleted rows.
        //}

        //       reader.Close();
        // }
        // else
        // {
        // No rows affected.
        //  }//
        // }

        //  break;
        // }

        // }

    }
    public sealed class SqlContext { }
    public sealed class SqlTriggerContext { }
}