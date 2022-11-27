using JGC.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JGC.Common.Services
{
    public class ITRService : IITRService
    {
        public async Task<bool> IsImplementedITR(string SheetName)
        {
            if (SheetName == "7000-030A" || SheetName == "7000-031A" || SheetName == "7000-040A" || SheetName == "7000-041A" || SheetName == "7000-042A"
             || SheetName == "7000-080A" || SheetName == "7000-090A" || SheetName == "7000-091A" || SheetName == "8000-003A" || SheetName == "8100-001A" || SheetName == "8100-002A"
             || SheetName == "8121-002A" || SheetName == "8121-004A" || SheetName == "8260-002A" || SheetName == "8161-001A" || SheetName == "8140-001A" || SheetName == "8161-002A"
            
             || SheetName == "8000-101A-Standard" || SheetName == "8000-004A" || SheetName == "8211-002A" || SheetName == "7000-101A" || SheetName == "8000-101A" || SheetName == "8140-002A-Standard" || SheetName == "8140-002A"
             || SheetName == "8121-002A-Standard" || SheetName == "8121-004A-Standard" || SheetName == "8140-001A-Standard" || SheetName == "8161-001A-Standard" || SheetName == "8140-004A" || SheetName == "8140-004A-Standard" || SheetName == "8170-002A"
             || SheetName == "8211-002A-Standard" || SheetName =="8300-003A" || SheetName == "8170-007A")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_3XA(string SheetName)
        {
            if (SheetName == "7000-030A" || SheetName == "7000-031A")
                return true;
            else
                return false;
        }

        public async Task<bool> ITR_8140_001A(string SheetName)
        {
            if (SheetName == "8140-001A" || SheetName == "8140-001A-Standard")
                return true;
            else
                return false;
        }

        public async Task<bool> ITR_4XA(string SheetName)
        {
            if (SheetName == "7000-040A" || SheetName == "7000-041A" || SheetName == "7000-042A")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_80_9XA(string SheetName)
        {
            if (SheetName == "7000-080A" || SheetName == "7000-090A" || SheetName == "7000-091A")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_8XA(string SheetName)
        {
            if (SheetName == "8000-003A" || SheetName == "8000-004A")
                return true;
            else
                return false;
        }

        public async Task<bool> ITR_8100_001A(string SheetName)
        {
            if (SheetName == "8100-001A")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_81_2XA(string SheetName)
        {
            if (SheetName == "8100-002A")
                return true;
            else
                return false;
        }

        public async Task<bool> ITR_8121_002A(string SheetName)
        {
            if (SheetName == "8121-002A" || SheetName == "8121-002A-Standard")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_8260_002A(string SheetName)
        {
            if (SheetName == "8260-002A")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_8161_1XA(string SheetName)
        {
            if (SheetName == "8161-001A" || SheetName == "8161-001A-Standard")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_8121_004XA(string SheetName)
        {
            if (SheetName == "8121-004A" || SheetName == "8121-004A-Standard")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_8161_2XA(string SheetName)
        {
            if (SheetName == "8161-002A")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_8000_101A(string SheetName)
        {
            if (SheetName == "8000-101A-Standard")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_8211_002A(string SheetName)
        {
            if (SheetName == "8211-002A" || SheetName == "8211-002A-Standard")
                return true;
            else
                return false;
        }        
        public async Task<bool> ITR_7000_101AHarmony(string SheetName)
        {
            if (SheetName == "7000-101A" || SheetName == "8000-101A")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_8140_002A(string SheetName)
        {
            if (SheetName == "8140-002A-Standard" || SheetName == "8140-002A")
                return true;
            else
                return false;
        }

        public async Task<bool> ITR_8140_004A(string SheetName)
        {
            if (SheetName == "8140-004A-Standard" || SheetName == "8140-004A")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_8170_002A(string SheetName)
        {
            if (SheetName == "8170-002A")
                return true;
            else
                return false;
        }

        public async Task<bool> ITR_8300_003A(string SheetName)
        {
            if (SheetName == "8300-003A")
                return true;
            else
                return false;
        }
        public async Task<bool> ITR_8170_007A(string SheetName)
        {
            if (SheetName == "8170-007A")
                return true;
            else
                return false;
        }
        
    }
}
