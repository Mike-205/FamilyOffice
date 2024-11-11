using System;
using System.Data.SQLite;
using System.IO;

namespace Family_Office.DataAccess
{
    public static class DatabaseHelper
    {
        private static readonly string DbFilePath = "example5.db"; // Path to SQLite DB file

        public static SQLiteConnection GetConnection()
        {
            if (!File.Exists(DbFilePath))
            {
                SQLiteConnection.CreateFile(DbFilePath);
                Console.WriteLine("Database file created.");
            }
            return new SQLiteConnection($"Data Source={DbFilePath};Version=3;");
        }

        public static void InitializeDatabase()
        {
            CreateTables();
        }

        private static void CreateTables()
        {
            CreateUserTable();
            CreateFamilyOfficeTable();
            CreateFamilyMemberTable();
            CreateDocumentsTable();
            CreateGoalsTable();
            CreateCashAssetTable();
            CreateCurrencyTypesTable();
            CreateGoldInvestmentTable();
            CreatePropertyInvestmentTable();
            CreateSettingsTable();
            CreateThirdPartyTable();
            CreateBankAccountTable();
        }

        private static void ExecuteTableCreation(string query, string tableName)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"{tableName} table initialized.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error initializing {tableName} table: {ex.Message}");
                    }
                }
            }
        }

        public static void CreateUserTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS User (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT,
                    LastName TEXT,
                    Username TEXT DEFAULT 'admin',
                    Password TEXT DEFAULT '12345',
                    Profile BLOB,
                    DOB TEXT,
                    FamRelation TEXT,
                    Contact TEXT,
                    Email TEXT,
                    Phone TEXT
                );";
            ExecuteTableCreation(query, "User");
        }

        public static void CreateFamilyOfficeTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS FamilyOffice (
                    FamilyOfficeID INTEGER PRIMARY KEY AUTOINCREMENT,
                    FamilyOfficeName TEXT,
                    FamilyOfficeLogo BLOB,
                    EstablishmentDate TEXT,
                    Vision TEXT,
                    HeadOfFamily TEXT,
                    ChiefInvestOfficer TEXT,
                    Motto TEXT,
                    Purpose TEXT
                );";
            ExecuteTableCreation(query, "FamilyOffice");
        }

        public static void CreateFamilyMemberTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS FamilyMember (
                    FamilyMemberID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomMemberID TEXT,
                    FullName TEXT,
                    NickName TEXT,
                    ProfilePicture TEXT,
                    DOB TEXT,
                    FamilyRelation TEXT,
                    Role TEXT,
                    Status TEXT,
                    JoiningFamilyDate TEXT,
                    CountryCode1 Text,
                    Telephone1 TEXT,
                    CountryCode2 TEXT,
                    Telephone2 TEXT,
                    PersonalEmail TEXT,
                    FamilyEmail TEXT,
                    HomeAddress TEXT,
                    Nationality TEXT,
                    CurrentResidence TEXT,
                    PassPortNo INTEGER,
                    PassportExpiryDate TEXT,
                    EmiratesID INTEGER,
                    EmiratesIDExpiryDate TEXT,
                    AadharNo INTEGER,
                    PanCardNumber INTEGER,
                    EducationStatus TEXT,
                    MainSkills TEXT,
                    SecondarySkills TEXT,
                    EducatedFrom TEXT,
                    Document BLOB
                );";
            ExecuteTableCreation(query, "FamilyMember");
        }

        public static void CreateDocumentsTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Documents (
                    DocumentID INTEGER PRIMARY KEY AUTOINCREMENT,
                    DocumentType TEXT,
                    Document BLOB
                );";
            ExecuteTableCreation(query, "Documents");
        }

        public static void CreateGoalsTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Goals (
                    GoalID INTEGER PRIMARY KEY AUTOINCREMENT,
                    VisionYear INTEGER,
                    TargetAmount REAL,
                    RateOFCompounding REAL
                );";
            ExecuteTableCreation(query, "Goals");
        }

        public static void CreateCashAssetTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS CashAsset (
                    CashAssetID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Location TEXT,
                    InCareOf TEXT,
                    Currency TEXT DEFAULT 'USD',
                    Amount REAL
                );";
            ExecuteTableCreation(query, "CashAsset");
        }

        public static void CreateCurrencyTypesTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS CurrencyTypes (
                    CurrencyCode TEXT PRIMARY KEY,
                    CurrencyName TEXT,
                    CurrencySymbol TEXT,
                    ExchangeRate REAL
                );";
            ExecuteTableCreation(query, "CurrencyTypes");
        }

        public static void CreateGoldInvestmentTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS GoldInvestment (
                    GoldInvestmentID INTEGER PRIMARY KEY AUTOINCREMENT,
                    GoldType TEXT,
                    TotalGrams REAL,
                    Carat INTEGER,
                    TotalPerGram REAL,
                    StorageLocation TEXT,
                    Country TEXT,
                    AnnualStorageCost REAL,
                    AnnualMaintenanceCost REAL,
                    Document BLOB,
                    PurchaseDate TEXT,
                    InCareOf TEXT,
                    Currency TEXT
                );";
            ExecuteTableCreation(query, "GoldInvestment");
        }

        public static void CreatePropertyInvestmentTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS PropertyInvestment (
                    PropertyInvestmentID INTEGER PRIMARY KEY AUTOINCREMENT,
                    PropertyType TEXT,
                    Purpose TEXT,
                    Country TEXT,
                    Address TEXT,
                    Coordinates TEXT,
                    TotalArea INTEGER,
                    TotalPurchasePrice REAL,
                    PricePerUnit REAL,
                    AnnualMaintenanceCost REAL,
                    BrokerCost REAL,
                    Ownership TEXT,
                    Document BLOB,
                    UnitOfMeasurement TEXT
                );";
            ExecuteTableCreation(query, "PropertyInvestment");
        }

        public static void CreateSettingsTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Settings (
                    SettingID INTEGER PRIMARY KEY AUTOINCREMENT,
                    DateFormat TEXT DEFAULT 'DD/MM/YYYY',
                    BaseCurrency TEXT DEFAULT 'USD - US Dollar',
                    ShowExchangeRatesInList INTEGER DEFAULT 0,  -- 0 for false
                    ShowBaseCurrencyEquivalent INTEGER DEFAULT 0,  -- 0 for false
                    UseThousandsSeparator INTEGER DEFAULT 1,  -- 1 for true
                    DecimalPlaces INTEGER DEFAULT 2,
                    FontFamily TEXT DEFAULT 'Arial',
                    BodyFontSize INTEGER DEFAULT 12,
                    HeaderFontSize INTEGER DEFAULT 24,
                    PrimaryColor TEXT DEFAULT '#262693',
                    SecondaryColor TEXT DEFAULT '#000000',
                    BackgroundColor TEXT DEFAULT '#FFFFFF',
                    HighContrast INTEGER DEFAULT 0,  -- 0 for false
                    LargeTextMode INTEGER DEFAULT 0,  -- 0 for false
                    Scale INTEGER DEFAULT 100,
                    MarginSize INTEGER DEFAULT 5,
                    BorderWidth REAL DEFAULT 0.5,
                    BackgroundImage TEXT DEFAULT '/Images/backgroundImage1.jpg',
                    EnableHoverEffects INTEGER DEFAULT 0,  -- 0 for false
                    ShowFocusIndication INTEGER DEFAULT 0,  -- 0 for false
                    UseAnimation INTEGER DEFAULT 0  -- 0 for false
                );";
            ExecuteTableCreation(query, "Settings");
        }

        public static void CreateThirdPartyTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS ThirdParty (
                    ThirdPartyID INTEGER PRIMARY KEY AUTOINCREMENT,
                    PartyType TEXT,
                    PartyID TEXT,
                    FullName TEXT,
                    FirstAssociationDate TEXT,
                    Photo BLOB,
                    MainNumberCode TEXT,
                    MainMobileNumber TEXT,
                    AltNumberCode TEXT,
                    AltMobileNumber TEXT,
                    EmailAddress TEXT,
                    HomeAddress TEXT,
                    Notes TEXT,
                    Document BLOB,
                    CurrentStatus TEXT,
                    AadharCardNo INTEGER,
                    PanCardNo INTEGER,
                    RelationToUs TEXT,
                    OpeningBalance REAL,
                    CurrencyType TEXT
                );";
            ExecuteTableCreation(query, "ThirdParty");
        }

        public static void CreateBankAccountTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS BankAccount (
                    AccountID INTEGER PRIMARY KEY AUTOINCREMENT,
                    BankName TEXT,
                    Country TEXT,
                    AccountHolder TEXT,
                    SwiftCode TEXT,
                    AccountType TEXT,
                    BranchLocation TEXT,
                    AccountNumber INTEGER,
                    Nominee TEXT,
                    OpeningBalance REAL
                );";
            ExecuteTableCreation(query, "BankAccount");
        }
    }
}
