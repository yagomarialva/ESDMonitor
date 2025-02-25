-- Criando a tabela roles no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE fct_auto_test.roles (
        ID NUMBER PRIMARY KEY,
        RolesName VARCHAR2(50) UNIQUE NOT NULL,
        Created DATE DEFAULT SYSDATE,  -- Definindo data de criação automaticamente
        LastUpdated DATE DEFAULT SYSDATE  -- Definindo data de última atualização automaticamente
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de tabela já existente
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela roles no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE fct_auto_test.roles_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de sequência já existente
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência roles_seq no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER fct_auto_test.roles_trigger
    BEFORE INSERT ON fct_auto_test.roles
    FOR EACH ROW
    BEGIN
        -- Atribui o próximo valor da sequência à coluna ID
        :NEW.ID := fct_auto_test.roles_seq.NEXTVAL;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de trigger já existente
            RAISE;
        END IF;
END;
/

------------------------------------------------------------------------------------
-- Populando a tabela FCT_AUTO_TEST.roles com dados iniciais
BEGIN
    EXECUTE IMMEDIATE 'INSERT INTO FCT_AUTO_TEST.roles (RolesName) VALUES (''administrador'')';
    EXECUTE IMMEDIATE 'INSERT INTO FCT_AUTO_TEST.roles (RolesName) VALUES (''operador'')';
    EXECUTE IMMEDIATE 'INSERT INTO FCT_AUTO_TEST.roles (RolesName) VALUES (''tecnico'')';
END;
/

--------------------------------------------------------------------------------
-- Criação da tabela FCT_AUTO_TEST.authentication
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE fct_auto_test.authentication (
        ID NUMBER PRIMARY KEY,
        Username VARCHAR2(16) NOT NULL,
        RolesName VARCHAR2(50) NOT NULL,
        Badge VARCHAR2(255) UNIQUE NOT NULL,
        Password VARCHAR2(50) NOT NULL,
        Created DATE DEFAULT SYSDATE,  -- Definindo data de criação automaticamente
        LastUpdated DATE DEFAULT SYSDATE  -- Definindo data de última atualização automaticamente
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de tabela já existente
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela authentication no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE fct_auto_test.auth_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de sequência já existente
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência auth_seq no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER fct_auto_test.auth_trigger
    BEFORE INSERT ON fct_auto_test.authentication
    FOR EACH ROW
    BEGIN
        -- Atribui o próximo valor da sequência à coluna ID
        :NEW.ID := fct_auto_test.auth_seq.NEXTVAL;
        :NEW.Created := SYSDATE;  -- Define a data de criação automaticamente
        :NEW.LastUpdated := SYSDATE;  -- Define a data de última atualização automaticamente
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de trigger já existente
            RAISE;
        END IF;
END;
/

------------------------------------------------------------------------------------
-- Populando a tabela FCT_AUTO_TEST.authentication
BEGIN
    EXECUTE IMMEDIATE 'INSERT INTO FCT_AUTO_TEST.authentication (USERNAME, ROLESNAME, BADGE, PASSWORD, CREATED, LASTUPDATED) 
    VALUES (''admin'', ''administrador'', ''admincompal'', ''inNWbDieA4KNSwWeLzW1cQ=='', NULL, NULL)';
    EXECUTE IMMEDIATE 'INSERT INTO FCT_AUTO_TEST.authentication (USERNAME, ROLESNAME, BADGE, PASSWORD, CREATED, LASTUPDATED) 
    VALUES (''esp32'', ''esp32'', ''esp32'', ''vijPQ5f4YZ9DsG8dCzNKLw=='', NULL, NULL)';
    EXECUTE IMMEDIATE 'INSERT INTO FCT_AUTO_TEST.authentication (USERNAME, ROLESNAME, BADGE, PASSWORD, CREATED, LASTUPDATED) 
    VALUES (''ueasupervisor'', ''ueasupervisor'', ''ueasupervisor'', ''ZhwdE79iTgym8N8LY53LVg=='', NULL, NULL)';
END;
/


-- Criação da tabela fct_auto_test.users
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE fct_auto_test.users (
        ID NUMBER PRIMARY KEY,
        Name VARCHAR2(50) NOT NULL,
        Badge VARCHAR2(255) UNIQUE NOT NULL,
        Created DATE DEFAULT SYSDATE NOT NULL,
        LastUpdated DATE DEFAULT SYSDATE
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de tabela já existente
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela users
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE fct_auto_test.users_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de sequência já existente
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência users_seq
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER fct_auto_test.users_trigger
    BEFORE INSERT ON fct_auto_test.users
    FOR EACH ROW
    BEGIN
        :NEW.ID := fct_auto_test.users_seq.NEXTVAL;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de trigger já existente
            RAISE;
        END IF;
END;
/

-- Criação da tabela fct_auto_test.images
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE fct_auto_test.images (
        ID NUMBER PRIMARY KEY,
        UserId NUMBER NOT NULL,
        PictureStream BLOB,
        Embedding CLOB,
        Created DATE DEFAULT SYSDATE NOT NULL,
        LastUpdated DATE DEFAULT SYSDATE,
        CONSTRAINT fk_users FOREIGN KEY (UserId) REFERENCES fct_auto_test.users(ID)
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de tabela já existente
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela images
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE fct_auto_test.images_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de sequência já existente
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência images_seq
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER fct_auto_test.images_trigger
    BEFORE INSERT ON fct_auto_test.images
    FOR EACH ROW
    BEGIN
        :NEW.ID := fct_auto_test.images_seq.NEXTVAL;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de trigger já existente
            RAISE;
        END IF;
END;
/

-- Criação da tabela fct_auto_test.jig
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE fct_auto_test.jig (
        ID NUMBER PRIMARY KEY,
        SerialNumberJig VARCHAR2(50) NOT NULL,
        Name VARCHAR2(50) NOT NULL,
        Description VARCHAR2(250),
        Created DATE DEFAULT SYSDATE,
        LastUpdated DATE DEFAULT SYSDATE
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de tabela já existente
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela jig
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE fct_auto_test.jig_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de sequência já existente
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência jig_seq
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER fct_auto_test.jig_trigger
    BEFORE INSERT ON fct_auto_test.jig
    FOR EACH ROW
    BEGIN
        :NEW.ID := fct_auto_test.jig_seq.NEXTVAL;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN  -- Ignora erro de trigger já existente
            RAISE;
        END IF;
END;
/


--------------------------------------------------------------------------------

-- Criando a tabela station no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE fct_auto_test.station (
        ID NUMBER PRIMARY KEY,
        Name VARCHAR2(50) UNIQUE NOT NULL,
        Sizex NUMBER(10) NOT NULL,
        Sizey NUMBER(10) NOT NULL,
        Created DATE DEFAULT SYSDATE,
        Lastupdated DATE DEFAULT SYSDATE
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela station no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE fct_auto_test.station_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência station_seq no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER fct_auto_test.station_trigger
    BEFORE INSERT ON fct_auto_test.station
    FOR EACH ROW
    BEGIN
        IF :NEW.ID IS NULL THEN
            SELECT fct_auto_test.station_seq.NEXTVAL INTO :NEW.ID FROM dual;
        END IF;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4088 THEN  -- Ignora erro de revalidação falha ou trigger inválido
            RAISE;
        END IF;
END;
/

--------------------------------------------------------------------------------

-- Criação da tabela fct_auto_test.line
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE fct_auto_test.line (
        ID NUMBER PRIMARY KEY,
        Name VARCHAR2(255) UNIQUE NOT NULL,
        Created DATE DEFAULT SYSDATE,
        Lastupdated DATE DEFAULT SYSDATE
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para gerar valores do ID da tabela line no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE fct_auto_test.line_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para usar a sequência na inserção de novos registros na tabela line no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER fct_auto_test.line_trigger
    BEFORE INSERT ON fct_auto_test.line
    FOR EACH ROW
    BEGIN
        IF :NEW.ID IS NULL THEN
            SELECT fct_auto_test.line_seq.NEXTVAL INTO :NEW.ID FROM dual;
        END IF;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4088 THEN  -- Ignora erro de revalidação falha ou trigger inválido
            RAISE;
        END IF;
END;
/

--------------------------------------------------------------------------------

-- Criando a tabela FCT_AUTO_TEST.monitorEsd
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE fct_auto_test.monitorEsd (
        ID NUMBER PRIMARY KEY,
        SerialNumberEsp VARCHAR2(20),
        Description VARCHAR2(250),
        Created DATE DEFAULT SYSDATE NOT NULL,
        LastUpdated DATE DEFAULT SYSDATE NOT NULL
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela monitorEsd dentro do schema FCT_AUTO_TEST
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE fct_auto_test.monitorEsd_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência monitorEsd_seq dentro do schema FCT_AUTO_TEST
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER fct_auto_test.monitorEsd_trigger
    BEFORE INSERT ON fct_auto_test.monitorEsd
    FOR EACH ROW
    BEGIN
        IF :NEW.ID IS NULL THEN
            SELECT fct_auto_test.monitorEsd_seq.NEXTVAL INTO :NEW.ID FROM dual;
        END IF;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

--------------------------------------------------------------------------------

-- Criação da tabela FCT_AUTO_TEST.LogMonitorEsd
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE fct_auto_test.LogMonitorEsd (
        ID NUMBER PRIMARY KEY,
        SerialNumberEsp VARCHAR2(100) NOT NULL,
        MessageType VARCHAR2(250),
        MonitorEsdId NUMBER NOT NULL,
        JigId NUMBER(38),
        Ip VARCHAR2(250) NOT NULL,
        Status NUMBER(1) CHECK (status IN (0, 1)),
        MessageContent VARCHAR2(250),
        Description VARCHAR2(250),
        Created DATE DEFAULT SYSDATE NOT NULL,
        LastUpdated DATE DEFAULT SYSDATE NOT NULL
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para gerar valores do ID:
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE fct_auto_test.logMonitorEsd_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para usar a sequência na inserção de novos registros
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER fct_auto_test.logMonitorEsd_trigger
    BEFORE INSERT ON fct_auto_test.LogMonitorEsd
    FOR EACH ROW
    BEGIN
        IF :NEW.ID IS NULL THEN
            SELECT fct_auto_test.logMonitorEsd_seq.NEXTVAL INTO :NEW.ID FROM dual;
        END IF;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/
-------------------------------------------------------------------------------
-- Criando a tabela LastLogMonitorEsd no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE fct_auto_test.LastLogMonitorEsd (
        ID NUMBER PRIMARY KEY,
        SerialNumberEsp VARCHAR2(100) NOT NULL,
        MessageType VARCHAR2(250),
        MonitorEsdID INTEGER NOT NULL,
        JigId NUMBER(38),
        IP VARCHAR2(250) NOT NULL,
        Status NUMBER(1) CHECK (Status IN (0, 1)),
        MessageContent VARCHAR2(250),
        Description VARCHAR2(250),
        Created DATE DEFAULT SYSDATE NOT NULL,
        LastUpdated DATE DEFAULT SYSDATE NOT NULL,
        CONSTRAINT fk_lastlog_monitoresd FOREIGN KEY (MonitorEsdID) REFERENCES fct_auto_test.monitorEsd(ID)
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela LastLogMonitorEsd no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE fct_auto_test.lasLogMonitorEsd_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência lasLogMonitorEsd_seq no schema fct_auto_test
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER fct_auto_test.lasLogMonitorEsd_trigger
    BEFORE INSERT ON fct_auto_test.LastLogMonitorEsd
    FOR EACH ROW
    WHEN (NEW.ID IS NULL)
    BEGIN
        SELECT fct_auto_test.lasLogMonitorEsd_seq.NEXTVAL INTO :NEW.ID FROM dual;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/
--------------------------------------------------------------------------------

-- Criando a tabela linkStationAndLine dentro do schema FCT_AUTO_TEST
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE fct_auto_test.linkStationAndLine (
        ID NUMBER PRIMARY KEY,
        ORDERSLIST NUMBER NOT NULL,
        StationId NUMBER NOT NULL,
        LineId NUMBER NOT NULL,
        Created DATE DEFAULT SYSDATE NOT NULL,
        LastUpdated DATE DEFAULT SYSDATE NOT NULL,
        CONSTRAINT fk_station FOREIGN KEY (stationid) REFERENCES fct_auto_test.station(id),
        CONSTRAINT fk_line FOREIGN KEY (lineid) REFERENCES fct_auto_test.line(id)
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela linkStationAndLine dentro do schema FCT_AUTO_TEST
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE fct_auto_test.linkStationAndLine_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência linkStationAndLine_seq
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER fct_auto_test.linkStationAndLine_trigger
    BEFORE INSERT ON fct_auto_test.linkStationAndLine
    FOR EACH ROW
    BEGIN
        IF :NEW.ID IS NULL THEN
            SELECT fct_auto_test.linkStationAndLine_seq.NEXTVAL INTO :NEW.ID FROM dual;
        END IF;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/


-- Criando a tabela StationView no schema FCT_AUTO_TEST
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE FCT_AUTO_TEST.stationview (
        ID NUMBER PRIMARY KEY,
        MonitorEsdId NUMBER NOT NULL,
        LinkStationAndLineId NUMBER NOT NULL,
        PositionSequence NUMBER NOT NULL,
        Created DATE DEFAULT SYSDATE NOT NULL,
        LastUpdated DATE DEFAULT SYSDATE NOT NULL,
        CONSTRAINT fk_monitor FOREIGN KEY (MonitorEsdId) REFERENCES FCT_AUTO_TEST.monitorEsd(ID),
        CONSTRAINT fk_linkStAndLine FOREIGN KEY (LinkStationAndLineId) REFERENCES FCT_AUTO_TEST.linkStationAndLine(ID)
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela StationView
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE FCT_AUTO_TEST.stationview_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER FCT_AUTO_TEST.stationview_trigger
    BEFORE INSERT ON FCT_AUTO_TEST.stationview
    FOR EACH ROW
    WHEN (NEW.ID IS NULL)
    BEGIN
        SELECT FCT_AUTO_TEST.stationview_seq.NEXTVAL INTO :NEW.ID FROM dual;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando a tabela ProduceActivity no schema FCT_AUTO_TEST
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE FCT_AUTO_TEST.produce_activity (
        ID NUMBER PRIMARY KEY,
        UserId NUMBER NOT NULL,
        JigId NUMBER NOT NULL,
        MonitorEsdId NUMBER NOT NULL,
        LinkStationAndLineID NUMBER,
        IsLocked NUMBER(1) CHECK (IsLocked IN (0, 1)),
        Description VARCHAR2(250),
        Created DATE DEFAULT SYSDATE NOT NULL,
        LastUpdated DATE DEFAULT SYSDATE NOT NULL,
        CONSTRAINT fk_prod_act_jig FOREIGN KEY (JigId) REFERENCES FCT_AUTO_TEST.jig(ID),
        CONSTRAINT fk_prod_act_monitor FOREIGN KEY (MonitorEsdId) REFERENCES FCT_AUTO_TEST.monitorEsd(ID),
        CONSTRAINT fk_prod_act_user FOREIGN KEY (UserId) REFERENCES FCT_AUTO_TEST.users(ID),
        CONSTRAINT fk_prod_act_link_st_line FOREIGN KEY (LinkStationAndLineID) REFERENCES FCT_AUTO_TEST.linkStationAndLine(ID)
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela ProduceActivity
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE FCT_AUTO_TEST.produce_activity_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER FCT_AUTO_TEST.produce_activity_trigger
    BEFORE INSERT ON FCT_AUTO_TEST.produce_activity
    FOR EACH ROW
    WHEN (NEW.ID IS NULL)
    BEGIN
        SELECT FCT_AUTO_TEST.produce_activity_seq.NEXTVAL INTO :NEW.ID FROM dual;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando a tabela RecordStatusProduce no schema FCT_AUTO_TEST
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE FCT_AUTO_TEST.record_status_produce (
        ID NUMBER PRIMARY KEY,
        ProduceActivityId NUMBER NOT NULL,
        UserId NUMBER NOT NULL,
        Description VARCHAR2(250) NOT NULL,
        Status NUMBER(1) CHECK (Status IN (0, 1)),
        DateEvent DATE DEFAULT SYSDATE,
        CONSTRAINT fk_rsp_prod_act FOREIGN KEY (ProduceActivityId) REFERENCES FCT_AUTO_TEST.produce_activity(ID),
        CONSTRAINT fk_rsp_users FOREIGN KEY (UserId) REFERENCES FCT_AUTO_TEST.users(ID)
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando uma sequência para a tabela RecordStatusProduce
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE FCT_AUTO_TEST.record_status_produce_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criando um gatilho para preencher o ID automaticamente usando a sequência
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER FCT_AUTO_TEST.record_status_produce_trigger
    BEFORE INSERT ON FCT_AUTO_TEST.record_status_produce
    FOR EACH ROW
    WHEN (NEW.ID IS NULL)
    BEGIN
        SELECT FCT_AUTO_TEST.record_status_produce_seq.NEXTVAL INTO :NEW.ID FROM dual;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-----------------------------------------------------------------

-- Criação da tabela FCT_AUTO_TEST.fc_embedding
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE FCT_AUTO_TEST.fc_embedding (
        ID INTEGER PRIMARY KEY,
        UserId INTEGER NOT NULL,
        embedding_Value VARCHAR2(40),
        CONSTRAINT fk_embedding_users FOREIGN KEY (UserId) REFERENCES FCT_AUTO_TEST.users(ID) ON DELETE CASCADE
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação da sequência para gerar valores do ID
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE FCT_AUTO_TEST.fc_embedding_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação do gatilho para usar a sequência na inserção de novos registros
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER FCT_AUTO_TEST.fc_embedding_trigger
    BEFORE INSERT ON FCT_AUTO_TEST.fc_embedding
    FOR EACH ROW
    BEGIN
        :NEW.ID := FCT_AUTO_TEST.fc_embedding_seq.NEXTVAL;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação do índice
BEGIN
    EXECUTE IMMEDIATE 'CREATE INDEX FCT_AUTO_TEST.idx_fc_embedding_userid ON FCT_AUTO_TEST.fc_embedding (UserId)';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-------------------------------------------------------------

-- Criação da tabela FCT_AUTO_TEST.fc_area
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE FCT_AUTO_TEST.fc_area (
        ID INTEGER PRIMARY KEY,
        UserId INTEGER NOT NULL,
        Face_Confidence VARCHAR2(10),
        H VARCHAR2(5),
        W VARCHAR2(5),
        X VARCHAR2(5),
        Y VARCHAR2(5),
        CONSTRAINT fk_area_users FOREIGN KEY (UserId) REFERENCES FCT_AUTO_TEST.users(ID) ON DELETE CASCADE
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação da sequência para gerar valores do ID
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE FCT_AUTO_TEST.fc_area_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação do gatilho para usar a sequência na inserção de novos registros
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER FCT_AUTO_TEST.fc_area_trigger
    BEFORE INSERT ON FCT_AUTO_TEST.fc_area
    FOR EACH ROW
    BEGIN
        :NEW.ID := FCT_AUTO_TEST.fc_area_seq.NEXTVAL;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação do índice
BEGIN
    EXECUTE IMMEDIATE 'CREATE INDEX FCT_AUTO_TEST.idx_fc_area_userid ON FCT_AUTO_TEST.fc_area (UserId)';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-------------------------------------------------------------

-- Criação da tabela FCT_AUTO_TEST.fc_eye
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE FCT_AUTO_TEST.fc_eye (
        ID INTEGER PRIMARY KEY,
        UserId INTEGER NOT NULL,
        Left_Eye VARCHAR2(10),
        Left_Right VARCHAR2(10),
        CONSTRAINT fk_eye_users FOREIGN KEY (UserId) REFERENCES FCT_AUTO_TEST.users(ID) ON DELETE CASCADE
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação da sequência para gerar valores do ID
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE FCT_AUTO_TEST.fc_eye_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação do gatilho para usar a sequência na inserção de novos registros
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER FCT_AUTO_TEST.fc_eye_trigger
    BEFORE INSERT ON FCT_AUTO_TEST.fc_eye
    FOR EACH ROW
    BEGIN
        :NEW.ID := FCT_AUTO_TEST.fc_eye_seq.NEXTVAL;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação do índice
BEGIN
    EXECUTE IMMEDIATE 'CREATE INDEX FCT_AUTO_TEST.idx_fc_eye_userid ON FCT_AUTO_TEST.fc_eye (UserId)';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/


------------------------------------------------------------------------------------

-- Criação da tabela FCT_AUTO_TEST.StatusJigAndUser
BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE FCT_AUTO_TEST.StatusJigAndUser (
        ID NUMBER(38) PRIMARY KEY,
        MONITORESDID NUMBER(38) NOT NULL,
        STATUS NUMBER(1) CHECK (STATUS IN (0, 1)),
        LASTUPDATED DATE NOT NULL
    )';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação da sequência para gerar valores do ID
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE FCT_AUTO_TEST.statusJigAndUser_seq START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação do gatilho para usar a sequência na inserção de novos registros
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER FCT_AUTO_TEST.statusJigAndUser_trigger
    BEFORE INSERT ON FCT_AUTO_TEST.StatusJigAndUser
    FOR EACH ROW
    WHEN (NEW.ID IS NULL)
    BEGIN
        SELECT FCT_AUTO_TEST.statusJigAndUser_seq.NEXTVAL INTO :NEW.ID FROM dual;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Criação do gatilho adicional para manipulação de atualizações
BEGIN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER FCT_AUTO_TEST.trg_update_status_jig_user
    AFTER INSERT ON FCT_AUTO_TEST.logmonitoresd
    FOR EACH ROW
    BEGIN
        IF :NEW.messagetype = ''operador'' THEN
            MERGE INTO FCT_AUTO_TEST.StatusJigAndUser sju
            USING dual ON (sju.MONITORESDID = :NEW.MONITORESDID)
            WHEN MATCHED THEN
                UPDATE SET sju.STATUS = :NEW.STATUS, sju.LASTUPDATED = SYSDATE
            WHEN NOT MATCHED THEN
                INSERT (ID, MONITORESDID, STATUS, LASTUPDATED)
                VALUES (FCT_AUTO_TEST.statusJigAndUser_seq.NEXTVAL, :NEW.MONITORESDID, :NEW.STATUS, SYSDATE);
        END IF;

        IF :NEW.messagetype = ''jig'' THEN
            MERGE INTO FCT_AUTO_TEST.StatusJigAndUser sju
            USING dual ON (sju.MONITORESDID = :NEW.MONITORESDID)
            WHEN MATCHED THEN
                UPDATE SET sju.STATUS = :NEW.STATUS, sju.LASTUPDATED = SYSDATE
            WHEN NOT MATCHED THEN
                INSERT (ID, MONITORESDID, STATUS, LASTUPDATED)
                VALUES (FCT_AUTO_TEST.statusJigAndUser_seq.NEXTVAL, :NEW.MONITORESDID, :NEW.STATUS, SYSDATE);
        END IF;
    END;';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/




