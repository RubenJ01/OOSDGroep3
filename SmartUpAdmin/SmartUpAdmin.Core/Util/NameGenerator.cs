namespace SmartUp.Core.Util
{
    public class NameGenerator
    {
        public static string GenerateRandomName()
        {
            string[] firstNames = { "John", "Jane", "Alex", "Emily", "Michael", "Sophia", "Daniel", "Olivia", "David", "Emma", "Liam", 
                "Ava", "Noah", "Isabella", "Ethan", "Piet", "Joëlle", "Tyrone", "Tim", "Eric", "Jarno", "Scott", "Stefan", "Abe", "Rob",
            "André", "Monique", "Abraham", "Mozes", "Cees", "Mariska", "Marielle", "Alexander", "Willem", "Olga", "Tarzan", "Sven",
            "Gerard", "Joling", "Gordon", "Nova", "Karolien", "Bakker", "Carlijn", "Saar", "Sarath", "Judith", "Kevin", "Henk", "Floris",
            "Jelmer", "Renske", "Luteijn", "Arno", "Arnout", "Will", "Kris", "Demian", "Sepp", "Robin", "Jason", "Niels", "Kornelis", "Gup",
            "Julian", "Kok-chan", "Mark", "Jerry", "Bernardus", "Koos", "Alexandrea", "Andrea", "Dale", "Dana", "Raffael"};
            Random random = new Random();
            int index = random.Next(firstNames.Length);
            return firstNames[index];
        }

        public static string GenerateRandomInfix()
        {
            string[] infixes = { "von", "de", "van", "Mc", "Mac", "di", "el", "al", "La", "Le", "del", "St.", "Fitz" };
            Random random = new Random();
            int index = random.Next(infixes.Length);
            return infixes[index];
        }
    }

}
