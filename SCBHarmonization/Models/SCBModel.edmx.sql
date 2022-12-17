
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/30/2021 18:11:59
-- Generated from EDMX file: C:\Users\ikenna.benedict\source\repos\NIBSSDataHarmonization.ikenna.benedict\SCBHarmonization\Models\SCBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SCBDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Credentials]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Credentials];
GO
IF OBJECT_ID(N'[dbo].[ELMAH_Error]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ELMAH_Error];
GO
IF OBJECT_ID(N'[dbo].[ExceptionLoggers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExceptionLoggers];
GO
IF OBJECT_ID(N'[dbo].[tbl_AccountClass]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_AccountClass];
GO
IF OBJECT_ID(N'[dbo].[tbl_AccountDesignation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_AccountDesignation];
GO
IF OBJECT_ID(N'[dbo].[tbl_AccountType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_AccountType];
GO
IF OBJECT_ID(N'[dbo].[tbl_Channel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Channel];
GO
IF OBJECT_ID(N'[dbo].[tbl_Currency]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Currency];
GO
IF OBJECT_ID(N'[dbo].[tbl_Dashboad]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Dashboad];
GO
IF OBJECT_ID(N'[dbo].[tbl_DataRetrieve]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_DataRetrieve];
GO
IF OBJECT_ID(N'[dbo].[tbl_DestInstType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_DestInstType];
GO
IF OBJECT_ID(N'[dbo].[tbl_Exception]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Exception];
GO
IF OBJECT_ID(N'[dbo].[tbl_Exceptionb]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Exceptionb];
GO
IF OBJECT_ID(N'[dbo].[tbl_Exceptionc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Exceptionc];
GO
IF OBJECT_ID(N'[dbo].[tbl_GetLoadedData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GetLoadedData];
GO
IF OBJECT_ID(N'[dbo].[tbl_HarmonizationRetrieve]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_HarmonizationRetrieve];
GO
IF OBJECT_ID(N'[dbo].[tbl_Harmortization]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Harmortization];
GO
IF OBJECT_ID(N'[dbo].[tbl_PaymentTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_PaymentTypes];
GO
IF OBJECT_ID(N'[dbo].[tbl_PreHarmonization]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_PreHarmonization];
GO
IF OBJECT_ID(N'[dbo].[tbl_ResponseSummary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ResponseSummary];
GO
IF OBJECT_ID(N'[dbo].[tbl_SrcInsType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_SrcInsType];
GO
IF OBJECT_ID(N'[dbo].[tbl_Status]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Status];
GO
IF OBJECT_ID(N'[dbo].[tbl_TopFive_ResponseSummary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_TopFive_ResponseSummary];
GO
IF OBJECT_ID(N'[dbo].[tbl_TransactionCount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_TransactionCount];
GO
IF OBJECT_ID(N'[dbo].[TransactionFailStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionFailStatus];
GO
IF OBJECT_ID(N'[dbo].[TransactionStatuses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionStatuses];
GO
IF OBJECT_ID(N'[dbo].[TransactionSuccessStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionSuccessStatus];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'tbl_AccountClass'
CREATE TABLE [dbo].[tbl_AccountClass] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccountClassTypes] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_AccountDesignation'
CREATE TABLE [dbo].[tbl_AccountDesignation] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccountDesignationTypes] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_AccountType'
CREATE TABLE [dbo].[tbl_AccountType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccountTypes] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_Channel'
CREATE TABLE [dbo].[tbl_Channel] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ChannelTypes] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_Currency'
CREATE TABLE [dbo].[tbl_Currency] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CurrencyTypes] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_DestInstType'
CREATE TABLE [dbo].[tbl_DestInstType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DestInstTypes] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_PaymentTypes'
CREATE TABLE [dbo].[tbl_PaymentTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PaymentTypes] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_SrcInsType'
CREATE TABLE [dbo].[tbl_SrcInsType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SrcInstTypes] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_Status'
CREATE TABLE [dbo].[tbl_Status] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_TransactionCount'
CREATE TABLE [dbo].[tbl_TransactionCount] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SucTransCount] int  NOT NULL,
    [FailTransCount] int  NOT NULL,
    [TransDate] datetime  NOT NULL
);
GO

-- Creating table 'TblTransactionTypes'
CREATE TABLE [dbo].[TblTransactionTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [TransactionGroupName] nvarchar(max)  NULL
);
GO

-- Creating table 'Credentials'
CREATE TABLE [dbo].[Credentials] (
    [id] int IDENTITY(1,1) NOT NULL,
    [USERNAME] nvarchar(50)  NOT NULL,
    [KEY] nvarchar(50)  NOT NULL,
    [IV] nvarchar(50)  NOT NULL,
    [Date] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'tbl_Dashboad'
CREATE TABLE [dbo].[tbl_Dashboad] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SucTransCount] int  NULL,
    [FailTransCount] int  NULL,
    [TransDate] datetime  NULL,
    [SuccessCount] int  NULL
);
GO

-- Creating table 'tbl_GetLoadedData'
CREATE TABLE [dbo].[tbl_GetLoadedData] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] varchar(13)  NULL,
    [Vat] varchar(13)  NULL,
    [Fee] varchar(13)  NULL,
    [TransID] varchar(50)  NULL,
    [SrcAcctNo] varchar(10)  NULL,
    [SrcInstCode] varchar(9)  NULL,
    [SrcInstBranchCode] varchar(6)  NULL,
    [SrcInstType] int  NOT NULL,
    [SrcInstUniqueID] varchar(15)  NULL,
    [DestAcctNo] varchar(10)  NULL,
    [DestInstCode] varchar(9)  NULL,
    [DestInstBranchCode] varchar(6)  NULL,
    [DestInstType] int  NULL,
    [DestInstUniqueID] varchar(15)  NULL,
    [PaymentType] int  NULL,
    [BankIncome] varchar(13)  NULL,
    [TransDate] datetime  NOT NULL,
    [PsspParty] varchar(35)  NULL,
    [AccountType] int  NULL,
    [AccountClass] int  NULL,
    [AccountDesignation] int  NULL,
    [Currency] int  NULL,
    [Channels] int  NULL,
    [TransactionTypeCode] varchar(13)  NULL,
    [PepDesignatedAccount] bit  NOT NULL,
    [CypherSecurityLevyExempt] bit  NOT NULL,
    [StampDutyExempt] bit  NOT NULL,
    [Inflow] bit  NOT NULL,
    [AdditionalField] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_HarmonizationRetrieve'
CREATE TABLE [dbo].[tbl_HarmonizationRetrieve] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] varchar(13)  NULL,
    [Vat] varchar(13)  NULL,
    [Fee] varchar(13)  NULL,
    [TransID] varchar(50)  NULL,
    [SrcAcctNo] varchar(10)  NULL,
    [SrcInstCode] varchar(9)  NULL,
    [SrcInstBranchCode] varchar(6)  NULL,
    [SrcInstType] int  NULL,
    [SrcInstUniqueID] varchar(15)  NULL,
    [DestAcctNo] varchar(10)  NULL,
    [DestInstCode] varchar(9)  NULL,
    [DestInstBranchCode] varchar(6)  NULL,
    [DestInstType] int  NULL,
    [DestInstUniqueID] varchar(15)  NULL,
    [PaymentType] int  NULL,
    [BankIncome] varchar(13)  NULL,
    [TransDate] datetime  NOT NULL,
    [PsspParty] varchar(35)  NULL,
    [AccountType] int  NULL,
    [AccountClass] int  NULL,
    [AccountDesignation] int  NULL,
    [Currency] int  NULL,
    [Channels] int  NULL,
    [TransactionTypeCode] varchar(13)  NULL,
    [PepDesignatedAccount] bit  NOT NULL,
    [CypherSecurityLevyExempt] bit  NOT NULL,
    [StampDutyExempt] bit  NOT NULL,
    [AdditionalField] nvarchar(max)  NULL,
    [Inflow] bit  NOT NULL
);
GO

-- Creating table 'tbl_Harmortization'
CREATE TABLE [dbo].[tbl_Harmortization] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] varchar(13)  NULL,
    [Vat] varchar(13)  NULL,
    [Fee] varchar(13)  NULL,
    [TransID] varchar(50)  NULL,
    [SrcAcctNo] varchar(10)  NULL,
    [SrcInstCode] varchar(9)  NULL,
    [SrcInstBranchCode] varchar(6)  NULL,
    [SrcInstType] int  NOT NULL,
    [SrcInstUniqueID] varchar(15)  NULL,
    [DestAcctNo] varchar(10)  NULL,
    [DestInstCode] varchar(9)  NULL,
    [DestInstBranchCode] varchar(6)  NULL,
    [DestInstType] int  NULL,
    [DestInstUniqueID] varchar(15)  NULL,
    [PaymentType] int  NULL,
    [BankIncome] varchar(13)  NULL,
    [TransDate] datetime  NOT NULL,
    [PsspParty] varchar(35)  NULL,
    [AccountType] int  NULL,
    [AccountClass] int  NULL,
    [AccountDesignation] int  NULL,
    [Currency] int  NULL,
    [Channels] int  NULL,
    [TransactionTypeCode] varchar(13)  NULL,
    [PepDesignatedAccount] bit  NOT NULL,
    [CypherSecurityLevyExempt] bit  NOT NULL,
    [StampDutyExempt] bit  NOT NULL,
    [AdditionalField] nvarchar(max)  NULL,
    [Inflow] bit  NOT NULL
);
GO

-- Creating table 'TransactionStatuses'
CREATE TABLE [dbo].[TransactionStatuses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(2)  NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Data] nvarchar(max)  NOT NULL,
    [SendDate] datetime  NOT NULL
);
GO

-- Creating table 'StatusesLastFives'
CREATE TABLE [dbo].[StatusesLastFives] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(2)  NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Data] nvarchar(max)  NOT NULL,
    [SendDate] datetime  NOT NULL
);
GO

-- Creating table 'tbl_ResponseSummary'
CREATE TABLE [dbo].[tbl_ResponseSummary] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Total] int  NULL,
    [Successful] int  NULL,
    [Failed] int  NULL,
    [SendDate] datetime  NULL
);
GO

-- Creating table 'tbl_TopFive_ResponseSummary'
CREATE TABLE [dbo].[tbl_TopFive_ResponseSummary] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Total] int  NULL,
    [Successful] int  NULL,
    [Failed] int  NULL,
    [SendDate] datetime  NULL
);
GO

-- Creating table 'TransactionFailStatus'
CREATE TABLE [dbo].[TransactionFailStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(2)  NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Data] nvarchar(max)  NOT NULL,
    [SendDate] datetime  NOT NULL
);
GO

-- Creating table 'TransactionSuccessStatus'
CREATE TABLE [dbo].[TransactionSuccessStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(2)  NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Data] nvarchar(max)  NOT NULL,
    [SendDate] datetime  NOT NULL
);
GO

-- Creating table 'tbl_Exception'
CREATE TABLE [dbo].[tbl_Exception] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] varchar(14)  NULL,
    [Vat] varchar(14)  NULL,
    [Fee] varchar(14)  NULL,
    [TransID] varchar(51)  NULL,
    [SrcAcctNo] varchar(11)  NULL,
    [SrcInstCode] varchar(10)  NULL,
    [SrcInstBranchCode] varchar(7)  NULL,
    [SrcInstType] int  NULL,
    [SrcInstUniqueID] varchar(16)  NULL,
    [DestAcctNo] varchar(11)  NULL,
    [DestInstCode] varchar(10)  NULL,
    [DestInstBranchCode] varchar(7)  NULL,
    [DestInstType] int  NULL,
    [DestInstUniqueID] varchar(16)  NULL,
    [PaymentType] int  NULL,
    [BankIncome] varchar(14)  NULL,
    [TransDate] varchar(51)  NOT NULL,
    [PsspParty] varchar(36)  NULL,
    [AccountType] int  NULL,
    [AccountClass] int  NULL,
    [AccountDesignation] int  NULL,
    [Currency] int  NULL,
    [Channels] int  NULL,
    [TransactionTypeCode] varchar(14)  NULL,
    [PepDesignatedAccount] bit  NOT NULL,
    [CypherSecurityLevyExempt] bit  NOT NULL,
    [StampDutyExempt] bit  NOT NULL,
    [ExceptionType] nvarchar(500)  NULL,
    [AdditionalField] nvarchar(max)  NULL,
    [Inflow] bit  NULL
);
GO

-- Creating table 'tbl_Exceptionc'
CREATE TABLE [dbo].[tbl_Exceptionc] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] varchar(14)  NULL,
    [Vat] varchar(14)  NULL,
    [Fee] varchar(14)  NULL,
    [TransID] varchar(51)  NULL,
    [SrcAcctNo] varchar(11)  NULL,
    [SrcInstCode] varchar(10)  NULL,
    [SrcInstBranchCode] varchar(7)  NULL,
    [SrcInstType] int  NULL,
    [SrcInstUniqueID] varchar(16)  NULL,
    [DestAcctNo] varchar(11)  NULL,
    [DestInstCode] varchar(10)  NULL,
    [DestInstBranchCode] varchar(7)  NULL,
    [DestInstType] int  NULL,
    [DestInstUniqueID] varchar(16)  NULL,
    [PaymentType] int  NULL,
    [BankIncome] varchar(11)  NULL,
    [TransDate] varchar(51)  NOT NULL,
    [PsspParty] varchar(36)  NULL,
    [AccountType] int  NULL,
    [AccountClass] int  NULL,
    [AccountDesignation] int  NULL,
    [Currency] int  NULL,
    [Channels] int  NULL,
    [TransactionTypeCode] varchar(14)  NULL,
    [PepDesignatedAccount] bit  NOT NULL,
    [CypherSecurityLevyExempt] bit  NOT NULL,
    [StampDutyExempt] bit  NOT NULL,
    [ExceptionType] nvarchar(500)  NULL,
    [AdditionalField] nvarchar(max)  NULL,
    [Inflow] bit  NOT NULL
);
GO

-- Creating table 'tbl_DataRetrieve'
CREATE TABLE [dbo].[tbl_DataRetrieve] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] varchar(13)  NULL,
    [Vat] varchar(13)  NULL,
    [Fee] varchar(13)  NULL,
    [TransID] varchar(50)  NULL,
    [SrcAcctNo] varchar(10)  NULL,
    [SrcInstCode] varchar(9)  NULL,
    [SrcInstBranchCode] varchar(6)  NULL,
    [SrcInstType] int  NULL,
    [SrcInstUniqueID] varchar(15)  NULL,
    [DestAcctNo] varchar(10)  NULL,
    [DestInstCode] varchar(9)  NULL,
    [DestInstBranchCode] varchar(6)  NULL,
    [DestInstType] int  NULL,
    [DestInstUniqueID] varchar(15)  NULL,
    [PaymentType] int  NULL,
    [BankIncome] varchar(13)  NULL,
    [TransDate] datetime  NOT NULL,
    [PsspParty] varchar(35)  NULL,
    [AccountType] int  NULL,
    [AccountClass] int  NULL,
    [AccountDesignation] int  NULL,
    [Currency] int  NULL,
    [Channels] int  NULL,
    [TransactionTypeCode] varchar(13)  NULL,
    [PepDesignatedAccount] bit  NOT NULL,
    [CypherSecurityLevyExempt] bit  NOT NULL,
    [StampDutyExempt] bit  NOT NULL,
    [AdditionalField] nvarchar(max)  NULL,
    [Inflow] bit  NOT NULL,
    [TenantId] int  NULL
);
GO

-- Creating table 'SaveLastLoadedDatas'
CREATE TABLE [dbo].[SaveLastLoadedDatas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TransactionDate] varchar(50)  NULL
);
GO

-- Creating table 'ExceptionLoggers'
CREATE TABLE [dbo].[ExceptionLoggers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ExceptionMessageWithSolution] varchar(max)  NOT NULL,
    [ControllerName] varchar(150)  NOT NULL,
    [ExceptionStackTrace] varchar(max)  NOT NULL,
    [LogTime] datetime  NOT NULL,
    [ActionName] varchar(150)  NULL,
    [InnerException] varchar(max)  NULL
);
GO

-- Creating table 'tbl_Exceptionb'
CREATE TABLE [dbo].[tbl_Exceptionb] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] varchar(14)  NULL,
    [Vat] varchar(14)  NULL,
    [Fee] varchar(14)  NULL,
    [TransID] varchar(51)  NULL,
    [SrcAcctNo] varchar(11)  NULL,
    [SrcInstCode] varchar(10)  NULL,
    [SrcInstBranchCode] varchar(7)  NULL,
    [SrcInstType] int  NOT NULL,
    [SrcInstUniqueID] varchar(16)  NULL,
    [DestAcctNo] varchar(11)  NULL,
    [DestInstCode] varchar(10)  NULL,
    [DestInstBranchCode] varchar(7)  NULL,
    [DestInstType] int  NOT NULL,
    [DestInstUniqueID] varchar(16)  NULL,
    [PaymentType] int  NOT NULL,
    [BankIncome] varchar(14)  NULL,
    [TransDate] varchar(51)  NOT NULL,
    [PsspParty] varchar(36)  NULL,
    [AccountType] int  NOT NULL,
    [AccountClass] int  NOT NULL,
    [AccountDesignation] int  NOT NULL,
    [Currency] int  NOT NULL,
    [Channels] int  NOT NULL,
    [TransactionTypeCode] varchar(14)  NULL,
    [PepDesignatedAccount] bit  NOT NULL,
    [CypherSecurityLevyExempt] bit  NOT NULL,
    [StampDutyExempt] bit  NOT NULL,
    [ExceptionType] nvarchar(500)  NULL,
    [AdditionalField] nvarchar(max)  NULL,
    [Inflow] bit  NOT NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'tbl_PreHarmonization'
CREATE TABLE [dbo].[tbl_PreHarmonization] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] varchar(14)  NULL,
    [Vat] varchar(14)  NULL,
    [Fee] varchar(14)  NULL,
    [TransID] varchar(51)  NULL,
    [SrcAcctNo] varchar(11)  NULL,
    [SrcInstCode] varchar(10)  NULL,
    [SrcInstBranchCode] varchar(7)  NULL,
    [SrcInstType] int  NULL,
    [SrcInstUniqueID] varchar(16)  NULL,
    [DestAcctNo] varchar(11)  NULL,
    [DestInstCode] varchar(10)  NULL,
    [DestInstBranchCode] varchar(7)  NULL,
    [DestInstType] int  NULL,
    [DestInstUniqueID] varchar(16)  NULL,
    [PaymentType] int  NULL,
    [BankIncome] varchar(14)  NULL,
    [TransDate] varchar(51)  NULL,
    [PsspParty] varchar(36)  NULL,
    [AccountType] int  NULL,
    [AccountClass] int  NULL,
    [AccountDesignation] int  NULL,
    [Currency] int  NULL,
    [Channels] int  NULL,
    [TransactionTypeCode] varchar(14)  NULL,
    [PepDesignatedAccount] bit  NULL,
    [CypherSecurityLevyExempt] bit  NULL,
    [StampDutyExempt] bit  NULL,
    [AdditionalField] nvarchar(max)  NULL,
    [Inflow] bit  NULL
);
GO

-- Creating table 'ELMAH_Error'
CREATE TABLE [dbo].[ELMAH_Error] (
    [ErrorId] uniqueidentifier  NOT NULL,
    [Application] nvarchar(60)  NOT NULL,
    [Host] nvarchar(50)  NOT NULL,
    [Type] nvarchar(100)  NOT NULL,
    [Source] nvarchar(60)  NOT NULL,
    [Message] nvarchar(500)  NOT NULL,
    [User] nvarchar(50)  NOT NULL,
    [StatusCode] int  NOT NULL,
    [TimeUtc] datetime  NOT NULL,
    [Sequence] int IDENTITY(1,1) NOT NULL,
    [AllXml] nvarchar(max)  NOT NULL,
    [VisitorsIPAddress] nvarchar(30)  NULL,
    [Manufacturer] nvarchar(30)  NULL
);
GO

-- Creating table 'tbl_SampleData'
CREATE TABLE [dbo].[tbl_SampleData] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] varchar(14)  NULL,
    [Vat] varchar(14)  NULL,
    [Fee] varchar(14)  NULL,
    [TransID] varchar(51)  NULL,
    [SrcAcctNo] varchar(11)  NULL,
    [SrcInstCode] varchar(10)  NULL,
    [SrcInstBranchCode] varchar(7)  NULL,
    [SrcInstType] int  NULL,
    [SrcInstUniqueID] varchar(16)  NULL,
    [DestAcctNo] varchar(11)  NULL,
    [DestInstCode] varchar(10)  NULL,
    [DestInstBranchCode] varchar(7)  NULL,
    [DestInstType] int  NULL,
    [DestInstUniqueID] varchar(16)  NULL,
    [PaymentType] int  NULL,
    [BankIncome] varchar(14)  NULL,
    [TransDate] varchar(51)  NOT NULL,
    [PsspParty] varchar(36)  NULL,
    [AccountType] int  NULL,
    [AccountClass] int  NULL,
    [AccountDesignation] int  NULL,
    [Currency] int  NULL,
    [Channels] int  NULL,
    [TransactionTypeCode] varchar(14)  NULL,
    [PepDesignatedAccount] bit  NOT NULL,
    [CypherSecurityLevyExempt] bit  NOT NULL,
    [StampDutyExempt] bit  NOT NULL,
    [AdditionalField] nvarchar(max)  NULL,
    [Inflow] bit  NOT NULL
);
GO

-- Creating table 'LoadingGuidLines'
CREATE TABLE [dbo].[LoadingGuidLines] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParameterName] varchar(50)  NULL,
    [Description] varchar(500)  NULL,
    [Comment] varchar(500)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'tbl_AccountClass'
ALTER TABLE [dbo].[tbl_AccountClass]
ADD CONSTRAINT [PK_tbl_AccountClass]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_AccountDesignation'
ALTER TABLE [dbo].[tbl_AccountDesignation]
ADD CONSTRAINT [PK_tbl_AccountDesignation]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_AccountType'
ALTER TABLE [dbo].[tbl_AccountType]
ADD CONSTRAINT [PK_tbl_AccountType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_Channel'
ALTER TABLE [dbo].[tbl_Channel]
ADD CONSTRAINT [PK_tbl_Channel]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_Currency'
ALTER TABLE [dbo].[tbl_Currency]
ADD CONSTRAINT [PK_tbl_Currency]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_DestInstType'
ALTER TABLE [dbo].[tbl_DestInstType]
ADD CONSTRAINT [PK_tbl_DestInstType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_PaymentTypes'
ALTER TABLE [dbo].[tbl_PaymentTypes]
ADD CONSTRAINT [PK_tbl_PaymentTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_SrcInsType'
ALTER TABLE [dbo].[tbl_SrcInsType]
ADD CONSTRAINT [PK_tbl_SrcInsType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_Status'
ALTER TABLE [dbo].[tbl_Status]
ADD CONSTRAINT [PK_tbl_Status]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_TransactionCount'
ALTER TABLE [dbo].[tbl_TransactionCount]
ADD CONSTRAINT [PK_tbl_TransactionCount]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TblTransactionTypes'
ALTER TABLE [dbo].[TblTransactionTypes]
ADD CONSTRAINT [PK_TblTransactionTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'Credentials'
ALTER TABLE [dbo].[Credentials]
ADD CONSTRAINT [PK_Credentials]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_Dashboad'
ALTER TABLE [dbo].[tbl_Dashboad]
ADD CONSTRAINT [PK_tbl_Dashboad]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_GetLoadedData'
ALTER TABLE [dbo].[tbl_GetLoadedData]
ADD CONSTRAINT [PK_tbl_GetLoadedData]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_HarmonizationRetrieve'
ALTER TABLE [dbo].[tbl_HarmonizationRetrieve]
ADD CONSTRAINT [PK_tbl_HarmonizationRetrieve]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_Harmortization'
ALTER TABLE [dbo].[tbl_Harmortization]
ADD CONSTRAINT [PK_tbl_Harmortization]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionStatuses'
ALTER TABLE [dbo].[TransactionStatuses]
ADD CONSTRAINT [PK_TransactionStatuses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StatusesLastFives'
ALTER TABLE [dbo].[StatusesLastFives]
ADD CONSTRAINT [PK_StatusesLastFives]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_ResponseSummary'
ALTER TABLE [dbo].[tbl_ResponseSummary]
ADD CONSTRAINT [PK_tbl_ResponseSummary]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_TopFive_ResponseSummary'
ALTER TABLE [dbo].[tbl_TopFive_ResponseSummary]
ADD CONSTRAINT [PK_tbl_TopFive_ResponseSummary]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionFailStatus'
ALTER TABLE [dbo].[TransactionFailStatus]
ADD CONSTRAINT [PK_TransactionFailStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionSuccessStatus'
ALTER TABLE [dbo].[TransactionSuccessStatus]
ADD CONSTRAINT [PK_TransactionSuccessStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_Exception'
ALTER TABLE [dbo].[tbl_Exception]
ADD CONSTRAINT [PK_tbl_Exception]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_Exceptionc'
ALTER TABLE [dbo].[tbl_Exceptionc]
ADD CONSTRAINT [PK_tbl_Exceptionc]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_DataRetrieve'
ALTER TABLE [dbo].[tbl_DataRetrieve]
ADD CONSTRAINT [PK_tbl_DataRetrieve]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SaveLastLoadedDatas'
ALTER TABLE [dbo].[SaveLastLoadedDatas]
ADD CONSTRAINT [PK_SaveLastLoadedDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExceptionLoggers'
ALTER TABLE [dbo].[ExceptionLoggers]
ADD CONSTRAINT [PK_ExceptionLoggers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_Exceptionb'
ALTER TABLE [dbo].[tbl_Exceptionb]
ADD CONSTRAINT [PK_tbl_Exceptionb]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_PreHarmonization'
ALTER TABLE [dbo].[tbl_PreHarmonization]
ADD CONSTRAINT [PK_tbl_PreHarmonization]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ErrorId] in table 'ELMAH_Error'
ALTER TABLE [dbo].[ELMAH_Error]
ADD CONSTRAINT [PK_ELMAH_Error]
    PRIMARY KEY CLUSTERED ([ErrorId] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_SampleData'
ALTER TABLE [dbo].[tbl_SampleData]
ADD CONSTRAINT [PK_tbl_SampleData]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LoadingGuidLines'
ALTER TABLE [dbo].[LoadingGuidLines]
ADD CONSTRAINT [PK_LoadingGuidLines]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------