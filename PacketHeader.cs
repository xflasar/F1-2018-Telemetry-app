using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP
{
    class PacketHeader
    {
        protected int m_packetFormat;         // 2018
        protected int m_packetVersion;        // Version of this packet type, all start from 1
        protected int m_packetId;             // Identifier for the packet type, see below
        protected ulong m_sessionUID;           // Unique identifier for the session
        protected float m_sessionTime;          // Session timestamp
        protected uint m_frameIdentifier;      // Identifier for the frame the data was retrieved on
        protected int m_playerCarIndex;       // Index of player's car in the array

        #region Getters
        // Getters
        public int GetPacketFormat()
        {
            return m_packetFormat;
        }

        public int GetPacketVersion()
        {
            return m_packetVersion;
        }
        public int GetPacketID()
        {
            return m_packetId;
        }
        public ulong GetSessionUID()
        {
            return m_sessionUID;
        }
        public float GetSessionTime()
        {
            return m_sessionTime;
        }
        public uint GetFrameIdentifier()
        {
            return m_frameIdentifier;
        }
        public int GetPlayerCarIndex()
        {
            return m_playerCarIndex;
        }
        #endregion
        #region Setters
        //Setters
        public void SetPacketFormat(int value)
        {
            m_packetFormat = value;
        }
        public void SetPacketVersion(int value)
        {
            m_packetVersion = value;
        }
        public void SetPacketID(int value)
        {
            m_packetId = value;
        }
        public void SetSessionUID(ulong value)
        {
            m_sessionUID = value;
        }
        public void SetSessionTime(float value)
        {
            m_sessionTime = value;
        }
        public void SetFrameIdentifier(int value)
        {
            m_frameIdentifier = (uint)value;
        }
        public void SetPlayerCarIndex(int value)
        {
            m_playerCarIndex = value;
        }
        #endregion
    }
}
