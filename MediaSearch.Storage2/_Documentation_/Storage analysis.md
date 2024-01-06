# Storage analysis 

```mermaid

classDiagram

  class IStorage {
        <<interface>>
        + Exists() : bool
        + Create() : bool
        + Remove() : bool
        + Any() : bool
        + IsEmpty() : bool
        + Clear()
        
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
    class AStorageMemory {
          <<abstract>>
    }

    class AStorageSqLite {
          <<abstract>>
    }

    class AStorageMsSql {
          <<abstract>>
    }

    class AStorageMySql {
          <<abstract>>
    }

    class AStorageXml {
          <<abstract>>
    }

    class AStorageJson {
          <<abstract>>
    }
  }

  class `TStorage(Memory|SqLite|...)(Movies|Series|...)` {

  }



AStorageSqLite --|> IStorage : implements
AStorageMsSql --|> IStorage : implements
AStorageMySql --|> IStorage : implements
AStorageXml --|> IStorage : implements
AStorageJson --|> IStorage : implements
AStorageMemory --|> IStorage : implements


IStorageMediasMovies --|> IStorageMedias : inherits
IStorageMediasSeries --|> IStorageMedias : inherits

`TStorage(Memory|SqLite|...)(Movies|Series|...)` ..|> AStorageMemory : inherits
`TStorage(Memory|SqLite|...)(Movies|Series|...)` ..|> AStorageSqLite : inherits
`TStorage(Memory|SqLite|...)(Movies|Series|...)` ..|> AStorageMsSql : inherits
`TStorage(Memory|SqLite|...)(Movies|Series|...)` ..|> AStorageMySql : inherits
`TStorage(Memory|SqLite|...)(Movies|Series|...)` ..|> AStorageXml : inherits
`TStorage(Memory|SqLite|...)(Movies|Series|...)` ..|> AStorageJson : inherits
`TStorage(Memory|SqLite|...)(Movies|Series|...)` ..|> IStorageMediasMovies : implements
`TStorage(Memory|SqLite|...)(Movies|Series|...)` ..|> IStorageMediasSeries : implements
TStorageMemoryMedias ..|> AStorageMemory : inherits
TStorageMemoryMedias ..|> IStorageMedias : implements


