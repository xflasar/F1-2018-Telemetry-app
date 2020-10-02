using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP
{
    class CarStatus
    {
        Byte m_tractionControl;
        Byte m_antiLockBrakes;
        Byte m_fuelMix;

        float m_fuelInTank;
        float m_fuelCapacity;

        UInt16 m_maxRPM;
        UInt16 m_idleRPM;

        Byte m_maxGears;
        Byte m_drsAllowed;
        
        Byte m_tyresWear1;
        Byte m_tyresWear2;
        Byte m_tyresWear3;
        Byte m_tyresWear4;

        Byte m_tyreCompound;

        Byte m_tyresDamage1;
        Byte m_tyresDamage2;
        Byte m_tyresDamage3;
        Byte m_tyresDamage4;

        float m_ersStoredEnergy;
        Byte m_ersDeployMode;

        float m_ersHarvestedThisLapKinetic;
        float m_ersHarvestedThisLapHeat;
        float m_ersDeployedThisLap;

        #region Getters
        public Byte GetTractionControl(Byte value)
        {
            return m_tractionControl = value;
        }

        public Byte GetAntiLockBrakes(Byte value)
        {
            return m_antiLockBrakes;
        }

        public Byte GetFuelMix(Byte value)
        {
            return m_fuelMix;
        }

        public float GetFuelInTank(float value)
        {
            return m_fuelInTank;
        }

        public float GetFuelCapacity(float value)
        {
            return m_fuelCapacity;
        }
        public UInt16 GetMaxRPM(UInt16 value)
        {
            return m_maxRPM;
        }

        public UInt16 GetIdleRPM(UInt16 value)
        {
            return m_idleRPM;
        }

        public Byte GetMaxGears(Byte value)
        {
            return m_maxGears;
        }

        public Byte GetDRSAllowed(Byte value)
        {
            return m_drsAllowed;
        }

        public Byte GetTireWear1(Byte value)
        {
            return m_tyresWear1;
        }
        public Byte GetTireWear2(Byte value)
        {
            return m_tyresWear2;
        }
        public Byte GetTireWear3(Byte value)
        {
            return m_tyresWear3;
        }
        public Byte GetTireWear4(Byte value)
        {
            return m_tyresWear4;
        }

        public Byte GetTireCompound(Byte value)
        {
            return m_tyreCompound;
        }

        public Byte GetTireDamage1(Byte value)
        {
            return m_tyresDamage1;
        }
        public Byte GetTireDamage2(Byte value)
        {
            return m_tyresDamage2;
        }
        public Byte GetTireDamage3(Byte value)
        {
            return m_tyresDamage3;
        }
        public Byte GetTireDamage4(Byte value)
        {
            return m_tyresDamage4;
        }

        public float GetERSStoredEnergy(float value)
        {
            return m_ersStoredEnergy;
        }
        
        public Byte GetERSDeployMode(Byte value)
        {
            return m_ersDeployMode;
        }

        public float GetERSHarvestedEnergyThisLapKinetic(float value)
        {
            return m_ersHarvestedThisLapKinetic;
        }

        public float GetERSHarvestedEnergyThisLapHeat(float value)
        {
            return m_ersHarvestedThisLapHeat;
        }

        public float GetERSDeployedThisLap(float value)
        {
             return m_ersDeployedThisLap;
        }
        #endregion

        #region Setters
        public void SetTractionControl(Byte value)
        {
             m_tractionControl = value;
        }

        public void SetAntiLockBrakes(Byte value)
        {
             m_antiLockBrakes = value;
        }

        public void SetFuelMix(Byte value)
        {
             m_fuelMix = value;
        }

        public void SetFuelInTank(float value)
        {
             m_fuelInTank = value;
        }

        public void SetFuelCapacity(float value)
        {
             m_fuelCapacity = value;
        }
        public void SetMaxRPM(UInt16 value)
        {
             m_maxRPM = value;
        }

        public void SetIdleRPM(UInt16 value)
        {
             m_idleRPM = value;
        }

        public void SetMaxGears(Byte value)
        {
             m_maxGears = value;
        }

        public void SetDRSAllowed(Byte value)
        {
             m_drsAllowed = value;
        }

        public void SetTireWear1(Byte value)
        {
             m_tyresWear1 = value;
        }
        public void SetTireWear2(Byte value)
        {
             m_tyresWear2 = value;
        }
        public void SetTireWear3(Byte value)
        {
             m_tyresWear3 = value;
        }
        public void SetTireWear4(Byte value)
        {
             m_tyresWear4 = value;
        }

        public void SetTireCompound(Byte value)
        {
             m_tyreCompound = value;
        }

        public void SetTireDamage1(Byte value)
        {
             m_tyresDamage1 = value;
        }
        public void SetTireDamage2(Byte value)
        {
             m_tyresDamage2 = value;
        }
        public void SetTireDamage3(Byte value)
        {
             m_tyresDamage3 = value;
        }
        public void SetTireDamage4(Byte value)
        {
             m_tyresDamage4 = value;
        }

        public void SetERSStoredEnergy(float value)
        {
             m_ersStoredEnergy = value;
        }

        public void SetERSDeployMode(Byte value)
        {
             m_ersDeployMode = value;
        }

        public void SetERSHarvestedEnergyThisLapKinetic(float value)
        {
             m_ersHarvestedThisLapKinetic = value;
        }

        public void SetERSHarvestedEnergyThisLapHeat(float value)
        {
             m_ersHarvestedThisLapHeat = value;
        }

        public void SetERSDeployedThisLap(float value)
        {
             m_ersDeployedThisLap = value;
        }
        #endregion

        public void PrintOut()
        {
            Console.WriteLine("Traction control: " + m_tractionControl);
            Console.WriteLine("ABS: " + m_antiLockBrakes);
            Console.WriteLine("Fuel mix: " + m_fuelMix);
            Console.WriteLine("Fuel in Tank: " + m_fuelInTank);
            Console.WriteLine("Fuel capacity: " + m_fuelCapacity);
            Console.WriteLine("Max RPM: " + m_maxRPM);
            Console.WriteLine("Idle RPM: " + m_idleRPM);
            Console.WriteLine("Max Gears: " + m_maxGears);
            Console.WriteLine("DRS Allowed: " + m_drsAllowed);
            Console.WriteLine("Tyres Wear 1: " + m_tyresWear1);
            Console.WriteLine("Tyres Wear 2: " + m_tyresWear2);
            Console.WriteLine("Tyres Wear 3: " + m_tyresWear3);
            Console.WriteLine("Tyres Wear 4: " + m_tyresWear4);
            Console.WriteLine("Tyre Compound: " + m_tyreCompound);
            Console.WriteLine("Tyre 1 Damage: " + m_tyresDamage1);
            Console.WriteLine("Tyre 2 Damage: " + m_tyresDamage2);
            Console.WriteLine("Tyre 3 Damage: " + m_tyresDamage3);
            Console.WriteLine("Tyre 4 Damage: " + m_tyresDamage4);
            Console.WriteLine("ERS Stored Energy: " + m_ersStoredEnergy);
            Console.WriteLine("ERS Deploy Mode: " + m_ersDeployMode);
            Console.WriteLine("ERS Harvested by Kinetic: " + m_ersHarvestedThisLapKinetic);
            Console.WriteLine("ERS Harvested by Heat: " + m_ersHarvestedThisLapHeat);
            Console.WriteLine("ERS Deployed time: " + m_ersDeployedThisLap);

        }
    }
}
