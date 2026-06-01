CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS "Customers" (
    "Id" uuid PRIMARY KEY,
    "FullName" varchar(150) NOT NULL,
    "Email" varchar(150) NOT NULL,
    "Cpf" varchar(14) NOT NULL UNIQUE,
    "BirthDate" date NOT NULL,
    "Address" varchar(250) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS "Orders" (
    "Id" uuid PRIMARY KEY,
    "CustomerId" uuid NOT NULL REFERENCES "Customers"("Id") ON DELETE CASCADE,
    "OrderDate" timestamp with time zone NOT NULL,
    "TotalAmount" numeric(18, 2) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS "OrderItems" (
    "Id" uuid PRIMARY KEY,
    "OrderId" uuid NOT NULL REFERENCES "Orders"("Id") ON DELETE CASCADE,
    "ProductName" varchar(150) NOT NULL,
    "Quantity" integer NOT NULL CHECK ("Quantity" > 0),
    "UnitPrice" numeric(18, 2) NOT NULL CHECK ("UnitPrice" > 0),
    "Subtotal" numeric(18, 2) NOT NULL
);

CREATE INDEX IF NOT EXISTS "IX_Customers_Cpf" ON "Customers" ("Cpf");

CREATE INDEX IF NOT EXISTS "IX_Orders_CustomerId" ON "Orders" ("CustomerId");

CREATE INDEX IF NOT EXISTS "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");