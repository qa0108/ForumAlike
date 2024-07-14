USE [master]
GO
/****** Object:  Database [ForumDB]    Script Date: 7/15/2024 2:12:07 AM ******/
CREATE DATABASE [ForumDB]
GO
ALTER DATABASE [ForumDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ForumDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ForumDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ForumDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ForumDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ForumDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ForumDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ForumDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ForumDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ForumDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ForumDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ForumDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ForumDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ForumDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ForumDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ForumDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ForumDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ForumDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ForumDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ForumDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ForumDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ForumDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ForumDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ForumDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ForumDB] SET RECOVERY FULL 
GO
ALTER DATABASE [ForumDB] SET  MULTI_USER 
GO
ALTER DATABASE [ForumDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ForumDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ForumDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ForumDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ForumDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ForumDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ForumDB', N'ON'
GO
ALTER DATABASE [ForumDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [ForumDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ForumDB]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 7/15/2024 2:12:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Posts]    Script Date: 7/15/2024 2:12:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts](
	[PostID] [int] IDENTITY(1,1) NOT NULL,
	[ThreadID] [int] NULL,
	[UserID] [int] NULL,
	[Content] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PostID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Replies]    Script Date: 7/15/2024 2:12:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Replies](
	[ReplyID] [int] IDENTITY(1,1) NOT NULL,
	[PostID] [int] NULL,
	[UserID] [int] NULL,
	[ParentReplyID] [int] NULL,
	[Content] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReplyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 7/15/2024 2:12:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Threads]    Script Date: 7/15/2024 2:12:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Threads](
	[ThreadID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL,
	[UserID] [int] NULL,
	[Title] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ThreadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/15/2024 2:12:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[RoleID] [int] NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedAt]) VALUES (1, N'General Discussion', N'Talk about anything and everything', CAST(N'2024-07-09T20:54:52.960' AS DateTime))
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedAt]) VALUES (2, N'Technical Support', N'Get help with technical issues', CAST(N'2024-07-09T20:54:52.960' AS DateTime))
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [CreatedAt]) VALUES (3, N'Coding', N'Discuss programming and software development', CAST(N'2024-07-09T20:54:52.960' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Posts] ON 
GO
INSERT [dbo].[Posts] ([PostID], [ThreadID], [UserID], [Content], [CreatedAt]) VALUES (1, 1, 1, N'Writing clean code is essential for maintainability. Here are some tips: 1. Use meaningful variable names. 2. Keep functions small and focused. 3. Comment your code wisely.', CAST(N'2024-07-09T20:54:52.963' AS DateTime))
GO
INSERT [dbo].[Posts] ([PostID], [ThreadID], [UserID], [Content], [CreatedAt]) VALUES (2, 2, 2, N'Python is a great language for beginners. Start by installing Python and working through some tutorials on basic syntax and concepts.', CAST(N'2024-07-09T20:54:52.963' AS DateTime))
GO
INSERT [dbo].[Posts] ([PostID], [ThreadID], [UserID], [Content], [CreatedAt]) VALUES (3, 3, 3, N'Debugging can be challenging. Use print statements to trace your code, and make use of debugging tools available in your IDE.', CAST(N'2024-07-09T20:54:52.963' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Posts] OFF
GO
SET IDENTITY_INSERT [dbo].[Replies] ON 
GO
INSERT [dbo].[Replies] ([ReplyID], [PostID], [UserID], [ParentReplyID], [Content], [CreatedAt]) VALUES (1, 1, 2, NULL, N'Thanks for the tips! I also find that keeping a consistent coding style helps.', CAST(N'2024-07-09T20:54:52.963' AS DateTime))
GO
INSERT [dbo].[Replies] ([ReplyID], [PostID], [UserID], [ParentReplyID], [Content], [CreatedAt]) VALUES (2, 1, 3, 1, N'I agree! Code readability is key. Do you use any specific style guides?', CAST(N'2024-07-09T20:54:52.963' AS DateTime))
GO
INSERT [dbo].[Replies] ([ReplyID], [PostID], [UserID], [ParentReplyID], [Content], [CreatedAt]) VALUES (3, 2, 1, NULL, N'I recommend checking out "Automate the Boring Stuff with Python" as a great starting resource.', CAST(N'2024-07-09T20:54:52.963' AS DateTime))
GO
INSERT [dbo].[Replies] ([ReplyID], [PostID], [UserID], [ParentReplyID], [Content], [CreatedAt]) VALUES (4, 3, 2, NULL, N'Good advice! I also like to use breakpoints and step through the code to see where things go wrong.', CAST(N'2024-07-09T20:54:52.963' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Replies] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'Admin')
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'Moderator')
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (3, N'User')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Threads] ON 
GO
INSERT [dbo].[Threads] ([ThreadID], [CategoryID], [UserID], [Title], [CreatedAt]) VALUES (1, 3, 1, N'Best Practices for Writing Clean Code', CAST(N'2024-07-09T20:54:52.963' AS DateTime))
GO
INSERT [dbo].[Threads] ([ThreadID], [CategoryID], [UserID], [Title], [CreatedAt]) VALUES (2, 3, 2, N'How to Get Started with Python', CAST(N'2024-07-09T20:54:52.963' AS DateTime))
GO
INSERT [dbo].[Threads] ([ThreadID], [CategoryID], [UserID], [Title], [CreatedAt]) VALUES (3, 3, 3, N'Debugging Tips and Tricks', CAST(N'2024-07-09T20:54:52.963' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Threads] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserID], [UserName], [PasswordHash], [Email], [RoleID], [CreatedAt]) VALUES (1, N'AdminUser', N'adminpasswordhash', N'admin@example.com', 1, CAST(N'2024-07-09T20:54:52.960' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [UserName], [PasswordHash], [Email], [RoleID], [CreatedAt]) VALUES (2, N'ModeratorUser', N'moderatorpasswordhash', N'moderator@example.com', 2, CAST(N'2024-07-09T20:54:52.960' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [UserName], [PasswordHash], [Email], [RoleID], [CreatedAt]) VALUES (3, N'RegularUser', N'userpasswordhash', N'user@example.com', 3, CAST(N'2024-07-09T20:54:52.960' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [UserName], [PasswordHash], [Email], [RoleID], [CreatedAt]) VALUES (4, N'string', N'123', N'test@gmail.com', 2, CAST(N'2024-07-09T16:53:16.023' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [UserName], [PasswordHash], [Email], [RoleID], [CreatedAt]) VALUES (5, N'test 2', N'123', N'test2@gmail.com', 3, CAST(N'2024-07-10T00:03:17.647' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Posts] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Replies] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Threads] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD FOREIGN KEY([ThreadID])
REFERENCES [dbo].[Threads] ([ThreadID])
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Replies]  WITH CHECK ADD FOREIGN KEY([ParentReplyID])
REFERENCES [dbo].[Replies] ([ReplyID])
GO
ALTER TABLE [dbo].[Replies]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[Posts] ([PostID])
GO
ALTER TABLE [dbo].[Replies]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Threads]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Threads]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
USE [master]
GO
ALTER DATABASE [ForumDB] SET  READ_WRITE 
GO
