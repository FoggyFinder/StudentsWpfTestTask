namespace Converters

open System.Collections.ObjectModel
open System.Windows.Controls
open System
open FsXaml

type ValidationErrorsToStringConverter() =
     inherit ConverterBase
        (fun value _ _ _ ->
             match value with
             | :? ReadOnlyObservableCollection<ValidationError> as collection ->
                collection
                |> Seq.map (fun v -> v.ErrorContent |> unbox) 
                |> String.concat Environment.NewLine
                |> box                     
             | _ -> null )

type AgeToStringConverter() =
     inherit ConverterBase
        (fun value _ _ _ ->
             match value with
             | :? int ->
                value 
                |> unbox
                |> AgeToStringConverter.ageToStr
                |> box                    
             | _ -> null )

     static member ageToStr age = 
           if age >= 11 && age <= 14 then 
               sprintf "%A лет" age
           else
               match age % 10 with
               |1 -> sprintf "%A год" age
               |m when m = 0 || m >= 5 
                  -> sprintf "%A лет" age
               |_ -> sprintf "%A года" age