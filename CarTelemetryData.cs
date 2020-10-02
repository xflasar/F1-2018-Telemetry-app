using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP
{
    class CarTelemetryData
    {
        UInt16 m_speed;                      // Speed of car in kilometres per hour
        Byte m_throttle;                   // Amount of throttle applied (0 to 100)
        Byte m_steer;                      // Steering (-100 (full lock left) to 100 (full lock right))
        Byte m_brake;                      // Amount of brake applied (0 to 100)
        Byte m_clutch;                     // Amount of clutch applied (0 to 100)
        Byte m_gear;                       // Gear selected (1-8, N=0, R=-1)
        UInt16 m_engineRPM;                  // Engine RPM
        Byte m_drs;                        // 0 = off, 1 = on
        Byte m_revLightsPercent;   // Rev lights indicator (percentage)
        
        UInt16 m_brakesTemperature1;
        UInt16 m_brakesTemperature2;
        UInt16 m_brakesTemperature3;
        UInt16 m_brakesTemperature4;// Brakes temperature (celsius)

        UInt16 m_tyresSurfaceTemperature1; // Tyres surface temperature (celsius)
        UInt16 m_tyresSurfaceTemperature2; // Tyres surface temperature (celsius)
        UInt16 m_tyresSurfaceTemperature3; // Tyres surface temperature (celsius)
        UInt16 m_tyresSurfaceTemperature4; // Tyres surface temperature (celsius)

        UInt16 m_tyresInnerTemperature1;   // Tyres inner temperature (celsius)
        UInt16 m_tyresInnerTemperature2;
        UInt16 m_tyresInnerTemperature3;
        UInt16 m_tyresInnerTemperature4;
        UInt16 m_engineTemperature;          // Engine temperature (celsius)

        float m_tyresPressure1;           // Tyres pressure (PSI)
        float m_tyresPressure2;
        float m_tyresPressure3;
        float m_tyresPressure4;


        #region Gettery
        public UInt16 GetSpeed()
        {
            return m_speed;
        }
        public Byte GetThrottle()
        {
            return m_throttle;
        }
        public Byte GetSteer()
        {
            return m_steer;
        }
        public Byte GetBrake()
        {
            return m_brake;
        }
        public Byte GetClutch()
        {
            return m_clutch;
        }
        public Byte GetGear()
        {
            return m_gear;
        }
        public UInt16 GetEngineRPM()
        {
            return m_engineRPM;
        }
        public Byte GetDRS()
        {
            return m_drs;
        }
        public Byte GetRevLightsPerc()
        {
            return m_revLightsPercent;
        }
        public UInt16 GetBrakesTemperature1()
        {
            return m_brakesTemperature1;
        }
        public UInt16 GetBrakesTemperature2()
        {
            return m_brakesTemperature2;
        }
        public UInt16 GetBrakesTemperature3()
        {
            return m_brakesTemperature3;
        }
        public UInt16 GetBrakesTemperature4()
        {
            return m_brakesTemperature4;
        }
        public UInt16 GetTiresSurfaceTemperature1()
        {
            return m_tyresSurfaceTemperature1;
        }

        public UInt16 GetTiresSurfaceTemperature2()
        {
            return m_tyresSurfaceTemperature2;
        }
        public UInt16 GetTiresSurfaceTemperature3()
        {
            return m_tyresSurfaceTemperature3;
        }
        public UInt16 GetTiresSurfaceTemperature4()
        {
            return m_tyresSurfaceTemperature4;
        }
        public UInt16 GetTiresInnerTemperature1()
        {
            return m_tyresInnerTemperature1;
        }

        public UInt16 GetTiresInnerTemperature2()
        {
            return m_tyresInnerTemperature2;
        }
        public UInt16 GetTiresInnerTemperature3()
        {
            return m_tyresInnerTemperature3;
        }
        public UInt16 GetTiresInnerTemperature4()
        {
            return m_tyresInnerTemperature4;
        }
        public UInt16 GetEngineTemperature()
        {
            return m_engineTemperature;
        }

        public float GetTiresPressure1()
        {
            return m_tyresPressure1;
        }
        public float GetTiresPressure2()
        {
            return m_tyresPressure2;
        }
        public float GetTiresPressure3()
        {
            return m_tyresPressure3;
        }
        public float GetTiresPressure4()
        { 
            return m_tyresPressure4;
        }
        #endregion
        #region Settery
        public void SetSpeed(UInt16 value)
        {
            m_speed = value;
        }
        public void SetThrottle(Byte value)
        {
            m_throttle = value;
        }
        public void SetSteer(Byte value)
        {
            m_steer  = value;
        }
        public void SetBrake(Byte value)
        {
            m_brake = value;
        }
        public void SetClutch(Byte value)
        {
            m_clutch = value;
        }
        public void SetGear(Byte value)
        {
            m_gear = value;
        }
        public void SetEngineRPM(UInt16 value)
        {
            m_engineRPM = value;
        }
        public void SetDRS(Byte value)
        {
            m_drs = value;
        }
        public void SetRevLightsPerc(Byte value)
        {
            m_revLightsPercent = value;
        }
        public void SetBrakesTemperature1(UInt16 value)
        {
            m_brakesTemperature1 = value;
        }
        public void SetBrakesTemperature2(UInt16 value)
        {
            m_brakesTemperature2 = value;
        }
        public void SetBrakesTemperature3(UInt16 value)
        {
            m_brakesTemperature3 = value;
        }
        public void SetBrakesTemperature4(UInt16 value)
        {
            m_brakesTemperature4 = value;
        }
        public void SetTiresSurfaceTemperature1(UInt16 value)
        {
            m_tyresSurfaceTemperature1 = value;
        }

        public void SetTiresSurfaceTemperature2(UInt16 value)
        {
            m_tyresSurfaceTemperature2 = value;
        }
        public void SetTiresSurfaceTemperature3(UInt16 value)
        {
            m_tyresSurfaceTemperature3 = value;
        }
        public void SetTiresSurfaceTemperature4(UInt16 value)
        {
            m_tyresSurfaceTemperature4 = value;
        }

        public void SetTiresInnerTemperature1(UInt16 value)
        {
            m_tyresInnerTemperature1 = value;
        }

        public void SetTiresInnerTemperature2(UInt16 value)
        {
            m_tyresInnerTemperature2 = value;
        }
        public void SetTiresInnerTemperature3(UInt16 value)
        {
            m_tyresInnerTemperature3 = value;
        }
        public void SetTiresInnerTemperature4(UInt16 value)
        {
            m_tyresInnerTemperature4 = value;
        }
        public void SetEngineTemperature(UInt16 value)
        {
            m_engineTemperature = value;
        }

        public void SetTiresPressure1(float value)
        {
            m_tyresPressure1 = value;
        }
        public void SetTiresPressure2(float value)
        {
            m_tyresPressure2 = value;
        }
        public void SetTiresPressure3(float value)
        {
            m_tyresPressure3 = value;
        }
        public void SetTiresPressure4(float value)
        {
            m_tyresPressure4 = value;
        }
        #endregion
    }
}
