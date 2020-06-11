CREATE TABLE public.Users(
	mail             VARCHAR (50)  ,
	password_hash    VARCHAR (256) ,
  accept_pet       BOOL          ,
  accept_smoke     BOOL          ,
  accept_music     BOOL          ,
  accept_talking   BOOL          ,
  accept_deviation BOOL          ,
  accept_everyone  BOOL          ,
	CONSTRAINT prk_constraint_users PRIMARY KEY (mail)
) WITHOUT OIDS;

CREATE TABLE public.Address(
	id_address       SERIAL				 ,
	number				   INT					 ,
	street					 VARCHAR (100) ,
	complement			 VARCHAR (100) ,
	postal_code			 VARCHAR (10)  ,
	city						 VARCHAR (25)  ,
	state						 VARCHAR (25)  ,
	country				   VARCHAR (25)  ,
	CONSTRAINT prk_constraint_address PRIMARY KEY (id_address)
) WITHOUT OIDS;

CREATE TABLE public.Travel(
	id_travel        SERIAL				 ,
	driver           VARCHAR (50)  ,
	from_					   INT 					 ,
	to_							 INT 					 ,
	time_start			 timestamp with time zone ,
	time_end  			 timestamp with time zone ,
	seats						 INT           ,
	CONSTRAINT prk_constraint_travel PRIMARY KEY (id_travel)
) WITHOUT OIDS;

CREATE TABLE public.Deviation(
	id_travel        INT	,
	addr             INT  ,
	deviation_order  INT 	,
	CONSTRAINT prk_constraint_deviation PRIMARY KEY (id_travel, deviation_order)
) WITHOUT OIDS;

CREATE TABLE public.Booking(
	id_travel        INT					 ,
	author           VARCHAR (50)  ,
	pending					 BOOL					 ,
	seats						 INT				   ,
	CONSTRAINT prk_constraint_booking PRIMARY KEY (id_travel, author)
) WITHOUT OIDS;

ALTER TABLE public.Travel ADD CONSTRAINT FK_Travel_user FOREIGN KEY (driver) REFERENCES public.Users(mail);
ALTER TABLE public.Travel ADD CONSTRAINT FK_Travel_from FOREIGN KEY (from_) REFERENCES public.Address(id_address);
ALTER TABLE public.Travel ADD CONSTRAINT FK_Travel_to FOREIGN KEY (to_) REFERENCES public.Address(id_address);
ALTER TABLE public.Deviation ADD CONSTRAINT FK_Deviation_travel FOREIGN KEY (id_travel) REFERENCES public.Travel(id_travel);
ALTER TABLE public.Booking ADD CONSTRAINT FK_Booking_travel FOREIGN KEY (id_travel) REFERENCES public.Travel(id_travel);
ALTER TABLE public.Booking ADD CONSTRAINT FK_Booking_user FOREIGN KEY (author) REFERENCES public.Users(mail);
