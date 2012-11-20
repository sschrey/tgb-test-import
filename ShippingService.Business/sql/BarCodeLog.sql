CREATE TABLE dbo.BarcodeScanLog
(
	UserName varchar(50) not null,
	CreatedOn datetime not null,
	PickSlipScan varchar(50) not null,
	BoxScan varchar(50) not null,
	Success bit not null
)