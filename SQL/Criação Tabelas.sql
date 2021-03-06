USE [Escola]
GO
/****** Object:  Table [dbo].[Aluno]    Script Date: 17/05/2022 11:23:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aluno](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPessoa] [int] NOT NULL,
	[IdTurma] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Boletim]    Script Date: 17/05/2022 11:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Boletim](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdAluno] [int] NOT NULL,
	[IdGradeAula] [int] NOT NULL,
	[DescricaoAvaliacao] [varchar](100) NOT NULL,
	[Nota] [decimal](4, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Escola]    Script Date: 17/05/2022 11:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Escola](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](200) NOT NULL,
	[Endereco] [varchar](200) NOT NULL,
	[Numero] [decimal](6, 0) NOT NULL,
	[Complemento] [varchar](100) NULL,
	[Bairro] [varchar](100) NOT NULL,
	[Cidade] [varchar](100) NOT NULL,
	[Estado] [varchar](50) NOT NULL,
	[Pais] [varchar](20) NOT NULL,
	[CEP] [varchar](15) NOT NULL,
	[Telefone] [varchar](15) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GradeAula]    Script Date: 17/05/2022 11:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GradeAula](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTurma] [int] NOT NULL,
	[IdProfessor] [int] NOT NULL,
	[IdMateria] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Materia]    Script Date: 17/05/2022 11:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Materia](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pessoa]    Script Date: 17/05/2022 11:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pessoa](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](200) NOT NULL,
	[CPF] [varchar](14) NOT NULL,
	[RG] [varchar](13) NOT NULL,
	[Telefone] [varchar](15) NOT NULL,
	[Endereco] [varchar](200) NOT NULL,
	[Numero] [decimal](6, 0) NOT NULL,
	[Complemento] [varchar](100) NULL,
	[Bairro] [varchar](100) NOT NULL,
	[Cidade] [varchar](100) NOT NULL,
	[Estado] [varchar](50) NOT NULL,
	[Pais] [varchar](20) NOT NULL,
	[CEP] [varchar](15) NOT NULL,
	[DataNascimento] [date] NOT NULL,
	[NomeMae] [varchar](200) NULL,
	[NomePai] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Professor]    Script Date: 17/05/2022 11:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Professor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPessoa] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Turma]    Script Date: 17/05/2022 11:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Turma](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdEscola] [int] NOT NULL,
	[Tipo] [varchar](100) NOT NULL,
	[Descricao] [varchar](100) NOT NULL,
	[Serie] [varchar](50) NOT NULL,
	[Ano] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Aluno]  WITH CHECK ADD  CONSTRAINT [fk_Aluno_Pessoa] FOREIGN KEY([IdPessoa])
REFERENCES [dbo].[Pessoa] ([Id])
GO
ALTER TABLE [dbo].[Aluno] CHECK CONSTRAINT [fk_Aluno_Pessoa]
GO
ALTER TABLE [dbo].[Aluno]  WITH CHECK ADD  CONSTRAINT [fk_Aluno_Turma] FOREIGN KEY([IdTurma])
REFERENCES [dbo].[Turma] ([Id])
GO
ALTER TABLE [dbo].[Aluno] CHECK CONSTRAINT [fk_Aluno_Turma]
GO
ALTER TABLE [dbo].[Boletim]  WITH CHECK ADD  CONSTRAINT [fk_Boletim_Aluno] FOREIGN KEY([IdAluno])
REFERENCES [dbo].[Aluno] ([Id])
GO
ALTER TABLE [dbo].[Boletim] CHECK CONSTRAINT [fk_Boletim_Aluno]
GO
ALTER TABLE [dbo].[Boletim]  WITH CHECK ADD  CONSTRAINT [fk_Boletim_GradeAula] FOREIGN KEY([IdGradeAula])
REFERENCES [dbo].[GradeAula] ([Id])
GO
ALTER TABLE [dbo].[Boletim] CHECK CONSTRAINT [fk_Boletim_GradeAula]
GO
ALTER TABLE [dbo].[GradeAula]  WITH CHECK ADD  CONSTRAINT [fk_GradeAula_Materia] FOREIGN KEY([IdMateria])
REFERENCES [dbo].[Materia] ([Id])
GO
ALTER TABLE [dbo].[GradeAula] CHECK CONSTRAINT [fk_GradeAula_Materia]
GO
ALTER TABLE [dbo].[GradeAula]  WITH CHECK ADD  CONSTRAINT [fk_GradeAula_Professor] FOREIGN KEY([IdProfessor])
REFERENCES [dbo].[Professor] ([Id])
GO
ALTER TABLE [dbo].[GradeAula] CHECK CONSTRAINT [fk_GradeAula_Professor]
GO
ALTER TABLE [dbo].[GradeAula]  WITH CHECK ADD  CONSTRAINT [fk_GradeAula_Turma] FOREIGN KEY([IdTurma])
REFERENCES [dbo].[Turma] ([Id])
GO
ALTER TABLE [dbo].[GradeAula] CHECK CONSTRAINT [fk_GradeAula_Turma]
GO
ALTER TABLE [dbo].[Professor]  WITH CHECK ADD  CONSTRAINT [fk_Professor_Pessoa] FOREIGN KEY([IdPessoa])
REFERENCES [dbo].[Pessoa] ([Id])
GO
ALTER TABLE [dbo].[Professor] CHECK CONSTRAINT [fk_Professor_Pessoa]
GO
ALTER TABLE [dbo].[Turma]  WITH CHECK ADD  CONSTRAINT [fk_Turma_Escola] FOREIGN KEY([IdEscola])
REFERENCES [dbo].[Escola] ([Id])
GO
ALTER TABLE [dbo].[Turma] CHECK CONSTRAINT [fk_Turma_Escola]
GO
