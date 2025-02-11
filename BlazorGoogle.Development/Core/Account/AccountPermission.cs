using BlazorGoogle.Development.Core.Enums;
using BlazorGoogle.Development.Data;
using System.Data;
using System.Security;

namespace BlazorGoogle.Development.Core.Account
{
    public class AccountPermission
    {

        public static void AssignPermission(string accountId, PermissionType permission)
        {
            Database db = new(new TransactionDatabase());
            db.Command.Parameters.AddWithValue("@AccountID", accountId);
            db.Command.Parameters.AddWithValue("@PermissionID", permission);
            db.RunSP("Core.Account_Permission_Create");
            db.Close();
        }

        public static void UnassignPermission(string accountId, PermissionType permission)
        {
            Database db = new(new TransactionDatabase());
            db.Command.Parameters.AddWithValue("@AccountID", accountId);
            db.Command.Parameters.AddWithValue("@PermissionID", permission);
            db.RunSP("Core.Account_Permission_Delete");
            db.Close();
        }

        public static IEnumerable<PermissionType> ReadAllPermissionsByAccountId(string id)
        {
            Database db = new(new TransactionDatabase());
            var lstPermission = new List<PermissionType>();

            db.Command.Parameters.AddWithValue("@ID", id);
            db.RunSPReturnReader("Core.Account_Permission_Read_All_By_Account_ID");

            while (db.Reader.Read())
            {
                lstPermission.Add((PermissionType)db.Reader["Account_Permission_ID"]);
            }

            db.Close();

            return lstPermission;
        }
    }
}
