﻿```mermaid
---
title: "MediaStreams"
---
classDiagram

class IMediaStream {
    <<interface>>
    + Name : string
    + Index : int
    + CodecType : ECodecType
    + CodecName : string
    + CodecLongName : string
  }

  class AMediaStream {
    <<abstract>>
  }

  class TMediaStreamAudio {

  }
  class TMediaStreamVideo {

  }
  class TMediaStreamData {

  }
  class TMediaStreamSubTitle {

  }
  class TMediaStreamGeneric {

  }

  class IMediaStreams {
    <<interface>>
    + Add(params IMediaStreams[]) : bool
    + AddRange(IEnumerable~IMediaStream~) : bool
    + Get(int) : IMediaStream?
    + GetAll() : IEnumerable~IMediaStream~
    + Remove(IMediaStream) : bool
    + Clear()
    + Any() : bool
    + Any(Predicate~IMediaStream~) : bool
    + IsEmpty()
  }

  class TMediaStreams {
    + MediaStreamsVideo : IEnumerable~TMediaStreamVideo~
    + MediaStreamsAudio : IEnumerable~TMediaStreamAudio~
    + MediaStreamsSubTitle : IEnumerable~TMediaStreamSubTitle~
    + MediaStreamsData : IEnumerable~TMediaStreamData~
    + MediaStreamsGeneric : IEnumerable~TMediaStreamGeneric~
  }

IMediaStreams ..> "0..n" IMediaStream : Dependency
TMediaStreams ..|> IMediaStreams : implements
TMediaStreams ..> "0..n" IMediaStream : contains
AMediaStream <|-- IMediaStream
TMediaStreamAudio <|-- AMediaStream : inheritance
TMediaStreamVideo <|-- AMediaStream : inheritance
TMediaStreamSubTitle <|-- AMediaStream : inheritance
TMediaStreamData <|-- AMediaStream : inheritance
TMediaStreamGeneric <|-- AMediaStream : inheritance