using Google.Protobuf;
using static System.Console;

namespace ProtobufSerializer;

public static class Program
{
    public static void Main()
    {
        // Given Example.proto, create a new Serializer instance
        // passing in a dictionary listing the field tags and types.
        var serializer = new Serializer(new Dictionary<uint, IProtoType> 
        { 
            [1] = ProtoType.String,
            [3] = ProtoType.Int32,
            [5] = ProtoType.Int64,
            [7] = ProtoType.Repeated(ProtoType.Int32),
            [9] = ProtoType.Repeated(ProtoType.String),
            [101] = ProtoType.Embedded(new Dictionary<uint, IProtoType> 
            { 
                [1] = ProtoType.Int32,
                [3] = ProtoType.String
            })
        });

        // Demonstrate usage
        ShouldBeAbleToSerializeAndDeserializeMessage(serializer);

        // Just to show that it works as expected, use a protoc code generated
        // message serializer against our no-code-gen serializer
        ShouldBeAbleToDeserializeMessage(serializer);
        ShouldBeAbleToSerializeMessage(serializer);
    }

    public static void ShouldBeAbleToSerializeAndDeserializeMessage(Serializer serializer)
    {
        WriteLine("\nSerialize and Deserialize\n");

        // Create a new ad-hoc message, specifying tags and values.
        var example = new Dictionary<uint, object>
        {
            [1] = "Superman",
            [7] = new object[] { 1, int.MaxValue, 3 },
            [5] = long.MaxValue,
            [3] = 570,
            [9] = new object[] { "one", "two", "three" },
            [101] = new Dictionary<uint, object>
            {
                [1] = int.MaxValue,
                [3] = "Ford Focus"
            }
        };

        // use the no-code-gen serializer to serialize the message to protobuf.
        var protobufBytes = serializer.Serialize(example);

        WriteLine($"bytes.Lenth = {protobufBytes.Length}");
        WriteLine(BitConverter.ToString(protobufBytes));

        // use the no-code-gen serializer to deserialize the protobuf message.
        var result = serializer.Deserialize(protobufBytes);

        serializer.MessageDefinition.WriteValue(result);
    }

    public static void ShouldBeAbleToDeserializeMessage(Serializer serializer)
    {
        WriteLine("\nDeserialize\n");

        // Create a new instance of the protoc code generated type Example.
        var example = new Example
        {
            Name = "Superman",
            Age = 570,
            StarsInGalaxy = long.MaxValue,
            Car = new Car
            {
                Miles = int.MaxValue,
                Model = "Ford Focus"
            }
        };

        example.Scores.AddRange(new[] { 1, int.MaxValue, 3 });
        example.Children.AddRange(new[] { "one", "two", "three" });

        // Serialize example to protobuf using its code generated serializer.
        var protobufBytes = example.ToByteArray();

        WriteLine($"bytes.Lenth = {protobufBytes.Length}");
        WriteLine(BitConverter.ToString(protobufBytes));

        // use the no-code-gen serializer to deserialize the protobuf message.
        var result = serializer.Deserialize(protobufBytes);

        serializer.MessageDefinition.WriteValue(result);
    }

    public static void ShouldBeAbleToSerializeMessage(Serializer serializer)
    {
        WriteLine("\nSerialize\n");

        // Create a new ad-hoc message, specifying tags and values.
        var example = new Dictionary<uint, object>
        {
            [1] = "Superman",
            [3] = 570,
            [5] = long.MaxValue,
            [7] = new object[] { 1, int.MaxValue, 3 },
            [9] = new object[] { "one", "two", "three" },
            [101] = new Dictionary<uint, object>
            {
                [1] = int.MaxValue,
                [3] = "Ford Focus"
            }
        };

        // use the no-code-gen serializer to serialize the message to protobuf.
        var protobufBytes = serializer.Serialize(example);

        WriteLine($"bytes.Lenth = {protobufBytes.Length}");
        WriteLine(BitConverter.ToString(protobufBytes));

        // deserialize the protobuf message using the code-gen'd deserializer.
        var result = new Example();
        result.MergeFrom(protobufBytes);

        WriteLine($"result.Name = {result.Name}");
        WriteLine($"result.Age = {result.Age}");
        WriteLine($"result.StarsInGalaxy = {result.StarsInGalaxy}");
        WriteLine($"result.Scores = {string.Join(", ", result.Scores.ToArray() )}");
        WriteLine($"result.Children = {string.Join(", ", result.Children.ToArray() )}");
        WriteLine($"result.Car.Miles = {result.Car.Miles}");
        WriteLine($"result.Car.Model = {result.Car.Model}");
    }

    public static void WriteValue(this IDictionary<uint, IProtoType> messageDefinition, IDictionary<uint, object> value, string indent = "")
    {
        foreach(var (key, item) in value)
        {
            if(!messageDefinition.ContainsKey(key))
            {
                throw new InvalidOperationException($"Invalid key: {key}");
            }

            if(messageDefinition[key] is ProtoRepeated)
            {
                var array = (object[])item;
                Write($"{indent}[{key}] = ( ");
                foreach(var element in array)
                {
                    Write($"{element} ");
                }
                WriteLine(")");
            }
            else if(messageDefinition[key] is ProtoEmbedded protoEmbedded)
            {
                WriteLine($"{indent}[{key}] = {{");
                WriteValue(protoEmbedded.MessageDefinition, (IDictionary<uint, object>)item, indent + "    ");
                WriteLine($"{indent}}}");
            }
            else
            {
                WriteLine($"{indent}[{key}] = {item}");
            }
        }
    }
}
