/****** Object:  Database [PinoLandMVC]    Script Date: 2/13/2013 10:21:10 AM ******/
CREATE DATABASE [PinoLandMVC]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PinoLandMVC', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\PinoLandMVC.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PinoLandMVC_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\PinoLandMVC_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [PinoLandMVC] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PinoLandMVC].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PinoLandMVC] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PinoLandMVC] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PinoLandMVC] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PinoLandMVC] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PinoLandMVC] SET ARITHABORT OFF 
GO
ALTER DATABASE [PinoLandMVC] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PinoLandMVC] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [PinoLandMVC] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PinoLandMVC] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PinoLandMVC] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PinoLandMVC] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PinoLandMVC] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PinoLandMVC] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PinoLandMVC] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PinoLandMVC] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PinoLandMVC] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PinoLandMVC] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PinoLandMVC] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PinoLandMVC] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PinoLandMVC] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PinoLandMVC] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PinoLandMVC] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PinoLandMVC] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PinoLandMVC] SET RECOVERY FULL 
GO
ALTER DATABASE [PinoLandMVC] SET  MULTI_USER 
GO
ALTER DATABASE [PinoLandMVC] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PinoLandMVC] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PinoLandMVC] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PinoLandMVC] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PinoLandMVC', N'ON'
GO
/****** Object:  User [website]    Script Date: 2/13/2013 10:21:10 AM ******/
CREATE USER [website] FOR LOGIN [website] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [website]
GO
/****** Object:  StoredProcedure [dbo].[sp_SetLocationShape]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SetLocationShape]
	-- Add the parameters for the stored procedure here
	@LocationId as int
	,@lng1 as float, @lat1 as float
	,@lng2 as float, @lat2 as float
	,@lng3 as float, @lat3 as float
	,@lng4 as float, @lat4 as float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		UPDATE Location
		SET Shape = geography::Parse('POLYGON((' 
				 + cast(@lng1 as nvarchar(10)) + ' ' + cast(@lat1 as nvarchar(10)) +
		', ' + cast(@lng2 as nvarchar(10)) + ' ' + cast(@lat2 as nvarchar(10)) +
		', ' + cast(@lng3 as nvarchar(10)) + ' ' + cast(@lat3 as nvarchar(10)) +
		', ' + cast(@lng4 as nvarchar(10)) + ' ' + cast(@lat4 as nvarchar(10)) +
		', ' + cast(@lng1 as nvarchar(10)) + ' ' + cast(@lat1 as nvarchar(10)) +  -- repeat the first point to make it valid
		 '))')
		WHERE LocationId = @LocationId
	
END


GO
/****** Object:  Table [dbo].[Age]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Age](
	[AgeId] [int] IDENTITY(1,1) NOT NULL,
	[EconomyId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Probability] [float] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_Age] PRIMARY KEY CLUSTERED 
(
	[AgeId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Company]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[EconomyId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Company_Round]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company_Round](
	[RoundId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[StartingCash] [float] NOT NULL,
	[EndingCash] [float] NOT NULL,
	[Revenue] [float] NOT NULL,
	[Expenses] [float] NOT NULL,
	[FinanceActivity] [float] NOT NULL,
	[Submitted] [bit] NOT NULL,
	[SubmittedTimestamp] [datetime] NULL,
	[LastModifiedTimestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_Company_Round] PRIMARY KEY CLUSTERED 
(
	[RoundId] ASC,
	[CompanyId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Company_User]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company_User](
	[CompanyId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Company_User] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[EconomyId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Economy]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Economy](
	[EconomyId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Reference] [nvarchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[CurrentRoundId] [int] NULL,
 CONSTRAINT [PK_Economy] PRIMARY KEY CLUSTERED 
(
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Food_Good]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food_Good](
	[IndustryId] [int] NOT NULL,
	[GoodId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[TypeId] [int] NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
 CONSTRAINT [PK_Food_Good] PRIMARY KEY CLUSTERED 
(
	[IndustryId] ASC,
	[GoodId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Food_Good_Round]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food_Good_Round](
	[IndustryId] [int] NOT NULL,
	[GoodId] [int] NOT NULL,
	[RoundId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CapacityStart] [float] NOT NULL,
	[CapacityNew] [float] NOT NULL,
	[CapacitySold] [float] NOT NULL,
	[CapacityDecay] [float] NOT NULL,
	[CapacityEnd] [float] NOT NULL,
	[Production] [float] NOT NULL,
	[Price] [float] NOT NULL,
	[ActualSales] [float] NOT NULL,
	[CostOfProduction] [float] NOT NULL,
	[CostOfCapacity] [float] NOT NULL,
	[CostOfMaintenance] [float] NOT NULL,
	[CostOfInventory] [float] NOT NULL,
	[InventoryStart] [float] NOT NULL,
	[InventoryEnd] [float] NOT NULL,
	[IsRollover] [bit] NOT NULL,
 CONSTRAINT [PK_Food_Good_Round] PRIMARY KEY CLUSTERED 
(
	[IndustryId] ASC,
	[GoodId] ASC,
	[RoundId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Food_Household_Company_Round]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food_Household_Company_Round](
	[RoundId] [int] NOT NULL,
	[HouseholdId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[Reputation] [float] NOT NULL,
 CONSTRAINT [PK_Food_Household_Company_Round] PRIMARY KEY CLUSTERED 
(
	[RoundId] ASC,
	[HouseholdId] ASC,
	[CompanyId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Food_Industry]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food_Industry](
	[IndustryId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[EntryCostMean] [float] NOT NULL,
	[EntryCostStdDev] [float] NOT NULL,
	[CapacityCostMean] [float] NOT NULL,
	[CapacityCostStdDev] [float] NOT NULL,
	[MarginalCostMean] [float] NOT NULL,
	[MarginalCostStdDev] [float] NOT NULL,
	[CapacityDecayRate] [float] NOT NULL,
	[CapacityResaleRate] [float] NOT NULL,
	[MaintenanceCostMean] [float] NOT NULL,
	[MaintenanceCostStdDev] [float] NOT NULL,
	[CanHoldInventory] [bit] NOT NULL,
	[InventoryCostMean] [float] NOT NULL,
	[InventoryCostStdDev] [float] NOT NULL,
	[Elasticity] [float] NOT NULL,
 CONSTRAINT [PK_Food_Industry] PRIMARY KEY CLUSTERED 
(
	[IndustryId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Food_Industry_Age_Wealth]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food_Industry_Age_Wealth](
	[IndustryId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[AgeId] [int] NOT NULL,
	[WealthId] [int] NOT NULL,
	[Sigma] [float] NOT NULL,
	[SensitivityType] [float] NOT NULL,
	[SensitivityDistance] [float] NOT NULL,
 CONSTRAINT [PK_Food_Age_Wealth] PRIMARY KEY CLUSTERED 
(
	[IndustryId] ASC,
	[EconomyId] ASC,
	[AgeId] ASC,
	[WealthId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Food_Industry_Company]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food_Industry_Company](
	[CompanyId] [int] NOT NULL,
	[IndustryId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[EntryCost] [float] NOT NULL,
	[CapacityCost] [float] NOT NULL,
	[MarginalCost] [float] NOT NULL,
	[MaintenanceCost] [float] NOT NULL,
	[InventoryCost] [float] NOT NULL,
	[EntranceRoundId] [int] NULL,
 CONSTRAINT [PK_Food_Industry_Company] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[IndustryId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Food_Industry_Company_Round]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food_Industry_Company_Round](
	[RoundId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[IndustryId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[CostOfEntry] [float] NOT NULL,
	[CostOfCapacity] [float] NOT NULL,
	[CostOfProduction] [float] NOT NULL,
	[CostOfInventory] [float] NOT NULL,
	[CostOfMaintenance] [float] NOT NULL,
	[Revenue] [float] NOT NULL,
 CONSTRAINT [PK_Food_Industry_Company_Round] PRIMARY KEY CLUSTERED 
(
	[RoundId] ASC,
	[CompanyId] ASC,
	[IndustryId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Food_Industry_Good_Type]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food_Industry_Good_Type](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[IndustryId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Food_Industry_Good_Type] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC,
	[IndustryId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Food_Industry_Household_Company_Round]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food_Industry_Household_Company_Round](
	[RoundId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[IndustryId] [int] NOT NULL,
	[GoodId] [int] NOT NULL,
	[HouseholdId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[QuantityBought] [float] NOT NULL,
	[PriceBought] [float] NOT NULL,
	[Surplus] [float] NOT NULL,
 CONSTRAINT [PK_Food_Industry_Household_Company_Round] PRIMARY KEY CLUSTERED 
(
	[RoundId] ASC,
	[CompanyId] ASC,
	[IndustryId] ASC,
	[GoodId] ASC,
	[HouseholdId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Good]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Good](
	[IndustryId] [int] NOT NULL,
	[GoodId] [int] IDENTITY(1,1) NOT NULL,
	[EconomyId] [int] NOT NULL,
	[Identifier] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Good] PRIMARY KEY CLUSTERED 
(
	[IndustryId] ASC,
	[GoodId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Household]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Household](
	[HouseholdId] [int] IDENTITY(1,1) NOT NULL,
	[EconomyId] [int] NOT NULL,
	[Identifier] [nvarchar](50) NOT NULL,
	[LocationId] [int] NOT NULL,
	[AgeId] [int] NOT NULL,
	[WealthId] [int] NOT NULL,
	[ProfileId] [int] NOT NULL,
 CONSTRAINT [PK_Household] PRIMARY KEY CLUSTERED 
(
	[HouseholdId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IM_Role]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IM_Role](
	[RoleId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_IM_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IM_User]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IM_User](
	[UserId] [int] IDENTITY(100000,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](500) NULL,
	[LastName] [nvarchar](500) NULL,
	[Email] [nvarchar](500) NULL,
	[Password] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_IM_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IM_User_Role]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IM_User_Role](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_IM_User_Role] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Industry]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Industry](
	[IndustryId] [int] IDENTITY(1,1) NOT NULL,
	[EconomyId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Industry] PRIMARY KEY CLUSTERED 
(
	[IndustryId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Location]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[LocationId] [int] IDENTITY(1,1) NOT NULL,
	[EconomyId] [int] NOT NULL,
	[Identifier] [nvarchar](50) NOT NULL,
	[City] [nvarchar](50) NULL,
	[CenterX] [float] NOT NULL,
	[CenterY] [float] NOT NULL,
	[Shape] [geography] NULL,
	[ProfileId] [int] NOT NULL,
	[TotalPopulation] [int] NOT NULL,
	[Interactive] [bit] NOT NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Profile]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[ProfileId] [int] IDENTITY(1,1) NOT NULL,
	[EconomyId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Profile_Age_Wealth]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile_Age_Wealth](
	[ProfileId] [int] NOT NULL,
	[EconomyId] [int] NOT NULL,
	[AgeId] [int] NOT NULL,
	[WealthId] [int] NOT NULL,
	[Probability] [float] NOT NULL,
 CONSTRAINT [PK_Profile_Age_Wealth] PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC,
	[EconomyId] ASC,
	[AgeId] ASC,
	[WealthId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Round]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Round](
	[RoundId] [int] IDENTITY(1,1) NOT NULL,
	[EconomyId] [int] NOT NULL,
	[Identifier] [nvarchar](50) NOT NULL,
	[PreviousRoundId] [int] NULL,
	[Sequence] [int] NOT NULL,
 CONSTRAINT [PK_Round] PRIMARY KEY CLUSTERED 
(
	[RoundId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Wealth]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wealth](
	[WealthId] [int] IDENTITY(1,1) NOT NULL,
	[EconomyId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Probability] [float] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_Wealth] PRIMARY KEY CLUSTERED 
(
	[WealthId] ASC,
	[EconomyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[vw_Numbers]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Numbers] AS


with _x as (
select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 
							union all select 6 union all select 7 union all select 8 union all select 9
), _y as (
select ROW_NUMBER() over (order by a.a)
- 1 as n
from _x as a
cross join _x as b
cross join _x as c
cross join _x as d
--cross join _x as e
)
select * from  _y



GO
/****** Object:  View [dbo].[vw_LocationPoints]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_LocationPoints] AS

select EconomyId, LocationId, n.n as [sequence], l.Shape.STPointN(n.n).Lat as Lat, l.Shape.STPointN(n.n).Long as Long
FROM Location l
INNER JOIN vw_Numbers n
	ON	n.n >= 1
	-- Our map cells are polygons, which have N+1 points for N verticies (repeat the original)
	-- we don't want the original, so we skip it using < instead of <=
	AND n.n < l.Shape.STNumPoints()	



GO
/****** Object:  View [dbo].[vw_CashFlowStatement]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_CashFlowStatement] AS

SELECT TOP 100 PERCENT data.*, r.Sequence, r.Identifier 
FROM (
	SELECT 0 as GroupOrder, 'Beginning Balance' as GroupName, 1 as SectionOrder, 'Beginning Cash Balance' as Name, cr.StartingCash as Value, cr.RoundId, cr.CompanyId, cr.EconomyId
	FROM Company_Round cr

	UNION ALL

	SELECT 999 as GroupOrder, 'Ending Balance', 1 as SectionOrder, 'Ending Cash Balance' as Name, cr.EndingCash as Value, cr.RoundId, cr.CompanyId, cr.EconomyId
	FROM Company_Round cr

	UNION ALL

	SELECT 2 as GroupOrder, 'Operations', 1 as SectionOrder, unpvt.Name,-1 * unpvt.Value, unpvt.RoundId, unpvt.CompanyId, unpvt.EconomyId
	FROM (SELECT ficr.RoundId,
				ficr.CompanyId,
				ficr.EconomyId,
				ficr.CostOfProduction as Production,
				ficr.CostOfInventory as Inventory
				FROM Food_Industry_Company_Round ficr) pvt
				UNPIVOT (Value for Name in ([Production], [Inventory])) as unpvt
			
	UNION ALL

	SELECT 3 as GroupOrder, 'Investing', 2 as SectionOrder, unpvt.Name, -1 * unpvt.Value, unpvt.RoundId, unpvt.CompanyId, unpvt.EconomyId
	FROM (SELECT ficr.RoundId,
				ficr.CompanyId,
				ficr.EconomyId,
				ficr.CostOfEntry as Entry,
				ficr.CostOfCapacity as Capacity,
				ficr.CostOfMaintenance as Maintenance
				FROM Food_Industry_Company_Round ficr) pvt
				UNPIVOT (Value for Name in ([Entry], [Capacity], [Maintenance])) as unpvt

	UNION ALL
		
	SELECT 4 as GroupOrder, 'Financing' as GroupName, 1 as SectionOrder, 'Interest Gain (Loss)' as Name, FinanceActivity as Value, cr.RoundId, cr.CompanyId, cr.EconomyId
	FROM Company_Round cr

	) data
INNER JOIN Round r
	ON	r.RoundId = data.RoundId
	AND	r.EconomyId = data.EconomyId


GO
/****** Object:  View [dbo].[vw_FoodMapData]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[vw_FoodMapData] as 
select 
c.CompanyId
, fgr.RoundId
, fgr.EconomyId
, c.Name as CompanyName
, figt.TypeId as FoodTypeId
, figt.Name as FoodTypeName
, fgr.CapacityStart
, l.LocationId 
, l.Identifier as LocationIdentifier
from Food_Good_Round fgr
inner join Food_Good fg
	on	fg.IndustryId = fgr.IndustryId
	and	fg.GoodId = fgr.GoodId
	and fg.EconomyId = fgr.EconomyId
inner join Food_Industry_Good_Type figt
	on	figt.TypeId = fg.TypeId
	and	figt.IndustryId = fg.IndustryId
	and figt.EconomyId = fg.EconomyId
inner join Company c
	on	c.CompanyId = fg.CompanyId
	and c.EconomyId = fg.EconomyId
inner join Location l
	on	l.LocationId = fg.LocationId
	and	l.EconomyId = fg.EconomyId



GO
/****** Object:  View [dbo].[vw_HouseholdDemographics]    Script Date: 2/13/2013 10:21:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_HouseholdDemographics] AS 
SELECT h.EconomyId, h.HouseholdId, h.ProfileId, h.LocationId, h.AgeId, h.WealthId,
h.Identifier, p.Name as 'ProfileName', l.Identifier as 'Location'
,a.Name as 'AgeName', w.Name as 'WealthName'
FROM  Household h
inner join Age a
	on	a.AgeId = h.AgeId	
	and a.EconomyId = h.EconomyId
inner join Wealth w
	on	w.WealthId = h.WealthId
	and w.EconomyId = h.EconomyId
inner join Location l
	on	l.LocationId = h.LocationId
	and	l.EconomyId = h.EconomyId
inner join Profile p
	on	p.ProfileId = h.ProfileId
	and p.EconomyId = h.EconomyId


GO
ALTER TABLE [dbo].[Company_Round] ADD  CONSTRAINT [DF_Company_Round_FinanceActivity]  DEFAULT ((0)) FOR [FinanceActivity]
GO
ALTER TABLE [dbo].[Company_Round] ADD  CONSTRAINT [DF_Company_Round_Submitted]  DEFAULT ((0)) FOR [Submitted]
GO
ALTER TABLE [dbo].[Company_Round] ADD  CONSTRAINT [DF_Company_Round_LastModifiedTimestamp]  DEFAULT (getutcdate()) FOR [LastModifiedTimestamp]
GO
ALTER TABLE [dbo].[Economy] ADD  CONSTRAINT [DF_Economy_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Food_Good] ADD  CONSTRAINT [DF_Food_Good_Latitude]  DEFAULT ((0)) FOR [Latitude]
GO
ALTER TABLE [dbo].[Food_Good] ADD  CONSTRAINT [DF_Food_Good_Longitude]  DEFAULT ((0)) FOR [Longitude]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_CapacityStart]  DEFAULT ((0)) FOR [CapacityStart]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_CapacityNew]  DEFAULT ((0)) FOR [CapacityNew]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_CapacitySold]  DEFAULT ((0)) FOR [CapacitySold]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_CapacityDecay]  DEFAULT ((0)) FOR [CapacityDecay]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_CapacityEnd]  DEFAULT ((0)) FOR [CapacityEnd]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_Production]  DEFAULT ((0)) FOR [Production]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_ActualSales]  DEFAULT ((0)) FOR [ActualSales]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_CostOfProduction]  DEFAULT ((0)) FOR [CostOfProduction]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_CostOfCapacity]  DEFAULT ((0)) FOR [CostOfCapacity]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_CostOfMaintenance]  DEFAULT ((0)) FOR [CostOfMaintenance]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_CostOfInventory]  DEFAULT ((0)) FOR [CostOfInventory]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_StartInventory]  DEFAULT ((0)) FOR [InventoryStart]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_InventoryEnd]  DEFAULT ((0)) FOR [InventoryEnd]
GO
ALTER TABLE [dbo].[Food_Good_Round] ADD  CONSTRAINT [DF_Food_Good_Round_IsRollover]  DEFAULT ((0)) FOR [IsRollover]
GO
ALTER TABLE [dbo].[Food_Industry] ADD  CONSTRAINT [DF_Food_Industry_MaintenanceCostMean]  DEFAULT ((0)) FOR [MaintenanceCostMean]
GO
ALTER TABLE [dbo].[Food_Industry] ADD  CONSTRAINT [DF_Food_Industry_MaintenanceCostStdDev]  DEFAULT ((0)) FOR [MaintenanceCostStdDev]
GO
ALTER TABLE [dbo].[Food_Industry] ADD  CONSTRAINT [DF_Food_Industry_InventoryCostStdDev]  DEFAULT ((0)) FOR [InventoryCostStdDev]
GO
ALTER TABLE [dbo].[Food_Industry_Company] ADD  CONSTRAINT [DF_Food_Industry_Company_MaintenanceCost]  DEFAULT ((0)) FOR [MaintenanceCost]
GO
ALTER TABLE [dbo].[Food_Industry_Company] ADD  CONSTRAINT [DF_Food_Industry_Company_InventoryCost]  DEFAULT ((0)) FOR [InventoryCost]
GO
ALTER TABLE [dbo].[Food_Industry_Company_Round] ADD  CONSTRAINT [DF_Food_Industry_Company_Round_CostOfMaintenance]  DEFAULT ((0)) FOR [CostOfMaintenance]
GO
ALTER TABLE [dbo].[Food_Industry_Household_Company_Round] ADD  CONSTRAINT [DF_Food_Industry_Household_Company_Round_QuantityBought]  DEFAULT ((0)) FOR [QuantityBought]
GO
ALTER TABLE [dbo].[Food_Industry_Household_Company_Round] ADD  CONSTRAINT [DF_Food_Industry_Household_Company_Round_PriceBought]  DEFAULT ((0)) FOR [PriceBought]
GO
ALTER TABLE [dbo].[Food_Industry_Household_Company_Round] ADD  CONSTRAINT [DF_Food_Industry_Household_Company_Round_Surplus]  DEFAULT ((0)) FOR [Surplus]
GO
ALTER TABLE [dbo].[IM_User] ADD  CONSTRAINT [DF_IM_User_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[Location] ADD  CONSTRAINT [DF_Location_Population]  DEFAULT ((0)) FOR [TotalPopulation]
GO
ALTER TABLE [dbo].[Location] ADD  CONSTRAINT [DF_Location_Interactive]  DEFAULT ((1)) FOR [Interactive]
GO
ALTER TABLE [dbo].[Age]  WITH CHECK ADD  CONSTRAINT [FK_Age_Economy] FOREIGN KEY([EconomyId])
REFERENCES [dbo].[Economy] ([EconomyId])
GO
ALTER TABLE [dbo].[Age] CHECK CONSTRAINT [FK_Age_Economy]
GO
ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK_Company_Economy] FOREIGN KEY([EconomyId])
REFERENCES [dbo].[Economy] ([EconomyId])
GO
ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK_Company_Economy]
GO
ALTER TABLE [dbo].[Company_Round]  WITH CHECK ADD  CONSTRAINT [FK_Company_Round_Company] FOREIGN KEY([CompanyId], [EconomyId])
REFERENCES [dbo].[Company] ([CompanyId], [EconomyId])
GO
ALTER TABLE [dbo].[Company_Round] CHECK CONSTRAINT [FK_Company_Round_Company]
GO
ALTER TABLE [dbo].[Company_Round]  WITH CHECK ADD  CONSTRAINT [FK_Company_Round_Round] FOREIGN KEY([RoundId], [EconomyId])
REFERENCES [dbo].[Round] ([RoundId], [EconomyId])
GO
ALTER TABLE [dbo].[Company_Round] CHECK CONSTRAINT [FK_Company_Round_Round]
GO
ALTER TABLE [dbo].[Company_User]  WITH CHECK ADD  CONSTRAINT [FK_Company_User_Company] FOREIGN KEY([CompanyId], [EconomyId])
REFERENCES [dbo].[Company] ([CompanyId], [EconomyId])
GO
ALTER TABLE [dbo].[Company_User] CHECK CONSTRAINT [FK_Company_User_Company]
GO
ALTER TABLE [dbo].[Company_User]  WITH CHECK ADD  CONSTRAINT [FK_Company_User_IM_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[IM_User] ([UserId])
GO
ALTER TABLE [dbo].[Company_User] CHECK CONSTRAINT [FK_Company_User_IM_User]
GO
ALTER TABLE [dbo].[Economy]  WITH CHECK ADD  CONSTRAINT [FK_Economy_Round] FOREIGN KEY([CurrentRoundId], [EconomyId])
REFERENCES [dbo].[Round] ([RoundId], [EconomyId])
GO
ALTER TABLE [dbo].[Economy] CHECK CONSTRAINT [FK_Economy_Round]
GO
ALTER TABLE [dbo].[Food_Good]  WITH CHECK ADD  CONSTRAINT [FK_Food_Good_Company] FOREIGN KEY([CompanyId], [EconomyId])
REFERENCES [dbo].[Company] ([CompanyId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Good] CHECK CONSTRAINT [FK_Food_Good_Company]
GO
ALTER TABLE [dbo].[Food_Good]  WITH CHECK ADD  CONSTRAINT [FK_Food_Good_Food_Industry] FOREIGN KEY([IndustryId], [EconomyId])
REFERENCES [dbo].[Food_Industry] ([IndustryId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Good] CHECK CONSTRAINT [FK_Food_Good_Food_Industry]
GO
ALTER TABLE [dbo].[Food_Good]  WITH CHECK ADD  CONSTRAINT [FK_Food_Good_Food_Industry_Good_Type] FOREIGN KEY([TypeId], [IndustryId], [EconomyId])
REFERENCES [dbo].[Food_Industry_Good_Type] ([TypeId], [IndustryId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Good] CHECK CONSTRAINT [FK_Food_Good_Food_Industry_Good_Type]
GO
ALTER TABLE [dbo].[Food_Good]  WITH CHECK ADD  CONSTRAINT [FK_Food_Good_Good] FOREIGN KEY([IndustryId], [GoodId], [EconomyId])
REFERENCES [dbo].[Good] ([IndustryId], [GoodId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Good] CHECK CONSTRAINT [FK_Food_Good_Good]
GO
ALTER TABLE [dbo].[Food_Good]  WITH CHECK ADD  CONSTRAINT [FK_Food_Good_Location] FOREIGN KEY([LocationId], [EconomyId])
REFERENCES [dbo].[Location] ([LocationId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Good] CHECK CONSTRAINT [FK_Food_Good_Location]
GO
ALTER TABLE [dbo].[Food_Good_Round]  WITH CHECK ADD  CONSTRAINT [FK_Food_Good_Round_Company] FOREIGN KEY([CompanyId], [EconomyId])
REFERENCES [dbo].[Company] ([CompanyId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Good_Round] CHECK CONSTRAINT [FK_Food_Good_Round_Company]
GO
ALTER TABLE [dbo].[Food_Good_Round]  WITH CHECK ADD  CONSTRAINT [FK_Food_Good_Round_Food_Good] FOREIGN KEY([IndustryId], [GoodId], [EconomyId])
REFERENCES [dbo].[Food_Good] ([IndustryId], [GoodId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Good_Round] CHECK CONSTRAINT [FK_Food_Good_Round_Food_Good]
GO
ALTER TABLE [dbo].[Food_Good_Round]  WITH CHECK ADD  CONSTRAINT [FK_Food_Good_Round_Food_Industry_Company_Round] FOREIGN KEY([RoundId], [CompanyId], [IndustryId], [EconomyId])
REFERENCES [dbo].[Food_Industry_Company_Round] ([RoundId], [CompanyId], [IndustryId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Good_Round] CHECK CONSTRAINT [FK_Food_Good_Round_Food_Industry_Company_Round]
GO
ALTER TABLE [dbo].[Food_Good_Round]  WITH CHECK ADD  CONSTRAINT [FK_Food_Good_Round_Round] FOREIGN KEY([RoundId], [EconomyId])
REFERENCES [dbo].[Round] ([RoundId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Good_Round] CHECK CONSTRAINT [FK_Food_Good_Round_Round]
GO
ALTER TABLE [dbo].[Food_Household_Company_Round]  WITH NOCHECK ADD  CONSTRAINT [FK_Food_Household_Company_Round_Company] FOREIGN KEY([CompanyId], [EconomyId])
REFERENCES [dbo].[Company] ([CompanyId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Household_Company_Round] CHECK CONSTRAINT [FK_Food_Household_Company_Round_Company]
GO
ALTER TABLE [dbo].[Food_Household_Company_Round]  WITH NOCHECK ADD  CONSTRAINT [FK_Food_Household_Company_Round_Household] FOREIGN KEY([HouseholdId], [EconomyId])
REFERENCES [dbo].[Household] ([HouseholdId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Household_Company_Round] CHECK CONSTRAINT [FK_Food_Household_Company_Round_Household]
GO
ALTER TABLE [dbo].[Food_Household_Company_Round]  WITH NOCHECK ADD  CONSTRAINT [FK_Food_Household_Company_Round_Round] FOREIGN KEY([RoundId], [EconomyId])
REFERENCES [dbo].[Round] ([RoundId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Household_Company_Round] CHECK CONSTRAINT [FK_Food_Household_Company_Round_Round]
GO
ALTER TABLE [dbo].[Food_Industry]  WITH CHECK ADD  CONSTRAINT [FK_Food_Industry_Economy] FOREIGN KEY([EconomyId])
REFERENCES [dbo].[Economy] ([EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry] CHECK CONSTRAINT [FK_Food_Industry_Economy]
GO
ALTER TABLE [dbo].[Food_Industry]  WITH CHECK ADD  CONSTRAINT [FK_Food_Industry_Industry] FOREIGN KEY([IndustryId], [EconomyId])
REFERENCES [dbo].[Industry] ([IndustryId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry] CHECK CONSTRAINT [FK_Food_Industry_Industry]
GO
ALTER TABLE [dbo].[Food_Industry_Age_Wealth]  WITH CHECK ADD  CONSTRAINT [FK_Food_Age_Wealth_Age] FOREIGN KEY([AgeId], [EconomyId])
REFERENCES [dbo].[Age] ([AgeId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Age_Wealth] CHECK CONSTRAINT [FK_Food_Age_Wealth_Age]
GO
ALTER TABLE [dbo].[Food_Industry_Age_Wealth]  WITH CHECK ADD  CONSTRAINT [FK_Food_Age_Wealth_Economy] FOREIGN KEY([EconomyId])
REFERENCES [dbo].[Economy] ([EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Age_Wealth] CHECK CONSTRAINT [FK_Food_Age_Wealth_Economy]
GO
ALTER TABLE [dbo].[Food_Industry_Age_Wealth]  WITH CHECK ADD  CONSTRAINT [FK_Food_Age_Wealth_Wealth] FOREIGN KEY([WealthId], [EconomyId])
REFERENCES [dbo].[Wealth] ([WealthId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Age_Wealth] CHECK CONSTRAINT [FK_Food_Age_Wealth_Wealth]
GO
ALTER TABLE [dbo].[Food_Industry_Age_Wealth]  WITH CHECK ADD  CONSTRAINT [FK_Food_Industry_Age_Wealth_Food_Industry] FOREIGN KEY([IndustryId], [EconomyId])
REFERENCES [dbo].[Food_Industry] ([IndustryId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Age_Wealth] CHECK CONSTRAINT [FK_Food_Industry_Age_Wealth_Food_Industry]
GO
ALTER TABLE [dbo].[Food_Industry_Company]  WITH CHECK ADD  CONSTRAINT [FK_Food_Industry_Company_Company] FOREIGN KEY([CompanyId], [EconomyId])
REFERENCES [dbo].[Company] ([CompanyId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Company] CHECK CONSTRAINT [FK_Food_Industry_Company_Company]
GO
ALTER TABLE [dbo].[Food_Industry_Company]  WITH CHECK ADD  CONSTRAINT [FK_Food_Industry_Company_Food_Industry] FOREIGN KEY([IndustryId], [EconomyId])
REFERENCES [dbo].[Food_Industry] ([IndustryId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Company] CHECK CONSTRAINT [FK_Food_Industry_Company_Food_Industry]
GO
ALTER TABLE [dbo].[Food_Industry_Company]  WITH CHECK ADD  CONSTRAINT [FK_Food_Industry_Company_Round] FOREIGN KEY([EntranceRoundId], [EconomyId])
REFERENCES [dbo].[Round] ([RoundId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Company] CHECK CONSTRAINT [FK_Food_Industry_Company_Round]
GO
ALTER TABLE [dbo].[Food_Industry_Company_Round]  WITH CHECK ADD  CONSTRAINT [FK_Food_Industry_Company_Round_Company_Round] FOREIGN KEY([RoundId], [CompanyId], [EconomyId])
REFERENCES [dbo].[Company_Round] ([RoundId], [CompanyId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Company_Round] CHECK CONSTRAINT [FK_Food_Industry_Company_Round_Company_Round]
GO
ALTER TABLE [dbo].[Food_Industry_Company_Round]  WITH CHECK ADD  CONSTRAINT [FK_Food_Industry_Company_Round_Food_Industry_Company] FOREIGN KEY([CompanyId], [IndustryId], [EconomyId])
REFERENCES [dbo].[Food_Industry_Company] ([CompanyId], [IndustryId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Company_Round] CHECK CONSTRAINT [FK_Food_Industry_Company_Round_Food_Industry_Company]
GO
ALTER TABLE [dbo].[Food_Industry_Company_Round]  WITH CHECK ADD  CONSTRAINT [FK_Food_Industry_Company_Round_Round] FOREIGN KEY([RoundId], [EconomyId])
REFERENCES [dbo].[Round] ([RoundId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Company_Round] CHECK CONSTRAINT [FK_Food_Industry_Company_Round_Round]
GO
ALTER TABLE [dbo].[Food_Industry_Good_Type]  WITH CHECK ADD  CONSTRAINT [FK_Food_Industry_Good_Type_Food_Industry] FOREIGN KEY([IndustryId], [EconomyId])
REFERENCES [dbo].[Food_Industry] ([IndustryId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Good_Type] CHECK CONSTRAINT [FK_Food_Industry_Good_Type_Food_Industry]
GO
ALTER TABLE [dbo].[Food_Industry_Household_Company_Round]  WITH NOCHECK ADD  CONSTRAINT [FK_Food_Industry_Household_Company_Round_Food_Good] FOREIGN KEY([IndustryId], [GoodId], [EconomyId])
REFERENCES [dbo].[Food_Good] ([IndustryId], [GoodId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Household_Company_Round] CHECK CONSTRAINT [FK_Food_Industry_Household_Company_Round_Food_Good]
GO
ALTER TABLE [dbo].[Food_Industry_Household_Company_Round]  WITH NOCHECK ADD  CONSTRAINT [FK_Food_Industry_Household_Company_Round_Household] FOREIGN KEY([HouseholdId], [EconomyId])
REFERENCES [dbo].[Household] ([HouseholdId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Household_Company_Round] CHECK CONSTRAINT [FK_Food_Industry_Household_Company_Round_Household]
GO
ALTER TABLE [dbo].[Food_Industry_Household_Company_Round]  WITH NOCHECK ADD  CONSTRAINT [FK_Food_Industry_Household_Company_Round_Round] FOREIGN KEY([RoundId], [EconomyId])
REFERENCES [dbo].[Round] ([RoundId], [EconomyId])
GO
ALTER TABLE [dbo].[Food_Industry_Household_Company_Round] CHECK CONSTRAINT [FK_Food_Industry_Household_Company_Round_Round]
GO
ALTER TABLE [dbo].[Good]  WITH CHECK ADD  CONSTRAINT [FK_Good_Good] FOREIGN KEY([IndustryId], [EconomyId])
REFERENCES [dbo].[Industry] ([IndustryId], [EconomyId])
GO
ALTER TABLE [dbo].[Good] CHECK CONSTRAINT [FK_Good_Good]
GO
ALTER TABLE [dbo].[Household]  WITH NOCHECK ADD  CONSTRAINT [FK_Household_Age] FOREIGN KEY([AgeId], [EconomyId])
REFERENCES [dbo].[Age] ([AgeId], [EconomyId])
GO
ALTER TABLE [dbo].[Household] CHECK CONSTRAINT [FK_Household_Age]
GO
ALTER TABLE [dbo].[Household]  WITH NOCHECK ADD  CONSTRAINT [FK_Household_Household] FOREIGN KEY([EconomyId])
REFERENCES [dbo].[Economy] ([EconomyId])
GO
ALTER TABLE [dbo].[Household] CHECK CONSTRAINT [FK_Household_Household]
GO
ALTER TABLE [dbo].[Household]  WITH NOCHECK ADD  CONSTRAINT [FK_Household_Location] FOREIGN KEY([LocationId], [EconomyId])
REFERENCES [dbo].[Location] ([LocationId], [EconomyId])
GO
ALTER TABLE [dbo].[Household] CHECK CONSTRAINT [FK_Household_Location]
GO
ALTER TABLE [dbo].[Household]  WITH NOCHECK ADD  CONSTRAINT [FK_Household_Profile] FOREIGN KEY([ProfileId], [EconomyId])
REFERENCES [dbo].[Profile] ([ProfileId], [EconomyId])
GO
ALTER TABLE [dbo].[Household] CHECK CONSTRAINT [FK_Household_Profile]
GO
ALTER TABLE [dbo].[Household]  WITH NOCHECK ADD  CONSTRAINT [FK_Household_Wealth] FOREIGN KEY([WealthId], [EconomyId])
REFERENCES [dbo].[Wealth] ([WealthId], [EconomyId])
GO
ALTER TABLE [dbo].[Household] CHECK CONSTRAINT [FK_Household_Wealth]
GO
ALTER TABLE [dbo].[IM_User_Role]  WITH CHECK ADD  CONSTRAINT [FK_IM_User_Role_IM_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[IM_Role] ([RoleId])
GO
ALTER TABLE [dbo].[IM_User_Role] CHECK CONSTRAINT [FK_IM_User_Role_IM_Role]
GO
ALTER TABLE [dbo].[IM_User_Role]  WITH CHECK ADD  CONSTRAINT [FK_IM_User_Role_IM_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[IM_User] ([UserId])
GO
ALTER TABLE [dbo].[IM_User_Role] CHECK CONSTRAINT [FK_IM_User_Role_IM_User]
GO
ALTER TABLE [dbo].[Industry]  WITH CHECK ADD  CONSTRAINT [FK_Industry_Economy] FOREIGN KEY([EconomyId])
REFERENCES [dbo].[Economy] ([EconomyId])
GO
ALTER TABLE [dbo].[Industry] CHECK CONSTRAINT [FK_Industry_Economy]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Economy] FOREIGN KEY([EconomyId])
REFERENCES [dbo].[Economy] ([EconomyId])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Economy]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Profile] FOREIGN KEY([ProfileId], [EconomyId])
REFERENCES [dbo].[Profile] ([ProfileId], [EconomyId])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Profile]
GO
ALTER TABLE [dbo].[Profile]  WITH CHECK ADD  CONSTRAINT [FK_Profile_Economy] FOREIGN KEY([EconomyId])
REFERENCES [dbo].[Economy] ([EconomyId])
GO
ALTER TABLE [dbo].[Profile] CHECK CONSTRAINT [FK_Profile_Economy]
GO
ALTER TABLE [dbo].[Profile_Age_Wealth]  WITH CHECK ADD  CONSTRAINT [FK_Profile_Age_Wealth_Age] FOREIGN KEY([AgeId], [EconomyId])
REFERENCES [dbo].[Age] ([AgeId], [EconomyId])
GO
ALTER TABLE [dbo].[Profile_Age_Wealth] CHECK CONSTRAINT [FK_Profile_Age_Wealth_Age]
GO
ALTER TABLE [dbo].[Profile_Age_Wealth]  WITH CHECK ADD  CONSTRAINT [FK_Profile_Age_Wealth_Profile] FOREIGN KEY([ProfileId], [EconomyId])
REFERENCES [dbo].[Profile] ([ProfileId], [EconomyId])
GO
ALTER TABLE [dbo].[Profile_Age_Wealth] CHECK CONSTRAINT [FK_Profile_Age_Wealth_Profile]
GO
ALTER TABLE [dbo].[Profile_Age_Wealth]  WITH CHECK ADD  CONSTRAINT [FK_Profile_Age_Wealth_Wealth] FOREIGN KEY([WealthId], [EconomyId])
REFERENCES [dbo].[Wealth] ([WealthId], [EconomyId])
GO
ALTER TABLE [dbo].[Profile_Age_Wealth] CHECK CONSTRAINT [FK_Profile_Age_Wealth_Wealth]
GO
ALTER TABLE [dbo].[Round]  WITH CHECK ADD  CONSTRAINT [FK_Round_Round] FOREIGN KEY([PreviousRoundId], [EconomyId])
REFERENCES [dbo].[Round] ([RoundId], [EconomyId])
GO
ALTER TABLE [dbo].[Round] CHECK CONSTRAINT [FK_Round_Round]
GO
ALTER TABLE [dbo].[Wealth]  WITH CHECK ADD  CONSTRAINT [FK_Wealth_Economy] FOREIGN KEY([EconomyId])
REFERENCES [dbo].[Economy] ([EconomyId])
GO
ALTER TABLE [dbo].[Wealth] CHECK CONSTRAINT [FK_Wealth_Economy]
GO
ALTER DATABASE [PinoLandMVC] SET  READ_WRITE 
GO
