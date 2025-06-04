using System;

namespace Kolizei
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Kollizei kollizei = new Kollizei();

            kollizei.Work();
        }
    }

    interface IDamageable
    {
        void TakeDamage(int damage);
    }

    class Kollizei
    {
        private List<Unit> _units = new List<Unit>();

        public Kollizei()
        {
            GenerateUnits();
        }

        public void Work()
        {
            const string CommandWatchFight = "1";
            const string CommandExit = "2";

            bool isWork = true;
            int numberUnit1 = 1;
            int numberUnit2 = 2;

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine($"Добро пожаловать в Колизей! На арене выступают бойцы:\n");

                ShoyUnits();

                Console.WriteLine($"Выберите пункт в меню:");
                Console.WriteLine($"{CommandWatchFight} - Смотреть бой");
                Console.WriteLine($"{CommandExit} - Выход\n\n");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandWatchFight:
                        Fight(ChoiceUnit(numberUnit1), ChoiceUnit(numberUnit2));
                        break;

                    case CommandExit:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Ошибка ввода команды.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void GenerateUnits()
        {
            _units.Add(new Warrior("Воин", 1000, 15, 5));
            _units.Add(new Mage("Маг", 1000, 15, 5));
            _units.Add(new Berserker("Берсерк", 1000, 15, 5));
            _units.Add(new Gladiator("Гладиатор", 1000, 15, 5));
            _units.Add(new Rogue("Разбойник", 1000, 15, 5));
        }

        private void Fight(Unit unit1, Unit unit2)
        {
            Console.Clear();
            Console.WriteLine($"Бой между: {unit1.Name} и {unit2.Name}");

            int numberRound = 0;

            while (unit1.Heals > 0 && unit2.Heals > 0)
            {
                Console.WriteLine($"\n** {new string('_', 25)}Раунд - {++numberRound}{new string('_', 25)}");
                Console.WriteLine($"ХП {unit1.Name} [{unit1.Heals}] \nХП {unit2.Name} [{unit2.Heals}]");

                unit1.Attack(unit2);
                unit2.Attack(unit1);
            }

            if (unit1.Heals < 1 && unit2.Heals < 1)
            {
                Console.WriteLine("\nНичья!");
            }
            else if (unit2.Heals < 1)
            {
                Console.WriteLine($"\nПобедил - {unit1.Name}");
            }
            else
            {
                Console.WriteLine($"\nПобедил - {unit2.Name}");
            }

            Console.ReadLine();
            Console.Clear();
        }

        private void ShoyUnits()
        {
            for (int i = 0; i < _units.Count; i++)
            {
                Console.Write($"{i + 1}) - ");
                _units[i].ShoyInfo();
            }
        }

        private Unit ChoiceUnit(int numberUnit)
        {
            Unit unit = null;
            bool isWork = true;

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine($"Выберите {numberUnit}-ого война: ");
                ShoyUnits();

                while (isWork)
                {
                    int numberSelectUnit = Utilite.GetNumberInRange();

                    if (numberSelectUnit > 0 && numberSelectUnit <= _units.Count)
                    {
                        unit = _units[numberSelectUnit - 1].Clone();
                        isWork = false;
                    }
                    else
                    {
                        Console.WriteLine("Такого война нету");
                    }
                }
            }

            return unit;
        }
    }

    abstract class Unit : IDamageable
    {
        public int Heals { get; protected set; }
        public string Name { get; protected set; }
        public int Damage { get; protected set; }
        public int Defens { get; protected set; }

        public Unit(string name, int heals, int damege, int defens)
        {
            Name = name;
            Heals = heals;
            Damage = damege;
            Defens = defens;

            ShoyInfo();
        }

        public virtual void ShoyInfo()
        {
            Console.WriteLine($"{Name} - Здоровье {Heals}; Урон {Damage}; Защита {Defens}");
        }

        public virtual void Attack(IDamageable unit)
        {
            Console.WriteLine($"{Name} Наносит удар");
            unit.TakeDamage(Damage);
        }

        public virtual void TakeDamage(int damage)
        {
            damage = Math.Max(damage - Defens, 0);

            Heals -= damage;

            Console.WriteLine($"{Name} - {damage} ХП");
        }

        public abstract Unit Clone();
    }

    class Warrior : Unit
    {
        private int _percentageDoubleDamage = 40;
        private int _increasedDamageRatio = 2;
        private int _maximymPercentageDoubleDamage = 100;

        public Warrior(string name, int heals, int damage, int defens) : base(name, heals, damage, defens) { }

        public override void ShoyInfo()
        {
            base.ShoyInfo();
            Console.WriteLine($"С шансом {_percentageDoubleDamage} % может при атаке нанести удвоенный урон.");
        }

        public override void Attack(IDamageable unite)
        {
            if (_percentageDoubleDamage > Utilite.GenerateRandomNumber(0, _maximymPercentageDoubleDamage + 1))
            {
                Console.WriteLine($"{Name} - Критический урон ");
                int increasedDamage = Damage * _increasedDamageRatio;
                unite.TakeDamage(increasedDamage);
            }
            else
            {
                Console.WriteLine($"{Name} - Удар мечём ");
                unite.TakeDamage(Damage);
            }
        }
        public override Unit Clone()
        {
            return new Warrior(Name, Heals, Damage, Defens);
        }
    }

    class Mage : Unit
    {
        private int _manaPoints = 100;
        private int _magicDamage = 90;
        private int _priceCastFireball = 25;

        public Mage(string name, int heals, int damage, int defens) : base(name, heals, damage, defens) { }

        public override void ShoyInfo()
        {
            base.ShoyInfo();
            Console.WriteLine($"Имеет {_manaPoints} маны и может кастовать огненный шар.");
        }

        public override void Attack(IDamageable unit)
        {
            if (_manaPoints >= _priceCastFireball)
            {
                Console.WriteLine($"{Name} -  Фаербол!");
                unit.TakeDamage(_magicDamage);
                _manaPoints -= _priceCastFireball;

                if (_manaPoints < _priceCastFireball)
                    Console.WriteLine($"{Name} - [{_manaPoints}] Мана закончилась");
            }
            else
            {
                unit.TakeDamage(Damage);

                Console.WriteLine($"{Name} - Удар посохом");
            }
        }

        public override Unit Clone()
        {
            return new Mage(Name, Heals, Damage, Defens);
        }
    }

    class Berserker : Unit
    {
        private int _amountRage = 0;
        private int _maximumAmountRage = 80;
        private int _hitPointsRecovery = 35;

        public Berserker(string name, int heals, int damage, int defens) : base(name, heals, damage, defens) { }

        public override void ShoyInfo()
        {
            base.ShoyInfo();
            Console.WriteLine($"При получении урона накапливает ярость максимум {_maximumAmountRage} и может за счет нее применять исцеление на {_hitPointsRecovery} ХР.");
        }

        public override void TakeDamage(int damage)
        {
            damage = Math.Max(damage - Defens, 0);

            Heals -= damage;
            _amountRage += damage;

            if (_amountRage >= _maximumAmountRage)
            {
                Heals += _hitPointsRecovery;
                Console.WriteLine($"{Name} - Ярость +{_hitPointsRecovery}ХП");
                _amountRage = 0;
            }

            Console.WriteLine($"{Name} - {damage} ХП");
        }

        public override Unit Clone()
        {
            return new Berserker(Name, Heals, Damage, Defens);
        }
    }

    class Gladiator : Unit
    {
        private int _hitsCount = 0;
        private int _maxHitsCount = 2;

        public Gladiator(string name, int heals, int damage, int defens) : base(name, heals, damage, defens) { }

        public override void ShoyInfo()
        {
            base.ShoyInfo();
            Console.WriteLine($"Может повторно атаковать просле проведения {_maxHitsCount} атак");
        }
        public override void Attack(IDamageable warrior)
        {
            _hitsCount++;

            if (_hitsCount > _maxHitsCount)
            {
                _hitsCount = 0;

                for (int i = 0; i < _maxHitsCount; i++)
                {
                    Console.WriteLine($"{Name} - Удар");

                    warrior.TakeDamage(Damage);
                }
            }
            else
            {
                Console.WriteLine($"{Name} - Удар");

                warrior.TakeDamage(Damage);
            }
        }

        public override Unit Clone()
        {
            return new Gladiator(Name, Heals, Damage, Defens);
        }
    }

    class Rogue : Unit
    {
        private int _percentChanceEvasion = 85;
        private int _maximumPercentChanceEvasion = 100;

        public Rogue(string name, int heals, int damage, int defens) : base(name, heals, damage, defens) { }
        public override void TakeDamage(int damage)
        {
            if (_percentChanceEvasion > Utilite.GenerateRandomNumber(0,_maximumPercentChanceEvasion + 1))
            {
                Console.WriteLine($"{Name} Уклонение!");
            }
            else
            {
                damage = Math.Max(damage - Defens, 0);

                Heals -= damage;

                Console.WriteLine($"{Name} - {damage} к ХП");
            }
        }

        public override void ShoyInfo()
        {
            base.ShoyInfo();
            Console.WriteLine($"Имеет {_percentChanceEvasion} % шанс уклониться от атаки.");
        }
        public override Unit Clone()
        {
            return new Rogue(Name, Heals, Damage, Defens);
        }
    }

    class Utilite
    {
        public static Random s_random = new Random();

        public static int GenerateRandomNumber(int lowerLimitRangeRandom, int upperLimitRangeRandom)
        {
            int numberRandom = s_random.Next(lowerLimitRangeRandom, upperLimitRangeRandom);
            return numberRandom;
        }

        public static int GetNumberInRange(int lowerLimitRangeNumbers = Int32.MinValue, int upperLimitRangeNumbers = Int32.MaxValue)
        {
            bool isEnterNumber = true;
            int enterNumber = 0;
            string userInput;

            while (isEnterNumber)
            {
                Console.WriteLine($"Введите число.");

                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out enterNumber) == false)
                    Console.WriteLine("Не корректный ввод.");
                else if (VerifyForAcceptableNumber(enterNumber, lowerLimitRangeNumbers, upperLimitRangeNumbers))
                    isEnterNumber = false;
            }

            return enterNumber;
        }

        private static bool VerifyForAcceptableNumber(int number, int lowerLimitRangeNumbers, int upperLimitRangeNumbers)
        {
            if (number < lowerLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за нижний предел допустимого значения.");
                return false;
            }
            else if (number > upperLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за верхний предел допустимого значения.");
                return false;
            }

            return true;
        }
    }
}