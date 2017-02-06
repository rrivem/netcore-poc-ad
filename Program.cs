using System;
using Novell.Directory.Ldap;

namespace ConsoleApplication
{
    public class Program
    {
	public static void Print(LdapEntry entry)
	{
		Console.WriteLine($"DN: {entry.DN}");
		foreach (LdapAttribute attr in entry.getAttributeSet())
		{
			Console.WriteLine($"{attr.Name}: {attr.StringValue}");
		}
	}
	
	public static bool Find(LdapConnection conn, string search, string attrName, string attrValue)
	{
		var filter = $"({attrName}={attrValue})";
		Console.WriteLine($"Searching: {search} {filter}");
		try
		{
			var lsc = conn.Search(
				search,
				LdapConnection.SCOPE_SUB,
				filter,
				null,
				false);
	
			while (lsc.hasMore())
			{
				Print(lsc.next());
			}
		}
		catch (LdapException ex)
		{
			Console.WriteLine($"{ex.Message} {ex.LdapErrorMessage}");
		}

		return false;
	}

        public static void Main(string[] args)
        {
            string host = args[0];
            string login = args[1];
            string pass = args[2];
            string searchBase = args[3];
            string attrName = args[4];
            string attrValue = args[5];
            var conn = new LdapConnection();
            conn.Connect(host, LdapConnection.DEFAULT_PORT);
            conn.Bind(login, pass);
            Console.WriteLine("Bind successful");

            Find(conn, searchBase, attrName, attrValue);
            
            Console.WriteLine("Disconnecting...");
            conn.Disconnect();
        }
    }
}
