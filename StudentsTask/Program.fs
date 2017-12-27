open CoreType
open System
open Views
open Gjallarhorn.Bindable.Framework
open Gjallarhorn.Wpf
open Navigation
open Components.AppComponent
open Components.StudentComponent

[<STAThread>]
[<EntryPoint>]
let main _ = 
    let updateNavigation (_ : ApplicationCore<Student list,_,_>) request : UIFactory<Student list,_,_> =  
        match request with
        |ViewStudents ->
           Navigation.Page.fromComponent StudentsControl id appComponent id
        |AddStudent x ->
            Navigation.Page.dialog StudentDialog (fun _ -> x) studentComponent Add
        |EditStudent x ->
            Navigation.Page.dialog StudentDialog (fun _ -> x) studentComponent Edit
            
    let navigator = 
        Navigation.singlePage App MainWin ViewStudents updateNavigation

    let app = app navigator.Navigate

    Framework.RunApplication (navigator, app)
    1

