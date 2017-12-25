namespace Components

open CoreType
open Navigation
open Gjallarhorn.Bindable
open Gjallarhorn
open Gjallarhorn.Validation
open Gjallarhorn.Bindable.Framework

module StudentComponent =
    type StudentUpdate = 
        |FirstName of string
        |LastName of string
        |Age of int 
        |Gender of Gender

    let studentBind (nav:Dispatch<CollectionNav>) source model = 
        let mstudent = model |> Signal.get |> Mutable.create

        [Female; Male]
        |> Signal.constant
        |> Bind.Explicit.oneWay source "Genders" 

        let first = 
            mstudent
            |> Signal.map (fun student -> student.FirstName)
            |> Bind.Explicit.twoWayValidated source "FirstName" 
                (Validators.notNullOrWhitespace >> Validators.noSpaces)
            |> Observable.toMessage FirstName

        let last = 
            mstudent 
            |> Signal.map (fun student -> student.LastName)
            |> Bind.Explicit.twoWayValidated source "LastName" 
                (Validators.notNullOrWhitespace >> Validators.noSpaces)
            |> Observable.toMessage LastName  
        
        let age = 
            mstudent 
            |> Signal.map (fun student -> student.Age)
            |> Bind.Explicit.twoWayValidated source "Age" 
                (Validators.isBetween 16 100)
            |> Observable.toMessage Age

        let gender = 
            mstudent 
            |> Signal.map (fun student -> student.Gender)
            |> Bind.Explicit.twoWayValidated source "Gender" Validators.noValidation
            |> Observable.toMessage Gender   

        let upd msg =  
            match msg with
            | FirstName name -> Mutable.step (editFirstName name) mstudent
            | LastName name -> Mutable.step (editLastName name) mstudent
            | Age age -> Mutable.step (editAge age) mstudent
            | Gender gender -> Mutable.step (editGender gender) mstudent

        [last; age; gender]
        |> List.fold Observable.merge first
        |> Observable.subscribe upd
        |> source.AddDisposable
        
        [
            Bind.Explicit.createCommandChecked "SaveCommand" source.Valid source
            |> Observable.map(fun _ -> mstudent.Value)
        ]       

    let studentComponent = Component.fromExplicit studentBind 

module AppComponent = 
    type AppMessages = 
        |Add of Student
        |Edit of Student
        |Remove of Student seq
        |Save 

    let appBind (nav:Dispatch<CollectionNav>) source (model : ISignal<Student list>) =
        let defaultStudent =  
            {ID = -1 ; FirstName = ""; LastName =""; Age=0; Gender = Male}
        model |> Bind.Explicit.oneWay source "Students" 
    
        Bind.Explicit.createMessageParam "Edit" EditStudent source
        |> Observable.subscribe nav
        |> source.AddDisposable

        Bind.Explicit.createCommand "Add" source
        |> Observable.subscribe(fun _ -> defaultStudent |> AddStudent |> nav)
        |> source.AddDisposable

        [
            Bind.Explicit.createCommandParam "Remove" source
            |> Observable.map (Seq.singleton >> Remove)
            Bind.Explicit.createCommandParam "RemoveAll" source
            |> Observable.map (Seq.cast >> Remove)
        ]

    let appComponent = Component.fromExplicit appBind

    let app nav = 
        let path = "students.xml"
        let model : Student list = XmlReader.readFromFile path |> List.ofSeq
        let getId = 
            function
            |[] -> 0
            |x -> x |> List.map (fun x -> x.ID) |> List.max |> (+) 1
        let update message model =
            match message with
            |Add student -> 
                model
                |> getId
                |> editId student
                |> add model
            |Edit newValue ->
                model
                |> List.map
                    (fun student -> if student.ID = newValue.ID then newValue else student)
            |Remove students -> 
                model |> List.filter (fun student -> Seq.contains student students |> not)
            |Save ->
                XmlReader.writeToFile path model
                model
        let navigation = Dispatcher<CollectionNav>()

        Framework.application model update appComponent nav
        |> Framework.withNavigation navigation