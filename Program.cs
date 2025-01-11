namespace Kolizei
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class Arena
    {

    }

    class Kollizei
    {

    }

    class Unit
    {
        public Unit (string name, int heals,int attaks,int defens) 
        {
            Name = name;
            Heals = heals;
            Ataks = attaks;
            Defens = defens;               
        }

        public int Heals { get; private set; }
        public string Name { get; private set; }
        public int Ataks { get; private set; }
        public int Defens { get; private set; }

    }

    interface IDamageable
    {
        void TakeDamage(int damage);
    }

    class Arcer : Unit, IDamageable
    {
        public Arcer(string name, int heals, int attaks, int defens) : base(name, heals, attaks, defens)
        {

        }

        public void TakeDamage(int damage)
        {
            throw new NotImplementedException();
        }
    }
}
