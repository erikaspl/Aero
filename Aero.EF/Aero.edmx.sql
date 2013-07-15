
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 07/15/2013 17:11:09
-- Generated from EDMX file: C:\Dropbox\dev\Aero\Aero.EF\Aero.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Aero];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_VendorPart]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Parts] DROP CONSTRAINT [FK_VendorPart];
GO
IF OBJECT_ID(N'[dbo].[FK_ContactVendor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Vendors] DROP CONSTRAINT [FK_ContactVendor];
GO
IF OBJECT_ID(N'[dbo].[FK_POPart]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[POes] DROP CONSTRAINT [FK_POPart];
GO
IF OBJECT_ID(N'[dbo].[FK_PriorityRFQ]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RFQs] DROP CONSTRAINT [FK_PriorityRFQ];
GO
IF OBJECT_ID(N'[dbo].[FK_ContactCustommer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [FK_ContactCustommer];
GO
IF OBJECT_ID(N'[dbo].[FK_PartRFQ]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RFQs] DROP CONSTRAINT [FK_PartRFQ];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Vendors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vendors];
GO
IF OBJECT_ID(N'[dbo].[Contacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contacts];
GO
IF OBJECT_ID(N'[dbo].[Parts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Parts];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[RFQs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RFQs];
GO
IF OBJECT_ID(N'[dbo].[POes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[POes];
GO
IF OBJECT_ID(N'[dbo].[Priorities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Priorities];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Vendors'
CREATE TABLE [dbo].[Vendors] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ContactId] int  NOT NULL
);
GO

-- Creating table 'Contacts'
CREATE TABLE [dbo].[Contacts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [Address1] nvarchar(max)  NULL,
    [Address2] nvarchar(max)  NULL,
    [Address3] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [County] nvarchar(max)  NULL,
    [PostCode] nvarchar(max)  NULL,
    [Country] nvarchar(max)  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'Parts'
CREATE TABLE [dbo].[Parts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PartNumber] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Qty] smallint  NOT NULL,
    [Condition] int  NOT NULL,
    [Price] decimal(18,2)  NOT NULL,
    [UpdateDate] datetime  NOT NULL,
    [Source] nvarchar(max)  NULL,
    [NSN] nvarchar(max)  NULL,
    [Model] nvarchar(max)  NULL,
    [VendorId] int  NOT NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContactId] int  NULL,
    [Name] nvarchar(max)  NOT NULL,
    [UserName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RFQs'
CREATE TABLE [dbo].[RFQs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [YourRef] nvarchar(max)  NULL,
    [Qty] smallint  NOT NULL,
    [NeedBy] datetime  NULL,
    [Comment] nvarchar(max)  NULL,
    [PriorityId] int  NOT NULL,
    [PartId] int  NOT NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'POes'
CREATE TABLE [dbo].[POes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Number] nvarchar(max)  NOT NULL,
    [UnitPrice] decimal(18,2)  NOT NULL,
    [Qty] smallint  NOT NULL,
    [DeliveryDate] datetime  NOT NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [CustomerId] int  NOT NULL,
    [Part_Id] int  NOT NULL
);
GO

-- Creating table 'Priorities'
CREATE TABLE [dbo].[Priorities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [Display] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Vendors'
ALTER TABLE [dbo].[Vendors]
ADD CONSTRAINT [PK_Vendors]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [PK_Contacts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Parts'
ALTER TABLE [dbo].[Parts]
ADD CONSTRAINT [PK_Parts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RFQs'
ALTER TABLE [dbo].[RFQs]
ADD CONSTRAINT [PK_RFQs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'POes'
ALTER TABLE [dbo].[POes]
ADD CONSTRAINT [PK_POes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Priorities'
ALTER TABLE [dbo].[Priorities]
ADD CONSTRAINT [PK_Priorities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [VendorId] in table 'Parts'
ALTER TABLE [dbo].[Parts]
ADD CONSTRAINT [FK_VendorPart]
    FOREIGN KEY ([VendorId])
    REFERENCES [dbo].[Vendors]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_VendorPart'
CREATE INDEX [IX_FK_VendorPart]
ON [dbo].[Parts]
    ([VendorId]);
GO

-- Creating foreign key on [ContactId] in table 'Vendors'
ALTER TABLE [dbo].[Vendors]
ADD CONSTRAINT [FK_ContactVendor]
    FOREIGN KEY ([ContactId])
    REFERENCES [dbo].[Contacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContactVendor'
CREATE INDEX [IX_FK_ContactVendor]
ON [dbo].[Vendors]
    ([ContactId]);
GO

-- Creating foreign key on [Part_Id] in table 'POes'
ALTER TABLE [dbo].[POes]
ADD CONSTRAINT [FK_POPart]
    FOREIGN KEY ([Part_Id])
    REFERENCES [dbo].[Parts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_POPart'
CREATE INDEX [IX_FK_POPart]
ON [dbo].[POes]
    ([Part_Id]);
GO

-- Creating foreign key on [PriorityId] in table 'RFQs'
ALTER TABLE [dbo].[RFQs]
ADD CONSTRAINT [FK_PriorityRFQ]
    FOREIGN KEY ([PriorityId])
    REFERENCES [dbo].[Priorities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PriorityRFQ'
CREATE INDEX [IX_FK_PriorityRFQ]
ON [dbo].[RFQs]
    ([PriorityId]);
GO

-- Creating foreign key on [ContactId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_ContactCustommer]
    FOREIGN KEY ([ContactId])
    REFERENCES [dbo].[Contacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContactCustommer'
CREATE INDEX [IX_FK_ContactCustommer]
ON [dbo].[Customers]
    ([ContactId]);
GO

-- Creating foreign key on [PartId] in table 'RFQs'
ALTER TABLE [dbo].[RFQs]
ADD CONSTRAINT [FK_PartRFQ]
    FOREIGN KEY ([PartId])
    REFERENCES [dbo].[Parts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PartRFQ'
CREATE INDEX [IX_FK_PartRFQ]
ON [dbo].[RFQs]
    ([PartId]);
GO

-- Creating foreign key on [CustomerId] in table 'RFQs'
ALTER TABLE [dbo].[RFQs]
ADD CONSTRAINT [FK_CustomerRFQ]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerRFQ'
CREATE INDEX [IX_FK_CustomerRFQ]
ON [dbo].[RFQs]
    ([CustomerId]);
GO

-- Creating foreign key on [CustomerId] in table 'POes'
ALTER TABLE [dbo].[POes]
ADD CONSTRAINT [FK_CustomerPO]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerPO'
CREATE INDEX [IX_FK_CustomerPO]
ON [dbo].[POes]
    ([CustomerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------