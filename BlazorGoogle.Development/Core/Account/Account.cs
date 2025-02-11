using BlazorGoogle.Development.Core.Enums;
using BlazorGoogle.Development.Data;
using System.Data;
using System.Security;

namespace BlazorGoogle.Development.Core.Account
{
    public class Account : BaseDataObject
    {
        #region Private Variables

        private string _Email;
        private string _FirstName;
        private string _LastName;
        private List<PermissionType> _Permissions = [];

        #endregion

        #region Properties

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        public string FullName
        {
            get { return $"{_FirstName} {LastName}"; }
        }

        public IEnumerable<PermissionType> Permissions
        {
            get { return _Permissions; }
            set { _Permissions = (List<PermissionType>)value; }
        }

        #endregion

        #region CRUD
        public void ExtractFromReader(IDataReader reader)
        {
            ID = reader["Account_ID"].ToString();
            _FirstName = reader["Account_First_Name"].ToString();
            _LastName = reader["Account_Last_Name"].ToString();
            _Email = reader["Account_Email"].ToString();
        }

        public static Account Create(string email, string firstName, string lastName)
        {
            Database db = new(new TransactionDatabase());
            string id = null;

            db.Command.Parameters.AddWithValue("@Email", email);
            db.Command.Parameters.AddWithValue("@FirstName", firstName);
            db.Command.Parameters.AddWithValue("@LastName", lastName);


            id = (string)db.RunSPReturnScalar("Core.Account_Create");

            return Read(id);
        }

        public static Account Read(string id)
        {
            Database db = new(new TransactionDatabase());
            Account act = null;

            db.Command.Parameters.AddWithValue("@ID", id);

            db.RunSPReturnReader("Core.Account_Read");

            if (db.Reader.Read())
            {
                act = new Account();
                act.ExtractFromReader(db.Reader);
            }

            db.Close();

            return act;
        }


        public void Update()
        {
            Database db = new(new TransactionDatabase());

            db.Command.Parameters.AddWithValue("@ID", ID);
            db.Command.Parameters.AddWithValue("@FirstName", _FirstName);
            db.Command.Parameters.AddWithValue("@LastName", _LastName);
            db.Command.Parameters.AddWithValue("@FullName", FullName);
            db.RunSP("Core.Account_Update");
            db.Close();
        }

        #endregion

        #region Other Methods

        public bool HasPermission(PermissionType permission)
        {
            return _Permissions.Contains(permission);
        }

        public static Account FindByEmail(string email)
        {
            Database db = new(new TransactionDatabase());
            Account act = null;

            db.Command.Parameters.AddWithValue("@Email", email);

            db.RunSPReturnReader("Core.Account_Find_By_Email");

            if (db.Reader.Read())
            {
                act = new Account();
                act.ExtractFromReader(db.Reader);
            }

            db.Close();

            return act;
        }

        public static IEnumerable<Account> ReadAll()
        {
            Database db = new(new TransactionDatabase());
            Account act = null;
            var lstAccount = new List<Account>();

            db.RunSPReturnReader("Core.Account_Read_All");

            while (db.Reader.Read())
            {
                act = new Account();
                ((Account)null).ExtractFromReader(db.Reader);
                lstAccount.Add(null);
            }

            db.Close();

            return lstAccount;
        }

        #endregion

    }
}
