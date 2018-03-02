open CoreType
open System
open Views
open Gjallarhorn.Bindable.Framework
open Gjallarhorn.Wpf
open Navigation
open AppComponent
open StudentComponent

let getId = 
    function
    |[] -> 0
    |x -> 
        x |> List.maxBy (fun x -> x.ID) |> fun x -> x.ID + 1

[<STAThread>]
[<EntryPoint>]
let main _ = 
    let path = "students.xml"
    let model : Student list = XmlWorker.readFromFile path |> List.ofSeq

    let updateNavigation (_ : ApplicationCore<Student list,_,_>) request : UIFactory<Student list,_,_> =  
        match request with
        |ViewStudents ->
           Navigation.Page.fromComponent StudentsControl id appComponent id
        |AddStudent ->
            Navigation.Page.dialog StudentDialog (fun _ -> defaultStudent) studentComponent Add
        |EditStudent x ->
            Navigation.Page.dialog StudentDialog (fun _ -> x) studentComponent Edit

    let update message model =
        match message with
        |Add student -> 
            model
            |> getId
            |> editId student
            |> add model
        |Edit newValue ->
            model
            |> edit newValue
        |Remove students -> 
            model
            |> remove students
        |Save ->
            XmlWorker.writeToFile path model
            model
            
    let nav = Navigation.singlePage App MainWin ViewStudents updateNavigation
    let app = Framework.application model update appComponent nav.Navigate

    Framework.RunApplication(nav, app)

    1