module CoreType

open System

type Gender = |Male |Female
type Student = {ID:int; FirstName : string; LastName : string; Age : int; Gender : Gender; }

//•создание нового элемента 
let create id firstname lastname age gender = {
        ID = id
        FirstName = firstname
        LastName = lastname
        Age = age
        Gender = gender
    }
//•отображение списка уже существующих элементов;
//let optionCreate student = 
//    if
//        String.IsNullOrWhiteSpace(student.FirstName)
//        || String.IsNullOrWhiteSpace(student.LastName)
//        || student.Age < 16 
//        || student.Age > 100
//    then None else Some student

//let resultCreate firstname lastname age sex = 
//    if String.IsNullOrWhiteSpace firstname then 
//        Error("Параметр имя обязателен к заполнению")
//    elif String.IsNullOrWhiteSpace firstname then 
//        Error("Параметр имя обязателен к заполнению")
//    elif age < 16 || age > 100 then 
//        Error("Параметр имя обязателен к заполнению")
//    else
//    {
//        ID = 0
//        FirstName = firstname
//        LastName = lastname
//        Age = age
//        Gender = sex
//    } |> Ok

//добавление в список;
let add xs student = student :: xs
//•редактирование любой записи в списке;
let editFirstName firstname student = { student with FirstName = firstname }
let editLastName lastname student = { student with LastName = lastname }
let editAge age student = { student with Age = age}
let editGender gender student  = { student with Gender = gender }
let editId student id = {student with ID = id}
