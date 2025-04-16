﻿Create Table Candidates(
    [Id]                      INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]               NVARCHAR (50)  NOT NULL,
    [LastName]                NVARCHAR (50)  NOT NULL,
    [IsEnrollment]            BIT            NOT NULL,
    [Address]                 NVARCHAR (70)  NULL,
    [BirthCity]               NVARCHAR (70)  NULL,
    [BirthCountry]            NVARCHAR (70)  NULL,
    [BirthState]              NVARCHAR (2)   NULL,
    [City]                    NVARCHAR (70)  NULL,
    [DOB]                     NVARCHAR (MAX) NULL,
    [DriverLicenseNumber]     NVARCHAR (35)  NULL,
    [DriverLicenseState]      NVARCHAR (2)   NULL,
    [Email]                   NVARCHAR (254) NULL,
    [Gender]                  NVARCHAR (35)  NULL,
    [LicenseNumber]           NVARCHAR (35)  NULL,
    [MiddleName]              NVARCHAR (35)  NULL,
    [Photo]                   NVARCHAR (MAX) NULL,
    [PostalCode]              NVARCHAR (35)  NULL,
    [PrimaryPhone]            NVARCHAR (35)  NULL,
    [SSN]                     NVARCHAR (MAX) NULL,
    [SecondaryPhone]          NVARCHAR (35)  NULL,
    [State]                   NVARCHAR (2)   NULL,
    [Age]                     INT            DEFAULT ((0)) NOT NULL,
    [AliasNames]              NVARCHAR (MAX) NULL,
    [BirthCounty]             NVARCHAR (MAX) NULL,
    [BirthPlace]              NVARCHAR (MAX) NULL,
    [BusinessStructure]       NVARCHAR (MAX) NULL,
    [BusinessStructureOther]  NVARCHAR (MAX) NULL,
    [County]                  NVARCHAR (MAX) NULL,
    [DriverLicenseExpiration] DATETIME2 (7)  NULL,
    [EyeColor]                NVARCHAR (MAX) NULL,
    [HairColor]               NVARCHAR (MAX) NULL,
    [Height]                  NVARCHAR (MAX) NULL,
    [HeightFeet]              NVARCHAR (MAX) NULL,
    [HeightInches]            NVARCHAR (MAX) NULL,
    [HomePhone]               NVARCHAR (MAX) NULL,
    [MaritalStatus]           NVARCHAR (MAX) NULL,
    [MobilePhone]             NVARCHAR (MAX) NULL,
    [NameSuffix]              NVARCHAR (MAX) NULL,
    [OfficeAddress]           NVARCHAR (MAX) NULL,
    [OfficeCity]              NVARCHAR (MAX) NULL,
    [OfficeState]             NVARCHAR (MAX) NULL,
    [PhysicalMarks]           NVARCHAR (MAX) NULL,
    [UsCitizen]               NVARCHAR (MAX) NULL,
    [Weight]                  NVARCHAR (MAX) NULL,
    [WorkFax]                 NVARCHAR (MAX) NULL,
    [WorkPhone]               NVARCHAR (MAX) NULL,
    [ConcurrencyToken]        ROWVERSION     NULL,
    CONSTRAINT [PK_Candidates] PRIMARY KEY CLUSTERED ([Id] ASC)
);