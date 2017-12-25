module Navigation

open CoreType

type CollectionNav =
    | ViewStudents
    | AddStudent of Student
    | EditStudent of Student