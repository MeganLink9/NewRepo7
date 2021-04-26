using System;
using System.Reflection;
using System.Linq;

namespace Lab5
{
    //Напишіть класи тут
    public enum Gender { Male, Female };

    public class Cat
    {
        public string Name { get; }

        public Lab5.Gender Gender { get; }

        public static readonly double MaxEnergy = 20;
        public static readonly double MinEnergy = 0;
        public static readonly double SleepEnergyGain = 10;
        public static readonly double JumpEnergyDrain = 0.5;

        private double _energy;
        public double Energy
        {
            get { return _energy; }
            private set
            {
                if (value > MaxEnergy) _energy = MaxEnergy;
                else
                {
                    if (value < MinEnergy)
                    {
                        throw new Exception("Not enough energy to jump");
                    }
                    else _energy = value;
                }
            }
        }

        public Cat(string name, Lab5.Gender gender)
        {
            Name = name;
            Gender = gender;
            Energy = MaxEnergy;
        }

        public void Jump()
        {
            Energy -= JumpEnergyDrain;
        }

        public void Sleep()
        {
            Energy += SleepEnergyGain;
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var faradayTheCat = new Cat("Faraday", Gender.Male);
            var adaTheCat = new Cat("Ada", Gender.Female);

            PrintCatInfo(faradayTheCat);
            PrintCatInfo(adaTheCat);

            faradayTheCat.Jump();
            PrintCatInfo(faradayTheCat);
            for (var i = 0; i < 40; i++)
            {
                try
                {
                    faradayTheCat.Jump();
                    PrintCatInfo(faradayTheCat);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(i);
                    break;
                }
            }
            PrintCatInfo(adaTheCat);
            PrintCatInfo(faradayTheCat);
            for (var i = 0; i < 4; i++)
            {
                faradayTheCat.Sleep();
                PrintCatInfo(faradayTheCat);
            }
            PrintCatInfo(adaTheCat);
            adaTheCat.Sleep();
            PrintCatInfo(adaTheCat);
            PrintCatClassInfo();
        }

        private static void PrintCatInfo(Cat cat)
        {
            Console.WriteLine($"{cat.Name}, {cat.Gender}, {cat.Energy}");
            Console.WriteLine($"{typeof(Cat).GetField("_energy", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(cat)}");
        }

        private static void PrintCatClassInfo()
        {
            var catType = typeof(Cat);
            var props = catType.GetProperties();
            var fields = catType.GetFields().Concat(catType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)).ToArray();
            var constructors = catType.GetConstructors();
            var methods = catType.GetMethods();

            var sortLambda =
                new Func<MemberInfo, MemberInfo, int>(
                    (first, second) =>
                            String.Compare(first.Name, second.Name, StringComparison.Ordinal));
            Array.Sort(props, sortLambda.Invoke);
            Array.Sort(fields, sortLambda.Invoke);
            Array.Sort(constructors, sortLambda.Invoke);
            Array.Sort(methods, sortLambda.Invoke);
            foreach (var propertyInfo in props)
            {
                Console.WriteLine(
                    $"{propertyInfo.Name}, {propertyInfo.PropertyType}, {propertyInfo.CanRead}, {propertyInfo.CanWrite}");
            }
            foreach (var fieldInfo in fields)
            {
                Console.WriteLine(
                    $"{fieldInfo.Name}, {fieldInfo.FieldType}, {fieldInfo.IsPrivate}, {fieldInfo.IsPublic}, {fieldInfo.IsStatic}, {fieldInfo.IsInitOnly}, {fieldInfo.IsLiteral}");
            }
            foreach (var constructorInfo in constructors)
            {
                Console.WriteLine(
                    $"{constructorInfo.Name}, {constructorInfo.DeclaringType}, {constructorInfo.IsPublic}, {string.Join(", ", constructorInfo.GetParameters().ToArray<object>())}");
            }
            foreach (var methodInfo in methods)
            {
                Console.WriteLine(
                    $"{methodInfo.Name}, {methodInfo.DeclaringType}, {methodInfo.IsPublic}, {string.Join(", ", methodInfo.GetParameters().ToArray<object>())}");
            }
        }
    }
}
