using EFModels.LogsDB;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace EFModels
{
    public class AccessMatrix
    {
        private long BPId;
        public Dictionary<string, Dictionary<string, bool>> Matrix { get; set; }

        public AccessMatrix()
        {
            Matrix = new Dictionary<string, Dictionary<string, bool>>();
        }

        public AccessMatrix(long BPId)
        {
            this.BPId = BPId;
            Matrix = new Dictionary<string, Dictionary<string, bool>>();
        }
    }
}
