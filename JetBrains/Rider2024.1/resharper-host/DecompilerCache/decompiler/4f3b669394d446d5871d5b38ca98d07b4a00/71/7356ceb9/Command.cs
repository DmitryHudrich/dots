// Decompiled with JetBrains decompiler
// Type: Pitstop.Infrastructure.Messaging.Command
// Assembly: Pitstop.Infrastructure.Messaging, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4F3B6693-94D4-46D5-871D-5B38CA98D07B
// Assembly location: /home/w1thluv/.nuget/packages/pitstop.infrastructure.messaging/5.1.0/lib/net8.0/Pitstop.Infrastructure.Messaging.dll

using System;

#nullable disable
namespace Pitstop.Infrastructure.Messaging
{
  public class Command : Message
  {
    public Command()
    {
    }

    public Command(Guid messageId)
      : base(messageId)
    {
    }

    public Command(string messageType)
      : base(messageType)
    {
    }

    public Command(Guid messageId, string messageType)
      : base(messageId, messageType)
    {
    }
  }
}
