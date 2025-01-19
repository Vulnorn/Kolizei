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

    interface IDamageable
    {
        void TakeDamage(int damage);
    }

   abstract class Unit
    {
        public Unit(string name, int heals, int attaks, int defens, int alternativeResource)
        {
            Name = name;
            Heals = heals;
            Attaks = attaks;
            Defens = defens;
            AlternativeResource = alternativeResource;
        }

        public int Heals { get; private set; }
        public string Name { get; private set; }
        public int Attaks { get; private set; }
        public int Defens { get; private set; }
        public int AlternativeResource {  get; private set; }

        public abstract void ShoyInfo();
        public abstract int Attack();
    }

    class Warrior : Unit, IDamageable
    {

    }

    class Mage : Unit, IDamageable
    {

    }

    class Berserker : Unit, IDamageable
    {
 
    }

    class Gladiator : Unit, IDamageable
    {
 
    }

    class Rogue : Unit, IDamageable
    {

    }
}
