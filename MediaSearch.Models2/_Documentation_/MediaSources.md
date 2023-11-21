```mermaid
---
title: "MediaSources"
---
classDiagram

  class IMediaSource {
    <<interface>>
    + DateAdded : DateOnly
    + CreationDate : DateOnly
    + CreationYear : int
    + Description : string
    + Languages : IListWithPrincipal~ELanguage~
  }

  class AMediaSource {
    <<abstract>>
    + DateAdded : DateOnly
    + CreationDate : DateOnly
    + CreationYear : int
    + Description : string
    + Languages : IListWithPrincipal~ELanguage~
  }
  
  class IMediaSourcePhysical {
    <<interface>>
    + StoragePlace : string
    + StorageType : EStorageType
  }

   class TMediaSourcePhysical {
    + TMediaSourcePhysical()
    + TMediaSourcePhysical(IMediaSourcePhysical)
    + Empty$
  }

  class IMediaSourceVirtual {
    <<interface>>
    + StorageRoot : string
    + StoragePath : string
    + FileName : string
    + FileExtension : string
    + FullFileName : string
    + Size : long
    + MediaStreams : IMediaStreams
  }

  class TMediaSourceVirtual {
    + TMediaSourceVirtual()
    + TMediaSourceVirtual(IMediaSourceVirtual)
    + Empty$
  }

  class IMediaSources {
    <<interface>>
    + GetDefaul() : IMediaSource?
    + Get(ELanguage) : IMediasource?
    + GetAll() : IEnumerable~IMediaSource~
    + Add(params IMediaSource[]) : bool
    + AddRange(IEnumerable~IMediaSource~) : bool
    + Remove(IMediaSource) : bool
    + Clear()
    + SetDefault(IMediaSource)
    + Any()
    + Any(Predicate~IMediaSource~)
    + IsEmpty()
  }

  class TMediaSources {
    + TMediaSources()
    + TMediaSources(params IMediaSource[])
    + TMediaSources(IMediaSources)
  }

AMediaSource ..|> IMediaSource : implements
TMediaSourceVirtual --|> AMediaSource : inherits
TMediaSourceVirtual ..|> IMediaSourceVirtual : implements
TMediaSourcePhysical --|> AMediaSource : inherits
TMediaSourcePhysical ..|> IMediaSourcePhysical : implements
IMediaSources "1" ..> "0..n" IMediaSource : contains
TMediaSources ..|> IMediaSources : implements

IMediaSourceVirtual --|> IMediaSource : composition
 
IMediaSourcePhysical --|> IMediaSource : composition
