using fastipc.Message;
using ProtoBuf;

namespace Messages
{
	[ProtoContract]
	public class Ping : Message { }

	[ProtoContract]
	public class Pong : Message { }
}
