using Google.Protobuf;

namespace ProtobufSerializer;

/// <summary>
/// Very simple no-code-gen protobuf serializer.
/// 
/// Has many limitations!
/// 1. Tags must be maximum 31 (because we only support single byte tags).
/// 2. Only int, long, string currently supported.
/// 3. Repeated fields of above types only.
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
            MessageDefinition[key].WriteWithTag(output, input, key);
        }
        output.CheckNoSpaceLeft();
        return bytes;

        int CalculateMessageSize(IDictionary<uint, object> value)
            => value.Sum(x => MessageDefinition[x.Key].ComputeSizeWithTag(x.Value));
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
            value.AddOrAppend(field, MessageDefinition[field].Read(input));
        }

        return value;
    }
}

public static class ValueExtensions
{
    public static void AddOrAppend(this IDictionary<uint, object> value, uint field, object item)
    {
        if(value.ContainsKey(field))
        {
            // this must be a repeated field
            if(value[field] is object[] objectArray && item is object[] items)
            {
                var newArray = new object[objectArray.Length + items.Length];
                objectArray.CopyTo(newArray, 0);
                items.CopyTo(newArray, objectArray.Length);
                value[field] = newArray;
            }
            else
            {
                throw new InvalidOperationException("Expected a repeated field here.");
            }
        }
        else
        {
            value.Add(field, item);
        }
    }
}

public interface IProtoType 
{ 
    uint WireType { get; }
    int ComputeSize(object input);
    int ComputeSizeWithTag(object input) => 1 + ComputeSize(input);
    void Write(CodedOutputStream output, object input);

    // tag byte format is AAAAA_BBB where A bits are the tag number and B bits are the wire type.
    void WriteWithTag(CodedOutputStream output, object input, uint key)
    {
        output.WriteRawTag(BitConverter.GetBytes((key << 3) + WireType)[0]);
        Write(output, input);
    }

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

    private readonly bool IsPackedRepeatedField = ProtoType.WireType == 0;

    public int ComputeSize(object input)
    {
        var array = (object[])input;
        var bodySize = ComputeBodySize(array);
        return CodedOutputStream.ComputeLengthSize(bodySize) + bodySize;
    }

    public int ComputeSizeWithTag(object input)
    {
        if(IsPackedRepeatedField)
        {
            return 1 + ComputeSize(input);
        }
        else
        {
            var array = (object[])input;
            return array.Sum(x => 1 + ProtoType.ComputeSize(x));
        }
    }

    private int ComputeBodySize(object[] array) => array.Sum(x => ProtoType.ComputeSize(x));

    public void Write(CodedOutputStream output, object input)
        => throw new InvalidOperationException("Cannot call Write() on a ProtoRepeated field.");

    public void WriteWithTag(CodedOutputStream output, object input, uint key)
    {
        var items = (object[])input;
        if (IsPackedRepeatedField)
        {
            output.WriteRawTag(BitConverter.GetBytes((key << 3) + WireType)[0]);
            output.WriteLength(ComputeBodySize(items));
            foreach(var item in items)
            {
                ProtoType.Write(output, item);
            }
        }
        else
        {
            foreach(var item in items)
            {
                ProtoType.WriteWithTag(output, item, key);
            }
        }
    }

    public object Read(CodedInputStream input)
    {
        var items = new List<object>();

        if(IsPackedRepeatedField)
        {
            var size = input.ReadLength();
            var endOfBody = input.Position + size;

            while(input.Position < endOfBody)
            {
                items.Add(ProtoType.Read(input));
            }
        }
        else
        {
            items.Add(ProtoType.Read(input));
        }

        return items.ToArray();
    }
}
