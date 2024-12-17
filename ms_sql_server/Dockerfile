FROM rapidfort/microsoft-sql-server-2019-ib:latest 

USER root

RUN chmod -R 777 var
RUN chmod -R 777 var/opt
RUN chmod -R 777 var/opt/mssql
RUN mkdir -p var/opt/mssql/.system
RUN chmod -R 777 var/opt/mssql/.system
      
CMD ["/opt/mssql/bin/sqlservr"]
