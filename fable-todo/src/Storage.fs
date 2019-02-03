module App.Storage

open Fable.Core
open Fable.Import
open Elmish

open App.Model
open App.Update

//-------------------------------------

// Local storage interface
module S =
    let private STORAGE_KEY = "elmish-react-todomvc"
    let private decoder = Thoth.Json.Decode.Auto.generateDecoder<Model>()
    let load (): Model option =
        Browser.localStorage.getItem(STORAGE_KEY)
        |> unbox
        |> Core.Option.bind (Thoth.Json.Decode.fromString decoder >> function | Ok r -> Some r | _ -> None)

    let save (model: Model) =
        Browser.localStorage.setItem(STORAGE_KEY, Thoth.Json.Encode.Auto.toString(1, model))


let setStorage (model:Model) : Cmd<Msg> =
    Cmd.attemptFunc S.save model (string >> Failure)

let updateWithStorage (msg:Msg) (model:Model) =
  match msg with
  // If the Msg is Failure we know the model hasn't changed
  | Failure _ -> (model, [])
  | _         -> let (newModel, cmds) = update msg model
                 (newModel, Cmd.batch [ setStorage newModel; cmds ])