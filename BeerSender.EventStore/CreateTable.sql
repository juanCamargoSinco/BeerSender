CREATE TABLE [dbo].[Events](
    [AggregateId] [uniqueidentifier] NOT NULL,
    [SequenceNumber] [int] NOT NULL,
    [Timestamp] [datetime2](7) NOT NULL,
    [EventTypeName] [varchar](256) NOT NULL,
    [EventBody] [nvarchar](max) NOT NULL,
    [RowVersion] [timestamp] NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED --Evita crear el mismo numero de secuencia para el mismo agregado
    (
        [AggregateId] ASC,
        [SequenceNumber] ASC
    )
)