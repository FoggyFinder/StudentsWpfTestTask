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
