# Storage analysis 

```mermaid

classDiagram

  class IStorage:::red {
        <<interface>>
        
  }

  class IStorageMedias {
        <<interface>>
  }

  namespace KindOfContent {
    class IStorageMediasMovies {
          <<interface>>
    }

    class IStorageMediasSeries {
          <<interface>>
    }
  }

  namespace KindOfStorage {
    class IStorageMemory {
          <<interface>>
    }

    class IStorageSqLite {
          <<interface>>
    }

    class IStorageMsSql {
          <<interface>>
    }

    class IStorageMySql {
          <<interface>>
    }

    class IStorageXml {
          <<interface>>
    }

    class IStorageJson {
          <<interface>>
    }
  }

  class AStorage {

  }

  class ALoggable {

  }

  class `TStorageMemoryMedias(Movies|Series|...)` {

  }



IStorageSqLite <|-- IStorage : inherits
IStorageMsSql <|-- IStorage : inherits
IStorageMySql <|-- IStorage : inherits
IStorageXml <|-- IStorage : inherits
IStorageJson <|-- IStorage : inherits
IStorageMemory <|-- IStorage : inherits



IStorageMediasMovies <|-- IStorageMedias : inherits
IStorageMediasSeries <|-- IStorageMedias : inherits

AStorage <|-- IStorage : implements
AStorage <|-- ALoggable : inherits

`TStorageMemoryMedias(Movies|Series|...)` <|-- AStorage : inherits
`TStorageMemoryMedias(Movies|Series|...)` <|.. IStorageMemory : compose
`TStorageMemoryMedias(Movies|Series|...)` <|.. IStorageMediasMovies : compose



