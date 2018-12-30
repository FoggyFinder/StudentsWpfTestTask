module AppComponent

open CoreType
open Navigation
open Gjallarhorn.Bindable

type AppMessages = 
    | Add of Student
    | Edit of Student
    | Remove of Student seq
    | Save 

let defaultStudent = { 
    ID = -1 
    FirstName = ""
    LastName = ""
    Age = 0
    Gender = Male
}

type AppViewModel =
    {
        Students : Student list
        Add : VmCmd<CollectionNav>
        Edit : VmCmd<CollectionNav>
        Remove : VmCmd<AppMessages>
        RemoveAll : VmCmd<AppMessages>
        Save : VmCmd<AppMessages>
    }

let appvd = { 
    Students = [] 
    Edit = Vm.cmd (CollectionNav.EditStudent defaultStudent) 
    Add = Vm.cmd CollectionNav.AddStudent
    Remove = Vm.cmd (AppMessages.Remove [defaultStudent]) 
    RemoveAll = Vm.cmd (AppMessages.Remove [defaultStudent]) 
    Save = Vm.cmd AppMessages.Save
}
    
let appComponent = 
    let hasStudents = List.isEmpty >> not   
    Component.create<Student list, CollectionNav, AppMessages> [
        <@ appvd.Students @> |> Bind.oneWay id
        <@ appvd.Edit @> |> Bind.cmdParam EditStudent |> Bind.toNav
        <@ appvd.Add @> |> Bind.cmd |> Bind.toNav
        <@ appvd.Save @> |> Bind.cmd
        <@ appvd.Remove @> |> Bind.cmdParamIf hasStudents (Seq.singleton >> Remove)
        <@ appvd.RemoveAll @> |> Bind.cmdParamIf hasStudents (Seq.cast >> Remove)
    ]  