using Google.Protobuf;
using static System.Console;

namespace ProtobufSerializer;

public static class Program
{
    public static void Main()
    {
        // Given Example.proto, create a new Serializer instance
        // passing in a dictionary listing the field tags and types.
        var serializer = new Serializer(new Dictionary<uint, Type> 
        { 
            [1] = typeof(string),
            [3] = typeof(int),
            [5] = typeof(long),
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
        // Create a new ad-hoc message, specifying tags and values.
        var example = new Dictionary<uint, object>
        {
            [1] = "Superman",
            [3] = 570,
            [5] = long.MaxValue
        };

        // use the no-code-gen serializer to serialize the message to protobuf.
        var protobufBytes = serializer.Serialize(example);

        WriteLine($"bytes.Lenth = {protobufBytes.Length}");
        WriteLine(BitConverter.ToString(protobufBytes));

        // use the no-code-gen serializer to deserialize the protobuf message.
        var result = serializer.Deserialize(protobufBytes);

        WriteLine($"result.Count = {result.Count}");
        WriteLine($"result[1] = {result[1]}");
        WriteLine($"result[3] = {result[3]}");
        WriteLine($"result[5] = {result[5]}");
    }

    public static void ShouldBeAbleToDeserializeMessage(Serializer serializer)
    {
        // Create a new instance of the protoc code generated type Example.
        var example = new Example 
        { 
            Name = "Superman",
            Age = 570,
            StarsInGalaxy = long.MaxValue
        };

        // Serialize example to protobuf using its code generated serializer.
        var protobufBytes = example.ToByteArray();

        WriteLine($"bytes.Lenth = {protobufBytes.Length}");
        WriteLine(BitConverter.ToString(protobufBytes));

        // use the no-code-gen serializer to deserialize the protobuf message.
        var result = serializer.Deserialize(protobufBytes);

        WriteLine($"result.Count = {result.Count}");
        WriteLine($"result[1] = {result[1]}");
        WriteLine($"result[3] = {result[3]}");
        WriteLine($"result[5] = {result[5]}");
    }

    public static void ShouldBeAbleToSerializeMessage(Serializer serializer)
    {
        // Create a new ad-hoc message, specifying tags and values.
        var example = new Dictionary<uint, object>
        {
            [1] = "Superman",
            [3] = 570,
            [5] = long.MaxValue
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
    }
}
