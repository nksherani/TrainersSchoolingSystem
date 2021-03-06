USE [Trainers]
GO
/****** Object:  User [trainers]    Script Date: 4/28/2019 3:45:55 PM ******/
CREATE USER [trainers] FOR LOGIN [trainers] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [trainers]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [trainers]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [trainers]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [trainers]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [trainers]
GO
ALTER ROLE [db_datareader] ADD MEMBER [trainers]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [trainers]
GO
ALTER ROLE [db_denydatareader] ADD MEMBER [trainers]
GO
ALTER ROLE [db_denydatawriter] ADD MEMBER [trainers]
GO
/****** Object:  Schema [HangFire]    Script Date: 4/28/2019 3:45:55 PM ******/
CREATE SCHEMA [HangFire]
GO
/****** Object:  Table [dbo].[Arrear]    Script Date: 4/28/2019 3:45:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Arrear](
	[ArrearId] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NULL,
	[StaffId] [int] NULL,
	[Amount] [numeric](18, 0) NULL,
	[ArrearType] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ArrearId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 4/28/2019 3:45:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[ClassId] [int] IDENTITY(1,1) NOT NULL,
	[ClassName] [varchar](max) NULL,
	[Section] [varchar](max) NULL,
	[ClassAdvisor] [int] NULL,
	[Level] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ClassId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Configuration]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configuration](
	[ConfigurationId] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](max) NULL,
	[Value] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ConfigurationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DailyAttendance]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DailyAttendance](
	[DailyAttendanceId] [int] IDENTITY(1,1) NOT NULL,
	[StaffId] [int] NULL,
	[Status] [varchar](max) NULL,
	[AttendanceDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[DailyAttendanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Designation]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Designation](
	[DesignationId] [int] IDENTITY(1,1) NOT NULL,
	[DesignationName] [varchar](max) NULL,
	[Category] [varchar](max) NULL,
	[PaidLeaves] [int] NULL,
	[ShortLeavesScale] [int] NULL,
	[LateComingScale] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[DesignationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrolment]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrolment](
	[EnrolmentId] [int] IDENTITY(1,1) NOT NULL,
	[IsActive] [bit] NULL,
	[Student] [int] NULL,
	[GRNo] [varchar](max) NULL,
	[RollNo] [int] NULL,
	[Class] [int] NULL,
	[MonthlyFee] [numeric](18, 0) NULL,
	[AdmissionFee] [numeric](18, 0) NULL,
	[AnnualFee] [numeric](18, 0) NULL,
	[PaymentMode] [varchar](max) NULL,
	[LastClass] [varchar](max) NULL,
	[LastInstitude] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[EnrolmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fee]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fee](
	[FeeId] [int] IDENTITY(1,1) NOT NULL,
	[Class] [int] NULL,
	[FeeType] [varchar](max) NULL,
	[Amount] [numeric](18, 0) NULL,
	[Description] [varchar](max) NULL,
	[Year] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[FeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lookup]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lookup](
	[LookupId] [int] IDENTITY(1,1) NOT NULL,
	[LookupTypeId] [int] NULL,
	[LookupText] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LookupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LookupType]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupType](
	[LookupTypeId] [int] IDENTITY(1,1) NOT NULL,
	[LookupTypeName] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LookupTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaidFee]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaidFee](
	[PaidFeeId] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NULL,
	[ChallanNo] [int] NULL,
	[Description] [varchar](max) NULL,
	[CalculatedAmount] [numeric](18, 0) NULL,
	[ReceivedAmount] [numeric](18, 0) NULL,
	[PaymentDate] [datetime] NULL,
	[Month] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[PaidFeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parent]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parent](
	[ParentId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NULL,
	[CNIC] [varchar](max) NULL,
	[Profession] [varchar](max) NULL,
	[OrganizationType] [varchar](max) NULL,
	[Education] [varchar](max) NULL,
	[MonthlyIncome] [varchar](max) NULL,
	[Mobile] [varchar](max) NULL,
	[Landline] [varchar](max) NULL,
	[Address] [varchar](max) NULL,
	[OfficePhone] [varchar](max) NULL,
	[OfficeAddress] [varchar](max) NULL,
	[Email] [varchar](max) NULL,
	[Relation] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ParentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Salary]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Salary](
	[SalaryId] [int] IDENTITY(1,1) NOT NULL,
	[StaffId] [int] NULL,
	[BasicPay] [numeric](18, 0) NULL,
	[Bonus] [numeric](18, 0) NULL,
	[PF] [numeric](18, 0) NULL,
	[EOBI] [numeric](18, 0) NULL,
	[LoanDeduction] [numeric](18, 0) NULL,
	[GrossPay] [numeric](18, 0) NULL,
	[NetPay] [numeric](18, 0) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SalaryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalaryPayment]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalaryPayment](
	[SalaryPaymentId] [int] IDENTITY(1,1) NOT NULL,
	[ChallanNo] [int] NULL,
	[StaffId] [int] NULL,
	[Amount] [numeric](18, 0) NULL,
	[PaymentDate] [datetime] NULL,
	[Month] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SalaryPaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[StaffId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](max) NULL,
	[LastName] [varchar](max) NULL,
	[CNIC] [varchar](max) NULL,
	[Designation] [int] NULL,
	[Qualification] [varchar](max) NULL,
	[Gender] [varchar](max) NULL,
	[DateOfBirth] [datetime] NULL,
	[Age] [int] NULL,
	[FatherName] [varchar](max) NULL,
	[SpouseName] [varchar](max) NULL,
	[Mobile] [varchar](max) NULL,
	[LandLine] [varchar](max) NULL,
	[PostalCode] [varchar](max) NULL,
	[StreetAddress] [varchar](max) NULL,
	[City] [varchar](max) NULL,
	[JoiningDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StaffAttendance]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StaffAttendance](
	[StaffAttendanceId] [int] IDENTITY(1,1) NOT NULL,
	[WorkingDays] [int] NULL,
	[StaffId] [int] NULL,
	[Absents] [int] NULL,
	[ShortLeaves] [int] NULL,
	[LateComings] [int] NULL,
	[Month] [int] NULL,
	[Year] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[StaffAttendanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](max) NULL,
	[LastName] [varchar](max) NULL,
	[Gender] [varchar](max) NULL,
	[DateOfBirth] [datetime] NULL,
	[FamilyCode] [int] NULL,
	[Age] [int] NULL,
	[PlaceOfBirth] [varchar](max) NULL,
	[Religion] [varchar](max) NULL,
	[Nationality] [varchar](max) NULL,
	[MotherTongue] [varchar](max) NULL,
	[BloodGroup] [varchar](max) NULL,
	[BFormNo] [varchar](max) NULL,
	[AdmissionBasis] [varchar](max) NULL,
	[Father] [int] NULL,
	[Mother] [int] NULL,
	[Guardian] [int] NULL,
	[Mobile] [varchar](max) NULL,
	[LandLine] [varchar](max) NULL,
	[PostalCode] [varchar](max) NULL,
	[StreetAddress] [varchar](max) NULL,
	[City] [varchar](max) NULL,
	[JoiningDate] [datetime] NULL,
	[ClassAdmitted] [int] NULL,
	[EndDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject](
	[SubjectId] [int] IDENTITY(1,1) NOT NULL,
	[SubjectName] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SubjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubjectAssignment]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubjectAssignment](
	[SubjectAssignmentId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](max) NULL,
	[Teacher] [int] NULL,
	[Subject] [int] NULL,
	[Class] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SubjectAssignmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrainerUser]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainerUser](
	[TrainerUserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](max) NULL,
	[FirstName] [varchar](max) NULL,
	[LastName] [varchar](max) NULL,
	[Email] [varchar](max) NULL,
	[Mobile] [varchar](max) NULL,
	[Landline] [varchar](max) NULL,
	[Address] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TrainerUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Arrear]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Arrear]  WITH CHECK ADD FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[Arrear]  WITH CHECK ADD FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([StudentId])
GO
ALTER TABLE [dbo].[Arrear]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD FOREIGN KEY([ClassAdvisor])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Configuration]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Configuration]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[DailyAttendance]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[DailyAttendance]  WITH CHECK ADD FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[DailyAttendance]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Designation]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Designation]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Enrolment]  WITH CHECK ADD FOREIGN KEY([Class])
REFERENCES [dbo].[Class] ([ClassId])
GO
ALTER TABLE [dbo].[Enrolment]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Enrolment]  WITH CHECK ADD FOREIGN KEY([Student])
REFERENCES [dbo].[Student] ([StudentId])
GO
ALTER TABLE [dbo].[Enrolment]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Fee]  WITH CHECK ADD FOREIGN KEY([Class])
REFERENCES [dbo].[Class] ([ClassId])
GO
ALTER TABLE [dbo].[Fee]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Fee]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Lookup]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Lookup]  WITH CHECK ADD FOREIGN KEY([LookupTypeId])
REFERENCES [dbo].[LookupType] ([LookupTypeId])
GO
ALTER TABLE [dbo].[Lookup]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[LookupType]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[LookupType]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[PaidFee]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[PaidFee]  WITH CHECK ADD FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([StudentId])
GO
ALTER TABLE [dbo].[PaidFee]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Parent]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Parent]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Salary]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Salary]  WITH CHECK ADD FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[Salary]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[SalaryPayment]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[SalaryPayment]  WITH CHECK ADD FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[SalaryPayment]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD FOREIGN KEY([Designation])
REFERENCES [dbo].[Designation] ([DesignationId])
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[StaffAttendance]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[StaffAttendance]  WITH CHECK ADD FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[StaffAttendance]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([ClassAdmitted])
REFERENCES [dbo].[Class] ([ClassId])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([Father])
REFERENCES [dbo].[Parent] ([ParentId])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([Guardian])
REFERENCES [dbo].[Parent] ([ParentId])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([Mother])
REFERENCES [dbo].[Parent] ([ParentId])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Subject]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[Subject]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[SubjectAssignment]  WITH CHECK ADD FOREIGN KEY([Class])
REFERENCES [dbo].[Class] ([ClassId])
GO
ALTER TABLE [dbo].[SubjectAssignment]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[SubjectAssignment]  WITH CHECK ADD FOREIGN KEY([Subject])
REFERENCES [dbo].[Subject] ([SubjectId])
GO
ALTER TABLE [dbo].[SubjectAssignment]  WITH CHECK ADD FOREIGN KEY([Teacher])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[SubjectAssignment]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[TrainerUser]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
ALTER TABLE [dbo].[TrainerUser]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TrainerUser] ([TrainerUserId])
GO
/****** Object:  StoredProcedure [dbo].[ClasswiseCount]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClasswiseCount]
AS
	select count(std.StudentId) as StudentCount, cls.ClassName from Student as std 
left join Enrolment as enr 
on enr.Student = std.StudentId
and std.EndDate is null
left join Class cls
on cls.ClassId=enr.Class
group by cls.ClassName, cls.Level
order by cls.Level
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[GenerateFeeSlips]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GenerateFeeSlips]
	@StudentId int,
	@Month int
AS

DECLARE @annualFees int;
DECLARE @IsNew int;
DECLARE @currentMonth varchar(max);
SET @currentMonth = @Month;
IF @currentMonth = ( SELECT Value from Configuration where [Key] = 'FirstMonth')
	SET @annualFees = (select Amount From Fee where FeeType = 'AnnualFee');
ELSE
	set @annualFees = 0;
declare @challanno int;

set @challanno =(select top 1 sp.ChallanNo from PaidFee as sp where sp.StudentId=@StudentId and 
concat(@Month,DATENAME(year,GETDATE())) = 
concat(DATENAME(month,sp.Month),DATENAME(year,sp.Month))) ;
if @challanno is null
set @challanno =(select max(sp.ChallanNo)+1 from PaidFee as sp) ;  
if @challanno is null
set @challanno = 1

select @challanno as ChallanNo, std.StudentId, enr.GRNo,enr.RollNo,std.FirstName,std.LastName,pnt.Name as FatherName,
CONCAT( cls.ClassName,'-',cls.Section) as Class, 0 as AnnualFee,
isnull(arr.Amount,0) as ArrearAmount,
coalesce(arr.ArrearType,'Receivable') as ArrearType,
0 as UnpaidAmount,
0 as AdmissionFee

from (select * from student where StudentId = @StudentId) as std
left join Parent pnt
on pnt.ParentId = std.Father
left join (select * from enrolment where IsActive=1)  as enr
on std.StudentId = enr.Student
left join Arrear  as arr
on std.StudentId = arr.StudentId
left join Class as cls
on cls.ClassId=enr.Class
left join (
select StudentId, Sum(CalculatedAmount) as UnpaidAmount from PaidFee 
where ReceivedAmount is null and (Month<(Month(GETDATE())+1) and year(CreatedDate)=year(getdate()) 
or year(CreatedDate)<year(getdate())
)
and CONCAT( @Month, YEAR(GetDate())) <> CONCAT( Month, YEAR(CreatedDate))
group by StudentId 
) as unp
on unp.StudentId = std.StudentId


RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[GeneratePaySlips]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GeneratePaySlips]
	@StaffId int,
	@Month int

AS
declare @unpaidleaves int;
declare @paidleaves int;
declare @latefactor int;
declare @shortleavefactor int;
declare @totalabsents int;
declare @totalSLs int;
declare @totalLates int;
select @paidleaves = dsg1.PaidLeaves, @latefactor = dsg1.LateComingScale, 
@shortleavefactor = dsg1.ShortLeavesScale from Designation dsg1 where dsg1.DesignationId = 
(select Designation from Staff where StaffId = @StaffId);

set @totalabsents = (select count(*) from DailyAttendance dat1 where dat1.Status='Absent' 
and dat1.StaffId = @StaffId and Year(dat1.CreatedDate) = Year(getdate()));--3
set @totalSLs = (select count(*) from DailyAttendance dat1 where dat1.Status='ShortLeave' 
and dat1.StaffId = @StaffId and Year(dat1.CreatedDate) = Year(getdate()));--2
set @totalLates = (select count(*) from DailyAttendance dat1 where dat1.Status='LateArrival' 
and dat1.StaffId = @StaffId and Year(dat1.CreatedDate) = Year(getdate()));--0

--annual total leaves
if @shortleavefactor>0
set @totalabsents = @totalabsents + @totalSLs/@shortleavefactor;

if @latefactor>0
set @totalabsents = @totalabsents + @totalLates/@latefactor;

--total unpaidleaves
if @totalabsents>@paidleaves
set @unpaidleaves = abs(@totalabsents - @paidleaves);
else
set @unpaidleaves=0;

--leaves for current month
set @totalabsents = (select count(*) from DailyAttendance dat1 where dat1.Status='Absent' 
and dat1.StaffId = @StaffId and 
concat(@Month,DATENAME(year,GETDATE())) = 
concat(month(dat1.AttendanceDate),DATENAME(year,dat1.AttendanceDate)));

set @totalSLs = (select count(*) from DailyAttendance dat1 where dat1.Status='ShortLeave' 
and dat1.StaffId = @StaffId and 
concat(@Month,DATENAME(year,GETDATE())) = 
concat(month(dat1.AttendanceDate),DATENAME(year,dat1.AttendanceDate)));

set @totalLates = (select count(*) from DailyAttendance dat1 where dat1.Status='LateArrival' 
and dat1.StaffId = @StaffId and 
concat(@Month,DATENAME(year,GETDATE())) = 
concat(month(dat1.AttendanceDate),DATENAME(year,dat1.AttendanceDate)));

--monthly total leaves
if @shortleavefactor>0
set @totalabsents = @totalabsents + @totalSLs/@shortleavefactor;

if @latefactor>0
set @totalabsents = @totalabsents + @totalLates/@latefactor;

----------------------------------------------------------------------------------
----------------------------------------------------------------------------------
declare @deductableLeaves int;
if(@unpaidleaves>=@totalabsents)
set @deductableLeaves = @totalabsents;
else
--set @deductableLeaves = @totalabsents - @unpaidleaves;
set @deductableLeaves = @unpaidleaves;
declare @challanno int;
set @challanno =(select sp.ChallanNo from SALARYPAYMENT as sp where sp.StaffId=@StaffId and 
concat(@Month,DATENAME(year,GETDATE())) = 
concat(month(sp.Month),DATENAME(year,sp.CreatedDate))) ;  
if @challanno is null
set @challanno =(select max(sp.ChallanNo)+1 from SALARYPAYMENT as sp) ;  
if @challanno is null
set @challanno = 1
select @challanno as ChallanNo, concat(@Month,'-',DATENAME(year,GETDATE())) as Month,
GETDATE() as IssueDate,
stf.StaffId as StaffId, Concat(stf.FirstName,' ',stf.LastName) as Name,
dsg.DesignationName as Designation,stf.JoiningDate,
isnull(slr.BasicPay,0) as BasicPay, isnull(slr.Bonus,0) as Bonus,isnull(slr.PF,0) as PF,
isnull(slr.EOBI,0) as EOBI,isnull(slr.LoanDeduction,0) as LoanDeduction,
isnull(slr.GrossPay,0) as GrossPay,@deductableLeaves as UnpaidLeaves,
isnull(round(floor(slr.BasicPay*@deductableLeaves/26),-2),0) as AmountDeducted,
isnull(round(slr.NetPay,-2)-round(floor(slr.BasicPay*@deductableLeaves/26),-2),0) as NetPay

from Staff stf
left join  Designation dsg
on stf.Designation = dsg.DesignationId --and stf.StaffId = @StaffId
--and dsg.Category='Teaching' 
and stf.EndDate is null
left join Salary slr
on slr.StaffId = stf.StaffId

where stf.StaffId = @StaffId

RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[GetCurrentStaff]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCurrentStaff]
AS
		select 
stf.StaffId as StaffId, Concat(stf.FirstName,' ',stf.LastName) as Name,
stf.Gender, stf.DateOfBirth, isnull(stf.Age,0) as Age,
dsg.DesignationName as Designation,stf.JoiningDate,stf.Mobile,
stf.StreetAddress as Address,
case
when dat.Status = 'Absent' then 1
when dat.Status = 'Present' then 2
when dat.Status = 'ShortLeave' then 3
when dat.Status = 'LateArrival' then 4
when dat.Status is null then 0
end as Status
from Staff stf
left join  Designation dsg
on stf.Designation = dsg.DesignationId
--and dsg.Category='Teaching' 
and stf.EndDate is null
left join DailyAttendance dat
on dat.StaffId = stf.StaffId
and convert(varchar, getdate(), 2) = convert(varchar, dat.AttendanceDate, 2)

RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[GetUnpaidStudents]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUnpaidStudents]
AS
	
select std.StudentId,enr.GRNo,enr.RollNo,
std.FirstName,std.LastName,
CONCAT( cls.ClassName,'-',cls.Section) as Class, 
isnull(unp.UnpaidAmount,0)+isnull(arr.Amount,0) as UnpaidAmount, 
coalesce(arr.ArrearType,'Receivable') as ArrearType

from Student as std
left join Enrolment as enr
on enr.Student = std.StudentId and enr.IsActive=1
left join Class as cls
on cls.ClassId=enr.Class
left join (select StudentId, Sum(CalculatedAmount) as UnpaidAmount from PaidFee 
where ReceivedAmount is null
group by StudentId ) as unp
on unp.StudentId=std.StudentId
left join arrear arr
on unp.StudentId = arr.StudentId
where UnpaidAmount>0

RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[HardReset]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[HardReset]
AS

	delete from AspNetRoles;
	delete from AspNetUsers;
	delete from Enrolment;
	delete from PaidFee;
	delete from Arrear;
	delete from Student;
	delete from SubjectAssignment;
	delete from Class;
	delete from Subject;
	delete from StaffAttendance;
	delete from DailyAttendance;
	delete from SalaryPayment;
	delete from Salary;
	delete from Staff;
	delete from Parent;
	delete from Lookup;
	delete from LookupType;
	delete from Fee;
	delete from Configuration;
	delete from Fee;
	delete from Designation;
	delete from TrainerUser;

RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[MonthlyFeeCollection]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MonthlyFeeCollection]
	
AS
	select CONVERT(numeric,sum(temp.Y)) as Y,temp.X from(
select pf.ReceivedAmount as Y, concat(datename(month,pf.PaymentDate),'-',datename(year,pf.PaymentDate)) as X
from PaidFee  pf where ReceivedAmount is not null and datename(year,pf.PaymentDate) = datename(year,GETDATE())
) as temp group by temp.X
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[MonthlySalaryPayments]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MonthlySalaryPayments]
	
AS
	
select CONVERT(numeric, sum(temp.Y)) as Y,temp.X from(
select pf.Amount as Y, concat(datename(month, DATEADD(month, pf.Month-1, CAST('2008-01-01' AS datetime)))
,'-',datename(year,pf.PaymentDate)) as X
from SalaryPayment  pf where Amount is not null and datename(year,pf.PaymentDate) = datename(year,GETDATE())
) as temp group by temp.X


RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[SoftReset]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SoftReset]
AS

	delete from Enrolment;
	delete from PaidFee;
	delete from Arrear;
	delete from Student;
	delete from SubjectAssignment;
	delete from Class;
	delete from Subject;
	delete from StaffAttendance;
	delete from DailyAttendance;
	delete from SalaryPayment;
	delete from Salary;
	delete from Staff;
	delete from Parent;
	delete from Designation;

	DBCC CHECKIDENT ('Enrolment', RESEED, 0) 
	DBCC CHECKIDENT ('PaidFee', RESEED, 0) 
	DBCC CHECKIDENT ('Arrear', RESEED, 0) 
	DBCC CHECKIDENT ('Student', RESEED, 0) 
	DBCC CHECKIDENT ('SubjectAssignment', RESEED, 0) 
	DBCC CHECKIDENT ('Class', RESEED, 0) 
	DBCC CHECKIDENT ('Subject', RESEED, 0) 
	DBCC CHECKIDENT ('StaffAttendance', RESEED, 0) 
	DBCC CHECKIDENT ('DailyAttendance', RESEED, 0) 
	DBCC CHECKIDENT ('SalaryPayment', RESEED,0) 
	DBCC CHECKIDENT ('Salary', RESEED, 0) 
	DBCC CHECKIDENT ('Staff', RESEED, 0) 
	DBCC CHECKIDENT ('Parent', RESEED,0) 
	DBCC CHECKIDENT ('Designation', RESEED, 0) 
	
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[temp]    Script Date: 4/28/2019 3:45:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[temp]
	
AS
declare @TeacherId int;
declare @unpaidleaves int;
declare @paidleaves int;
declare @latefactor int;
declare @shortleavefactor int;
declare @totalabsents int;
declare @totalSLs int;
declare @totalLates int;
select @paidleaves = dsg1.PaidLeaves, @latefactor = dsg1.LateComingScale, 
@shortleavefactor = dsg1.ShortLeavesScale from Designation dsg1 where dsg1.DesignationId = 
(select Designation from Staff where StaffId = 7);

set @totalabsents = (select count(*) from DailyAttendance dat1 where dat1.Status='Absent' 
and dat1.StaffId = 7 and Year(dat1.CreatedDate) = Year(getdate()));--3
set @totalSLs = (select count(*) from DailyAttendance dat1 where dat1.Status='ShortLeave' 
and dat1.StaffId = 7 and Year(dat1.CreatedDate) = Year(getdate()));--2
set @totalLates = (select count(*) from DailyAttendance dat1 where dat1.Status='LateArrival' 
and dat1.StaffId = 7 and Year(dat1.CreatedDate) = Year(getdate()));--0

--annual total leaves
set @totalabsents = @totalabsents + @totalSLs/@shortleavefactor + @totalLates/@latefactor;

--total unpaidleaves
if @totalabsents>@paidleaves
set @unpaidleaves = abs(@totalabsents - @paidleaves);
else
set @unpaidleaves=0;
set @totalabsents = (select count(*) from DailyAttendance dat1 where dat1.Status='Absent' 
and dat1.StaffId = 7 and 
concat(DATENAME(month,GETDATE()),DATENAME(year,GETDATE())) = 
concat(DATENAME(month,dat1.AttendanceDate),DATENAME(year,dat1.AttendanceDate)));

set @totalSLs = (select count(*) from DailyAttendance dat1 where dat1.Status='ShortLeave' 
and dat1.StaffId = 7 and 
concat(DATENAME(month,GETDATE()),DATENAME(year,GETDATE())) = 
concat(DATENAME(month,dat1.AttendanceDate),DATENAME(year,dat1.AttendanceDate)));

set @totalLates = (select count(*) from DailyAttendance dat1 where dat1.Status='LateArrival' 
and dat1.StaffId = 7 and 
concat(DATENAME(month,GETDATE()),DATENAME(year,GETDATE())) = 
concat(DATENAME(month,dat1.AttendanceDate),DATENAME(year,dat1.AttendanceDate)));

--monthly total leaves
set @totalabsents = @totalabsents + @totalSLs/@shortleavefactor + @totalLates/@latefactor;

declare @deductableLeaves int;
if(@unpaidleaves>=@totalabsents)
set @deductableLeaves = @totalabsents;
else
set @deductableLeaves = @totalabsents - @unpaidleaves;

select @paidleaves as paidleaves, @deductableLeaves as totalleaves, @unpaidleaves as unpaidleaves
,@totalSLs as SLs

from DailyAttendance dat1

RETURN 0
GO
