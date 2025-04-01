using System;
using MessagePack;

[MessagePackObject]
public class TKPacket
{
    [Key(0)]
    public int PacketID { get; set; } //@TK 패킷 구분자 
}

[MessagePackObject]
public class TKPacketChat : TKPacket
{
    [Key(1)]
    public string Message { get; set; }
    [Key(2)]
    public DateTime SendTime { get; set; }
    [Key(3)]
    public string UserID { get; set; } //@tk GUID
    [Key(4)]
    public string NickName { get; set; }
}



