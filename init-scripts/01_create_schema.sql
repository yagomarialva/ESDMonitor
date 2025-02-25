CREATE USER fct_auto_test IDENTIFIED BY fct_auto_test_oracle;
GRANT CONNECT, RESOURCE TO fct_auto_test;
ALTER USER fct_auto_test QUOTA UNLIMITED ON USERS;

-- Concedendo permissão DBA ao usuário
GRANT DBA TO fct_auto_test;
