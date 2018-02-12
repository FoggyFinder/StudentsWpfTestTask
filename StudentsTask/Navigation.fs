module Navigation

open CoreType

type CollectionNav =
    | ViewStudents
    | AddStudent
    | EditStudent of Student