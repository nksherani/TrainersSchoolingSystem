USE [master]
GO

/****** Object:  Database [Trainers]    Script Date: 2/11/2019 1:19:59 AM ******/
CREATE DATABASE [Trainers]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Trainers', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Trainers.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Trainers_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Trainers_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

ALTER DATABASE [Trainers] SET COMPATIBILITY_LEVEL = 140
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Trainers].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Trainers] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [Trainers] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [Trainers] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [Trainers] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [Trainers] SET ARITHABORT OFF 
GO

ALTER DATABASE [Trainers] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [Trainers] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Trainers] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Trainers] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Trainers] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [Trainers] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [Trainers] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Trainers] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [Trainers] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Trainers] SET  ENABLE_BROKER 
GO

ALTER DATABASE [Trainers] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Trainers] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Trainers] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Trainers] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Trainers] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Trainers] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Trainers] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Trainers] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [Trainers] SET  MULTI_USER 
GO

ALTER DATABASE [Trainers] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Trainers] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Trainers] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Trainers] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [Trainers] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Trainers] SET QUERY_STORE = OFF
GO

ALTER DATABASE [Trainers] SET  READ_WRITE 
GO

