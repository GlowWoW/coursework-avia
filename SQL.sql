create database Air

create table Ship--Самолёт
(C_ship  int primary key not null identity
, Name  varchar(50) not null
, Capacity  int not null --Вместимость
)

create table Line--Авиакомпания
(C_Line  int primary key not null identity
, Name varchar(50) not null 
)

create table Client--Клиент
(C_Client  int primary key not null identity
, Passport varchar(11) not null UNIQUE
, Fam  varchar(50) not null
, Im  varchar(50) not null
, Otch  varchar(50) not null
, Dr date not null
)

create table City--Город
(C_City int primary key not null identity
, Name varchar(50) not null 
)

create table Period--Периодичность
(C_Period  int
Wday int
, CONSTRAINT PK_Period PRIMARY KEY (C_Period, Wday)
)

create table Flight--Расписание
(C_Flight  int primary key not null identity
, C_Period int 
, DepTime time not null
, DepPoint int FOREIGN KEY REFERENCES City(C_City)
, DestPoint int FOREIGN KEY REFERENCES City (C_City)
, C_Ship int foreign key references Ship --Самолёт
, C_Line int foreign key references Line --Авиалиния
)

create table Tickets--Счётчик билетов
(C_Flight  int foreign key references Flight
, DepDate date not null
, Tickets_cnt int --Проданных
, Tickets_cnt0 int --Оставшихся
, CONSTRAINT PK_Tickets PRIMARY KEY (C_Flight, DepDate)
)


create table Сonsignment--Заказ
(C_Cons  int primary key not null identity
, C_Flight int not null
, DepDate date not null
, C_Client int foreign key references Client,
CONSTRAINT FK_Сonsignment1 FOREIGN KEY (C_Flight, DepDate) REFERENCES Tickets (C_Flight, DepDate)
)