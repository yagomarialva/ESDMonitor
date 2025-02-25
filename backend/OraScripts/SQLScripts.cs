using System.Net.NetworkInformation;

namespace BiometricFaceApi.OraScripts
{
    public class SQLScripts
    {
        /// <summary>
        /// Comando SQL USERS
        /// </summary>
        public const string VerifyTables = @"SELECT COUNT(1)
                                                FROM INTO FCT_AUTO_TEST.all_tables
                                                WHERE table_name = '{0}'";
        /// <summary>
        /// Comando SQL FCEMBEDDING
        /// </summary>
        public static class FcEmbeddingQueries
        {
            // Pesquisa usuário por ID usando o índice no USERID
            public const string GetEmbeddingByUserId = @"SELECT ID, USERID, EMBEDDING_VALUE 
                                                 FROM FCT_AUTO_TEST.fc_embedding 
                                                 WHERE USERID = :userId";

            // Insere dados na tabela Fc_Embedding
            public const string InsertEmbedding = @"INSERT INTO FCT_AUTO_TEST.fc_embedding (ID, USERID, EMBEDDING_VALUE) 
                                            VALUES (:Id, :UserId, :EmbeddingValue)";

            // Atualiza dados na tabela Fc_Embedding
            public const string UpdateEmbedding = @"UPDATE FCT_AUTO_TEST.fc_embedding
                                            SET USERID = :UserId,
                                                EMBEDDING_VALUE = :EmbeddingValue
                                            WHERE ID = :id";

            // Deleta dados na tabela Fc_Embedding
            public const string DeleteEmbedding = @"DELETE FROM FCT_AUTO_TEST.fc_embedding 
                                            WHERE USERID = :userId";
        }
        /// <summary>
        /// Comando SQL FCAREA
        /// </summary>
        public static class FcAreaQueries
        {
            // Pesquisa usuário por ID usando o índice no USERID
            public const string GetAreaByUserId = @"SELECT ID, USERID, FACE_CONFIDENCE, H, W, X, Y 
                                            FROM FCT_AUTO_TEST.fc_area 
                                            WHERE USERID = :userId";

            // Insere dados na tabela Fc_Area
            public const string InsertArea = @"INSERT INTO FCT_AUTO_TEST.fc_area (ID, USERID, FACE_CONFIDENCE, H, W, X, Y)
                                       VALUES (:Id, :UserId, :FaceConfidence, :H, :W, :X, :Y)";

            // Atualiza dados na tabela Fc_Area
            public const string UpdateArea = @"UPDATE fc_area
                                       SET FCT_AUTO_TEST.FACE_CONFIDENCE = :FaceConfidence,
                                           H = :H, 
                                           W = :W, 
                                           X = :X, 
                                           Y = :Y
                                       WHERE USERID = :UserId AND ID = :id";

            // Deleta dados na tabela Fc_Area
            public const string DeleteArea = @"DELETE FROM FCT_AUTO_TEST.fc_area 
                                       WHERE USERID = :userId";
        }
        /// <summary>
        /// Comando SQL FCEYE
        /// </summary>
        public static class FcEyeQueries
        {
            // Pesquisa usuário por ID usando o índice no USERID
            public const string GetEyeUserId = @"SELECT ID, USERID, LEFT_EYE, LEFT_RIGHT
                                         FROM FCT_AUTO_TEST.fc_eye 
                                         WHERE USERID = :userId";

            // Insere dados na tabela Fc_Eye
            public static string InsertEye = @"INSERT INTO FCT_AUTO_TEST.fc_eye (ID, USERID, LEFT_EYE, LEFT_RIGHT)
                                       VALUES (:Id, :UserId, :LeftEye, :LeftRight)";

            // Atualiza dados na tabela Fc_Eye
            public static string UpdateEye = @"UPDATE FCT_AUTO_TEST.fc_eye
                                       SET LEFT_EYE = :LeftEye,
                                           LEFT_RIGHT = :LeftRight
                                       WHERE USERID = :UserId AND ID = :id";

            // Deleta dados na tabela Fc_Eye
            public static string DeleteEye = @"DELETE FROM FCT_AUTO_TEST.fc_eye 
                                       WHERE USERID = :userId";
        }
        /// <summary>
        /// Comando SQL USERS
        /// </summary> 
        public static class UserQueries 
        {
            //pesquisa e retornar todos os users
            public const string GetAllUsers = @"SELECT ID, NAME, BADGE, CREATED, LASTUPDATED FROM FCT_AUTO_TEST.users";
            //Restorna todos o total de usuários cadastrados.
            public const string GetCountUsers = "SELECT COUNT(*) FROM FCT_AUTO_TEST.Users";
            // Lista de usuários com paginação
            public const string GetPaginatedUsers = @"SELECT * 
                                                    FROM (
                                                    SELECT a.*, ROWNUM rnum 
                                                    FROM (
                                                        SELECT * FROM FCT_AUTO_TEST.users 
                                                        ORDER BY LASTUPDATED DESC
                                                    ) a 
                                                    WHERE ROWNUM <= :Offset + :Limit
                                                    ) 
                                                    WHERE rnum > :Offset";

            // Pesquisa usuário por ID
            public const string GetUserById = @"SELECT * FROM FCT_AUTO_TEST.users WHERE ID = :id";
            // Pesquisa usuário por matrícula (badge)
            public const string GetUserByBadge = @"SELECT * FROM FCT_AUTO_TEST.users WHERE BADGE = :badgeLower";
            // Pesquisa usuário por nome
            public const string GetUserByName = @"SELECT * FROM FCT_AUTO_TEST.users WHERE NAME = :nameLower";
            // Retorna a contagem de usuários
            public const string GetUsersCount = @"SELECT COUNT(*) FROM FCT_AUTO_TEST.users";
            // Insere um novo usuário
            public const string InsertUser = @"INSERT INTO FCT_AUTO_TEST.users (
                                                                 NAME,
                                                                 BADGE,
                                                                 CREATED,
                                                                 LASTUPDATED
                                                             ) VALUES (
                                                                 :name,
                                                                 :badge,
                                                                 :created,
                                                                 :lastUpdated
                                                             )";
            // Atualiza um usuário existente
            public const string UpdateUser = @"UPDATE FCT_AUTO_TEST.users 
                                                        SET 
                                                            NAME = :name, 
                                                            BADGE = :badge,
                                                            LASTUPDATED = :lastUpdated 
                                                        WHERE ID = :id";
            // Deleta um usuário
            public const string DeleteUser = @"DELETE FROM FCT_AUTO_TEST.users WHERE ID = :id";
        }
        /// <summary>
        /// COMANDOS SQL IMAGES
        /// </summary>
        public static class ImageQueries 
        {
            // Retorna todas as imagens
            public const string GetAllImage = @"SELECT * FROM FCT_AUTO_TEST.images";
            // Lista de imagens com paginação
            public const string GetListImage = @"SELECT * 
                                                    FROM (
                                                    SELECT a.*, ROWNUM rnum 
                                                    FROM (
                                                        SELECT * FROM FCT_AUTO_TEST.images 
                                                        ORDER BY LASTUPDATED DESC
                                                    ) a 
                                                    WHERE ROWNUM <= :Offset + :Limit
                                                    ) 
                                                    WHERE rnum > :Offset";
            // Pesquisa imagem por ID
            public const string GetImageById = @"SELECT * FROM FCT_AUTO_TEST.images WHERE ID = :id";
            // Pesquisa imagem por UserID
            public const string GetUserId = @"SELECT * FROM FCT_AUTO_TEST.images WHERE USERID = :userId";
            // Pesquisa imagem por conteúdo (PictureStream)
            public const string GetImageByString = @"SELECT * FROM FCT_AUTO_TEST.images WHERE PICTURESTREAM = :img";
            //Pesquisa imagem por embedding(Caracteristica)
            public const string GetEmbeddingByString = @"SELECT *
                                                                   FROM FCT_AUTO_TEST.images
                                                                   WHERE CONTAINS(EMBEDDING, :embedding, 1) > 0";
            // Insere uma nova imagem
            public const string InsertImage = @"INSERT INTO FCT_AUTO_TEST.images (
                                                                  USERID,
                                                                  PICTURESTREAM,
                                                                  EMBEDDING,
                                                                  CREATED,
                                                                  LASTUPDATED
                                                               ) VALUES (
                                                                  :userId,
                                                                  :pictureStream,
                                                                  :embedding,
                                                                  :created,
                                                                  :lastUpdated
                                                               )";
            // Atualiza uma imagem existente
            public const string UpdateImage = @"UPDATE FCT_AUTO_TEST.images SET
                                                                USERID = :userId, 
                                                                PICTURESTREAM = :pictureStream,
                                                                EMBEDDING = :embedding,
                                                                LASTUPDATED = :lastUpdated
                                                          WHERE ID = :ID";
            // Deleta uma imagem
            public const string DeleteImage = @"DELETE FROM FCT_AUTO_TEST.images WHERE ID = :id";
        }
        /// <summary>
        /// Comandos SQL para a tabela Line
        /// </summary>
        public static class LineQueries 
        {
            // Pesquisa e lista todas as linhas (somente as colunas necessárias)
            public const string GetAllLine = @"SELECT ID, NAME, CREATED, LASTUPDATED FROM FCT_AUTO_TEST.line";
            // Pesquisa linha por ID
            public const string GetLineById = @"SELECT ID, NAME, CREATED, LASTUPDATED 
                                        FROM FCT_AUTO_TEST.line 
                                        WHERE ID = :id";
            // Pesquisa linha por nome (caso-insensível)
            public const string GetLineByName = @"SELECT ID, NAME, CREATED, LASTUPDATED 
                                          FROM FCT_AUTO_TEST.line 
                                          WHERE LOWER(NAME) = :nameLower";
            // Insere uma nova linha
            public const string InsertLine = @"INSERT INTO FCT_AUTO_TEST.line (
                                          NAME,
                                          CREATED, 
                                          LASTUPDATED
                                      ) VALUES (
                                          :name,
                                          :created,
                                          :lastUpdated
                                      )";
            // Atualiza uma linha existente
            public const string UpdateLine = @"UPDATE FCT_AUTO_TEST.line 
                                       SET NAME = :name,
                                           LASTUPDATED = :lastUpdated
                                       WHERE ID = :id";
            // Deleta uma linha por ID
            public const string DeleteLine = @"DELETE FROM FCT_AUTO_TEST.line WHERE ID = :id";
        }
        /// <summary>
        /// Comandos SQL para a tabela Station
        /// </summary>
        public static class StationQueries 
        {
            // Pesquisa e lista todas as estações (somente colunas necessárias)
            public const string GetAllLStation = @"SELECT ID, NAME, SIZEX, SIZEY, CREATED, LASTUPDATED 
                                           FROM FCT_AUTO_TEST.station";
            // Pesquisa estação por ID
            public const string GetStationId = @"SELECT * FROM FCT_AUTO_TEST.station WHERE ID = :id";
            // Pesquisa estação por nome (caso-insensível)
            public const string GetStationName = @"SELECT ID, NAME, SIZEX, SIZEY, CREATED, LASTUPDATED 
                                             FROM FCT_AUTO_TEST.station 
                                             WHERE LOWER(NAME) = :nameLower";
            // Insere uma nova estação
            public const string InsertStation = @"INSERT INTO FCT_AUTO_TEST.station (
                                              NAME, 
                                              SIZEX,
                                              SIZEY,
                                              CREATED,
                                              LASTUPDATED
                                          ) VALUES (
                                              :name, 
                                              :sizex, 
                                              :sizey,
                                              :created, 
                                              :lastUpdated
                                          )";
            // Atualiza uma estação existente
            public const string UpdateStation = @"UPDATE FCT_AUTO_TEST.station 
                                          SET NAME = :name, 
                                              SIZEX = :sizex, 
                                              SIZEY = :sizey, 
                                              LASTUPDATED = :lastUpdated 
                                          WHERE ID = :id";
            // Deleta uma estação por ID
            public const string DeleteStation = @"DELETE FROM FCT_AUTO_TEST.station WHERE ID = :id";
        }
        /// <summary>
        /// Comandos SQL para a tabela Jig
        /// </summary>
        public static class JigQueries
        {
            // Pesquisa e lista todos os jigs (somente colunas necessárias)
            public const string GetAllJig = @"SELECT ID, NAME, SERIALNUMBERJIG, DESCRIPTION, CREATED, LASTUPDATED 
                                       FROM FCT_AUTO_TEST.jig";
            // Pesquisa jig por ID
            public const string GetJigId = @"SELECT * FROM FCT_AUTO_TEST.jig WHERE ID = :id";
            // Pesquisa jig por Serial Number (retorna as colunas necessárias)
            public const string GetjigBySerialNumber = @"SELECT ID, NAME, SERIALNUMBERJIG, DESCRIPTION, CREATED, LASTUPDATED 
                                                 FROM FCT_AUTO_TEST.jig 
                                                 WHERE SERIALNUMBERJIG = :serialNumberJig";
            // Pesquisa jig por nome (caso-insensível)
            public const string GetJigName = @"SELECT ID, NAME, SERIALNUMBERJIG, DESCRIPTION, CREATED, LASTUPDATED 
                                         FROM FCT_AUTO_TEST.jig 
                                         WHERE LOWER(NAME) = :nameLower";
            // Insere um novo jig
            public const string InsertJig = @"INSERT INTO FCT_AUTO_TEST.jig (
                                          NAME, 
                                          SERIALNUMBERJIG,
                                          DESCRIPTION,
                                          CREATED, 
                                          LASTUPDATED
                                      ) VALUES (
                                          :name, 
                                          :serialNumberJig, 
                                          :description, 
                                          :created, 
                                          :lastUpdated
                                      )";
            // Atualiza um jig existente
            public const string UpdateJig = @"UPDATE FCT_AUTO_TEST.jig    
                                      SET NAME = :name,
                                          SERIALNUMBERJIG = :serialNumberJig,
                                          DESCRIPTION = :description,
                                          LASTUPDATED = :lastUpdated 
                                      WHERE ID = :id";
            // Deleta um jig por ID
            public const string DeleteJig = @"DELETE FROM FCT_AUTO_TEST.jig WHERE ID = :id";
        }
        /// <summary>
        /// Comandos SQL para a tabela MonitoresEsd
        /// </summary>
        public static class MonitoresdQueries 
        {
            // Pesquisa e lista todos os monitoresEsd (somente colunas necessárias)
            public const string GetAllMonitor = @"SELECT ID, SERIALNUMBERESP, DESCRIPTION,CREATED, LASTUPDATED 
                                           FROM FCT_AUTO_TEST.monitoresd";
            // Pesquisa monitorEsd por ID
            public const string GetMonitorId = @"SELECT ID, SERIALNUMBERESP, DESCRIPTION, CREATED, LASTUPDATED
                                                                                FROM FCT_AUTO_TEST.monitoresd
                                                                                WHERE ID = :id";
            // Pesquisa monitorEsd por Serial Number
            public const string GetSerialNumber = @"SELECT ID, SERIALNUMBERESP, DESCRIPTION, CREATED, LASTUPDATED 
                                                     FROM FCT_AUTO_TEST.monitoresd 
                                                     WHERE SERIALNUMBERESP = :serial";
            // Pesquisa monitorEsd por IP Number
            public const string GetByIP = @"SELECT ID, SERIALNUMBERESP, DESCRIPTION,  CREATED, LASTUPDATED 
                                           FROM FCT_AUTO_TEST.monitoresd 
                                           WHERE IP = :ip";

            // Pesquisa por logs do monitorEsd
            public const string GetByLogs = @"SELECT ID, SERIALNUMBERESP, DESCRIPTION, CREATED, LASTUPDATED 
                                             FROM FCT_AUTO_TEST.monitoresd 
                                             WHERE LOGS = :logs";
            // Pesquisa monitorEsd por Status
            public const string GetStatus = @"SELECT ID, SERIALNUMBERESP, DESCRIPTION, CREATED, LASTUPDATED 
                                               FROM FCT_AUTO_TEST.monitoresd 
                                               WHERE LOWER(STATUS) = :statusLower";
            // Pesquisa monitorEsd por Status do Operador
            public const string GetStatusOP = @"SELECT ID, SERIALNUMBERESP, DESCRIPTION, CREATED, LASTUPDATED 
                                                       FROM FCT_AUTO_TEST.monitoresd 
                                                       WHERE LOWER(STATUSOPERADOR) = :statusOperadorLower";
            // Pesquisa monitorEsd por Status do Jig
            public const string GetStatusJig = @"SELECT ID, SERIALNUMBERESP, DESCRIPTION, CREATED, LASTUPDATED 
                                                  FROM FCT_AUTO_TEST.monitoresd 
                                                  WHERE LOWER(STATUSJIG) = :statusJigLower";
            // Insere um novo monitorEsd
            public const string InsertMonitorEsd = @"INSERT INTO FCT_AUTO_TEST.monitoresd (
                                               SERIALNUMBERESP,
                                               DESCRIPTION, 
                                               CREATED, 
                                               LASTUPDATED
                                            ) VALUES (
                                               :serialNumberEsp,
                                               :description, 
                                               :created, 
                                               :lastUpdated
                                            )";
            // Atualiza um monitorEsd existente
            public const string UpdateMonitorEsd = @"UPDATE FCT_AUTO_TEST.monitoresd 
                                             SET 
                                                 SERIALNUMBERESP = :serialNumberEsp,
                                                 DESCRIPTION = :description, 
                                                 LASTUPDATED = :lastUpdated
                                             WHERE ID = :id";
            // Deleta um monitorEsd por ID
            public const string DeleteMonitor = @"DELETE FROM FCT_AUTO_TEST.monitoresd WHERE ID = :id";
        }
        /// <summary>
        /// Comandos SQL para a tabela linkStationAndLine
        /// </summary>
        public static class LinkStationAndLineQueries 
        {
            // Pesquisa e lista todos os links entre estações e linhas
            public const string GetAllLinks = @"SELECT ID, ORDERSLIST, LINEID, STATIONID, CREATED, LASTUPDATED 
                                        FROM FCT_AUTO_TEST.linkStationAndLine";
            // Pesquisa link por ID de linha
            public const string GetByLineId = @"SELECT * FROM FCT_AUTO_TEST.linkStationAndLine WHERE LINEID = :lineId";
            // Pesquisa link por ID do link
            public const string GetByLinkId = @"SELECT * FROM FCT_AUTO_TEST.linkStationAndLine WHERE ID = :id";
            // Pesquisa link por ID da estação
            public const string GetByLinkStationId = @"SELECT * FROM FCT_AUTO_TEST.linkStationAndLine WHERE STATIONID = :stationId ";
            // Pesquisa link de linha e estação por IDs de linha e estação
            public const string GetByLinkLineAndStationById = @"SELECT ID, ORDERSLIST, LINEID, STATIONID, CREATED, LASTUPDATED 
                                                        FROM FCT_AUTO_TEST.linkStationAndLine 
                                                        WHERE LINEID = :lineId AND STATIONID = :stationId";
            // Pesquisa por ID de linha e estação e retorna campos selecionados
            public const string GetLineAndStationbyId = @"SELECT ID, ORDERSLIST, LINEID, STATIONID, CREATED, LASTUPDATED 
                                                  FROM FCT_AUTO_TEST.linkStationAndLine 
                                                  WHERE LINEID = :lineId AND STATIONID = :stationId";
            // Insere um novo link entre estação e linha
            public const string InsertLinkAndStation = @"INSERT INTO FCT_AUTO_TEST.linkStationAndLine (
                                                   ORDERSLIST, 
                                                   LINEID, 
                                                   STATIONID, 
                                                   CREATED, 
                                                   LASTUPDATED
                                                ) VALUES (
                                                   :ordersList, 
                                                   :lineId, 
                                                   :stationId, 
                                                   :created, 
                                                   :lastUpdated
                                                )";
            // Atualiza um link existente entre estação e linha
            public const string UpdateLinkAndStation = @"UPDATE FCT_AUTO_TEST.linkStationAndLine 
                                                 SET 
                                                     ORDERSLIST = :ordersList, 
                                                     LINEID = :lineId, 
                                                     STATIONID = :stationId, 
                                                     LASTUPDATED = :lastUpdated 
                                                 WHERE ID = :id";
            // Deleta um link entre estação e linha por ID
            public const string DeleteLinkAndStation = @"DELETE FROM FCT_AUTO_TEST.linkStationAndLine WHERE ID = :id";
        }
        /// <summary>
        /// Comandos SQL para a tabela produceactivity
        /// </summary>
        public static class ProduceActivityQueries 
        {
            // Pesquisa e lista todos os registros de produceActivity
            public const string GetAllProcuceAct = @"SELECT * FROM FCT_AUTO_TEST.produce_activity";
            // Pesquisa produceActivity por ID
            public const string GetProduceActById = @"SELECT * FROM FCT_AUTO_TEST.produce_activity WHERE ID = :id";
            // Pesquisa produceActivity por ID de monitor
            public const string GetMonitorActById = @"SELECT * FROM FCT_AUTO_TEST.produce_activity WHERE MONITORPRODUCE = :monitorEsdId";
            // Pesquisa produceActivity por ID de jig
            public const string GetJigActById = @"SELECT * FROM FCT_AUTO_TEST.produce_activity WHERE JIGPRODUCE = :jigId";
            // Pesquisa produceActivity por ID de usuário
            public const string GetUserActById = @"SELECT * FROM FCT_AUTO_TEST.produce_activity WHERE USERPRODUCE = :userId";
            // Pesquisa produceActivity por ID de linkStationAndLine
            public const string GetLinkStationAndLineById = @"SELECT * FROM FCT_AUTO_TEST.produce_activity WHERE LINKSTATIONANDLINEID = :linkStationAndLineId";
            // Pesquisa produceActivity por valor de isLocked
            public const string GetIsLockedId = @"SELECT * FROM FCT_AUTO_TEST.produce_activity WHERE ISLOCKED = :id";
            // Insere um novo registro em produceActivity
            public const string InsertProduceAct = @"INSERT INTO FCT_AUTO_TEST.produce_activity (
                                                USERID, 
                                                JIGID, 
                                                MONITORESDID, 
                                                LINKSTATIONANDLINEID, 
                                                ISLOCKED, 
                                                DESCRIPTION, 
                                                CREATED, 
                                                LASTUPDATED
                                            ) VALUES (
                                                :userId, 
                                                :jigId, 
                                                :monitorEsdId, 
                                                :linkStationAndLineId, 
                                                :isLocked, 
                                                :description, 
                                                :created, 
                                                :lastUpdated
                                            )";
            // Atualiza um registro existente em produceActivity
            public const string UpdateProduceAct = @"UPDATE FCT_AUTO_TEST.produce_activity 
                                             SET 
                                                 USERID = :userId, 
                                                 JIGID = :jigId, 
                                                 MONITORESDID = :monitorEsdId, 
                                                 LINKSTATIONANDLINEID = :linkStationAndLineId, 
                                                 ISLOCKED = :isLocked, 
                                                 DESCRIPTION = :description, 
                                                 LASTUPDATED = :lastUpdated 
                                             WHERE ID = :id";
            // Deleta um registro de produceActivity por ID
            public const string DeleteProduceAct = @"DELETE FROM FCT_AUTO_TEST.produce_activity WHERE ID = :id";
        }
        /// <summary>
        /// Comandos SQL para a tabela recordStatusProduce
        /// </summary>
        public static class RecordStatusProduceQueries 
        {
            // Pesquisa e lista todos os registros de recordStatusProduce
            public const string GetAllRecordStatus = @"SELECT * FROM FCT_AUTO_TEST.record_status_produce";

            // Pesquisa recordStatusProduce por ID
            public const string GetRecordStatusById = @"SELECT * FROM FCT_AUTO_TEST.record_status_produce WHERE ID = :id";

            // Pesquisa recordStatusProduce por produceActivityId
            public const string GetRecordProduceActId = @"SELECT * FROM FCT_AUTO_TEST.record_status_produce WHERE PRODUCEACTIVITYID = :produceActivityId";

            // Pesquisa recordStatusProduce por userId
            public const string GetRecordProduceUserId = @"SELECT * FROM FCT_AUTO_TEST.record_status_produce WHERE USERID = :userId";

            // Insere um novo registro em recordStatusProduce
            public const string InsertRecordStatusProduce = @"INSERT INTO FCT_AUTO_TEST.record_status_produce (
                                                        PRODUCEACTIVITYID,
                                                        USERID,
                                                        DESCRIPTION,
                                                        STATUS,
                                                        DATEEVENT
                                                    ) VALUES (
                                                        :produceActivityId, 
                                                        :userId, 
                                                        :description, 
                                                        :status, 
                                                        :dateEvent
                                                    )";

            // Atualiza um registro existente em recordStatusProduce
            public const string UpdateRecordStatusProduce = @"UPDATE FCT_AUTO_TEST.record_status_produce 
                                                      SET 
                                                          PRODUCEACTIVITYID = :produceActivityId, 
                                                          USERID = :userId, 
                                                          DESCRIPTION = :description, 
                                                          STATUS = :status, 
                                                          DATEEVENT = :dateEvent 
                                                      WHERE ID = :id";

            // Deleta um registro de recordStatusProduce por ID
            public const string DeleteRecordStatusProduce = @"DELETE FROM FCT_AUTO_TEST.record_status_produce WHERE ID = :id";

        }
        /// <summary>
        /// Comandos SQL para a tabela roles
        /// </summary>
        public static class RolesQueries
        {
            // Pesquisa e lista todos os registros de roles
            public const string GetAllRoles = @"SELECT * FROM FCT_AUTO_TEST.roles";

            // Pesquisa roles por ID
            public const string GetRolesById = @"SELECT * FROM FCT_AUTO_TEST.roles WHERE ID = :id";

            // Pesquisa roles por nome
            public const string GetRolesByRolesName = @"SELECT * FROM FCT_AUTO_TEST.roles WHERE ROLESNAME = :rolesNameLower";

            // Insere um novo registro em roles
            public const string InsertRoles = @"INSERT INTO FCT_AUTO_TEST.roles (
                                                ROLESNAME,
                                                CREATED,
                                                LASTUPDATED
                                            ) VALUES (
                                                :rolesName,
                                                :created,
                                                :lastUpdated
                                            )";

            // Atualiza um registro existente em roles
            public const string UpdateRoles = @"UPDATE FCT_AUTO_TEST.roles
                                        SET 
                                            ROLESNAME = :rolesName,
                                            LASTUPDATED = :lastUpdated
                                        WHERE
                                            ID = :id";

            // Deleta um registro de roles por ID
            public const string DeleteRoles = @"DELETE FROM FCT_AUTO_TEST.roles WHERE ID = :id";
        }
        /// <summary>
        /// Comandos SQL para a tabela authentication
        /// </summary>
        public static class AuthenticationQueries
        {
            // Realiza a pesquisa de usuário e senha para autenticação
            public const string AuthenticateUser = @"SELECT 
                                                 ID, 
                                                 USERNAME, 
                                                 ROLESNAME, 
                                                 BADGE, 
                                                 PASSWORD 
                                             FROM 
                                                 FCT_AUTO_TEST.authentication  
                                             WHERE 
                                                 USERNAME = :username 
                                             AND PASSWORD = :password";

            // Pesquisa authentication por ID
            public const string GetAuthenticationById = @"SELECT * FROM FCT_AUTO_TEST.authentication WHERE ID = :id";

            // Pesquisa authentication por username
            public const string GetAuthenticationByUserName = @"SELECT * FROM FCT_AUTO_TEST.authentication WHERE USERNAME = :username";

            // Pesquisa authentication por badge
            public const string GetAuthenticationByBadge = @"SELECT * FROM FCT_AUTO_TEST.authentication WHERE BADGE = :badge";

            // Insere authentication
            public const string InsertAuthentication = @"INSERT INTO FCT_AUTO_TEST.authentication ( 
                                                         USERNAME, 
                                                         ROLESNAME, 
                                                         BADGE, 
                                                         PASSWORD,
                                                         CREATED,
                                                         LASTUPDATED
                                                     ) VALUES ( 
                                                         :username,
                                                         :rolesname,
                                                         :badge,
                                                         :password,
                                                         :created,
                                                         :lastUpdated
                                                     )";

            // Atualiza authentication
            public const string UpdateAuthentication = @"UPDATE FCT_AUTO_TEST.authentication
                                                 SET 
                                                     USERNAME = :username,
                                                     ROLESNAME = :rolesname,
                                                     BADGE = :badge,
                                                     PASSWORD = :password,
                                                     LASTUPDATED = :lastUpdated
                                                 WHERE
                                                     ID = :id";

            // Deleta authentication
            public const string DeleteAuthentication = @"DELETE FROM FCT_AUTO_TEST.authentication WHERE ID = :id";
        }
        /// <summary>
        /// Comandos SQL para a tabela stationview
        /// </summary>   
        public static class StationViewQueries
        {
            // Pesquisa e lista todos os stationview
            public const string GetAllStationView = @"SELECT * FROM FCT_AUTO_TEST.stationview";

            // Pesquisa stationview por ID
            public const string GetStationViewById = @"SELECT * FROM FCT_AUTO_TEST.stationview WHERE ID = :id";

            // Procura o ID de monitor na tabela stationview
            public const string GetMonitorByIdInST = @"SELECT * FROM FCT_AUTO_TEST.stationview WHERE MONITORESDID = :id";

            // Pesquisa positionsequence por ID
            public const string GetStationViewPositionById = @"SELECT * FROM FCT_AUTO_TEST.stationview WHERE POSITIONSEQUENCE = :positionId";

            // Pesquisa positionsequence por link station and line ID
            public const string GetStationViewPositionByLinkId = @"SELECT POSITIONSEQUENCE FROM FCT_AUTO_TEST.stationview WHERE LINKSTATIONANDLINEID = :linkStationAndLineId AND POSITIONSEQUENCE = :sequanceNumber";

            // Deleta stationview
            public const string DeleteStationView = @"DELETE FROM FCT_AUTO_TEST.StationView WHERE ID = :id";

            // Insere stationview
            public const string InsertStationView = @"INSERT INTO FCT_AUTO_TEST.StationView ( 
                                                 MONITORESDID, 
                                                 LINKSTATIONANDLINEID, 
                                                 POSITIONSEQUENCE, 
                                                 CREATED, 
                                                 LASTUPDATED
                                              ) VALUES ( 
                                                 :monitoresdId, 
                                                 :linkStationAndLineId, 
                                                 :positionSequence, 
                                                 :created, 
                                                 :lastUpdated
                                              )";

            // Atualiza stationview
            public const string UpdateStationView = @"UPDATE FCT_AUTO_TEST.StationView
                                              SET 
                                                  MONITORESDID = :monitoresdId, 
                                                  LINKSTATIONANDLINEID = :linkStationAndLineId, 
                                                  POSITIONSEQUENCE = :positionSequence, 
                                                  LASTUPDATED = :lastUpdate
                                              WHERE 
                                                  ID = :id";

            // Deleta monitor da tabela stationview
            public const string Delete_MonitorByStationView = @"DELETE FROM FCT_AUTO_TEST.stationview WHERE MONITORESDID = :id";
        }
        /// <summary>
        /// Comandos SQL para a tabela logmonitorEsd
        /// </summary>
        public static class LogMonitorEsdQueries
        {
            // Retorna uma lista de logs cadastrados
            public const string GetAllLogMonitor = @"SELECT * FROM FCT_AUTO_TEST.logmonitoresd";

            // Pesquisa logMonitor por ID
            public const string GetLogMonitorById = @"SELECT * FROM FCT_AUTO_TEST.logmonitoresd WHERE ID = :id";

            // Pesquisa monitorEsd na tabela logMonitor por ID
            public const string GetMonitorEsdInLogById = @"SELECT * FROM FCT_AUTO_TEST.logmonitoresd WHERE MONITORESDID = :monitorId";

            // Pesquisa monitorEsd na tabela logMonitor por IP
            public const string GetMonitorEsdInLogByIP = @"SELECT * FROM FCT_AUTO_TEST.logmonitoresd WHERE IP = :ip";

            // Pesquisa SERIALNUMBER de monitorEsd na tabela logMonitor
            public const string GetMonitorEsdInLogBySerialNumber = @"SELECT * FROM FCT_AUTO_TEST.logmonitoresd WHERE SERIALNUMBERESP = :serialNumberEsp";

            // Retorna uma lista paginada de logs do monitor esd
            public const string GetListMonitorByIdWithPagination = @"SELECT *
                                                 FROM (
                                                     SELECT t.*, ROWNUM rnum
                                                     FROM (
                                                         SELECT * 
                                                         FROM FCT_AUTO_TEST.logmonitoresd
                                                         WHERE MONITORESDID = :monitorId
                                                         ORDER BY CREATED DESC
                                                     ) t
                                                     WHERE ROWNUM <= :limit + :offset
                                                 )
                                                 WHERE rnum > :offset";

            // Retorna uma lista de logs em ordem crescente
            public const string GetListLogBySerialNumberIncreWithLimit = @"SELECT ID, 
                                                                   SerialNumberEsp, 
                                                                   MessageType, 
                                                                   MonitorEsdID, 
                                                                   JigId,
                                                                   IP, 
                                                                   Status, 
                                                                   MessageContent, 
                                                                   Description, 
                                                                   TO_CHAR(Created, 'YYYY-MM-DD HH24:MI:SS') AS Created, 
                                                                   TO_CHAR(LastUpdated, 'YYYY-MM-DD HH24:MI:SS') AS LastUpdated
                                                            FROM (
                                                                SELECT t.*, ROWNUM rnum
                                                                FROM (
                                                                    SELECT * 
                                                                    FROM FCT_AUTO_TEST.logmonitoresd
                                                                    WHERE SerialNumberEsp = :serialNumberEsp
                                                                    ORDER BY Created ASC
                                                                ) t
                                                                WHERE ROWNUM <= :limit
                                                            )
                                                            WHERE rnum > 0
                                                            ORDER BY Created ASC";

            // Retorna uma lista dos 250 últimos
            public const string GetListLogDesc = @"SELECT ID, 
                                                            SerialNumberEsp, 
                                                            MessageType, 
                                                            MonitorEsdID, 
                                                            JigId,
                                                            IP, 
                                                            Status, 
                                                            MessageContent, 
                                                            Description, 
                                                            TO_CHAR(Created, 'YYYY-MM-DD HH24:MI:SS') AS Created, 
                                                            TO_CHAR(LastUpdated, 'YYYY-MM-DD HH24:MI:SS') AS LastUpdated
                                                     FROM (
                                                         SELECT t.*, ROWNUM rnum
                                                         FROM (
                                                             SELECT ID, 
                                                                    SerialNumberEsp, 
                                                                    MessageType, 
                                                                    MonitorEsdID, 
                                                                    JigId, 
                                                                    IP, 
                                                                    Status, 
                                                                    MessageContent, 
                                                                    Description, 
                                                                    Created, 
                                                                    LastUpdated
                                                             FROM FCT_AUTO_TEST.logmonitoresd
                                                             ORDER BY Created ASC
                                                         ) t
                                                         WHERE ROWNUM <= 250
                                                     )
                                                     WHERE rnum > 0
                                                     ORDER BY Created DESC;";

            // Retorna uma lista de logs em ordem decrescente
            public const string GetListLogBySerialNumberDescWithLimit = @"SELECT ID, 
                                                                   SerialNumberEsp, 
                                                                   MessageType, 
                                                                   MonitorEsdID, 
                                                                   JigId,
                                                                   IP, 
                                                                   Status, 
                                                                   MessageContent, 
                                                                   Description, 
                                                                   TO_CHAR(Created, 'YYYY-MM-DD HH24:MI:SS') AS Created, 
                                                                   TO_CHAR(LastUpdated, 'YYYY-MM-DD HH24:MI:SS') AS LastUpdated
                                                            FROM (
                                                                SELECT t.*, ROWNUM rnum
                                                                FROM (
                                                                    SELECT * 
                                                                    FROM FCT_AUTO_TEST.logmonitoresd
                                                                    WHERE SerialNumberEsp = :serialNumberEsp
                                                                    ORDER BY Created DESC
                                                                ) t
                                                                WHERE ROWNUM <= :limit
                                                            )
                                                            WHERE rnum > 0
                                                            ORDER BY Created DESC";

            // Retorna logs baseados no conteúdo da mensagem
            public const string GetMessageContent = @"SELECT * FROM FCT_AUTO_TEST.logmonitoresd WHERE MESSAGECONTENT = :messageContentLower";

            // Retorna logs baseados no tipo de mensagem
            public const string GetMessageType = @"SELECT * FROM FCT_AUTO_TEST.logmonitoresd WHERE MESSAGETYPE = :messageTypeLower";

            // Pesquisa logs baseados no tipo de mensagem
            public const string SelectLogMonitor = @"SELECT * FROM FCT_AUTO_TEST.logmonitoresd WHERE ID = :id AND MESSAGETYPE = :messageType";

            // Deleta log de um monitorEsd específico
            public const string DeleteLogMonitorEsd = @"DELETE FROM FCT_AUTO_TEST.logmonitoresd WHERE ID = :id";

            // Deleta monitorEsd da tabela Log
            public const string Delete_MonitorEsd = @"DELETE FROM FCT_AUTO_TEST.logmonitoresd WHERE MONITORESDID = :id";

            // Insere logMonitorEsd
            public const string InsertLogMonitorEsd = @"INSERT INTO FCT_AUTO_TEST.logmonitoresd (
                                                     SerialNumberEsp,
                                                     MessageType, 
                                                     MonitorEsdID,
                                                     JigId,
                                                     Ip,
                                                     Status,
                                                     MessageContent,
                                                     Description,
                                                     Created,
                                                     LastUpdated
                                                 ) VALUES (
                                                     :SerialNumberEsp,
                                                     :MessageType, 
                                                     :MonitorEsdID,
                                                     :JigId,
                                                     :Ip,
                                                     :Status,
                                                     :MessageContent,
                                                     :Description,
                                                     :Created,
                                                     :LastUpdated
                                                 )";

            // Atualiza logMonitorEsd
            public const string UpdateLogMonitorEsd = @"UPDATE FCT_AUTO_TEST.logmonitoresd
                                         SET 
                                             SerialNumberEsp = :serialNumberEsp,
                                             MessageType = :messageType,
                                             MonitorEsdID = :monitorEsdId,
                                             JigId = :jigId,
                                             Ip = :ip,
                                             Status = :status,
                                             MessageContent = :messageContent,
                                             Description = :description,
                                             LastUpdated = :lastUpdated
                                         WHERE ID = :id";

            // Atualiza o status de um logMonitorEsd
            public const string UpdateLogMonitorStatus = @"UPDATE FCT_AUTO_TEST.logmonitoresd
                                             SET 
                                                 STATUS = :status,
                                                 Description = :description,
                                                 MessageType = :messageType,
                                                 JigId = :jigId,
                                                 LastUpdated = :lastUpdated
                                             WHERE ID = :id
                                               AND MESSAGETYPE = :messageType";
        }
        /// <summary>
        /// Comandos SQL para a tabela lastlogmonitorEsd
        /// </summary>
        public static class LastLogMonitorEsdQueries
        {
            // Retorna uma lista de logs cadastrados
            public const string GetAllLastLogMonitor = @"SELECT * FROM  FCT_AUTO_TEST.lastlogmonitorEsd";

            // Pesquisa logMonitor por ID
            public const string GetLastLogMonitorById = @"SELECT * FROM  FCT_AUTO_TEST.lastlogmonitorEsd WHERE ID = :id";

            // Pesquisa monitorEsd na tabela logMonitor por ID
            public const string GetMonitorEsdInLastLogById = @"SELECT *
                                                                     FROM  FCT_AUTO_TEST.lastlogmonitorEsd
                                                                     WHERE MONITORESDID = :monitorEsd
                                                                       AND JIGID = :jigId";

            //Pesquisa jig na tabela lastlogs por id 
            public const string GetJigIdInLastLogById = @"SELECT * 
                                                               FROM  FCT_AUTO_TEST.lastlogmonitorEsd 
                                                               WHERE JIGID = :jigId";

            // Pesquisa SERIALNUMBERESP de monitorEsd na tabela logMonitor
            public const string GetMonitorEsdInLastLogBySerialNumber = @"SELECT * FROM  FCT_AUTO_TEST.lastlogmonitorEsd WHERE SERIALNUMBERESP = :serialNumberEsp";

            //Pesquisa o ultimo log atraves do campo created
            public const string GetLastLogForCreated = @"SELECT *
                                                                FROM  FCT_AUTO_TEST.lastLogmonitoresd
                                                                WHERE ROWNUM = 1
                                                                ORDER BY Created DESC";


            public const string GetMessageType = @"SELECT *
                                                             FROM  FCT_AUTO_TEST.lastlogmonitorEsd
                                                             WHERE SERIALNUMBERESP = :serialNumberEsp
                                                             AND MESSAGETYPE = :messageType";

            // Insere logMonitorEsd
            public const string InsertLastLogs = @"INSERT INTO  FCT_AUTO_TEST.lastlogmonitorEsd (
                                             SerialNumberEsp,
                                             MessageType, 
                                             MonitorEsdID,
                                             JigId,
                                             Ip,
                                             Status,
                                             MessageContent,
                                             Description,
                                             Created,
                                             LastUpdated
                                         ) VALUES (
                                             :SerialNumberEsp,
                                             :MessageType, 
                                             :MonitorEsdID,
                                             :JigId,
                                             :Ip,
                                             :Status,
                                             :MessageContent,
                                             :Description,
                                             :Created,
                                             :LastUpdated
                                         )";

            // Atualiza logMonitorEsd
            public const string UpdateLastLogs = @"UPDATE  FCT_AUTO_TEST.LastLogMonitorEsd
                                 SET 
                                     MessageType = :messageType,
                                     MonitorEsdID = :monitorEsdId,
                                     JigId = :jigId,
                                     Ip = :ip,
                                     Status = :status,
                                     MessageContent = :messageContent,
                                     Description = :description,
                                     LastUpdated = :lastUpdated
                                 WHERE SERIALNUMBERESP = :serialNumberEsp";
        }
        /// <summary>
        /// Comandos SQL para a tabela statusjigandusers
        /// </summary>
        public static class StatusJigAndUserQueries
        {
            // Insere um novo StatusJigAndUser
            public const string InsertStatusJigAndUser = @"INSERT INTO FCT_AUTO_TEST.StatusJigAndUser (
                                                     MONITORESDID, 
                                                     JIGID, 
                                                     USERID, 
                                                     STATUS, 
                                                     LASTUPDATED
                                                 ) VALUES ( 
                                                     :monitoresdid, 
                                                     :jigid, 
                                                     :userid, 
                                                     :status, 
                                                     :lastUpdated
                                                 )";

            // Atualiza um StatusJigAndUser
            public const string UpdateStatusJigAndUser = @"UPDATE FCT_AUTO_TEST.StatusJigAndUser
                                                   SET 
                                                       MONITORESDID = :monitoresdid,
                                                       JIGID = :jigid,
                                                       USERID = :userid,
                                                       STATUS = :status,
                                                       LASTUPDATED = :lastUpdated
                                                   WHERE
                                                       ID = :id";

            public const string GetLastSatusJigAndUser = @"SELECT *
                                                    FROM (
                                                        SELECT *
                                                        FROM FCT_AUTO_TEST.StatusJigAndUser
                                                        WHERE MonitorEsdId = :monitorEsdId
                                                          AND UserId = :userId
                                                          AND JigId = :jigId
                                                        ORDER BY LastUpdated DESC
                                                    )
                                                    WHERE ROWNUM = 1";

            // Deleta um StatusJigAndUser
            public const string DeleteStatusJigAndUser = @"DELETE FROM FCT_AUTO_TEST.StatusJigAndUser WHERE ID = :id";

            // Pesquisa StatusJigAndUser por ID
            public const string GetStatusJigAndUserById = @"SELECT * FROM FCT_AUTO_TEST.StatusJigAndUser WHERE ID = :id";

            // Pesquisa StatusJigAndUser por MONITORESDID
            public const string GetStatusJigAndUserByMonitorEsdId = @"SELECT * FROM FCT_AUTO_TEST.StatusJigAndUser WHERE MONITORESDID = :monitoresdid";

            // Pesquisa StatusJigAndUser por USERID
            public const string GetStatusJigAndUserByUserId = @"SELECT * FROM FCT_AUTO_TEST.StatusJigAndUser WHERE USERID = :userid";

            // Pesquisa StatusJigAndUser por JIGID
            public const string GetStatusJigAndUserByJigId = @"SELECT * FROM FCT_AUTO_TEST.StatusJigAndUser WHERE JIGID = :jigid";
        }
    }
}
