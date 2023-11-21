# Models2 Medias classes

## Medias
```mermaid
classDiagram

class IMedia {
  <<interface>>
  + Id : string
  + Name : string
  + MediaType : EMediaType
  + DateAdded : DateOnly
  + DefaultLanguage : ELanguage
  + CreationDate : DateOnly
  + CreationYear : int
  + Groups : List~string~
  + IsGroupMember : bool
  + IsInvalid : bool
  # MediaInfos : IMediaInfos
  # MediaSources : IMediaSources
  # MediaPictures : IMediaPictures
}

class AMedia {
  <<abstract>>
  + Id : string
  + Name : string
  + DEFAULT_LANGUAGE$ : ELanguage
  + MediaType : EMediaType
  + DefaultLanguage : ELanguage
  + DateAdded : DateOnly
  + Groups : List~string~
  + IsGroupMember : bool
  + IsInvalid : bool
  # MediaInfos : IMediaInfos
  # MediaSources : IMediaSources
  # MediaPictures : IMediaPictures
}

  class EMediaType {
    <<enum>>
    Unknown
    Movie
    Serie
    SerieSeason
    SerieEpisode
    Music
    Video
    Theatre
    Spectacle
    Concert
    eBook
    eBookAudio
    Podcast
  }

  class TMediaMovie {
  }

AMedia ..|> IMedia : implements

TMediaMovie --|> AMedia : inherits
TMediaSerie --|> AMedia : inherits
TMediaSerieSeason --|> AMedia : inherits
TMediaSerieSeasonEpisode --|> AMedia : inherits

 ```

 ## MediaSources
 ```mermaid
 classDiagram

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
}
  class IMediaSource {
    <<interface>>
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

  class ELanguage {
    Unknown
    French
    FrenchFrance
    FrenchQuebec
    FrenchBelgium
    English
    EnglishUs
    EnglishUk
    Chinese
    ChineseCantonese
    ChineseMandarin
    Japanese
    Korean
    German
    Dutch
  }
  class EStorageType {
    <<enum>>
    Unknown = 0,
    Book = 1,
    DVD = 2,
    CD = 3,
    BluRay = 4,
    Magazine = 5,
    Tape = 6,
    Vinyl = 7
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

IMediaSources ..> "0..n" IMediaSource : contains

 IMediaSourcePhysical --|> IMediaSource : inherits
 IMediaSourceVirtual --|> IMediaSource : inherits
 IMediaSourceVirtual ..> "1" IMediaStreams : contains
 IMediaStreams ..> "0..n" IMediaStream : contains
 ```

 ## MediaInfos
 ```mermaid
 classDiagram


class IMediaInfos {
  <<interface>>
  + GetDefault() : IMediaInfo?
  + Get(ELanguage) : IMediaInfo?
  + GetAll() : IEnumerable~IMediaInfo~
  + Add(params IMediaInfo[])
  + AddRange(IEnumerable~IMediaInfo~)
  + Remove(IMediaInfo)
  + Clear()
  + SetDefault(IMediaInfo)
}
  class IMediaInfo {
    <<interface>>
    + DateOnly : DateAdded
    + DateOnly : CreationDate
    + int : CreationYear
    + string : Description
    + IListWithPrincipal~ELanguage~ : Languages
    + List~string~ : Tags
  }

  class ELanguage {
    Unknown
    French
    FrenchFrance
    FrenchQuebec
    FrenchBelgium
    English
    EnglishUs
    EnglishUk
    Chinese
    ChineseCantonese
    ChineseMandarin
    Japanese
    Korean
    German
    Dutch
  }

IMediaInfos ..> "1..n" IMediaInfo : contains
 
 ```

 ## MediaPictures
 ```mermaid
 classDiagram

 class IMediaPictures {
  <<interface>>
  + GetDefault() : IMediaPicture?
  + Get(ELanguage) : IMediaPicture?
  + GetAll() : IEnumerable~IMediaPicture~
  + Add(params IMediaPicture[]) : bool
  + AddRange(IEnumerable~IMediaPicture~) : bool
  + Remove(IMediaPicture) : bool
  + Clear()
  + SetDefault(IMediaPïcture)
}
  class IMediaPicture {
    <<interface>>
  }

IMediaPictures ..> "0..n" IMediaPicture : contains
```