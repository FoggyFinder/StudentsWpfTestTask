module StudentComponent 

open CoreType
open Navigation
open Gjallarhorn.Bindable
open Gjallarhorn
open Gjallarhorn.Validation

type StudentUpdate = 
    | FirstName of string
    | LastName of string
    | Age of int 
    | Gender of Gender

let studentBind _ source model = 
    let mstudent = model |> Signal.get |> Mutable.create

    [ Female; Male ]
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

    [ last; age; gender ]
    |> List.fold Observable.merge first
    |> Observable.subscribe upd
    |> source.AddDisposable
        
    [
        Bind.Explicit.createCommandChecked "SaveCommand" source.Valid source
        |> Observable.map(fun _ -> mstudent.Value)
    ]       

let studentComponent : IComponent<_,CollectionNav,_> = Component.fromExplicit studentBind 