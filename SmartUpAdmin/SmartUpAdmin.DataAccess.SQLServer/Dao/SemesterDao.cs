using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using System.Diagnostics;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class SemesterDao
    {
        private static SemesterDao? instance;
        private SemesterDao()
        {
        }

        public static SemesterDao GetInstance()
        {
            if (instance == null)
            {
                instance = new SemesterDao();
            }
            return instance;
        }

        public void FillTable()
        {
            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();
                Dictionary<string, (string Abbreviation, string Description)> itSemesters = new Dictionary<string, (string, string)>
                {
                    { "Introduction to Programming", ("ITP", "In de inleidende cursus Programmeren (ITP) zullen studenten uitgebreid kennismaken met de fundamentele concepten van programmeren en basisalgoritmen. Deze cursus biedt niet alleen een solide basis voor het begrijpen van programmeerprincipes en het schrijven van efficiënte code, maar moedigt studenten ook aan om creatief te denken en probleemoplossende vaardigheden te ontwikkelen. Door middel van praktische oefeningen en projecten zullen studenten hands-on ervaring opdoen, wat hen in staat stelt om abstracte concepten toe te passen in real-world scenario's. Daarnaast zal de cursus de nadruk leggen op het belang van samenwerking en communicatie binnen een ontwikkelteam, waardoor studenten niet alleen individuele vaardigheden verwerven, maar ook leren hoe ze effectief kunnen bijdragen aan grotere projecten en initiatieven.") },
                    { "Data Structures and Algorithms", ("DSA", "In de uitgebreide cursus Datastructuren en Algoritmen (DSA) zullen studenten diepgaand onderzoek doen naar geavanceerde gegevensstructuren en de analyse van algoritmen. Deze cursus biedt niet alleen een theoretisch begrip van complexe structuren, maar benadrukt ook het belang van efficiëntie en optimalisatie bij het ontwerpen van algoritmen. Studenten zullen uitgedaagd worden met complexe programmeeropdrachten en projecten, waardoor ze hun vaardigheden verder kunnen ontwikkelen en verfijnen. Door te focussen op probleemoplossing en kritisch denken, zullen studenten in staat zijn om complexe problemen op te lossen en robuuste, schaalbare oplossingen te implementeren. Bovendien zal de cursus aandacht besteden aan de praktische toepassing van deze concepten in real-world situaties, waardoor studenten goed voorbereid worden op uitdagende taken in het werkveld.") },
                    { "Object-Oriented Programming", ("OOP", "Principes van Objectgeoriënteerd Ontwerp en Programmering (OOP) worden uitgebreid behandeld in deze cursus. Studenten zullen niet alleen de basisprincipes van objectgeoriënteerd programmeren begrijpen, maar ook diep ingaan op de toepassing ervan in het ontwerp van complexe softwarearchitecturen. De cursus zal hands-on programmeeroefeningen en projecten bevatten, waarbij studenten worden uitgedaagd om flexibele en herbruikbare code te schrijven. Door middel van praktische voorbeelden zullen studenten leren hoe ze effectief objecten kunnen modelleren en relaties tussen hen kunnen vaststellen. Bovendien zal de cursus de nadruk leggen op softwareontwerp principes, zoals encapsulatie, overerving en polymorfisme, waardoor studenten in staat worden gesteld om goed gestructureerde en onderhoudsvriendelijke code te produceren.") },
                    { "Database Management Systems", ("DBMS", "In deze uitgebreide cursus over Relationele Databaseconcepten en SQL (DBMS) zullen studenten een diepgaand begrip ontwikkelen van databasesystemen en gerelateerde concepten. Naast het leren van relationele databaseontwerp principes, zullen studenten praktische ervaring opdoen met het schrijven van complexe SQL-query's en het beheren van grote datasets. De cursus zal ook de integratie van databases in moderne applicatieontwikkeling benadrukken, waardoor studenten leren hoe ze efficiënte en schaalbare opslagoplossingen kunnen implementeren. Door middel van real-world case studies en projecten zullen studenten worden blootgesteld aan verschillende databasebeheerscenario's, waardoor ze klaar zijn voor uitdagende rollen op het gebied van gegevensbeheer en -analyse.") },
                    { "Web Development and Design", ("WDD", "De cursus Webontwikkeling en -ontwerp (WDD) plaatst studenten in het hart van de front-end en back-end webontwikkeling. Naast het verwerven van technische vaardigheden voor het bouwen van moderne, responsieve websites, worden studenten aangemoedigd om aandacht te besteden aan gebruikerservaring en visueel ontwerp. Door middel van praktische projecten zullen studenten niet alleen HTML, CSS en JavaScript beheersen, maar ook kennis maken met frameworks en tools die veel gebruikt worden in de webontwikkelingsindustrie. Daarnaast zal de cursus zich richten op de principes van schaalbare en onderhoudsvriendelijke code in het kader van webontwikkeling, waardoor studenten goed voorbereid worden op het veeleisende landschap van moderne webapplicaties.") },
                    { "Software Engineering Principles", ("SEP", "De cursus Principes van Software Engineering (SEP) biedt een diepgaand begrip van de levenscyclus van softwareontwikkeling en best practices in de branche. Studenten zullen niet alleen bekend raken met traditionele en agile ontwikkelmethodologieën, maar ook leren hoe ze effectieve softwareontwerpen kunnen maken, inclusief het opstellen van gedetailleerde specificaties en documentatie. Door middel van collaboratieve projecten zullen studenten ervaren hoe ze software kunnen ontwikkelen in teamverband, waarbij de nadruk ligt op communicatie, versiebeheer en probleemoplossend vermogen. Bovendien zal de cursus aandacht besteden aan softwaretesten en kwaliteitsborging, waardoor studenten leren hoe ze betrouwbare en foutloze softwareproducten kunnen leveren.") },
                    { "Networking and Security", ("NS", "Computernetwerken en informatiebeveiliging vormen de kern van de cursus Netwerken en Beveiliging (NS). Studenten zullen niet alleen een diepgaand begrip ontwikkelen van computernetwerkarchitecturen en protocollen, maar ook leren hoe ze effectieve beveiligingsmaatregelen kunnen implementeren. De cursus omvat hands-on labs waarin studenten netwerken configureren, beheren en beveiligen, waardoor ze praktische vaardigheden ontwikkelen die essentieel zijn in het moderne IT-landschap. Daarnaast zal de cursus de nieuwste trends en technologieën op het gebied van cybersecurity behandelen, waardoor studenten goed voorbereid worden op uitdagende rollen als netwerk- en beveiligingsspecialisten.") },
                    { "Artificial Intelligence and Machine Learning", ("AIML", "De fundamenten van Kunstmatige Intelligentie en Machine Learning (AIML) worden uitgebreid behandeld in deze cursus. Studenten zullen niet alleen de theoretische principes van AI en machine learning begrijpen, maar ook hands-on ervaring opdoen met het ontwikkelen en trainen van machine learning-modellen. Door middel van projecten zullen studenten leren hoe ze algoritmische oplossingen kunnen toepassen op echte problemen, en zullen ze inzicht krijgen in de ethische overwegingen van AI. De cursus zal ook de toepassing van AI in verschillende domeinen verkennen, waardoor studenten worden voorbereid op boeiende carrières in opkomende technologische gebieden.") },
                    { "Cloud Computing Technologies", ("CCT", "Cloud-infrastructuur en -diensten worden uitgebreid behandeld in de cursus Cloud Computing Technologies (CCT). Studenten zullen niet alleen vertrouwd raken met de basisprincipes van cloudarchitecturen, maar ook leren hoe ze cloudservices effectief kunnen implementeren en beheren. De cursus omvat hands-on labs waarin studenten werken met populaire cloudplatforms, waardoor ze praktische vaardigheden ontwikkelen die essentieel zijn in de hedendaagse IT-omgeving. Bovendien zal de cursus de integratie van cloudoplossingen in bestaande infrastructuur benadrukken, waardoor studenten worden voorbereid op de evoluerende eisen van schaalbare en flexibele IT-oplossingen.") },
                    { "Capstone Project and Final Assessment", ("CPFA", "Het Capstone-project en de eindbeoordeling (CPFA) vormen het hoogtepunt van het curriculum, waarbij studenten hun opgedane kennis toepassen op een uitgebreid geïntegreerd project. Gedurende deze fase zullen studenten niet alleen technische vaardigheden demonstreren, maar ook hun vermogen om complexe IT-problemen op te lossen, effectief te communiceren en samen te werken in een team. Het project omvat alle aspecten van het IT-gebied, waaronder programmering, databasebeheer, netwerkontwerp en meer. Door dit project zullen studenten niet alleen hun individuele competenties aantonen, maar ook laten zien hoe ze diverse IT-vaardigheden kunnen combineren om robuuste oplossingen te leveren. Het finale assessment zal niet alleen de technische aspecten evalueren, maar ook het vermogen van studenten om projecten te plannen, te documenteren en professioneel te presenteren, waardoor ze klaar zijn om met vertrouwen de arbeidsmarkt te betreden.") }
                 };


                Random random = new Random();
                foreach (var semester in itSemesters)
                {
                    string name = semester.Key;
                    string abbreviation = semester.Value.Abbreviation;
                    string description = semester.Value.Description;
                    string insertQuery = "INSERT INTO semester (name, abbreviation, description) " +
                        "VALUES(@name, @abbreviation, @description)";
                    using (SqlCommand command = new SqlCommand(insertQuery, con))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@abbreviation", abbreviation);
                        command.Parameters.AddWithValue("@description", description);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
            finally
            {
                if (con.State != System.Data.ConnectionState.Closed)
                {
                    DatabaseConnection.CloseConnection(con);
                }
            }
        }

        public List<String> GetAllSemesterAbbreviations()
        {
            string query = "SELECT abbreviation FROM semester;";
            List<string> abbreviations = new List<string>();
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string abbreviation = reader["abbreviation"].ToString();
                                abbreviations.Add(abbreviation);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
                finally
                {
                    DatabaseConnection.CloseConnection(connection);
                }
            }
            return abbreviations;
        }

        public List<Semester> GetAllSemesters()
        {
            string query = "SELECT * FROM semester";
            List<Semester> semesters = new List<Semester>();
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader["name"].ToString();
                                string abbreviation = reader["abbreviation"].ToString();
                                string description = reader["description"].ToString();
                                semesters.Add(new Semester(name, abbreviation, description));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
                finally
                {
                    DatabaseConnection.CloseConnection(connection);
                }
            }
            return semesters;
        }

        public List<String> GetAllSemesterNames()
        {
            string query = "SELECT name FROM semester;";
            List<string> names = new List<string>();
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader["name"].ToString();
                                names.Add(name);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
                finally
                {
                    DatabaseConnection.CloseConnection(connection);
                }
            }
            return names;
        }

        public void AddSemester(SqlConnection connection, Semester semester)
        {
            string query = "INSERT INTO semester (name, abbreviation, description) " +
                "VALUES(@name, @abbreviation, @description)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", semester.Name);
                command.Parameters.AddWithValue("@abbreviation", semester.Abbreviation);
                command.Parameters.AddWithValue("@description", semester.Description);

                command.ExecuteNonQuery();
            }

        }


        public Semester GetSemesterByName(string name)
        {
            string query = "SELECT * FROM semester WHERE name = @name";
            Semester semester = null;
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string abbreviation = reader["abbreviation"].ToString();
                                string description = reader["description"].ToString();
                                semester = new Semester(name, abbreviation, description);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
                finally
                {
                    DatabaseConnection.CloseConnection(connection);
                }
            }
            return semester;
        }

        public void UpdateSemester(SqlConnection connection, Semester semester, Semester OldSemester)
        {
            string query = "UPDATE semester SET name = @name, abbreviation = @abbreviation, description = @description WHERE name = @Oldname";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", semester.Name);
                command.Parameters.AddWithValue("@abbreviation", semester.Abbreviation);
                command.Parameters.AddWithValue("@description", semester.Description);
                command.Parameters.AddWithValue("@Oldname", OldSemester.Name);

                command.ExecuteNonQuery();
            }
        }
    }
}
