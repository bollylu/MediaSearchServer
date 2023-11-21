```mermaid
---
title: "Medias"
---
classDiagram

  class IMediaMovie {
    <<interface>>
  }

  class TMediaMovie {
    + TMediaMovie()
    + TMediaMovie(ILogger)
    + TMediaMovie(IMediaMovie)
    + TMediaMovie(string title)
  }

  class IMediaSerie {
    <<interface>>
    + SerieType : ESerieType
    + AddSeason(IMediaSerieSeason) : bool
    + RemoveSeason(IMediaSerieSeason) : bool
    + RemoveSeason(int) : bool
    + GetSeason(int) : IMediaSerieSeason?
    + GetSeasons() : IEnumerable~IMediaSerieSeason~
    + Clear()
    + Any()
    + Any(Predicate~IMediaSerieSeason~)
    + IsEmpty()
  }

  class TMediaSerie {
    + TMediaSerie()
    + TMediaSerie(ILogger)
    + TMediaSerie(IMediaSerie)
  }

  class IMediaSerieSeason {
    <<interface>>
    + SerieType : ESerieType
    + Number : int
    + AddEpisode(IMediaSerieEpisode) : bool
    + RemoveEpisode(IMediaSerieEpisode) : bool
    + RemoveEpisode(int) : bool
    + GetEpisode(int) : IMediaSerieEpisode?
    + GetEpisodes() : IEnumerable~IMediaSerieEpisode~
    + Clear()
    + Any()
    + Any(Predicate~IMediaSerieEpisode~)
    + IsEmpty()
  }

  class TMediaSerieSeason {
    + TMediaSerieSeason()
    + TMediaSerieSeason(ILogger)
    + TMediaSerieSeason(IMediaSerieSeason)
    + TMediaSerieSeason(ESerieType, int seasonNumber)
  }

  class IMediaSerieEpisode {
    <<interface>>
    + SerieType : ESerieType
    + Number : int
    + AbsoluteNumber : int
    + Season : int
  }

  class TMediaSerieEpisode {
    + TMediaSerieEpisode()
    + TMediaSerieEpisode(ILogger)
    + TMediaSerieEpisode(IMediaSerieEpisode)
  }
  class IMedia {
    <<interface>>
    + Name : string
    + MediaType : EMediaType
    + DateAdded : DateOnly
    + DefaultLanguage : ELanguage
    + IsInvalid : bool
  }

  class AMedia {
    <<abstract>>
    + Name : string
    + DEFAULT_LANGUAGE$ : ELanguage
    + Id : string
    + MediaType : EMediaType
    + DefaultLanguage : ELanguage
    + DateAdded : DateOnly
    + CreationDate : DateOnly
    + CreationYear : int
    + Groups : List~string~
    + IsGroupMember : bool
    + IsInvalid : bool
    # MediaInfos : IMediaInfos
    # MediaSources : IMediaSources
    # MediaPictures : IMediaPictures
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
  }

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

  
AMedia ..|> IMedia : implements
IMediaMovie ..|> IMedia : implements
IMediaSerie ..|> IMedia : implements
IMediaSerieSeason ..|> IMedia : implements
IMediaSerieEpisode ..|> IMedia : implements

AMedia "1" --> "0..n" IMediaSources : contains
AMedia "1" --> "1..n" IMediaInfos : contains
AMedia "1" --> "0..n" IMediaPictures : contains


TMediaSerie ..|> IMediaSerie : implements
TMediaSerieSeason ..|> IMediaSerieSeason : implements
TMediaSerieEpisode ..|> IMediaSerieEpisode : implements

TMediaMovie ..|> IMediaMovie : implements
TMediaMovie --|> AMedia : inherits
TMediaSerie --|> AMedia : inherits

TMediaSerieSeason --|> AMedia : inherits

TMediaSerieEpisode --|> AMedia : inherits