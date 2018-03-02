module CoreType

type Gender = |Male |Female
type Student = {ID:int; FirstName : string; LastName : string; Age : int; Gender : Gender; }

//создание нового элемента 
let create id firstname lastname age gender = {
        ID = id
        FirstName = firstname
        LastName = lastname
        Age = age
        Gender = gender
    }

//добавление в список;
let add xs student = student :: xs
//удаление из списка
let remove students = List.filter (fun student -> Seq.contains student students |> not)    
//редактирование любой записи в списке;
let editFirstName firstname student = { student with FirstName = firstname }
let editLastName lastname student = { student with LastName = lastname }
let editAge age student = { student with Age = age}
let editGender gender student  = { student with Gender = gender }
let editId student id = {student with ID = id}
let edit student = List.map (fun st -> if st.ID = student.ID then student else st)