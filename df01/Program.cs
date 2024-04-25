using MySql.Data.MySqlClient;
using System;

namespace df01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //připojovací řetězec k navázání spojení na určitou databázovou službu
            //"server=localhost" - server, ke kterému se bude aplikace připojovat
            //"port=3306" - specifikuje port, na kterém běží databázový server
            //"user=root" - uživatelské jméno, které bude použito k přihlášení do databáze
            //"password=" - heslo, které bude použito k přihlášení do databáze
            //"database=db01" - specifikuje jméno databáze, ke kterému se má aplikace připojit
            string connectionString = "server=localhost;port=3306;user=root;password=;database=db01;";

            try
            {
                //konstrukce using (instance třídy se po skončení této konstrukce sama zavře)
                //vytvoření instance třídy MySqlConnection zastupující spojení s MySQL databází
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    //spojení se otevře zavoláním metody open
                    connection.Open();
                    Console.WriteLine("Připojeno k databázi");

                    //příkaz pro vyhledání prodoktu
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        //určuje text příkazu SQL, který chceme provést 
                        //"@id" je pojmenování pro proměnnou, která bude nahrazena skutečnou hodnotou, když bude dotaz vykonán
                        command.CommandText = "SELECT id, name, price FROM products WHERE id = @id";

                        //do parametru (@id) se očekává, že bude obsahovat hodnotu UInt32 (celé kladné číslo)
                        command.Parameters.Add("@id", MySqlDbType.UInt32);

                        //command.Parameters.AddWithValue("@id", id);

                        //zkontroluje syntaxi sql příkazu a zjistí zda je správně sestavený
                        //přípraví si plán provedení dotazu (urči jak bude dotaz proveden a jak bude optimalizován)
                        command.Prepare();

                        Console.Write("Zadej id produktu, které chceš zobrazit: ");
                        int id;
                        if (!int.TryParse(Console.ReadLine(), out id))
                        {
                            Console.WriteLine("Tohle není číslo. Bye.");
                            return;
                        }

                        //skutečné dosazení do proměnné "@id" hodnotu proměnné s názvem "id"
                        command.Parameters["@id"].Value = id;

                        //ExecuteNonQuery - DML (manipulace s daty)
                        //ExecuteReader - 
                        //ExecuteScalar

                        using (MySqlDataReader data = command.ExecuteReader())
                        {
                            if (data.Read())
                            {
                                Console.WriteLine(String.Format("{0}\t{1}\t{2}", data["id"], data["name"], data["price"]));
                            }
                            else
                            {
                                Console.WriteLine("Takové id tady nemáme.");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Něco se nepovedlo...");
                throw;
            }

            Console.ReadLine();
        }
    }
}
