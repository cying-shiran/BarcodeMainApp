using System.Data.SQLite;

namespace BarcodeMainApp.DataAccess
{
    public static class DatabaseInitializer
    {
        private const string CreateTablesSql = @"
            PRAGMA foreign_keys = ON;

            CREATE TABLE IF NOT EXISTS Category (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS Brand (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                DisplayName TEXT NOT NULL,
                Code TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS CategoryBrand (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CategoryId INTEGER NOT NULL,
                BrandId INTEGER NOT NULL,
                FOREIGN KEY (CategoryId) REFERENCES Category(Id),
                FOREIGN KEY (BrandId) REFERENCES Brand(Id)
            );

            CREATE TABLE IF NOT EXISTS ToolName (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS Coating (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BrandId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                FOREIGN KEY (BrandId) REFERENCES Brand(Id)
            );

            CREATE TABLE IF NOT EXISTS ProductCode (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                DisplayCode TEXT NOT NULL UNIQUE,
                ConvertedCode TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS Spec (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                DisplaySpec TEXT NOT NULL UNIQUE,
                Comp1 TEXT NOT NULL,
                Comp2 TEXT NOT NULL,
                Comp3 TEXT,
                ConvertedSpec TEXT NOT NULL,
                CalcBase INTEGER NOT NULL DEFAULT 16
            );

            CREATE TABLE IF NOT EXISTS Barcode (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                RouteCode TEXT NOT NULL,
                CategoryId INTEGER NOT NULL,
                BrandId INTEGER NOT NULL,
                ToolNameId INTEGER NOT NULL,
                CoatingId INTEGER NOT NULL,
                ProductCodeId INTEGER NOT NULL,
                SpecId INTEGER NOT NULL,
                Quantity INTEGER NOT NULL,
                FullBarcode TEXT NOT NULL UNIQUE,
                OrderDate TEXT NOT NULL,
                CreatedTime TEXT NOT NULL,
                FOREIGN KEY (CategoryId) REFERENCES Category(Id),
                FOREIGN KEY (BrandId) REFERENCES Brand(Id),
                FOREIGN KEY (ToolNameId) REFERENCES ToolName(Id),
                FOREIGN KEY (CoatingId) REFERENCES Coating(Id),
                FOREIGN KEY (ProductCodeId) REFERENCES ProductCode(Id),
                FOREIGN KEY (SpecId) REFERENCES Spec(Id)
            );

            CREATE TABLE IF NOT EXISTS PrintInfo (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BarcodeId INTEGER NOT NULL UNIQUE,
                RouteCode TEXT NOT NULL,
                DisplayCode TEXT NOT NULL,
                DisplayName TEXT NOT NULL,
                DisplaySpec TEXT NOT NULL,
                CoatingName TEXT,
                FullBarcode TEXT NOT NULL,
                IsPrinted INTEGER NOT NULL DEFAULT 0,
                LastPrintTime TEXT,
                FOREIGN KEY (BarcodeId) REFERENCES Barcode(Id)
            );

            CREATE TABLE IF NOT EXISTS PrintLog (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BarcodeId INTEGER NOT NULL,
                PrintedTime TEXT NOT NULL,
                IsSuccess INTEGER NOT NULL DEFAULT 1,
                Remark TEXT,
                FOREIGN KEY (BarcodeId) REFERENCES Barcode(Id)
            );
        ";

        public static void Initialize(string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(CreateTablesSql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}