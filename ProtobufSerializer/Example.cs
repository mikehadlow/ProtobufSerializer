// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Example.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from Example.proto</summary>
public static partial class ExampleReflection {

  #region Descriptor
  /// <summary>File descriptor for Example.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static ExampleReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "Cg1FeGFtcGxlLnByb3RvIl8KB0V4YW1wbGUSDAoEbmFtZRgBIAEoCRILCgNh",
          "Z2UYAyABKAUSFwoPc3RhcnNfaW5fZ2FsYXh5GAUgASgDEg4KBnNjb3JlcxgH",
          "IAMoBRIQCghjaGlsZHJlbhgJIAMoCWIGcHJvdG8z"));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::Example), global::Example.Parser, new[]{ "Name", "Age", "StarsInGalaxy", "Scores", "Children" }, null, null, null, null)
        }));
  }
  #endregion

}
#region Messages
public sealed partial class Example : pb::IMessage<Example>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    , pb::IBufferMessage
#endif
{
  private static readonly pb::MessageParser<Example> _parser = new pb::MessageParser<Example>(() => new Example());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<Example> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::ExampleReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Example() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Example(Example other) : this() {
    name_ = other.name_;
    age_ = other.age_;
    starsInGalaxy_ = other.starsInGalaxy_;
    scores_ = other.scores_.Clone();
    children_ = other.children_.Clone();
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Example Clone() {
    return new Example(this);
  }

  /// <summary>Field number for the "name" field.</summary>
  public const int NameFieldNumber = 1;
  private string name_ = "";
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string Name {
    get { return name_; }
    set {
      name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "age" field.</summary>
  public const int AgeFieldNumber = 3;
  private int age_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int Age {
    get { return age_; }
    set {
      age_ = value;
    }
  }

  /// <summary>Field number for the "stars_in_galaxy" field.</summary>
  public const int StarsInGalaxyFieldNumber = 5;
  private long starsInGalaxy_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public long StarsInGalaxy {
    get { return starsInGalaxy_; }
    set {
      starsInGalaxy_ = value;
    }
  }

  /// <summary>Field number for the "scores" field.</summary>
  public const int ScoresFieldNumber = 7;
  private static readonly pb::FieldCodec<int> _repeated_scores_codec
      = pb::FieldCodec.ForInt32(58);
  private readonly pbc::RepeatedField<int> scores_ = new pbc::RepeatedField<int>();
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<int> Scores {
    get { return scores_; }
  }

  /// <summary>Field number for the "children" field.</summary>
  public const int ChildrenFieldNumber = 9;
  private static readonly pb::FieldCodec<string> _repeated_children_codec
      = pb::FieldCodec.ForString(74);
  private readonly pbc::RepeatedField<string> children_ = new pbc::RepeatedField<string>();
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public pbc::RepeatedField<string> Children {
    get { return children_; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as Example);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(Example other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Name != other.Name) return false;
    if (Age != other.Age) return false;
    if (StarsInGalaxy != other.StarsInGalaxy) return false;
    if(!scores_.Equals(other.scores_)) return false;
    if(!children_.Equals(other.children_)) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    if (Name.Length != 0) hash ^= Name.GetHashCode();
    if (Age != 0) hash ^= Age.GetHashCode();
    if (StarsInGalaxy != 0L) hash ^= StarsInGalaxy.GetHashCode();
    hash ^= scores_.GetHashCode();
    hash ^= children_.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void WriteTo(pb::CodedOutputStream output) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    output.WriteRawMessage(this);
  #else
    if (Name.Length != 0) {
      output.WriteRawTag(10);
      output.WriteString(Name);
    }
    if (Age != 0) {
      output.WriteRawTag(24);
      output.WriteInt32(Age);
    }
    if (StarsInGalaxy != 0L) {
      output.WriteRawTag(40);
      output.WriteInt64(StarsInGalaxy);
    }
    scores_.WriteTo(output, _repeated_scores_codec);
    children_.WriteTo(output, _repeated_children_codec);
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
    if (Name.Length != 0) {
      output.WriteRawTag(10);
      output.WriteString(Name);
    }
    if (Age != 0) {
      output.WriteRawTag(24);
      output.WriteInt32(Age);
    }
    if (StarsInGalaxy != 0L) {
      output.WriteRawTag(40);
      output.WriteInt64(StarsInGalaxy);
    }
    scores_.WriteTo(ref output, _repeated_scores_codec);
    children_.WriteTo(ref output, _repeated_children_codec);
    if (_unknownFields != null) {
      _unknownFields.WriteTo(ref output);
    }
  }
  #endif

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    if (Name.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
    }
    if (Age != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Age);
    }
    if (StarsInGalaxy != 0L) {
      size += 1 + pb::CodedOutputStream.ComputeInt64Size(StarsInGalaxy);
    }
    size += scores_.CalculateSize(_repeated_scores_codec);
    size += children_.CalculateSize(_repeated_children_codec);
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(Example other) {
    if (other == null) {
      return;
    }
    if (other.Name.Length != 0) {
      Name = other.Name;
    }
    if (other.Age != 0) {
      Age = other.Age;
    }
    if (other.StarsInGalaxy != 0L) {
      StarsInGalaxy = other.StarsInGalaxy;
    }
    scores_.Add(other.scores_);
    children_.Add(other.children_);
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(pb::CodedInputStream input) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    input.ReadRawMessage(this);
  #else
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 10: {
          Name = input.ReadString();
          break;
        }
        case 24: {
          Age = input.ReadInt32();
          break;
        }
        case 40: {
          StarsInGalaxy = input.ReadInt64();
          break;
        }
        case 58:
        case 56: {
          scores_.AddEntriesFrom(input, _repeated_scores_codec);
          break;
        }
        case 74: {
          children_.AddEntriesFrom(input, _repeated_children_codec);
          break;
        }
      }
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
          break;
        case 10: {
          Name = input.ReadString();
          break;
        }
        case 24: {
          Age = input.ReadInt32();
          break;
        }
        case 40: {
          StarsInGalaxy = input.ReadInt64();
          break;
        }
        case 58:
        case 56: {
          scores_.AddEntriesFrom(ref input, _repeated_scores_codec);
          break;
        }
        case 74: {
          children_.AddEntriesFrom(ref input, _repeated_children_codec);
          break;
        }
      }
    }
  }
  #endif

}

#endregion


#endregion Designer generated code
