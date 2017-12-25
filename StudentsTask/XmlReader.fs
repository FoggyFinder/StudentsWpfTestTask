module XmlReader

open CoreType

open FSharp.Data

let [<Literal>] Sample = """
<Students>
<Student Id="125">
    <FirstName>firstTest</FirstName><Last>lastname</Last><Age>19</Age><Gender>true</Gender>
</Student>
<Student Id="125">
    <FirstName>firstTest</FirstName><Last>lastname</Last><Age>19</Age><Gender>true</Gender>
</Student>
</Students>"""

type Students = XmlProvider<Sample>

let fromBool = function | true -> Female | false -> Male

let toCoreStudent (student:Students.Student) = 
    student.Gender
    |> fromBool
    |> create student.Id student.FirstName student.Last student.Age 

let readFromFile (path : string) = 
    Students.Load path
    |> fun x -> x.Students
    |> Seq.map toCoreStudent

let toBool = function | Male -> false |Female -> true

let fromCoreStudent (student:Student) = 
    Students.Student(student.ID, student.FirstName, student.LastName, student.Age, toBool student.Gender)

let toXmlStudents data =
    data
    |> Seq.map fromCoreStudent
    |> Seq.toArray
    |> Students.Students

let writeToFile (path : string) data =
    let students = data |> toXmlStudents
    students.XElement.Save path



