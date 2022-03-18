using Google.Protobuf;

namespace ProtobufSerializer;

/// <summary>
/// Very simple no-code-gen protobuf serializer.
/// 
/// Has many limitations!
/// 1. Tags must be maximum 31 (because we only support single byte tags).
/// 2. Only int, long, and string currently supported.
/// 3. No arrays or sub types supported.
/// </summary>
public class Serializer
{
    private readonly IDictionary<uint, Type> messageDefinition;
    private readonly HashSet<uint> fieldSet;

    public Serializer(IDictionary<uint, Type> messageDefinition)
    {
        this.messageDefinition = messageDefinition;
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
            switch (messageDefinition[key])
            {
                case Type t when t == typeof(int):
                    output.WriteInt32((int)(input ?? 0));
                    break;
                case Type t when t == typeof(long):
                    output.WriteInt64((long)(input ?? 0));
                    break;
                case Type t when t == typeof(string):
                    output.WriteString((string)(input ?? ""));
                    break;
                default:
                    throw new InvalidOperationException($"Invalid type.");
            }
        }
        output.CheckNoSpaceLeft();
        return bytes;

        int CalculateMessageSize(IDictionary<uint, object> value)
        {
            var size = 0;
            foreach(var (key, input) in value)
            {
                size += 1 + messageDefinition[key] switch
                {
                    Type t when t == typeof(int) =>  CodedOutputStream.ComputeInt32Size((int)(input ?? 0)),
                    Type t when t == typeof(long) =>  CodedOutputStream.ComputeInt64Size((long)(input ?? 0)),
                    Type t when t == typeof(string) =>  CodedOutputStream.ComputeStringSize((string)(input ?? "")),
                    Type t => throw new InvalidOperationException($"Unsupported type {t.Name}")
                };
            }
            return size;
        }

        // tag byte format is AAAAA_BBB where A bits are the tag number and B bits are the wire type.
        byte CreateTag(uint key)
        {
            uint wiretype = messageDefinition[key] switch 
            { 
                Type t when t == typeof(int) => 0,
                Type t when t == typeof(long) => 0,
                Type t when t == typeof(string) => 2, // delimited
                Type t => throw new InvalidOperationException($"Unsupported type {t.Name}")
            };

            return BitConverter.GetBytes((key << 3) + wiretype)[0];
        }
    }

    public IDictionary<uint, object> Deserialize(byte[] bytes)
    {
        using var input = new CodedInputStream(bytes);
        var value = new Dictionary<uint, object>();

        uint tag;
        while((tag = input.ReadTag()) != 0)
        {
            var field = tag >> 3;

            if(!messageDefinition.ContainsKey(field))
            {
                throw new InvalidOperationException($"Unexpected field value: {field}");
            }

            switch (messageDefinition[field])
            {
                case Type t when t == typeof(int):
                    value.Add(field, input.ReadInt32());
                    break;
                case Type t when t == typeof(long):
                    value.Add(field, input.ReadInt64());
                    break;
                case Type t when t == typeof(string):
                    value.Add(field, input.ReadString());
                    break;
                default:
                    throw new InvalidOperationException($"Invalid type.");
            }
        }

        return value;
    }
}
