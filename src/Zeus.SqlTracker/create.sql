create table TrackedEvent (
    EventId int not null identity(1,1),
    ProductId int not null,
    EventType tinyint not null,
    EventDate datetime2 not null,
    constraint PK_TrackedEvent_Id primary key (EventId)
)