using DomainLayer;
using DomainLayer.Interfaces;
using DomainLayer.Objects;
using System.Data;
using System.Data.SqlClient;

namespace PersistenceLayer {
    public class PaymentHubRepo : IPaymentHubRepo {

        // SETUP SECTION
        private string _connectionString;
        public PaymentHubRepo(string connectionString) {
            _connectionString = connectionString;
        }

        // BANK SECTION

        // ADD BANK
        public void AddBank(Bank bank) {
            try {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = "INSERT INTO Bank (Naam, Email, TelefoonNummer, ContactPersoonId) OUTPUT inserted.Id VALUES (@Naam, @Email, @TelefoonNummer, @ContactPersoonId)";
                    command.Parameters.AddWithValue("@Naam", bank.Naam);
                    command.Parameters.AddWithValue("@Email", bank.Email);
                    command.Parameters.AddWithValue("@TelefoonNummer", bank.TelefoonNummer);
                    command.Parameters.AddWithValue("@ContactPersoonId", bank.ContactPersoon.Id);
                    int insertedBankId = (int)command.ExecuteNonQuery();
                    bank.Id = insertedBankId;
                    connection.Close();
                }
            } catch (Exception ex){
                throw new Exception("AddBank", ex);
            }
        }

        // GET BANKS
        public List<Bank> GetBanks() {
            try {
                List<Bank> banks = new List<Bank>();
                string sql = "SELECT * FROM Bank";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                    using (SqlCommand command = connection.CreateCommand()) {
                        connection.Open();
                        command.CommandText = sql;
                        IDataReader dataReader = command.ExecuteReader();
                        while (dataReader.Read()) {
                             Bank bank = new Bank();
                             bank.Id = (int)dataReader["Id"];
                             bank.Naam = (string)dataReader["Naam"];
                             bank.Email = (string)dataReader["Email"];
                             bank.TelefoonNummer = (string)dataReader["TelefoonNummer"];
                             int contactPersoonId =  (int)dataReader["ContactPersoonId"];
                             bank.ContactPersoon = GetUserById(contactPersoonId);
                             banks.Add(bank);
                        }
                        dataReader.Close();
                        connection.Close();
                    }
                return banks;

            } catch (Exception ex) {
                throw new Exception("GetBank", ex);
            }
        }

        // GET BANK BY ID
        public Bank GetBankById(int id) {
            try {
                string sql = $"SELECT * FROM Bank WHERE Id={id}";
                Bank bank = new Bank();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                    using (SqlCommand command = connection.CreateCommand()) {
                        connection.Open();
                        command.CommandText = sql;
                        IDataReader dataReader = command.ExecuteReader();
                        while (dataReader.Read()) {
                            bank.Id = (int)dataReader["Id"];
                            bank.Naam = (string)dataReader["Naam"];
                            bank.Email = (string)dataReader["Email"];
                            bank.TelefoonNummer = (string)dataReader["TelefoonNummer"];
                            int contactPersoonId = (int)dataReader["ContactPersoonId"];
                            bank.ContactPersoon = GetUserById(contactPersoonId);
                        }
                        dataReader.Close();
                        connection.Close();
                    }
                return bank;
            } catch (Exception ex) {
                throw new Exception("GetBank", ex);
            }
        }   

        // GET BANK BY NAME
        public Bank GetBankByName(string name) {
            try {
                string sql = "SELECT * FROM Bank WHERE Naam=@Naam";
                Bank bank = new Bank();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.AddWithValue("@Naam", name);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) {
                        bank.Id = (int)reader["Id"];
                        bank.Naam = (string)reader["Naam"];
                        bank.Email = (string)reader["Email"];
                        bank.TelefoonNummer = (string)reader["TelefoonNummer"];
                        int contactPersoonId = (int)reader["ContactPersoonId"];
                        bank.ContactPersoon = GetUserById(contactPersoonId);
                    }
                    reader.Close();
                    connection.Close();
                }
                return bank;
            } catch (Exception ex) {
                throw new Exception("GetBankByName", ex);
            }

        }

        // GET BANKID BY NAME
        public int GetBankIdByName(string name) {
            try {
                string sql = "SELECT Id FROM Bank WHERE Naam=@Naam";
                Bank bank = new Bank();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.AddWithValue("@Naam", name);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) {
                        bank.Id = (int)reader["Id"];
                    }
                    reader.Close();
                    connection.Close();
                }
                return bank.Id;
            } catch (Exception ex) {
                throw new Exception("GetBankIdByName", ex);
            }

        }

        // UPDATE BANK
        public void UpdateBankById(int Id, Bank bank) {
            try {
                string sql = $"UPDATE Bank SET Naam={bank.Naam}, Email={bank.Email}, TelefoonNummer={bank.TelefoonNummer}, ContactPersoonId={bank.ContactPersoon.Id} WHERE Id={Id}";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                    using (SqlCommand command = connection.CreateCommand()) {
                        connection.Open();
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
            } catch (Exception ex) {
                throw new Exception("UpdateBank", ex);
            }
        }

        // DELETE BANK
        public void DeleteBank(int Id) {
            try {
                string sql = $"DELETE FROM Bank WHERE Id = @id";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("DeleteBank", ex);
            }
        }



        // WEBSHOPS SECTION


        // ADD WEBSHOPS
        public void AddWebshop(Webshop webshop) {
            try {
                string sql = "INSERT INTO Webshop(Naam, Email, TelefoonNummer) OUTPUT inserted.Id VALUES (@Naam, @Email, @TelefoonNummer)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Naam", webshop.Naam);
                    if (webshop.Email != null) {
                        command.Parameters.AddWithValue("@Email", webshop.Email);
                    } else {
                        command.Parameters.AddWithValue("@Email", DBNull.Value);
                    }
                    if (webshop.TelefoonNummer != null) {
                        command.Parameters.AddWithValue("@TelefoonNummer", webshop.TelefoonNummer);
                    } else {
                        command.Parameters.AddWithValue("@TelefoonNummer", DBNull.Value);
                    }
                    int insertedId = (int)command.ExecuteScalar();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("AddWebshop", ex);
            }
        }

        // GET WEBSHOPS
        public List<Webshop> GetAllWebshop() {
            try {
                List<Webshop> webshops = new List<Webshop>();
                string sql = "SELECT * FROM Webshop";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read()) {
                        Webshop webshop = new Webshop();
                        webshop.Id = (int)dataReader["Id"];
                        webshop.Naam = (string)dataReader["Naam"];
                        webshop.Email = (string)dataReader["Email"];
                        webshop.TelefoonNummer = (string)dataReader["TelefoonNummer"];
                        webshops.Add(webshop);
                    }
                    dataReader.Close();
                    connection.Close();
                }
                return webshops;

            } catch (Exception ex) {
                throw new Exception("GetAllWebshop", ex);
            }
        }

        // GET WEBSHOP BY ID
        public Webshop GetWebshopById(int id) {
            try {
                string sql = $"SELECT * FROM Webshop WHERE Id={id}";
                Webshop webshop = new Webshop();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read()) {
                        webshop.Id = (int)dataReader["Id"];
                        webshop.Naam = (string)dataReader["Naam"];
                        webshop.Email = (string)dataReader["Email"];
                        webshop.TelefoonNummer = (string)dataReader["TelefoonNummer"];
                    }
                    dataReader.Close();
                    connection.Close();
                }
                return webshop;

            } catch (Exception ex) {
                throw new Exception("GetWebshopById", ex);
            }
        }

        // GET WEBSHOP BY NAME
        public Webshop GetWebshopByName(string name) {
            try {
                Webshop webshop = new Webshop();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = "SELECT * FROM Webshop WHERE Naam=@name";
                    command.Parameters.AddWithValue("@name", name);

                    using (SqlDataReader dataReader = command.ExecuteReader()) {
                        while (dataReader.Read()) {
                            webshop.Id = (int)dataReader["Id"];
                            webshop.Naam = (string)dataReader["Naam"];
                            webshop.Email = (string)dataReader["Email"];
                            webshop.TelefoonNummer = (string)dataReader["TelefoonNummer"];
                        }
                    }
                }
                return webshop;
            } catch (Exception ex) {
                throw new Exception("GetWebshopByName", ex);
            }
        }

        // GET WEBSHOPID BY NAME
        public int GetWebshopIdByName(string name) {
            try {
                string sql = $"SELECT * FROM Webshop WHERE Naam={name}";
                Webshop webshop = new Webshop();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read()) {
                        webshop.Id = (int)dataReader["Id"];
                        webshop.Naam = (string)dataReader["Naam"];
                        webshop.Email = (string)dataReader["Email"];
                        webshop.TelefoonNummer = (string)dataReader["TelefoonNummer"];
                    }
                    dataReader.Close();
                    connection.Close();
                }
                return webshop.Id;

            } catch (Exception ex) {
                throw new Exception("GetWebshopIdByName", ex);
            }
        }

        // UPDATE WEBSHOP
        public void UpdateWebshopById(int id, Webshop webshop) {
            try {
                string sql = $"UPDATE Webshop SET Naam = @Name, Email = @Email, TelefoonNummer = @TelefoonNummer WHERE Id = @id";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Name", webshop.Naam);
                    if (webshop.Email != null) {
                        command.Parameters.AddWithValue("@Email", webshop.Email);
                    } else {
                        command.Parameters.AddWithValue("@Email", DBNull.Value);
                    }
                    if (webshop.TelefoonNummer != null) {
                        command.Parameters.AddWithValue("@TelefoonNummer", webshop.TelefoonNummer);
                    } else {
                        command.Parameters.AddWithValue("@Email", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("UpdateWebshopById", ex);
            }
        }

        // DELETE WEBSHOP
        public void DeleteWebshopById(int id) {
            try {
                string sql = $"DELETE FROM Webshop WHERE Id = @id";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("DeleteWebshopById", ex);
            }
        }


        // USERACCOUNT SECTION

        // ADD USERACCOUNT
        public void AddUserAccount(UserAccount ua) {
            try {
                string sql = "INSERT INTO UserAccount(Wachtwoord, Email) OUTPUT inserted.Id VALUES (@Wachtwoord, @Email)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Wachtwoord", ua.Password);
                    command.Parameters.AddWithValue("@Email", ua.Email);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("AddUserAccount", ex);
            }
        }

        // GET ALL USERACCOUNTS
        public List<UserAccount> GetUserAccounts() {
            try {
                List<UserAccount> userAccounts = new List<UserAccount>();
                string sql = "SELECT * FROM UserAccount";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read()) {
                        UserAccount ua = new UserAccount();
                        ua.Id = (int)dataReader["Id"];
                        ua.Password = (string)dataReader["Wachtwoord"];
                        ua.Email = (string)dataReader["Email"];
                        userAccounts.Add(ua);
                    }
                    dataReader.Close();
                    connection.Close();
                }
                return userAccounts;

            } catch (Exception ex) {
                throw new Exception("GetAllUserAccounts", ex);
            }
        }

        // GET USERACCOUNT BY ID
        public UserAccount GetUserAccountById(int id) {
            try {
                string sql = $"SELECT * FROM UserAccount WHERE Id={id}";
                UserAccount ua = new UserAccount();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read()) {
                        ua.Id = (int)dataReader["Id"];
                        ua.Password = (string)dataReader["WachtwoorHash"];
                        ua.Email = (string)dataReader["Email"];
                    }
                    dataReader.Close();
                    connection.Close();
                }
                return ua;
            } catch (Exception ex) {
                throw new Exception("GetUserAccountById", ex);
            }
        }

        // GET USERACCOUNT BY EMAIL
        public UserAccount GetUserAccountByEmail(string email) {
            try {
                string sql = "SELECT * FROM UserAccount WHERE Email=@Email";
                UserAccount ua = new UserAccount();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.AddWithValue("@Email", email);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) {
                        ua.Id = (int)reader["Id"];
                        ua.Password = (string)reader["Wachtwoord"];
                        ua.Email = (string)reader["Email"];
                    }
                    reader.Close();
                    connection.Close();
                }
                return ua;
            } catch (Exception ex) {
                throw new Exception("GetUserAccountByEmail", ex);
            }

        }

        // UPDATE USERACCOUNT
        public void UpdateUserAccountById(int id, UserAccount ua) {
            try {
                string sql = $"UPDATE UserAccount SET Wachtwoord = @Wachtwoord, Email = @Email WHERE Id = @id";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Wachtwoord", ua.Password);
                    command.Parameters.AddWithValue("@Email", ua.Email);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("UpdateUserAccountById", ex);
            }
        }

        // DELETE USERACCOUNT
        public void DeleteUserAccountById(int id) {
            try {
                string sql = $"DELETE FROM UserAccount WHERE Id = @id";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("DeleteUserAccountById", ex);
            }
        }

        // PAYMENT SECTION
        
        // ADD PAYMENT

        public void AddPayment(Payment payment) {
            try {
                string sql = "INSERT INTO Payment(Bedrag, BegunstigdeWebshopId, UserId, Betaald, Datum, TijdStip) VALUES (@Bedrag, @BegunstigdeWebshopId, @UserId, @Betaald, @Datum, @TijdStip)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.AddWithValue("@Bedrag", payment.Bedrag);
                    command.Parameters.AddWithValue("@BegunstigdeWebshopId", payment.BegunstigeWebshopId);
                    command.Parameters.AddWithValue("@UserId", payment.UserId);
                    command.Parameters.AddWithValue("@Betaald", payment.Betaald);
                    command.Parameters.AddWithValue("@Datum", payment.Datum.Date);
                    command.Parameters.AddWithValue("@TijdStip", payment.Datum.TimeOfDay);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("AddPayment", ex);
            }
        }

        // GET ALL PAYMENTS
        public List<Payment> GetAllPayments() {
            try {
                List<Payment> payments = new List<Payment>();
                string sql = "SELECT * FROM Payment";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read()) {
                        Payment payment = new Payment();
                        payment.Id = (int)dataReader["Id"];
                        payment.Bedrag = (decimal)dataReader["Bedrag"];
                        payment.BegunstigeWebshopId = (int)dataReader["BegunstigdeWebshopId"];
                        payment.UserId = (int)dataReader["UserId"];
                        if((string)dataReader["Betaald"] == "1") {
                            payment.Betaald = true;
                        } else {
                            payment.Betaald = false;
                        }
                        payment.Datum = (DateTime)dataReader["Datum"];
                        payments.Add(payment);
                    }
                    dataReader.Close();
                    connection.Close();
                }
                return payments;
            } catch (Exception ex) {
                throw new Exception("GetAllPayments", ex);
            }
        }

        // GET PAYMENT BY ID
        public Payment GetPaymentById(int id) {
            try {
                string sql = $"SELECT * FROM Payment WHERE Id={id}";
                Payment payment = new Payment();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read()) {
                        payment.Id = (int)dataReader["Id"];
                        payment.Bedrag = (decimal)dataReader["Bedrag"];
                        payment.BegunstigeWebshopId = (int)dataReader["BegunstigdeWebshopId"];
                        payment.UserId = (int)dataReader["UserId"];
                        string betaald = (string)dataReader["Betaald"];
                        if (betaald == "1") {
                            payment.Betaald = true;
                        } else {
                            payment.Betaald = false;
                        }
                        payment.Datum = (DateTime)dataReader["Datum"];
                    }
                    dataReader.Close();
                    connection.Close();
                }
                return payment;
            } catch (Exception ex) {
                throw new Exception("GetPaymentById", ex);
            }
        }

        // GET PAYMENTS BY USERID
        public List<Payment> GetPaymentsByUserId(int userId) {
            try {
                List<Payment> payments = new List<Payment>();
                string sql = $"SELECT * FROM Payment WHERE UserId={userId}";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read()) {
                        Payment payment = new Payment();
                        payment.Id = (int)dataReader["Id"];
                        payment.Bedrag = (decimal)dataReader["Bedrag"];
                        payment.BegunstigeWebshopId = (int)dataReader["BegunstigdeWebshopId"];
                        payment.UserId = (int)dataReader["UserId"];
                        if ((string)dataReader["Betaald"] == "0") {
                            payment.Betaald = false;
                        } else {
                            payment.Betaald = true;
                        }
                        payment.Datum = (DateTime)dataReader["Datum"];
                        payments.Add(payment);
                    }
                    dataReader.Close();
                    connection.Close();
                }
                return payments;
            } catch (Exception ex) {
                throw new Exception("GetPaymentsByUserId", ex);
            }
        }

        // GET PAYMENTS BY WEBSHOPID

        public List<Payment> GetPaymentsByWebshopId(int webshopId) {
            try {
                List<Payment> payments = new List<Payment>();
                string sql = $"SELECT * FROM Payment WHERE BegunstigdeWebshopId={webshopId}";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read()) {
                        Payment payment = new Payment();
                        payment.Id = (int)dataReader["Id"];
                        payment.Bedrag = (decimal)dataReader["Bedrag"];
                        payment.BegunstigeWebshopId = (int)dataReader["BegunstigdeWebshopId"];
                        payment.UserId = (int)dataReader["UserId"];
                        payment.Betaald = (bool)dataReader["Betaald"];
                        payment.Datum = (DateTime)dataReader["Datum"];
                        payments.Add(payment);
                    }
                    dataReader.Close();
                    connection.Close();
                }
                return payments;
            } catch (Exception ex) {
                throw new Exception("GetPaymentsByWebshopId", ex);
            }
        }

        // UPDATE PAYMENT
        public void UpdatePayment(Payment payment) {
            try {
                string sql = "UPDATE Payment SET Bedrag=@Bedrag, BegunstigdeWebshopId=@BegunstigdeWebshopId, UserId=@UserId, Betaald=@Betaald, Datum=@Datum, TijdStip=@TijdStip WHERE Id=@Id";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Bedrag", payment.Bedrag);
                    command.Parameters.AddWithValue("@BegunstigdeWebshopId", payment.BegunstigeWebshopId);
                    command.Parameters.AddWithValue("@UserId", payment.UserId);
                    command.Parameters.AddWithValue("@Betaald", payment.Betaald);
                    command.Parameters.AddWithValue("@Datum", payment.Datum.Date);
                    command.Parameters.AddWithValue("@TijdStip", payment.Datum.TimeOfDay);
                    command.Parameters.AddWithValue("@Id", payment.Id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("UpdatePayment", ex);
            }
        }

        // SET BETAALD TO TRUE
        public void SetBetaaldToTrue(int id) {
            try {
                string sql = $"UPDATE Payment SET Betaald=1 WHERE Id={id}";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("SetBetaaldToTrue", ex);
            }
        }

        // DELETE PAYMENT
        public void DeletePayment(int id) {
            try {
                string sql = $"DELETE FROM Payment WHERE Id={id}";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("DeletePayment", ex);
            }
        }

        // GET PAYMENTID BY USERID AND WEBSHOPID
        public int GetPaymentIdByUserIdAndWebshopId(int userId, int webshopId) {
            try {
                int paymentId = 0;
                string sql = $"SELECT Id FROM Payment WHERE UserId={userId} AND BegunstigdeWebshopId={webshopId}";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read()) {
                        paymentId = (int)dataReader["Id"];
                    }
                    dataReader.Close();
                    connection.Close();
                }
                return paymentId;
            } catch (Exception ex) {
                throw new Exception("GetPaymentIdByUserIdAndWebshopId", ex);
            }
        }


        // USERS SECTION

        // ADD USER
        public void AddUser(User user) {
            try {
                string sql = "INSERT INTO [User] (Voornaam, Achternaam, TelefoonNummer, Email) " +
             "OUTPUT inserted.Id " +
             "VALUES (@Voornaam, @Achternaam, @TelefoonNummer, @Email)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Voornaam", user.Voornaam);
                    command.Parameters.AddWithValue("@Achternaam", user.Achternaam);
                    command.Parameters.AddWithValue("@TelefoonNummer", user.TelefoonNummer);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("AddUser", ex);
            }
        }

        // GET ALL USERS

        public List<User> GetUsers() {
            try {
                List<User> users = new List<User>();
                string sql = "SELECT * FROM [PaymentHub].[dbo].[User]";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        User user = new User();
                        user.Id = (int)reader["Id"];
                        //user.Aanspreking = (string)reader["Aanspreking"];
                        user.Voornaam = (string)reader["Voornaam"];
                        user.Achternaam = (string)reader["Voornaam"];
                        //user.Adress.StraatNaam = (string)reader["Straat"];
                        //user.Adress.HuisNummer = (string)reader["Huisnummer"];
                        //user.Adress.Gemeente = (string)reader["Gemeente"];
                        user.TelefoonNummer = (string)reader["TelefoonNummer"];
                        //user.Adress.Land = (string)reader["Land"];
                        //user.Taal = (string)reader["Taal"];
                        //user.GeboorteDatum = (DateTime)reader["GeboorteDatum"];
                        //user.Adress.BusNummer = (string)reader["BusNummer"];
                        user.Email = (string)reader["Email"];
                        users.Add(user);
                    }

                    connection.Close();
                }
                return users;
            } catch (Exception ex) {

                throw new Exception("GetAllUsers", ex);
            }
        }

        // GET USER BY ID

        public User GetUserById(int id) {
            User user = new();
            try {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = "SELECT * from [PaymentHub].[dbo].[User] WHERE Id=@Id";
                    command.Parameters.AddWithValue("@Id", id);
                    using (IDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            user.Id = (int)reader["Id"];
                            //user.Aanspreking = (string)reader["Aanspreking"];
                            user.Voornaam = (string)reader["Voornaam"];
                            user.Achternaam = (string)reader["Achternaam"]; // Correct property name
                            //user.Adress.StraatNaam = (string)reader["Straat"];
                            //user.Adress.HuisNummer = (string)reader["Huisnummer"];
                            //user.Adress.Gemeente = (string)reader["Gemeente"];
                            user.TelefoonNummer = (string)reader["TelefoonNummer"];
                            //user.Adress.Land = (string)reader["Land"];
                            //user.Taal = (string)reader["Taal"];
                            //user.GeboorteDatum = (DateTime)reader["GeboorteDatum"];
                            //user.Adress.BusNummer = (string)reader["BusNummer"];
                            user.Email = (string)reader["Email"];
                        }
                    }
                }

                return user;

            } catch (Exception ex) {

                throw new Exception("GetUserById", ex);
            }
        }

        // GET USER BY Email
        public User GetUserByEmail(string email) {
            try {
                string sql = "SELECT * FROM [PaymentHub].[dbo].[User] WHERE Email=@Email";
                User user = new User();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.AddWithValue("@Email", email);
                    connection.Open();
                    using (IDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            user.Id = (int)reader["Id"];
                            //user.Aanspreking = (string)reader["Aanspreking"];
                            user.Voornaam = (string)reader["Voornaam"];
                            user.Achternaam = (string)reader["Achternaam"];
                            //user.Adress.StraatNaam = (string)reader["Straat"];
                            //user.Adress.HuisNummer = (string)reader["Huisnummer"];
                            //user.Adress.Gemeente = (string)reader["Gemeente"];
                            user.TelefoonNummer = (string)reader["TelefoonNummer"];
                            //user.Adress.Land = (string)reader["Land"];
                            //user.Taal = (string)reader["Taal"];
                            //user.GeboorteDatum = (DateTime)reader["GeboorteDatum"];
                            //user.Adress.BusNummer = (string)reader["BusNummer"];
                            user.Email = (string)reader["Email"];
                        }
                    }
                    connection.Close();
                }
                return user;
            } catch (Exception ex) {

                throw new Exception("GetUserByEmail", ex);
            }
        }

        // GET USERID BY Email

        public int GetUserIdByEmail(string email) {
            try {
                string sql = $"SELECT Id from [PaymentHub].[dbo].[User] WHERE Email={email}";
                int id = 0;
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        id = (int)reader["Id"];
                    }
                    connection.Close();
                }
                return id;
            } catch (Exception ex) {

                throw new Exception("GetUserIdByEmail", ex);
            }
        }

        // UPDATE USER BY ID
        public void UpdateUserByID(int id, User u) {
            try {
                string sql = "UPDATE [PaymentHub].[dbo].[User] SET Aanspreking=@Aanspreking, Voornaam=@Voornaam, Achternaam=@Achternaam, StraatNaam=@StraatNaam, Huisnummer=@Huisnummer, Gemeente=@Gemeente, TelefoonNummer=@TelefoonNummer,Land=@Land,Taal=@Taal,GeboorteDatum=@GeboorteDatum, Email=@Email, BusNummer = @BusNummer WHERE Id=@Id";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Aanspreking", u.Aanspreking);
                    command.Parameters.AddWithValue("@Voornaam", u.Voornaam);
                    command.Parameters.AddWithValue("@Achternaam", u.Achternaam);
                    command.Parameters.AddWithValue("@StraatNaam", u.Adress.StraatNaam);
                    command.Parameters.AddWithValue("@Huisnummer", u.Adress.HuisNummer);
                    command.Parameters.AddWithValue("@Gemeente", u.Adress.Gemeente);
                    command.Parameters.AddWithValue("@TelefoonNummer", u.TelefoonNummer);
                    command.Parameters.AddWithValue("@Land", u.Adress.Land);
                    command.Parameters.AddWithValue("@Taal", u.Taal);
                    command.Parameters.AddWithValue("@GeboorteDatum", u.GeboorteDatum);
                    command.Parameters.AddWithValue("@BusNummer", u.Adress.BusNummer);
                    command.Parameters.AddWithValue("@Email", u.Email);
                    command.Parameters.AddWithValue("@Id", u.Id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("UpdatePayment", ex);
            }
        }

        // GET ACCOUNTNUMBERS BY USERID
        public List<AccountNumber> GetAccountNumbersByUserId(int id) {
            try {
                string sql = $"SELECT * from RekeningNummer WHERE UserId={id}";
                List<AccountNumber> rekeningNummers = new List<AccountNumber>();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        AccountNumber accountNumber = new AccountNumber();
                        accountNumber.Id = (int)reader["Id"];
                        accountNumber.BankId = (int)reader["BankId"];
                        accountNumber.UserId = (int)reader["UserId"];
                        accountNumber.IBAN = (string)reader["IBAN"];
                        rekeningNummers.Add(accountNumber);
                    }
                    connection.Close();
                }
                return rekeningNummers;
            } catch (Exception ex) {
                throw new Exception("GetAccountNumbersByUserId", ex);
            }
        }

        // DELETE USER BY ID
        public void DeleteUser(int id) {
            try {
                string sql = $"DELETE FROM User WHERE Id={id}";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("DeleteUser", ex);
            }
        }

        // ACCOUNTNUMBER SECTION

        // GET ALL ACCOUNTNUMBERS
        public List<AccountNumber> GetAllAccountNumbers() {
            try {
                string sql = "SELECT * from RekeningNummer";
                List<AccountNumber> accountNumbers = new List<AccountNumber>();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        AccountNumber accountNumber = new AccountNumber();
                        accountNumber.Id = (int)reader["Id"];
                        accountNumber.IBAN = (string)reader["IBAN"];
                        accountNumber.UserId = (int)reader["UserId"];
                        accountNumber.BankId = (int)reader["BankId"];
                        accountNumbers.Add(accountNumber);
                    }
                    connection.Close();
                }
                return accountNumbers;
            } catch (Exception ex) {
                throw new Exception("GetAllAccountNumbers", ex);
            }
        }

        // GET ACCOUNTNUMBER BY UserID
        public List<AccountNumber> GetAccountNumberByUserId(int id) {
            try {
                string sql = $"SELECT * from RekeningNummer WHERE UserId={id}";
                List<AccountNumber> accountNumbers = new List<AccountNumber>();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        AccountNumber accountNumber = new AccountNumber();
                        accountNumber.Id = (int)reader["Id"];
                        accountNumber.IBAN = (string)reader["IBAN"];
                        accountNumber.UserId = (int)reader["UserId"];
                        accountNumber.BankId = (int)reader["BankId"];
                        accountNumbers.Add(accountNumber);
                    }
                    connection.Close();
                }
                return accountNumbers;
            } catch (Exception ex) {
                throw new Exception("GetAccountNumberByUserId", ex);
            }
        }

        // GET ACCOUNTNUMBER BY BANKID

        public AccountNumber GetAccountNumberByBankId(int id) {
            try {
                string sql = $"SELECT * from RekeningNummer WHERE BankId={id}";
                AccountNumber accountNumber = new AccountNumber();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        accountNumber.Id = (int)reader["Id"];
                        accountNumber.IBAN = (string)reader["IBAN"];
                        accountNumber.UserId = (int)reader["UserId"];
                        accountNumber.BankId = (int)reader["BankId"];
                    }
                    connection.Close();
                }
                return accountNumber;
            } catch (Exception ex) {
                throw new Exception("GetAccountNumberByBankId", ex);
            }
        }

        // GET ACCOUNTNUMBER BY IBAN
        public AccountNumber GetAccountNumberByIBAN(string iban) {
            try {
                string sql = $"SELECT * from RekeningNummer WHERE IBAN={iban}";
                AccountNumber accountNumber = new AccountNumber();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        accountNumber.Id = (int)reader["Id"];
                        accountNumber.IBAN = (string)reader["IBAN"];
                        accountNumber.UserId = (int)reader["UserId"];
                        accountNumber.BankId = (int)reader["BankId"];
                    }
                    connection.Close();
                }
                return accountNumber;
            } catch (Exception ex) {
                throw new Exception("GetAccountNumberByIBAN", ex);
            }
        }

        // GET ACCOUNTNUMBER ID BY IBAN
        public int GetAccountNumberIdByIBAN(string iban) {
            try {
                string sql = $"SELECT Id from RekeningNummer WHERE IBAN={iban}";
                int id = 0;
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        id = (int)reader["Id"];
                    }
                    connection.Close();
                }
                return id;
            } catch (Exception ex) {
                throw new Exception("GetAccountNumberIdByIBAN", ex);
            }
        }
        // GET ACCOUNTNUMBER BY USER ID AND BANK ID
        public List<AccountNumber> GetAccountNumbersByUserIdAndBankId(int UserId, int BankId) {
            try {
                string sql = $"SELECT * from RekeningNummer WHERE BankId={BankId} AND UserId={UserId}";
                List<AccountNumber> accountNumbers = new List<AccountNumber>();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        AccountNumber accountNumber = new AccountNumber();
                        accountNumber.Id = (int)reader["Id"];
                        accountNumber.IBAN = (string)reader["IBAN"];
                        accountNumber.UserId = (int)reader["UserId"];
                        accountNumber.BankId = (int)reader["BankId"];
                        accountNumbers.Add(accountNumber);
                    }
                    connection.Close();
                }
                return accountNumbers;
            } catch (Exception ex) {
                throw new Exception("GetAccountNumberByBankId", ex);
            }
        }

        // ADD ACCOUNTNUMBER
        public void AddAccountNumber(AccountNumber accountNumber) {
            try {
                string sql = $"INSERT INTO RekeningNummer (IBAN, UserId, BankId) VALUES (@IBAN, @UserId, @BankId)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@IBAN", accountNumber.IBAN);
                    command.Parameters.AddWithValue("@UserId", accountNumber.UserId);
                    command.Parameters.AddWithValue("@BankId", accountNumber.BankId);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("AddAccountNumber", ex);
            }
        }
        
        // UPDATE ACCOUNTNUMBER
        public void UpdateAccountNumber(int Id, AccountNumber accountNumber) {
            try {
                string sql = $"UPDATE RekeningNummer SET IBAN=@IBAN, UserId=@UserId, BankId=@BankId WHERE Id={Id}";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@IBAN", accountNumber.IBAN);
                    command.Parameters.AddWithValue("@UserId", accountNumber.UserId);
                    command.Parameters.AddWithValue("@BankId", accountNumber.BankId);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("UpdateAccountNumber", ex);
            }
        }

        // DELETE ACCOUNTNUMBER
        public void DeleteAccountNumber(int Id) {
            try {
                string sql = $"DELETE FROM RekeningNummer WHERE Id={Id}";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand()) {
                    connection.Open();
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } catch (Exception ex) {
                throw new Exception("DeleteAccountNumber", ex);
            }
        }
    }
}
