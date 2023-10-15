create database CRMDB
go


use CRMDB
go

-- Se crea la tabla Customers (o Clients)
create table Customers
(
	Id int identity(1,1) primary key,
	Name varchar(50) not null,
	LastName varchar(50) not null,
	Address varchar(255)
)
go