using Google.Protobuf;

namespace ProtobufSerializer;

/// <summary>
/// Very simple no-code-gen protobuf serializer.
/// 
/// Has many limitations!
/// 1. Tags must be maximum 31 (because we only support single byte tags).
/// 2. Only int, long, and string currently supported.
/// 3. Only packed arrays (of numbers) supported.
/// 4. No sub types.
/// </summary>
public class Serializer
{
    public IDictionary<uint, IProtoType> MessageDefinition { get; }
    private readonly HashSet<uint> fieldSet;

    public Serializer(IDictionary<uint, IProtoType> messageDefinition)
    {
        MessageDefinition = messageDefinition;
        fieldSet = new HashSet<uint>(messageDefinition.Keys);
    }

    public byte[] Serialize(IDictionary<uint, object> value)
    {
        var valueFieldSet = new HashSet<uint>(value.Keys);
        if(fieldSet.Except(valueFieldSet).Any())
        {
            throw new ArgumentException("Input value key set differs from messageDefinition.");
        }

        var bytes = new byte[CalculateMessageSize(value)];
        var output = new CodedOutputStream(bytes);

        foreach(var (key, input) in value)
        {
            output.WriteRawTag(CreateTag(key));
            MessageDefinition[key].Write(output, input);
        }
        output.CheckNoSpaceLeft();
        return bytes;

        int CalculateMessageSize(IDictionary<uint, object> value)
            => value.Sum(x => 1 + MessageDefinition[x.Key].ComputeSize(x.Value));

        // tag byte format is AAAAA_BBB where A bits are the tag number and B bits are the wire type.
        byte CreateTag(uint key)
            => BitConverter.GetBytes((key << 3) + MessageDefinition[key].WireType)[0];
    }

    public IDictionary<uint, object> Deserialize(byte[] bytes)
    {
        using var input = new CodedInputStream(bytes);
        var value = new Dictionary<uint, object>();

        uint tag;
        while((tag = input.ReadTag()) != 0)
        {
            var field = tag >> 3;

            if(!MessageDefinition.ContainsKey(field))
            {
                throw new InvalidOperationException($"Unexpected field value: {field}");
            }
            value.Add(field, MessageDefinition[field].Read(input));
        }

        return value;
    }
}

public interface IProtoType 
{ 
    uint WireType { get; }
    int ComputeSize(object input);
    void Write(CodedOutputStream output, object input);
    object Read(CodedInputStream input);
}

public static class ProtoType
{
    public static IProtoType Int32 => new ProtoInt32();
    public static IProtoType Int64 => new ProtoInt64();
    public static IProtoType String => new ProtoString();
    public static IProtoType Repeated(IProtoType protoType) => new ProtoRepeated(protoType);
}

public struct ProtoInt32 : IProtoType 
{ 
    public uint WireType => 0;
    public int ComputeSize(object input) => CodedOutputStream.ComputeInt32Size((int)input);
    public void Write(CodedOutputStream output, object input) => output.WriteInt32((int)input);
    public object Read(CodedInputStream input) => input.ReadInt32();
}

public struct ProtoInt64 : IProtoType 
{ 
    public uint WireType => 0; 
    public int ComputeSize(object input) => CodedOutputStream.ComputeInt64Size((long)input);
    public void Write(CodedOutputStream output, object input) => output.WriteInt64((long)input);
    public object Read(CodedInputStream input) => input.ReadInt64();
}

public struct ProtoString : IProtoType 
{ 
    public uint WireType => 2; 
    public int ComputeSize(object input) => CodedOutputStream.ComputeStringSize((string)input);
    public void Write(CodedOutputStream output, object input) => output.WriteString((string)input);
    public object Read(CodedInputStream input) => input.ReadString();
}

public record ProtoRepeated(IProtoType ProtoType) : IProtoType
{
    public uint WireType => 2;

    public int ComputeSize(object input)
    {
        var array = (object[])input;
        var bodySize = ComputeBodySize(array);
        return CodedOutputStream.ComputeLengthSize(bodySize) + bodySize;
    }

    private int ComputeBodySize(object[] array) => array.Sum(x => ProtoType.ComputeSize(x));

    public void Write(CodedOutputStream output, object input)
    {
        var items = (object[])input;
        output.WriteLength(ComputeBodySize(items));
        foreach(var item in items)
        {
            ProtoType.Write(output, item);
        }
    }

    public object Read(CodedInputStream input)
    {
        var size = input.ReadLength();
        var endOfBody = input.Position + size;

        var items = new List<object>();
        while(input.Position < endOfBody)
        {
            items.Add(ProtoType.Read(input));
        }
        return items.ToArray();
    }
}
