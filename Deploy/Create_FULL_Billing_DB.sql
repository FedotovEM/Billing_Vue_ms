/*_________ Создание доменов ___________*/
create domain CURRENCY as numeric(15,2);
create domain PKFIELD as integer check (VALUE > 0);
create domain TMONTH as smallint check (VALUE between 1 and 12);
create domain TYEAR as smallint	check (VALUE between 1990 and 2100);

/*_________ Создание генераторов ___________*/
create sequence GEN_Unit start with 1 increment by 1;
create sequence GEN_ReceptionPoint start with 1 increment by 1;
create sequence GEN_Mode start with 1 increment by 1;
create sequence GEN_AbonentMode start with 1 increment by 1;
create sequence GEN_Price start with 1 increment by 1;
create sequence GEN_Remains start with 1 increment by 1;
create sequence GEN_NachislSumma start with 1 increment by 1;
create sequence GEN_PaySumma start with 1 increment by 1;
create sequence GEN_Street start with 1 increment by 1;
create sequence GEN_Request start with 1 increment by 1;
create sequence GEN_Disrepair start with 1 increment by 1;
create sequence GEN_Executor start with 1 increment by 1;
create sequence GEN_Services start with 1 increment by 1;
create sequence GEN_User start with 1 increment by 1;


/*_________ Создание таблиц ___________*/
/*_________ Создание таблицы «Пользователь» _________*/
create table "user"(
 UserCD PKFIELD primary key not null default nextval('GEN_User'),
 UserName varchar(80),
 UserPassword varchar(80),
 UserEmail varchar(80),
 UserRole varchar(50)
);

/*_________ Street ___________*/
create table Street (
	StreetCD PKFIELD not null default nextval('GEN_Street'),
	StreetName varchar(50) not null check (StreetName similar to '([А-Яа-я]*[ \-,]?)*'),
	constraint PK_STREET primary key (StreetCD)
	);

/*_________ Disrepair ___________*/
create table Disrepair (
	FailureCD PKFIELD not null default nextval('GEN_Disrepair'),
	FailureName varchar(50) not null check (FailureName similar to '([А-Яа-я]*[ \-,()\.]?)*'),
	constraint PK_DISREPAIR primary key (FailureCD)
	);

/*_________ Executor ___________*/
create table Executor (
	ExecutorCD PKFIELD not null default nextval('GEN_Executor'),
	FIO varchar(50) not null check (FIO similar to '([А-Яа-я]*[ \-\.]?)*'),
	constraint PK_EXECUTOR primary key (ExecutorCD) 
	);

/*_________ Unit ___________*/
create table Unit (
	UnitCD PKFIELD not null default nextval('GEN_Unit'),
	UnitsName varchar(15) not null,
	constraint PK_Unit primary key (UnitCD)
);

/*_________ ReceptionPoint ___________*/
create table ReceptionPoint (
	ReceptionPointCD PKFIELD not null default nextval('GEN_ReceptionPoint'),
	ReceptionName varchar(50) not null check(ReceptionName similar to '([А-Яа-я]*[ \-,()\.]?)*'),
	constraint PK_ReceptionPoint primary key (ReceptionPointCD)
);

/*_________ Abonent ___________*/
create table Abonent (
	AccountCD varchar(6) not null check(AccountCD similar to '[0-9]{6}'),
	FIO varchar(70) check (FIO similar to '([А-Яа-я]*[ \-\.]?)*'),
	StreetCD PKFIELD null,
	HouseNo smallint null check(HouseNo > 0),
	FlatNo smallint null check(FlatNo > 0),
	Phone varchar(17) null,
	corpus integer check(corpus > 0),
	District varchar(20),
	CountPerson integer not null check(CountPerson > 0),
	SizeFlat numeric(15,2) not null check(SizeFlat > 0),
	constraint PK_ABONENT primary key (AccountCD),
	constraint FK_ABONENT_STREET foreign key (StreetCD) references Street(StreetCD) 
		on update cascade on delete set null
	);

/*_________ Services ___________*/
create table Services (
	ServiceCD PKFIELD not null default nextval('GEN_Services'),
	ServiceName varchar(50) not null check (ServiceName similar TO '([А-Яа-я]*[ \-,()\.]?)*'),
	UnitsCD PKFIELD not null,
	constraint PK_SERVICES primary key (ServiceCD),
	constraint FK_Services_Unit foreign key (UnitsCD) references Unit(UnitCD)
	);

/*_________ Mode ___________*/
create table Mode (
	ModeCD PKFIELD not null default nextval('GEN_Mode'),
	ModeName varchar(230) not null check (ModeName similar to '([А-Яа-я]*[0-9]*[ \-,()\.]?)*'),
	Norma numeric(15,4) not null check(NORMA > 0),
	ServiceCD PKFIELD not null,
	constraint PK_Mode primary key (ModeCD),
	constraint FK_Mode_Services foreign key (ServiceCD) references Services(ServiceCD)
		on update cascade
);

/*_________ AbonentMode ___________*/
create table AbonentMode (
	AbonentModeСD PKFIELD not null default nextval('GEN_AbonentMode'),
	AccountCD varchar(6) not null,
	ModeCD PKFIELD not null,
	Counterr bool not null,
	constraint PK_AbonentMode primary key (AbonentModeСD),
	constraint FK_AbonentMode_Abonent foreign key (AccountCD) references Abonent(AccountCD)
		on update cascade,
	constraint FK_AbonentMode_Mode foreign key (ModeCD) references Mode(ModeCD)
		on update cascade
);

/*_________ NachislSumma ___________*/
create table NachislSumma (
	NachislFactCD PKFIELD not null default nextval('GEN_NachislSumma'),
	NachislSum CURRENCY not null,
	NachislYear TYEAR not null,
	NachislMonth TMONTH not null,
	NachType varchar(20),
	AbonentModeСD PKFIELD not null,
	CountResources numeric(15,2) not null check(CountResources >= 0),
	constraint PK_NACHISLSUMMA primary key (NachislFactCD),
	constraint FK_NACHISLSUMMA_ABONENTMODE foreign key (AbonentModeСD) references AbonentMode(AbonentModeСD)
		on update cascade
	);

/*_________ PaySumma ___________*/
create table PaySumma (
	PayFactCD PKFIELD not null default nextval('GEN_PaySumma'),
	PaySum CURRENCY not null,
	PayDate date null,
	PayMonth TMONTH not null,
	PayYear TYEAR not null,
	AbonentModeСD PKFIELD not null,
	PayType varchar(30),
	ReceptionPointCD PKFIELD not null,
	constraint PK_PAYSUMMA primary key (PayFactCD),
	constraint FK_PaySumma_AbonMode foreign key (AbonentModeСD) references AbonentMode(AbonentModeСD)
		ON update cascade,
	constraint FK_PaySumma_ReceptionPoint foreign key (ReceptionPointCD) references ReceptionPoint(ReceptionPointCD)
		on update cascade
	);

/*_________ Request ___________*/
create table Request (
	RequestCD PKFIELD not null default nextval('GEN_Request'),
	AccountCD varchar(6) null,
	FailureCD PKFIELD null,
	ExecutorCD PKFIELD null,
	IncomingDate date not null default now(),
	ExecutionDate date,
	Executed bool not null default false,
	check(ExecutionDate >= IncomingDate),
	constraint PK_REQUEST primary key (RequestCD),
	constraint FK_REQUEST_ABONENT foreign key (AccountCD) references Abonent(AccountCD)
		on update cascade
		on delete set null,
	constraint FK_REQUEST_DISREPAIR foreign key (FailureCD) references Disrepair(FailureCD)
		on update cascade
		on delete set null,
	constraint FK_Request_Executor foreign key (ExecutorCD) references Executor(ExecutorCD)
		on update cascade
		on delete set  null
	);

/*_________ Price ___________*/
create table Price (
	PriceCD PKFIELD not null default nextval('GEN_Price'),
	PriceValue CURRENCY not null,
	ModeCD PKFIELD not null,
	constraint PK_Price primary key (PriceCD),
	constraint FK_Price_Servicess foreign key (ModeCD) references Mode(ModeCD)
		on update cascade
);

/*_________ Remains ___________*/
create table Remains (
	RemainCD PKFIELD not null default nextval('GEN_Remains'),
	AccountCD varchar(6) not null,
	ServiceCD PKFIELD not null,
	Remmonth TMONTH not null,
	Remyear TYEAR not null,
	Remainsum CURRENCY not null default 0,
	constraint PK_Remains primary key (RemainCD),
	constraint FK_Remains_Abonent foreign key (AccountCD) references Abonent(AccountCD)
		on update cascade,
	constraint FK_Remains_Servicess foreign key (ServiceCD) references Services(ServiceCD)
		on update cascade
);

/*_________ Создание функций, процедур и триггеров ___________*/
--НАЧИСЛЕНИЯ ПО НОРМАТИВУ
create or replace function nachi_to_norm(nmonth tmonth, nyear tyear) 
returns int
LANGUAGE plpgsql
as $$
declare
newnachislcd int;
rec record;
begin
 if (nmonth is null)
 	then raise exception 'Ошибка! Не введен месяц для расчёта начислений!'
 	using errcode='null_value_not_allowed';
 end if;
 if (nyear is null)
 	then raise exception 'Ошибка! Не введен год для расчёта начислений!'
 	using errcode='null_value_not_allowed';
 end if;
 for rec in
 (
 select tabl."abonentmodeСd", tabl.accountcd , tabl.servicecd, 
 (CASE WHEN tabl.servicecd in (select s.servicecd 
 								from services s 
 								where servicename not in ('Теплоснабжение', 'Капитальный ремонт')) 
 		THEN tabl.countperson*tabl.norma*tabl.pricevalue
 	  else
 	  	tabl.sizeflat*tabl.norma*tabl.pricevalue
  end) nsum,
  (CASE WHEN tabl.counterr is true 
 			THEN 'фактический'
 	  	else 'нормативный'
  end) nachType,
  (CASE WHEN tabl.servicecd in (select s.servicecd 
 								from services s 
 								where servicename not in ('Теплоснабжение', 'Капитальный ремонт')) 
  			THEN tabl.countperson*tabl.norma
 		else
 	  		tabl.sizeflat*tabl.norma
 		end) nvolume
 from (select am."abonentmodeСd", a.accountcd, a.countperson, a.sizeflat, s.servicecd, counterr, norma, pricevalue
 		from abonentmode am join mode m using(modecd)
 		join price p using(modecd)
 		join services s using(servicecd)
 		join abonent a on am.accountcd = a.accountcd) tabl
 order by tabl.accountcd, tabl.servicecd
 )
 
 loop
 newnachislcd:=nextval('Gen_NachislSumma');
 insert into nachislsumma (NachislFactCD, NachType, AbonentModeСD, NachislSum, NachislMonth, NachislYear, CountResources)
 values
 (newnachislcd, rec.nachType, rec."abonentmodeСd", rec.nsum, nmonth::tmonth, nyear::tyear, rec.nvolume);
 end loop;
 return 1;

 exception
 	when null_value_not_allowed then
		 raise notice 'Исключительная ситуация: %', sqlerrm;
		 return -1;
	when invalid_parameter_value then
		 raise notice 'Исключительная ситуация: %', sqlerrm;
		 return -1;
end;$$;

--НАЧИСЛЕНИЯ ПО НОРМАТИВУ ПО ЛИЦЕВОМУ СЧЕТУ
create or replace function nachi_to_norm_account(newaccountcd varchar(6), nmonth tmonth, nyear tyear) 
returns int
LANGUAGE plpgsql
as $$
declare
rec record;
begin 
 FOR rec IN
 (
  select tabl."abonentmodeСd", tabl.accountcd , tabl.servicecd, 
 (CASE WHEN tabl.servicecd in (select s.servicecd 
 								from services s 
 								where servicename not SIMILAR TO ('Теплоснабжение')) 
 		THEN tabl.countperson*tabl.norma*tabl.pricevalue
 	  else
 	  	tabl.sizeflat*tabl.norma*tabl.pricevalue
  end) nsum,
  (CASE WHEN tabl.counterr is true 
 			THEN 'фактический'
 	  	else 'нормативный'
  end) nachType,
  (CASE WHEN tabl.servicecd in (select s.servicecd 
 								from services s 
 								where servicename not SIMILAR TO ('Теплоснабжение')) 
  			THEN tabl.countperson*tabl.norma
 		else
 	  		tabl.sizeflat*tabl.norma
 		end) nvolume
 from (select am."abonentmodeСd", a.accountcd, a.countperson, a.sizeflat, s.servicecd, counterr, norma, pricevalue
 		from abonentmode am join mode m using(modecd)
 		join price p using(modecd)
 		join services s using(servicecd)
 		join abonent a on am.accountcd = a.accountcd) tabl
 where tabl.accountcd = newaccountcd
 order by tabl.accountcd, tabl.servicecd
 )
 
 loop
	 update nachislsumma set NachType=rec.nachType, AbonentModeСD=rec."abonentmodeСd", NachislSum=rec.nsum, CountResources=rec.nvolume
 			where AbonentModeСD=rec."abonentmodeСd" and nachislmonth=nmonth and nachislyear=nyear;
 END LOOP;
 RETURN 1;
end;$$;

--НАЧИСЛЕНИЯ ПО ФАКТУ
create or replace function nachisl_new_fact(newaccountcd varchar(6), newservicecd pkfield,
												newmonth tmonth, newyear tyear, newvolume numeric(10,3)) 
returns void
LANGUAGE plpgsql
as $$
declare
rec record;
begin 
 if (newaccountcd not in (select a.accountcd from abonent a))
 	then raise exception 'Ошибка! Абонента с таким л/с не существует!'
 	using errcode='integrity_constraint_violation';
 end if;
 if (newaccountcd is null)
 	then raise exception 'Ошибка! Не введен лицевой счет для расчёта начислений!'
 	using errcode='null_value_not_allowed';
 end if;
 if (newservicecd not in (select s.servicecd from services s))
 	then raise exception 'Ошибка! Услуги с таким кодом не существует!'
 	using errcode='integrity_constraint_violation';
 end if;
 if (newservicecd is null)
 	then raise exception 'Ошибка! Не введен код услуги для расчёта начислений!'
 	using errcode='null_value_not_allowed';
 end if;
 if (newmonth is null)
 	then raise exception 'Ошибка! Не введен месяц для расчёта начислений!'
 	using errcode='null_value_not_allowed';
 end if;
 if (newyear is null)
 	then raise exception 'Ошибка! Не введен год для расчёта начислений!'
 	using errcode='null_value_not_allowed';
 end if;
 if (newvolume is null or newvolume < 0)
 	then raise exception 'Ошибка! Введен не корректный объем!'
 	using errcode='null_value_not_allowed';
 end if;

select tabl.abonentmodeСd, tabl.accountcd, tabl.servicecd, (newvolume*tabl.pricevalue) as "new_nach"
 from (select am.abonentmodeСd, am.accountcd, s.servicecd, pricevalue
 		from abonentmode am join mode m using(modecd)
 		join price p using(modecd)
 		join services s using(servicecd)
 		) tabl
 where tabl.accountcd=newaccountcd and tabl.servicecd=newservicecd
 into rec;
 update nachislsumma set NachislSum=rec.new_nach, CountResources=newvolume
 where abonentmodeСd=rec.abonentmodeСd and nachislmonth=newmonth and nachislyear=newyear;
 exception
 	when null_value_not_allowed then
		 raise notice 'Исключительная ситуация: %', sqlerrm;
	when integrity_constraint_violation then
		 raise notice 'Исключительная ситуация: %', sqlerrm;
end;$$;

--РАЗНЕСЕНИЕ ОПЛАТ
create or replace function pay(BillPaidAccountcd varchar(6), PaidServicecd pkfield, paymonth tmonth, payyear tyear, 
								payvolume numeric(15,2), paidsum currency, newreceptionpointcd pkfield) 
returns void
language plpgsql
as $$
declare
rec record;
newpayfactcd int;
begin 
 if (BillPaidAccountcd not in (select a.accountcd from abonent a))
 	then raise exception 'Ошибка! Абонента с таким л/с не существует!'
 	using errcode='integrity_constraint_violation'; end if;
 if (BillPaidAccountcd is null)
 	then raise exception 'Ошибка! Не введен лицевой счет для расчёта начислений!'
 	using errcode='null_value_not_allowed'; end if;
 if (PaidServicecd not in (select s.servicecd from services s))
 	then raise exception 'Ошибка! Услуги с таким кодом не существует!'
 	using errcode='integrity_constraint_violation'; end if;
 if (PaidServicecd is null)
 	then raise exception 'Ошибка! Не введен код услуги для расчёта начислений!'
 	using errcode='null_value_not_allowed'; end if;
 if (paymonth is null)
 	then raise exception 'Ошибка! Не введен месяц, за который производится оплата!'
 	using errcode='null_value_not_allowed'; end if;
 if (payyear is null)
 	then raise exception 'Ошибка! Не введен год, за который производится оплата!'
 	using errcode='null_value_not_allowed'; end if;
  if (newreceptionpointcd not in (select rp.receptionpointcd from receptionpoint rp))
 	then raise exception 'Ошибка! Пункта приёма платежей с таким кодом не существует!'
 	using errcode='integrity_constraint_violation'; end if;
 if (newreceptionpointcd is null)
 	then raise exception 'Ошибка! Не введен код пункта приёма платежей!'
 	using errcode='null_value_not_allowed'; end if;
  if (paidsum is null or paidsum < 0)
 	then raise exception 'Ошибка! Введена не корректная сумма платежа!'
 	using errcode='null_value_not_allowed'; end if;
 if (payvolume is not null) 
	then perform nachisl_new_fact(BillPaidAccountcd, PaidServicecd, paymonth, payyear, payvolume); end if;

select tabl.abonentmodeСd, current_date as "pay_date", 
 	(case when tabl.counterr is true 
 			then 'фактический'
 	  	else 'нормативный' end) payType
 from (select am.abonentmodeСd, am.accountcd, s.servicecd, counterr
 		from abonentmode am join mode m using(modecd)
 		join services s using(servicecd)
 		) tabl
 where tabl.accountcd=BillPaidAccountcd and tabl.servicecd=PaidServicecd
 into rec;

 newpayfactcd:=nextval('gen_paysumma');
 insert into paysumma (payfactcd, paysum, paydate, paymonth, payyear, abonentmodeСd, paytype, receptionpointcd)
 values
 (newpayfactcd, paidsum, rec.pay_date, paymonth, payyear, rec."abonentmodeСd", rec.payType, newreceptionpointcd);
 
exception
 	when null_value_not_allowed then
		 raise notice 'Исключительная ситуация: %', sqlerrm;
	when integrity_constraint_violation then
		 raise notice 'Исключительная ситуация: %', sqlerrm;
end;$$;

--ПРОЦЕДУРА РАСЧЁТА ЗНАЧЕНИЯ САЛЬДО ПО АБОНЕНТУ ПО УСЛУГЕ ЗА ОПРЕДЕЛЁННЫЙ МЕСЯЦ ГОДА 
create or replace procedure ResultSaldo (Var_Account varchar(6), Var_Serv pkfield, Var_Year tyear, Var_Month tmonth)
language plpgsql
as $$
declare
Var_Ostat abonent.currency = 0;
Var_Begin_Ostatok abonent.currency = 0;
Var_Nach abonent.currency = 0;
Var_Pay abonent.currency = 0;
Var_PredResult_Ostatok abonent.currency = 0;
begin
 select COALESCE((SELECT SUM(n.nachislsum) SumNach
 		from abonent.nachislsumma n join abonent.abonentmode am using("abonentmodeСd") 
 			join abonent."mode" m using(modecd)
 		where n.nachislmonth = Var_Month and n.nachislyear = Var_Year
 			and m.servicecd = Var_Serv and am.accountcd = Var_Account), 0),
	COALESCE((SELECT SUM(p.paysum) SumPay
 		from abonent.paysumma p join abonent.abonentmode am using("abonentmodeСd") 
 			join abonent."mode" m using(modecd)
  		where p.paymonth = Var_Month and p.payyear = Var_Year 
 			and m.servicecd = Var_Serv and am.accountcd = Var_Account), 0)
  into Var_Nach, Var_Pay;
 
 select coalesce((select r.remainsum
  into Var_Begin_Ostatok
  from abonent.remains r 
  where r.remmonth = (Var_Month - 1) and r.remyear = Var_Year 
		and r.servicecd = Var_Serv and r.accountcd = Var_Account), 0);

 Var_Ostat := Var_Begin_Ostatok + (Var_Nach - Var_Pay);
 
select r.remainsum 
  from abonent.remains r 
  where r.remmonth = Var_Month and r.remyear = Var_Year 
		and r.servicecd = Var_Serv and r.accountcd = Var_Account
 into Var_PredResult_Ostatok;
 if not found then
 	insert into abonent.Remains (accountcd, servicecd, remmonth, remyear, remainsum) 
 	values 
	(Var_Account, Var_Serv, Var_Month, Var_Year, Var_Ostat);	
 else 
 	update abonent.remains
 	set remainsum = Var_Ostat
 	where accountcd = Var_Account and servicecd = Var_Serv
 		and remyear = Var_Year and remmonth = Var_Month;
 end if;
end $$;

--ФУНКЦИЯ ОБНОВЛЕНИЯ ЗНАЧЕНИЯ ОСТАТКА ПРИ ИЗМЕНЕНИИ В НАЧИСЛЕНИЯХ
create or replace function update_remain_sum_nachisl()
returns trigger
language plpgsql
as $$
declare
ins_rec record;
begin
select am.accountcd, m.servicecd
	from abonent.abonentmode am join abonent."mode" m using(modecd)
	where am."abonentmodeСd" = new."abonentmodeСd" 
	into ins_rec;
 call abonent.ResultSaldo (ins_rec.accountcd, ins_rec.servicecd, new.nachislyear, 
		new.nachislmonth);
return new;
 exception
 when invalid_parameter_value then
 raise notice 'Исключительная ситуация: %', sqlerrm; 
return null;
end; $$;

--ТРИГГЕР ОБНОВЛЕНИЯ ЗНАЧЕНИЯ ОСТАТКА ПРИ ИЗМЕНЕНИИ В НАЧИСЛЕНИЯХ
create or replace trigger update_remainsum_from_nachisl
after update or insert on abonent.nachislsumma
for each row
execute procedure update_remain_sum_nachisl();

--ФУНКЦИЯ ОБНОВЛЕНИЯ ЗНАЧЕНИЯ ОСТАТКА ПРИ ИЗМЕНЕНИИ В ПЛАТЕЖАХ
create or replace function update_remain_sum_pay()
returns trigger
language plpgsql
as $$
declare
ins_rec record;
begin
select am.accountcd, m.servicecd
	from abonent.abonentmode am join abonent."mode" m using(modecd)
	where am."abonentmodeСd" = new."abonentmodeСd" 
	into ins_rec;
 call abonent.ResultSaldo (ins_rec.accountcd, ins_rec.servicecd, new.payyear, 
		new.paymonth);
return new;
 exception
 when invalid_parameter_value then
 raise notice 'Исключительная ситуация: %', sqlerrm; 
return null;
end; $$;

--ТРИГГЕР ОБНОВЛЕНИЯ ЗНАЧЕНИЯ ОСТАТКА ПРИ ИЗМЕНЕНИИ В ПЛАТЕЖАХ
create or replace trigger update_remainsum_from_pay
after update or insert on abonent.paysumma
for each row
execute procedure update_remain_sum_pay();

--ФУНКЦИЯ СОСТАВЛЕНИЯ ОТЧЁТА ПО РЕМОНТНЫМ ЗАЯВКАМ ЗА МЕСЯЦ
create or replace function get_request_report_by_month(searchmonth tmonth, searchyear tyear) 
returns table (
	failurecd pkfield,
	failurename varchar(50),
	execfio varchar(50),
	countReq int,
	countPreviousMonth int
)
language plpgsql
as $$
begin
 return query 
 	select d.failurecd, d.failurename, (CASE WHEN e.fio is not null 
  							THEN e.fio
 							else 'Не назначен' end) execfio, count(r.requestcd)::int as countReq,
 							(select count(r2.requestcd)
 							from request r2 left join executor e2 using(executorcd)
 							where extract(month from r2.incomingdate) = searchmonth - 1 and extract(year from r2.incomingdate) = searchyear 
 							 	and r2.failurecd = d.failurecd and e2.fio = e.fio)::int as countPreviousMonth 							
		from request r join disrepair d using(failurecd)
			left join executor e using(executorcd)
		where extract(month from r.incomingdate) = searchmonth and extract(year from r.incomingdate) = searchyear
		group by d.failurecd, e.fio 
	union
	select d2.failurecd, d2.failurename, null as execfio, count(r2.requestcd)::int as countReq,
 							(select count(r2.requestcd)
 							from request r2
 							where extract(month from r2.incomingdate) = searchmonth - 1 and extract(year from r2.incomingdate) = searchyear 
 							 	and r2.failurecd = d2.failurecd)::int as countPreviousMonth 		
		from request r2 join disrepair d2 using(failurecd)
		where extract(month from r2.incomingdate) = searchmonth and extract(year from r2.incomingdate) = searchyear
		group by d2.failurecd
	order by failurename;
end;$$;

--ФУНКЦИЯ СОСТАВЛЕНИЯ КАРТОЧКИ АБОНЕНТА
create or replace function get_abonent_card(searchAccountcd varchar(6)) 
returns table (
	abonentmodeСd pkfield,
	accountcd varchar(6),
	counterr bool,
	modecd pkfield,
	modename varchar(230),
	norma numeric(15, 4),
	normaname text,
	pricecd pkfield,
	pricevalue numeric(15, 4),
	pricename text,
	servicecd pkfield,
	servicename varchar(50),
	unitcd pkfield,
	unitsname varchar(15)
)
language plpgsql
as $$
begin
 return query 
 	select am."abonentmodeСd", am.accountcd, am.counterr, m.modecd, m.modename, m.norma,
 (CASE WHEN s.servicecd in (select s2.servicecd 
 								from services s2
 								where s.servicename not in ('Теплоснабжение', 'Капитальный ремонт')) 
 		THEN u.unitsname || '/чел'
 	  else
 	  	u.unitsname || '/м2'
  end) normaname,  p.pricecd, p.pricevalue, 'руб/' || u.unitsname as pricename, s.servicecd, s.servicename, u.unitcd, u.unitsname
from abonentmode am
	join "mode" m using(modecd)
	join price p on m.modecd = p.modecd
	join services s using(servicecd)
	join unit u on s.unitscd = u.unitcd 
where am.accountcd = searchAccountcd;
end;$$;


/*_________ ВЕДЕНИЕ ИСТОРИИ ПО АБОНЕНТУ _________*/
create sequence GEN_Abonent_hist start with 1 increment by 1;

/*_________ Abonent_hist _________*/
create table Abonent_hist (
	HistCD PKFIELD not null primary key default nextval('GEN_Abonent_hist'),
    AccountCD varchar(6) not null,
    FIO varchar(70),
    StreetCD PKFIELD,
    HouseNo smallint,
    FlatNo smallint,
    Phone varchar(17),
    Corpus integer,
    District varchar(20),
    CountPerson integer,
    SizeFlat numeric(15,2),
    start_date date not null default now(),
    end_date date,
    deleted boolean not null default false
);

--ФУНКЦИЯ ВЕДЕНИЯ ИСТОРИИ ДАННЫХ ПО АБОНЕНТУ ПРИ ДОБАВЛЕНИИ НОВОГО
create or replace function trg_abonent_insert() 
returns trigger 
language plpgsql
as $$
begin
    insert into abonent_hist (AccountCD, FIO, StreetCD, HouseNo, FlatNo, Phone, 
    			corpus, District, CountPerson, SizeFlat, start_date)
    values (new.AccountCD, new.FIO, new.StreetCD, new.HouseNo, new.FlatNo, new.Phone, 
   			new.corpus, new.District, new.CountPerson, new.SizeFlat, now());
    return new;
end; $$;

--ТРИГГЕР ВЕДЕНИЯ ИСТОРИИ ДАННЫХ ПО АБОНЕНТУ ПРИ ДОБАВЛЕНИИ НОВОГО
create trigger abonent_insert
after insert on Abonent
for each row
execute function trg_abonent_insert();

--ФУНКЦИЯ ВЕДЕНИЯ ИСТОРИИ ДАННЫХ ПО АБОНЕНТУ ПРИ ОБНОВЛЕНИИ СВЕДЕНИЙ О НЁМ
create or replace function trg_abonent_update() 
returns trigger 
language plpgsql
as $$
begin
    update abonent_hist
    set end_date = now()
    where AccountCD = OLD.AccountCD AND end_date is null;
    
    insert into abonent_hist (AccountCD, FIO, StreetCD, HouseNo, FlatNo, Phone, 
    			corpus, District, CountPerson, SizeFlat, start_date)
    values (new.AccountCD, new.FIO, new.StreetCD, new.HouseNo, new.FlatNo, new.Phone,
   			new.corpus, new.District, new.CountPerson, new.SizeFlat, now());
    return new;
end; $$; 

--ТРИГГЕР ВЕДЕНИЯ ИСТОРИИ ДАННЫХ ПО АБОНЕНТУ ПРИ ОБНОВЛЕНИИ СВЕДЕНИЙ О НЁМ
create trigger abonent_update
after update on Abonent
for each row
execute function trg_abonent_update();


--ФУНКЦИЯ ВЕДЕНИЯ ИСТОРИИ ДАННЫХ ПО АБОНЕНТУ ПРИ ЕГО УДАЛЕНИИ
create or replace function trg_abonent_delete() 
returns trigger 
language plpgsql
as $$
begin
    update Abonent_hist
    set end_date = now(), deleted = true
    where AccountCD = old.AccountCD and end_date is null;
    return null;
end; $$;

--ТРИГГЕР ВЕДЕНИЯ ИСТОРИИ ДАННЫХ ПО АБОНЕНТУ ПРИ ЕГО УДАЛЕНИИ
create trigger abonent_delete
before delete on Abonent
for each row
execute function trg_abonent_delete();



/*_________ Вставка данных в таблицы ___________*/

/*_________ Вставка данных в таблицу Street ___________*/
insert into Street (StreetName)
values
('ЦИОЛКОВСКОГО УЛИЦА'),
('НОВАЯ УЛИЦА'),
('ВОЙКОВ ПЕРЕУЛОК'),
('ТАТАРСКАЯ УЛИЦА'),
('ГАГАРИНА УЛИЦА'),
('МОСКОВСКАЯ УЛИЦА'),
('КУТУЗОВА УЛИЦА'),
('МОСКОВСКОЕ ШОССЕ');

/*_________ Вставка данных в таблицу Disrepair ___________*/
insert into Disrepair (FailureCD, FailureName)
values 
(1, 'Засорилась водогрейная колонка'),
(2, 'Не горит АГВ'),
(3, 'Течет из водогрейной колонки'),
(4, 'Неисправна печная горелка'),
(5, 'Неисправен газовый счетчик'),
(6, 'Плохое поступление газа на горелку плиты'),
(7, 'Туго поворачивается пробка крана плиты'),
(8, 'При закрытии краника горелка плиты не гаснет'),
(12, 'Неизвестна');

/*_________ Вставка данных в таблицу Executor ___________*/
insert into Executor (ExecutorCD, FIO)
values 
(1, 'Стародубцев Е. М.'),
(2, 'Булгаков Т. И.'),
(3, 'Шубин В. Г.'),
(4, 'Шлюков М. К.'),
(5, 'Школьников С. М.'),
(6, 'Степанов А. В.');

/*_________ Вставка данных в таблицу Unit ___________*/
insert into Unit (unitsname) values 
('Гкал'),
('кВт*ч'),
('м3'),
('м2');

/*_________ Вставка данных в таблицу ReceptionPoint ___________*/
insert into ReceptionPoint (receptionname) values 
('Касса'),
('Банк'),
('Терминал'),
('ТСЖ');

/*_________ Вставка данных в таблицу Abonent ___________*/
insert into Abonent (AccountCD, FIO, StreetCD, HouseNo, FlatNo, Phone, corpus, District, CountPerson, SizeFlat)
values 
('005488','Аксенов С. А.',3,4,1,'556893',4,'Советский р-н',4,60),
('115705','Мищенко Е. В.',3,1,82,'769975',3,'Советский р-н',3,54),
('015527','Конюхов В. С.',3,1,65,'761699',5,'Советский р-н',5,52.3),
('443690','Тулупова М. И.',7,5,1,'214833',3,'Железнодорожный р-н',3,55),
('136159','Свирина З. А.',7,39,1,NULL,7,'Железнодорожный р-н',4,46),
('443069','Стародубцев Е. В.',4,51,55,'683014',9,'Железнодорожный р-н',2,57.1),
('136160','Шмаков С. В.',4,9,15,NULL,8,'Железнодорожный р-н',4,43.50),
('126112','Маркова В. П.',4,7,11,'683301',2,'Железнодорожный р-н',4,65.50),
('136169','Денисова Е. К.',4,7,13,'680305',3,'Железнодорожный р-н',2,55.1),
('080613','Лукашина Р. М.',8,35,11,'254417',5,'Московский р-н',3,54.7),
('080047','Шубина Т. П.',8,39,36,'257842',3,'Московский р-н',2,55.1),
('080270','Тимошкина Н. Г.',6,35,6,'321002',3,'Железнодорожный р-н',2,55.1);

/*_________ Вставка данных в таблицу Services ___________*/
insert into Services (ServiceCD, ServiceName, UnitsCD)
values
(1, 'Газоснабжение',3),
(2, 'Электроснабжение',2),
(3, 'Теплоснабжение',1),
(4, 'Водоснабжение',3),--Новые
(5,'Горячее водоснабжение',3),
(6, 'Холодное водоснабжение',3),
(7, 'Капитальный ремонт',4),
(8, 'Коммунальные услуги на общедомовые нужды',3),
(9, 'Обращение с ТКО',3),
(10, 'Водоотведение',3);

/*_________ Вставка данных в таблицу Mode ___________*/
insert into Mode (modename, norma,servicecd) values 
('Многоквартирные и жилые дома со стенами из камня, кирпича',0.0388,3),
('Многоквартирные и жилые дома со стенами из панелей, блоков',0.0376,3),


('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные ваннами, унитазами (закрытый водоразбор ГВС)',3.23,5),
('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные ваннами, унитазами (открытый водоразбор ГВС)',2.7,5),
('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные душами, унитазами (закрытый водоразбор ГВС)',2.85,5),

('Многоквартирные и жилые дома со стенами из смешанных и других материалов (в т.ч. щитовые, засыпные)',0.0485,3),

('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные ваннами, унитазами (закрытый водоразбор ГВС)',4.29,6),
('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные ваннами, унитазами (открытый водоразбор ГВС)',4.82,6),
('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные душами, унитазами (открытый водоразбор ГВС)',4.44,6),
('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные душами общего пользования, унитазами (закрытый водоразбор ГВС)',3.97,6),

('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные ваннами, унитазами',7.52,10),
('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные душами, унитазами',6.82,10),

('Коммунальная услуга по холодному водоснабжению на общедомовые нужды (кроме МКД с централизованным ХВС, водонагревателями, водоотведением, этажностью 10-16, более 16)',0.03,8),
('Коммунальная услуга по холодному водоснабжению на общедомовые нужды для МКД с централизованным ХВС, водонагревателями, водоотведением, этажностью 10-16, более 16',0.02,8),

('Население, проживающее в городах, поселках городского типа',1,2),
('Население, проживающее в городских населенных пунктах, в домах, оборудованных в установленном порядке стационарными электроплитами и (или) электроотопительными установками',1,2),
('Население, проживающее в сельских населенных пунктах',1,2),

('Многоквартиные дома',2.28,9),
('Индивидуальные жилые дома',2.31,9),

('Жилые дома',1,7),

('Газовая плита при наличии центрального отопления и горячего водоснабжения',10,1),
('Газовая плита при отсутствии газового водонагревателя и центрального горячего водоснабжения',16.5,1),

('Многоквартирные и жилые дома со стенами из дерева',0.0347,3),
('Газовый водонагреватель',15,1),
('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные душами, унитазами (открытый водоразбор ГВС)',8,5),
('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные душами общего пользования, унитазами (закрытый водоразбор ГВС))',2.85,5),
('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные душами общего пользования, унитазами (открытый водоразбор ГВС))',2.38,5),
('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные душами, унитазами (закрытый водоразбор ГВС)',3.97,6),

('Коммунальная услуга по горячему водоснабжению на общедомовые нужд',0.03,8),

('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные душами общего пользования, унитазам',6.82,10),
('Жилые дома, с централизованными водопроводом, канализацией, горячим водоснабжением, оборудованные унитазами',3.82,10);

/*_________ Вставка данных в таблицу AbonentMode ___________*/
insert into AbonentMode (accountcd, modecd, counterr) values 
('005488',2,false),
('005488',3,true),
('005488',7,true),
('005488',11,false),
('005488',13,false),
('005488',15,true),
('005488',18,false),
('005488',20,false),
('005488',21,true),

('015527',1,false),
('015527',4,true),
('015527',8,false),
('015527',11,false),
('015527',13,false),
('015527',15,true),
('015527',18,false),
('015527',20,false),
('015527',21,false),

('443069',6,false),
('443069',5,true),
('443069',28,true),
('443069',12,false),
('443069',13,false),
('443069',15,true),
('443069',18,false),
('443069',20,false),
('443069',21,false),

('080047',6,false),
('080047',5,true),
('080047',28,true),
('080047',12,false),
('080047',13,false),
('080047',15,true),
('080047',18,false),
('080047',20,false),
('080047',21,false),

('080270',2,false),
('080270',3,true),
('080270',7,true),
('080270',11,false),
('080270',13,false),
('080270',15,true),
('080270',18,false),
('080270',20,false),
('080270',21,true),

('136169',23,false),
('136169',4,true),
('136169',8,false),
('136169',11,false),
('136169',13,false),
('136169',17,true),
('136169',19,false),
('136169',20,false),
('136169',21,false),

('115705',1,false),
('115705',4,true),
('115705',8,false),
('115705',11,false),
('115705',13,false),
('115705',15,true),
('115705',18,false),
('115705',20,false),
('115705',21,false),

('126112',2,false),
('126112',3,true),
('126112',7,true),
('126112',11,false),
('126112',13,false),
('126112',15,true),
('126112',18,false),
('126112',20,false),
('126112',21,true),

('136159',6,false),
('136159',5,true),
('136159',28,true),
('136159',12,false),
('136159',13,false),
('136159',15,true),
('136159',18,false),
('136159',20,false),
('136159',21,false),

('136160',23,false),
('136160',4,true),
('136160',8,false),
('136160',11,false),
('136160',13,false),
('136160',17,true),
('136160',19,false),
('136160',20,false),
('136160',21,false),

('443690',6,false),
('443690',5,true),
('443690',28,true),
('443690',12,false),
('443690',13,false),
('443690',15,true),
('443690',18,false),
('443690',20,false),
('443690',21,false),

('080613',23,false),
('080613',4,true),
('080613',8,false),
('080613',11,false),
('080613',13,false),
('080613',17,true),
('080613',19,false),
('080613',20,false),
('080613',21,false);

/*_________ Вставка данных в таблицу NachislSumma ___________*/
insert into NachislSumma (NachType, AbonentModeСD, NachislSum, NachislMonth, NachislYear, CountResources)
values
('фактический', 6, 58.7, 12, 2019, 11),
('фактический', 6, 46, 12, 2018, 11),
('фактический', 6, 56, 4, 2021, 11),
('фактический', 60, 40, 1, 2018, 11),
('фактический', 60, 250, 9, 2019, 11),
('фактический', 87, 20, 5, 2019, 11),
('фактический', 87, 56, 1, 2021, 11),
('фактический', 51, 20, 5, 2019, 11),
('фактический', 33, 80, 10, 2020, 11),
('фактический', 33, 80, 10, 2019, 11),
('фактический', 42, 46, 12, 2019, 11),
('фактический', 105, 56, 6, 2019, 11),
('фактический', 60, 250, 9, 2018, 11),
('фактический', 60, 58.7, 8, 2019, 11),
('фактический', 51, 58.7, 11, 2019, 11),
('фактический', 24, 80, 9, 2019, 11),
('фактический', 24, 38.5, 8, 2019, 11),
('нормативный', 90, 18.3, 1, 2020, 11),
('нормативный', 18, 28.32, 7, 2020, 11),
('нормативный', 36, 19.56, 3, 2020, 11),
('нормативный', 108, 10.6, 9, 2020, 11),
('нормативный', 27, 38.28, 12, 2020, 11),
('нормативный', 18, 38.32, 4, 2021, 11),
('нормативный', 63, 37.15, 10, 2021, 11),
('нормативный', 108, 12.6, 8, 2018, 11),
('нормативный', 54, 25.32, 1, 2021, 11),
('фактический', 45, 57.1, 2, 2020, 11),
('нормативный', 81, 8.3, 8, 2021, 11),
('фактический', 9, 62.13, 4, 2018, 11),
('нормативный', 63, 37.8, 5, 2019, 11),
('нормативный', 99, 17.8, 6, 2020, 11),
('нормативный', 36, 22.56, 5, 2021, 11),
('фактический', 72, 15.3, 8, 2018, 11),
('нормативный', 36, 32.56, 9, 2019, 11),
('нормативный', 108, 12.6, 4, 2020, 11),
('нормативный', 63, 37.15, 11, 2021, 11),
('фактический', 45, 58.1, 12, 2018, 11),
('нормативный', 54, 28.32, 1, 2019, 11),
('нормативный', 18, 18.32, 2, 2020, 11),
('нормативный', 99, 21.67, 3, 2021, 11),
('нормативный', 108, 22.86, 4, 2018, 11),
('фактический', 45, 60.1, 5, 2019, 11),
('нормативный', 54, 28.32, 2, 2020, 11),
('нормативный', 36, 22.2, 7, 2021, 11),
('фактический', 72, 25.3, 8, 2019, 11),
('нормативный', 27, 38.32, 9, 2019, 11),
('нормативный', 81, 8.3, 10, 2020, 11),
('нормативный', 63, 37.15, 6, 2021, 11),
('нормативный', 90, 18.3, 12, 2018, 11),
('нормативный', 1, 279.8, 5, 2020, 11),
('нормативный', 1, 266.7, 2, 2021, 11),
('нормативный', 10, 343.36, 11, 2021, 11),
('нормативный', 28, 271.6, 2, 2021, 11),
('нормативный', 37, 278.25, 11, 2021, 11),
('нормативный', 100, 254.4, 7, 2019, 11),
('нормативный', 100, 258.8, 2, 2021, 11),
('нормативный', 100, 239.33, 5, 2021, 11),
('нормативный', 64, 179.9, 4, 2020, 11),
('нормативный', 73, 180.13, 9, 2021, 11),
('нормативный', 82, 238.8, 3, 2018, 11),
('нормативный', 82, 237.38, 3, 2019, 11),
('нормативный', 46, 349.19, 6, 2020, 11),
('нормативный', 46, 346.18, 7, 2020, 11),
('нормативный', 91, 290.33, 3, 2021, 11),
('фактический', 11, 580.1, 7, 2020, 11),
('нормативный', 12, 611.3, 10, 2021, 11),
('фактический', 38, 444.34, 3, 2019, 11),
('фактический', 39, 453.43, 6, 2020, 11),
('фактический', 38, 454.6, 4, 2021, 11),
('фактический', 56, 553.85, 1, 2020, 11),
('фактический', 66, 435.5, 6, 2020, 11),
('фактический', 75, 349.38, 4, 2019, 11),
('фактический', 74, 418.88, 6, 2020, 11),
('фактический', 47, 528.44, 10, 2021, 11),
('фактический', 20, 466.69, 5, 2020, 11),
('фактический', 21, 444.45, 10, 2021, 11),
('фактический', 93, 480.88, 8, 2019, 11),
('фактический', 92, 500.13, 9, 2020, 11);

/*_________ Вставка данных в таблицу PaySumma ___________*/
insert into PaySumma (PayType, AbonentModeСD, PaySum, PayDate, PayMonth, PayYear, ReceptionPointCD)
values 
('фактический', 6, 58.7, '2020-01-08', 12, 2019, 1),
('фактический', 6, 40, '2019-01-06', 12, 2018, 1),
('фактический', 6, 56, '2021-05-06', 4, 2021, 1),
('фактический', 60, 40, '2018-02-10', 1, 2018, 1),
('фактический', 60, 250, '2019-10-03', 9, 2019, 1),
('фактический', 87, 20, '2019-06-13', 5, 2019, 1),
('фактический', 87, 56, '2021-02-12', 1, 2021, 2),
('фактический', 51, 20, '2019-06-22', 5, 2021, 2),
('фактический', 33, 80, '2020-11-26', 10, 2020, 2),
('фактический', 33, 80, '2019-11-21', 10, 2019, 2),
('фактический', 42, 30, '2020-01-03', 12, 2019, 2),
('фактический', 105, 58.5, '2019-07-19', 6, 2019, 2),
('фактический', 60, 250, '2018-10-06', 9, 2018, 3),
('фактический', 60, 58.7, '2019-09-04', 8, 2019, 3),
('фактический', 51, 58.7, '2019-12-01', 11, 2019, 3),
('фактический', 24, 80, '2019-10-03', 9, 2019, 3),
('фактический', 24, 38.5, '2019-09-13', 8, 2019, 4),
('нормативный', 90, 18, '2020-02-05', 1, 2020, 2),
('нормативный', 18, 30, '2020-08-03', 7, 2020, 2),
('нормативный', 36, 19.56, '2020-04-02', 3, 2020, 2),
('нормативный', 108, 11, '2020-10-03', 9, 2020, 2),
('нормативный', 27, 38.28, '2021-02-04', 12, 2020, 2),
('нормативный', 18, 40, '2021-05-07', 4, 2021, 2),
('нормативный', 63, 37.15, '2021-11-04', 10, 2021, 4),
('нормативный', 108, 12, '2018-09-20', 8, 2018, 4),
('нормативный', 54, 25.32, '2021-02-03', 1, 2021, 4),
('фактический', 45, 60, '2020-03-05', 2, 2020, 4),
('нормативный', 81, 8.3, '2021-09-10', 8, 2021, 4),
('фактический', 9, 65, '2018-05-03', 4, 2018, 4),
('нормативный', 63, 37.8, '2019-07-12', 5, 2019, 4),
('нормативный', 99, 20, '2020-07-10', 6, 2020, 4),
('нормативный', 36, 22.56, '2021-06-25', 5, 2021, 1),
('фактический', 72, 15.3, '2018-09-08', 8, 2018, 1),
('нормативный', 36, 32.56, '2019-10-18', 9, 2019, 1),
('нормативный', 108, 12.6, '2020-05-22', 4, 2020, 1),
('нормативный', 63, 37.15, '2021-12-23', 11, 2021, 1),
('фактический', 45, 58.1, '2019-01-07', 12, 2018, 1),
('нормативный', 54, 28.32, '2019-02-08', 1, 2019, 1),
('нормативный', 18, 20, '2020-03-18', 2, 2020, 1),
('нормативный', 99, 19.47, '2021-04-10', 3, 2021, 1),
('нормативный', 108, 22.86, '2018-05-04', 4, 2018, 3),
('фактический', 45, 60, '2019-06-07', 5, 2019, 3),
('нормативный', 54, 28.32, '2020-03-05', 2, 2020, 3),
('нормативный', 36, 22.2, '2021-08-10', 7, 2021, 3),
('фактический', 72, 25.3, '2019-09-10', 8, 2019, 3),
('нормативный', 27, 38.32, '2019-10-09', 9, 2019, 3),
('нормативный', 81, 8.3, '2020-11-14', 10, 2020, 3),
('нормативный', 63, 37.15, '2021-08-10', 6, 2021, 3),
('нормативный', 90, 16, '2019-01-07', 12, 2018, 3),
('нормативный', 1, 280, '2020-06-10', 5, 2020, 4),
('нормативный', 1, 260, '2021-03-11', 2, 2021, 4),
('нормативный', 10, 345, '2021-12-15', 11, 2021, 4),
('нормативный', 28, 271.6, '2021-03-12', 2, 2021, 4),
('нормативный', 37, 278, '2021-12-06', 11, 2021, 4),
('нормативный', 100, 254.4, '2019-08-10', 7, 2019, 4),
('нормативный', 100, 258.8, '2021-03-8', 2, 2021, 4),
('нормативный', 100, 239.35, '2021-06-11', 5, 2021, 4),
('нормативный', 64, 179.9, '2020-05-01', 4, 2020, 4),
('нормативный', 73, 180.13, '2021-10-21', 9, 2021, 4),
('нормативный', 82, 240, '2018-04-04', 3, 2018, 1),
('нормативный', 82, 200, '2019-04-06', 3, 2019, 1),
('нормативный', 46, 349.19, '2020-07-14', 6, 2020, 1),
('нормативный', 46, 346.18, '2020-08-13', 7, 2020, 1),
('нормативный', 91, 295, '2021-04-09', 3, 2021, 1),
('фактический', 12, 580.1, '2020-08-08', 7, 2020, 1),
('фактический', 11, 611.3, '2021-11-03', 10, 2021, 1),
('фактический', 38, 444.5, '2019-04-18', 3, 2019, 1),
('фактический', 39, 450, '2020-07-14', 6, 2020, 1),
('фактический', 38, 460, '2021-05-12', 4, 2021, 1),
('нормативный', 57, 553.85, '2020-02-02', 1, 2020, 2),
('фактический', 66, 435.5, '2020-07-12', 6, 2020, 2),
('фактический', 74, 349.38, '2019-05-18', 4, 2019, 2),
('фактический', 75, 420, '2020-07-09', 6, 2020, 2),
('нормативный', 48, 528.44, '2021-11-26', 10, 2021, 2),
('фактический', 20, 466.69, '2020-06-03', 5, 2020, 2),
('фактический', 21, 444.45, '2021-11-16', 10, 2021, 2),
('фактический', 93, 485, '2019-09-05', 8, 2019, 2);

/*_________ Вставка данных в таблицу Request ___________*/
insert into Request (AccountCD, ExecutorCD, FailureCD, IncomingDate, ExecutionDate, Executed)
values
('005488', 1, 1, '2019-12-17', '2019-12-20', true),
('115705', 3, 1, '2019-08-07', '2019-08-12', true),
('015527', 1, 12, '2020-02-28', '2020-03-08', false),
('080270', 4, 1, '2019-12-31', NULL, false),
('080613', 1, 6, '2019-06-16', '2019-06-24', true),
('080047', 3, 2, '2020-10-20', '2020-10-24', true),
('136169', 2, 1, '2019-11-06', '2019-11-08', true),
('136159', 3, 12, '2019-04-01', '2019-04-03', false),
('136160', 1, 6, '2021-01-12', '2021-01-12', true),
('443069', 5, 2, '2019-08-08', '2019-08-10', true),
('005488', 5, 8, '2018-09-04', '2018-12-05', true),
('005488', 4, 6, '2021-04-04', '2021-04-13', true),
('115705', 4, 5, '2018-09-20', '2018-09-23', true),
('115705', NULL, 3, '2019-12-28', NULL, false),
('115705', 1, 5, '2019-08-15', '2019-09-06', true),
('115705', 2, 3, '2020-12-28', '2021-01-04', true),
('080270', 4, 8, '2019-12-17', '2019-12-27', true),
('080047', 3, 2, '2019-10-11', '2019-10-11', true),
('443069', 1, 2, '2019-09-13', '2019-09-14', true),
('136160', 1, 7, '2019-05-18', '2019-05-25', true),
('136169', 5, 7, '2019-05-07', '2019-05-08', true);

/*_________ Вставка данных в таблицу Price ___________*/
insert into Price (pricevalue, modecd) values
(2414.62,1),
(2414.62,2),
(184.35,3),
(184.35,4),
(172.27,5),
(2414.62,6),
(28.21,7),
(28.21,8),
(28.21,9),
(28.21,10),
(32.84,11),
(32.84,12),
(28.21,13),
(28.21,14),
(5.08,15),
(3.56,16),
(3.56,17),
(95.94,18),
(97.20,19),
(11.65,20),
(73.17,21),
(120.7305,22),
(2414.62,23),
(109.755,24),
(178.31,25),
(227.88,26),
(227.88,27),
(178.31,28),
(190.38,29),
(32.84,30),
(32.84,31);

/*_________ Вставка данных в таблицу User ___________*/
insert into "user" (UserName, UserPassword, UserEmail, UserRole) values
('stu', 'asu', 'asu@yandex.ru',	'Admin'),
('operatorIR', '123456', 'operat.ir@yandex.ru',	'User');
