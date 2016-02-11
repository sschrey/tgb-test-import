CREATE TABLE CarrierModeFilter
(
	ID varchar(50) primary key not null,
	Carrier varchar(50) not null,
	CarrierMode varchar(50) not null,
	IsDefault bit not null
)

insert into carriermodefilter(id, carrier, carriermode, IsDefault)
values('87433-728', '87433', '728', 1)

insert into carriermodefilter(id, carrier, carriermode, IsDefault)
values('87433-48N', '87433', '48N', 0)

insert into carriermodefilter(id, carrier, carriermode, IsDefault)
values('87433-30', '87433', '30', 0)

insert into carriermodefilter(id, carrier, carriermode, IsDefault)
values('87433-15', '87433', '15', 0)

insert into carriermodefilter(id, carrier, carriermode, IsDefault)
values('87433-15D', '87433', '15D', 0)

insert into carriermodefilter(id, carrier, carriermode, IsDefault)
values('87433-15N', '87433', '15N', 0)