﻿```mermaid
---
title: "MediaInfos"
---
classDiagram

  class IMediaInfos {
    <<interface>>
    + GetDefault() : IMediaInfo?
    + Get(ELanguage) : IMediaInfo?
    + GetAll() : IEnumerable~IMediaInfo~
    + Add(params IMediaInfo[]) : bool
    + AddRange(IEnumerable~IMediaInfo~) : bool
    + Remove(IMediaInfo) : bool
    + Clear()
    + SetDefault(IMediaInfo) : bool
    + Any() : bool
    + Any(Predicate~IMediaInfo~) : bool
    + IsEmpty() : bool
  }

  class TMediaInfos {
    + TMediaInfos()
    + TMediaInfos(IMediaInfos)
    + TMediaInfos(params IMediaInfos[])
  }

  class IMediaInfo {
    <<interface>>
    + Language : ELanguage
    + Name : string
    + Description : string
    + DateAdded : DateOnly
    + Tags : List~string~
  }

  class IGroupMembership {
    + Groups : List~string~
    + IsGroupMember()
  }

  class ICreation {
    + CreationDate : DateOnly
    + CreationYear : int
  }

  class AMediaInfo {
    # AMediaInfo()
    # AMediaInfo(IMediaInfo)
  }

  class TMediaInfo {
    + TMediaInfo()
    + TMediaInfo(IMediaInfo)
    + Empty$
  }

  IMediaInfos "1" ..> "0..n" IMediaInfo : contains
  TMediaInfos ..|> IMediaInfos : implements
  AMediaInfo ..|> IMediaInfo : implements
  TMediaInfo --|> AMediaInfo : inherits
  IMediaInfo <|-- IGroupMembership : composes
  IMediaInfo <|-- ICreation : composes