module XmlReader

open CoreType

open FSharp.Data

let [<Literal>] Sample = """
<Students>
  <Student Id="0">
    <FirstName>Robert</FirstName>
    <Last>Jarman</Last>
    <Age>21</Age>
    <Gender>0</Gender>
  </Student>
  <Student Id="2">
    <FirstName>Leona</FirstName>
    <Last>Menders</Last>
    <Age>20</Age>
    <Gender>1</Gender>
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



