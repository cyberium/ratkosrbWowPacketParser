using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;

namespace WowPacketParser.Parsing
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class ParserAttribute : Attribute
    {
        public ParserAttribute(Opcode opcode)
        {
            Opcode = opcode;
        }

        /// <summary>
        /// [addedInVersion, +inf[
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="addedInVersion"></param>
        public ParserAttribute(Opcode opcode, ClientVersionBuild addedInVersion)
        {
            if (ClientVersion.AddedInVersion(addedInVersion))
                Opcode = opcode;
        }

        public ParserAttribute(Opcode opcode, byte retailExpansion, byte retailMajor, byte retailMinor, byte classicExpansion, byte classicMajor, byte classicMinor, byte tbcExpansion, byte tbcMajor, byte tbcMinor)
        {
            if (ClientVersion.AddedInVersion(retailExpansion, retailMajor, retailMinor, classicExpansion, classicMajor, classicMinor, tbcExpansion, tbcMajor, tbcMinor))
                Opcode = opcode;
        }
        
        /// <summary>
        /// [addedInVersion, removedInVersion[
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="addedInVersion"></param>
        /// <param name="removedInVersion"></param>
        public ParserAttribute(Opcode opcode, ClientVersionBuild addedInVersion, ClientVersionBuild removedInVersion)
        {
            if (ClientVersion.InVersion(addedInVersion, removedInVersion))
                Opcode = opcode;
        }

        public Opcode Opcode { get; private set; }
    }
}
