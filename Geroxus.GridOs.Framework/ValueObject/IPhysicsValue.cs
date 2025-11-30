using System;

namespace IngameScript
{
    public struct PhysicsValue<T>
    {
        public PhysicsValue(PhysicsUnit unit, T value)
        {
            Unit = unit;
            Value = value;
        }

        public PhysicsUnit Unit { get; }
        public T Value { get; }

        public override string ToString()
        {
            return $"{Value:N}{Unit.Short()}";
        }
    }

    public enum PhysicsUnit
    {
        Newton,
        Kilonewton,
        Kilogram,
        MeterPerSecond,
        MeterPerSecondSquared,
        NewtonPerKilogram
    }

    public static class PhysicsUniExtensions
    {
        public static String Short(this PhysicsUnit unit)
        {
            switch (unit)
            {
                case PhysicsUnit.Newton: return "N";
                case PhysicsUnit.Kilonewton: return "kN";
                case PhysicsUnit.Kilogram: return "kg";
                case PhysicsUnit.NewtonPerKilogram: return "N/kg";
                default: return unit.ToString();
            }
        }
    }
}