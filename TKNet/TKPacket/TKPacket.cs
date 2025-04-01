using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKPacket
{
    [MessagePackObject]
    public class TKPacket
    {
        [Key(0)]
        public int PacketID { get; set; }
    }

    [MessagePackObject]
    public class TKPacketChat : TKPacket
    {
        [Key(1)]
        public string Message { get; set; }
        [Key(2)]
        public DateTime SendTime { get; set; }
        [Key(3)]
        public string UserID { get; set; } //GUID 
        [Key(4)]
        public string NickName { get; set; }
    }
}
